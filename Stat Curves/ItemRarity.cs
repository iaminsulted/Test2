using System;
using System.Collections.Generic;
using System.Linq;

namespace StatCurves
{
	// Token: 0x02000008 RID: 8
	public class ItemRarity
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000388F File Offset: 0x00001A8F
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00003897 File Offset: 0x00001A97
		public int rarity { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000038A0 File Offset: 0x00001AA0
		// (set) Token: 0x06000048 RID: 72 RVA: 0x000038A8 File Offset: 0x00001AA8
		public string name { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000038B1 File Offset: 0x00001AB1
		// (set) Token: 0x0600004A RID: 74 RVA: 0x000038B9 File Offset: 0x00001AB9
		public float levelDiff { get; private set; }

		// Token: 0x0600004B RID: 75 RVA: 0x000038C2 File Offset: 0x00001AC2
		private ItemRarity(int rarity, string name, float levelDiff)
		{
			this.rarity = rarity;
			this.name = name;
			this.levelDiff = levelDiff;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000038E4 File Offset: 0x00001AE4
		public static ItemRarity GetItemRarity(RarityType rarityType)
		{
			return ItemRarity.GetItemRarity((int)rarityType);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000038FC File Offset: 0x00001AFC
		public static ItemRarity GetItemRarity(int rarity)
		{
			return ItemRarity.allRarities.FirstOrDefault((ItemRarity r) => r.rarity == rarity);
		}

		// Token: 0x0400002E RID: 46
		private static List<ItemRarity> allRarities = new List<ItemRarity>
		{
			new ItemRarity(0, RarityType.Junk.ToString(), -2f),
			new ItemRarity(1, RarityType.Common.ToString(), -1f),
			new ItemRarity(2, RarityType.Uncommon.ToString(), 0f),
			new ItemRarity(3, RarityType.Rare.ToString(), 1f),
			new ItemRarity(4, RarityType.Epic.ToString(), 2f),
			new ItemRarity(5, RarityType.Legendary.ToString(), 3f)
		};
	}
}
