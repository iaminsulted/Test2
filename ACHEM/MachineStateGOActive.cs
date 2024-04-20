using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State GO Active")]
public class MachineStateGOActive : MachineState
{
	private void Start()
	{
		if (InteractiveObject != null)
		{
			InteractiveObject.StateUpdated += OnMachineStateUpdate;
		}
		if (InteractiveObject != null)
		{
			OnMachineStateUpdate(InteractiveObject.State);
		}
	}

	private void OnDestroy()
	{
		if (InteractiveObject != null)
		{
			InteractiveObject.StateUpdated -= OnMachineStateUpdate;
		}
	}

	private void OnMachineStateUpdate(byte state)
	{
		if (base.TargetTransform != null)
		{
			base.TargetTransform.gameObject.SetActive(state != State);
		}
		else
		{
			Debug.LogError("TargetTransform on MachineListener was null");
		}
	}
}
