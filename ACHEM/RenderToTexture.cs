using UnityEngine;

public class RenderToTexture : MonoBehaviour
{
	public Camera cam;

	private bool released;

	private void Start()
	{
		if (!cam)
		{
			cam = GetComponent<Camera>();
			if (!cam)
			{
				Debug.Log("No Camera was found on " + base.gameObject.name + " to use RenderToTexture.");
				Debug.Log("Please remove the RenderToTexture script or attach a Camera with a target Render Texture");
			}
		}
	}

	private void OnDestroy()
	{
		if ((bool)cam.targetTexture)
		{
			cam.targetTexture.Release();
		}
	}
}
