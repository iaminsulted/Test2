using UnityEngine;

public class OverwriteRendererQueue : MonoBehaviour
{
	public int renderQueue = 3000;

	private void Start()
	{
		GetComponent<Renderer>().material.renderQueue = renderQueue;
	}
}
