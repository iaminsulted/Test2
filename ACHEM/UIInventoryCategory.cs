using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventoryCategory : MonoBehaviour
{
	[Serializable]
	public class SubcategoryEntry
	{
		public UIInventory.FilterType ItemCategory;

		public UIInventoryCategoryButton ItemCategoryButton;
	}

	public SubcategoryEntry mainCategory;

	public List<SubcategoryEntry> subCategories;

	public List<UISortButton> sortButtons;

	public UILabel label;

	public GameObject mainGlow;

	public GameObject glow;

	public GameObject FilterSortPanel;

	public GameObject arrow;

	public Material grayOutMaterial;

	public UIButton ShowHideSortsBtn;

	public UIGrid SortGrid;

	public UISprite SortMenuButtonHighlight;

	public UIWidget Top;

	public UIWidget Bottom;

	public Action<UIInventory.FilterType, bool> onFilterClicked;

	public Action<UIInventory.SortType> onSortClicked;

	private float initialFilterSortPanelYPosition;

	private UIInventoryTabs inventoryTabs;

	private bool initialized;

	private bool isSortsShowing;

	private List<UIEventListener.VoidDelegate> subscribedFuncs = new List<UIEventListener.VoidDelegate>();

	private bool isSortButtonsEnabled;

	public void Init(UIInventoryTabs inventoryTabs)
	{
		if (initialized)
		{
			return;
		}
		this.inventoryTabs = inventoryTabs;
		subscribedFuncs.Clear();
		if (subCategories != null)
		{
			for (int i = 0; i < subCategories.Count; i++)
			{
				int categoryIndex = i;
				UIEventListener.VoidDelegate voidDelegate = delegate
				{
					OnSubCategoryClicked(categoryIndex);
				};
				subscribedFuncs.Add(voidDelegate);
				UIEventListener uIEventListener = UIEventListener.Get(subCategories[i].ItemCategoryButton.Button.gameObject);
				uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, voidDelegate);
			}
		}
		UIEventListener uIEventListener2 = UIEventListener.Get(mainCategory.ItemCategoryButton.Button.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnMainCategoryClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(ShowHideSortsBtn.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(onShowHideSortsClicked));
		foreach (UISortButton sortButton in sortButtons)
		{
			sortButton.onResetAll = (Action)Delegate.Combine(sortButton.onResetAll, new Action(OnResetAllSortButtons));
			sortButton.onClick = (Action<UIInventory.SortType>)Delegate.Combine(sortButton.onClick, new Action<UIInventory.SortType>(OnSortButtonClicked));
			sortButton.Init();
		}
		initialFilterSortPanelYPosition = FilterSortPanel.transform.position.y;
	}

	private void UpdateNewIcon(UIButton btn, bool isNew)
	{
	}

	private void UpdateLabel(UILabel label, bool isGrayedOut)
	{
		if (isGrayedOut)
		{
			label.color = Color.gray;
		}
		else
		{
			label.color = Color.white;
		}
	}

	private void UpdateColor(UIInventoryCategoryButton btn, bool isGrayedOut)
	{
		if (isGrayedOut)
		{
			btn.Texture.material = grayOutMaterial;
		}
		else
		{
			btn.Texture.material = null;
		}
	}

	public void UpdateCategoryItemCount(List<int> itemCategoryCounts, HashSet<UIInventory.FilterType> categoriesContainingNew)
	{
		UpdateColor(mainCategory.ItemCategoryButton, itemCategoryCounts[(int)mainCategory.ItemCategory] == 0);
		UpdateLabel(label, itemCategoryCounts[(int)mainCategory.ItemCategory] == 0);
		for (int i = 0; i < subCategories.Count; i++)
		{
			UpdateColor(subCategories[i].ItemCategoryButton, itemCategoryCounts[(int)subCategories[i].ItemCategory] == 0);
		}
	}

	private void OnMainCategoryClicked(GameObject go)
	{
		glow.SetActive(value: false);
		mainGlow.SetActive(value: true);
		onFilterClicked?.Invoke(mainCategory.ItemCategory, arg2: true);
		FilterSortPanel.SetActive(value: true);
	}

	public void OnSubCategoryClicked(int categoryIndex)
	{
		glow.transform.SetParent(subCategories[categoryIndex].ItemCategoryButton.Button.transform, worldPositionStays: false);
		glow.SetActive(value: true);
		mainGlow.SetActive(value: false);
		onFilterClicked?.Invoke(subCategories[categoryIndex].ItemCategory, arg2: false);
	}

	public UIInventory.SortType Show(UIInventory.FilterType? subCategory, UIInventory.SortType sortType)
	{
		if (mainCategory.ItemCategory != UIInventory.FilterType.Equipped)
		{
			arrow.SetActive(value: true);
		}
		if (!subCategory.HasValue)
		{
			mainGlow.SetActive(value: true);
		}
		else
		{
			mainGlow.SetActive(value: false);
			for (int i = 0; i < subCategories.Count; i++)
			{
				if (subCategories[i].ItemCategory == subCategory.Value)
				{
					glow.transform.SetParent(subCategories[i].ItemCategoryButton.Button.transform, worldPositionStays: false);
					glow.SetActive(value: true);
					break;
				}
			}
		}
		bool flag = false;
		foreach (UISortButton sortButton in sortButtons)
		{
			for (int j = 0; j < sortButton.entries.Count; j++)
			{
				if (sortButton.entries[j].SortType == sortType)
				{
					sortButton.SetButtonState(j + 1);
					flag = true;
					break;
				}
				sortButton.SetButtonState(0);
			}
		}
		if (flag)
		{
			EnableSortButtons();
			FilterSortPanel.SetActive(value: true);
			return sortType;
		}
		DisableSortButtons();
		FilterSortPanel.SetActive(value: true);
		return UIInventory.SortType.MostRecentlyAdded;
	}

	private void EnableSortButtons()
	{
		if (isSortsShowing)
		{
			return;
		}
		isSortsShowing = true;
		SortMenuButtonHighlight.gameObject.SetActive(value: true);
		foreach (UISortButton sortButton in sortButtons)
		{
			sortButton.gameObject.SetActive(value: true);
		}
		SortGrid.Reposition();
		if (inventoryTabs.WindowBottomLeftCorner != null)
		{
			float num = inventoryTabs.WindowBottomLeftCorner.position.y - Bottom.worldCorners[1].y;
			if (num > 0f)
			{
				FilterSortPanel.transform.position = new Vector3(FilterSortPanel.transform.position.x, initialFilterSortPanelYPosition + num, FilterSortPanel.transform.position.z);
			}
		}
	}

	private void DisableSortButtons()
	{
		if (!isSortsShowing)
		{
			return;
		}
		isSortsShowing = false;
		SortMenuButtonHighlight.gameObject.SetActive(value: false);
		foreach (UISortButton sortButton in sortButtons)
		{
			sortButton.SetButtonState(0);
			sortButton.gameObject.SetActive(value: false);
		}
		SortGrid.Reposition();
		FilterSortPanel.transform.position = new Vector3(FilterSortPanel.transform.position.x, initialFilterSortPanelYPosition, FilterSortPanel.transform.position.z);
	}

	private UIInventory.SortType SelectFirstSortButton()
	{
		UISortButton uISortButton = sortButtons.First();
		uISortButton.SetButtonState(1);
		return uISortButton.entries[0].SortType;
	}

	private void onShowHideSortsClicked(GameObject go)
	{
		if (!isSortsShowing)
		{
			EnableSortButtons();
			onSortClicked?.Invoke(SelectFirstSortButton());
		}
		else
		{
			DisableSortButtons();
			onSortClicked?.Invoke(UIInventory.SortType.MostRecentlyAdded);
		}
	}

	public void Hide()
	{
		mainGlow.SetActive(value: false);
		FilterSortPanel.SetActive(value: false);
		arrow.SetActive(value: false);
	}

	private void OnResetAllSortButtons()
	{
		foreach (UISortButton sortButton in sortButtons)
		{
			sortButton.ResetButtonState();
		}
	}

	private void OnSortButtonClicked(UIInventory.SortType sortingType)
	{
		onSortClicked?.Invoke(sortingType);
	}

	private void OnDestroy()
	{
		UIEventListener uIEventListener = UIEventListener.Get(mainCategory.ItemCategoryButton.Button.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnMainCategoryClicked));
		if (subscribedFuncs.Count > 0)
		{
			for (int i = 0; i < subCategories.Count; i++)
			{
				UIEventListener uIEventListener2 = UIEventListener.Get(subCategories[i].ItemCategoryButton.Button.gameObject);
				uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, subscribedFuncs[i]);
			}
		}
		subscribedFuncs.Clear();
		foreach (UISortButton sortButton in sortButtons)
		{
			sortButton.onClick = (Action<UIInventory.SortType>)Delegate.Remove(sortButton.onClick, new Action<UIInventory.SortType>(OnSortButtonClicked));
			sortButton.onResetAll = (Action)Delegate.Remove(sortButton.onResetAll, new Action(OnResetAllSortButtons));
		}
	}
}
