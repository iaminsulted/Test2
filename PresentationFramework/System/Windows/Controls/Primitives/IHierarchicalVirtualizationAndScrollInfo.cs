using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200083B RID: 2107
	public interface IHierarchicalVirtualizationAndScrollInfo
	{
		// Token: 0x17001C8E RID: 7310
		// (get) Token: 0x06007B8E RID: 31630
		// (set) Token: 0x06007B8F RID: 31631
		HierarchicalVirtualizationConstraints Constraints { get; set; }

		// Token: 0x17001C8F RID: 7311
		// (get) Token: 0x06007B90 RID: 31632
		HierarchicalVirtualizationHeaderDesiredSizes HeaderDesiredSizes { get; }

		// Token: 0x17001C90 RID: 7312
		// (get) Token: 0x06007B91 RID: 31633
		// (set) Token: 0x06007B92 RID: 31634
		HierarchicalVirtualizationItemDesiredSizes ItemDesiredSizes { get; set; }

		// Token: 0x17001C91 RID: 7313
		// (get) Token: 0x06007B93 RID: 31635
		Panel ItemsHost { get; }

		// Token: 0x17001C92 RID: 7314
		// (get) Token: 0x06007B94 RID: 31636
		// (set) Token: 0x06007B95 RID: 31637
		bool MustDisableVirtualization { get; set; }

		// Token: 0x17001C93 RID: 7315
		// (get) Token: 0x06007B96 RID: 31638
		// (set) Token: 0x06007B97 RID: 31639
		bool InBackgroundLayout { get; set; }
	}
}
