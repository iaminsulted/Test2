using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Trigger Machine List")]
public class MATriggerMachineList : ListenerAction
{
	public List<BaseMachine> Machines;

	public bool checkRequirement;
}
