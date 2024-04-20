using System.Collections;
using UnityEngine;

public class SM_hooverThis : MonoBehaviour
{
	public float Amount = 1f;

	public float Duration = 1f;

	public bool UpAndDown = true;

	private void Start()
	{
		StartCoroutine(Hoover());
	}

	private IEnumerator Hoover()
	{
		Debug.Log(1);
		Vector3 start = base.transform.position;
		Vector3 end = (UpAndDown ? (base.transform.position + Vector3.up * Amount) : (base.transform.position + Vector3.left * Amount));
		float elapsedTime = 0f;
		while (elapsedTime < Duration)
		{
			float num = elapsedTime / Duration;
			num = num * num * (3f - 2f * num);
			base.transform.position = Vector3.Lerp(start, end, num);
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= Duration)
			{
				Vector3 vector = start;
				start = end;
				end = vector;
				elapsedTime = 0f;
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
