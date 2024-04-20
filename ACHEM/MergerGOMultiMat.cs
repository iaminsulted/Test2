using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MergerGOMultiMat
{
	public static void merge(GameObject go)
	{
		Dictionary<Material, List<GameObject>> dictionary = new Dictionary<Material, List<GameObject>>();
		MeshRenderer[] componentsInChildren = go.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			if (meshRenderer != null && meshRenderer.enabled && meshRenderer.sharedMaterials.Length < 2)
			{
				Material material = meshRenderer.sharedMaterials[0];
				if (material == null)
				{
					Debug.Log(meshRenderer.name);
				}
				if (!dictionary.ContainsKey(material))
				{
					dictionary[material] = new List<GameObject>();
				}
				dictionary[material].Add(meshRenderer.gameObject);
			}
		}
		foreach (KeyValuePair<Material, List<GameObject>> item in dictionary)
		{
			if (item.Key == null)
			{
				Debug.Log(item.Key);
			}
			mergeGOs(go, item.Value, item.Key.name);
		}
	}

	public static void mergeGOs(GameObject parent, List<GameObject> gos, string name)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.transform.parent = parent.transform;
		gameObject.layer = parent.layer;
		CombineInstance[] array = new CombineInstance[gos.Count];
		for (int i = 0; i < gos.Count; i++)
		{
			array[i].mesh = gos[i].GetComponent<MeshFilter>().sharedMesh;
			array[i].transform = gos[i].transform.localToWorldMatrix;
		}
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		gameObject.GetComponent<Renderer>().sharedMaterials = gos[0].GetComponent<Renderer>().sharedMaterials;
		gameObject.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
		gameObject.GetComponent<Renderer>().receiveShadows = true;
		gameObject.SetActive(value: true);
		gameObject.GetComponent<Renderer>().enabled = true;
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(array, mergeSubMeshes: true, useMatrices: true);
		mesh.RecalculateBounds();
		gameObject.GetComponent<MeshFilter>().mesh = mesh;
		foreach (GameObject go in gos)
		{
			Object.DestroyImmediate(go.GetComponent<MeshRenderer>());
		}
	}
}
