using UnityEngine;

public class OverwriteParticleSystemRendererQueue : MonoBehaviour
{
	public int renderQueue = 3000;

	private void Start()
	{
		GetComponent<ParticleSystemRenderer>().material.renderQueue = renderQueue;
	}
}
