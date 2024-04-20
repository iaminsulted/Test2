using System;
using System.Collections.Generic;
using UnityEngine;

public class UIFriendRequests : UIMenuWindow
{
	private static UIFriendRequests instance;

	private static DateTime LastPull;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIInventoryFriendRequest> itemGOs;

	private UIInventoryClass selectedItem;

	private ObjectPool<GameObject> itemGOpool;

	public UIGrid grid;

	public static void Show()
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/UIFriendRequests"), UIManager.Instance.transform).GetComponent<UIFriendRequests>();
		}
	}

	private void OnEnable()
	{
		grid.gameObject.SetActive(value: false);
		Init();
		container = itemGOprefab.transform.parent;
		itemGOs = new List<UIInventoryFriendRequest>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		Load();
	}

	public void Load()
	{
		refresh();
	}

	public void refresh()
	{
		foreach (UIInventoryFriendRequest itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
		}
		itemGOs.Clear();
		foreach (IMessage friendRequest in Session.MyPlayerData.friendRequests)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIInventoryFriendRequest component = obj.GetComponent<UIInventoryFriendRequest>();
			component.Init(friendRequest);
			itemGOs.Add(component);
		}
		grid.gameObject.SetActive(value: true);
		grid.repositionNow = true;
	}
}
