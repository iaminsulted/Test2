using System;
using UnityEngine;

public static class TransformExtensions
{
	public static Transform FindChildLike(this Transform trans, string search)
	{
		foreach (Transform tran in trans)
		{
			if (tran.gameObject.name.ToLowerInvariant().Contains(search.ToLowerInvariant()))
			{
				return tran;
			}
		}
		return null;
	}

	public static Transform FindChildRecursiveLike(this Transform trans, string search)
	{
		return trans.FindChildRecursiveHelper(search.ToLowerInvariant(), includeSimilar: true);
	}

	public static Transform FindChildRecursive(this Transform trans, string name)
	{
		return trans.FindChildRecursiveHelper(name, includeSimilar: false);
	}

	private static Transform FindChildRecursiveHelper(this Transform trans, string name, bool includeSimilar)
	{
		foreach (Transform tran in trans)
		{
			if (includeSimilar)
			{
				if (tran.gameObject.name.ToLowerInvariant().Contains(name))
				{
					return tran;
				}
			}
			else if (string.Compare(tran.gameObject.name, name, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return tran;
			}
			Transform transform2 = tran.FindChildRecursive(name);
			if (transform2 != null)
			{
				return transform2;
			}
		}
		return null;
	}
}
