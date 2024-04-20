using System;

namespace System.Windows.Controls
{
	// Token: 0x020007DD RID: 2013
	internal interface IStackMeasureScrollData
	{
		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x060073C0 RID: 29632
		// (set) Token: 0x060073C1 RID: 29633
		Vector Offset { get; set; }

		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x060073C2 RID: 29634
		// (set) Token: 0x060073C3 RID: 29635
		Size Viewport { get; set; }

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x060073C4 RID: 29636
		// (set) Token: 0x060073C5 RID: 29637
		Size Extent { get; set; }

		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x060073C6 RID: 29638
		// (set) Token: 0x060073C7 RID: 29639
		Vector ComputedOffset { get; set; }

		// Token: 0x060073C8 RID: 29640
		void SetPhysicalViewport(double value);
	}
}
