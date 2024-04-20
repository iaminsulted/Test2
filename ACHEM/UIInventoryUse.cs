using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventoryUse : UIWindow
{
	public static Color colorbtndefault = new Color(40f / 51f, 40f / 51f, 40f / 51f);

	public static Color colorbtnselected = new Color(13f / 15f, 26f / 51f, 0.08627451f);

	private static UIInventoryUse instance;

	public UIButton btnClose;

	public List<InventoryItem> curitems;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIInventoryUseItem> itemGOs;

	private UIItem selectedItem;

	private ObjectPool<GameObject> itemGOpool;

	private UIItemDetails uiInventoryItemDetail;

	public static void Load()
	{
		if (instance == null)
		{
			UIWindow.ClearWindows();
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/InventoryUse"), UIManager.Instance.transform).GetComponent<UIInventoryUse>();
			instance.Init();
		}
	}

	public static void Toggle()
	{
		if (instance == null)
		{
			Load();
		}
		else
		{
			instance.Close();
		}
	}

	protected override void Init()
	{
		base.Init();
		container = itemGOprefab.transform.parent;
		itemGOs = new List<UIInventoryUseItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		Session.MyPlayerData.ItemAdded += OnItemAdded;
		Session.MyPlayerData.ItemRemoved += OnItemRemoved;
		Session.MyPlayerData.ItemUpdated += OnItemUpdated;
		Game.Instance.combat.CDTrigger += OnSpellCast;
		curitems = Session.MyPlayerData.items.Where((InventoryItem x) => x.SpellID > 0).ToList();
		refresh();
	}

	private void OnCloseClick(GameObject go)
	{
		Close();
	}

	private void OnItemAdded(InventoryItem iItem)
	{
		curitems = Session.MyPlayerData.items;
		GameObject obj = itemGOpool.Get();
		obj.transform.SetParent(container, worldPositionStays: false);
		obj.SetActive(value: true);
		UIInventoryUseItem component = obj.GetComponent<UIInventoryUseItem>();
		component.Init(iItem);
		component.Clicked += OnItemClicked;
		itemGOs.Add(component);
		container.GetComponent<UIGrid>().Reposition();
	}

	private void OnItemRemoved(InventoryItem iItem)
	{
		curitems = Session.MyPlayerData.items;
		for (int num = itemGOs.Count - 1; num >= 0; num--)
		{
			UIInventoryUseItem uIInventoryUseItem = itemGOs[num];
			if (uIInventoryUseItem.Item == iItem)
			{
				if (selectedItem == uIInventoryUseItem)
				{
					selectedItem.Selected = false;
				}
				uIInventoryUseItem.gameObject.transform.SetAsLastSibling();
				itemGOpool.Release(uIInventoryUseItem.gameObject);
				uIInventoryUseItem.Clicked -= OnItemClicked;
				itemGOs.RemoveAt(num);
				break;
			}
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().InvalidateBounds();
		container.parent.GetComponent<UIPanel>().SetDirty();
	}

	private void OnItemUpdated(InventoryItem iItem)
	{
		for (int num = itemGOs.Count - 1; num >= 0; num--)
		{
			UIInventoryUseItem uIInventoryUseItem = itemGOs[num];
			if (uIInventoryUseItem.Item == iItem)
			{
				uIInventoryUseItem.Init(iItem);
				break;
			}
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().InvalidateBounds();
		container.parent.GetComponent<UIPanel>().SetDirty();
	}

	public void OnMCAddClick()
	{
		UIIAPStore.Show();
	}

	public void refresh()
	{
		foreach (UIInventoryUseItem itemGO in itemGOs)
		{
			itemGO.gameObject.transform.SetAsLastSibling();
			itemGOpool.Release(itemGO.gameObject);
			itemGO.Clicked -= OnItemClicked;
		}
		itemGOs.Clear();
		foreach (InventoryItem curitem in curitems)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIInventoryUseItem component = obj.GetComponent<UIInventoryUseItem>();
			component.Init(curitem);
			component.Clicked += OnItemClicked;
			itemGOs.Add(component);
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void OnItemClicked(UIItem si)
	{
		if (selectedItem != null)
		{
			selectedItem.Selected = false;
		}
		selectedItem = si as UIInventoryItem;
		InventoryItem inventoryItem = (InventoryItem)si.Item;
		if (inventoryItem.IsGuardian && !Session.MyPlayerData.IsGuardian())
		{
			Confirmation.Show("Guardian Only", "You need to be a Guardian to use this item, would you like to become a Guardian?", delegate(bool b)
			{
				if (b)
				{
					UIIAPStore.Show();
				}
			});
		}
		else if (inventoryItem.Level > Entities.Instance.me.Level)
		{
			Notification.ShowText("Requires Level " + inventoryItem.Level + " to use");
		}
		else
		{
			Game.Instance.SendItemUseRequest(inventoryItem);
		}
	}

	public void OnSpellCast(int spellId, float cooldown)
	{
		for (int num = itemGOs.Count - 1; num >= 0; num--)
		{
			UIInventoryUseItem uIInventoryUseItem = itemGOs[num];
			if (uIInventoryUseItem.spell.ID == spellId)
			{
				uIInventoryUseItem.CoolDown(Time.time, cooldown);
			}
		}
	}

	protected override void Destroy()
	{
		Session.MyPlayerData.ItemAdded -= OnItemAdded;
		Session.MyPlayerData.ItemRemoved -= OnItemRemoved;
		Session.MyPlayerData.ItemUpdated -= OnItemUpdated;
		Game.Instance.combat.CDTrigger -= OnSpellCast;
		base.Destroy();
	}
}
