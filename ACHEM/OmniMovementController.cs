using System;
using System.Collections.Generic;
using UnityEngine;

public class OmniMovementController : ClientMovementController
{
	private const float Rotation_Degrees_Per_Second = 120f;

	private static Dictionary<int, InputAction> MovementKeyMap;

	public event Action CharacterMoved;

	private static void SetMovementAction(MovementState moveState, InputAction inputAction)
	{
		MovementKeyMap[(int)moveState] = inputAction;
	}

	static OmniMovementController()
	{
		MovementKeyMap = new Dictionary<int, InputAction>();
		SetMovementAction(MovementState.Forward, InputAction.Forward);
		SetMovementAction(MovementState.Backward, InputAction.Backward);
		SetMovementAction(MovementState.LeftRotate, InputAction.LeftRotate);
		SetMovementAction(MovementState.RightRotate, InputAction.RightRotate);
		SetMovementAction(MovementState.LeftStrafe, InputAction.LeftStrafe);
		SetMovementAction(MovementState.RightStrafe, InputAction.RightStrafe);
	}

	public void Awake()
	{
		UIGame.JSDoubleClicked = (Action)Delegate.Combine(UIGame.JSDoubleClicked, new Action(OnJoystickDoubleClick));
	}

	public void Update()
	{
		MovementState num = base.State;
		base.State = MovementState.None;
		if (UIGame.ControlScheme == ControlScheme.HANDHELD)
		{
			JoystickUpdate();
		}
		KeyUpdate();
		TargetAutoRunUpdate();
		ApplyAutoRun();
		if ((num == MovementState.None && IsMoving()) || base.ShouldTurnWithCamera)
		{
			SnapToCameraOrientation();
		}
		if (base.State != 0)
		{
			this.CharacterMoved?.Invoke();
		}
	}

	public void OnDestroy()
	{
		UIGame.JSDoubleClicked = (Action)Delegate.Remove(UIGame.JSDoubleClicked, new Action(OnJoystickDoubleClick));
	}

	public void SnapToCameraOrientation()
	{
		float y = camController.transform.localEulerAngles.y;
		if (y > 0f)
		{
			camController.RotateHorizontal(0f - y);
			base.transform.Rotate(new Vector3(0f, y, 0f));
		}
	}

	private void OnJoystickDoubleClick()
	{
		if ((bool)SettingsManager.IsAutoRunOn)
		{
			ToggleAutoRun();
		}
	}

	private void JoystickUpdate()
	{
		Vector2 normalized = UIGame.JSDelta.normalized;
		if (normalized.y <= -0.5f)
		{
			base.State |= MovementState.Backward;
		}
		else if (normalized.y >= 0.5f)
		{
			base.State |= MovementState.Forward;
		}
		if (normalized.x >= 0.5f)
		{
			base.State |= MovementState.RightStrafe;
		}
		if (normalized.x <= -0.5f)
		{
			base.State |= MovementState.LeftStrafe;
		}
	}

	private void KeyUpdate()
	{
		if (InputManager.InputEnabled)
		{
			foreach (int key in MovementKeyMap.Keys)
			{
				if (InputManager.GetActionKey(MovementKeyMap[key]))
				{
					base.State |= (MovementState)(byte)key;
				}
			}
			if (InputManager.GetActionKeyDown(InputAction.Jump))
			{
				Jump();
			}
		}
		if (InputManager.GetActionKey(InputAction.MouseRight))
		{
			if ((base.State & MovementState.LeftRotate) == MovementState.LeftRotate)
			{
				base.State &= ~MovementState.LeftRotate;
				base.State |= MovementState.LeftStrafe;
			}
			if ((base.State & MovementState.RightRotate) == MovementState.RightRotate)
			{
				base.State &= ~MovementState.RightRotate;
				base.State |= MovementState.RightStrafe;
			}
			if (Input.GetMouseButton(0) && UIGame.ControlScheme == ControlScheme.PC)
			{
				base.State |= MovementState.Forward;
			}
		}
		if (InputManager.GetActionKeyDown(InputAction.Autorun) || InputManager.GetActionKeyDown(InputAction.MouseMiddle))
		{
			ToggleAutoRun();
		}
		if (base.State.IsLeftRotate())
		{
			float num = 120f * (float)SettingsManager.TurnSpeed * Time.deltaTime;
			base.transform.Rotate(0f, 0f - num, 0f);
			if (!UICamera.isOverUI && InputManager.GetActionKey(InputAction.MouseRight))
			{
				camController.RotateHorizontal(num);
			}
			else
			{
				camController.panToTarget = false;
			}
		}
		if (base.State.IsRightRotate())
		{
			float num2 = 120f * (float)SettingsManager.TurnSpeed * Time.deltaTime;
			base.transform.Rotate(0f, num2, 0f);
			if (!UICamera.isOverUI && InputManager.GetActionKey(InputAction.MouseRight))
			{
				camController.RotateHorizontal(0f - num2);
			}
			else
			{
				camController.panToTarget = false;
			}
		}
	}

	private void ToggleAutoRun()
	{
		base.IsAutoRunEnabled = !base.IsAutoRunEnabled;
		SnapToCameraOrientation();
	}
}
