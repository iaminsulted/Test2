using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set Machine State")]
public class MASetMachineState : ListenerAction
{
	public BaseMachine Machine;

	public byte State;

	public int TargetMachineID;
}
