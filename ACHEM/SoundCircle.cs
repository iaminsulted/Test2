using UnityEngine;

public class SoundCircle : MonoBehaviour
{
	public Transform AudioSource;

	public float Radius;

	public float OuterRadius;

	public bool HasOuterRadius;

	private void Update()
	{
		if (!(Entities.Instance.me?.wrapper == null) && !(AudioSource == null))
		{
			Vector3 position = Entities.Instance.me.wrapper.transform.position;
			position.y = base.transform.position.y;
			Vector3 normalized = (position - base.transform.position).normalized;
			float sqrMagnitude = (position - base.transform.position).sqrMagnitude;
			if (sqrMagnitude > OuterRadius * OuterRadius && HasOuterRadius)
			{
				AudioSource.position = base.transform.position + normalized * OuterRadius;
			}
			else if (sqrMagnitude > Radius * Radius)
			{
				AudioSource.position = position;
			}
			else
			{
				AudioSource.position = base.transform.position + normalized * Radius;
			}
		}
	}
}
