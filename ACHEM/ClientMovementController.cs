using System;
using UnityEngine;

public abstract class ClientMovementController : MovementController
{
	private float lastMoveSynchTime;

	private float lastRotSynchTime;

	private Vector3 lastSynchPos;

	private Quaternion lastSynchRot;

	private MovementState lastSyncState;

	private AutoRun autoRun;

	private RequestMovement moveRequest = new RequestMovement();

	private const float moveSynchDelta = 0.5f;

	private const float rotSynchDelta = 0.2f;

	private float lastRotFrameValue;

	private int disableAutorunSteps;

	protected CameraController camController;

	protected Transform autoRunTarget;

	private float range;

	public bool isAutoRunEnabled;

	public bool IsAutoRunEnabled
	{
		get
		{
			return isAutoRunEnabled;
		}
		set
		{
			isAutoRunEnabled = value;
			if (!isAutoRunEnabled)
			{
				autoRunTarget = null;
			}
			else
			{
				disableAutorunSteps = 0;
			}
		}
	}

	protected bool ShouldTurnWithCamera
	{
		get
		{
			bool num = (UIGame.ControlScheme == ControlScheme.HANDHELD || Input.GetMouseButton(1)) && (IsMoving() || Entities.Instance.me.IsChargingAoe) && autoRunTarget == null;
			bool flag = (UIGame.ControlScheme == ControlScheme.HANDHELD || !Input.GetMouseButton(0)) && IsMoving() && Entities.Instance.me.TargetTransform != null && autoRunTarget == null;
			return num || flag;
		}
	}

	public event Action<AutoRun> TargetInRange;

	public virtual void Start()
	{
		lastMoveSynchTime = Time.time;
		lastSynchPos = base.transform.position;
		lastSynchRot = base.transform.localRotation;
		lastSyncState = MovementState.None;
	}

	public void FixedUpdate()
	{
		BroadcastMovement();
		lastRotFrameValue = base.transform.rotation.eulerAngles.y;
	}

	public override void LookAt2D(Transform target)
	{
		float y = base.transform.rotation.eulerAngles.y;
		base.LookAt2D(target);
		camController.RotateHorizontal(y - base.transform.rotation.eulerAngles.y);
	}

	public override void Stop()
	{
		base.Stop();
		IsAutoRunEnabled = false;
		BroadcastMovement(forcesync: true);
	}

	protected void ApplyAutoRun()
	{
		if (!base.State.IsForward() && disableAutorunSteps == 0)
		{
			disableAutorunSteps = 1;
		}
		if (base.State.IsForward() && disableAutorunSteps == 1)
		{
			disableAutorunSteps = 2;
		}
		if (!base.State.IsForward() && disableAutorunSteps == 2)
		{
			IsAutoRunEnabled = false;
		}
		else if (base.State.IsBackward())
		{
			if (IsAutoRunEnabled)
			{
				IsAutoRunEnabled = false;
			}
		}
		else if (IsAutoRunEnabled)
		{
			base.State |= MovementState.Forward;
		}
	}

	protected void TargetAutoRunUpdate()
	{
		if (!IsAutoRunEnabled || autoRunTarget == null)
		{
			return;
		}
		if (IsMoving())
		{
			IsAutoRunEnabled = false;
			return;
		}
		LookAt2D(autoRunTarget);
		if ((base.transform.position - autoRunTarget.position).magnitude <= range)
		{
			IsAutoRunEnabled = false;
			BroadcastMovement(forcesync: true);
			this.TargetInRange?.Invoke(autoRun);
		}
	}

	public void BroadcastMovement(bool forcesync = false, bool jump = false, bool forceFullPacket = false)
	{
		float time = Time.time;
		Vector3 position = base.transform.position;
		Quaternion localRotation = base.transform.localRotation;
		float y = base.transform.localRotation.eulerAngles.y;
		bool flag = time - lastMoveSynchTime > 0.5f;
		bool flag2 = (position - lastSynchPos).sqrMagnitude > 3f;
		bool flag3 = false;
		bool num = Quaternion.Angle(localRotation, lastSynchRot) > 15f && time - lastRotSynchTime > 0.2f;
		bool flag4 = Mathf.Abs(y - lastRotFrameValue) >= 30f;
		if (num || flag4)
		{
			flag3 = true;
		}
		if (!(forcesync || jump || lastSyncState != base.State || flag3) && !(flag && flag2))
		{
			return;
		}
		if (jump)
		{
			moveRequest.cmd = 4;
		}
		else if (lastSyncState != base.State || forceFullPacket)
		{
			moveRequest.cmd = 1;
		}
		else
		{
			moveRequest.cmd = 2;
			if (Quaternion.Angle(lastSynchRot, localRotation) < 0.01f && Vector3.Distance(lastSynchPos, position) < 0.01f)
			{
				return;
			}
		}
		lastRotSynchTime = time;
		lastMoveSynchTime = time;
		lastSynchRot = localRotation;
		lastSynchPos = position;
		lastSyncState = base.State;
		moveRequest.state = base.State;
		moveRequest.posX = position.x;
		moveRequest.posY = position.y;
		moveRequest.posZ = position.z;
		moveRequest.rotY = y;
		moveRequest.timeStamp = GameTime.realtimeSinceServerStartup;
		AEC.getInstance().sendRequest(moveRequest);
	}

	public void SetCamController(CameraController camController)
	{
		this.camController = camController;
	}

	public void TargetAutoRun(Transform target, float range, AutoRun autoRun)
	{
		autoRunTarget = target;
		this.range = range;
		IsAutoRunEnabled = true;
		this.autoRun = autoRun;
		LookAt2D(target);
	}
}
