using UnityEngine;

public class SM_moveThis : MonoBehaviour
{
	public float translationSpeedX;

	public float translationSpeedY = 1f;

	public float translationSpeedZ;

	public bool local = true;

	private Vector3 increment;

	public void Start()
	{
		increment = new Vector3(translationSpeedX, translationSpeedY, translationSpeedZ);
	}

	public void Update()
	{
		if (local)
		{
			base.transform.Translate(increment * Time.deltaTime);
		}
		else
		{
			base.transform.Translate(increment * Time.deltaTime, Space.World);
		}
	}
}
