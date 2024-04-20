using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001CA RID: 458
	internal interface IDocumentScrollInfo : IScrollInfo
	{
		// Token: 0x06000FD1 RID: 4049
		void MakePageVisible(int pageNumber);

		// Token: 0x06000FD2 RID: 4050
		void MakeSelectionVisible();

		// Token: 0x06000FD3 RID: 4051
		Rect MakeVisible(object o, Rect r, int pageNumber);

		// Token: 0x06000FD4 RID: 4052
		void ScrollToNextRow();

		// Token: 0x06000FD5 RID: 4053
		void ScrollToPreviousRow();

		// Token: 0x06000FD6 RID: 4054
		void ScrollToHome();

		// Token: 0x06000FD7 RID: 4055
		void ScrollToEnd();

		// Token: 0x06000FD8 RID: 4056
		void SetScale(double scale);

		// Token: 0x06000FD9 RID: 4057
		void SetColumns(int columns);

		// Token: 0x06000FDA RID: 4058
		void FitColumns(int columns);

		// Token: 0x06000FDB RID: 4059
		void FitToPageWidth();

		// Token: 0x06000FDC RID: 4060
		void FitToPageHeight();

		// Token: 0x06000FDD RID: 4061
		void ViewThumbnails();

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000FDE RID: 4062
		// (set) Token: 0x06000FDF RID: 4063
		DynamicDocumentPaginator Content { get; set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000FE0 RID: 4064
		int PageCount { get; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000FE1 RID: 4065
		int FirstVisiblePageNumber { get; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000FE2 RID: 4066
		double Scale { get; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000FE3 RID: 4067
		int MaxPagesAcross { get; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000FE4 RID: 4068
		// (set) Token: 0x06000FE5 RID: 4069
		double VerticalPageSpacing { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000FE6 RID: 4070
		// (set) Token: 0x06000FE7 RID: 4071
		double HorizontalPageSpacing { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000FE8 RID: 4072
		// (set) Token: 0x06000FE9 RID: 4073
		bool ShowPageBorders { get; set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000FEA RID: 4074
		// (set) Token: 0x06000FEB RID: 4075
		bool LockViewModes { get; set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000FEC RID: 4076
		ITextView TextView { get; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000FED RID: 4077
		ITextContainer TextContainer { get; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000FEE RID: 4078
		ReadOnlyCollection<DocumentPageView> PageViews { get; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000FEF RID: 4079
		// (set) Token: 0x06000FF0 RID: 4080
		DocumentViewer DocumentViewerOwner { get; set; }
	}
}
