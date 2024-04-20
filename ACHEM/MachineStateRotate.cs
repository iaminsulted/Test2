using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Rotate")]
public class MachineStateRotate : MachineStateNonStatic, ITimedListener
{
	public Vector3 Rotation;

	private Vector3 current;

	public float LastServerTimestamp => lastServerTimeStamp;

	private void Start()
	{
		if (InteractiveObject is ClientTrigger)
		{
			Init();
			SetTransformation();
			Interpolate(InteractiveObject.State == State);
		}
	}

	private void OnDestroy()
	{
		InteractiveObject.StateUpdated -= OnMachineStateUpdate;
	}

	private void Update()
	{
		if (update)
		{
			ApplyAction();
		}
	}

	public void Init()
	{
		InteractiveObject.StateUpdated += OnMachineStateUpdate;
	}

	public void SyncToServer(bool last)
	{
		SetTransformation();
		Interpolate(InteractiveObject.State == State);
	}

	private void OnMachineStateUpdate(byte state)
	{
		SetTransformation();
		Interpolate(state == State);
	}

	private void SetTransformation()
	{
		current = base.TargetTransform.localEulerAngles;
		NormalizeRotation();
	}

	private void Interpolate(bool active)
	{
		if (active)
		{
			if (InteractiveObject is BaseMachine)
			{
				float num = GameTime.realtimeSinceServerStartup - lastServerTimeStamp;
				current += Rotation * num;
				NormalizeRotation();
				ApplyRotation();
			}
			lastServerTimeStamp = GameTime.realtimeSinceServerStartup;
			update = true;
		}
		else
		{
			update = false;
		}
	}

	private void NormalizeRotation()
	{
		current.x %= 360f;
		current.y %= 360f;
		current.z %= 360f;
	}

	private void ApplyAction()
	{
		current += Rotation * (GameTime.realtimeSinceServerStartup - lastServerTimeStamp);
		lastServerTimeStamp = GameTime.realtimeSinceServerStartup;
		NormalizeRotation();
		ApplyRotation();
	}

	private void ApplyRotation()
	{
		base.TargetTransform.localEulerAngles = current;
	}
}
