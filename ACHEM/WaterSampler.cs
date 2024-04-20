using UnityEngine;

public class WaterSampler : MonoBehaviour
{
	[HideInInspector]
	public FogMode fogMode = FogMode.Linear;

	[HideInInspector]
	public Color fogColor;

	[HideInInspector]
	public float fogDensity = 0.01f;

	[HideInInspector]
	public float fogNearDistance = -40f;

	[HideInInspector]
	public float fogFarDistance = 75f;
}
