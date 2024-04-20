using UnityEngine;

public class CameraWaterRaycast : MonoBehaviour
{
	private const float RayDistance = 15f;

	private const FogMode waterModeDefault = FogMode.Linear;

	private const float waterDensityDefault = 0.01f;

	private const float waterNearDistanceDefault = -40f;

	private const float waterFarDistanceDefault = 75f;

	private FogMode fogMode;

	private Color fogColor;

	private float fogDensity;

	private float fogNearDistance;

	private float fogFarDistance;

	private Color waterColor;

	private float waterDensity;

	private float waterNearDistance;

	private float waterFarDistance;

	private bool isUnderwater;

	private RaycastHit hit;

	private Camera main;

	private void Start()
	{
		main = Camera.main;
	}

	private void Update()
	{
		if (!RenderSettings.fog)
		{
			return;
		}
		if (!isUnderwater)
		{
			if (!Physics.Raycast(base.transform.position, Vector3.up, out hit, 15f, 1 << Layers.WATER, QueryTriggerInteraction.Collide))
			{
				return;
			}
			WaterSampler component = hit.collider.gameObject.GetComponent<WaterSampler>();
			if (component != null)
			{
				SetFogProperties();
				RenderSettings.fogMode = component.fogMode;
				waterColor = component.fogColor;
				waterDensity = component.fogDensity;
				waterNearDistance = component.fogNearDistance;
				waterFarDistance = component.fogFarDistance;
				isUnderwater = true;
				SetFogToWater();
				return;
			}
			Renderer component2 = hit.collider.gameObject.GetComponent<Renderer>();
			if (component2 != null && component2.material != null && component2.material.HasProperty("_Color"))
			{
				SetFogProperties();
				RenderSettings.fogMode = FogMode.Linear;
				waterColor = component2.material.GetColor("_Color");
				waterDensity = 0.01f;
				waterNearDistance = -40f;
				waterFarDistance = 75f;
				isUnderwater = true;
				SetFogToWater();
			}
		}
		else if (!Physics.Raycast(base.transform.position, Vector3.up, 15f, 1 << Layers.WATER, QueryTriggerInteraction.Collide))
		{
			RenderSettings.fogMode = fogMode;
			isUnderwater = false;
			SetFogToNormal();
		}
	}

	public void SetFogProperties()
	{
		fogMode = RenderSettings.fogMode;
		fogColor = RenderSettings.fogColor;
		fogDensity = RenderSettings.fogDensity;
		fogNearDistance = RenderSettings.fogStartDistance;
		fogFarDistance = RenderSettings.fogEndDistance;
	}

	public void SetFogToWater()
	{
		RenderSettings.fogColor = waterColor;
		RenderSettings.fogDensity = waterDensity;
		RenderSettings.fogStartDistance = waterNearDistance;
		RenderSettings.fogEndDistance = waterFarDistance;
		main.backgroundColor = RenderSettings.fogColor;
	}

	public void SetFogToNormal()
	{
		RenderSettings.fogColor = fogColor;
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.fogStartDistance = fogNearDistance;
		RenderSettings.fogEndDistance = fogFarDistance;
		main.backgroundColor = RenderSettings.fogColor;
	}
}
