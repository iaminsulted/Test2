using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(0f, 0f, -360f * Time.deltaTime);
	}
}
