using System;
using System.Collections.Generic;
using System.Linq;

namespace StatCurves
{
	// Token: 0x0200000B RID: 11
	public class ItemStatWeight
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003AF9 File Offset: 0x00001CF9
		private float attack { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003B01 File Offset: 0x00001D01
		private float maxHealth { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003B09 File Offset: 0x00001D09
		private float armor { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003B11 File Offset: 0x00001D11
		private float crit { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003B19 File Offset: 0x00001D19
		private float evasion { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003B21 File Offset: 0x00001D21
		private float haste { get; }

		// Token: 0x0600005A RID: 90 RVA: 0x00003B2C File Offset: 0x00001D2C
		private ItemStatWeight()
		{
			this.attack = 0f;
			this.maxHealth = 0f;
			this.armor = 0f;
			this.crit = 0f;
			this.evasion = 0f;
			this.haste = 0f;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003B83 File Offset: 0x00001D83
		private ItemStatWeight(float maxHealth, float attack, float armor, float evasion, float crit, float haste)
		{
			this.maxHealth = maxHealth;
			this.attack = attack;
			this.armor = armor;
			this.evasion = evasion;
			this.crit = crit;
			this.haste = haste;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003BBC File Offset: 0x00001DBC
		public static ItemStatWeight GetStatWeights(EquipItemSlot equipSlot)
		{
			bool flag = ItemStatWeight.AllWeights.ContainsKey(equipSlot);
			ItemStatWeight result;
			if (flag)
			{
				result = ItemStatWeight.AllWeights[equipSlot];
			}
			else
			{
				result = new ItemStatWeight();
			}
			return result;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003BF4 File Offset: 0x00001DF4
		public float GetMinStatPercentByType(Stat stat)
		{
			bool flag = Stats.IsStatPrimary(stat);
			List<float> source;
			if (flag)
			{
				source = new List<float>
				{
					this.maxHealth,
					this.attack
				};
			}
			else
			{
				source = new List<float>
				{
					this.armor,
					this.crit,
					this.evasion,
					this.haste
				};
			}
			return (from value in source
			where value > 0f
			select value).DefaultIfEmpty(0f).Min();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003CA4 File Offset: 0x00001EA4
		public float GetWeightByStat(Stat stat)
		{
			switch (stat)
			{
			case Stat.MaxHealth:
				return this.maxHealth;
			case Stat.Attack:
				return this.attack;
			case Stat.Armor:
				return this.armor;
			case Stat.Evasion:
				return this.evasion;
			case Stat.Crit:
				return this.crit;
			case Stat.Haste:
				return this.haste;
			}
			return 0f;
		}

		// Token: 0x04000044 RID: 68
		private static readonly Dictionary<EquipItemSlot, ItemStatWeight> AllWeights = new Dictionary<EquipItemSlot, ItemStatWeight>
		{
			{
				EquipItemSlot.Armor,
				new ItemStatWeight(0.45f, 0.2f, 0.35f, 0.15f, 0.15f, 0f)
			},
			{
				EquipItemSlot.Belt,
				new ItemStatWeight(0f, 0f, 0.15f, 0.15f, 0f, 0.15f)
			},
			{
				EquipItemSlot.Gloves,
				new ItemStatWeight(0.1f, 0.1f, 0f, 0f, 0.1f, 0.3f)
			},
			{
				EquipItemSlot.Boots,
				new ItemStatWeight(0f, 0f, 0.15f, 0.35f, 0f, 0.15f)
			},
			{
				EquipItemSlot.Shoulders,
				new ItemStatWeight(0.05f, 0.1f, 0.15f, 0f, 0.3f, 0f)
			},
			{
				EquipItemSlot.Back,
				new ItemStatWeight(0f, 0f, 0f, 0.2f, 0.2f, 0.2f)
			},
			{
				EquipItemSlot.Helm,
				new ItemStatWeight(0.15f, 0.1f, 0.2f, 0.15f, 0f, 0f)
			},
			{
				EquipItemSlot.Weapon,
				new ItemStatWeight(0.25f, 0.5f, 0f, 0f, 0.25f, 0.2f)
			},
			{
				EquipItemSlot.Pistol,
				new ItemStatWeight(0.25f, 0.5f, 0f, 0f, 0.25f, 0.2f)
			},
			{
				EquipItemSlot.Bow,
				new ItemStatWeight(0.25f, 0.5f, 0f, 0f, 0.25f, 0.2f)
			}
		};
	}
}
