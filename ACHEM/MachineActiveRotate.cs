using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Rotate")]
public class MachineActiveRotate : MachineActiveNonStatic, ITimedListener
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
			Interpolate(InteractiveObject.IsActive);
		}
	}

	private void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnMachineActiveUpdate;
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
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdate;
	}

	public void SyncToServer(bool last)
	{
		SetTransformation();
		Interpolate(InteractiveObject.IsActive);
	}

	private void OnMachineActiveUpdate(bool active)
	{
		SetTransformation();
		Interpolate(active);
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
