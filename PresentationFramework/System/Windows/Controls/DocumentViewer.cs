using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	// Token: 0x02000778 RID: 1912
	[TemplatePart(Name = "PART_FindToolBarHost", Type = typeof(ContentControl))]
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
	public class DocumentViewer : DocumentViewerBase
	{
		// Token: 0x060067F2 RID: 26610 RVA: 0x002B6940 File Offset: 0x002B5940
		static DocumentViewer()
		{
			DocumentViewer.CreateCommandBindings();
			DocumentViewer.RegisterMetadata();
			ControlsTraceLogger.AddControl(TelemetryControls.DocumentViewer);
		}

		// Token: 0x060067F3 RID: 26611 RVA: 0x002B6E4A File Offset: 0x002B5E4A
		public DocumentViewer()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXInstantiated);
			this.SetUp();
		}

		// Token: 0x060067F4 RID: 26612 RVA: 0x002B6E6B File Offset: 0x002B5E6B
		public void ViewThumbnails()
		{
			this.OnViewThumbnailsCommand();
		}

		// Token: 0x060067F5 RID: 26613 RVA: 0x002B6E73 File Offset: 0x002B5E73
		public void FitToWidth()
		{
			this.OnFitToWidthCommand();
		}

		// Token: 0x060067F6 RID: 26614 RVA: 0x002B6E7B File Offset: 0x002B5E7B
		public void FitToHeight()
		{
			this.OnFitToHeightCommand();
		}

		// Token: 0x060067F7 RID: 26615 RVA: 0x002B6E83 File Offset: 0x002B5E83
		public void FitToMaxPagesAcross()
		{
			this.OnFitToMaxPagesAcrossCommand();
		}

		// Token: 0x060067F8 RID: 26616 RVA: 0x002B6E8B File Offset: 0x002B5E8B
		public void FitToMaxPagesAcross(int pagesAcross)
		{
			if (!DocumentViewer.ValidateMaxPagesAcross(pagesAcross))
			{
				throw new ArgumentOutOfRangeException("pagesAcross");
			}
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitColumns(pagesAcross);
				return;
			}
		}

		// Token: 0x060067F9 RID: 26617 RVA: 0x002B6EBA File Offset: 0x002B5EBA
		public void Find()
		{
			this.OnFindCommand();
		}

		// Token: 0x060067FA RID: 26618 RVA: 0x002B6EC2 File Offset: 0x002B5EC2
		public void ScrollPageUp()
		{
			this.OnScrollPageUpCommand();
		}

		// Token: 0x060067FB RID: 26619 RVA: 0x002B6ECA File Offset: 0x002B5ECA
		public void ScrollPageDown()
		{
			this.OnScrollPageDownCommand();
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x002B6ED2 File Offset: 0x002B5ED2
		public void ScrollPageLeft()
		{
			this.OnScrollPageLeftCommand();
		}

		// Token: 0x060067FD RID: 26621 RVA: 0x002B6EDA File Offset: 0x002B5EDA
		public void ScrollPageRight()
		{
			this.OnScrollPageRightCommand();
		}

		// Token: 0x060067FE RID: 26622 RVA: 0x002B6EE2 File Offset: 0x002B5EE2
		public void MoveUp()
		{
			this.OnMoveUpCommand();
		}

		// Token: 0x060067FF RID: 26623 RVA: 0x002B6EEA File Offset: 0x002B5EEA
		public void MoveDown()
		{
			this.OnMoveDownCommand();
		}

		// Token: 0x06006800 RID: 26624 RVA: 0x002B6EF2 File Offset: 0x002B5EF2
		public void MoveLeft()
		{
			this.OnMoveLeftCommand();
		}

		// Token: 0x06006801 RID: 26625 RVA: 0x002B6EFA File Offset: 0x002B5EFA
		public void MoveRight()
		{
			this.OnMoveRightCommand();
		}

		// Token: 0x06006802 RID: 26626 RVA: 0x002B6F02 File Offset: 0x002B5F02
		public void IncreaseZoom()
		{
			this.OnIncreaseZoomCommand();
		}

		// Token: 0x06006803 RID: 26627 RVA: 0x002B6F0A File Offset: 0x002B5F0A
		public void DecreaseZoom()
		{
			this.OnDecreaseZoomCommand();
		}

		// Token: 0x06006804 RID: 26628 RVA: 0x002B6F12 File Offset: 0x002B5F12
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.FindContentHost();
			this.InstantiateFindToolBar();
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXStyleCreated);
			if (base.ContextMenu == null)
			{
				base.ContextMenu = null;
			}
		}

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x06006805 RID: 26629 RVA: 0x002B6F41 File Offset: 0x002B5F41
		public static RoutedUICommand ViewThumbnailsCommand
		{
			get
			{
				return DocumentViewer._viewThumbnailsCommand;
			}
		}

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x06006806 RID: 26630 RVA: 0x002B6F48 File Offset: 0x002B5F48
		public static RoutedUICommand FitToWidthCommand
		{
			get
			{
				return DocumentViewer._fitToWidthCommand;
			}
		}

		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x06006807 RID: 26631 RVA: 0x002B6F4F File Offset: 0x002B5F4F
		public static RoutedUICommand FitToHeightCommand
		{
			get
			{
				return DocumentViewer._fitToHeightCommand;
			}
		}

		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x06006808 RID: 26632 RVA: 0x002B6F56 File Offset: 0x002B5F56
		public static RoutedUICommand FitToMaxPagesAcrossCommand
		{
			get
			{
				return DocumentViewer._fitToMaxPagesAcrossCommand;
			}
		}

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x06006809 RID: 26633 RVA: 0x002B6F5D File Offset: 0x002B5F5D
		// (set) Token: 0x0600680A RID: 26634 RVA: 0x002B6F6F File Offset: 0x002B5F6F
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.HorizontalOffsetProperty, value);
			}
		}

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x0600680B RID: 26635 RVA: 0x002B6F82 File Offset: 0x002B5F82
		// (set) Token: 0x0600680C RID: 26636 RVA: 0x002B6F94 File Offset: 0x002B5F94
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.VerticalOffsetProperty, value);
			}
		}

		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x0600680D RID: 26637 RVA: 0x002B6FA7 File Offset: 0x002B5FA7
		public double ExtentWidth
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ExtentWidthProperty);
			}
		}

		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x0600680E RID: 26638 RVA: 0x002B6FB9 File Offset: 0x002B5FB9
		public double ExtentHeight
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ExtentHeightProperty);
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x0600680F RID: 26639 RVA: 0x002B6FCB File Offset: 0x002B5FCB
		public double ViewportWidth
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ViewportWidthProperty);
			}
		}

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x06006810 RID: 26640 RVA: 0x002B6FDD File Offset: 0x002B5FDD
		public double ViewportHeight
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ViewportHeightProperty);
			}
		}

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x06006811 RID: 26641 RVA: 0x002B6FEF File Offset: 0x002B5FEF
		// (set) Token: 0x06006812 RID: 26642 RVA: 0x002B7001 File Offset: 0x002B6001
		public bool ShowPageBorders
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.ShowPageBordersProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.ShowPageBordersProperty, value);
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x06006813 RID: 26643 RVA: 0x002B700F File Offset: 0x002B600F
		// (set) Token: 0x06006814 RID: 26644 RVA: 0x002B7021 File Offset: 0x002B6021
		public double Zoom
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ZoomProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.ZoomProperty, value);
			}
		}

		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x06006815 RID: 26645 RVA: 0x002B7034 File Offset: 0x002B6034
		// (set) Token: 0x06006816 RID: 26646 RVA: 0x002B7046 File Offset: 0x002B6046
		public int MaxPagesAcross
		{
			get
			{
				return (int)base.GetValue(DocumentViewer.MaxPagesAcrossProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.MaxPagesAcrossProperty, value);
			}
		}

		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x06006817 RID: 26647 RVA: 0x002B7059 File Offset: 0x002B6059
		// (set) Token: 0x06006818 RID: 26648 RVA: 0x002B706B File Offset: 0x002B606B
		public double VerticalPageSpacing
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.VerticalPageSpacingProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.VerticalPageSpacingProperty, value);
			}
		}

		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x06006819 RID: 26649 RVA: 0x002B707E File Offset: 0x002B607E
		// (set) Token: 0x0600681A RID: 26650 RVA: 0x002B7090 File Offset: 0x002B6090
		public double HorizontalPageSpacing
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.HorizontalPageSpacingProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.HorizontalPageSpacingProperty, value);
			}
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x0600681B RID: 26651 RVA: 0x002B70A3 File Offset: 0x002B60A3
		public bool CanMoveUp
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanMoveUpProperty);
			}
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x0600681C RID: 26652 RVA: 0x002B70B5 File Offset: 0x002B60B5
		public bool CanMoveDown
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanMoveDownProperty);
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x0600681D RID: 26653 RVA: 0x002B70C7 File Offset: 0x002B60C7
		public bool CanMoveLeft
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanMoveLeftProperty);
			}
		}

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x0600681E RID: 26654 RVA: 0x002B70D9 File Offset: 0x002B60D9
		public bool CanMoveRight
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanMoveRightProperty);
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x0600681F RID: 26655 RVA: 0x002B70EB File Offset: 0x002B60EB
		public bool CanIncreaseZoom
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanIncreaseZoomProperty);
			}
		}

		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x06006820 RID: 26656 RVA: 0x002B70FD File Offset: 0x002B60FD
		public bool CanDecreaseZoom
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanDecreaseZoomProperty);
			}
		}

		// Token: 0x06006821 RID: 26657 RVA: 0x002B710F File Offset: 0x002B610F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentViewerAutomationPeer(this);
		}

		// Token: 0x06006822 RID: 26658 RVA: 0x002B7118 File Offset: 0x002B6118
		protected override void OnDocumentChanged()
		{
			if (!(base.Document is FixedDocument) && !(base.Document is FixedDocumentSequence) && base.Document != null)
			{
				throw new NotSupportedException(SR.Get("DocumentViewerOnlySupportsFixedDocumentSequence"));
			}
			base.OnDocumentChanged();
			this.AttachContent();
			if (this._findToolbar != null)
			{
				this._findToolbar.DocumentLoaded = (base.Document != null);
			}
			if (!this._firstDocumentAssignment)
			{
				this.OnGoToPageCommand(1);
			}
			this._firstDocumentAssignment = false;
		}

		// Token: 0x06006823 RID: 26659 RVA: 0x002B7198 File Offset: 0x002B6198
		protected override void OnBringIntoView(DependencyObject element, Rect rect, int pageNumber)
		{
			int num = pageNumber - 1;
			if (num >= 0 && num < base.PageCount)
			{
				this._documentScrollInfo.MakeVisible(element, rect, num);
			}
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x002B71C5 File Offset: 0x002B61C5
		protected override void OnPreviousPageCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.ScrollToPreviousRow();
			}
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x002B71DA File Offset: 0x002B61DA
		protected override void OnNextPageCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.ScrollToNextRow();
			}
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x002B71EF File Offset: 0x002B61EF
		protected override void OnFirstPageCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.MakePageVisible(0);
			}
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x002B7205 File Offset: 0x002B6205
		protected override void OnLastPageCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.MakePageVisible(base.PageCount - 1);
			}
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x002B7222 File Offset: 0x002B6222
		protected override void OnGoToPageCommand(int pageNumber)
		{
			if (this.CanGoToPage(pageNumber) && this._documentScrollInfo != null)
			{
				Invariant.Assert(pageNumber > 0, "PageNumber must be positive.");
				this._documentScrollInfo.MakePageVisible(pageNumber - 1);
			}
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x002B7251 File Offset: 0x002B6251
		protected virtual void OnViewThumbnailsCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.ViewThumbnails();
			}
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x002B7266 File Offset: 0x002B6266
		protected virtual void OnFitToWidthCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitToPageWidth();
			}
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x002B727B File Offset: 0x002B627B
		protected virtual void OnFitToHeightCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitToPageHeight();
			}
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x002B7290 File Offset: 0x002B6290
		protected virtual void OnFitToMaxPagesAcrossCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitColumns(this.MaxPagesAcross);
			}
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x002B6E8B File Offset: 0x002B5E8B
		protected virtual void OnFitToMaxPagesAcrossCommand(int pagesAcross)
		{
			if (!DocumentViewer.ValidateMaxPagesAcross(pagesAcross))
			{
				throw new ArgumentOutOfRangeException("pagesAcross");
			}
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitColumns(pagesAcross);
				return;
			}
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x002B72AB File Offset: 0x002B62AB
		protected virtual void OnFindCommand()
		{
			this.GoToFind();
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x002B72B3 File Offset: 0x002B62B3
		protected override void OnKeyDown(KeyEventArgs e)
		{
			e = this.ProcessFindKeys(e);
			base.OnKeyDown(e);
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x002B72C5 File Offset: 0x002B62C5
		protected virtual void OnScrollPageUpCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.PageUp();
			}
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x002B72DA File Offset: 0x002B62DA
		protected virtual void OnScrollPageDownCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.PageDown();
			}
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x002B72EF File Offset: 0x002B62EF
		protected virtual void OnScrollPageLeftCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.PageLeft();
			}
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x002B7304 File Offset: 0x002B6304
		protected virtual void OnScrollPageRightCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.PageRight();
			}
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x002B7319 File Offset: 0x002B6319
		protected virtual void OnMoveUpCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.LineUp();
			}
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x002B732E File Offset: 0x002B632E
		protected virtual void OnMoveDownCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.LineDown();
			}
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x002B7343 File Offset: 0x002B6343
		protected virtual void OnMoveLeftCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.LineLeft();
			}
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x002B7358 File Offset: 0x002B6358
		protected virtual void OnMoveRightCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.LineRight();
			}
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x002B7370 File Offset: 0x002B6370
		protected virtual void OnIncreaseZoomCommand()
		{
			if (this.CanIncreaseZoom)
			{
				double zoom = this.Zoom;
				this.FindZoomLevelIndex();
				if (this._zoomLevelIndex > 0)
				{
					this._zoomLevelIndex--;
				}
				this._updatingInternalZoomLevel = true;
				this.Zoom = DocumentViewer._zoomLevelCollection[this._zoomLevelIndex];
				this._updatingInternalZoomLevel = false;
			}
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x002B73CC File Offset: 0x002B63CC
		protected virtual void OnDecreaseZoomCommand()
		{
			if (this.CanDecreaseZoom)
			{
				double zoom = this.Zoom;
				this.FindZoomLevelIndex();
				if (zoom == DocumentViewer._zoomLevelCollection[this._zoomLevelIndex] && this._zoomLevelIndex < DocumentViewer._zoomLevelCollection.Length - 1)
				{
					this._zoomLevelIndex++;
				}
				this._updatingInternalZoomLevel = true;
				this.Zoom = DocumentViewer._zoomLevelCollection[this._zoomLevelIndex];
				this._updatingInternalZoomLevel = false;
			}
		}

		// Token: 0x0600683A RID: 26682 RVA: 0x002B743C File Offset: 0x002B643C
		protected override ReadOnlyCollection<DocumentPageView> GetPageViewsCollection(out bool changed)
		{
			changed = this._pageViewCollectionChanged;
			this._pageViewCollectionChanged = false;
			ReadOnlyCollection<DocumentPageView> result;
			if (this._documentScrollInfo != null && this._documentScrollInfo.PageViews != null)
			{
				result = this._documentScrollInfo.PageViews;
			}
			else
			{
				result = new ReadOnlyCollection<DocumentPageView>(new List<DocumentPageView>(0));
			}
			return result;
		}

		// Token: 0x0600683B RID: 26683 RVA: 0x002B748A File Offset: 0x002B648A
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			if (!e.Handled)
			{
				base.Focus();
				e.Handled = true;
			}
		}

		// Token: 0x0600683C RID: 26684 RVA: 0x002B74A9 File Offset: 0x002B64A9
		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				e.Handled = true;
				if (e.Delta < 0)
				{
					this.DecreaseZoom();
					return;
				}
				this.IncreaseZoom();
			}
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x002B74E4 File Offset: 0x002B64E4
		internal void InvalidateDocumentScrollInfo()
		{
			this._internalIDSIChange = true;
			base.SetValue(DocumentViewer.ExtentWidthPropertyKey, this._documentScrollInfo.ExtentWidth);
			base.SetValue(DocumentViewer.ExtentHeightPropertyKey, this._documentScrollInfo.ExtentHeight);
			base.SetValue(DocumentViewer.ViewportWidthPropertyKey, this._documentScrollInfo.ViewportWidth);
			base.SetValue(DocumentViewer.ViewportHeightPropertyKey, this._documentScrollInfo.ViewportHeight);
			if (this.HorizontalOffset != this._documentScrollInfo.HorizontalOffset)
			{
				this.HorizontalOffset = this._documentScrollInfo.HorizontalOffset;
			}
			if (this.VerticalOffset != this._documentScrollInfo.VerticalOffset)
			{
				this.VerticalOffset = this._documentScrollInfo.VerticalOffset;
			}
			base.SetValue(DocumentViewerBase.MasterPageNumberPropertyKey, this._documentScrollInfo.FirstVisiblePageNumber + 1);
			double num = DocumentViewer.ScaleToZoom(this._documentScrollInfo.Scale);
			if (this.Zoom != num)
			{
				this.Zoom = num;
			}
			if (this.MaxPagesAcross != this._documentScrollInfo.MaxPagesAcross)
			{
				this.MaxPagesAcross = this._documentScrollInfo.MaxPagesAcross;
			}
			this._internalIDSIChange = false;
		}

		// Token: 0x0600683E RID: 26686 RVA: 0x002B7615 File Offset: 0x002B6615
		internal void InvalidatePageViewsInternal()
		{
			this._pageViewCollectionChanged = true;
			base.InvalidatePageViews();
		}

		// Token: 0x0600683F RID: 26687 RVA: 0x002B7624 File Offset: 0x002B6624
		internal bool BringPointIntoView(Point point)
		{
			FrameworkElement frameworkElement = this._documentScrollInfo as FrameworkElement;
			if (frameworkElement != null)
			{
				Transform transform = base.TransformToDescendant(frameworkElement) as Transform;
				Rect rect = Rect.Transform(new Rect(frameworkElement.RenderSize), transform.Value);
				double num = this.VerticalOffset;
				double num2 = this.HorizontalOffset;
				if (point.Y > rect.Y + rect.Height)
				{
					num += point.Y - (rect.Y + rect.Height);
				}
				else if (point.Y < rect.Y)
				{
					num -= rect.Y - point.Y;
				}
				if (point.X < rect.X)
				{
					num2 -= rect.X - point.X;
				}
				else if (point.X > rect.X + rect.Width)
				{
					num2 += point.X - (rect.X + rect.Width);
				}
				this.VerticalOffset = Math.Max(num, 0.0);
				this.HorizontalOffset = Math.Max(num2, 0.0);
			}
			return false;
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x06006840 RID: 26688 RVA: 0x002B7752 File Offset: 0x002B6752
		internal ITextSelection TextSelection
		{
			get
			{
				if (base.TextEditor != null)
				{
					return base.TextEditor.Selection;
				}
				return null;
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x06006841 RID: 26689 RVA: 0x002B7769 File Offset: 0x002B6769
		internal IDocumentScrollInfo DocumentScrollInfo
		{
			get
			{
				return this._documentScrollInfo;
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x06006842 RID: 26690 RVA: 0x002B7771 File Offset: 0x002B6771
		internal ScrollViewer ScrollViewer
		{
			get
			{
				return this._scrollViewer;
			}
		}

		// Token: 0x06006843 RID: 26691 RVA: 0x002B777C File Offset: 0x002B677C
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(DocumentViewer.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(DocumentViewer.QueryEnabledHandler);
			DocumentViewer._viewThumbnailsCommand = new RoutedUICommand(SR.Get("DocumentViewerViewThumbnailsCommandText"), "ViewThumbnailsCommand", typeof(DocumentViewer), null);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), DocumentViewer._viewThumbnailsCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			DocumentViewer._fitToWidthCommand = new RoutedUICommand(SR.Get("DocumentViewerViewFitToWidthCommandText"), "FitToWidthCommand", typeof(DocumentViewer), null);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), DocumentViewer._fitToWidthCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.D2, ModifierKeys.Control));
			DocumentViewer._fitToHeightCommand = new RoutedUICommand(SR.Get("DocumentViewerViewFitToHeightCommandText"), "FitToHeightCommand", typeof(DocumentViewer), null);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), DocumentViewer._fitToHeightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			DocumentViewer._fitToMaxPagesAcrossCommand = new RoutedUICommand(SR.Get("DocumentViewerViewFitToMaxPagesAcrossCommandText"), "FitToMaxPagesAcrossCommand", typeof(DocumentViewer), null);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), DocumentViewer._fitToMaxPagesAcrossCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ApplicationCommands.Find, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.ScrollPageUp, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Prior);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.ScrollPageDown, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Next);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.ScrollPageLeft, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.ScrollPageRight, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.MoveUp, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Up);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.MoveDown, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Down);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.MoveLeft, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Left);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.MoveRight, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Right);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.Zoom, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.IncreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Add, ModifierKeys.Control), new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Shift), new KeyGesture(Key.OemPlus, ModifierKeys.Control), new KeyGesture(Key.OemPlus, ModifierKeys.Control | ModifierKeys.Shift));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.DecreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Subtract, ModifierKeys.Control), new KeyGesture(Key.Subtract, ModifierKeys.Control | ModifierKeys.Shift), new KeyGesture(Key.OemMinus, ModifierKeys.Control), new KeyGesture(Key.OemMinus, ModifierKeys.Control | ModifierKeys.Shift));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.PreviousPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Prior, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.NextPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Next, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.FirstPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Home, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.LastPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.End, ModifierKeys.Control));
			InputBinding inputBinding = new InputBinding(NavigationCommands.Zoom, new KeyGesture(Key.D1, ModifierKeys.Control));
			inputBinding.CommandParameter = 100.0;
			CommandManager.RegisterClassInputBinding(typeof(DocumentViewer), inputBinding);
			InputBinding inputBinding2 = new InputBinding(DocumentViewer.FitToMaxPagesAcrossCommand, new KeyGesture(Key.D3, ModifierKeys.Control));
			inputBinding2.CommandParameter = 1;
			CommandManager.RegisterClassInputBinding(typeof(DocumentViewer), inputBinding2);
			InputBinding inputBinding3 = new InputBinding(DocumentViewer.FitToMaxPagesAcrossCommand, new KeyGesture(Key.D4, ModifierKeys.Control));
			inputBinding3.CommandParameter = 2;
			CommandManager.RegisterClassInputBinding(typeof(DocumentViewer), inputBinding3);
		}

		// Token: 0x06006844 RID: 26692 RVA: 0x002B7B04 File Offset: 0x002B6B04
		private static void QueryEnabledHandler(object target, CanExecuteRoutedEventArgs args)
		{
			DocumentViewer documentViewer = target as DocumentViewer;
			Invariant.Assert(documentViewer != null, "Target of QueryEnabledEvent must be DocumentViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (documentViewer == null)
			{
				return;
			}
			args.Handled = true;
			if (args.Command == DocumentViewer.ViewThumbnailsCommand || args.Command == DocumentViewer.FitToWidthCommand || args.Command == DocumentViewer.FitToHeightCommand || args.Command == DocumentViewer.FitToMaxPagesAcrossCommand || args.Command == NavigationCommands.Zoom)
			{
				args.CanExecute = true;
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				args.CanExecute = (documentViewer.TextEditor != null);
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageUp || args.Command == ComponentCommands.MoveUp)
			{
				args.CanExecute = documentViewer.CanMoveUp;
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageDown || args.Command == ComponentCommands.MoveDown)
			{
				args.CanExecute = documentViewer.CanMoveDown;
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageLeft || args.Command == ComponentCommands.MoveLeft)
			{
				args.CanExecute = documentViewer.CanMoveLeft;
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageRight || args.Command == ComponentCommands.MoveRight)
			{
				args.CanExecute = documentViewer.CanMoveRight;
				return;
			}
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				args.CanExecute = documentViewer.CanIncreaseZoom;
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				args.CanExecute = documentViewer.CanDecreaseZoom;
				return;
			}
			if (args.Command == NavigationCommands.PreviousPage || args.Command == NavigationCommands.FirstPage)
			{
				args.CanExecute = documentViewer.CanGoToPreviousPage;
				return;
			}
			if (args.Command == NavigationCommands.NextPage || args.Command == NavigationCommands.LastPage)
			{
				args.CanExecute = documentViewer.CanGoToNextPage;
				return;
			}
			if (args.Command == NavigationCommands.GoToPage)
			{
				args.CanExecute = (documentViewer.Document != null);
				return;
			}
			args.Handled = false;
			Invariant.Assert(false, "Command not handled in QueryEnabledHandler.");
		}

		// Token: 0x06006845 RID: 26693 RVA: 0x002B7CF4 File Offset: 0x002B6CF4
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			DocumentViewer documentViewer = target as DocumentViewer;
			Invariant.Assert(documentViewer != null, "Target of ExecuteEvent must be DocumentViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (documentViewer == null)
			{
				return;
			}
			if (args.Command == DocumentViewer.ViewThumbnailsCommand)
			{
				documentViewer.OnViewThumbnailsCommand();
				return;
			}
			if (args.Command == DocumentViewer.FitToWidthCommand)
			{
				documentViewer.OnFitToWidthCommand();
				return;
			}
			if (args.Command == DocumentViewer.FitToHeightCommand)
			{
				documentViewer.OnFitToHeightCommand();
				return;
			}
			if (args.Command == DocumentViewer.FitToMaxPagesAcrossCommand)
			{
				DocumentViewer.DoFitToMaxPagesAcross(documentViewer, args.Parameter);
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				documentViewer.OnFindCommand();
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageUp)
			{
				documentViewer.OnScrollPageUpCommand();
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageDown)
			{
				documentViewer.OnScrollPageDownCommand();
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageLeft)
			{
				documentViewer.OnScrollPageLeftCommand();
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageRight)
			{
				documentViewer.OnScrollPageRightCommand();
				return;
			}
			if (args.Command == ComponentCommands.MoveUp)
			{
				documentViewer.OnMoveUpCommand();
				return;
			}
			if (args.Command == ComponentCommands.MoveDown)
			{
				documentViewer.OnMoveDownCommand();
				return;
			}
			if (args.Command == ComponentCommands.MoveLeft)
			{
				documentViewer.OnMoveLeftCommand();
				return;
			}
			if (args.Command == ComponentCommands.MoveRight)
			{
				documentViewer.OnMoveRightCommand();
				return;
			}
			if (args.Command == NavigationCommands.Zoom)
			{
				DocumentViewer.DoZoom(documentViewer, args.Parameter);
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				documentViewer.DecreaseZoom();
				return;
			}
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				documentViewer.IncreaseZoom();
				return;
			}
			if (args.Command == NavigationCommands.PreviousPage)
			{
				documentViewer.PreviousPage();
				return;
			}
			if (args.Command == NavigationCommands.NextPage)
			{
				documentViewer.NextPage();
				return;
			}
			if (args.Command == NavigationCommands.FirstPage)
			{
				documentViewer.FirstPage();
				return;
			}
			if (args.Command == NavigationCommands.LastPage)
			{
				documentViewer.LastPage();
				return;
			}
			Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
		}

		// Token: 0x06006846 RID: 26694 RVA: 0x002B7ED0 File Offset: 0x002B6ED0
		private static void DoFitToMaxPagesAcross(DocumentViewer dv, object data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			int pagesAcross = 0;
			bool flag = true;
			if (data is int)
			{
				pagesAcross = (int)data;
			}
			else if (data is string)
			{
				try
				{
					pagesAcross = Convert.ToInt32((string)data, CultureInfo.CurrentCulture);
				}
				catch (ArgumentNullException)
				{
					flag = false;
				}
				catch (FormatException)
				{
					flag = false;
				}
				catch (OverflowException)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				throw new ArgumentException(SR.Get("DocumentViewerArgumentMustBeInteger"), "data");
			}
			dv.OnFitToMaxPagesAcrossCommand(pagesAcross);
		}

		// Token: 0x06006847 RID: 26695 RVA: 0x002B7F74 File Offset: 0x002B6F74
		private static void DoZoom(DocumentViewer dv, object data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (dv._zoomPercentageConverter == null)
			{
				dv._zoomPercentageConverter = new ZoomPercentageConverter();
			}
			object obj = dv._zoomPercentageConverter.ConvertBack(data, typeof(double), null, CultureInfo.InvariantCulture);
			if (obj == DependencyProperty.UnsetValue)
			{
				throw new ArgumentException(SR.Get("DocumentViewerArgumentMustBePercentage"), "data");
			}
			dv.Zoom = (double)obj;
		}

		// Token: 0x06006848 RID: 26696 RVA: 0x002B7FE8 File Offset: 0x002B6FE8
		private static void RegisterMetadata()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DocumentViewer), new FrameworkPropertyMetadata(typeof(DocumentViewer)));
			DocumentViewer._dType = DependencyObjectType.FromSystemTypeInternal(typeof(DocumentViewer));
		}

		// Token: 0x06006849 RID: 26697 RVA: 0x002B8021 File Offset: 0x002B7021
		private void SetUp()
		{
			base.IsSelectionEnabled = true;
			base.SetValue(TextBoxBase.AcceptsTabProperty, false);
			this.CreateIDocumentScrollInfo();
		}

		// Token: 0x0600684A RID: 26698 RVA: 0x002B803C File Offset: 0x002B703C
		private void CreateIDocumentScrollInfo()
		{
			if (this._documentScrollInfo == null)
			{
				this._documentScrollInfo = new DocumentGrid();
				this._documentScrollInfo.DocumentViewerOwner = this;
				FrameworkElement frameworkElement = this._documentScrollInfo as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.Name = "DocumentGrid";
					frameworkElement.Focusable = false;
					frameworkElement.SetValue(KeyboardNavigation.IsTabStopProperty, false);
					base.TextEditorRenderScope = frameworkElement;
				}
			}
			this.AttachContent();
			this._documentScrollInfo.VerticalPageSpacing = this.VerticalPageSpacing;
			this._documentScrollInfo.HorizontalPageSpacing = this.HorizontalPageSpacing;
		}

		// Token: 0x0600684B RID: 26699 RVA: 0x002B80C4 File Offset: 0x002B70C4
		private void AttachContent()
		{
			this._documentScrollInfo.Content = ((base.Document != null) ? (base.Document.DocumentPaginator as DynamicDocumentPaginator) : null);
			base.IsSelectionEnabled = true;
		}

		// Token: 0x0600684C RID: 26700 RVA: 0x002B80F4 File Offset: 0x002B70F4
		private void FindContentHost()
		{
			ScrollViewer scrollViewer = base.Template.FindName("PART_ContentHost", this) as ScrollViewer;
			if (scrollViewer == null)
			{
				throw new NotSupportedException(SR.Get("DocumentViewerStyleMustIncludeContentHost"));
			}
			this._scrollViewer = scrollViewer;
			this._scrollViewer.Focusable = false;
			Invariant.Assert(this._documentScrollInfo != null, "IDocumentScrollInfo cannot be null.");
			this._scrollViewer.Content = this._documentScrollInfo;
			this._scrollViewer.ScrollInfo = this._documentScrollInfo;
			if (this._documentScrollInfo.Content != base.Document)
			{
				this.AttachContent();
			}
		}

		// Token: 0x0600684D RID: 26701 RVA: 0x002B818C File Offset: 0x002B718C
		private void InstantiateFindToolBar()
		{
			ContentControl contentControl = base.Template.FindName("PART_FindToolBarHost", this) as ContentControl;
			if (contentControl != null)
			{
				if (this._findToolbar == null)
				{
					this._findToolbar = new FindToolBar();
					this._findToolbar.FindClicked += this.OnFindInvoked;
					this._findToolbar.DocumentLoaded = (base.Document != null);
				}
				if (!this._findToolbar.IsAncestorOf(this))
				{
					((IAddChild)contentControl).AddChild(this._findToolbar);
				}
			}
		}

		// Token: 0x0600684E RID: 26702 RVA: 0x002B8210 File Offset: 0x002B7210
		private void OnFindInvoked(object sender, EventArgs e)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXFindBegin);
			try
			{
				if (this._findToolbar != null && base.TextEditor != null)
				{
					ITextRange textRange = base.Find(this._findToolbar);
					if (textRange != null && !textRange.IsEmpty)
					{
						base.Focus();
						if (this._documentScrollInfo != null)
						{
							this._documentScrollInfo.MakeSelectionVisible();
						}
						this._findToolbar.GoToTextBox();
					}
					else
					{
						string text = this._findToolbar.SearchUp ? SR.Get("DocumentViewerSearchUpCompleteLabel") : SR.Get("DocumentViewerSearchDownCompleteLabel");
						text = string.Format(CultureInfo.CurrentCulture, text, this._findToolbar.SearchText);
						Window parent = null;
						if (Application.Current != null && Application.Current.CheckAccess())
						{
							parent = Application.Current.MainWindow;
						}
						SecurityHelper.ShowMessageBoxHelper(parent, text, SR.Get("DocumentViewerSearchCompleteTitle"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
					}
				}
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXFindEnd);
			}
		}

		// Token: 0x0600684F RID: 26703 RVA: 0x002B830C File Offset: 0x002B730C
		private void GoToFind()
		{
			if (this._findToolbar != null)
			{
				this._findToolbar.GoToTextBox();
			}
		}

		// Token: 0x06006850 RID: 26704 RVA: 0x002B8324 File Offset: 0x002B7324
		private KeyEventArgs ProcessFindKeys(KeyEventArgs e)
		{
			if (this._findToolbar == null || base.Document == null)
			{
				return e;
			}
			if (e.Key == Key.F3)
			{
				e.Handled = true;
				this._findToolbar.SearchUp = ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
				this.OnFindInvoked(this, EventArgs.Empty);
			}
			return e;
		}

		// Token: 0x06006851 RID: 26705 RVA: 0x002B837C File Offset: 0x002B737C
		private void FindZoomLevelIndex()
		{
			if (DocumentViewer._zoomLevelCollection != null)
			{
				if (this._zoomLevelIndex < 0 || this._zoomLevelIndex >= DocumentViewer._zoomLevelCollection.Length)
				{
					this._zoomLevelIndex = 0;
					this._zoomLevelIndexValid = false;
				}
				if (!this._zoomLevelIndexValid)
				{
					double zoom = this.Zoom;
					int num = 0;
					while (num < DocumentViewer._zoomLevelCollection.Length - 1 && zoom < DocumentViewer._zoomLevelCollection[num])
					{
						num++;
					}
					this._zoomLevelIndex = num;
					this._zoomLevelIndexValid = true;
				}
			}
		}

		// Token: 0x06006852 RID: 26706 RVA: 0x002B83F4 File Offset: 0x002B73F4
		private static bool DoubleValue_Validate(object value)
		{
			bool result;
			if (value is double)
			{
				double d = (double)value;
				result = (!double.IsNaN(d) && !double.IsInfinity(d));
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06006853 RID: 26707 RVA: 0x002B842B File Offset: 0x002B742B
		private static double ScaleToZoom(double scale)
		{
			return scale * 100.0;
		}

		// Token: 0x06006854 RID: 26708 RVA: 0x002B8438 File Offset: 0x002B7438
		private static double ZoomToScale(double zoom)
		{
			return zoom / 100.0;
		}

		// Token: 0x06006855 RID: 26709 RVA: 0x002B8445 File Offset: 0x002B7445
		private static bool ValidateOffset(object value)
		{
			return DocumentViewer.DoubleValue_Validate(value) && (double)value >= 0.0;
		}

		// Token: 0x06006856 RID: 26710 RVA: 0x002B8468 File Offset: 0x002B7468
		private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			double num = (double)e.NewValue;
			if (!documentViewer._internalIDSIChange && documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.SetHorizontalOffset(num);
			}
			documentViewer.SetValue(DocumentViewer.CanMoveLeftPropertyKey, num > 0.0);
			documentViewer.SetValue(DocumentViewer.CanMoveRightPropertyKey, num < documentViewer.ExtentWidth - documentViewer.ViewportWidth);
		}

		// Token: 0x06006857 RID: 26711 RVA: 0x002B84D8 File Offset: 0x002B74D8
		private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			double num = (double)e.NewValue;
			if (!documentViewer._internalIDSIChange && documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.SetVerticalOffset(num);
			}
			documentViewer.SetValue(DocumentViewer.CanMoveUpPropertyKey, num > 0.0);
			documentViewer.SetValue(DocumentViewer.CanMoveDownPropertyKey, num < documentViewer.ExtentHeight - documentViewer.ViewportHeight);
		}

		// Token: 0x06006858 RID: 26712 RVA: 0x002B8548 File Offset: 0x002B7548
		private static void OnExtentWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			documentViewer.SetValue(DocumentViewer.CanMoveRightPropertyKey, documentViewer.HorizontalOffset < (double)e.NewValue - documentViewer.ViewportWidth);
		}

		// Token: 0x06006859 RID: 26713 RVA: 0x002B8584 File Offset: 0x002B7584
		private static void OnExtentHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			documentViewer.SetValue(DocumentViewer.CanMoveDownPropertyKey, documentViewer.VerticalOffset < (double)e.NewValue - documentViewer.ViewportHeight);
		}

		// Token: 0x0600685A RID: 26714 RVA: 0x002B85C0 File Offset: 0x002B75C0
		private static void OnViewportWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			double num = (double)e.NewValue;
			documentViewer.SetValue(DocumentViewer.CanMoveRightPropertyKey, documentViewer.HorizontalOffset < documentViewer.ExtentWidth - (double)e.NewValue);
		}

		// Token: 0x0600685B RID: 26715 RVA: 0x002B8608 File Offset: 0x002B7608
		private static void OnViewportHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			double num = (double)e.NewValue;
			documentViewer.SetValue(DocumentViewer.CanMoveDownPropertyKey, documentViewer.VerticalOffset < documentViewer.ExtentHeight - num);
		}

		// Token: 0x0600685C RID: 26716 RVA: 0x002B8644 File Offset: 0x002B7644
		private static void OnShowPageBordersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.ShowPageBorders = (bool)e.NewValue;
			}
		}

		// Token: 0x0600685D RID: 26717 RVA: 0x002B8678 File Offset: 0x002B7678
		private static object CoerceZoom(DependencyObject d, object value)
		{
			double num = (double)value;
			if (num < DocumentViewerConstants.MinimumZoom)
			{
				return DocumentViewerConstants.MinimumZoom;
			}
			if (num > DocumentViewerConstants.MaximumZoom)
			{
				return DocumentViewerConstants.MaximumZoom;
			}
			return value;
		}

		// Token: 0x0600685E RID: 26718 RVA: 0x002B86B4 File Offset: 0x002B76B4
		private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (documentViewer._documentScrollInfo != null)
			{
				double num = (double)e.NewValue;
				if (!documentViewer._internalIDSIChange)
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXZoom, (int)num);
					documentViewer._documentScrollInfo.SetScale(DocumentViewer.ZoomToScale(num));
				}
				documentViewer.SetValue(DocumentViewer.CanIncreaseZoomPropertyKey, num < DocumentViewerConstants.MaximumZoom);
				documentViewer.SetValue(DocumentViewer.CanDecreaseZoomPropertyKey, num > DocumentViewerConstants.MinimumZoom);
				if (!documentViewer._updatingInternalZoomLevel)
				{
					documentViewer._zoomLevelIndexValid = false;
				}
			}
		}

		// Token: 0x0600685F RID: 26719 RVA: 0x002B8740 File Offset: 0x002B7740
		private static bool ValidateMaxPagesAcross(object value)
		{
			int num = (int)value;
			return num > 0 && num <= DocumentViewerConstants.MaximumMaxPagesAcross;
		}

		// Token: 0x06006860 RID: 26720 RVA: 0x002B8768 File Offset: 0x002B7768
		private static void OnMaxPagesAcrossChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (!documentViewer._internalIDSIChange)
			{
				documentViewer._documentScrollInfo.SetColumns((int)e.NewValue);
			}
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x002B8445 File Offset: 0x002B7445
		private static bool ValidatePageSpacing(object value)
		{
			return DocumentViewer.DoubleValue_Validate(value) && (double)value >= 0.0;
		}

		// Token: 0x06006862 RID: 26722 RVA: 0x002B879C File Offset: 0x002B779C
		private static void OnVerticalPageSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.VerticalPageSpacing = (double)e.NewValue;
			}
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x002B87D0 File Offset: 0x002B77D0
		private static void OnHorizontalPageSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.HorizontalPageSpacing = (double)e.NewValue;
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x06006864 RID: 26724 RVA: 0x002B8803 File Offset: 0x002B7803
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return DocumentViewer._dType;
			}
		}

		// Token: 0x04003467 RID: 13415
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnHorizontalOffsetChanged)), new ValidateValueCallback(DocumentViewer.ValidateOffset));

		// Token: 0x04003468 RID: 13416
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnVerticalOffsetChanged)), new ValidateValueCallback(DocumentViewer.ValidateOffset));

		// Token: 0x04003469 RID: 13417
		private static readonly DependencyPropertyKey ExtentWidthPropertyKey = DependencyProperty.RegisterReadOnly("ExtentWidth", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DocumentViewer.OnExtentWidthChanged)));

		// Token: 0x0400346A RID: 13418
		public static readonly DependencyProperty ExtentWidthProperty = DocumentViewer.ExtentWidthPropertyKey.DependencyProperty;

		// Token: 0x0400346B RID: 13419
		private static readonly DependencyPropertyKey ExtentHeightPropertyKey = DependencyProperty.RegisterReadOnly("ExtentHeight", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DocumentViewer.OnExtentHeightChanged)));

		// Token: 0x0400346C RID: 13420
		public static readonly DependencyProperty ExtentHeightProperty = DocumentViewer.ExtentHeightPropertyKey.DependencyProperty;

		// Token: 0x0400346D RID: 13421
		private static readonly DependencyPropertyKey ViewportWidthPropertyKey = DependencyProperty.RegisterReadOnly("ViewportWidth", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DocumentViewer.OnViewportWidthChanged)));

		// Token: 0x0400346E RID: 13422
		public static readonly DependencyProperty ViewportWidthProperty = DocumentViewer.ViewportWidthPropertyKey.DependencyProperty;

		// Token: 0x0400346F RID: 13423
		private static readonly DependencyPropertyKey ViewportHeightPropertyKey = DependencyProperty.RegisterReadOnly("ViewportHeight", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DocumentViewer.OnViewportHeightChanged)));

		// Token: 0x04003470 RID: 13424
		public static readonly DependencyProperty ViewportHeightProperty = DocumentViewer.ViewportHeightPropertyKey.DependencyProperty;

		// Token: 0x04003471 RID: 13425
		public static readonly DependencyProperty ShowPageBordersProperty = DependencyProperty.Register("ShowPageBorders", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnShowPageBordersChanged)));

		// Token: 0x04003472 RID: 13426
		public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnZoomChanged), new CoerceValueCallback(DocumentViewer.CoerceZoom)));

		// Token: 0x04003473 RID: 13427
		public static readonly DependencyProperty MaxPagesAcrossProperty = DependencyProperty.Register("MaxPagesAcross", typeof(int), typeof(DocumentViewer), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnMaxPagesAcrossChanged)), new ValidateValueCallback(DocumentViewer.ValidateMaxPagesAcross));

		// Token: 0x04003474 RID: 13428
		public static readonly DependencyProperty VerticalPageSpacingProperty = DependencyProperty.Register("VerticalPageSpacing", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(10.0, new PropertyChangedCallback(DocumentViewer.OnVerticalPageSpacingChanged)), new ValidateValueCallback(DocumentViewer.ValidatePageSpacing));

		// Token: 0x04003475 RID: 13429
		public static readonly DependencyProperty HorizontalPageSpacingProperty = DependencyProperty.Register("HorizontalPageSpacing", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(10.0, new PropertyChangedCallback(DocumentViewer.OnHorizontalPageSpacingChanged)), new ValidateValueCallback(DocumentViewer.ValidatePageSpacing));

		// Token: 0x04003476 RID: 13430
		private static readonly DependencyPropertyKey CanMoveUpPropertyKey = DependencyProperty.RegisterReadOnly("CanMoveUp", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(false));

		// Token: 0x04003477 RID: 13431
		public static readonly DependencyProperty CanMoveUpProperty = DocumentViewer.CanMoveUpPropertyKey.DependencyProperty;

		// Token: 0x04003478 RID: 13432
		private static readonly DependencyPropertyKey CanMoveDownPropertyKey = DependencyProperty.RegisterReadOnly("CanMoveDown", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(false));

		// Token: 0x04003479 RID: 13433
		public static readonly DependencyProperty CanMoveDownProperty = DocumentViewer.CanMoveDownPropertyKey.DependencyProperty;

		// Token: 0x0400347A RID: 13434
		private static readonly DependencyPropertyKey CanMoveLeftPropertyKey = DependencyProperty.RegisterReadOnly("CanMoveLeft", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(false));

		// Token: 0x0400347B RID: 13435
		public static readonly DependencyProperty CanMoveLeftProperty = DocumentViewer.CanMoveLeftPropertyKey.DependencyProperty;

		// Token: 0x0400347C RID: 13436
		private static readonly DependencyPropertyKey CanMoveRightPropertyKey = DependencyProperty.RegisterReadOnly("CanMoveRight", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(false));

		// Token: 0x0400347D RID: 13437
		public static readonly DependencyProperty CanMoveRightProperty = DocumentViewer.CanMoveRightPropertyKey.DependencyProperty;

		// Token: 0x0400347E RID: 13438
		private static readonly DependencyPropertyKey CanIncreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanIncreaseZoom", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(true));

		// Token: 0x0400347F RID: 13439
		public static readonly DependencyProperty CanIncreaseZoomProperty = DocumentViewer.CanIncreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x04003480 RID: 13440
		private static readonly DependencyPropertyKey CanDecreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanDecreaseZoom", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(true));

		// Token: 0x04003481 RID: 13441
		public static readonly DependencyProperty CanDecreaseZoomProperty = DocumentViewer.CanDecreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x04003482 RID: 13442
		private IDocumentScrollInfo _documentScrollInfo;

		// Token: 0x04003483 RID: 13443
		private ScrollViewer _scrollViewer;

		// Token: 0x04003484 RID: 13444
		private ZoomPercentageConverter _zoomPercentageConverter;

		// Token: 0x04003485 RID: 13445
		private FindToolBar _findToolbar;

		// Token: 0x04003486 RID: 13446
		private const double _horizontalOffsetDefault = 0.0;

		// Token: 0x04003487 RID: 13447
		private const double _verticalOffsetDefault = 0.0;

		// Token: 0x04003488 RID: 13448
		private const double _extentWidthDefault = 0.0;

		// Token: 0x04003489 RID: 13449
		private const double _extentHeightDefault = 0.0;

		// Token: 0x0400348A RID: 13450
		private const double _viewportWidthDefault = 0.0;

		// Token: 0x0400348B RID: 13451
		private const double _viewportHeightDefault = 0.0;

		// Token: 0x0400348C RID: 13452
		private const bool _showPageBordersDefault = true;

		// Token: 0x0400348D RID: 13453
		private const double _zoomPercentageDefault = 100.0;

		// Token: 0x0400348E RID: 13454
		private const int _maxPagesAcrossDefault = 1;

		// Token: 0x0400348F RID: 13455
		private const double _verticalPageSpacingDefault = 10.0;

		// Token: 0x04003490 RID: 13456
		private const double _horizontalPageSpacingDefault = 10.0;

		// Token: 0x04003491 RID: 13457
		private const bool _canMoveUpDefault = false;

		// Token: 0x04003492 RID: 13458
		private const bool _canMoveDownDefault = false;

		// Token: 0x04003493 RID: 13459
		private const bool _canMoveLeftDefault = false;

		// Token: 0x04003494 RID: 13460
		private const bool _canMoveRightDefault = false;

		// Token: 0x04003495 RID: 13461
		private const bool _canIncreaseZoomDefault = true;

		// Token: 0x04003496 RID: 13462
		private const bool _canDecreaseZoomDefault = true;

		// Token: 0x04003497 RID: 13463
		private static RoutedUICommand _viewThumbnailsCommand;

		// Token: 0x04003498 RID: 13464
		private static RoutedUICommand _fitToWidthCommand;

		// Token: 0x04003499 RID: 13465
		private static RoutedUICommand _fitToHeightCommand;

		// Token: 0x0400349A RID: 13466
		private static RoutedUICommand _fitToMaxPagesAcrossCommand;

		// Token: 0x0400349B RID: 13467
		private static double[] _zoomLevelCollection = new double[]
		{
			5000.0,
			4000.0,
			3200.0,
			2400.0,
			2000.0,
			1600.0,
			1200.0,
			800.0,
			400.0,
			300.0,
			200.0,
			175.0,
			150.0,
			125.0,
			100.0,
			75.0,
			66.0,
			50.0,
			33.0,
			25.0,
			10.0,
			5.0
		};

		// Token: 0x0400349C RID: 13468
		private int _zoomLevelIndex;

		// Token: 0x0400349D RID: 13469
		private bool _zoomLevelIndexValid;

		// Token: 0x0400349E RID: 13470
		private bool _updatingInternalZoomLevel;

		// Token: 0x0400349F RID: 13471
		private bool _internalIDSIChange;

		// Token: 0x040034A0 RID: 13472
		private bool _pageViewCollectionChanged;

		// Token: 0x040034A1 RID: 13473
		private bool _firstDocumentAssignment = true;

		// Token: 0x040034A2 RID: 13474
		private const string _findToolBarHostName = "PART_FindToolBarHost";

		// Token: 0x040034A3 RID: 13475
		private const string _contentHostName = "PART_ContentHost";

		// Token: 0x040034A4 RID: 13476
		private static DependencyObjectType _dType;
	}
}
