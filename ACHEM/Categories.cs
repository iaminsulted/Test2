using System.Collections.Generic;

public class Categories
{
	public enum ItemCategory
	{
		All,
		Equipped,
		Weapon,
		Wearable,
		Armor,
		Helm,
		Shoulders,
		Belt,
		Back,
		Gloves,
		Boots,
		Consumable,
		Item
	}

	public static readonly Dictionary<ItemCategory, string> ItemCategoryIcons = new Dictionary<ItemCategory, string>
	{
		{
			ItemCategory.All,
			"icon-category-bag"
		},
		{
			ItemCategory.Equipped,
			"icon-category-bag"
		},
		{
			ItemCategory.Item,
			"icon-category-bag"
		},
		{
			ItemCategory.Armor,
			"icon-category-chest"
		},
		{
			ItemCategory.Belt,
			"icon-category-bellt"
		},
		{
			ItemCategory.Gloves,
			"icon-category-glove"
		},
		{
			ItemCategory.Boots,
			"icon-category-shoes"
		},
		{
			ItemCategory.Shoulders,
			"icon-category-shoulder"
		},
		{
			ItemCategory.Back,
			"icon-category-cape"
		},
		{
			ItemCategory.Helm,
			"icon-category-helmets"
		},
		{
			ItemCategory.Weapon,
			"icon-category-swords"
		},
		{
			ItemCategory.Consumable,
			"icon-category-potions"
		}
	};

	public static readonly Dictionary<ItemCategory, string> ItemCategoryLabels = new Dictionary<ItemCategory, string>
	{
		{
			ItemCategory.All,
			"All Items"
		},
		{
			ItemCategory.Equipped,
			"Equipped Items"
		},
		{
			ItemCategory.Item,
			"Items"
		},
		{
			ItemCategory.Armor,
			"Armor"
		},
		{
			ItemCategory.Belt,
			"Belts"
		},
		{
			ItemCategory.Gloves,
			"Gloves"
		},
		{
			ItemCategory.Boots,
			"Boots"
		},
		{
			ItemCategory.Shoulders,
			"Shoulders"
		},
		{
			ItemCategory.Back,
			"Capes"
		},
		{
			ItemCategory.Helm,
			"Helms"
		},
		{
			ItemCategory.Weapon,
			"Weapons"
		},
		{
			ItemCategory.Consumable,
			"Consumables"
		}
	};

	public List<InventoryItem> curitems;

	private ItemCategory CurrentSelection;
}
