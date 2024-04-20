using UnityEngine;

public class BakeProbes : MonoBehaviour
{
	public LightProbes bakedProbes;

	private void Awake()
	{
		BakeProbes bakeProbes = Object.FindObjectOfType<BakeProbes>();
		if (bakeProbes.bakedProbes != null)
		{
			LightmapSettings.lightProbes = bakeProbes.bakedProbes;
		}
	}

	public void ResetProbes()
	{
		LightmapSettings.lightProbes = bakedProbes;
	}
}
