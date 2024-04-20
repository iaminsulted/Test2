using System.Collections.Generic;

public static class Dungeons
{
	private static Dictionary<int, DungeonData> map = new Dictionary<int, DungeonData>();

	public static DungeonData Get(int ID)
	{
		if (map.ContainsKey(ID))
		{
			return map[ID];
		}
		return null;
	}

	public static List<DungeonData> GetList(List<int> ids)
	{
		List<DungeonData> list = new List<DungeonData>();
		foreach (int id in ids)
		{
			DungeonData dungeonData = Get(id);
			if (dungeonData != null)
			{
				list.Add(dungeonData);
			}
		}
		return list;
	}

	public static void Add(DungeonData dd)
	{
		if (!map.ContainsKey(dd.map.ID))
		{
			map.Add(dd.map.ID, dd);
		}
	}

	public static void Clear()
	{
		map.Clear();
	}
}
