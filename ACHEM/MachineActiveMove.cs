using StatCurves;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Move")]
public class MachineActiveMove : MachineActiveNonStatic
{
	public Vector3 Translation;

	public Interpolator.SpaceType SpaceType;

	private Vector3 rate;

	private void Start()
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

	private void OnMachineActiveUpdate(bool active)
	{
		SetRate(active);
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
