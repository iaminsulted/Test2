using System.Collections.Generic;
using UnityEngine;

public class DataPrefab : MonoBehaviour
{
	public int TableID;

	public int CellID = 1;

	public GameObject Environment;

	private GameObject environmentInstance;

	public string AreaName;

	public string objective;

	public bool isBossRoom;

	public bool previewCollider;

	public string FlythroughName = string.Empty;

	public float MaxCameraDistance;

	public float MinCameraDistance;

	public float CameraFarPlane = 2500f;

	public float maxClickDistance;

	[Range(0f, 2.5f)]
	public float intensity = 0.75f;

	public float focalLength = 10f;

	public float focalSize = 0.05f;

	public float aperture = 0.5f;

	public Transform ParticleGroup;

	public bool PlayOnStart;

	public bool UseCustomCulling;

	public CullingLayers CustomLayers;

	public List<BaseMachine> AutoTriggers;

	private void Awake()
	{
		if (Environment != null)
		{
			environmentInstance = Object.Instantiate(Environment);
			environmentInstance.transform.SetParent(base.transform);
		}
	}

	private void OnDestroy()
	{
		if (environmentInstance != null)
		{
			Object.Destroy(environmentInstance);
		}
	}
}
