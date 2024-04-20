using System.Collections.Generic;
using UnityEngine;

public class UniqueID : MonoBehaviour
{
	[ReadOnly]
	public string UniqueId;

	private static Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();

	private void Awake()
	{
		if (!map.ContainsKey(UniqueId))
		{
			map.Add(UniqueId, base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (map.ContainsKey(UniqueId))
		{
			map.Remove(UniqueId);
		}
	}

	public static GameObject Get(string id)
	{
		if (map.ContainsKey(id))
		{
			return map[id];
		}
		return null;
	}
}
