using UnityEngine;

public class DropSurface : MonoBehaviour
{
	public Transform Container;

	protected DropSurfaceType type;

	public DropSurfaceType Type => type;

	private void Start()
	{
		if (Container == null)
		{
			Container = base.transform.parent;
		}
	}
}
