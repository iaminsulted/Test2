using UnityEngine;

public class EntityCollisionTriggerCameraFog : EntityCollisionTrigger
{
	public float LerpTime = 1f;

	public bool Fog;

	public FogMode FogMode;

	public Color FogColor;

	public float FogDensity;

	public float FogStartDistance;

	public float FogEndDistance;
}
