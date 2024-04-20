using System;

namespace StatCurves
{
	// Token: 0x02000010 RID: 16
	public static class Modifiers
	{
		// Token: 0x0600006D RID: 109 RVA: 0x000043BC File Offset: 0x000025BC
		public static int GetRerollCost(int itemLevel, int itemRarity)
		{
			return (itemLevel + itemRarity) * 100;
		}
	}
}
