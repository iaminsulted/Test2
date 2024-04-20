using System;
using System.Collections.Generic;

namespace StatCurves
{
	// Token: 0x02000005 RID: 5
	public static class Infusion
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002AA4 File Offset: 0x00000CA4
		public static int GetUpgradeRuneIDForLevel(int level)
		{
			return Infusion.upgradeRuneIDForLevel.ContainsKey(level) ? Infusion.upgradeRuneIDForLevel[level] : 0;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002AD4 File Offset: 0x00000CD4
		public static int GetInertRuneIDForLevel(int level)
		{
			return Infusion.inertRuneIDForLevel.ContainsKey(level) ? Infusion.inertRuneIDForLevel[level] : 0;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002B04 File Offset: 0x00000D04
		public static int GetInfusionRuneCost(int currentOffset)
		{
			int result = 0;
			switch (currentOffset)
			{
			case 0:
				result = 1;
				break;
			case 1:
				result = 1;
				break;
			case 2:
				result = 2;
				break;
			case 3:
				result = 2;
				break;
			case 4:
				result = 2;
				break;
			case 5:
				result = 2;
				break;
			case 6:
				result = 3;
				break;
			case 7:
				result = 3;
				break;
			case 8:
				result = 4;
				break;
			case 9:
				result = 5;
				break;
			case 10:
				result = 5;
				break;
			case 11:
				result = 5;
				break;
			case 12:
				result = 5;
				break;
			case 13:
				result = 5;
				break;
			case 14:
				result = 5;
				break;
			case 15:
				result = 5;
				break;
			case 16:
				result = 5;
				break;
			case 17:
				result = 5;
				break;
			case 18:
				result = 5;
				break;
			case 19:
				result = 5;
				break;
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002BC8 File Offset: 0x00000DC8
		public static int GetExtractionGoldCost(int currentOffset, int level)
		{
			return Infusion.GetInfusionGoldCost(currentOffset, level) / 5;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002BE4 File Offset: 0x00000DE4
		public static int GetInfusionGoldCost(int currentOffset, int level)
		{
			return level * currentOffset * 20 + level * 10;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002C04 File Offset: 0x00000E04
		public static int GetRunesPerDay()
		{
			return 10;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002C18 File Offset: 0x00000E18
		public static int GetInfusionCapstoneCost(int currentOffset)
		{
			return 100;
		}

		// Token: 0x0400001F RID: 31
		public const int Default_Infusion_Cap = 10;

		// Token: 0x04000020 RID: 32
		public const int Capstone_Cost = 100;

		// Token: 0x04000021 RID: 33
		public const int Runes_Per_Day = 10;

		// Token: 0x04000022 RID: 34
		private static readonly Dictionary<int, int> upgradeRuneIDForLevel = new Dictionary<int, int>
		{
			{
				35,
				7219
			},
			{
				45,
				12220
			}
		};

		// Token: 0x04000023 RID: 35
		private static readonly Dictionary<int, int> inertRuneIDForLevel = new Dictionary<int, int>
		{
			{
				35,
				7218
			},
			{
				45,
				12221
			}
		};
	}
}
