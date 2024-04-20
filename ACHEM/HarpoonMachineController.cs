using System;
using System.Collections;
using UnityEngine;

public class HarpoonMachineController : MonoBehaviour
{
	private const float SyncTime = 0.5f;

	public HarpoonMachine machine;

	public Transform cam;

	public Transform horizontalPivot;

	public float horizontalAngleClamp;

	public Transform verticalPivot;

	public float verticalDownAngleClamp;

	public float verticalUpAngleClamp;

	public Transform firePoint;

	public float fireCooldown;

	public int itemID;

	private AEC client;

	private float elapsedSync;

	private CursorLockMode previousLock;

	private float elapsedFire;

	private Entity me;

	public void Update()
	{
		UpdateCamera();
		elapsedSync += Time.deltaTime;
		if (elapsedFire < fireCooldown)
		{
			elapsedFire += Time.deltaTime;
			ProgressBar.Show(elapsedFire / fireCooldown, "Reloading...");
			if (elapsedFire >= fireCooldown)
			{
				ProgressBar.Hide();
			}
		}
		if (elapsedSync >= 0.5f)
		{
			Sync();
		}
	}

	public void OnDestroy()
	{
		Disable();
	}

	public void OnDisable()
	{
		Disable();
	}

	public void Init()
	{
		client = AEC.getInstance();
		Camera camera = cam.gameObject.AddComponent<Camera>();
		int num = 0;
		num |= 1 << Layers.DEFAULT;
		num |= 1 << Layers.WATER;
		num |= 1 << Layers.TERRAIN;
		num |= 1 << Layers.FAR;
		num |= 1 << Layers.MIDDLE;
		num |= 1 << Layers.MIDDLEFAR;
		num |= 1 << Layers.CLOSE;
		num |= 1 << Layers.BG;
		num |= 1 << Layers.SNOW;
		num |= 1 << Layers.OTHER_PLAYERS;
		num |= 1 << Layers.NPCS;
		camera.cullingMask = num;
		camera.farClipPlane = Game.Instance.CurrentCell.CameraFarPlane;
		base.enabled = false;
		cam.gameObject.SetActive(value: false);
		me = Entities.Instance.me;
	}

