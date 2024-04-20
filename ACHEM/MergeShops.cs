using System.Collections.Generic;

public static class MergeShops
{
	public static Dictionary<int, MergeShop> map = new Dictionary<int, MergeShop>();

	public static void Add(MergeShop shop)
	{
		if (Get(shop.MergeShopID) == null)
		{
			map[shop.MergeShopID] = shop;
		}
	}

	public static void Clear()
	{
		map = new Dictionary<int, MergeShop>();
	}

	public static MergeShop Get(int ID)
	{
		if (map.ContainsKey(ID))
		{
			return map[ID];
		}
		return null;
	}
}
