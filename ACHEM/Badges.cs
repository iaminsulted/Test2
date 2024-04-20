using System;
using System.Collections.Generic;

public class Badges
{
	public static Dictionary<int, Badge> map;

	public static Dictionary<int, BadgeTitleCategory> Categories;

	public static bool IsLoaded;

	public static event Action BadgesLoaded;

	static Badges()
	{
		map = new Dictionary<int, Badge>();
		Categories = new Dictionary<int, BadgeTitleCategory>();
	}

	public static void Init(Dictionary<int, Badge> badges, Dictionary<int, BadgeTitleCategory> categories)
	{
		foreach (Badge value in badges.Values)
		{
			if (!map.ContainsKey(value.ID))
			{
				map.Add(value.ID, value);
			}
		}
		foreach (BadgeTitleCategory value2 in categories.Values)
		{
			if (!Categories.ContainsKey(value2.ID))
			{
				Categories.Add(value2.ID, value2);
			}
		}
		IsLoaded = true;
		Badges.BadgesLoaded?.Invoke();
	}

	public static void Clear()
	{
		map = new Dictionary<int, Badge>();
	}

	public static void Add(Badge badge)
	{
		if (!map.ContainsKey(badge.ID))
		{
			map.Add(badge.ID, badge);
		}
	}

	public static void Add(List<Badge> badges)
	{
		foreach (Badge badge in badges)
		{
			if (!map.ContainsKey(badge.ID))
			{
				map.Add(badge.ID, badge);
			}
		}
	}

	public static bool HasKey(int ID)
	{
		return map.ContainsKey(ID);
	}

	public static Badge Get(int ID)
	{
		if (map.ContainsKey(ID))
		{
			return map[ID];
		}
		return null;
	}
}
