using System;
using System.Collections.Generic;

public static class LootBags
{
	public static Dictionary<int, ComLoot> Bags = new Dictionary<int, ComLoot>();

	public static event Action<int> BagAdded;

	public static event Action<int> BagRemoved;

	public static event Action<int, int> LootItemRemoved;

	public static event Action Cleared;

	public static void AddLoot(ComLoot loot)
	{
		Bags.Add(loot.ID, loot);
		LootBags.BagAdded?.Invoke(loot.ID);
	}

	public static void RemoveItem(int lootBagID, int lootItemID, int itemID)
	{
		if (Bags.ContainsKey(lootBagID))
		{
			Bags[lootBagID].Items.RemoveAll((LootItem p) => p.LootItemID == lootItemID);
			LootBags.LootItemRemoved?.Invoke(lootBagID, lootItemID);
			Session.MyPlayerData.AutoEquipPotion(lootBagID, itemID);
			if (Bags[lootBagID].Items.Count == 0)
			{
				RemoveLoot(lootBagID);
			}
		}
	}

	public static void RemoveLoot(int lootID)
	{
		if (Bags.ContainsKey(lootID))
		{
			Bags.Remove(lootID);
			LootBags.BagRemoved?.Invoke(lootID);
		}
	}

	public static void Clear()
	{
		Bags.Clear();
		LootBags.Cleared?.Invoke();
	}

	public static int Count()
	{
		return Bags.Count;
	}
}
