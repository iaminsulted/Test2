using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;

namespace MS.Internal.Documents
{
	// Token: 0x020001CF RID: 463
	internal class ReaderTwoPageViewer : ReaderPageViewer
	{
		// Token: 0x0600104F RID: 4175 RVA: 0x0013F930 File Offset: 0x0013E930
		protected override void OnPreviousPageCommand()
		{
			base.GoToPage(Math.Max(1, this.MasterPageNumber - 2));
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0013F946 File Offset: 0x0013E946
		protected override void OnNextPageCommand()
		{
			base.GoToPage(Math.Min(base.PageCount, this.MasterPageNumber + 2));
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0013F961 File Offset: 0x0013E961
		protected override void OnLastPageCommand()
		{
			base.GoToPage(base.PageCount);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0013F96F File Offset: 0x0013E96F
		protected override void OnGoToPageCommand(int pageNumber)
		{
			base.OnGoToPageCommand((pageNumber - 1) / 2 * 2 + 1);
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0013F980 File Offset: 0x0013E980
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DocumentViewerBase.MasterPageNumberProperty)
			{
				int num = (int)e.NewValue;
				num = (num - 1) / 2 * 2 + 1;
				if (num != (int)e.NewValue)
				{
					base.GoToPage(num);
				}
			}
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0013F9CF File Offset: 0x0013E9CF
		static ReaderTwoPageViewer()
		{
			DocumentViewerBase.CanGoToNextPagePropertyKey.OverrideMetadata(typeof(ReaderTwoPageViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, null, new CoerceValueCallback(ReaderTwoPageViewer.CoerceCanGoToNextPage)));
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0013F9FC File Offset: 0x0013E9FC
		private static object CoerceCanGoToNextPage(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is ReaderTwoPageViewer);
			ReaderTwoPageViewer readerTwoPageViewer = (ReaderTwoPageViewer)d;
			return readerTwoPageViewer.MasterPageNumber < readerTwoPageViewer.PageCount - 1;
		}
	}
}
