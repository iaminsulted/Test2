using System;
using System.Collections.Generic;
using System.Linq;

public static class Wars
{
	private static Dictionary<int, War> map;

	public static List<War> All => map.Values.ToList();

	public static bool IsActive => map.Count > 0;

	public static event Action WarsUpdated;

	static Wars()
	{
		map = new Dictionary<int, War>();
	}

	public static void Clear()
	{
		map = new Dictionary<int, War>();
	}

	public static void Set(List<War> wars)
	{
		if (wars == null)
		{
			map.Clear();
		}
		else
		{
			foreach (War war in wars)
			{
				int warID = war.WarID;
				if (!map.ContainsKey(warID))
				{
					map.Add(warID, war);
				}
				else
				{
					map[warID] = war;
				}
			}
		}
		if (Wars.WarsUpdated != null)
		{
			Wars.WarsUpdated();
		}
	}

	public static bool HasKey(int id)
	{
		return map.ContainsKey(id);
	}

	public static War Get(int id)
	{
		if (map.ContainsKey(id))
		{
			return map[id];
		}
		return null;
	}
}
