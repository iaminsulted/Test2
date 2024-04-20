using UnityEngine;

public class CameraMan : MonoBehaviour
{
	public float moveSpeed;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.W))
		{
			base.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S))
		{
			base.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
		}
		float angle = Input.GetAxis("Mouse X") * 40f * Time.deltaTime;
		float angle2 = Input.GetAxis("Mouse Y") * -40f * Time.deltaTime;
		Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
		Quaternion quaternion2 = Quaternion.AngleAxis(angle2, Vector3.right);
		if (Input.GetMouseButton(0))
		{
			base.transform.localRotation *= quaternion * quaternion2;
		}
	}
}
