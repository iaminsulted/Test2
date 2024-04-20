using System;
using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class UIInventory : UIMenuWindow
{
	public enum FilterType
	{
		All,
		Equipped,
		Melee,
		Wearable,
		Armor,
		Helm,
		Shoulders,
		Belt,
		Cape,
		Gloves,
		Boots,
		Consumable,
		AllItems,
		LootBox,
		Usable,
		TravelForm,
		Pet,
		Resources,
		Tokens,
		FishingRod,
		Bow,
		Pistol,
		AllWeapons,
		AllArmor,
		Bag,
		Bobber,
		Fish,
		Crystal,
		Item,
		Moment,
		Furniture,
		Structure,
		WallDecor,
		Lighting,
		FloorDecor,
		Statue,
		Plants,
		Miscellaneous
	}

	public enum SortType
	{
		MostRecentlyAdded,
		Newest,
		Oldest,
		AtoZ,
		ZtoA,
		PowerGreatestToLeast,
		PowerLeastToGreatest
	}

	public class InventoryEntry
	{
		public InventoryItem InvItem;

		public FilterType EquipCategory;

		public InventoryEntry(InventoryItem invItem)
		{
			InvItem = invItem;
		}

		public InventoryEntry()
		{
		}
	}

	public static readonly Dictionary<FilterType, string> FilterLabels = new Dictionary<FilterType, string>
	{
		{
			FilterType.All,
			"Inventory"
		},
		{
			FilterType.Equipped,
			"Equipped Items"
		},
		{
			FilterType.AllItems,
			"All Items"
		},
		{
			FilterType.Armor,
			"Armor"
		},
		{
			FilterType.Belt,
			"Belts"
		},
		{
			FilterType.Gloves,
			"Gloves"
		},
		{
			FilterType.Boots,
			"Boots"
		},
		{
			FilterType.Shoulders,
			"Shoulders"
		},
		{
			FilterType.Cape,
			"Capes"
		},
		{
			FilterType.Helm,
			"Helms"
		},
		{
			FilterType.Melee,
			"Melee Weapons"
		},
		{
			FilterType.Consumable,
			"Consumables"
		},
		{
			FilterType.Usable,
			"Usables"
		},
		{
			FilterType.TravelForm,
			"Travel Forms"
		},
		{
			FilterType.Pet,
			"Pet Collection"
		},
		{
			FilterType.Resources,
			"Resources"
		},
		{
			FilterType.Tokens,
			"Tokens"
		},
		{
			FilterType.Bow,
			"Bows"
		},
		{
			FilterType.Pistol,
			"Pistols"
		},
		{
			FilterType.FishingRod,
			"Fishing Rods"
		},
		{
			FilterType.AllWeapons,
			"All Weapons"
		},
		{
			FilterType.AllArmor,
			"All Armor"
		},
		{
			FilterType.Bag,
			"Bag"
		},
		{
			FilterType.Bobber,
			"Bobbers"
		},
		{
			FilterType.Fish,
			"Fish"
		},
		{
			FilterType.Crystal,
			"Travel Crystals"
		},
		{
			FilterType.Item,
			"Items"
		},
		{
			FilterType.Moment,
			"Moments"
		}
	};

	public static List<FilterType> EquippedFilters = new List<FilterType>
	{
		FilterType.Melee,
		FilterType.Bow,
		FilterType.Pistol,
		FilterType.Helm,
		FilterType.Shoulders,
		FilterType.Armor,
		FilterType.Gloves,
		FilterType.Cape,
		FilterType.Belt,
		FilterType.Boots,
		FilterType.Pet
	};

	public List<InventoryItem> currentItems;

	public List<InventoryEntry> displayedItems = new List<InventoryEntry>();

	public UIInventoryItemDetail uiInventoryItemDetail;

	public GameObject AveragePower;

	public GameObject EquipBest;

	public GameObject GlowEquipBest;

	public UILabel CategorySelectLabel;

	public UILabel InventorySizeLabel;

	public UILabel TitleLabel;

	public UIInput SearchInput;

	public UILabel averagePowerLabel;

	public UILabel recommendedPowerLabel;

	public UIPooledScrollview PooledScrollview;

	public ScrollviewPool<InventoryEntry> itemPool = new ScrollviewPool<InventoryEntry>();

	public UITable PowerTable;

	public UIInventoryTabs inventoryTabs;

	public Action<List<int>> onRefreshItems;

	protected List<InventoryItem> selectedItems = new List<InventoryItem>();

	private string search;

	private EquipItemSlot EquipItemSlot;

	private List<int> categoryCounts = new List<int>();

	private Dictionary<int, HashSet<FilterType>> itemCategories = new Dictionary<int, HashSet<FilterType>>();

	public static UIInventory Instance { get; private set; }

	public int CurrentBank { get; protected set; }

	public static void Load()
	{
		if (Instance == null)
		{
			Instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Inventory2"), UIManager.Instance.transform).GetComponent<UIInventory>();
			Instance.Init();
		}
	}

	private void RefreshItemCategories()
	{
		HashSet<FilterType> hashSet = new HashSet<FilterType>();
		itemCategories.Clear();
		categoryCounts.Clear();
		int num = Enum.GetNames(typeof(FilterType)).Length;
		for (int i = 0; i < num; i++)
		{
			categoryCounts.Add(0);
		}
		foreach (InventoryItem currentItem in currentItems)
		{
			List<FilterType> categoriesForItem = GetCategoriesForItem(currentItem);
			if (currentItem.IsNew)
			{
				hashSet.UnionWith(categoriesForItem);
			}
			foreach (FilterType item in categoriesForItem)
			{
				List<int> list = categoryCounts;
				int index = (int)item;
				int value = list[index] + 1;
				list[index] = value;
			}
			if (itemCategories.TryGetValue(currentItem.CharItemID, out var value2))
			{
				value2.UnionWith(categoriesForItem);
				continue;
			}
			itemCategories[currentItem.CharItemID] = new HashSet<FilterType>();
			itemCategories[currentItem.CharItemID].UnionWith(categoriesForItem);
		}
		if (inventoryTabs != null && !(this is UIBankInventory))
		{
			inventoryTabs.RefreshCategoryCounts(categoryCounts, hashSet);
		}
		onRefreshItems?.Invoke(categoryCounts);
	}

	private static bool IsArmorSlot(EquipItemSlot equip)
	{
		if (equip == EquipItemSlot.Armor || equip == EquipItemSlot.Helm || equip == EquipItemSlot.Shoulders || equip == EquipItemSlot.Belt || equip == EquipItemSlot.Back || equip == EquipItemSlot.Gloves || equip == EquipItemSlot.Boots)
		{
			return true;
		}
		return false;
	}

	public static List<FilterType> GetCategoriesForItem(InventoryItem item)
	{
		List<FilterType> list = new List<FilterType>();
		if (item.Type == ItemType.HouseItem)
		{
			return list;
		}
		if (item.Type == ItemType.Map)
		{
			return list;
		}
		if (item.Type != ItemType.Moment)
		{
			list.Add(FilterType.All);
		}
		if (item.TakesBagSpace)
		{
			list.Add(FilterType.Bag);
		}
		if (item.IsEquipped)
		{
			list.Add(FilterType.Equipped);
		}
		if (item.Type == ItemType.Crystal)
		{
			list.Add(FilterType.Crystal);
			list.Add(FilterType.AllItems);
		}
		if (item.Type == ItemType.Fish)
		{
			list.Add(FilterType.Fish);
		}
		if (item.Type == ItemType.Bobber)
		{
			list.Add(FilterType.Bobber);
		}
		if (ItemSlots.IsWeaponSlot(item.EquipSlot) || ItemSlots.IsToolSlot(item.EquipSlot))
		{
			list.Add(FilterType.AllWeapons);
		}
		if (IsArmorSlot(item.EquipSlot))
		{
			list.Add(FilterType.AllArmor);
		}
		if (item.EquipSlot == EquipItemSlot.Weapon)
		{
			list.Add(FilterType.Melee);
		}
		if (item.EquipSlot == EquipItemSlot.Bow)
		{
			list.Add(FilterType.Bow);
		}
		if (item.EquipSlot == EquipItemSlot.Pistol)
		{
			list.Add(FilterType.Pistol);
		}
		if (item.EquipSlot == EquipItemSlot.FishingRod)
		{
			list.Add(FilterType.FishingRod);
		}
		if (item.EquipSlot == EquipItemSlot.Armor)
		{
			list.Add(FilterType.Armor);
		}
		if (item.EquipSlot == EquipItemSlot.Helm)
		{
			list.Add(FilterType.Helm);
		}
		if (item.EquipSlot == EquipItemSlot.Shoulders)
		{
			list.Add(FilterType.Shoulders);
		}
		if (item.EquipSlot == EquipItemSlot.Belt)
		{
			list.Add(FilterType.Belt);
		}
		if (item.EquipSlot == EquipItemSlot.Back)
		{
			list.Add(FilterType.Cape);
		}
		if (item.EquipSlot == EquipItemSlot.Gloves)
		{
			list.Add(FilterType.Gloves);
		}
		if (item.EquipSlot == EquipItemSlot.Boots)
		{
			list.Add(FilterType.Boots);
		}
		if (item.IsTravelForm)
		{
			list.Add(FilterType.TravelForm);
		}
		if (item.EquipSlot == EquipItemSlot.Pet)
		{
			list.Add(FilterType.Pet);
		}
		if (item.Type == ItemType.Token || item.Type == ItemType.ClassToken || item.Type == ItemType.Moment)
		{
			list.Add(FilterType.Tokens);
		}
		if (item.Type == ItemType.Moment)
		{
			list.Add(FilterType.Moment);
		}
		if (!item.IsTravelForm && item.Type == ItemType.Consumable)
		{
			list.Add(FilterType.Consumable);
		}
		if (item.Type == ItemType.Fish || item.Type == ItemType.Ore || item.Type == ItemType.Bobber || item.Type == ItemType.FishingRod)
		{
			list.Add(FilterType.Resources);
		}
		if (!item.IsTravelForm && item.Type == ItemType.Item)
		{
			list.Add(FilterType.Item);
		}
		if (!item.IsTravelForm && (item.Type == ItemType.Item || item.Type == ItemType.Consumable || item.Type == ItemType.Crystal))
		{
			list.Add(FilterType.AllItems);
		}
		return list;
	}

	public bool IsItemInCategory(InventoryItem item, FilterType category)
	{
		if (itemCategories.TryGetValue(item.CharItemID, out var value) && value.Contains(category))
		{
			return true;
		}
		return false;
	}

	private bool IsInSearch(InventoryItem item)
	{
		if (search == null)
		{
			return true;
		}
		search = search.Trim();
		if (search == string.Empty)
		{
			return true;
		}
		return item.Name.ToLower().Contains(search);
	}

	public static void Toggle()
	{
		if (Instance == null)
		{
			Load();
		}
		else
		{
			Instance.Close();
		}
	}

	public void UpdateSearch()
	{
		search = SearchInput.value.ToLower();
		Refresh();
	}

	public void ClearSearch()
	{
		search = "";
		SearchInput.value = "";
		Refresh();
	}

	protected override void Init()
	{
		base.Init();
		Setup();
	}

	protected virtual void Setup()
	{
		currentItems = Session.MyPlayerData.allItems[CurrentBank].Where((InventoryItem x) => !x.IsHidden && x.Type != ItemType.HouseItem && x.Type != ItemType.Map).ToList();
		if (lblGold != null)
		{
			lblGold.text = Session.MyPlayerData.Gold.ToString("n0");
		}
		if (lblMC != null)
		{
			lblMC.text = Session.MyPlayerData.MC.ToString("n0");
		}
		InventorySizeLabel.text = $"({currentItems.Count((InventoryItem p) => p.TakesBagSpace)} / {Session.MyPlayerData.BagSlots})";
		if (uiInventoryItemDetail != null)
		{
			uiInventoryItemDetail.Visible = false;
		}
		inventoryTabs.Init(currentItems);
		UIInventoryTabs uIInventoryTabs = inventoryTabs;
		uIInventoryTabs.onFilterButtonClicked = (Action<FilterType, SortType, FilterType>)Delegate.Combine(uIInventoryTabs.onFilterButtonClicked, new Action<FilterType, SortType, FilterType>(OnFilterButtonClicked));
		UIInventoryTabs uIInventoryTabs2 = inventoryTabs;
		uIInventoryTabs2.onSortButtonClicked = (Action<SortType>)Delegate.Combine(uIInventoryTabs2.onSortButtonClicked, new Action<SortType>(OnSortButtonClicked));
		itemPool.Init(PooledScrollview, delegate(InventoryEntry inventoryEntry, int index, GameObject uiGO)
		{
			uiGO.SetActive(value: true);
			UIInventoryItem component = uiGO.GetComponent<UIInventoryItem>();
			if (inventoryEntry.InvItem != null)
			{
				component.Selected = selectedItems.Contains(inventoryEntry.InvItem);
				component.Init(inventoryEntry.InvItem);
			}
			else
			{
				component.InitAsEquipSlot(inventoryEntry.EquipCategory);
				component.Selected = false;
			}
		}, null, 10);
		if (AveragePower != null)
		{
			AveragePower.GetComponent<UIDragScrollView>().scrollView = PooledScrollview.scrollView;
			AveragePower.transform.SetParent(PooledScrollview.PoolRoot, worldPositionStays: true);
			AveragePower.transform.localPosition = new Vector3(0f, -38f, 0f);
		}
		Refresh();
		foreach (GameObject poolObject in itemPool.PoolObjects)
		{
			poolObject.GetComponent<UIInventoryItem>().Clicked += OnItemClicked;
			poolObject.GetComponent<UIDragScrollView>().scrollView = PooledScrollview.scrollView;
		}
		AudioManager.Play2DSFX("UI_Bag_Open");
	}

	protected virtual void OnEnable()
	{
		Session.MyPlayerData.ItemAdded += OnItemAdded;
		Session.MyPlayerData.ItemRemoved += OnItemRemoved;
		Session.MyPlayerData.ItemUpdated += OnItemUpdated;
		Session.MyPlayerData.ItemEquipped += OnItemUpdated;
		Session.MyPlayerData.ItemUnequipped += OnItemUpdated;
		Session.MyPlayerData.InventoryReload += OnInventoryReload;
		if (uiInventoryItemDetail != null)
		{
			UIInventoryItemDetail uIInventoryItemDetail = uiInventoryItemDetail;
			uIInventoryItemDetail.onClose = (Action)Delegate.Combine(uIInventoryItemDetail.onClose, new Action(OnDetailClosed));
		}
	}

	protected virtual void OnDisable()
	{
		if (uiInventoryItemDetail != null)
		{
			UIInventoryItemDetail uIInventoryItemDetail = uiInventoryItemDetail;
			uIInventoryItemDetail.onClose = (Action)Delegate.Remove(uIInventoryItemDetail.onClose, new Action(OnDetailClosed));
		}
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.ItemAdded -= OnItemAdded;
			Session.MyPlayerData.ItemRemoved -= OnItemRemoved;
			Session.MyPlayerData.ItemUpdated -= OnItemUpdated;
			Session.MyPlayerData.ItemEquipped -= OnItemUpdated;
			Session.MyPlayerData.ItemUnequipped -= OnItemUpdated;
			Session.MyPlayerData.InventoryReload -= OnInventoryReload;
		}
		foreach (InventoryItem item in Session.MyPlayerData.items)
		{
			item.IsNew = false;
		}
	}

	public override void Close()
	{
		base.Close();
		AudioManager.Play2DSFX("UI_Bag_Close");
	}

	private void OnDetailClosed()
	{
		DeselectAll();
	}

	private void OnItemAdded(InventoryItem iItem)
	{
		Refresh(shouldReset: false);
	}

	private void OnInventoryReload()
	{
		Refresh(shouldReset: false);
	}

	private void OnItemRemoved(InventoryItem inventoryItem)
	{
		Refresh(shouldReset: false);
	}

	private void OnItemUpdated(InventoryItem inventoryItem)
	{
		if (inventoryTabs.CurrentCategory == FilterType.Equipped)
		{
			Refresh();
		}
		else
		{
			if (inventoryItem.BankID != CurrentBank)
			{
				return;
			}
			foreach (GameObject poolObject in itemPool.PoolObjects)
			{
				UIInventoryItem component = poolObject.GetComponent<UIInventoryItem>();
				if (component.Item == inventoryItem)
				{
					component.Init(inventoryItem);
					break;
				}
			}
		}
	}

	public string FindCurrentEquip()
	{
		InventoryItem inventoryItem = currentItems.FirstOrDefault((InventoryItem p) => p.EquipSlot == EquipItemSlot && p.IsStatEquip);
		if (inventoryItem != null)
		{
			return "[b]Currently[/b] " + inventoryItem.Name;
		}
		return "[b]Currently[/b] None";
	}

	public string FindCurrentCosmetic()
	{
		InventoryItem inventoryItem = currentItems.FirstOrDefault((InventoryItem p) => p.EquipSlot == EquipItemSlot && p.IsCosmeticEquip);
		if (inventoryItem != null)
		{
			return "[b]Currently[/b] " + inventoryItem.Name;
		}
		return "[b]Currently[/b] None";
	}

	public void Refresh(bool shouldReset = true)
	{
		RefreshCurrentItems();
		UpdateEquipBest(inventoryTabs.CurrentCategory);
		RefreshItemCategories();
		DisplayCurrentItems();
		if (shouldReset)
		{
			PooledScrollview.scrollView.ResetPosition();
		}
	}

	private void RefreshCurrentItems()
	{
		if (CurrentBank > 0)
		{
			currentItems = Session.MyPlayerData.allItems[CurrentBank].Where((InventoryItem x) => !x.IsHidden && x.TakesBagSpace).ToList();
			return;
		}
		if (CurrentBank == 0)
		{
			currentItems = Session.MyPlayerData.allItems[CurrentBank].Where((InventoryItem x) => !x.IsHidden).ToList();
			return;
		}
		currentItems.Clear();
		if (CurrentBank != -1)
		{
			return;
		}
		foreach (KeyValuePair<int, List<InventoryItem>> allItem in Session.MyPlayerData.allItems)
		{
			if (allItem.Key != 0)
			{
				currentItems.AddRange(allItem.Value.Where((InventoryItem x) => !x.IsHidden).ToList());
			}
		}
	}

	private void UpdateHeaders()
	{
		FilterType currentCategory = inventoryTabs.CurrentCategory;
		string text = FilterLabels[currentCategory];
		int num = categoryCounts[(int)currentCategory];
		if (CurrentBank == 0)
		{
			TitleLabel.text = "Inventory";
		}
		else if (CurrentBank > 0)
		{
			TitleLabel.text = "Bank";
		}
		if (CurrentBank == -1)
		{
			if (currentCategory == FilterType.Bag)
			{
				InventorySizeLabel.text = $"All Vaults Total ({num} / {Session.MyPlayerData.CurrentBankVault * Session.MyPlayerData.BankSlots})";
			}
			else
			{
				InventorySizeLabel.text = $"{text} in All Vaults ({num})";
			}
			return;
		}
		if (CurrentBank > 0)
		{
			if (currentCategory == FilterType.Bag)
			{
				InventorySizeLabel.text = $"Vault {CurrentBank} Total ({num} / {Session.MyPlayerData.BankSlots})";
			}
			else
			{
				InventorySizeLabel.text = $"{text} in Vault {CurrentBank} ({num})";
			}
			return;
		}
		switch (currentCategory)
		{
		case FilterType.Bag:
			InventorySizeLabel.text = $"Bag ({num} / {Session.MyPlayerData.BagSlots})";
			return;
		case FilterType.Equipped:
			InventorySizeLabel.text = $"{text} ({num}), Bag ({categoryCounts[24]} / {Session.MyPlayerData.BagSlots})";
			break;
		}
		if (CategoryTakesBagSpace(currentCategory))
		{
			InventorySizeLabel.text = $"{FilterLabels[currentCategory]} ({categoryCounts[(int)currentCategory]}), Bag ({categoryCounts[24]} / {Session.MyPlayerData.BagSlots})";
		}
		else
		{
			InventorySizeLabel.text = $"{FilterLabels[currentCategory]} ({categoryCounts[(int)currentCategory]})";
		}
	}

	private bool CategoryTakesBagSpace(FilterType category)
	{
		if (category != FilterType.Pet && category != FilterType.TravelForm && category != FilterType.Tokens && category != FilterType.Fish)
		{
			return category != FilterType.Crystal;
		}
		return false;
	}

	private void UpdateEquipBest(FilterType category)
	{
		if ((bool)AveragePower)
		{
			AveragePower.SetActive(value: false);
		}
		if ((bool)EquipBest)
		{
			EquipBest.SetActive(value: false);
		}
		if (category == FilterType.Equipped)
		{
			AveragePower.SetActive(value: true);
			EquipBest.SetActive(value: true);
			AveragePower.transform.SetSiblingIndex(0);
			EquipBest.transform.SetSiblingIndex(1);
			EquipBest.GetComponent<UIButton>().isEnabled = !Session.MyPlayerData.AreBestItemsEquipped();
			GlowEquipBest.SetActive(!Session.MyPlayerData.AreBestItemsEquipped());
			averagePowerLabel.text = "Average Power " + Session.MyPlayerData.AverageItemPower;
			int num = Entities.Instance.me.Level * 20;
			bool flag = Session.MyPlayerData.AverageItemPower < num;
			recommendedPowerLabel.text = "Recommended " + num + "+";
			recommendedPowerLabel.enabled = flag;
			PowerTable.Reposition();
		}
	}

	private void DisplayEquippedItems()
	{
		foreach (FilterType cat in EquippedFilters)
		{
			List<InventoryItem> list = (from item in currentItems
				where item.IsEquipped && IsItemInCategory(item, cat) && IsInSearch(item)
				orderby item.IsStatEquip descending
				select item).ToList();
			if ((list.Count == 0 || (list.All((InventoryItem i) => !i.IsStatEquip) && cat != FilterType.Pet)) && string.IsNullOrEmpty(search))
			{
				InventoryEntry inventoryEntry = new InventoryEntry();
				inventoryEntry.InvItem = null;
				inventoryEntry.EquipCategory = cat;
				displayedItems.Add(inventoryEntry);
			}
			if (!list.Any())
			{
				continue;
			}
			foreach (InventoryItem item in list)
			{
				displayedItems.Add(new InventoryEntry(item));
			}
		}
		Dictionary<int, InventoryEntry> dictionary = displayedItems.Select((InventoryEntry d, int i) => (d: d, i: i)).ToDictionary(((InventoryEntry d, int i) k) => k.i, ((InventoryEntry d, int i) v) => v.d);
		itemPool.SetDataCache(dictionary, dictionary.Count, -80);
	}

	public static IEnumerable<InventoryItem> GetSortedItems(SortType sortType, IEnumerable<InventoryItem> items)
	{
		return sortType switch
		{
			SortType.MostRecentlyAdded => from x in items
				orderby x.TransferID descending, x.CharItemID descending
				select x, 
			SortType.Newest => items.OrderByDescending((InventoryItem x) => x.CharItemID), 
			SortType.Oldest => items.OrderBy((InventoryItem x) => x.CharItemID), 
			SortType.AtoZ => items.OrderBy((InventoryItem x) => x.Name), 
			SortType.ZtoA => items.OrderByDescending((InventoryItem x) => x.Name), 
			SortType.PowerGreatestToLeast => from x in items
				where ItemSlots.IsPowerSlot(x.EquipSlot) && !x.IsCosmetic
				orderby x.GetCombatPower() descending
				select x, 
			SortType.PowerLeastToGreatest => from x in items
				where ItemSlots.IsPowerSlot(x.EquipSlot) && !x.IsCosmetic
				orderby x.GetCombatPower()
				select x, 
			_ => null, 
		};
	}

	private void DisplayCurrentItems()
	{
		CategorySelectLabel.text = FilterLabels[inventoryTabs.CurrentCategory];
		displayedItems.Clear();
		if (inventoryTabs.CurrentCategory == FilterType.Equipped)
		{
			DisplayEquippedItems();
			UpdateHeaders();
			return;
		}
		IEnumerable<InventoryItem> items = currentItems.Where((InventoryItem x) => IsItemInCategory(x, inventoryTabs.CurrentCategory) && IsInSearch(x));
		IEnumerable<InventoryItem> sortedItems = GetSortedItems(inventoryTabs.SortType, items);
		if (sortedItems != null && sortedItems.Any())
		{
			displayedItems = sortedItems.Select((InventoryItem x) => new InventoryEntry(x)).ToList();
		}
		else
		{
			displayedItems = new List<InventoryEntry>();
		}
		Dictionary<int, InventoryEntry> dictionary = displayedItems.Select((InventoryEntry d, int i) => (d: d, i: i)).ToDictionary(((InventoryEntry d, int i) k) => k.i, ((InventoryEntry d, int i) v) => v.d);
		itemPool.SetDataCache(dictionary, dictionary.Count);
		PooledScrollview.scrollView.InvalidateBounds();
		PooledScrollview.scrollView.UpdateScrollbars();
		UpdateHeaders();
	}

	private void OnFilterButtonClicked(FilterType category, SortType sortType, FilterType previousCategory)
	{
		if (category == FilterType.Equipped || previousCategory == FilterType.Equipped)
		{
			Refresh();
		}
		DisplayCurrentItems();
		PooledScrollview.scrollView.ResetPosition();
	}

	private void OnSortButtonClicked(SortType type)
	{
		PooledScrollview.scrollView.ResetPosition();
		DisplayCurrentItems();
	}

	private void OnItemClicked(UIItem selectedItem)
	{
		UIInventoryItem uIInventoryItem = selectedItem as UIInventoryItem;
		if (uIInventoryItem.Item == null)
		{
			inventoryTabs.SetCategoryAndRefreshTabs(uIInventoryItem.FilterType, SortType.Newest);
			Refresh();
		}
		else if (selectedItems.Contains(selectedItem.Item as InventoryItem))
		{
			if (this is UIBankInventory && selectedItem.Selected && selectedItem.Item.HasPreview)
			{
				UIPreview.Show(selectedItem.Item);
			}
			else if (uiInventoryItemDetail != null && uiInventoryItemDetail.Visible && selectedItem.Item.HasPreview)
			{
				UIPreview.Show(selectedItem.Item);
			}
		}
		else
		{
			if (this is UIBankInventory)
			{
				UIBankManager.Instance.DeselectAll();
			}
			SetSelectedItem(selectedItem as UIInventoryItem);
		}
	}

	public void DeselectAll()
	{
		selectedItems.Clear();
		foreach (GameObject poolObject in itemPool.PoolObjects)
		{
			poolObject.GetComponent<UIInventoryItem>().Selected = false;
		}
	}

	protected virtual void SetSelectedItem(UIInventoryItem selectedItem)
	{
		DeselectAll();
		selectedItems.Clear();
		InventoryItem item = selectedItem.Item as InventoryItem;
		selectedItems.Add(item);
		selectedItem.Selected = true;
		if (uiInventoryItemDetail != null)
		{
			uiInventoryItemDetail.LoadInventoryItem((InventoryItem)selectedItem.Item);
			if (selectedItem.Item.IsNew)
			{
				selectedItem.Item.IsNew = false;
			}
			selectedItem.IconNew.gameObject.SetActive(value: false);
		}
	}

	protected override void Destroy()
	{
		base.Destroy();
		UIInventoryTabs uIInventoryTabs = inventoryTabs;
		uIInventoryTabs.onFilterButtonClicked = (Action<FilterType, SortType, FilterType>)Delegate.Remove(uIInventoryTabs.onFilterButtonClicked, new Action<FilterType, SortType, FilterType>(OnFilterButtonClicked));
		UIInventoryTabs uIInventoryTabs2 = inventoryTabs;
		uIInventoryTabs2.onSortButtonClicked = (Action<SortType>)Delegate.Remove(uIInventoryTabs2.onSortButtonClicked, new Action<SortType>(OnSortButtonClicked));
		Instance = null;
	}

	protected override void Resume()
	{
		base.Resume();
		Refresh();
	}

	public void SendEquipBest(GameObject go)
	{
		Game.Instance.SendEquipBestRequest();
		Refresh();
	}
}
