using System;
using System.Collections.Generic;

namespace StatCurves
{
	// Token: 0x02000014 RID: 20
	public static class TradeSkill
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00004650 File Offset: 0x00002850
		static TradeSkill()
		{
			TradeSkill.xpToLevels[1] = 100;
			TradeSkill.xpToLevels[2] = 350;
			TradeSkill.xpToLevels[3] = 625;
			TradeSkill.xpToLevels[4] = 1000;
			for (int i = 5; i <= 50; i++)
			{
				TradeSkill.xpToLevels[i] = (10 + i * 2) * 70;
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000476C File Offset: 0x0000296C
		public static float GetMaximumFishingDelay(int playerLevel, int nodeLevel, RarityType itemRarity)
		{
			float num = 24f * TradeSkill.fishingRates[itemRarity] * (1f - TradeSkill.DiminishRate(playerLevel, nodeLevel));
			bool flag = num < 12f;
			if (flag)
			{
				num = 12f;
			}
			return num;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000047B4 File Offset: 0x000029B4
		public static float GetMinimumFishingDelay(int playerLevel, int nodeLevel, RarityType itemRarity)
		{
			float num = 14f * TradeSkill.fishingRates[itemRarity] * (1f - TradeSkill.DiminishRate(playerLevel, nodeLevel));
			bool flag = num < 7f;
			if (flag)
			{
				num = 7f;
			}
			return num;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000047FC File Offset: 0x000029FC
		public static int GetNodeXP(int tradeSkillLevel, RarityType itemRarity)
		{
			return (int)Math.Ceiling((double)((float)(10 + tradeSkillLevel * 2) * TradeSkill.xpMultipliers[itemRarity]));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004828 File Offset: 0x00002A28
		public static int GetXPToLevel(int playerLevel)
		{
			return TradeSkill.xpToLevels[playerLevel];
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004848 File Offset: 0x00002A48
		private static float DiminishRate(int playerLevel, int tradeSkillLevel)
		{
			int num = playerLevel - tradeSkillLevel;
			bool flag = num < 10;
			if (flag)
			{
				num = 0;
			}
			else
			{
				bool flag2 = num > 25;
				if (flag2)
				{
					num = 25;
				}
			}
			float num2 = (float)num / 25f;
			bool flag3 = num2 > 0.8f;
			if (flag3)
			{
				num2 = 0.8f;
			}
			return num2;
		}

		// Token: 0x0400008D RID: 141
		private const int BaseXP = 10;

		// Token: 0x0400008E RID: 142
		private const int DiminishLevelMin = 10;

		// Token: 0x0400008F RID: 143
		private const int DiminishLevelMax = 25;

		// Token: 0x04000090 RID: 144
		private const int UsagesToLevel = 70;

		// Token: 0x04000091 RID: 145
		private const float FishingDelayMin = 7f;

		// Token: 0x04000092 RID: 146
		private const float FishingDelayMinSoft = 14f;

		// Token: 0x04000093 RID: 147
		private const float FishingDelayMax = 12f;

		// Token: 0x04000094 RID: 148
		private const float FishingDelayMaxSoft = 24f;

		// Token: 0x04000095 RID: 149
		private static Dictionary<int, int> xpToLevels = new Dictionary<int, int>();

		// Token: 0x04000096 RID: 150
		private static Dictionary<RarityType, float> xpMultipliers = new Dictionary<RarityType, float>
		{
			{
				RarityType.Common,
				1f
			},
			{
				RarityType.Uncommon,
				1.32f
			},
			{
				RarityType.Rare,
				1.84f
			},
			{
				RarityType.Epic,
				2.58f
			},
			{
				RarityType.Legendary,
				3.62f
			}
		};

		// Token: 0x04000097 RID: 151
		private static Dictionary<RarityType, float> fishingRates = new Dictionary<RarityType, float>
		{
			{
				RarityType.Common,
				1f
			},
			{
				RarityType.Uncommon,
				0.94f
			},
			{
				RarityType.Rare,
				0.88f
			},
			{
				RarityType.Epic,
				0.81f
			},
			{
				RarityType.Legendary,
				0.72f
			}
		};

		// Token: 0x04000098 RID: 152
		public const float NodeRateCommon = 0.702f;

		// Token: 0x04000099 RID: 153
		public const float NodeRateUncommon = 0.156f;

		// Token: 0x0400009A RID: 154
		public const float NodeRateRare = 0.085f;

		// Token: 0x0400009B RID: 155
		public const float NodeRateEpic = 0.044f;

		// Token: 0x0400009C RID: 156
		public const float NodeRateLegendary = 0.013f;

		// Token: 0x0400009D RID: 157
		public const int MaxLevel = 50;
	}
}
