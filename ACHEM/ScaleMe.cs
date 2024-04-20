using System.Collections;
using UnityEngine;

public class ScaleMe : MonoBehaviour
{
	public Vector3 multiplier = new Vector3(1f, 1f, 1f);

	public float delay;

	private Vector3 origScale;

	private int interval;

	private void Awake()
	{
		origScale = base.transform.localScale;
		interval = 1;
		StartCoroutine("ScaleIt");
	}

	public IEnumerator ScaleIt()
	{
		yield return new WaitForSeconds(delay);
		while (interval > 0)
		{
			base.transform.localScale = new Vector3(origScale.x + multiplier.x * (float)interval, origScale.y + multiplier.y * (float)interval, origScale.z + multiplier.z * (float)interval);
			interval++;
			yield return new WaitForFixedUpdate();
		}
	}
}
