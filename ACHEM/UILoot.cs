using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UILoot : UIWindow
{
	public UIButton btnClose;

	public GameObject itemGOprefab;

	public UIButton btnLootAll;

	public UILabel labelLootAll;

	public static ComLoot SelectedLootBag;

	private static UILoot instance;

	private List<ComLoot> allLoot = new List<ComLoot>();

	private Transform container;

	private List<UILootItem> uiLootItems;

	private ObjectPool<GameObject> itemGOpool;

	private bool showingAllLoot;

	public static UILoot Instance => instance;

	public static void Load(ComLoot loot)
	{
		if (instance == null)
		{
			UIWindow.ClearWindows();
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Loot"), UIManager.Instance.transform).GetComponent<UILoot>();
			instance.Init();
		}
		SelectedLootBag = loot;
		instance.showingAllLoot = false;
		instance.InitLoot(new List<ComLoot> { loot });
	}

	public static void Toggle()
	{
		if (instance == null)
		{
			LoadAllNear();
		}
		else
		{
			instance.Close();
		}
	}

	public static void LoadAllNear()
	{
		if (LootBags.Bags.Count != 0)
		{
			if (instance == null)
			{
				UIWindow.ClearWindows();
				instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Loot"), UIManager.Instance.transform).GetComponent<UILoot>();
				instance.Init();
			}
			instance.showingAllLoot = true;
			instance.InitLoot(LootBags.Bags.Values.ToList());
		}
	}

	protected override void Init()
	{
		base.Init();
		container = itemGOprefab.transform.parent;
		uiLootItems = new List<UILootItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		AudioManager.Play2DSFX("sfx_engine_equip");
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnLootAll.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnLootAllClick));
		labelLootAll.text = "Loot All";
		if (Platform.IsDesktop && SettingsManager.GetKeyCodeByAction(InputAction.LootAll) != 0)
		{
			UILabel uILabel = labelLootAll;
			uILabel.text = uILabel.text + " [" + SettingsManager.GetHotkeyByAction(InputAction.LootAll) + "]";
		}
	}

	private void OnEnable()
	{
		LootBags.LootItemRemoved += OnLootItemRemoved;
		LootBags.BagAdded += OnLootAdded;
		LootBags.BagRemoved += OnLootRemoved;
	}

	private void OnDisable()
	{
		LootBags.LootItemRemoved -= OnLootItemRemoved;
		LootBags.BagAdded -= OnLootAdded;
		LootBags.BagRemoved -= OnLootRemoved;
	}

	public void OnCloseClick(GameObject go)
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnLootAll.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnLootAllClick));
		Close();
	}

	private void InitLoot(List<ComLoot> lootList)
	{
		allLoot.Clear();
		AddLoot(lootList);
		container.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void AddLoot(List<ComLoot> lootList)
	{
		foreach (ComLoot loot in lootList)
		{
			allLoot.Add(loot);
		}
		Refresh();
	}

	private void OnLootItemRemoved(int LootBagID, int LootItemID)
	{
		for (int num = uiLootItems.Count - 1; num >= 0; num--)
		{
			UILootItem uILootItem = uiLootItems[num];
			if (uILootItem.Item is LootItem lootItem && lootItem.LootBagID == LootBagID && lootItem.LootItemID == LootItemID)
			{
				uILootItem.gameObject.transform.SetAsLastSibling();
				itemGOpool.Release(uILootItem.gameObject);
				uILootItem.Clicked -= OnItemClicked;
				uiLootItems.RemoveAt(num);
				break;
			}
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().InvalidateBounds();
		container.parent.GetComponent<UIPanel>().SetDirty();
	}

	private void OnLootAdded(int lootID)
	{
		if (showingAllLoot && LootBags.Bags.TryGetValue(lootID, out var value))
		{
			AddLoot(new List<ComLoot> { value });
		}
	}

	private void OnLootRemoved(int lootID)
	{
		allLoot.RemoveAll((ComLoot p) => p.ID == lootID);
		if (allLoot.Count == 0)
		{
			Close();
		}
		else
		{
			Refresh();
		}
	}

	private void Refresh()
	{
		foreach (UILootItem uiLootItem in uiLootItems)
		{
			if (uiLootItem != null)
			{
				itemGOpool.Release(uiLootItem.gameObject);
				uiLootItem.Clicked -= OnItemClicked;
			}
		}
		uiLootItems.Clear();
		allLoot = allLoot.OrderBy((ComLoot loot) => loot.timeStamp).ToList();
		foreach (ComLoot item in allLoot)
		{
			foreach (LootItem item2 in item.Items)
			{
				MakeItem(item2, item);
			}
		}
		container.GetComponent<UIGrid>().Reposition();
	}

	private void MakeItem(LootItem item, ComLoot lootBag)
	{
		GameObject obj = itemGOpool.Get();
		obj.transform.SetParent(container, worldPositionStays: false);
		obj.SetActive(value: true);
		UILootItem component = obj.GetComponent<UILootItem>();
		component.modifierRarity = item.RarityModifier;
		component.Init(item);
		component.InitLoot(lootBag);
		component.Clicked += OnItemClicked;
		if (component.Item is LootItem lootItem)
		{
			lootItem.LootBagID = lootBag.ID;
		}
		uiLootItems.Add(component);
	}

	private void OnItemClicked(UIItem uiItem)
	{
		string msg;
		if (!(uiItem.Item is LootItem lootItem))
		{
			Debug.LogWarning("item needs to be a LootItem");
		}
		else if (!CanHaveItem(lootItem, out msg))
		{
			Notification.ShowText(msg);
		}
		else
		{
			Game.Instance.SendLootItemRequest(lootItem.LootBagID, lootItem.LootItemID);
		}
	}

	private bool CanHaveItem(Item item, out string msg)
	{
		msg = "";
		if (item == null)
		{
			msg = "Invalid Item";
		}
		else if (!Session.MyPlayerData.HasRoomInInventory(item))
		{
			msg = "Inventory is full";
		}
		else if (item.IsUnique && Session.MyPlayerData.allItems.Values.Any((List<InventoryItem> x) => x.Any((InventoryItem y) => y.ID == item.ID && y.BankID > 0)))
		{
			msg = "This item is in your bank. Cannot have multiple of this item";
		}
		else if (item.IsUnique && Session.MyPlayerData.HasMaxOfUniqueItem(item))
		{
			if (item.MaxStack == 1)
			{
				msg = "Cannot have multiple of this item";
			}
			else
			{
				msg = "Cannot have multiple stacks of this item";
			}
		}
		return string.IsNullOrEmpty(msg);
	}

	public void OnLootAllClick(GameObject go)
	{
		List<Item> list = new List<Item>();
		list = ((!Instance.showingAllLoot) ? ((IEnumerable<LootItem>)SelectedLootBag.Items).Select((Func<LootItem, Item>)((LootItem ui) => ui)).ToList() : uiLootItems.Select((UILootItem ui) => ui.Item).ToList());
		List<Item> list2 = list.Where((Item p) => p.MaxStack <= 1).ToList();
		list2.AddRange(from p in list
			where p.MaxStack > 1
			select p into i
			group i by i.ID into p
			select new Item(p.First(), p.Sum((Item s) => s.Qty)));
		foreach (Item item in list2)
		{
			string msg = "";
			if (!CanHaveItem(item, out msg))
			{
				Notification.ShowText(msg);
				break;
			}
		}
		string msg2;
		List<Item> list3 = list2.Where((Item p) => CanHaveItem(p, out msg2)).ToList();
		if (list3.Count > 0)
		{
			if (!Session.MyPlayerData.HasRoomInInventory(list3))
			{
				Notification.ShowText("Not enough room in inventory to loot all");
			}
			if (instance.showingAllLoot)
			{
				Game.Instance.SendLootAllItemsRequest();
			}
			else
			{
				Game.Instance.SendLootBagRequest(SelectedLootBag.ID);
			}
		}
	}

	protected override void Destroy()
	{
		foreach (UILootItem uiLootItem in uiLootItems)
		{
			uiLootItem.Clicked -= OnItemClicked;
		}
		base.Destroy();
	}
}
