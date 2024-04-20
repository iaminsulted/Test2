using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Trigger Random Machine")]
public class MATriggerRandomMachine : ListenerAction
{
	public List<BaseMachine> machines;

	public bool checkRequirement;

	public bool excludeTriggered;

	public float minDelayPerTrigger;

	public float maxDelayPerTrigger;

	public int count;
}
