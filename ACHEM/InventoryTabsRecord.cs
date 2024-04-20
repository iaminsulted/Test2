public class InventoryTabsRecord
{
	public UIInventory.FilterType MainCategory { get; private set; }

	public UIInventory.FilterType SubCategory { get; private set; }

	public UIInventory.SortType SortType { get; private set; }

	public bool IsMainCategory { get; private set; }

	public InventoryTabsRecord(UIInventory.FilterType mainCategory, UIInventory.FilterType? subCategory, UIInventory.SortType sortType)
	{
		Update(mainCategory, subCategory, sortType);
	}

	public void Update(UIInventory.FilterType mainCategory, UIInventory.FilterType? subCategory, UIInventory.SortType sortType)
	{
		MainCategory = mainCategory;
		SortType = sortType;
		if (subCategory.HasValue)
		{
			SubCategory = subCategory.Value;
			IsMainCategory = false;
		}
		else
		{
			MainCategory = mainCategory;
			IsMainCategory = true;
		}
	}
}
