using System;
using System.Collections.Generic;
using System.Linq;

namespace StatCurves
{
	// Token: 0x02000002 RID: 2
	public static class ClassRanks
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		static ClassRanks()
		{
			for (int i = 1; i <= 100; i++)
			{
				int value = 5;
				bool flag = i > 1;
				if (flag)
				{
					value = (int)Math.Round(Math.Pow((double)i, 2.0) / 3.0 + 7.0);
				}
				ClassRanks.XPPerKill[i] = value;
				bool flag2 = i > 10;
				if (flag2)
				{
					double y = ((double)i - 10.0) / 90.0 * (((double)i - 10.0) / 180.0);
					ClassRanks.timePerLevel[i] = ClassRanks.timePerLevel[i - 1] + Math.Pow(ClassRanks.timePerLevel[i - 1], y) * 10.0;
					ClassRanks.TotalXPToRankUp[i] = (int)Math.Ceiling(ClassRanks.timePerLevel[i] * (double)(8 * ClassRanks.XPPerKill[i]));
					bool flag3 = i == 100;
					if (flag3)
					{
						ClassRanks.timePerLevel[i + 1] = 2147483647.0;
					}
				}
			}
			for (int j = 11; j <= 50; j++)
			{
				int num = j * 100;
				ClassRanks.TotalGloryXPToRankUp[j] = ClassRanks.TotalGloryXPToRankUp[j - 1] + num;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000239C File Offset: 0x0000059C
		public static int GetMaxClassXP()
		{
			return ClassRanks.GetTotalXPToRankUp(99);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000023B8 File Offset: 0x000005B8
		public static int GetTotalXPToRankUp(int rank)
		{
			return ClassRanks.TotalXPToRankUp.ContainsKey(rank) ? ClassRanks.TotalXPToRankUp[rank] : 0;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000023E8 File Offset: 0x000005E8
		public static float GetRankXPPercent(int ClassXP, int ClassRank)
		{
			int num = ClassXP - ClassRanks.GetTotalXPToRankUp(ClassRank - 1);
			int num2 = ClassRanks.GetTotalXPToRankUp(ClassRank) - ClassRanks.GetTotalXPToRankUp(ClassRank - 1);
			return (float)num / (float)num2;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000241C File Offset: 0x0000061C
		public static int GetRank(int classXP)
		{
			bool flag = classXP > ClassRanks.TotalXPToRankUp[100];
			int result;
			if (flag)
			{
				result = 100;
			}
			else
			{
				result = (from x in ClassRanks.TotalXPToRankUp
				orderby x.Value
				select x).FirstOrDefault((KeyValuePair<int, int> x) => classXP < x.Value).Key;
			}
			return result;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000249C File Offset: 0x0000069C
		public static int GetXPPerKill(int classRank, int levelDifference)
		{
			int xp = ClassRanks.XPPerKill.ContainsKey(classRank) ? ClassRanks.XPPerKill[classRank] : 0;
			return ClassRanks.AdjustXPByLevelDifference(xp, levelDifference);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000024D4 File Offset: 0x000006D4
		public static int GetBaseQuestXP(int classRank, int levelDifference)
		{
			return ClassRanks.GetXPPerKill(classRank, levelDifference) * 35;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000024F0 File Offset: 0x000006F0
		public static int AdjustXPByLevelDifference(int xp, int levelDifference)
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
			float num = 0.1f * (float)levelDifference;
			xp = (int)((float)xp - (float)xp * num);
			xp = Math.Max(1, xp);
			return xp;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002540 File Offset: 0x00000740
		public static int GetGloryRank(int xp)
		{
			bool flag = xp < 126800;
			int result;
			if (flag)
			{
				result = (from x in ClassRanks.TotalGloryXPToRankUp
				orderby x.Value
				select x).FirstOrDefault((KeyValuePair<int, int> x) => xp < x.Value).Key;
			}
			else
			{
				result = 50 + (int)Math.Floor((double)(xp - 126800) / 126800.0);
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000025D8 File Offset: 0x000007D8
		public static int GetGloryToRankUp(int rank)
		{
			bool flag = ClassRanks.TotalGloryXPToRankUp.ContainsKey(rank);
			int result;
			if (flag)
			{
				result = ClassRanks.TotalGloryXPToRankUp[rank];
			}
			else
			{
				result = 126800;
			}
			return result;
		}

		// Token: 0x04000001 RID: 1
		public const int Max_Class_Rank = 100;

		// Token: 0x04000002 RID: 2
		public const int Ultimate_Rank = 5;

		// Token: 0x04000003 RID: 3
		public const int Cross_Skill_Rank = 10;

		// Token: 0x04000004 RID: 4
		public const int Class_XP_Per_Class_Token = 4;

		// Token: 0x04000005 RID: 5
		public const float Class_XP_Multiplier_By_Tier = 0.2f;

		// Token: 0x04000006 RID: 6
		public const float Level_Difference_Falloff = 0.1f;

		// Token: 0x04000007 RID: 7
		public const int Quest_Kill_Equivalent = 35;

		// Token: 0x04000008 RID: 8
		private static readonly Dictionary<int, int> TotalXPToRankUp = new Dictionary<int, int>
		{
			{
				1,
				15
			},
			{
				2,
				93
			},
			{
				3,
				337
			},
			{
				4,
				948
			},
			{
				5,
				2168
			},
			{
				6,
				4457
			},
			{
				7,
				8510
			},
			{
				8,
				15365
			},
			{
				9,
				26541
			},
			{
				10,
				31526
			}
		};

		// Token: 0x04000009 RID: 9
		private static readonly Dictionary<int, int> TotalGloryXPToRankUp = new Dictionary<int, int>
		{
			{
				1,
				100
			},
			{
				2,
				500
			},
			{
				3,
				1000
			},
			{
				4,
				1500
			},
			{
				5,
				2000
			},
			{
				6,
				2500
			},
			{
				7,
				3000
			},
			{
				8,
				3600
			},
			{
				9,
				4200
			},
			{
				10,
				4800
			}
		};

		// Token: 0x0400000A RID: 10
		private static readonly Dictionary<int, int> XPPerKill = new Dictionary<int, int>();

		// Token: 0x0400000B RID: 11
		private static readonly Dictionary<int, double> timePerLevel = new Dictionary<int, double>
		{
			{
				1,
				0.23000000417232513
			},
			{
				2,
				1.159999966621399
			},
			{
				3,
				3.4200000762939453
			},
			{
				4,
				7.730000019073486
			},
			{
				5,
				14.260000228881836
			},
			{
				6,
				23.8799991607666
			},
			{
				7,
				37.540000915527344
			},
			{
				8,
				56.5
			},
			{
				9,
				82.26000213623047
			},
			{
				10,
				83.26000213623047
			}
		};
	}
}
