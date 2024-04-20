using System.Collections.Generic;

public static class DailyRewards
{
	public static Dictionary<int, Item> map;

	public static int Count => map.Count;

	public static bool IsEmpty => map.Count == 0;

	static DailyRewards()
	{
		map = new Dictionary<int, Item>();
	}

	public static void Clear()
	{
		map.Clear();
	}

	public static void Add(int day, Item item)
	{
		if (!map.ContainsKey(day))
		{
			map.Add(day, item);
		}
	}

	public static bool HasKey(int day)
	{
		return map.ContainsKey(day);
	}

	public static Item Get(int day)
	{
		if (map.ContainsKey(day))
		{
			return map[day];
		}
		return null;
	}

	public static void Init(Dictionary<int, Item> map)
	{
		DailyRewards.map = map;
	}

	public static string GetChestIcon(int day)
	{
		if (!map.ContainsKey(day))
		{
			return "";
		}
		return map[day].ID switch
		{
			1200 => "CommonChestImage", 
			1362 => "RareChestImage", 
			1363 => "EpicChestImage", 
			_ => "", 
		};
	}
}
