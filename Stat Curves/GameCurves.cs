using System;
using System.Collections.Generic;

namespace StatCurves
{
	// Token: 0x02000003 RID: 3
	public static class GameCurves
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002610 File Offset: 0x00000810
		public static float GetStatCurve(float level)
		{
			bool flag = level > 100f;
			if (flag)
			{
				level = 100f;
			}
			return (float)(200.0 * Math.Pow(1.0850000381469727, (double)(level - 1f)));
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002658 File Offset: 0x00000858
		public static float GetNPCStatCurve(float level)
		{
			return (float)(200.0 * Math.Pow(1.0850000381469727, (double)(level - 1f - 3f)));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002694 File Offset: 0x00000894
		public static float GetBonusStatFromLevel(float level)
		{
			bool flag = level >= 10f;
			float result;
			if (flag)
			{
				result = 0f;
			}
			else
			{
				float num = 0.5f - 0.05f * level;
				result = GameCurves.GetExpectedStatFromLevel(level) * num;
			}
			return result;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000026D4 File Offset: 0x000008D4
		public static float GetExpectedStatFromLevel(float level)
		{
			return GameCurves.GetStatCurve(level) * 0.5f;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000026F4 File Offset: 0x000008F4
		public static float GetExpectedStatFromGear(float level)
		{
			return GameCurves.GetStatCurve(level) * 0.5f;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002714 File Offset: 0x00000914
		public static float GetStatWeight(Stat stat)
		{
			float result = 1f;
			bool flag = GameCurves.Stat_Weights.ContainsKey(stat);
			if (flag)
			{
				result = GameCurves.Stat_Weights[stat];
			}
			return result;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000274C File Offset: 0x0000094C
		public static float GetExtraStatWeight(Stat stat)
		{
			float result = 1f;
			bool flag = GameCurves.Extra_Stat_Weights.ContainsKey(stat);
			if (flag)
			{
				result = GameCurves.Extra_Stat_Weights[stat];
			}
			return result;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002784 File Offset: 0x00000984
		public static float GetDerivedStartingValue(Stat baseStat)
		{
			float result = 0f;
			bool flag = GameCurves.Derived_Stat_Start_Values.ContainsKey(baseStat);
			if (flag)
			{
				result = GameCurves.Derived_Stat_Start_Values[baseStat];
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000027BC File Offset: 0x000009BC
		public static float GetRealItemStatValue(Stat stat, float itemStatDiff, EquipItemSlot equipSlot, float effectiveLevel)
		{
			ItemStatWeight statWeights = ItemStatWeight.GetStatWeights(equipSlot);
			float num = statWeights.GetMinStatPercentByType(stat) * 0.25f;
			float level = effectiveLevel;
			float num2 = statWeights.GetWeightByStat(stat) * GameCurves.GetExpectedStatFromGear(level);
			float num3 = num * GameCurves.GetExpectedStatFromGear(level) * itemStatDiff;
			return (float)Math.Ceiling((double)(num2 + num3));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002810 File Offset: 0x00000A10
		public static float GetDynamicScaleMultiplier(int playerCount, int playerThreshold, bool isLegacy, float dynamicScalingAmount, float bossDynScalingAmount, bool isBoss)
		{
			float result;
			if (isLegacy)
			{
				float num = 1.2f;
				bool flag = playerCount <= 0;
				if (flag)
				{
					playerCount = 1;
				}
				bool flag2 = playerThreshold == 1 || playerThreshold == playerCount;
				if (flag2)
				{
					result = 1f;
				}
				else
				{
					float num2 = Math.Min(1f / (float)playerThreshold * num, 1f);
					float num3 = 1f - (1f - num2) / (float)(playerThreshold - 1) * (float)(playerThreshold - playerCount);
					result = num3;
				}
			}
			else
			{
				bool flag3 = playerCount <= 1;
				if (flag3)
				{
					result = 1f;
				}
				else
				{
					bool flag4 = isBoss && bossDynScalingAmount > 0f;
					if (flag4)
					{
						result = 1f + (float)(playerCount - 1) * bossDynScalingAmount;
					}
					else
					{
						result = 1f + (float)(playerCount - 1) * dynamicScalingAmount;
					}
				}
			}
			return result;
		}

		// Token: 0x0400000C RID: 12
		private const float NPC_Levels_Behind_Player = 3f;

		// Token: 0x0400000D RID: 13
		public const int PowerPerLevel = 20;

		// Token: 0x0400000E RID: 14
		private static readonly Dictionary<Stat, float> Stat_Weights = new Dictionary<Stat, float>
		{
			{
				Stat.MaxHealth,
				1f
			},
			{
				Stat.Attack,
				0.1f
			},
			{
				Stat.Armor,
				0.15f
			},
			{
				Stat.Crit,
				0.14f
			},
			{
				Stat.Evasion,
				0.05f
			},
			{
				Stat.Haste,
				0.1f
			}
		};

		// Token: 0x0400000F RID: 15
		private static readonly Dictionary<Stat, float> Extra_Stat_Weights = new Dictionary<Stat, float>
		{
			{
				Stat.MaxHealth,
				1f
			},
			{
				Stat.Attack,
				0.1f
			},
			{
				Stat.Armor,
				0.15f
			},
			{
				Stat.Crit,
				0.15f
			},
			{
				Stat.Evasion,
				0.1f
			},
			{
				Stat.Haste,
				0.2f
			}
		};

		// Token: 0x04000010 RID: 16
		private static readonly Dictionary<Stat, float> Derived_Stat_Start_Values = new Dictionary<Stat, float>
		{
			{
				Stat.Armor,
				0f
			},
			{
				Stat.Evasion,
				0.01f
			},
			{
				Stat.Crit,
				0.01f
			},
			{
				Stat.Haste,
				0f
			}
		};

		// Token: 0x04000011 RID: 17
		public const float Ult_From_Expected_Damage = 40f;

		// Token: 0x04000012 RID: 18
		public const float Ult_From_Expected_Health = 250f;

		// Token: 0x04000013 RID: 19
		public const int Starting_Stat_Value = 200;

		// Token: 0x04000014 RID: 20
		public const float Stat_Increase_Per_Level = 0.085f;

		// Token: 0x04000015 RID: 21
		public const float Stat_Percent_From_Leveling = 0.5f;

		// Token: 0x04000016 RID: 22
		public const float Gear_Stat_Variance = 0.25f;

		// Token: 0x04000017 RID: 23
		public const int Max_Level_For_Stat_Bonuses = 10;

		// Token: 0x04000018 RID: 24
		public const float Starting_Stat_Bonus_Percent = 0.5f;

		// Token: 0x04000019 RID: 25
		public const float Min_Cast_Speed = 0.1f;

		// Token: 0x0400001A RID: 26
		public const float Max_Cast_Speed = 10f;
	}
}
