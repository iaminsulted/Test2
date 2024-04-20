using StatCurves;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Move")]
public class MachineStateMove : MachineStateNonStatic
{
	public Vector3 Translation;

	public Interpolator.SpaceType SpaceType;

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
		if (rate != Vector3.zero)
		{
			if (SpaceType == Interpolator.SpaceType.Global)
			{
				base.Target.position += rate * Time.deltaTime;
			}
			else
			{
				base.Target.localPosition += rate * Time.deltaTime;
			}
		}
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
			rate = Translation;
		}
		else
		{
			rate = Vector3.zero;
		}
	}

	private void Interpolate()
	{
		if (InteractiveObject is BaseMachine)
		{
			float num = GameTime.realtimeSinceServerStartup - lastServerTimeStamp;
			if (SpaceType == Interpolator.SpaceType.Global)
			{
				base.Target.position += rate * num;
			}
			else
			{
				base.Target.localPosition += rate * num;
			}
		}
	}
}
