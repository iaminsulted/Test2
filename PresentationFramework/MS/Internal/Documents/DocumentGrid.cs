using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Utility;

namespace MS.Internal.Documents
{
	// Token: 0x020001BA RID: 442
	internal class DocumentGrid : FrameworkElement, IDocumentScrollInfo, IScrollInfo
	{
		// Token: 0x06000E7E RID: 3710 RVA: 0x00139447 File Offset: 0x00138447
		static DocumentGrid()
		{
			EventManager.RegisterClassHandler(typeof(DocumentGrid), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(DocumentGrid.OnRequestBringIntoView));
			DocumentGridContextMenu.RegisterClassHandler();
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00139470 File Offset: 0x00138470
		public DocumentGrid()
		{
			this.Initialize();
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x001394D0 File Offset: 0x001384D0
		internal DocumentPage GetDocumentPageFromPoint(Point point)
		{
			DocumentPageView documentPageViewFromPoint = this.GetDocumentPageViewFromPoint(point);
			if (documentPageViewFromPoint != null)
			{
				return documentPageViewFromPoint.DocumentPage;
			}
			return null;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x001394F0 File Offset: 0x001384F0
		public void LineUp()
		{
			if (this._canVerticallyScroll)
			{
				this.SetVerticalOffsetInternal(this.VerticalOffset - 16.0);
			}
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00139510 File Offset: 0x00138510
		public void LineDown()
		{
			if (this._canVerticallyScroll)
			{
				this.SetVerticalOffsetInternal(this.VerticalOffset + 16.0);
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLineDown);
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0013953C File Offset: 0x0013853C
		public void LineLeft()
		{
			if (this._canHorizontallyScroll)
			{
				this.SetHorizontalOffsetInternal(this.HorizontalOffset - 16.0);
			}
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0013955C File Offset: 0x0013855C
		public void LineRight()
		{
			if (this._canHorizontallyScroll)
			{
				this.SetHorizontalOffsetInternal(this.HorizontalOffset + 16.0);
			}
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0013957C File Offset: 0x0013857C
		public void PageUp()
		{
			this.SetVerticalOffsetInternal(this.VerticalOffset - this.ViewportHeight);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00139591 File Offset: 0x00138591
		public void PageDown()
		{
			this.SetVerticalOffsetInternal(this.VerticalOffset + this.ViewportHeight);
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageDown, (int)this.VerticalOffset);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x001395BE File Offset: 0x001385BE
		public void PageLeft()
		{
			this.SetHorizontalOffsetInternal(this.HorizontalOffset - this.ViewportWidth);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x001395D3 File Offset: 0x001385D3
		public void PageRight()
		{
			this.SetHorizontalOffsetInternal(this.HorizontalOffset + this.ViewportWidth);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x001395E8 File Offset: 0x001385E8
		public void MouseWheelUp()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				this.SetVerticalOffsetInternal(this.VerticalOffset - this.MouseWheelVerticalScrollAmount);
				return;
			}
			this.PageUp();
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0013960C File Offset: 0x0013860C
		public void MouseWheelDown()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				this.SetVerticalOffsetInternal(this.VerticalOffset + this.MouseWheelVerticalScrollAmount);
				return;
			}
			this.PageDown();
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00139630 File Offset: 0x00138630
		public void MouseWheelLeft()
		{
			if (this.CanMouseWheelHorizontallyScroll)
			{
				this.SetHorizontalOffsetInternal(this.HorizontalOffset - this.MouseWheelHorizontalScrollAmount);
				return;
			}
			this.PageLeft();
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00139654 File Offset: 0x00138654
		public void MouseWheelRight()
		{
			if (this.CanMouseWheelHorizontallyScroll)
			{
				this.SetHorizontalOffsetInternal(this.HorizontalOffset + this.MouseWheelHorizontalScrollAmount);
				return;
			}
			this.PageRight();
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00139678 File Offset: 0x00138678
		public Rect MakeVisible(Visual v, Rect r)
		{
			if (this.Content != null && v != null)
			{
				ContentPosition objectPosition = this.Content.GetObjectPosition(v);
				this.MakeContentPositionVisibleAsync(new DocumentGrid.MakeVisibleData(v, objectPosition, r));
			}
			return r;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x001396AC File Offset: 0x001386AC
		public Rect MakeVisible(object o, Rect r, int pageNumber)
		{
			ContentPosition objectPosition = this.Content.GetObjectPosition(o);
			this.MakeVisibleAsync(new DocumentGrid.MakeVisibleData(o as Visual, objectPosition, r), pageNumber);
			return r;
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x001396DC File Offset: 0x001386DC
		public void MakeSelectionVisible()
		{
			if (this.TextEditor != null && this.TextEditor.Selection != null)
			{
				ContentPosition contentPosition = this.TextEditor.Selection.Start.CreatePointer(LogicalDirection.Forward) as ContentPosition;
				this.MakeContentPositionVisibleAsync(new DocumentGrid.MakeVisibleData(null, contentPosition, Rect.Empty));
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0013972C File Offset: 0x0013872C
		public void MakePageVisible(int pageNumber)
		{
			if (Math.Abs(pageNumber - this._firstVisiblePageNumber) > 1)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageJump, this._firstVisiblePageNumber, pageNumber);
			}
			if (pageNumber < 0)
			{
				this.SetVerticalOffsetInternal(0.0);
				this.SetHorizontalOffsetInternal(0.0);
				return;
			}
			if (pageNumber < this._pageCache.PageCount && this._rowCache.RowCount != 0)
			{
				RowInfo rowForPageNumber = this._rowCache.GetRowForPageNumber(pageNumber);
				this.SetVerticalOffsetInternal(rowForPageNumber.VerticalOffset);
				double horizontalOffsetForPage = this.GetHorizontalOffsetForPage(rowForPageNumber, pageNumber);
				this.SetHorizontalOffsetInternal(horizontalOffsetForPage);
				return;
			}
			if (this._pageCache.IsPaginationCompleted && this._rowCache.HasValidLayout)
			{
				this.SetVerticalOffsetInternal(this.ExtentHeight);
				this.SetHorizontalOffsetInternal(this.ExtentWidth);
				return;
			}
			this._pageJumpAfterLayout = true;
			this._pageJumpAfterLayoutPageNumber = pageNumber;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00139810 File Offset: 0x00138810
		public void ScrollToNextRow()
		{
			int num = this._firstVisibleRow + 1;
			if (num < this._rowCache.RowCount)
			{
				RowInfo row = this._rowCache.GetRow(num);
				this.SetVerticalOffsetInternal(row.VerticalOffset);
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00139850 File Offset: 0x00138850
		public void ScrollToPreviousRow()
		{
			int num = this._firstVisibleRow - 1;
			if (num >= 0 && num < this._rowCache.RowCount)
			{
				RowInfo row = this._rowCache.GetRow(num);
				this.SetVerticalOffsetInternal(row.VerticalOffset);
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00139891 File Offset: 0x00138891
		public void ScrollToHome()
		{
			this.SetVerticalOffsetInternal(0.0);
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x001398A2 File Offset: 0x001388A2
		public void ScrollToEnd()
		{
			this.SetVerticalOffsetInternal(this.ExtentHeight);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x001398B0 File Offset: 0x001388B0
		public void SetScale(double scale)
		{
			if (!DoubleUtil.AreClose(scale, this.Scale))
			{
				if (scale <= 0.0)
				{
					throw new ArgumentOutOfRangeException("scale");
				}
				if (!Helper.IsDoubleValid(scale))
				{
					throw new ArgumentOutOfRangeException("scale");
				}
				this.QueueSetScale(scale);
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x001398FC File Offset: 0x001388FC
		public void SetColumns(int columns)
		{
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(columns, DocumentGrid.ViewMode.SetColumns));
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00139926 File Offset: 0x00138926
		public void FitColumns(int columns)
		{
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(columns, DocumentGrid.ViewMode.FitColumns));
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00139950 File Offset: 0x00138950
		public void FitToPageWidth()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(1, DocumentGrid.ViewMode.PageWidth));
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0013996B File Offset: 0x0013896B
		public void FitToPageHeight()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(1, DocumentGrid.ViewMode.PageHeight));
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00139986 File Offset: 0x00138986
		public void ViewThumbnails()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(1, DocumentGrid.ViewMode.Thumbnails));
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x001399A1 File Offset: 0x001389A1
		// (set) Token: 0x06000E9C RID: 3740 RVA: 0x001399A9 File Offset: 0x001389A9
		public bool CanHorizontallyScroll
		{
			get
			{
				return this._canHorizontallyScroll;
			}
			set
			{
				this._canHorizontallyScroll = value;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x001399B2 File Offset: 0x001389B2
		// (set) Token: 0x06000E9E RID: 3742 RVA: 0x001399BA File Offset: 0x001389BA
		public bool CanVerticallyScroll
		{
			get
			{
				return this._canVerticallyScroll;
			}
			set
			{
				this._canVerticallyScroll = value;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x001399C3 File Offset: 0x001389C3
		public double ExtentWidth
		{
			get
			{
				return this._rowCache.ExtentWidth;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x001399D0 File Offset: 0x001389D0
		public double ExtentHeight
		{
			get
			{
				return this._rowCache.ExtentHeight;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x001399DD File Offset: 0x001389DD
		public double ViewportWidth
		{
			get
			{
				return this._viewportWidth;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x001399E5 File Offset: 0x001389E5
		public double ViewportHeight
		{
			get
			{
				return this._viewportHeight;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x001399ED File Offset: 0x001389ED
		public double HorizontalOffset
		{
			get
			{
				return Math.Max(Math.Min(this._horizontalOffset, this.ExtentWidth - this.ViewportWidth), 0.0);
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00139A15 File Offset: 0x00138A15
		public void SetHorizontalOffset(double offset)
		{
			if (!DoubleUtil.AreClose(this._horizontalOffset, offset))
			{
				if (double.IsNaN(offset))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (this._documentLayoutsPending == 0)
				{
					this.SetHorizontalOffsetInternal(offset);
					return;
				}
				this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(offset, DocumentGrid.ViewMode.SetHorizontalOffset));
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x00139A55 File Offset: 0x00138A55
		public double VerticalOffset
		{
			get
			{
				return Math.Max(Math.Min(this._verticalOffset, this.ExtentHeight - this.ViewportHeight), 0.0);
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00139A7D File Offset: 0x00138A7D
		public void SetVerticalOffset(double offset)
		{
			if (!DoubleUtil.AreClose(this._verticalOffset, offset))
			{
				if (double.IsNaN(offset))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (this._documentLayoutsPending == 0)
				{
					this.SetVerticalOffsetInternal(offset);
					return;
				}
				this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(offset, DocumentGrid.ViewMode.SetVerticalOffset));
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x00139ABD File Offset: 0x00138ABD
		// (set) Token: 0x06000EA8 RID: 3752 RVA: 0x00139ACC File Offset: 0x00138ACC
		public DynamicDocumentPaginator Content
		{
			get
			{
				return this._pageCache.Content;
			}
			set
			{
				if (value != this._pageCache.Content)
				{
					this._textContainer = null;
					if (this._pageCache.Content != null)
					{
						this._pageCache.Content.GetPageNumberCompleted -= this.OnGetPageNumberCompleted;
					}
					if (this.ScrollOwner != null)
					{
						this.ScrollOwner.ScrollChanged -= new ScrollChangedEventHandler(this.OnScrollChanged);
						this._scrollChangedEventAttached = false;
					}
					this._pageCache.Content = value;
					if (this._pageCache.Content != null)
					{
						this._pageCache.Content.GetPageNumberCompleted += this.OnGetPageNumberCompleted;
					}
					this.ResetVisualTree(false);
					this.ResetPageViewCollection();
					this._firstVisiblePageNumber = 0;
					this._lastVisiblePageNumber = 0;
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageVisible, this._firstVisiblePageNumber, this._lastVisiblePageNumber);
					this._lastRowChangeExtentWidth = 0.0;
					this._lastRowChangeVerticalOffset = 0.0;
					if (this._documentLayout.ViewMode == DocumentGrid.ViewMode.Thumbnails)
					{
						this._documentLayout.ViewMode = DocumentGrid.ViewMode.SetColumns;
					}
					this.QueueUpdateDocumentLayout(this._documentLayout);
					base.InvalidateMeasure();
					this.InvalidateDocumentScrollInfo();
				}
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x00139C02 File Offset: 0x00138C02
		public int PageCount
		{
			get
			{
				return this._pageCache.PageCount;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00139C0F File Offset: 0x00138C0F
		public int FirstVisiblePageNumber
		{
			get
			{
				return this._firstVisiblePageNumber;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x00139C17 File Offset: 0x00138C17
		public double Scale
		{
			get
			{
				return this._rowCache.Scale;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00139C24 File Offset: 0x00138C24
		public int MaxPagesAcross
		{
			get
			{
				return this._maxPagesAcross;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x00139C2C File Offset: 0x00138C2C
		// (set) Token: 0x06000EAE RID: 3758 RVA: 0x00139C39 File Offset: 0x00138C39
		public double VerticalPageSpacing
		{
			get
			{
				return this._rowCache.VerticalPageSpacing;
			}
			set
			{
				if (!Helper.IsDoubleValid(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._rowCache.VerticalPageSpacing = value;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x00139C5A File Offset: 0x00138C5A
		// (set) Token: 0x06000EB0 RID: 3760 RVA: 0x00139C67 File Offset: 0x00138C67
		public double HorizontalPageSpacing
		{
			get
			{
				return this._rowCache.HorizontalPageSpacing;
			}
			set
			{
				if (!Helper.IsDoubleValid(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._rowCache.HorizontalPageSpacing = value;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x00139C88 File Offset: 0x00138C88
		// (set) Token: 0x06000EB2 RID: 3762 RVA: 0x00139C90 File Offset: 0x00138C90
		public bool ShowPageBorders
		{
			get
			{
				return this._showPageBorders;
			}
			set
			{
				if (this._showPageBorders != value)
				{
					this._showPageBorders = value;
					int count = this._childrenCollection.Count;
					for (int i = 0; i < count; i++)
					{
						DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
						if (documentGridPage != null)
						{
							documentGridPage.ShowPageBorders = this._showPageBorders;
						}
					}
				}
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x00139CE6 File Offset: 0x00138CE6
		// (set) Token: 0x06000EB4 RID: 3764 RVA: 0x00139CEE File Offset: 0x00138CEE
		public bool LockViewModes
		{
			get
			{
				return this._lockViewModes;
			}
			set
			{
				this._lockViewModes = value;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x00139CF8 File Offset: 0x00138CF8
		public ITextContainer TextContainer
		{
			get
			{
				if (this._textContainer == null && this.Content != null)
				{
					IServiceProvider serviceProvider = this.Content as IServiceProvider;
					if (serviceProvider != null)
					{
						this._textContainer = (ITextContainer)serviceProvider.GetService(typeof(ITextContainer));
					}
				}
				return this._textContainer;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x00139D45 File Offset: 0x00138D45
		public ITextView TextView
		{
			get
			{
				if (this.TextEditor != null)
				{
					return this.TextEditor.TextView;
				}
				return null;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x00139D5C File Offset: 0x00138D5C
		public ReadOnlyCollection<DocumentPageView> PageViews
		{
			get
			{
				return this._pageViews;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x00139D64 File Offset: 0x00138D64
		// (set) Token: 0x06000EB9 RID: 3769 RVA: 0x00139D6C File Offset: 0x00138D6C
		public ScrollViewer ScrollOwner
		{
			get
			{
				return this._scrollOwner;
			}
			set
			{
				this._scrollOwner = value;
				this.InvalidateDocumentScrollInfo();
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x00139D7B File Offset: 0x00138D7B
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x00139D83 File Offset: 0x00138D83
		public DocumentViewer DocumentViewerOwner
		{
			get
			{
				return this._documentViewerOwner;
			}
			set
			{
				this._documentViewerOwner = value;
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00139D8C File Offset: 0x00138D8C
		protected override Visual GetVisualChild(int index)
		{
			if (this._childrenCollection == null || index < 0 || index >= this._childrenCollection.Count)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._childrenCollection[index];
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x00139DDA File Offset: 0x00138DDA
		protected override int VisualChildrenCount
		{
			get
			{
				return this._childrenCollection.Count;
			}
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00139DE8 File Offset: 0x00138DE8
		protected override Size MeasureOverride(Size constraint)
		{
			if (double.IsInfinity(constraint.Width) || double.IsInfinity(constraint.Height))
			{
				constraint = this._defaultConstraint;
			}
			this.RecalculateVisualPages(this.VerticalOffset, constraint);
			int count = this._childrenCollection.Count;
			for (int i = 0; i < count; i++)
			{
				if (i == 0)
				{
					Border border = this._childrenCollection[i] as Border;
					if (border == null)
					{
						throw new InvalidOperationException(SR.Get("DocumentGridVisualTreeContainsNonBorderAsFirstElement"));
					}
					border.Measure(constraint);
				}
				else
				{
					DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
					if (documentGridPage == null)
					{
						throw new InvalidOperationException(SR.Get("DocumentGridVisualTreeContainsNonDocumentGridPage"));
					}
					Size pageSize = this._pageCache.GetPageSize(documentGridPage.PageNumber);
					pageSize.Width *= this.Scale;
					pageSize.Height *= this.Scale;
					if (!documentGridPage.IsMeasureValid)
					{
						documentGridPage.Measure(pageSize);
						Size pageSize2 = this._pageCache.GetPageSize(documentGridPage.PageNumber);
						if (pageSize2 != Size.Empty)
						{
							pageSize2.Width *= this.Scale;
							pageSize2.Height *= this.Scale;
							if (pageSize2.Width != pageSize.Width || pageSize2.Height != pageSize.Height)
							{
								documentGridPage.Measure(pageSize2);
							}
						}
					}
				}
			}
			return constraint;
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00139F58 File Offset: 0x00138F58
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			if (this._viewportHeight != arrangeSize.Height || this._viewportWidth != arrangeSize.Width)
			{
				this._viewportWidth = arrangeSize.Width;
				this._viewportHeight = arrangeSize.Height;
				if (this.LockViewModes && this.IsViewLoaded() && this._firstVisiblePageNumber < this._pageCache.PageCount && this._rowCache.HasValidLayout)
				{
					this.ApplyViewParameters(this._rowCache.GetRowForPageNumber(this._firstVisiblePageNumber));
					this.MeasureOverride(arrangeSize);
				}
				this.UpdateTextView();
			}
			if (this.IsViewportNonzero && this.ExecutePendingLayoutRequests())
			{
				this.MeasureOverride(arrangeSize);
			}
			if (this._previousConstraint != arrangeSize)
			{
				this._previousConstraint = arrangeSize;
				this.InvalidateDocumentScrollInfo();
			}
			if (this._childrenCollection.Count == 0)
			{
				return arrangeSize;
			}
			(this._childrenCollection[0] as UIElement).Arrange(new Rect(new Point(0.0, 0.0), arrangeSize));
			int num = 1;
			for (int i = this._firstVisibleRow; i < this._firstVisibleRow + this._visibleRowCount; i++)
			{
				double num2;
				double y;
				this.CalculateRowOffsets(i, out num2, out y);
				RowInfo row = this._rowCache.GetRow(i);
				for (int j = row.FirstPage; j < row.FirstPage + row.PageCount; j++)
				{
					if (num > this._childrenCollection.Count - 1)
					{
						throw new InvalidOperationException(SR.Get("DocumentGridVisualTreeOutOfSync"));
					}
					Size pageSize = this._pageCache.GetPageSize(j);
					pageSize.Width *= this.Scale;
					pageSize.Height *= this.Scale;
					UIElement uielement = this._childrenCollection[num] as UIElement;
					if (uielement == null)
					{
						throw new InvalidOperationException(SR.Get("DocumentGridVisualTreeContainsNonUIElement"));
					}
					Point location;
					if (this._pageCache.IsContentRightToLeft)
					{
						location = new Point(Math.Max(this.ViewportWidth, this.ExtentWidth) - (num2 + pageSize.Width), y);
					}
					else
					{
						location = new Point(num2, y);
					}
					uielement.Arrange(new Rect(location, pageSize));
					num2 += pageSize.Width + this.HorizontalPageSpacing;
					num++;
				}
			}
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
			if (adornerLayer != null && adornerLayer.GetAdorners(this) != null)
			{
				adornerLayer.Update(this);
			}
			return arrangeSize;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0013A1D0 File Offset: 0x001391D0
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			bool flag = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
			if (flag && this._rubberBandSelector == null)
			{
				IServiceProvider serviceProvider = this.Content as IServiceProvider;
				if (serviceProvider != null)
				{
					this._rubberBandSelector = (serviceProvider.GetService(typeof(RubberbandSelector)) as RubberbandSelector);
					if (this._rubberBandSelector != null)
					{
						this.DocumentViewerOwner.Focus();
						ITextRange selection = this.TextEditor.Selection;
						selection.Select(selection.Start, selection.Start);
						this.DocumentViewerOwner.IsSelectionEnabled = false;
						this._rubberBandSelector.AttachRubberbandSelector(this);
						return;
					}
				}
			}
			else if (!flag && this._rubberBandSelector != null)
			{
				if (this._rubberBandSelector != null)
				{
					this._rubberBandSelector.DetachRubberbandSelector();
					this._rubberBandSelector = null;
				}
				this.DocumentViewerOwner.IsSelectionEnabled = true;
			}
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0013A2A8 File Offset: 0x001392A8
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (VisualTreeHelper.GetParent(this) != null)
			{
				this.ResetVisualTree(false);
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0013A2C0 File Offset: 0x001392C0
		private void RecalculateVisualPages(double offset, Size constraint)
		{
			if (this._rowCache.RowCount == 0)
			{
				this.ResetVisualTree(false);
				this.ResetPageViewCollection();
				this._firstVisibleRow = 0;
				this._visibleRowCount = 0;
				this._firstVisiblePageNumber = 0;
				this._lastVisiblePageNumber = 0;
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageVisible, this._firstVisiblePageNumber, this._lastVisiblePageNumber);
				return;
			}
			int num = 0;
			int num2 = 0;
			this._rowCache.GetVisibleRowIndices(offset, offset + constraint.Height, out num, out num2);
			if (num2 == 0)
			{
				this.ResetVisualTree(false);
				this.ResetPageViewCollection();
				this._firstVisibleRow = 0;
				this._visibleRowCount = 0;
				this._firstVisiblePageNumber = 0;
				this._lastVisiblePageNumber = 0;
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageVisible, this._firstVisiblePageNumber, this._lastVisiblePageNumber);
				return;
			}
			int num3 = -1;
			int num4 = -1;
			if (this._childrenCollection.Count > 1)
			{
				DocumentGridPage documentGridPage = this._childrenCollection[1] as DocumentGridPage;
				num3 = ((documentGridPage != null) ? documentGridPage.PageNumber : -1);
				DocumentGridPage documentGridPage2 = this._childrenCollection[this._childrenCollection.Count - 1] as DocumentGridPage;
				num4 = ((documentGridPage2 != null) ? documentGridPage2.PageNumber : -1);
			}
			RowInfo row = this._rowCache.GetRow(num);
			this._firstVisiblePageNumber = row.FirstPage;
			RowInfo row2 = this._rowCache.GetRow(num + num2 - 1);
			this._lastVisiblePageNumber = row2.FirstPage + row2.PageCount - 1;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageVisible, this._firstVisiblePageNumber, this._lastVisiblePageNumber);
			this._firstVisibleRow = num;
			this._visibleRowCount = num2;
			if (this._firstVisiblePageNumber != num3 || this._lastVisiblePageNumber != num4)
			{
				ArrayList arrayList = new ArrayList();
				for (int i = this._firstVisibleRow; i < this._firstVisibleRow + this._visibleRowCount; i++)
				{
					RowInfo row3 = this._rowCache.GetRow(i);
					for (int j = row3.FirstPage; j < row3.FirstPage + row3.PageCount; j++)
					{
						if (j < num3 || j > num4 || this._childrenCollection.Count <= 1)
						{
							DocumentGridPage documentGridPage3 = new DocumentGridPage(this.Content);
							documentGridPage3.ShowPageBorders = this.ShowPageBorders;
							documentGridPage3.PageNumber = j;
							documentGridPage3.PageLoaded += this.OnPageLoaded;
							arrayList.Add(documentGridPage3);
						}
						else
						{
							arrayList.Add(this._childrenCollection[1 + j - Math.Max(0, num3)]);
						}
					}
				}
				this.ResetVisualTree(true);
				Collection<DocumentPageView> collection = new Collection<DocumentPageView>();
				DocumentGrid.VisualTreeModificationState visualTreeModificationState = DocumentGrid.VisualTreeModificationState.BeforeExisting;
				int num5 = 1;
				for (int k = 0; k < arrayList.Count; k++)
				{
					Visual visual = (Visual)arrayList[k];
					switch (visualTreeModificationState)
					{
					case DocumentGrid.VisualTreeModificationState.BeforeExisting:
						if (num5 < this._childrenCollection.Count && this._childrenCollection[num5] == visual)
						{
							visualTreeModificationState = DocumentGrid.VisualTreeModificationState.DuringExisting;
						}
						else
						{
							this._childrenCollection.Insert(num5, visual);
						}
						num5++;
						break;
					case DocumentGrid.VisualTreeModificationState.DuringExisting:
						if (num5 >= this._childrenCollection.Count || this._childrenCollection[num5] != visual)
						{
							visualTreeModificationState = DocumentGrid.VisualTreeModificationState.AfterExisting;
							this._childrenCollection.Add(visual);
						}
						num5++;
						break;
					case DocumentGrid.VisualTreeModificationState.AfterExisting:
						this._childrenCollection.Add(visual);
						break;
					}
					collection.Add(((DocumentGridPage)arrayList[k]).DocumentPageView);
				}
				this._pageViews = new ReadOnlyCollection<DocumentPageView>(collection);
				this.InvalidatePageViews();
				this.InvalidateDocumentScrollInfo();
			}
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0013A668 File Offset: 0x00139668
		private void OnPageLoaded(object sender, EventArgs args)
		{
			DocumentGridPage documentGridPage = sender as DocumentGridPage;
			Invariant.Assert(documentGridPage != null, "Invalid sender for OnPageLoaded event.");
			documentGridPage.PageLoaded -= this.OnPageLoaded;
			if (this._makeVisiblePageNeeded == documentGridPage.PageNumber)
			{
				this._makeVisiblePageNeeded = -1;
				this._makeVisibleDispatcher.Priority = DispatcherPriority.Background;
			}
			if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordXPS, EventTrace.Level.Info))
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientDRXPageLoaded, EventTrace.Keyword.KeywordXPS, EventTrace.Level.Info, documentGridPage.PageNumber);
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0013A6E8 File Offset: 0x001396E8
		private void CalculateRowOffsets(int row, out double xOffset, out double yOffset)
		{
			xOffset = 0.0;
			yOffset = 0.0;
			RowInfo row2 = this._rowCache.GetRow(row);
			double num = Math.Max(this.ViewportWidth, this.ExtentWidth);
			if (row == this._rowCache.RowCount - 1 && !this._pageCache.DynamicPageSizes)
			{
				xOffset = (num - this.ExtentWidth) / 2.0 + this.HorizontalPageSpacing / 2.0 - this.HorizontalOffset;
			}
			else
			{
				xOffset = (num - row2.RowSize.Width) / 2.0 + this.HorizontalPageSpacing / 2.0 - this.HorizontalOffset;
			}
			if (this.ExtentHeight > this.ViewportHeight)
			{
				yOffset = row2.VerticalOffset + this.VerticalPageSpacing / 2.0 - this.VerticalOffset;
				return;
			}
			yOffset = row2.VerticalOffset + (this.ViewportHeight - this.ExtentHeight) / 2.0 + this.VerticalPageSpacing / 2.0;
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0013A80C File Offset: 0x0013980C
		private void ResetVisualTree(bool pruneOnly)
		{
			for (int i = this._childrenCollection.Count - 1; i >= 1; i--)
			{
				DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
				if (documentGridPage != null && (!pruneOnly || this._rowCache.RowCount == 0 || documentGridPage.PageNumber < this._firstVisiblePageNumber || documentGridPage.PageNumber > this._lastVisiblePageNumber))
				{
					this._childrenCollection.Remove(documentGridPage);
					documentGridPage.PageLoaded -= this.OnPageLoaded;
					((IDisposable)documentGridPage).Dispose();
				}
			}
			if (this._documentGridBackground == null)
			{
				this._documentGridBackground = new Border();
				this._documentGridBackground.Background = Brushes.Transparent;
				this._childrenCollection.Add(this._documentGridBackground);
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0013A8CB File Offset: 0x001398CB
		private void ResetPageViewCollection()
		{
			this._pageViews = null;
			this.InvalidatePageViews();
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0013A8DC File Offset: 0x001398DC
		private void OnGetPageNumberCompleted(object sender, GetPageNumberCompletedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (e.UserState is DocumentGrid.MakeVisibleData)
			{
				DocumentGrid.MakeVisibleData data = (DocumentGrid.MakeVisibleData)e.UserState;
				this.MakeVisibleAsync(data, e.PageNumber);
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0013A91D File Offset: 0x0013991D
		private void MakeVisibleAsync(DocumentGrid.MakeVisibleData data, int pageNumber)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DocumentGrid.BringPageIntoViewCallback(this.BringPageIntoViewDelegate), data, new object[]
			{
				pageNumber
			});
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0013A950 File Offset: 0x00139950
		private void BringPageIntoViewDelegate(DocumentGrid.MakeVisibleData data, int pageNumber)
		{
			if (!this._rowCache.HasValidLayout || (data.Visual is FixedPage && data.Visual.VisualContentBounds == data.Rect) || pageNumber < this._firstVisiblePageNumber || pageNumber > this._lastVisiblePageNumber)
			{
				this.MakePageVisible(pageNumber);
			}
			if (this.IsPageLoaded(pageNumber))
			{
				this.MakeVisibleImpl(data);
				return;
			}
			this._makeVisiblePageNeeded = pageNumber;
			this._makeVisibleDispatcher = base.Dispatcher.BeginInvoke(DispatcherPriority.Inactive, new DispatcherOperationCallback(delegate(object arg)
			{
				this.MakeVisibleImpl((DocumentGrid.MakeVisibleData)arg);
				return null;
			}), data);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0013A9E8 File Offset: 0x001399E8
		private void MakeVisibleImpl(DocumentGrid.MakeVisibleData data)
		{
			if (data.Visual != null)
			{
				if (base.IsAncestorOf(data.Visual))
				{
					GeneralTransform generalTransform = data.Visual.TransformToAncestor(this);
					Rect rect = (data.Rect != Rect.Empty) ? data.Rect : data.Visual.VisualContentBounds;
					Rect r = generalTransform.TransformBounds(rect);
					this.MakeRectVisible(r, false);
					return;
				}
			}
			else if (data.ContentPosition != null)
			{
				ITextPointer textPointer = data.ContentPosition as ITextPointer;
				if (this.TextViewContains(textPointer))
				{
					this.MakeRectVisible(this.TextView.GetRectangleFromTextPosition(textPointer), false);
					return;
				}
			}
			else
			{
				Invariant.Assert(false, "Invalid object brought into view.");
			}
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0013AA94 File Offset: 0x00139A94
		private void MakeRectVisible(Rect r, bool alwaysCenter)
		{
			if (r != Rect.Empty)
			{
				Rect rect = new Rect(this.HorizontalOffset + r.X, this.VerticalOffset + r.Y, r.Width, r.Height);
				Rect rect2 = new Rect(this.HorizontalOffset, this.VerticalOffset, this.ViewportWidth, this.ViewportHeight);
				if (alwaysCenter || !rect.IntersectsWith(rect2))
				{
					this.SetHorizontalOffsetInternal(rect.X - this.ViewportWidth / 2.0);
					this.SetVerticalOffsetInternal(rect.Y - this.ViewportHeight / 2.0);
				}
			}
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0013AB4C File Offset: 0x00139B4C
		private void MakeIPVisible(Rect r)
		{
			if (r != Rect.Empty && this.TextEditor != null)
			{
				Rect rect = new Rect(this.HorizontalOffset, this.VerticalOffset, this.ViewportWidth, this.ViewportHeight);
				if (!rect.Contains(r))
				{
					if (r.X < this.HorizontalOffset)
					{
						this.SetHorizontalOffsetInternal(this.HorizontalOffset - (this.HorizontalOffset - r.X));
					}
					else if (r.X > this.HorizontalOffset + this.ViewportWidth)
					{
						this.SetHorizontalOffsetInternal(this.HorizontalOffset + (r.X - (this.HorizontalOffset + this.ViewportWidth)));
					}
					if (r.Y < this.VerticalOffset)
					{
						this.SetVerticalOffsetInternal(this.VerticalOffset - (this.VerticalOffset - r.Y));
						return;
					}
					if (r.Y + r.Height > this.VerticalOffset + this.ViewportHeight)
					{
						this.SetVerticalOffsetInternal(this.VerticalOffset + (r.Y + r.Height - (this.VerticalOffset + this.ViewportHeight)));
					}
				}
			}
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0013AC77 File Offset: 0x00139C77
		private void MakeContentPositionVisibleAsync(DocumentGrid.MakeVisibleData data)
		{
			if (data.ContentPosition != null && data.ContentPosition != ContentPosition.Missing)
			{
				this.Content.GetPageNumberAsync(data.ContentPosition, data);
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0013ACA8 File Offset: 0x00139CA8
		private void QueueSetScale(double scale)
		{
			if (this._setScaleOperation != null && this._setScaleOperation.Status == DispatcherOperationStatus.Pending)
			{
				this._setScaleOperation.Abort();
			}
			this._setScaleOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.SetScaleDelegate), scale);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0013ACFC File Offset: 0x00139CFC
		private object SetScaleDelegate(object scale)
		{
			if (!(scale is double))
			{
				return null;
			}
			double scale2 = (double)scale;
			this._documentLayout.ViewMode = DocumentGrid.ViewMode.Zoom;
			ITextPointer visibleSelection = this.GetVisibleSelection();
			if (visibleSelection != null)
			{
				int pageNumberForVisibleSelection = this.GetPageNumberForVisibleSelection(visibleSelection);
				this.UpdateLayoutScale(scale2);
				this.MakePageVisible(pageNumberForVisibleSelection);
				base.LayoutUpdated += this.OnZoomLayoutUpdated;
			}
			else
			{
				this.UpdateLayoutScale(scale2);
			}
			return null;
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0013AD64 File Offset: 0x00139D64
		private void UpdateLayoutScale(double scale)
		{
			if (!DoubleUtil.AreClose(scale, this.Scale))
			{
				double extentHeight = this.ExtentHeight;
				double extentWidth = this.ExtentWidth;
				this._rowCache.Scale = scale;
				double num = (extentHeight == 0.0) ? 1.0 : (this.ExtentHeight / extentHeight);
				double num2 = (extentWidth == 0.0) ? 1.0 : (this.ExtentWidth / extentWidth);
				this.SetVerticalOffsetInternal(this._verticalOffset * num);
				this.SetHorizontalOffsetInternal(this._horizontalOffset * num2);
				base.InvalidateMeasure();
				this.InvalidateChildMeasure();
				this.InvalidateDocumentScrollInfo();
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0013AE0A File Offset: 0x00139E0A
		private void QueueUpdateDocumentLayout(DocumentGrid.DocumentLayout layout)
		{
			this._documentLayoutsPending++;
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.UpdateDocumentLayoutDelegate), layout);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0013AE33 File Offset: 0x00139E33
		private object UpdateDocumentLayoutDelegate(object layout)
		{
			if (layout is DocumentGrid.DocumentLayout)
			{
				this.UpdateDocumentLayout((DocumentGrid.DocumentLayout)layout);
			}
			this._documentLayoutsPending--;
			return null;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0013AE58 File Offset: 0x00139E58
		private void UpdateDocumentLayout(DocumentGrid.DocumentLayout layout)
		{
			if (layout.ViewMode == DocumentGrid.ViewMode.SetHorizontalOffset)
			{
				this.SetHorizontalOffsetInternal(layout.Offset);
				return;
			}
			if (layout.ViewMode == DocumentGrid.ViewMode.SetVerticalOffset)
			{
				this.SetVerticalOffsetInternal(layout.Offset);
				return;
			}
			this._documentLayout = layout;
			this._maxPagesAcross = this._documentLayout.Columns;
			if (this.IsViewportNonzero)
			{
				if (this._documentLayout.ViewMode == DocumentGrid.ViewMode.Thumbnails)
				{
					this._maxPagesAcross = (this._documentLayout.Columns = this.CalculateThumbnailColumns());
				}
				int activeFocusPage = this.GetActiveFocusPage();
				this._rowCache.RecalcRows(activeFocusPage, this._documentLayout.Columns);
				this._isLayoutRequested = false;
				return;
			}
			this._isLayoutRequested = true;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0013AF06 File Offset: 0x00139F06
		private bool ExecutePendingLayoutRequests()
		{
			if (this._isLayoutRequested)
			{
				this.UpdateDocumentLayout(this._documentLayout);
				return true;
			}
			return false;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0013AF1F File Offset: 0x00139F1F
		private void SetHorizontalOffsetInternal(double offset)
		{
			if (!DoubleUtil.AreClose(this._horizontalOffset, offset))
			{
				if (double.IsNaN(offset))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				this._horizontalOffset = offset;
				base.InvalidateMeasure();
				this.InvalidateDocumentScrollInfo();
				this.UpdateTextView();
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0013AF5B File Offset: 0x00139F5B
		private void SetVerticalOffsetInternal(double offset)
		{
			if (!DoubleUtil.AreClose(this._verticalOffset, offset))
			{
				if (double.IsNaN(offset))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				this._verticalOffset = offset;
				base.InvalidateMeasure();
				this.InvalidateDocumentScrollInfo();
				this.UpdateTextView();
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0013AF98 File Offset: 0x00139F98
		private void UpdateTextView()
		{
			MultiPageTextView multiPageTextView = this.TextView as MultiPageTextView;
			if (multiPageTextView != null)
			{
				multiPageTextView.OnPageLayoutChanged();
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0013AFBC File Offset: 0x00139FBC
		private int CalculateThumbnailColumns()
		{
			if (!this.IsViewportNonzero)
			{
				return 1;
			}
			if (this._pageCache.PageCount == 0)
			{
				return 1;
			}
			Size pageSize = this._pageCache.GetPageSize(0);
			double num = this.ViewportWidth / this.ViewportHeight;
			int num2 = (int)Math.Floor(this.ViewportWidth / (this.CurrentMinimumScale * pageSize.Width + this.HorizontalPageSpacing));
			num2 = Math.Min(num2, this._pageCache.PageCount);
			num2 = Math.Min(num2, DocumentViewerConstants.MaximumMaxPagesAcross);
			int result = 1;
			double num3 = double.MaxValue;
			for (int i = 1; i <= num2; i++)
			{
				int num4 = (int)Math.Floor((double)(this._pageCache.PageCount / i));
				double num5 = pageSize.Width * (double)i;
				double num6 = pageSize.Height * (double)num4;
				double num7 = Math.Abs(num5 / num6 - num);
				if (num7 < num3)
				{
					num3 = num7;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0013B0A4 File Offset: 0x0013A0A4
		private void InvalidateChildMeasure()
		{
			int count = this._childrenCollection.Count;
			for (int i = 0; i < count; i++)
			{
				UIElement uielement = this._childrenCollection[i] as UIElement;
				if (uielement != null)
				{
					uielement.InvalidateMeasure();
				}
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0013B0E4 File Offset: 0x0013A0E4
		private bool RowIsClean(RowInfo row)
		{
			bool result = true;
			for (int i = row.FirstPage; i < row.FirstPage + row.PageCount; i++)
			{
				if (this._pageCache.IsPageDirty(i))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0013B124 File Offset: 0x0013A124
		private void EnsureFit(RowInfo pivotRow)
		{
			double num = this.CalculateScaleFactor(pivotRow);
			double num2 = num * this._rowCache.Scale;
			if (num2 < this.CurrentMinimumScale || num2 > DocumentViewerConstants.MaximumScale)
			{
				return;
			}
			if (!DoubleUtil.AreClose(1.0, num))
			{
				this.ApplyViewParameters(pivotRow);
				this.SetVerticalOffsetInternal(pivotRow.VerticalOffset);
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0013B180 File Offset: 0x0013A180
		private void ApplyViewParameters(RowInfo pivotRow)
		{
			if (this._pageCache.DynamicPageSizes)
			{
				this._maxPagesAcross = pivotRow.PageCount;
			}
			double num = this.CalculateScaleFactor(pivotRow) * this._rowCache.Scale;
			num = Math.Max(num, this.CurrentMinimumScale);
			num = Math.Min(num, DocumentViewerConstants.MaximumScale);
			this.UpdateLayoutScale(num);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0013B1DC File Offset: 0x0013A1DC
		private double CalculateScaleFactor(RowInfo pivotRow)
		{
			double num;
			if (this._pageCache.DynamicPageSizes)
			{
				num = pivotRow.RowSize.Width - (double)pivotRow.PageCount * this.HorizontalPageSpacing;
			}
			else
			{
				num = this.ExtentWidth - (double)this.MaxPagesAcross * this.HorizontalPageSpacing;
			}
			double num2 = pivotRow.RowSize.Height - this.VerticalPageSpacing;
			if (num <= 0.0 || num2 <= 0.0)
			{
				return 1.0;
			}
			double num3;
			if (this._pageCache.DynamicPageSizes)
			{
				num3 = this.ViewportWidth - (double)pivotRow.PageCount * this.HorizontalPageSpacing;
			}
			else
			{
				num3 = this.ViewportWidth - (double)this.MaxPagesAcross * this.HorizontalPageSpacing;
			}
			double num4 = this.ViewportHeight - this.VerticalPageSpacing;
			if (num3 <= 0.0 || num4 <= 0.0)
			{
				return 1.0;
			}
			double result = 1.0;
			switch (this._documentLayout.ViewMode)
			{
			case DocumentGrid.ViewMode.SetColumns:
			case DocumentGrid.ViewMode.Zoom:
				break;
			case DocumentGrid.ViewMode.FitColumns:
				result = Math.Min(num3 / num, num4 / num2);
				break;
			case DocumentGrid.ViewMode.PageWidth:
				result = num3 / num;
				break;
			case DocumentGrid.ViewMode.PageHeight:
				result = num4 / num2;
				break;
			case DocumentGrid.ViewMode.Thumbnails:
			{
				double num5 = this.ExtentHeight - this.VerticalPageSpacing * (double)this._rowCache.RowCount;
				double num6 = this.ViewportHeight - this.VerticalPageSpacing * (double)this._rowCache.RowCount;
				if (num6 <= 0.0)
				{
					result = 1.0;
				}
				else
				{
					result = Math.Min(num3 / num, num6 / num5);
				}
				break;
			}
			default:
				throw new InvalidOperationException(SR.Get("DocumentGridInvalidViewMode"));
			}
			return result;
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0013B3A4 File Offset: 0x0013A3A4
		private void Initialize()
		{
			this._pageCache = new PageCache();
			this._childrenCollection = new VisualCollection(this);
			this._rowCache = new RowCache();
			this._rowCache.PageCache = this._pageCache;
			this._rowCache.RowCacheChanged += this.OnRowCacheChanged;
			this._rowCache.RowLayoutCompleted += this.OnRowLayoutCompleted;
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0013B412 File Offset: 0x0013A412
		private void InvalidateDocumentScrollInfo()
		{
			if (this.ScrollOwner != null)
			{
				this.ScrollOwner.InvalidateScrollInfo();
			}
			if (this.DocumentViewerOwner != null)
			{
				this.DocumentViewerOwner.InvalidateDocumentScrollInfo();
			}
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0013B43A File Offset: 0x0013A43A
		private void InvalidatePageViews()
		{
			Invariant.Assert(this.DocumentViewerOwner != null, "DocumentViewerOwner cannot be null.");
			if (this.DocumentViewerOwner != null)
			{
				this.DocumentViewerOwner.InvalidatePageViewsInternal();
				this.DocumentViewerOwner.ApplyTemplate();
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXInvalidateView);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0013B47C File Offset: 0x0013A47C
		private ITextPointer GetVisibleSelection()
		{
			ITextPointer result = null;
			if (this.HasSelection())
			{
				ITextPointer start = this.TextEditor.Selection.Start;
				if (this.TextViewContains(start))
				{
					result = start;
				}
			}
			return result;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0013B4B0 File Offset: 0x0013A4B0
		private bool HasSelection()
		{
			return this.TextEditor != null && this.TextEditor.Selection != null;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0013B4CC File Offset: 0x0013A4CC
		private int GetPageNumberForVisibleSelection(ITextPointer selection)
		{
			Invariant.Assert(this.TextViewContains(selection));
			foreach (DocumentPageView documentPageView in this._pageViews)
			{
				DocumentPageTextView documentPageTextView = ((IServiceProvider)documentPageView).GetService(typeof(ITextView)) as DocumentPageTextView;
				if (documentPageTextView != null && documentPageTextView.IsValid && documentPageTextView.Contains(selection))
				{
					return documentPageView.PageNumber;
				}
			}
			Invariant.Assert(false, "Selection was in TextView, but not found in any visible page!");
			return 0;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0013B560 File Offset: 0x0013A560
		private Point GetActiveFocusPoint()
		{
			ITextPointer visibleSelection = this.GetVisibleSelection();
			if (visibleSelection != null && visibleSelection.HasValidLayout)
			{
				Rect rectangleFromTextPosition = this.TextView.GetRectangleFromTextPosition(visibleSelection);
				if (rectangleFromTextPosition != Rect.Empty)
				{
					return new Point(rectangleFromTextPosition.Left, rectangleFromTextPosition.Top);
				}
			}
			return new Point(0.0, 0.0);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0013B5C4 File Offset: 0x0013A5C4
		private int GetActiveFocusPage()
		{
			DocumentPageView documentPageViewFromPoint = this.GetDocumentPageViewFromPoint(this.GetActiveFocusPoint());
			if (documentPageViewFromPoint != null)
			{
				return documentPageViewFromPoint.PageNumber;
			}
			return this._firstVisiblePageNumber;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0013B5F0 File Offset: 0x0013A5F0
		private DocumentPageView GetDocumentPageViewFromPoint(Point point)
		{
			HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, point);
			for (DependencyObject dependencyObject = (hitTestResult != null) ? hitTestResult.VisualHit : null; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
			{
				DocumentPageView documentPageView = dependencyObject as DocumentPageView;
				if (documentPageView != null)
				{
					return documentPageView;
				}
			}
			return null;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0013B62D File Offset: 0x0013A62D
		private bool TextViewContains(ITextPointer tp)
		{
			return this.TextView != null && this.TextView.IsValid && this.TextView.Contains(tp);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0013B654 File Offset: 0x0013A654
		private double GetHorizontalOffsetForPage(RowInfo row, int pageNumber)
		{
			if (row == null)
			{
				throw new ArgumentNullException("row");
			}
			if (pageNumber < row.FirstPage || pageNumber > row.FirstPage + row.PageCount)
			{
				throw new ArgumentOutOfRangeException("pageNumber");
			}
			double num = this._pageCache.DynamicPageSizes ? Math.Max(0.0, (this.ExtentWidth - row.RowSize.Width) / 2.0) : 0.0;
			for (int i = row.FirstPage; i < pageNumber; i++)
			{
				num += this._pageCache.GetPageSize(i).Width * this.Scale + this.HorizontalPageSpacing;
			}
			return num;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0013B710 File Offset: 0x0013A710
		private bool RowCacheChangeIsVisible(RowCacheChange change)
		{
			int firstVisibleRow = this._firstVisibleRow;
			int num = this._firstVisibleRow + this._visibleRowCount;
			int start = change.Start;
			int num2 = change.Start + change.Count;
			return (start >= firstVisibleRow && start <= num) || (num2 >= firstVisibleRow && num2 <= num) || (start < firstVisibleRow && num2 > num);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0013B764 File Offset: 0x0013A764
		private bool IsPageLoaded(int pageNumber)
		{
			DocumentGridPage documentGridPageForPageNumber = this.GetDocumentGridPageForPageNumber(pageNumber);
			return documentGridPageForPageNumber != null && documentGridPageForPageNumber.IsPageLoaded;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0013B784 File Offset: 0x0013A784
		private bool IsViewLoaded()
		{
			bool result = true;
			for (int i = 1; i < this._childrenCollection.Count; i++)
			{
				DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
				if (documentGridPage != null && !documentGridPage.IsPageLoaded)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0013B7CC File Offset: 0x0013A7CC
		private DocumentGridPage GetDocumentGridPageForPageNumber(int pageNumber)
		{
			for (int i = 1; i < this._childrenCollection.Count; i++)
			{
				DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
				if (documentGridPage != null && documentGridPage.PageNumber == pageNumber)
				{
					return documentGridPage;
				}
			}
			return null;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0013B810 File Offset: 0x0013A810
		private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs args)
		{
			DocumentGrid documentGrid = sender as DocumentGrid;
			DocumentGrid documentGrid2 = args.TargetObject as DocumentGrid;
			if (documentGrid != null && documentGrid2 != null && documentGrid == documentGrid2)
			{
				args.Handled = true;
				documentGrid2.MakeIPVisible(args.TargetRect);
				return;
			}
			args.Handled = false;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0013B858 File Offset: 0x0013A858
		private void OnScrollChanged(object sender, EventArgs args)
		{
			if (this.ScrollOwner != null)
			{
				this._scrollChangedEventAttached = false;
				this.ScrollOwner.ScrollChanged -= new ScrollChangedEventHandler(this.OnScrollChanged);
			}
			if (this._rowCache.HasValidLayout)
			{
				this.EnsureFit(this._rowCache.GetRowForPageNumber(this.FirstVisiblePageNumber));
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0013B8B0 File Offset: 0x0013A8B0
		private void OnZoomLayoutUpdated(object sender, EventArgs args)
		{
			base.LayoutUpdated -= this.OnZoomLayoutUpdated;
			ITextPointer visibleSelection = this.GetVisibleSelection();
			if (visibleSelection != null)
			{
				this.MakeRectVisible(this.TextView.GetRectangleFromTextPosition(visibleSelection), true);
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0013B8EC File Offset: 0x0013A8EC
		private void OnRowCacheChanged(object source, RowCacheChangedEventArgs args)
		{
			if (this._savedPivotRow != null && this.RowIsClean(this._savedPivotRow))
			{
				if (this._documentLayout.ViewMode != DocumentGrid.ViewMode.Zoom && this._documentLayout.ViewMode != DocumentGrid.ViewMode.SetColumns)
				{
					if (this._savedPivotRow.FirstPage < this._rowCache.RowCount)
					{
						RowInfo rowForPageNumber = this._rowCache.GetRowForPageNumber(this._savedPivotRow.FirstPage);
						if (rowForPageNumber.RowSize.Width != this._savedPivotRow.RowSize.Width || rowForPageNumber.RowSize.Height != this._savedPivotRow.RowSize.Height)
						{
							this.ApplyViewParameters(rowForPageNumber);
						}
						this._savedPivotRow = null;
					}
				}
				else
				{
					this._savedPivotRow = null;
				}
			}
			if (this._pageCache.DynamicPageSizes && this._lastRowChangeVerticalOffset != this.VerticalOffset && this._lastRowChangeExtentWidth < this.ExtentWidth)
			{
				if (this._lastRowChangeExtentWidth != 0.0)
				{
					this.SetHorizontalOffsetInternal(this.HorizontalOffset + (this.ExtentWidth - this._lastRowChangeExtentWidth) / 2.0);
				}
				this._lastRowChangeExtentWidth = this.ExtentWidth;
			}
			this._lastRowChangeVerticalOffset = this.VerticalOffset;
			for (int i = 0; i < args.Changes.Count; i++)
			{
				RowCacheChange change = args.Changes[i];
				if (this.RowCacheChangeIsVisible(change))
				{
					base.InvalidateMeasure();
					this.InvalidateChildMeasure();
				}
			}
			this.InvalidateDocumentScrollInfo();
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0013BA78 File Offset: 0x0013AA78
		private void OnRowLayoutCompleted(object source, RowLayoutCompletedEventArgs args)
		{
			if (args == null)
			{
				return;
			}
			if (args.PivotRowIndex >= this._rowCache.RowCount)
			{
				throw new ArgumentOutOfRangeException("args");
			}
			RowInfo row = this._rowCache.GetRow(args.PivotRowIndex);
			if (!this.RowIsClean(row) && this._documentLayout.ViewMode != DocumentGrid.ViewMode.Zoom)
			{
				this._savedPivotRow = row;
			}
			else
			{
				this._savedPivotRow = null;
			}
			this.ApplyViewParameters(row);
			if (!this._firstRowLayout && !this._pageJumpAfterLayout)
			{
				this.MakePageVisible(row.FirstPage);
			}
			else if (this._pageJumpAfterLayout)
			{
				this.MakePageVisible(this._pageJumpAfterLayoutPageNumber);
				this._pageJumpAfterLayout = false;
			}
			this._firstRowLayout = false;
			if (!this._scrollChangedEventAttached && this.ScrollOwner != null && this._documentLayout.ViewMode != DocumentGrid.ViewMode.Zoom && this._documentLayout.ViewMode != DocumentGrid.ViewMode.SetColumns)
			{
				this._scrollChangedEventAttached = true;
				this.ScrollOwner.ScrollChanged += new ScrollChangedEventHandler(this.OnScrollChanged);
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x0013BB70 File Offset: 0x0013AB70
		private bool IsViewportNonzero
		{
			get
			{
				return this.ViewportWidth != 0.0 && this.ViewportHeight != 0.0;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0013BB99 File Offset: 0x0013AB99
		private TextEditor TextEditor
		{
			get
			{
				if (this.DocumentViewerOwner != null)
				{
					return this.DocumentViewerOwner.TextEditor;
				}
				return null;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0013BBB0 File Offset: 0x0013ABB0
		private double MouseWheelVerticalScrollAmount
		{
			get
			{
				return 16.0 * (double)SystemParameters.WheelScrollLines;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0013BBC2 File Offset: 0x0013ABC2
		private bool CanMouseWheelVerticallyScroll
		{
			get
			{
				return this._canVerticallyScroll && SystemParameters.WheelScrollLines > 0;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x0013BBB0 File Offset: 0x0013ABB0
		private double MouseWheelHorizontalScrollAmount
		{
			get
			{
				return 16.0 * (double)SystemParameters.WheelScrollLines;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0013BBD6 File Offset: 0x0013ABD6
		private bool CanMouseWheelHorizontallyScroll
		{
			get
			{
				return this._canHorizontallyScroll && SystemParameters.WheelScrollLines > 0;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x0013BBEA File Offset: 0x0013ABEA
		private double CurrentMinimumScale
		{
			get
			{
				if (this._documentLayout.ViewMode != DocumentGrid.ViewMode.Thumbnails)
				{
					return DocumentViewerConstants.MinimumScale;
				}
				return DocumentViewerConstants.MinimumThumbnailsScale;
			}
		}

		// Token: 0x04000A53 RID: 2643
		private PageCache _pageCache;

		// Token: 0x04000A54 RID: 2644
		private RowCache _rowCache;

		// Token: 0x04000A55 RID: 2645
		private ReadOnlyCollection<DocumentPageView> _pageViews;

		// Token: 0x04000A56 RID: 2646
		private bool _canHorizontallyScroll;

		// Token: 0x04000A57 RID: 2647
		private bool _canVerticallyScroll;

		// Token: 0x04000A58 RID: 2648
		private double _verticalOffset;

		// Token: 0x04000A59 RID: 2649
		private double _horizontalOffset;

		// Token: 0x04000A5A RID: 2650
		private double _viewportHeight;

		// Token: 0x04000A5B RID: 2651
		private double _viewportWidth;

		// Token: 0x04000A5C RID: 2652
		private int _firstVisibleRow;

		// Token: 0x04000A5D RID: 2653
		private int _visibleRowCount;

		// Token: 0x04000A5E RID: 2654
		private int _firstVisiblePageNumber;

		// Token: 0x04000A5F RID: 2655
		private int _lastVisiblePageNumber;

		// Token: 0x04000A60 RID: 2656
		private ScrollViewer _scrollOwner;

		// Token: 0x04000A61 RID: 2657
		private DocumentViewer _documentViewerOwner;

		// Token: 0x04000A62 RID: 2658
		private bool _showPageBorders = true;

		// Token: 0x04000A63 RID: 2659
		private bool _lockViewModes;

		// Token: 0x04000A64 RID: 2660
		private int _maxPagesAcross = 1;

		// Token: 0x04000A65 RID: 2661
		private Size _previousConstraint;

		// Token: 0x04000A66 RID: 2662
		private DocumentGrid.DocumentLayout _documentLayout = new DocumentGrid.DocumentLayout(1, DocumentGrid.ViewMode.SetColumns);

		// Token: 0x04000A67 RID: 2663
		private int _documentLayoutsPending;

		// Token: 0x04000A68 RID: 2664
		private RowInfo _savedPivotRow;

		// Token: 0x04000A69 RID: 2665
		private double _lastRowChangeExtentWidth;

		// Token: 0x04000A6A RID: 2666
		private double _lastRowChangeVerticalOffset;

		// Token: 0x04000A6B RID: 2667
		private ITextContainer _textContainer;

		// Token: 0x04000A6C RID: 2668
		private RubberbandSelector _rubberBandSelector;

		// Token: 0x04000A6D RID: 2669
		private bool _isLayoutRequested;

		// Token: 0x04000A6E RID: 2670
		private bool _pageJumpAfterLayout;

		// Token: 0x04000A6F RID: 2671
		private int _pageJumpAfterLayoutPageNumber;

		// Token: 0x04000A70 RID: 2672
		private bool _firstRowLayout = true;

		// Token: 0x04000A71 RID: 2673
		private bool _scrollChangedEventAttached;

		// Token: 0x04000A72 RID: 2674
		private Border _documentGridBackground;

		// Token: 0x04000A73 RID: 2675
		private const int _backgroundVisualIndex = 0;

		// Token: 0x04000A74 RID: 2676
		private const int _firstPageVisualIndex = 1;

		// Token: 0x04000A75 RID: 2677
		private readonly Size _defaultConstraint = new Size(250.0, 250.0);

		// Token: 0x04000A76 RID: 2678
		private VisualCollection _childrenCollection;

		// Token: 0x04000A77 RID: 2679
		private int _makeVisiblePageNeeded = -1;

		// Token: 0x04000A78 RID: 2680
		private DispatcherOperation _makeVisibleDispatcher;

		// Token: 0x04000A79 RID: 2681
		private DispatcherOperation _setScaleOperation;

		// Token: 0x04000A7A RID: 2682
		private const double _verticalLineScrollAmount = 16.0;

		// Token: 0x04000A7B RID: 2683
		private const double _horizontalLineScrollAmount = 16.0;

		// Token: 0x020009D1 RID: 2513
		// (Invoke) Token: 0x060083FD RID: 33789
		private delegate void BringPageIntoViewCallback(DocumentGrid.MakeVisibleData data, int pageNumber);

		// Token: 0x020009D2 RID: 2514
		private enum VisualTreeModificationState
		{
			// Token: 0x04003FBD RID: 16317
			BeforeExisting,
			// Token: 0x04003FBE RID: 16318
			DuringExisting,
			// Token: 0x04003FBF RID: 16319
			AfterExisting
		}

		// Token: 0x020009D3 RID: 2515
		private enum ViewMode
		{
			// Token: 0x04003FC1 RID: 16321
			SetColumns,
			// Token: 0x04003FC2 RID: 16322
			FitColumns,
			// Token: 0x04003FC3 RID: 16323
			PageWidth,
			// Token: 0x04003FC4 RID: 16324
			PageHeight,
			// Token: 0x04003FC5 RID: 16325
			Thumbnails,
			// Token: 0x04003FC6 RID: 16326
			Zoom,
			// Token: 0x04003FC7 RID: 16327
			SetHorizontalOffset,
			// Token: 0x04003FC8 RID: 16328
			SetVerticalOffset
		}

		// Token: 0x020009D4 RID: 2516
		private class DocumentLayout
		{
			// Token: 0x06008400 RID: 33792 RVA: 0x00324D74 File Offset: 0x00323D74
			public DocumentLayout(int columns, DocumentGrid.ViewMode viewMode) : this(columns, 0.0, viewMode)
			{
			}

			// Token: 0x06008401 RID: 33793 RVA: 0x00324D87 File Offset: 0x00323D87
			public DocumentLayout(double offset, DocumentGrid.ViewMode viewMode) : this(1, offset, viewMode)
			{
			}

			// Token: 0x06008402 RID: 33794 RVA: 0x00324D92 File Offset: 0x00323D92
			public DocumentLayout(int columns, double offset, DocumentGrid.ViewMode viewMode)
			{
				this._columns = columns;
				this._offset = offset;
				this._viewMode = viewMode;
			}

			// Token: 0x17001DAD RID: 7597
			// (get) Token: 0x06008404 RID: 33796 RVA: 0x00324DB8 File Offset: 0x00323DB8
			// (set) Token: 0x06008403 RID: 33795 RVA: 0x00324DAF File Offset: 0x00323DAF
			public DocumentGrid.ViewMode ViewMode
			{
				get
				{
					return this._viewMode;
				}
				set
				{
					this._viewMode = value;
				}
			}

			// Token: 0x17001DAE RID: 7598
			// (get) Token: 0x06008406 RID: 33798 RVA: 0x00324DC9 File Offset: 0x00323DC9
			// (set) Token: 0x06008405 RID: 33797 RVA: 0x00324DC0 File Offset: 0x00323DC0
			public int Columns
			{
				get
				{
					return this._columns;
				}
				set
				{
					this._columns = value;
				}
			}

			// Token: 0x17001DAF RID: 7599
			// (get) Token: 0x06008407 RID: 33799 RVA: 0x00324DD1 File Offset: 0x00323DD1
			public double Offset
			{
				get
				{
					return this._offset;
				}
			}

			// Token: 0x04003FC9 RID: 16329
			private DocumentGrid.ViewMode _viewMode;

			// Token: 0x04003FCA RID: 16330
			private int _columns;

			// Token: 0x04003FCB RID: 16331
			private double _offset;
		}

		// Token: 0x020009D5 RID: 2517
		private struct MakeVisibleData
		{
			// Token: 0x06008408 RID: 33800 RVA: 0x00324DD9 File Offset: 0x00323DD9
			public MakeVisibleData(Visual visual, ContentPosition contentPosition, Rect rect)
			{
				this._visual = visual;
				this._contentPosition = contentPosition;
				this._rect = rect;
			}

			// Token: 0x17001DB0 RID: 7600
			// (get) Token: 0x06008409 RID: 33801 RVA: 0x00324DF0 File Offset: 0x00323DF0
			public Visual Visual
			{
				get
				{
					return this._visual;
				}
			}

			// Token: 0x17001DB1 RID: 7601
			// (get) Token: 0x0600840A RID: 33802 RVA: 0x00324DF8 File Offset: 0x00323DF8
			public ContentPosition ContentPosition
			{
				get
				{
					return this._contentPosition;
				}
			}

			// Token: 0x17001DB2 RID: 7602
			// (get) Token: 0x0600840B RID: 33803 RVA: 0x00324E00 File Offset: 0x00323E00
			public Rect Rect
			{
				get
				{
					return this._rect;
				}
			}

			// Token: 0x04003FCC RID: 16332
			private Visual _visual;

			// Token: 0x04003FCD RID: 16333
			private ContentPosition _contentPosition;

			// Token: 0x04003FCE RID: 16334
			private Rect _rect;
		}
	}
}
