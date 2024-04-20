using System.Collections.Generic;
using UnityEngine;

public class CullingManager : MonoBehaviour
{
	public Camera playerCamera;

	public GameObject parentGameObject;

	public float volumeDistance = 100f;

	public float checkDistance = 150f;

	public int totalNumberOfVolumes;

	private CullingVolume[] volumes;

	private CullingVolume[] mergeVolumes;

	public List<CullingVolume> currentVolumes = new List<CullingVolume>();

	private float startTime;

	private void Start()
	{
		FindCamera();
		MergeStart();
	}

	private void FindCamera()
	{
		if (playerCamera == null)
		{
			playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		}
	}

	private void Initialize()
	{
		if (!(parentGameObject != null))
		{
			return;
		}
		volumes = (CullingVolume[])Object.FindObjectsOfType(typeof(CullingVolume));
		Renderer[] componentsInChildren = parentGameObject.GetComponentsInChildren<Renderer>();
		Light[] componentsInChildren2 = parentGameObject.GetComponentsInChildren<Light>();
		totalNumberOfVolumes = volumes.Length;
		CullingVolume[] array = volumes;
		foreach (CullingVolume cullingVolume in array)
		{
			Renderer[] array2 = componentsInChildren;
			foreach (Renderer renderer in array2)
			{
				if (cullingVolume.GetComponent<BoxCollider>().bounds.Intersects(renderer.bounds))
				{
					cullingVolume.cullRenderers.Add(renderer);
				}
			}
			Light[] array3 = componentsInChildren2;
			foreach (Light light in array3)
			{
				if ((cullingVolume.GetComponent<BoxCollider>().bounds.Contains(light.transform.position) && light.type == LightType.Point) || light.type == LightType.Spot)
				{
					cullingVolume.cullLights.Add(light);
				}
			}
		}
	}

	private void CheckDistance()
	{
		Vector3 position = playerCamera.transform.position;
		CullingVolume[] array = volumes;
		foreach (CullingVolume cullingVolume in array)
		{
			float num = Vector3.Distance(position, cullingVolume.transform.position);
			if (!currentVolumes.Contains(cullingVolume))
			{
				if (num <= checkDistance && num <= volumeDistance)
				{
					currentVolumes.Add(cullingVolume);
				}
			}
			else if (currentVolumes.Contains(cullingVolume))
			{
				if (num > checkDistance)
				{
					currentVolumes.Remove(cullingVolume);
				}
				else if (num <= checkDistance && num > volumeDistance)
				{
					currentVolumes.Remove(cullingVolume);
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (playerCamera == null)
		{
			FindCamera();
		}
		if (!(playerCamera != null) || !(parentGameObject != null))
		{
			return;
		}
		CheckDistance();
		CullingVolume[] array = volumes;
		foreach (CullingVolume cullingVolume in array)
		{
			if (currentVolumes.Contains(cullingVolume))
			{
				foreach (Renderer cullRenderer in cullingVolume.cullRenderers)
				{
					if (!cullRenderer.enabled)
					{
						cullRenderer.enabled = true;
					}
				}
				foreach (Light cullLight in cullingVolume.cullLights)
				{
					if (!cullLight.enabled)
					{
						cullLight.enabled = true;
					}
				}
			}
			if (currentVolumes.Contains(cullingVolume))
			{
				continue;
			}
			foreach (Renderer cullRenderer2 in cullingVolume.cullRenderers)
			{
				if (cullRenderer2.enabled)
				{
					cullRenderer2.enabled = false;
				}
			}
			foreach (Light cullLight2 in cullingVolume.cullLights)
			{
				if (cullLight2.enabled)
				{
					cullLight2.enabled = false;
				}
			}
		}
	}

	private void MergeStart()
	{
		mergeVolumes = (CullingVolume[])Object.FindObjectsOfType(typeof(CullingVolume));
		MeshRenderer[] componentsInChildren = parentGameObject.GetComponentsInChildren<MeshRenderer>();
		CullingVolume[] array = mergeVolumes;
		foreach (CullingVolume cullingVolume in array)
		{
			List<MeshRenderer> list = new List<MeshRenderer>();
			Dictionary<Material, List<GameObject>> dictionary = new Dictionary<Material, List<GameObject>>();
			MeshRenderer[] array2 = componentsInChildren;
			foreach (MeshRenderer meshRenderer in array2)
			{
				if (meshRenderer != null && cullingVolume.GetComponent<BoxCollider>().bounds.Contains(meshRenderer.transform.position))
				{
					list.Add(meshRenderer);
				}
			}
			foreach (MeshRenderer item in list)
			{
				if (item != null && item.sharedMaterials.Length < 2 && (item.gameObject.GetComponent<Animation>() == null || ((bool)item.gameObject.GetComponent<Animation>() && item.gameObject.GetComponent<Animation>().clip == null)))
				{
					Material key = item.sharedMaterials[0];
					if (!dictionary.ContainsKey(key))
					{
						dictionary[key] = new List<GameObject>();
					}
					dictionary[key].Add(item.gameObject);
				}
			}
			foreach (KeyValuePair<Material, List<GameObject>> item2 in dictionary)
			{
				if (item2.Value.Count >= 2)
				{
					MergeGameObjects(cullingVolume.gameObject, item2.Value, item2.Key.name);
				}
			}
		}
		Initialize();
	}

	private void MergeGameObjects(GameObject parent, List<GameObject> objectList, string name)
	{
		GameObject gameObject = new GameObject(name + " - combined");
		gameObject.transform.parent = parentGameObject.transform;
		CombineInstance[] array = new CombineInstance[objectList.Count];
		for (int i = 0; i < objectList.Count; i++)
		{
			array[i].mesh = objectList[i].GetComponent<MeshFilter>().sharedMesh;
			array[i].transform = objectList[i].transform.localToWorldMatrix;
		}
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		gameObject.GetComponent<Renderer>().sharedMaterials = objectList[0].GetComponent<Renderer>().sharedMaterials;
		gameObject.SetActive(value: true);
		gameObject.GetComponent<Renderer>().enabled = true;
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(array, mergeSubMeshes: true, useMatrices: true);
		mesh.RecalculateBounds();
		mesh.name = name + "- combined";
		if (mesh.vertexCount <= 65000)
		{
			gameObject.GetComponent<MeshFilter>().mesh = mesh;
			foreach (GameObject @object in objectList)
			{
				Object.DestroyImmediate(@object.GetComponent<MeshFilter>().mesh);
				Object.DestroyImmediate(@object.GetComponent<MeshFilter>());
				Object.DestroyImmediate(@object.GetComponent<MeshRenderer>());
				if ((bool)@object.GetComponent<Animator>())
				{
					Object.DestroyImmediate(@object.GetComponent<Animator>());
				}
				if ((bool)@object.GetComponent<Animation>())
				{
					Object.DestroyImmediate(@object.GetComponent<Animation>());
				}
			}
			Debug.Log("Time to merge: " + (GameTime.realtimeSinceServerStartup - startTime));
		}
		else
		{
			Debug.Log("Could not merge " + name);
			Object.DestroyImmediate(gameObject);
			Object.DestroyImmediate(mesh);
		}
	}
}
