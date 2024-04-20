using System;

namespace System.Windows.Controls
{
	// Token: 0x020007DC RID: 2012
	internal interface IStackMeasure
	{
		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x060073BA RID: 29626
		bool IsScrolling { get; }

		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x060073BB RID: 29627
		UIElementCollection InternalChildren { get; }

		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x060073BC RID: 29628
		Orientation Orientation { get; }

		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x060073BD RID: 29629
		bool CanVerticallyScroll { get; }

		// Token: 0x17001AD5 RID: 6869
		// (get) Token: 0x060073BE RID: 29630
		bool CanHorizontallyScroll { get; }

		// Token: 0x060073BF RID: 29631
		void OnScrollChange();
	}
}
