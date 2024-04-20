using System.Collections.Generic;
using UnityEngine;

public class TransformCatelog : Dictionary<string, Transform>
{
	public TransformCatelog(Transform transform)
	{
		Catelog(transform);
	}

	private void Catelog(Transform transform)
	{
		Add(transform.name, transform);
		foreach (Transform item in transform)
		{
			Catelog(item);
		}
	}
}
