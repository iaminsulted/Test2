using System;

namespace System.Windows
{
	// Token: 0x0200036B RID: 875
	internal static class LayoutDoubleUtil
	{
		// Token: 0x0600232D RID: 9005 RVA: 0x000F6CE8 File Offset: 0x000F5CE8
		internal static bool AreClose(double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			double num = value1 - value2;
			return num < 1.53E-06 && num > -1.53E-06;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x0017E86C File Offset: 0x0017D86C
		internal static bool LessThan(double value1, double value2)
		{
			return value1 < value2 && !LayoutDoubleUtil.AreClose(value1, value2);
		}

		// Token: 0x04001076 RID: 4214
		private const double eps = 1.53E-06;
	}
}
