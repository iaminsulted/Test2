using System.Collections.Generic;

public static class Shops
{
	private static Dictionary<int, Shop> map = new Dictionary<int, Shop>();

	private static Dictionary<int, Shop> LootBoxItemShop = new Dictionary<int, Shop>();

	public static void Clear()
	{
		map = new Dictionary<int, Shop>();
		LootBoxItemShop = new Dictionary<int, Shop>();
	}

	public static Shop Get(int ID, ShopType shopType)
	{
		if (shopType == ShopType.Shop && map.ContainsKey(ID))
		{
			return map[ID];
		}
		if (shopType == ShopType.LootBoxItemShop && LootBoxItemShop.ContainsKey(ID))
		{
			return LootBoxItemShop[ID];
		}
		return null;
	}

	public static void Add(Shop item)
	{
		if (item.Type == ShopType.Shop && !map.ContainsKey(item.ID))
		{
			map.Add(item.ID, item);
		}
		if (item.Type == ShopType.LootBoxItemShop && !LootBoxItemShop.ContainsKey(item.ID))
		{
			LootBoxItemShop.Add(item.ID, item);
		}
	}
}
