using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionManager
{
	private readonly int SELECTION_LAYER = Layers.SELECTIONCAMERA;

	private static SelectionManager instance;

	private List<(Transform, GameObject, int)> prevLayers = new List<(Transform, GameObject, int)>();

	private List<Transform> selection = new List<Transform>();

	public static SelectionManager Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}
			return instance = new SelectionManager();
		}
	}

	public void Select(Transform t)
	{
		if (!selection.Contains(t))
		{
			Select(new Transform[1] { t });
		}
	}

	public void Deselect(Transform t)
	{
		selection.Remove(t);
		foreach (var item in prevLayers.Where(((Transform, GameObject, int) x) => x.Item1 == t).ToList())
		{
			item.Item2.layer = item.Item3;
			prevLayers.Remove(item);
		}
	}

	private void SetLayers(Transform t, Transform parentT)
	{
		foreach (Transform item in t)
		{
			SetLayers(item, parentT);
		}
		prevLayers.Add((parentT, t.gameObject, t.gameObject.layer));
		t.gameObject.layer = SELECTION_LAYER;
	}

	public void Select(IEnumerable<Transform> ts)
	{
		foreach (Transform t in ts)
		{
			SetLayers(t, t);
		}
		selection.AddRange(ts);
	}

	public void DeselectAll()
	{
		foreach (var prevLayer in prevLayers)
		{
			if (prevLayer.Item2 != null)
			{
				prevLayer.Item2.layer = prevLayer.Item3;
			}
		}
		prevLayers.Clear();
		selection.Clear();
	}

	public IEnumerable<Transform> GetSelection()
	{
		return selection.AsEnumerable();
	}
}
