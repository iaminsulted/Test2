using System.Collections.Generic;
using UnityEngine;

public class ResourceCache
{
	public static Dictionary<string, Object> map = new Dictionary<string, Object>();

	public static List<string> assets = new List<string> { "PopupIA", "NPCPlate", "Materials/Spell_CircleTelegraph_Material", "Materials/Spell_RectTelegraph_Material" };

	public static void InitAssets()
	{
		foreach (string asset in assets)
		{
			map[asset] = Resources.Load(asset);
		}
	}

	public static T Load<T>(string asset) where T : Object
	{
		if (map.ContainsKey(asset))
		{
			return (T)map[asset];
		}
		return null;
	}
}
