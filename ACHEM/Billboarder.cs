using UnityEngine;

public class Billboarder : MonoBehaviour
{
	private Transform cameraT;

	private void Start()
	{
	}

	private void LateUpdate()
	{
		base.transform.LookAt(base.transform.position + cameraT.transform.rotation * Vector3.forward, cameraT.transform.rotation * Vector3.up);
	}

	public void setCamera(Camera c)
	{
		cameraT = c.transform;
	}
}
