using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectPool<T> where T : Object
{
	private readonly T refObj;

	private readonly List<T> available;

	private readonly List<T> used;

	public ObjectPool(T refObj, int startingPoolSize = 0)
	{
		if (refObj == null)
		{
			Debug.LogError("Object pool reference object is null! Type: " + typeof(T));
		}
		this.refObj = refObj;
		available = new List<T>();
		used = new List<T>();
		for (int i = 0; i < startingPoolSize; i++)
		{
			T item = Object.Instantiate(refObj);
			available.Add(item);
		}
	}

	public T Get()
	{
		if (available.Count > 0)
		{
			T val = available[0];
			available.RemoveAt(0);
			if (val != null)
			{
				used.Add(val);
				return val;
			}
		}
		return CreateNewObject();
	}

	public void Release(T obj)
	{
		used.Remove(obj);
		available.Add(obj);
		GameObject gameObject = obj as GameObject;
		if (gameObject != null)
		{
			gameObject.transform.SetAsLastSibling();
			gameObject.SetActive(value: false);
		}
	}

	private T CreateNewObject()
	{
		T val = Object.Instantiate(refObj);
		used.Add(val);
		return val;
	}
}
