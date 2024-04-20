using System;
using System.Collections.Generic;

namespace StatCurves
{
	// Token: 0x0200000F RID: 15
	public static class Levels
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00004040 File Offset: 0x00002240
		static Levels()
		{
			Levels.XPCurve[1] = 100;
			Levels.XPCurve[2] = 500;
			Levels.XPCurve[3] = 3000;
			Levels.XPCurve[4] = 5000;
			for (int i = 1; i <= 100; i++)
			{
				Levels.XPFromKill[i] = (int)Math.Round(Math.Pow((double)i, 2.0) / 6.0 + 10.0);
				bool flag = i >= 5;
				if (flag)
				{
					Levels.XPCurve[i] = (int)((Math.Pow((double)i, 2.0) / 3.0 + 10.0) * 200.0 * (double)((float)i / 8f + 2f));
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004150 File Offset: 0x00002350
		public static int GetXPToLevel(int level, int levelCap)
		{
			bool flag = level >= levelCap - 5;
			int result;
			if (flag)
			{
				int num = Levels.XPCurve.ContainsKey(level) ? Levels.XPCurve[level] : 0;
				num *= level - (levelCap - 6);
				result = num;
			}
			else
			{
				result = (Levels.XPCurve.ContainsKey(level) ? Levels.XPCurve[level] : 0);
			}
			return result;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000041B4 File Offset: 0x000023B4
		public static int GetGuildXpDivisor()
		{
			return 10;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000041C8 File Offset: 0x000023C8
		public static long GetXPToGuildLevel(int level, int levelCap, int memberCount)
		{
			bool flag = level < 1;
			long result;
			if (flag)
			{
				result = 0L;
			}
			else
			{
				long num = 0L;
				for (int i = 1; i <= level; i++)
				{
					bool flag2 = i >= levelCap - 5;
					if (flag2)
					{
						long num2 = (long)(Levels.XPCurve.ContainsKey(i) ? Levels.XPCurve[i] : 0);
						num2 *= (long)(i - (levelCap - 6));
						num += num2 * 2L;
					}
					else
					{
						num += (long)(Levels.XPCurve.ContainsKey(i) ? (Levels.XPCurve[i] * 2) : 0);
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004274 File Offset: 0x00002474
		public static int GetGuildLevelByXp(long xp, int levelCap, int memberCount)
		{
			int num = 1;
			bool flag = false;
			while (!flag)
			{
				long xptoGuildLevel = Levels.GetXPToGuildLevel(num, levelCap, memberCount);
				bool flag2 = xptoGuildLevel > xp || num >= levelCap;
				if (flag2)
				{
					return num;
				}
				num++;
			}
			return num;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000042C4 File Offset: 0x000024C4
		public static int GetXPPerKill(int level, int levelDifference)
		{
			int reward = Levels.XPFromKill.ContainsKey(level) ? Levels.XPFromKill[level] : 0;
			return Levels.AdjustRewardByLevelDifference(reward, levelDifference);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000042FC File Offset: 0x000024FC
		public static int GetBaseQuestXP(int level)
		{
			return Levels.GetXPPerKill(level, 0) * 35;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004318 File Offset: 0x00002518
		internal static int AdjustRewardByLevelDifference(int reward, int levelDifference)
		{
			bool flag = levelDifference < 0;
			if (flag)
			{
				levelDifference = 0;
			}
			else
			{
				bool flag2 = levelDifference > 10;
				if (flag2)
				{
					levelDifference = 10;
				}
			}
			bool flag3 = (float)levelDifference <= 4f;
			int result;
			if (flag3)
			{
				result = reward;
			}
			else
			{
				float num = 0.15f * (float)levelDifference;
				reward = (int)((float)reward - (float)reward * num);
				reward = Math.Max(1, reward);
				result = reward;
			}
			return result;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004380 File Offset: 0x00002580
		public static int GetScaledLevel(int playerLevel, int areaMinScalingLevel, int areaMaxScalingLevel)
		{
			bool flag = playerLevel >= areaMinScalingLevel && playerLevel <= areaMaxScalingLevel;
			int result;
			if (flag)
			{
				result = areaMinScalingLevel;
			}
			else
			{
				bool flag2 = playerLevel > areaMaxScalingLevel;
				if (flag2)
				{
					result = playerLevel - areaMaxScalingLevel + areaMinScalingLevel;
				}
				else
				{
					result = playerLevel;
				}
			}
			return result;
		}

		// Token: 0x0400006A RID: 106
		internal const int Max_Level_Curve_Generation = 100;

		// Token: 0x0400006B RID: 107
		internal const int Quest_Kill_Equivalent = 35;

		// Token: 0x0400006C RID: 108
		private const float Level_Falloff_Buffer = 4f;

		// Token: 0x0400006D RID: 109
		private const float Level_Difference_Falloff = 0.15f;

		// Token: 0x0400006E RID: 110
		public const int Level_End_Game = 35;

		// Token: 0x0400006F RID: 111
		private static Dictionary<int, int> XPCurve = new Dictionary<int, int>();

		// Token: 0x04000070 RID: 112
		private static Dictionary<int, int> XPFromKill = new Dictionary<int, int>();
	}
}
