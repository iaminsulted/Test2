using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Trigger CTA")]
public class MachineStateTriggerCTA : MachineState
{
	public List<ClientTriggerAction> Actions;

	private Transform targetGO;

	private bool highlighted;

	public float Range;

	private void Start()
	{
		InteractiveObject.StateUpdated += OnMachineStateUpdate;
	}

	private void OnDestroy()
	{
		InteractiveObject.StateUpdated -= OnMachineStateUpdate;
	}

	private void OnMachineStateUpdate(byte state)
	{
		if (state != State)
		{
			return;
		}
		foreach (ClientTriggerAction action in Actions)
		{
			action.Execute();
		}
	}
}
