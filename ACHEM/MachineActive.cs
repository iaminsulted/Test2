using UnityEngine.Serialization;

public class MachineActive : MachineListener
{
	[FormerlySerializedAs("interactiveObject")]
	public InteractiveObject InteractiveObject;

	public override InteractiveObject GetInteractiveObj()
	{
		return InteractiveObject;
	}
}
