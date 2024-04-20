using System;

namespace Standard
{
	// Token: 0x02000007 RID: 7
	internal static class DoubleUtilities
	{
		// Token: 0x06000020 RID: 32 RVA: 0x000F6CE8 File Offset: 0x000F5CE8
		public static bool AreClose(double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			double num = value1 - value2;
			return num < 1.53E-06 && num > -1.53E-06;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000F6D19 File Offset: 0x000F5D19
		public static bool LessThan(double value1, double value2)
		{
			return value1 < value2 && !DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000F6D2B File Offset: 0x000F5D2B
		public static bool GreaterThan(double value1, double value2)
		{
			return value1 > value2 && !DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000F6D3D File Offset: 0x000F5D3D
		public static bool LessThanOrClose(double value1, double value2)
		{
			return value1 < value2 || DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000F6D4C File Offset: 0x000F5D4C
		public static bool GreaterThanOrClose(double value1, double value2)
		{
			return value1 > value2 || DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000F6D5B File Offset: 0x000F5D5B
		public static bool IsFinite(double value)
		{
			return !double.IsNaN(value) && !double.IsInfinity(value);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000F6D70 File Offset: 0x000F5D70
		public static bool IsValidSize(double value)
		{
			return DoubleUtilities.IsFinite(value) && DoubleUtilities.GreaterThanOrClose(value, 0.0);
		}

		// Token: 0x04000020 RID: 32
		private const double Epsilon = 1.53E-06;
	}
}
