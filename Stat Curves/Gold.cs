using System;
using System.Collections.Generic;

namespace StatCurves
{
	// Token: 0x02000004 RID: 4
	public static class Gold
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000029DC File Offset: 0x00000BDC
		static Gold()
		{
			for (int i = 1; i < 100; i++)
			{
				bool flag = i <= 30;
				float num;
				if (flag)
				{
					num = (float)i * 0.55f;
				}
				else
				{
					num = 16.5f;
					num += (float)(i - 30) * 0.2f;
				}
				Gold.GoldFromKill[i] = (int)Math.Round((double)num);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A50 File Offset: 0x00000C50
		public static int GetGoldPerKill(int level, int levelDifference)
		{
			int reward = Gold.GoldFromKill.ContainsKey(level) ? Gold.GoldFromKill[level] : 0;
			return Levels.AdjustRewardByLevelDifference(reward, levelDifference);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A88 File Offset: 0x00000C88
		public static int GetBaseQuestGold(int level)
		{
			return Gold.GetGoldPerKill(level, 0) * 35;
		}

		// Token: 0x0400001B RID: 27
		private const int Gold_Curve_Max_Level = 30;

		// Token: 0x0400001C RID: 28
		private const float Gold_Increase_Before_Max = 0.55f;

		// Token: 0x0400001D RID: 29
		private const float Gold_Increase_After_Max = 0.2f;

		// Token: 0x0400001E RID: 30
		private static Dictionary<int, int> GoldFromKill = new Dictionary<int, int>();
	}
}
