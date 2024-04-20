using System;
using System.Collections.Generic;
using UnityEngine;

public class UIChestLootDetails : MonoBehaviour
{
	public UIButton btnClose;

	public UIItemDetails UIInventoryItemDetail;

	public GameObject itemGOprefab;

	private List<Item> localLootItems;

	private Transform container;

	private List<UIItem> itemGOs;

	private UIInventoryItem selectedItem;

	private ObjectPool<GameObject> itemGOpool;

	private UITable lootDetailsUITable;

	public static void Load(List<Item> LootItems)
	{
	}

	public void Start()
	{
		container = itemGOprefab.transform.parent;
		itemGOs = new List<UIItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		AudioManager.Play2DSFX("sfx_engine_equip");
	}

	public void OnCloseClick(GameObject go)
	{
		foreach (UIItem itemGO in itemGOs)
		{
			itemGO.Clicked -= OnItemClicked;
		}
		foreach (Transform item in base.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		lootDetailsUITable.gameObject.SetActive(value: false);
		localLootItems = null;
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
	}

	public void LoadLoot(List<Item> LootItems)
	{
		localLootItems = LootItems;
		lootDetailsUITable.gameObject.SetActive(value: false);
		refresh();
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
	}

	private void OnLootDestroyed()
	{
	}

	public void refresh()
	{
		foreach (UIItem itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
			itemGO.Clicked -= OnItemClicked;
		}
		itemGOs.Clear();
		foreach (Item localLootItem in localLootItems)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIItem component = obj.GetComponent<UIItem>();
			component.Init(localLootItem);
			component.Clicked += OnItemClicked;
			itemGOs.Add(component);
		}
		lootDetailsUITable = container.GetComponent<UITable>();
		lootDetailsUITable.Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void OnItemClicked(UIItem si)
	{
		Debug.LogWarning("item click worked");
		if (selectedItem != null)
		{
			if (selectedItem == si)
			{
				if (!UIInventoryItemDetail.Visible)
				{
					UIInventoryItemDetail.LoadInventoryItem((InventoryItem)selectedItem.Item);
				}
				else if (si.Item.HasPreview)
				{
					UIPreview.Show(selectedItem.Item);
				}
				selectedItem.Background.spriteName = "aq3dui-menuslot-selected";
				return;
			}
			selectedItem.Selected = false;
		}
		selectedItem = si as UIInventoryItem;
		selectedItem.Selected = true;
		UIInventoryItemDetail.LoadInventoryItem((InventoryItem)selectedItem.Item);
	}
}
