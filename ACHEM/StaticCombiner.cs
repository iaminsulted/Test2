using System.Collections.Generic;
using UnityEngine;

public class StaticCombiner : MonoBehaviour
{
	public bool batch;

	public GameObject[] roots;

	private Dictionary<Material, List<GameObject>> staticBatchGroups = new Dictionary<Material, List<GameObject>>();

	private void Start()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!renderer.GetComponent<NonStatic>())
			{
				if (staticBatchGroups.ContainsKey(renderer.sharedMaterial))
				{
					staticBatchGroups[renderer.sharedMaterial].Add(renderer.gameObject);
					continue;
				}
				staticBatchGroups.Add(renderer.sharedMaterial, new List<GameObject> { renderer.gameObject });
			}
		}
	}

	private void Update()
	{
		if (!batch)
		{
			return;
		}
		foreach (KeyValuePair<Material, List<GameObject>> staticBatchGroup in staticBatchGroups)
		{
			GameObject staticBatchRoot = new GameObject(staticBatchGroup.Key.name);
			StaticBatchingUtility.Combine(staticBatchGroup.Value.ToArray(), staticBatchRoot);
		}
		batch = false;
	}
}
