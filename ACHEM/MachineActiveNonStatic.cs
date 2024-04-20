using UnityEngine;

public class MachineActiveNonStatic : MachineActive, INonStatic
{
	public Transform transformParent
	{
		get
		{
			return base.TargetTransform.parent;
		}
		set
		{
			base.TargetTransform.parent = value;
		}
	}

	public Transform parent { get; set; }

	public Transform Target => base.TargetTransform;
}
