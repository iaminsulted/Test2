using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State GO Inactive")]
public class MachineStateGOInactive : MachineState
{
	private void Start()
	{
		InteractiveObject.StateUpdated += OnMachineStateUpdate;
		OnMachineStateUpdate(InteractiveObject.State);
	}

	private void OnDestroy()
	{
		InteractiveObject.StateUpdated -= OnMachineStateUpdate;
	}

	private void OnMachineStateUpdate(byte state)
	{
		base.TargetTransform.gameObject.SetActive(state == State);
	}
}
