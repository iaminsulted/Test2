using System.Collections.Generic;

public static class Items
{
	private static Dictionary<int, Item> map;

	static Items()
	{
		map = new Dictionary<int, Item>();
	}

	public static void Clear()
	{
		map = new Dictionary<int, Item>();
	}

	public static void Add(Item item)
	{
		if (!map.ContainsKey(item.ID))
		{
			map.Add(item.ID, item);
		}
	}

	public static Item Get(int ID)
	{
		if (map.ContainsKey(ID))
		{
			return map[ID];
		}
		return null;
	}

	public static void Add(List<Item> items)
	{
		foreach (Item item in items)
		{
			if (!map.ContainsKey(item.ID))
			{
				map.Add(item.ID, item);
			}
		}
	}
}
