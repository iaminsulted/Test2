using System;
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000841 RID: 2113
	public interface IScrollInfo
	{
		// Token: 0x06007BAD RID: 31661
		void LineUp();

		// Token: 0x06007BAE RID: 31662
		void LineDown();

		// Token: 0x06007BAF RID: 31663
		void LineLeft();

		// Token: 0x06007BB0 RID: 31664
		void LineRight();

		// Token: 0x06007BB1 RID: 31665
		void PageUp();

		// Token: 0x06007BB2 RID: 31666
		void PageDown();

		// Token: 0x06007BB3 RID: 31667
		void PageLeft();

		// Token: 0x06007BB4 RID: 31668
		void PageRight();

		// Token: 0x06007BB5 RID: 31669
		void MouseWheelUp();

		// Token: 0x06007BB6 RID: 31670
		void MouseWheelDown();

		// Token: 0x06007BB7 RID: 31671
		void MouseWheelLeft();

		// Token: 0x06007BB8 RID: 31672
		void MouseWheelRight();

		// Token: 0x06007BB9 RID: 31673
		void SetHorizontalOffset(double offset);

		// Token: 0x06007BBA RID: 31674
		void SetVerticalOffset(double offset);

		// Token: 0x06007BBB RID: 31675
		Rect MakeVisible(Visual visual, Rect rectangle);

		// Token: 0x17001C96 RID: 7318
		// (get) Token: 0x06007BBC RID: 31676
		// (set) Token: 0x06007BBD RID: 31677
		bool CanVerticallyScroll { get; set; }

		// Token: 0x17001C97 RID: 7319
		// (get) Token: 0x06007BBE RID: 31678
		// (set) Token: 0x06007BBF RID: 31679
		bool CanHorizontallyScroll { get; set; }

		// Token: 0x17001C98 RID: 7320
		// (get) Token: 0x06007BC0 RID: 31680
		double ExtentWidth { get; }

		// Token: 0x17001C99 RID: 7321
		// (get) Token: 0x06007BC1 RID: 31681
		double ExtentHeight { get; }

		// Token: 0x17001C9A RID: 7322
		// (get) Token: 0x06007BC2 RID: 31682
		double ViewportWidth { get; }

		// Token: 0x17001C9B RID: 7323
		// (get) Token: 0x06007BC3 RID: 31683
		double ViewportHeight { get; }

		// Token: 0x17001C9C RID: 7324
		// (get) Token: 0x06007BC4 RID: 31684
		double HorizontalOffset { get; }

		// Token: 0x17001C9D RID: 7325
		// (get) Token: 0x06007BC5 RID: 31685
		double VerticalOffset { get; }

		// Token: 0x17001C9E RID: 7326
		// (get) Token: 0x06007BC6 RID: 31686
		// (set) Token: 0x06007BC7 RID: 31687
		ScrollViewer ScrollOwner { get; set; }
	}
}
