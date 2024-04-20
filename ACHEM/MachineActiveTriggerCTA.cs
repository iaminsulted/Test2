using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Trigger CTA")]
public class MachineActiveTriggerCTA : MachineActive
{
	public List<ClientTriggerAction> Actions;

	public float Range;

	private Transform targetGO;

	private bool highlighted;

	private void Start()
	{
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdate;
	}

	private void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnMachineActiveUpdate;
	}

	private void OnMachineActiveUpdate(bool active)
	{
		if (!active)
		{
			return;
		}
		foreach (ClientTriggerAction action in Actions)
		{
			action.Execute();
		}
	}
}
