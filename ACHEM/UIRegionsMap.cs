using System.Collections.Generic;
using UnityEngine;

public class UIRegionsMap : UIMenuWindow
{
	public UIScrollView scrollview;

	public UITable table;

	public UIGrid grid;

	public UILabel title;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIItem> itemGOs;

	private ObjectPool<GameObject> itemGOpool;

	public static void Show()
	{
	}

	public void InitComplete()
	{
		itemGOs = new List<UIItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		base.gameObject.SetActive(value: false);
		container = itemGOprefab.transform.parent;
		refresh();
	}

	private void refresh()
	{
		foreach (UIMapListItem itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
		}
		itemGOs.Clear();
		container.GetComponent<UITable>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
		base.gameObject.SetActive(value: true);
	}

	public void OnItemClicked(UIItem item)
	{
	}
}
