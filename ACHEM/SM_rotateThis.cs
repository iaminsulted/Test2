using UnityEngine;

public class SM_rotateThis : MonoBehaviour
{
	public float rotationSpeedX = 90f;

	public float rotationSpeedY;

	public float rotationSpeedZ;

	public bool local = true;

	private Vector3 rotationVector;

	private void Start()
	{
		rotationVector = new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
	}

	private void Update()
	{
		if (local)
		{
			base.transform.Rotate(rotationVector * Time.deltaTime);
		}
		else
		{
			base.transform.Rotate(rotationVector * Time.deltaTime, Space.World);
		}
	}
}