	public void Enable()
	{
		elapsedFire = fireCooldown;
		elapsedSync = 0f;
		base.enabled = true;
		cam.gameObject.SetActive(value: true);
		InputManager.ActionEvent += OnActionKeyDown;
		if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
		{
			previousLock = Cursor.lockState;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		else
		{
			UIGame.Instance.ActionBar.DisableJump();
			UIJumpButton jumpButton = UIGame.Instance.ActionBar.JumpButton;
			jumpButton.Pressed = (Action)Delegate.Combine(jumpButton.Pressed, new Action(Disown));
		}
	}

	public void Disable()
	{
		if (me == null)
		{
			Debug.LogError("WARNING: me was null when trying to disable HarpoonMachineController");
			return;
		}
		me.Target = null;
		me.TargetNode = null;
		ProgressBar.Hide();
		base.enabled = false;
		cam.gameObject.SetActive(value: false);
		InputManager.ActionEvent -= OnActionKeyDown;
		if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
		{
			Cursor.visible = true;
			Cursor.lockState = previousLock;
		}
		else
		{
			UIJumpButton jumpButton = UIGame.Instance.ActionBar.JumpButton;
			jumpButton.Pressed = (Action)Delegate.Remove(jumpButton.Pressed, new Action(Disown));
			UIGame.Instance.ActionBar.EnableJump();
		}
	}

	private void OnActionKeyDown(InputAction action)
	{
		switch (action)
		{
		case InputAction.Spell_1:
		case InputAction.Spell_2:
		case InputAction.Spell_3:
		case InputAction.Spell_4:
		case InputAction.Spell_5:
		case InputAction.Cross_Skill:
			Fire();
			break;
		case InputAction.MouseLeft:
			if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
			{
				Fire();
			}
			break;
		case InputAction.Jump:
		case InputAction.Cancel:
		case InputAction.OpenLoot:
		case InputAction.LootAll:
			Disown();
			break;
		}
	}

	private void UpdateCamera()
	{
		Vector3 horizontalEuler = horizontalPivot.localEulerAngles;
		Vector3 verticalEuler = verticalPivot.localEulerAngles;
		if (horizontalEuler.y > 180f)
		{
			horizontalEuler.y -= 360f;
		}
		if (verticalEuler.x > 180f)
		{
			verticalEuler.x -= 360f;
		}
		switch (UICamera.currentScheme)
		{
		case UICamera.ControlScheme.Mouse:
			GetPCCameraMovement(ref horizontalEuler, ref verticalEuler);
			break;
		case UICamera.ControlScheme.Touch:
			GetMobileCameraMovement(ref horizontalEuler, ref verticalEuler);
			break;
		}
		horizontalEuler.y = Mathf.Clamp(horizontalEuler.y, 0f - horizontalAngleClamp, horizontalAngleClamp);
		verticalEuler.x = Mathf.Clamp(verticalEuler.x, verticalUpAngleClamp, verticalDownAngleClamp);
		horizontalPivot.localEulerAngles = horizontalEuler;
		verticalPivot.localEulerAngles = verticalEuler;
	}

	private void GetPCCameraMovement(ref Vector3 horizontalEuler, ref Vector3 verticalEuler)
	{
		float axisRaw = Input.GetAxisRaw("Mouse X");
		float axisRaw2 = Input.GetAxisRaw("Mouse Y");
		if (axisRaw != 0f)
		{
			horizontalEuler.y += axisRaw * 60f * Time.deltaTime;
		}
		if (axisRaw2 != 0f)
		{
			verticalEuler.x -= axisRaw2 * 60f * Time.deltaTime;
		}
		if (InputManager.GetActionKey(InputAction.Forward))
		{
			verticalEuler.x -= 60f * Time.deltaTime;
		}
		if (InputManager.GetActionKey(InputAction.Backward))
		{
			verticalEuler.x += 60f * Time.deltaTime;
		}
		if (InputManager.GetActionKey(InputAction.LeftRotate))
		{
			horizontalEuler.y -= 60f * Time.deltaTime;
		}
		if (InputManager.GetActionKey(InputAction.RightRotate))
		{
			horizontalEuler.y += 60f * Time.deltaTime;
		}
	}

	private void GetMobileCameraMovement(ref Vector3 horizontalEuler, ref Vector3 verticalEuler)
	{
		Vector2 normalized = UIGame.JSDelta.normalized;
		if (normalized.y >= 0.5f)
		{
			verticalEuler.x -= 60f * Time.deltaTime;
		}
		if (normalized.y <= -0.5f)
		{
			verticalEuler.x += 60f * Time.deltaTime;
		}
		if (normalized.x >= 0.5f)
		{
			horizontalEuler.y += 60f * Time.deltaTime;
		}
		if (normalized.x <= -0.5f)
		{
			horizontalEuler.y -= 60f * Time.deltaTime;
		}
	}

	private void Sync()
	{
		elapsedSync = 0f;
		client.sendRequest(new RequestMachineHarpoonSync(machine.ID, verticalPivot.localEulerAngles.x, horizontalPivot.localEulerAngles.y));
	}

	private void Fire()
	{
		if (firePoint == null)
		{
			if (AccessLevels.CanReceiveErrorMessages(Entities.Instance.me))
			{
				Chat.SendAdminMessage("Machine Error ID " + machine.ID + " - Harpoon Machine fire point is null");
			}
		}
		else if (!Session.MyPlayerData.HasItemInInventory(itemID))
		{
			Notification.ShowText("You do not have enough " + Items.Get(itemID).Name + "s.");
		}
		else
		{
			if (!(elapsedFire >= fireCooldown))
			{
				return;
			}
			bool flag = true;
			if (Physics.Raycast(firePoint.position, firePoint.forward, out var hitInfo, Game.Max_Click_Distance, Layers.LAYER_MASK_MOUSECLICK))
			{
				IClickable[] components = hitInfo.collider.GetComponents<IClickable>();
				IClickable[] array = components;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].OnClick(hitInfo.point);
				}
				flag = components.Length == 0;
			}
			if (flag)
			{
				me.Target = null;
				me.TargetNode = null;
			}
			StartCoroutine(ResetElapsedFire());
			Sync();
			client.sendRequest(new RequestMachineHarpoonFire(machine.ID));
		}
	}

	private IEnumerator ResetElapsedFire()
	{
		yield return new WaitForSeconds(0.25f);
		elapsedFire = 0f;
	}

	private void Disown()
	{
		client.sendRequest(new RequestMachineClick(machine.ID));
	}
}
