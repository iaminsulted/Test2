using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovementController : ClientMovementController
{
	private static Dictionary<MovementState, InputAction> MovementKeyMap;

	private bool isLMBHeld;

	private bool isRMBHeld;

	static KeyboardMovementController()
	{
		MovementKeyMap = new Dictionary<MovementState, InputAction>();
		MovementKeyMap[MovementState.Forward] = InputAction.Forward;
		MovementKeyMap[MovementState.Backward] = InputAction.Backward;
		MovementKeyMap[MovementState.LeftRotate] = InputAction.LeftRotate;
		MovementKeyMap[MovementState.RightRotate] = InputAction.RightRotate;
		MovementKeyMap[MovementState.LeftStrafe] = InputAction.LeftStrafe;
		MovementKeyMap[MovementState.RightStrafe] = InputAction.RightStrafe;
	}

	public void Awake()
	{
	}

	public void Update()
	{
		isLMBHeld = Input.GetMouseButton(0);
		isRMBHeld = Input.GetMouseButton(1);
		base.State = MovementState.None;
		if (!UICamera.inputHasFocus)
		{
			foreach (MovementState key in MovementKeyMap.Keys)
			{
				if (InputManager.GetActionKey(MovementKeyMap[key]))
				{
					base.State |= key;
				}
			}
			if (InputManager.GetActionKeyDown(InputAction.Jump))
			{
				OnJump();
			}
		}
		if (isRMBHeld)
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
			if (isLMBHeld)
			{
				base.State |= MovementState.Forward;
			}
		}
		if (InputManager.GetActionKeyDown(InputAction.Autorun) || InputManager.GetActionKeyDown(InputAction.MouseMiddle))
		{
			base.IsAutoRunEnabled = !base.IsAutoRunEnabled;
		}
		ApplyAutoRun();
	}
}
