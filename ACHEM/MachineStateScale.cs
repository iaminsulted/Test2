using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Scale")]
public class MachineStateScale : MachineStateNonStatic
{
	public Vector3 Scale;

	private Vector3 rate;

	private void Start()
	{
		InteractiveObject.StateUpdated += OnMachineStateUpdate;
		OnMachineStateUpdate(InteractiveObject.State);
	}

	private void OnDestroy()
	{
		InteractiveObject.StateUpdated -= OnMachineStateUpdate;
	}

	private void Update()
	{
		base.Target.localScale += rate * Time.deltaTime;
	}

	private void OnMachineStateUpdate(byte state)
	{
		SetRate(state == State);
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
