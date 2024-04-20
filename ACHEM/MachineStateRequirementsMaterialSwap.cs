using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Requirements Material Swap")]
public class MachineStateRequirementsMaterialSwap : MachineState
{
	public Material MatDefault;

	public Material MatInteractive;

	public Material MatTriggered;

	private void Start()
	{
		InteractiveObject.StateUpdated += OnMachineStateUpdate;
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdated;
		UpdateMaterial(InteractiveObject.State, InteractiveObject.IsActive);
	}

	private void OnDestroy()
	{
		InteractiveObject.StateUpdated -= OnMachineStateUpdate;
		InteractiveObject.ActiveUpdated -= OnMachineActiveUpdated;
	}

	private void OnMachineStateUpdate(byte state)
	{
		UpdateMaterial(InteractiveObject.State, InteractiveObject.IsActive);
	}

	private void OnMachineActiveUpdated(bool enabled)
	{
		UpdateMaterial(InteractiveObject.State, InteractiveObject.IsActive);
	}

	private void UpdateMaterial(int state, bool isActive)
	{
		if (state == State)
		{
			base.TargetTransform.GetComponent<Renderer>().sharedMaterial = MatTriggered;
		}
		else if (isActive)
		{
			base.TargetTransform.GetComponent<Renderer>().sharedMaterial = MatInteractive;
		}
		else
		{
			base.TargetTransform.GetComponent<Renderer>().sharedMaterial = MatDefault;
		}
	}
}
