using UnityEngine;

public class NonStatic : MonoBehaviour, INonStatic
{
	public Transform transformParent
	{
		get
		{
			return base.transform.parent;
		}
		set
		{
			base.transform.parent = value;
		}
	}

	public Transform parent { get; set; }

	public Transform Target => base.transform;
}
