using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active GO Inactive")]
public class MachineActiveGOInactive : MachineActive
{
	private void Start()
	{
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdated;
		base.TargetTransform.gameObject.SetActive(!InteractiveObject.IsActive);
	}

	private void OnMachineActiveUpdated(bool active)
	{
		base.TargetTransform.gameObject.SetActive(!InteractiveObject.IsActive);
	}
}
