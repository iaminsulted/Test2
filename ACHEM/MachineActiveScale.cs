using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Scale")]
public class MachineActiveScale : MachineActiveNonStatic
{
	public Vector3 Scale;

	private Vector3 rate;

	public void Start()
	{
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdate;
		OnMachineActiveUpdate(InteractiveObject.IsActive);
	}

	private void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnMachineActiveUpdate;
	}

	private void Update()
	{
		base.TargetTransform.localScale += rate * Time.deltaTime;
	}

	private void OnMachineActiveUpdate(bool active)
	{
		SetRate(active);
		Interpolate();
	}

	private void SetRate(bool active)
	{
		if (active)
		{
			rate = Scale;
		}
		else
		{
			rate = -Scale;
		}
	}

	private void Interpolate()
	{
		if (InteractiveObject is BaseMachine)
		{
			float num = GameTime.realtimeSinceServerStartup - lastServerTimeStamp;
			base.Target.localScale += rate * num;
		}
	}
}
