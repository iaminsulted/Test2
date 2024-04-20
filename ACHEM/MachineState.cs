using UnityEngine.Serialization;

public class MachineState : MachineListener
{
	[FormerlySerializedAs("Machine")]
	public InteractiveObject InteractiveObject;

	public byte State;

	public override InteractiveObject GetInteractiveObj()
	{
		return InteractiveObject;
	}
}
