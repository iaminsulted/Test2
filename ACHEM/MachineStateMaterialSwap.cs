using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Material Swap")]
public class MachineStateMaterialSwap : MachineState
{
	[Comment("State does not affect this script.")]
	public Material MatInactive;

	public Material MatActive;

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
		base.TargetTransform.GetComponent<Renderer>().sharedMaterial = ((state == 1) ? MatActive : MatInactive);
	}
}
