using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020001C7 RID: 455
	internal class FlowDocumentPaginator : DynamicDocumentPaginator, IServiceProvider, IFlowDocumentFormatter
	{
		// Token: 0x06000F83 RID: 3971 RVA: 0x0013DCA0 File Offset: 0x0013CCA0
		internal FlowDocumentPaginator(FlowDocument document)
		{
			this._pageSize = FlowDocumentPaginator._defaultPageSize;
			this._document = document;
			this._brt = new BreakRecordTable(this);
			this._dispatcherObject = new FlowDocumentPaginator.CustomDispatcherObject();
			this._backgroundPagination = true;
			this.InitiateNextAsyncOperation();
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0013DCF8 File Offset: 0x0013CCF8
		public override void GetPageAsync(int pageNumber, object userState)
		{
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.IsContentChangeInProgress)
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			DocumentPage documentPage = null;
			if (!this._backgroundPagination)
			{
				documentPage = this.GetPage(pageNumber);
			}
			else
			{
				if (this._brt.IsClean && !this._brt.HasPageBreakRecord(pageNumber))
				{
					documentPage = DocumentPage.Missing;
				}
				if (this._brt.HasPageBreakRecord(pageNumber))
				{
					documentPage = this.GetPage(pageNumber);
				}
				if (documentPage == null)
				{
					this._asyncRequests.Add(new FlowDocumentPaginator.GetPageAsyncRequest(pageNumber, userState, this));
					this.InitiateNextAsyncOperation();
				}
			}
			if (documentPage != null)
			{
				this.OnGetPageCompleted(new GetPageCompletedEventArgs(documentPage, pageNumber, null, false, userState));
			}
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0013DDDC File Offset: 0x0013CDDC
		public override DocumentPage GetPage(int pageNumber)
		{
			this._dispatcherObject.VerifyAccess();
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.IsContentChangeInProgress)
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			DocumentPage documentPage;
			using (this._document.Dispatcher.DisableProcessing())
			{
				this._document.StructuralCache.IsFormattingInProgress = true;
				try
				{
					if (this._brt.IsClean && !this._brt.HasPageBreakRecord(pageNumber))
					{
						documentPage = DocumentPage.Missing;
					}
					else
					{
						documentPage = this._brt.GetCachedDocumentPage(pageNumber);
						if (documentPage == null)
						{
							if (!this._brt.HasPageBreakRecord(pageNumber))
							{
								documentPage = this.FormatPagesTill(pageNumber);
							}
							else
							{
								documentPage = this.FormatPage(pageNumber);
							}
						}
					}
				}
				finally
				{
					this._document.StructuralCache.IsFormattingInProgress = false;
				}
			}
			return documentPage;
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0013DF08 File Offset: 0x0013CF08
		public override void GetPageNumberAsync(ContentPosition contentPosition, object userState)
		{
			if (contentPosition == null)
			{
				throw new ArgumentNullException("contentPosition");
			}
			if (contentPosition == ContentPosition.Missing)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			TextPointer textPointer = contentPosition as TextPointer;
			if (textPointer == null)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			if (textPointer.TextContainer != this._document.StructuralCache.TextContainer)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			int pageNumber = 0;
			if (!this._backgroundPagination)
			{
				pageNumber = this.GetPageNumber(contentPosition);
				this.OnGetPageNumberCompleted(new GetPageNumberCompletedEventArgs(contentPosition, pageNumber, null, false, userState));
				return;
			}
			if (this._brt.GetPageNumberForContentPosition(textPointer, ref pageNumber))
			{
				this.OnGetPageNumberCompleted(new GetPageNumberCompletedEventArgs(contentPosition, pageNumber, null, false, userState));
				return;
			}
			this._asyncRequests.Add(new FlowDocumentPaginator.GetPageNumberAsyncRequest(textPointer, userState, this));
			this.InitiateNextAsyncOperation();
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0013DFEC File Offset: 0x0013CFEC
		public override int GetPageNumber(ContentPosition contentPosition)
		{
			this._dispatcherObject.VerifyAccess();
			if (contentPosition == null)
			{
				throw new ArgumentNullException("contentPosition");
			}
			TextPointer textPointer = contentPosition as TextPointer;
			if (textPointer == null)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			if (textPointer.TextContainer != this._document.StructuralCache.TextContainer)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.IsContentChangeInProgress)
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			int num;
			using (this._document.Dispatcher.DisableProcessing())
			{
				this._document.StructuralCache.IsFormattingInProgress = true;
				num = 0;
				try
				{
					while (!this._brt.GetPageNumberForContentPosition(textPointer, ref num))
					{
						if (this._brt.IsClean)
						{
							num = -1;
							break;
						}
						this.FormatPage(num);
					}
				}
				finally
				{
					this._document.StructuralCache.IsFormattingInProgress = false;
				}
			}
			return num;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0013E130 File Offset: 0x0013D130
		public override ContentPosition GetPagePosition(DocumentPage page)
		{
			this._dispatcherObject.VerifyAccess();
			if (page == null)
			{
				throw new ArgumentNullException("page");
			}
			FlowDocumentPage flowDocumentPage = page as FlowDocumentPage;
			if (flowDocumentPage == null || flowDocumentPage.IsDisposed)
			{
				return ContentPosition.Missing;
			}
			Point point = new Point(0.0, 0.0);
			if (this._document.FlowDirection == FlowDirection.RightToLeft)
			{
				Point point2;
				new MatrixTransform(-1.0, 0.0, 0.0, 1.0, flowDocumentPage.Size.Width, 0.0).TryTransform(point, out point2);
				point = point2;
			}
			ITextView textView = (ITextView)((IServiceProvider)flowDocumentPage).GetService(typeof(ITextView));
			Invariant.Assert(textView != null, "Cannot access ITextView for FlowDocumentPage.");
			if (textView.TextSegments.Count == 0)
			{
				return ContentPosition.Missing;
			}
			ITextPointer textPointer = textView.GetTextPositionFromPoint(point, true);
			if (textPointer == null)
			{
				textPointer = textView.TextSegments[0].Start;
			}
			if (!(textPointer is TextPointer))
			{
				return ContentPosition.Missing;
			}
			return (ContentPosition)textPointer;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0013E24F File Offset: 0x0013D24F
		public override ContentPosition GetObjectPosition(object o)
		{
			this._dispatcherObject.VerifyAccess();
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			return this._document.GetObjectPosition(o);
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0013E278 File Offset: 0x0013D278
		public override void CancelAsync(object userState)
		{
			if (userState == null)
			{
				this.CancelAllAsyncOperations();
				return;
			}
			for (int i = 0; i < this._asyncRequests.Count; i++)
			{
				FlowDocumentPaginator.AsyncRequest asyncRequest = this._asyncRequests[i];
				if (asyncRequest.UserState == userState)
				{
					asyncRequest.Cancel();
					this._asyncRequests.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0013E2D1 File Offset: 0x0013D2D1
		public override bool IsPageCountValid
		{
			get
			{
				this._dispatcherObject.VerifyAccess();
				return this._brt.IsClean;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0013E2E9 File Offset: 0x0013D2E9
		public override int PageCount
		{
			get
			{
				this._dispatcherObject.VerifyAccess();
				return this._brt.Count;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0013E301 File Offset: 0x0013D301
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x0013E30C File Offset: 0x0013D30C
		public override Size PageSize
		{
			get
			{
				return this._pageSize;
			}
			set
			{
				this._dispatcherObject.VerifyAccess();
				Size pageSize = value;
				if (DoubleUtil.IsNaN(pageSize.Width))
				{
					pageSize.Width = FlowDocumentPaginator._defaultPageSize.Width;
				}
				if (DoubleUtil.IsNaN(pageSize.Height))
				{
					pageSize.Height = FlowDocumentPaginator._defaultPageSize.Height;
				}
				Size size = this.ComputePageSize();
				this._pageSize = pageSize;
				Size size2 = this.ComputePageSize();
				if (!DoubleUtil.AreClose(size, size2))
				{
					if (this._document.StructuralCache.IsFormattingInProgress)
					{
						this._document.StructuralCache.OnInvalidOperationDetected();
						throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
					}
					this.InvalidateBRT();
				}
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0013E3B8 File Offset: 0x0013D3B8
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x0013E3C0 File Offset: 0x0013D3C0
		public override bool IsBackgroundPaginationEnabled
		{
			get
			{
				return this._backgroundPagination;
			}
			set
			{
				this._dispatcherObject.VerifyAccess();
				if (value != this._backgroundPagination)
				{
					this._backgroundPagination = value;
					this.InitiateNextAsyncOperation();
				}
				if (!this._backgroundPagination)
				{
					this.CancelAllAsyncOperations();
				}
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0013E3F1 File Offset: 0x0013D3F1
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._document;
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0013E3FC File Offset: 0x0013D3FC
		internal void InitiateNextAsyncOperation()
		{
			if (this._backgroundPagination && this._backgroundPaginationOperation == null && (!this._brt.IsClean || this._asyncRequests.Count > 0))
			{
				this._backgroundPaginationOperation = this._document.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnBackgroundPagination), this);
			}
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0013E458 File Offset: 0x0013D458
		internal void CancelAllAsyncOperations()
		{
			for (int i = 0; i < this._asyncRequests.Count; i++)
			{
				this._asyncRequests[i].Cancel();
			}
			this._asyncRequests.Clear();
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0013E497 File Offset: 0x0013D497
		internal void OnPagesChanged(int pageStart, int pageCount)
		{
			this.OnPagesChanged(new PagesChangedEventArgs(pageStart, pageCount));
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0013E4A6 File Offset: 0x0013D4A6
		internal void OnPaginationProgress(int pageStart, int pageCount)
		{
			this.OnPaginationProgress(new PaginationProgressEventArgs(pageStart, pageCount));
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0013E4B5 File Offset: 0x0013D4B5
		internal void OnPaginationCompleted()
		{
			this.OnPaginationCompleted(EventArgs.Empty);
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000F97 RID: 3991 RVA: 0x0013E4C4 File Offset: 0x0013D4C4
		// (remove) Token: 0x06000F98 RID: 3992 RVA: 0x0013E4FC File Offset: 0x0013D4FC
		internal event BreakRecordTableInvalidatedEventHandler BreakRecordTableInvalidated;

		// Token: 0x06000F99 RID: 3993 RVA: 0x0013E531 File Offset: 0x0013D531
		private void InvalidateBRT()
		{
			if (this.BreakRecordTableInvalidated != null)
			{
				this.BreakRecordTableInvalidated(this, EventArgs.Empty);
			}
			this._brt.OnInvalidateLayout();
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0013E557 File Offset: 0x0013D557
		private void InvalidateBRTLayout(ITextPointer start, ITextPointer end)
		{
			this._brt.OnInvalidateLayout(start, end);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0013E568 File Offset: 0x0013D568
		private DocumentPage FormatPagesTill(int pageNumber)
		{
			while (!this._brt.HasPageBreakRecord(pageNumber) && !this._brt.IsClean)
			{
				this.FormatPage(this._brt.Count);
			}
			if (this._brt.IsClean)
			{
				return DocumentPage.Missing;
			}
			return this.FormatPage(pageNumber);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0013E5C0 File Offset: 0x0013D5C0
		private DocumentPage FormatPage(int pageNumber)
		{
			Invariant.Assert(this._brt.HasPageBreakRecord(pageNumber), "BreakRecord for specified page number does not exist.");
			PageBreakRecord pageBreakRecord = this._brt.GetPageBreakRecord(pageNumber);
			FlowDocumentPage flowDocumentPage = new FlowDocumentPage(this._document.StructuralCache);
			Size size = this.ComputePageSize();
			Thickness pageMargin = this._document.ComputePageMargin();
			PageBreakRecord brOut = flowDocumentPage.FormatFinite(size, pageMargin, pageBreakRecord);
			flowDocumentPage.Arrange(size);
			this._brt.UpdateEntry(pageNumber, flowDocumentPage, brOut, flowDocumentPage.DependentMax);
			return flowDocumentPage;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0013E640 File Offset: 0x0013D640
		private object OnBackgroundPagination(object arg)
		{
			DateTime now = DateTime.Now;
			this._backgroundPaginationOperation = null;
			this._dispatcherObject.VerifyAccess();
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.PtsContext.Disposed)
			{
				return null;
			}
			using (this._document.Dispatcher.DisableProcessing())
			{
				this._document.StructuralCache.IsFormattingInProgress = true;
				try
				{
					for (int i = 0; i < this._asyncRequests.Count; i++)
					{
						if (this._asyncRequests[i].Process())
						{
							this._asyncRequests.RemoveAt(i);
							i--;
						}
					}
					DateTime now2 = DateTime.Now;
					if (this._backgroundPagination && !this._brt.IsClean)
					{
						while (!this._brt.IsClean)
						{
							this.FormatPage(this._brt.Count);
							if ((DateTime.Now.Ticks - now.Ticks) / 10000L > 30L)
							{
								break;
							}
						}
						this.InitiateNextAsyncOperation();
					}
				}
				finally
				{
					this._document.StructuralCache.IsFormattingInProgress = false;
				}
			}
			return null;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0013E7A0 File Offset: 0x0013D7A0
		private Size ComputePageSize()
		{
			Size result = new Size(this._document.PageWidth, this._document.PageHeight);
			if (DoubleUtil.IsNaN(result.Width))
			{
				result.Width = this._pageSize.Width;
				double num = this._document.MaxPageWidth;
				if (result.Width > num)
				{
					result.Width = num;
				}
				double num2 = this._document.MinPageWidth;
				if (result.Width < num2)
				{
					result.Width = num2;
				}
			}
			if (DoubleUtil.IsNaN(result.Height))
			{
				result.Height = this._pageSize.Height;
				double num = this._document.MaxPageHeight;
				if (result.Height > num)
				{
					result.Height = num;
				}
				double num2 = this._document.MinPageHeight;
				if (result.Height < num2)
				{
					result.Height = num2;
				}
			}
			return result;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0013E883 File Offset: 0x0013D883
		object IServiceProvider.GetService(Type serviceType)
		{
			return ((IServiceProvider)this._document).GetService(serviceType);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0013E891 File Offset: 0x0013D891
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout)
		{
			if (affectsLayout)
			{
				this.InvalidateBRT();
				return;
			}
			this._brt.OnInvalidateRender();
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0013E8A8 File Offset: 0x0013D8A8
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout, ITextPointer start, ITextPointer end)
		{
			if (affectsLayout)
			{
				this.InvalidateBRTLayout(start, end);
				return;
			}
			this._brt.OnInvalidateRender(start, end);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0013E8C3 File Offset: 0x0013D8C3
		void IFlowDocumentFormatter.Suspend()
		{
			this.IsBackgroundPaginationEnabled = false;
			this.InvalidateBRT();
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0013E8D2 File Offset: 0x0013D8D2
		bool IFlowDocumentFormatter.IsLayoutDataValid
		{
			get
			{
				return !this._document.StructuralCache.IsContentChangeInProgress;
			}
		}

		// Token: 0x04000AA6 RID: 2726
		private readonly FlowDocument _document;

		// Token: 0x04000AA7 RID: 2727
		private readonly FlowDocumentPaginator.CustomDispatcherObject _dispatcherObject;

		// Token: 0x04000AA8 RID: 2728
		private readonly BreakRecordTable _brt;

		// Token: 0x04000AA9 RID: 2729
		private Size _pageSize;

		// Token: 0x04000AAA RID: 2730
		private bool _backgroundPagination;

		// Token: 0x04000AAB RID: 2731
		private const int _paginationTimeout = 30;

		// Token: 0x04000AAC RID: 2732
		private static Size _defaultPageSize = new Size(816.0, 1056.0);

		// Token: 0x04000AAD RID: 2733
		private List<FlowDocumentPaginator.AsyncRequest> _asyncRequests = new List<FlowDocumentPaginator.AsyncRequest>(0);

		// Token: 0x04000AAE RID: 2734
		private DispatcherOperation _backgroundPaginationOperation;

		// Token: 0x020009DB RID: 2523
		private abstract class AsyncRequest
		{
			// Token: 0x06008413 RID: 33811 RVA: 0x00324F86 File Offset: 0x00323F86
			internal AsyncRequest(object userState, FlowDocumentPaginator paginator)
			{
				this.UserState = userState;
				this.Paginator = paginator;
			}

			// Token: 0x06008414 RID: 33812
			internal abstract void Cancel();

			// Token: 0x06008415 RID: 33813
			internal abstract bool Process();

			// Token: 0x04003FDC RID: 16348
			internal readonly object UserState;

			// Token: 0x04003FDD RID: 16349
			protected readonly FlowDocumentPaginator Paginator;
		}

		// Token: 0x020009DC RID: 2524
		private class GetPageAsyncRequest : FlowDocumentPaginator.AsyncRequest
		{
			// Token: 0x06008416 RID: 33814 RVA: 0x00324F9C File Offset: 0x00323F9C
			internal GetPageAsyncRequest(int pageNumber, object userState, FlowDocumentPaginator paginator) : base(userState, paginator)
			{
				this.PageNumber = pageNumber;
			}

			// Token: 0x06008417 RID: 33815 RVA: 0x00324FAD File Offset: 0x00323FAD
			internal override void Cancel()
			{
				this.Paginator.OnGetPageCompleted(new GetPageCompletedEventArgs(null, this.PageNumber, null, true, this.UserState));
			}

			// Token: 0x06008418 RID: 33816 RVA: 0x00324FD0 File Offset: 0x00323FD0
			internal override bool Process()
			{
				if (!this.Paginator._brt.HasPageBreakRecord(this.PageNumber))
				{
					return false;
				}
				DocumentPage page = this.Paginator.FormatPage(this.PageNumber);
				this.Paginator.OnGetPageCompleted(new GetPageCompletedEventArgs(page, this.PageNumber, null, false, this.UserState));
				return true;
			}

			// Token: 0x04003FDE RID: 16350
			internal readonly int PageNumber;
		}

		// Token: 0x020009DD RID: 2525
		private class GetPageNumberAsyncRequest : FlowDocumentPaginator.AsyncRequest
		{
			// Token: 0x06008419 RID: 33817 RVA: 0x00325029 File Offset: 0x00324029
			internal GetPageNumberAsyncRequest(TextPointer textPointer, object userState, FlowDocumentPaginator paginator) : base(userState, paginator)
			{
				this.TextPointer = textPointer;
			}

			// Token: 0x0600841A RID: 33818 RVA: 0x0032503A File Offset: 0x0032403A
			internal override void Cancel()
			{
				this.Paginator.OnGetPageNumberCompleted(new GetPageNumberCompletedEventArgs(this.TextPointer, -1, null, true, this.UserState));
			}

			// Token: 0x0600841B RID: 33819 RVA: 0x0032505C File Offset: 0x0032405C
			internal override bool Process()
			{
				int pageNumber = 0;
				if (!this.Paginator._brt.GetPageNumberForContentPosition(this.TextPointer, ref pageNumber))
				{
					return false;
				}
				this.Paginator.OnGetPageNumberCompleted(new GetPageNumberCompletedEventArgs(this.TextPointer, pageNumber, null, false, this.UserState));
				return true;
			}

			// Token: 0x04003FDF RID: 16351
			internal readonly TextPointer TextPointer;
		}

		// Token: 0x020009DE RID: 2526
		private class CustomDispatcherObject : DispatcherObject
		{
		}
	}
}
