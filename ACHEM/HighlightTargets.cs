using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HighlightTargets
{
	private static Dictionary<HighlightTargetName, List<GameObject>> targets = new Dictionary<HighlightTargetName, List<GameObject>>();

	public static void Add(HighlightTarget target)
	{
		if (!targets.ContainsKey(target.Target))
		{
			targets.Add(target.Target, new List<GameObject>());
		}
		targets[target.Target].Add(target.gameObject);
	}

	public static void Remove(HighlightTarget target)
	{
		if (targets.ContainsKey(target.Target))
		{
			targets[target.Target].Remove(target.gameObject);
		}
	}

	public static GameObject Get(HighlightTargetName targetName)
	{
		if (targets.ContainsKey(targetName))
		{
			return targets[targetName].FirstOrDefault();
		}
		return null;
	}

	public static GameObject Get(HighlightTargetName targetName, Func<GameObject, bool> search)
	{
		if (targets.ContainsKey(targetName))
		{
			return targets[targetName].Where(search).FirstOrDefault();
		}
		return null;
	}

	public static List<GameObject> GetAll(HighlightTargetName targetName)
	{
		if (targets.ContainsKey(targetName))
		{
			return targets[targetName];
		}
		return null;
	}
}
