using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventoryTabs : MonoBehaviour
{
	public string Name;

	public UIInventory.FilterType initialMainCategory;

	private UIInventory.SortType initialSort;

	public Action<UIInventory.FilterType, UIInventory.SortType, UIInventory.FilterType> onFilterButtonClicked;

	public Action<UIInventory.SortType> onSortButtonClicked;

	public Transform WindowBottomLeftCorner;

	public bool initialCategoryIsSub;

	private UIInventory.SortType sortType;

	private UIInventory.FilterType MainCategory;

	private UIInventory.FilterType? SubCategory;

	private List<UIInventoryCategory> inventoryTabs;

	private bool initialized;

	public UIInventory.FilterType CurrentCategory
	{
		get
		{
			if (SubCategory.HasValue)
			{
				return SubCategory.Value;
			}
			return MainCategory;
		}
	}

	public UIInventory.SortType SortType
	{
		get
		{
			return sortType;
		}
		private set
		{
			sortType = value;
		}
	}

	public void Init(List<InventoryItem> iItems)
	{
		if (initialized)
		{
			return;
		}
		inventoryTabs = GetComponentsInChildren<UIInventoryCategory>().ToList();
		InventoryTabsRecord value;
		if (initialCategoryIsSub)
		{
			MainCategory = initialMainCategory;
			SubCategory = initialMainCategory;
			SortType = initialSort;
			Session.MyPlayerData.InventoryTabsRecords[Name] = new InventoryTabsRecord(MainCategory, SubCategory, SortType);
		}
		else if (iItems != null && iItems.Where((InventoryItem x) => x.IsNew).Any())
		{
			MainCategory = initialMainCategory;
			SubCategory = null;
			SortType = initialSort;
			Session.MyPlayerData.InventoryTabsRecords[Name] = new InventoryTabsRecord(MainCategory, SubCategory, SortType);
		}
		else if (Session.MyPlayerData.InventoryTabsRecords.TryGetValue(Name, out value))
		{
			MainCategory = value.MainCategory;
			SortType = value.SortType;
			if (value.IsMainCategory)
			{
				SubCategory = null;
			}
			else
			{
				SubCategory = value.SubCategory;
			}
		}
		else
		{
			MainCategory = initialMainCategory;
			SubCategory = null;
			SortType = initialSort;
			Session.MyPlayerData.InventoryTabsRecords[Name] = new InventoryTabsRecord(MainCategory, SubCategory, SortType);
		}
		foreach (UIInventoryCategory inventoryTab in inventoryTabs)
		{
			inventoryTab.onSortClicked = (Action<UIInventory.SortType>)Delegate.Combine(inventoryTab.onSortClicked, new Action<UIInventory.SortType>(OnSortButtonClicked));
			inventoryTab.onFilterClicked = (Action<UIInventory.FilterType, bool>)Delegate.Combine(inventoryTab.onFilterClicked, new Action<UIInventory.FilterType, bool>(OnFilterButtonClicked));
			inventoryTab.Init(this);
		}
		RefreshFilterPanel();
		initialized = true;
	}

	public void SetSortType(UIInventory.SortType sortType)
	{
		SortType = sortType;
		RefreshFilterPanel();
	}

	public void SetCategoryAndRefreshTabs(UIInventory.FilterType filterType, UIInventory.SortType sortType)
	{
		UIInventory.FilterType? filterType2 = null;
		UIInventory.FilterType? filterType3 = null;
		foreach (UIInventoryCategory inventoryTab in inventoryTabs)
		{
			if (inventoryTab.mainCategory.ItemCategory == filterType)
			{
				filterType2 = filterType;
				break;
			}
		}
		if (!filterType2.HasValue)
		{
			foreach (UIInventoryCategory inventoryTab2 in inventoryTabs)
			{
				foreach (UIInventoryCategory.SubcategoryEntry subCategory in inventoryTab2.subCategories)
				{
					if (subCategory.ItemCategory == filterType)
					{
						filterType2 = inventoryTab2.mainCategory.ItemCategory;
						filterType3 = subCategory.ItemCategory;
						break;
					}
				}
			}
		}
		MainCategory = filterType2.Value;
		SubCategory = filterType3 ?? null;
		RefreshFilterPanel();
	}

	private void RefreshFilterPanel()
	{
		foreach (UIInventoryCategory inventoryTab in inventoryTabs)
		{
			if (inventoryTab.mainCategory.ItemCategory == MainCategory)
			{
				UIInventory.SortType sortType = inventoryTab.Show(SubCategory, SortType);
				if (sortType != SortType)
				{
					SortType = sortType;
				}
			}
			else
			{
				inventoryTab.Hide();
			}
		}
	}

	public void RefreshCategoryCounts(List<int> categoryCounts, HashSet<UIInventory.FilterType> categoriesContainingNew)
	{
		foreach (UIInventoryCategory inventoryTab in inventoryTabs)
		{
			inventoryTab.UpdateCategoryItemCount(categoryCounts, categoriesContainingNew);
		}
	}

	private void UpdateRecord()
	{
		if (Session.MyPlayerData.InventoryTabsRecords.TryGetValue(Name, out var value))
		{
			value.Update(MainCategory, SubCategory, SortType);
		}
		else
		{
			Debug.LogError("Inventory tabs error: Tried to update a non-existent record (invalid key)");
		}
	}

	private void OnSortButtonClicked(UIInventory.SortType sortType)
	{
		SortType = sortType;
		UpdateRecord();
		onSortButtonClicked?.Invoke(sortType);
	}

	private void OnFilterButtonClicked(UIInventory.FilterType filterType, bool isMainButton)
	{
		UIInventory.FilterType currentCategory = CurrentCategory;
		if (isMainButton)
		{
			MainCategory = filterType;
			SubCategory = null;
		}
		else
		{
			SubCategory = filterType;
		}
		RefreshFilterPanel();
		UpdateRecord();
		onFilterButtonClicked?.Invoke(filterType, SortType, currentCategory);
	}
}
