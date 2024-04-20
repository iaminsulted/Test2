using UnityEngine;

public class ScrollToZoom : MonoBehaviour
{
	public GameObject zoomObject;

	public float maxZoom = 1f;

	public float minZoom = 1f;

	public float zoom = 1f;

	public float speed = 0.3f;

	private void Start()
	{
	}

	private void LateUpdate()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			maxZoom += speed;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			maxZoom -= speed;
		}
		zoom += (maxZoom - zoom) * Time.deltaTime * 2f;
		zoomObject.transform.localScale = Vector3.one * zoom;
	}
}
