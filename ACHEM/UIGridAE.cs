using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGridAE : MonoBehaviour
{
	public enum Arrangement
	{
		Horizontal,
		Vertical
	}

	public bool invertX;

	public bool invertY;

	public Arrangement arrangement;

	public int maxPerLine;

	public float cellWidth = 200f;

	public float cellHeight = 200f;

	public bool repositionNow;

	public bool sorted;

	public bool hideInactive = true;

	private bool mStarted;

	private void Start()
	{
		mStarted = true;
		Reposition();
	}

	private void Update()
	{
		if (repositionNow)
		{
			repositionNow = false;
			Reposition();
		}
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	public void Reposition()
	{
		if (!mStarted)
		{
			repositionNow = true;
			return;
		}
		Transform transform = base.transform;
		int num = 0;
		int num2 = 0;
		int num3 = ((!invertX) ? 1 : (-1));
		int num4 = (invertY ? 1 : (-1));
		if (sorted)
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if ((bool)child)
				{
					list.Add(child);
				}
			}
			list.Sort(SortByName);
			int j = 0;
			for (int count = list.Count; j < count; j++)
			{
				Transform transform2 = list[j];
				if (NGUITools.GetActive(transform2.gameObject) || !hideInactive)
				{
					float z = transform2.localPosition.z;
					transform2.localPosition = ((arrangement == Arrangement.Horizontal) ? new Vector3((float)num3 * cellWidth * (float)num, (float)num4 * cellHeight * (float)num2, z) : new Vector3((float)num3 * cellWidth * (float)num2, (float)num4 * cellHeight * (float)num, z));
					if (++num >= maxPerLine && maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < transform.childCount; k++)
			{
				Transform child2 = transform.GetChild(k);
				if (NGUITools.GetActive(child2.gameObject) || !hideInactive)
				{
					float z2 = child2.localPosition.z;
					child2.localPosition = ((arrangement == Arrangement.Horizontal) ? new Vector3((float)num3 * cellWidth * (float)num, (float)num4 * cellHeight * (float)num2, z2) : new Vector3((float)num3 * cellWidth * (float)num2, (float)num4 * cellHeight * (float)num, z2));
					if (++num >= maxPerLine && maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
		}
		UIScrollView uIScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		if (uIScrollView != null)
		{
			uIScrollView.UpdateScrollbars(recalculateBounds: true);
		}
	}
}
