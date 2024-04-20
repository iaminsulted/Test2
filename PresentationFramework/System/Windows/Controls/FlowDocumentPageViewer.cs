using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Printing;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007D7 RID: 2007
	[TemplatePart(Name = "PART_FindToolBarHost", Type = typeof(Decorator))]
	public class FlowDocumentPageViewer : DocumentViewerBase, IJournalState
	{
		// Token: 0x060072FA RID: 29434 RVA: 0x002E11EC File Offset: 0x002E01EC
		static FlowDocumentPageViewer()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIFlowDocumentPageViewer")));
			FlowDocumentPageViewer._dType = DependencyObjectType.FromSystemTypeInternal(typeof(FlowDocumentPageViewer));
			TextBoxBase.SelectionBrushProperty.OverrideMetadata(typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(FlowDocumentPageViewer.UpdateCaretElement)));
			TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(0.4, new PropertyChangedCallback(FlowDocumentPageViewer.UpdateCaretElement)));
			FlowDocumentPageViewer.CreateCommandBindings();
			EventManager.RegisterClassHandler(typeof(FlowDocumentPageViewer), Keyboard.KeyDownEvent, new KeyEventHandler(FlowDocumentPageViewer.KeyDownHandler), true);
		}

		// Token: 0x060072FB RID: 29435 RVA: 0x002E14DA File Offset: 0x002E04DA
		public FlowDocumentPageViewer()
		{
			base.LayoutUpdated += this.HandleLayoutUpdated;
		}

		// Token: 0x060072FC RID: 29436 RVA: 0x002E14FF File Offset: 0x002E04FF
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			this._findToolBarHost = (base.GetTemplateChild("PART_FindToolBarHost") as Decorator);
		}

		// Token: 0x060072FD RID: 29437 RVA: 0x002E152C File Offset: 0x002E052C
		public void IncreaseZoom()
		{
			this.OnIncreaseZoomCommand();
		}

		// Token: 0x060072FE RID: 29438 RVA: 0x002E1534 File Offset: 0x002E0534
		public void DecreaseZoom()
		{
			this.OnDecreaseZoomCommand();
		}

		// Token: 0x060072FF RID: 29439 RVA: 0x002E153C File Offset: 0x002E053C
		public void Find()
		{
			this.OnFindCommand();
		}

		// Token: 0x17001AA6 RID: 6822
		// (get) Token: 0x06007300 RID: 29440 RVA: 0x002E1544 File Offset: 0x002E0544
		public TextSelection Selection
		{
			get
			{
				ITextSelection textSelection = null;
				FlowDocument flowDocument = base.Document as FlowDocument;
				if (flowDocument != null)
				{
					textSelection = flowDocument.StructuralCache.TextContainer.TextSelection;
				}
				return textSelection as TextSelection;
			}
		}

		// Token: 0x17001AA7 RID: 6823
		// (get) Token: 0x06007301 RID: 29441 RVA: 0x002E1579 File Offset: 0x002E0579
		// (set) Token: 0x06007302 RID: 29442 RVA: 0x002E158B File Offset: 0x002E058B
		public double Zoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.ZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.ZoomProperty, value);
			}
		}

		// Token: 0x17001AA8 RID: 6824
		// (get) Token: 0x06007303 RID: 29443 RVA: 0x002E159E File Offset: 0x002E059E
		// (set) Token: 0x06007304 RID: 29444 RVA: 0x002E15B0 File Offset: 0x002E05B0
		public double MaxZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.MaxZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.MaxZoomProperty, value);
			}
		}

		// Token: 0x17001AA9 RID: 6825
		// (get) Token: 0x06007305 RID: 29445 RVA: 0x002E15C3 File Offset: 0x002E05C3
		// (set) Token: 0x06007306 RID: 29446 RVA: 0x002E15D5 File Offset: 0x002E05D5
		public double MinZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.MinZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.MinZoomProperty, value);
			}
		}

		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x06007307 RID: 29447 RVA: 0x002E15E8 File Offset: 0x002E05E8
		// (set) Token: 0x06007308 RID: 29448 RVA: 0x002E15FA File Offset: 0x002E05FA
		public double ZoomIncrement
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.ZoomIncrementProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.ZoomIncrementProperty, value);
			}
		}

		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x06007309 RID: 29449 RVA: 0x002E160D File Offset: 0x002E060D
		public virtual bool CanIncreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentPageViewer.CanIncreaseZoomProperty);
			}
		}

		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x0600730A RID: 29450 RVA: 0x002E161F File Offset: 0x002E061F
		public virtual bool CanDecreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentPageViewer.CanDecreaseZoomProperty);
			}
		}

		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x0600730B RID: 29451 RVA: 0x002E1631 File Offset: 0x002E0631
		// (set) Token: 0x0600730C RID: 29452 RVA: 0x002E1643 File Offset: 0x002E0643
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(FlowDocumentPageViewer.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.SelectionBrushProperty, value);
			}
		}

		// Token: 0x17001AAE RID: 6830
		// (get) Token: 0x0600730D RID: 29453 RVA: 0x002E1651 File Offset: 0x002E0651
		// (set) Token: 0x0600730E RID: 29454 RVA: 0x002E1663 File Offset: 0x002E0663
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.SelectionOpacityProperty, value);
			}
		}

		// Token: 0x17001AAF RID: 6831
		// (get) Token: 0x0600730F RID: 29455 RVA: 0x002E1676 File Offset: 0x002E0676
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentPageViewer.IsSelectionActiveProperty);
			}
		}

		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x06007310 RID: 29456 RVA: 0x002E1688 File Offset: 0x002E0688
		// (set) Token: 0x06007311 RID: 29457 RVA: 0x002E169A File Offset: 0x002E069A
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentPageViewer.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		// Token: 0x06007312 RID: 29458 RVA: 0x002E16A8 File Offset: 0x002E06A8
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FlowDocumentPageViewerAutomationPeer(this);
		}

		// Token: 0x06007313 RID: 29459 RVA: 0x002E16B0 File Offset: 0x002E06B0
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Escape && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
				e.Handled = true;
			}
			if (e.Key == Key.F3 && this.CanShowFindToolBar)
			{
				if (this.FindToolBar != null)
				{
					this.FindToolBar.SearchUp = ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
					this.OnFindInvoked(this, EventArgs.Empty);
				}
				else
				{
					this.ToggleFindToolBar(true);
				}
				e.Handled = true;
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x06007314 RID: 29460 RVA: 0x002E1740 File Offset: 0x002E0740
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Delta != 0)
			{
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					if (e.Delta > 0)
					{
						this.IncreaseZoom();
					}
					else
					{
						this.DecreaseZoom();
					}
				}
				else if (e.Delta > 0)
				{
					base.PreviousPage();
				}
				else
				{
					base.NextPage();
				}
				e.Handled = false;
			}
			if (!e.Handled)
			{
				base.OnMouseWheel(e);
			}
		}

		// Token: 0x06007315 RID: 29461 RVA: 0x002E17A5 File Offset: 0x002E07A5
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			DocumentViewerHelper.OnContextMenuOpening(base.Document as FlowDocument, this, e);
		}

		// Token: 0x06007316 RID: 29462 RVA: 0x002E17C0 File Offset: 0x002E07C0
		protected override void OnPageViewsChanged()
		{
			this._contentPosition = null;
			this.ApplyZoom();
			base.OnPageViewsChanged();
		}

		// Token: 0x06007317 RID: 29463 RVA: 0x002E17D8 File Offset: 0x002E07D8
		protected override void OnDocumentChanged()
		{
			this._contentPosition = null;
			if (this._oldDocument != null)
			{
				DynamicDocumentPaginator dynamicDocumentPaginator = this._oldDocument.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null)
				{
					dynamicDocumentPaginator.GetPageNumberCompleted -= this.HandleGetPageNumberCompleted;
				}
				FlowDocumentPaginator flowDocumentPaginator = this._oldDocument.DocumentPaginator as FlowDocumentPaginator;
				if (flowDocumentPaginator != null)
				{
					flowDocumentPaginator.BreakRecordTableInvalidated -= this.HandleAllBreakRecordsInvalidated;
				}
			}
			base.OnDocumentChanged();
			this._oldDocument = base.Document;
			if (base.Document != null && !(base.Document is FlowDocument))
			{
				base.Document = null;
				throw new NotSupportedException(SR.Get("FlowDocumentPageViewerOnlySupportsFlowDocument"));
			}
			if (base.Document != null)
			{
				DynamicDocumentPaginator dynamicDocumentPaginator2 = base.Document.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator2 != null)
				{
					dynamicDocumentPaginator2.GetPageNumberCompleted += this.HandleGetPageNumberCompleted;
				}
				FlowDocumentPaginator flowDocumentPaginator2 = base.Document.DocumentPaginator as FlowDocumentPaginator;
				if (flowDocumentPaginator2 != null)
				{
					flowDocumentPaginator2.BreakRecordTableInvalidated += this.HandleAllBreakRecordsInvalidated;
				}
			}
			if (!this.CanShowFindToolBar && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			this.OnGoToPageCommand(1);
		}

		// Token: 0x06007318 RID: 29464 RVA: 0x002E18F0 File Offset: 0x002E08F0
		protected virtual void OnPrintCompleted()
		{
			this.ClearPrintingState();
		}

		// Token: 0x06007319 RID: 29465 RVA: 0x002E18F8 File Offset: 0x002E08F8
		protected override void OnPreviousPageCommand()
		{
			if (this.CanGoToPreviousPage)
			{
				this._contentPosition = null;
				base.OnPreviousPageCommand();
			}
		}

		// Token: 0x0600731A RID: 29466 RVA: 0x002E190F File Offset: 0x002E090F
		protected override void OnNextPageCommand()
		{
			if (this.CanGoToNextPage)
			{
				this._contentPosition = null;
				base.OnNextPageCommand();
			}
		}

		// Token: 0x0600731B RID: 29467 RVA: 0x002E1926 File Offset: 0x002E0926
		protected override void OnFirstPageCommand()
		{
			if (this.CanGoToPreviousPage)
			{
				this._contentPosition = null;
				base.OnFirstPageCommand();
			}
		}

		// Token: 0x0600731C RID: 29468 RVA: 0x002E193D File Offset: 0x002E093D
		protected override void OnLastPageCommand()
		{
			if (this.CanGoToNextPage)
			{
				this._contentPosition = null;
				base.OnLastPageCommand();
			}
		}

		// Token: 0x0600731D RID: 29469 RVA: 0x002E1954 File Offset: 0x002E0954
		protected override void OnGoToPageCommand(int pageNumber)
		{
			if (this.CanGoToPage(pageNumber) && this.MasterPageNumber != pageNumber)
			{
				this._contentPosition = null;
				base.OnGoToPageCommand(pageNumber);
			}
		}

		// Token: 0x0600731E RID: 29470 RVA: 0x002E1976 File Offset: 0x002E0976
		protected virtual void OnFindCommand()
		{
			if (this.CanShowFindToolBar)
			{
				this.ToggleFindToolBar(this.FindToolBar == null);
			}
		}

		// Token: 0x0600731F RID: 29471 RVA: 0x002E1990 File Offset: 0x002E0990
		protected override void OnPrintCommand()
		{
			PrintDocumentImageableArea printDocumentImageableArea = null;
			FlowDocument flowDocument = base.Document as FlowDocument;
			if (this._printingState != null)
			{
				return;
			}
			if (flowDocument != null)
			{
				XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref printDocumentImageableArea);
				if (xpsDocumentWriter != null && printDocumentImageableArea != null)
				{
					FlowDocumentPaginator flowDocumentPaginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator as FlowDocumentPaginator;
					this._printingState = new FlowDocumentPrintingState();
					this._printingState.XpsDocumentWriter = xpsDocumentWriter;
					this._printingState.PageSize = flowDocumentPaginator.PageSize;
					this._printingState.PagePadding = flowDocument.PagePadding;
					this._printingState.IsSelectionEnabled = base.IsSelectionEnabled;
					CommandManager.InvalidateRequerySuggested();
					xpsDocumentWriter.WritingCompleted += this.HandlePrintCompleted;
					xpsDocumentWriter.WritingCancelled += this.HandlePrintCancelled;
					CommandManager.AddPreviewCanExecuteHandler(this, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
					ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
					for (int i = 0; i < pageViews.Count; i++)
					{
						pageViews[i].SuspendLayout();
					}
					if (base.IsSelectionEnabled)
					{
						base.IsSelectionEnabled = false;
					}
					flowDocumentPaginator.PageSize = new Size(printDocumentImageableArea.MediaSizeWidth, printDocumentImageableArea.MediaSizeHeight);
					Thickness thickness = flowDocument.ComputePageMargin();
					flowDocument.PagePadding = new Thickness(Math.Max(printDocumentImageableArea.OriginWidth, thickness.Left), Math.Max(printDocumentImageableArea.OriginHeight, thickness.Top), Math.Max(printDocumentImageableArea.MediaSizeWidth - (printDocumentImageableArea.OriginWidth + printDocumentImageableArea.ExtentWidth), thickness.Right), Math.Max(printDocumentImageableArea.MediaSizeHeight - (printDocumentImageableArea.OriginHeight + printDocumentImageableArea.ExtentHeight), thickness.Bottom));
					xpsDocumentWriter.WriteAsync(flowDocumentPaginator);
					return;
				}
			}
			else
			{
				base.OnPrintCommand();
			}
		}

		// Token: 0x06007320 RID: 29472 RVA: 0x002E1B36 File Offset: 0x002E0B36
		protected override void OnCancelPrintCommand()
		{
			if (this._printingState != null)
			{
				this._printingState.XpsDocumentWriter.CancelAsync();
				return;
			}
			base.OnCancelPrintCommand();
		}

		// Token: 0x06007321 RID: 29473 RVA: 0x002E1B57 File Offset: 0x002E0B57
		protected virtual void OnIncreaseZoomCommand()
		{
			if (this.CanIncreaseZoom)
			{
				base.SetCurrentValueInternal(FlowDocumentPageViewer.ZoomProperty, Math.Min(this.Zoom + this.ZoomIncrement, this.MaxZoom));
			}
		}

		// Token: 0x06007322 RID: 29474 RVA: 0x002E1B89 File Offset: 0x002E0B89
		protected virtual void OnDecreaseZoomCommand()
		{
			if (this.CanDecreaseZoom)
			{
				base.SetCurrentValueInternal(FlowDocumentPageViewer.ZoomProperty, Math.Max(this.Zoom - this.ZoomIncrement, this.MinZoom));
			}
		}

		// Token: 0x06007323 RID: 29475 RVA: 0x002E1BBC File Offset: 0x002E0BBC
		internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			DependencyObject dependencyObject = base.Document as DependencyObject;
			if (dependencyObject != null && LogicalTreeHelper.GetParent(dependencyObject) != this)
			{
				DependencyObject dependencyObject2 = route.PeekBranchNode() as DependencyObject;
				if (dependencyObject2 != null && DocumentViewerHelper.IsLogicalDescendent(dependencyObject2, dependencyObject))
				{
					FrameworkElement.AddIntermediateElementsToRoute(LogicalTreeHelper.GetParent(dependencyObject), route, args, LogicalTreeHelper.GetParent(dependencyObject2));
				}
			}
			return base.BuildRouteCore(route, args);
		}

		// Token: 0x06007324 RID: 29476 RVA: 0x002E1C14 File Offset: 0x002E0C14
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool flag = true;
			DependencyObject dependencyObject = base.Document as DependencyObject;
			if (dependencyObject != null && LogicalTreeHelper.GetParent(dependencyObject) != this)
			{
				DependencyObject dependencyObject2 = (branchNodeStack.Count > 0) ? branchNodeStack.Peek() : null;
				if (dependencyObject2 != null && DocumentViewerHelper.IsLogicalDescendent(dependencyObject2, dependencyObject))
				{
					flag = FrameworkElement.InvalidateAutomationIntermediateElements(LogicalTreeHelper.GetParent(dependencyObject), LogicalTreeHelper.GetParent(dependencyObject2));
				}
			}
			return flag & base.InvalidateAutomationAncestorsCore(branchNodeStack, out continuePastCoreTree);
		}

		// Token: 0x06007325 RID: 29477 RVA: 0x002E1C78 File Offset: 0x002E0C78
		internal bool BringPointIntoView(Point point)
		{
			ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
			bool result = false;
			if (pageViews.Count > 0)
			{
				Rect[] array = new Rect[pageViews.Count];
				int i;
				for (i = 0; i < pageViews.Count; i++)
				{
					Rect rect = new Rect(pageViews[i].RenderSize);
					rect = pageViews[i].TransformToAncestor(this).TransformBounds(rect);
					array[i] = rect;
				}
				i = 0;
				while (i < array.Length && !array[i].Contains(point))
				{
					i++;
				}
				if (i >= array.Length)
				{
					Rect rect = array[0];
					for (i = 1; i < array.Length; i++)
					{
						rect.Union(array[i]);
					}
					if (DoubleUtil.LessThan(point.X, rect.Left))
					{
						if (this.CanGoToPreviousPage)
						{
							this.OnPreviousPageCommand();
							result = true;
						}
					}
					else if (DoubleUtil.GreaterThan(point.X, rect.Right))
					{
						if (this.CanGoToNextPage)
						{
							this.OnNextPageCommand();
							result = true;
						}
					}
					else if (DoubleUtil.LessThan(point.Y, rect.Top))
					{
						if (this.CanGoToPreviousPage)
						{
							this.OnPreviousPageCommand();
							result = true;
						}
					}
					else if (DoubleUtil.GreaterThan(point.Y, rect.Bottom) && this.CanGoToNextPage)
					{
						this.OnNextPageCommand();
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06007326 RID: 29478 RVA: 0x002E1DD4 File Offset: 0x002E0DD4
		internal object BringContentPositionIntoView(object arg)
		{
			this.PrivateBringContentPositionIntoView(arg, false);
			return null;
		}

		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x06007327 RID: 29479 RVA: 0x002E1DDF File Offset: 0x002E0DDF
		internal ContentPosition ContentPosition
		{
			get
			{
				return this._contentPosition;
			}
		}

		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x06007328 RID: 29480 RVA: 0x002E1DE7 File Offset: 0x002E0DE7
		internal bool CanShowFindToolBar
		{
			get
			{
				return this._findToolBarHost != null && base.Document != null && base.TextEditor != null;
			}
		}

		// Token: 0x17001AB3 RID: 6835
		// (get) Token: 0x06007329 RID: 29481 RVA: 0x002E1E04 File Offset: 0x002E0E04
		internal bool IsPrinting
		{
			get
			{
				return this._printingState != null;
			}
		}

		// Token: 0x0600732A RID: 29482 RVA: 0x002E1E10 File Offset: 0x002E0E10
		private void HandleLayoutUpdated(object sender, EventArgs e)
		{
			if (base.Document != null && this._printingState == null)
			{
				DynamicDocumentPaginator dynamicDocumentPaginator = base.Document.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null)
				{
					if (this._contentPosition == null)
					{
						DocumentPageView masterPageView = base.GetMasterPageView();
						if (masterPageView != null && masterPageView.DocumentPage != null)
						{
							this._contentPosition = dynamicDocumentPaginator.GetPagePosition(masterPageView.DocumentPage);
						}
						if (this._contentPosition == ContentPosition.Missing)
						{
							this._contentPosition = null;
							return;
						}
					}
					else
					{
						this.PrivateBringContentPositionIntoView(this._contentPosition, true);
					}
				}
			}
		}

		// Token: 0x0600732B RID: 29483 RVA: 0x002E1E90 File Offset: 0x002E0E90
		private void HandleGetPageNumberCompleted(object sender, GetPageNumberCompletedEventArgs e)
		{
			if (base.Document != null && sender == base.Document.DocumentPaginator && e != null && !e.Cancelled && e.Error == null && e.UserState == this._bringContentPositionIntoViewToken)
			{
				int pageNumber = e.PageNumber + 1;
				this.OnGoToPageCommand(pageNumber);
			}
		}

		// Token: 0x0600732C RID: 29484 RVA: 0x002E1EE4 File Offset: 0x002E0EE4
		private void HandleAllBreakRecordsInvalidated(object sender, EventArgs e)
		{
			ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
			for (int i = 0; i < pageViews.Count; i++)
			{
				pageViews[i].DuplicateVisual();
			}
		}

		// Token: 0x0600732D RID: 29485 RVA: 0x002E1F18 File Offset: 0x002E0F18
		private bool IsValidContentPositionForDocument(IDocumentPaginatorSource document, ContentPosition contentPosition)
		{
			FlowDocument flowDocument = document as FlowDocument;
			TextPointer textPointer = contentPosition as TextPointer;
			return flowDocument == null || textPointer == null || flowDocument.ContentStart.TextContainer == textPointer.TextContainer;
		}

		// Token: 0x0600732E RID: 29486 RVA: 0x002E1F50 File Offset: 0x002E0F50
		private void PrivateBringContentPositionIntoView(object arg, bool isAsyncRequest)
		{
			ContentPosition contentPosition = arg as ContentPosition;
			if (contentPosition != null && base.Document != null)
			{
				DynamicDocumentPaginator dynamicDocumentPaginator = base.Document.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null && this.IsValidContentPositionForDocument(base.Document, contentPosition))
				{
					dynamicDocumentPaginator.CancelAsync(this._bringContentPositionIntoViewToken);
					if (isAsyncRequest)
					{
						dynamicDocumentPaginator.GetPageNumberAsync(contentPosition, this._bringContentPositionIntoViewToken);
					}
					else
					{
						int pageNumber = dynamicDocumentPaginator.GetPageNumber(contentPosition) + 1;
						this.OnGoToPageCommand(pageNumber);
					}
					this._contentPosition = contentPosition;
				}
			}
		}

		// Token: 0x0600732F RID: 29487 RVA: 0x002E1FC8 File Offset: 0x002E0FC8
		private void HandlePrintCompleted(object sender, WritingCompletedEventArgs e)
		{
			this.OnPrintCompleted();
		}

		// Token: 0x06007330 RID: 29488 RVA: 0x002E18F0 File Offset: 0x002E08F0
		private void HandlePrintCancelled(object sender, WritingCancelledEventArgs e)
		{
			this.ClearPrintingState();
		}

		// Token: 0x06007331 RID: 29489 RVA: 0x002E1FD0 File Offset: 0x002E0FD0
		private void ClearPrintingState()
		{
			if (this._printingState != null)
			{
				ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
				for (int i = 0; i < pageViews.Count; i++)
				{
					pageViews[i].ResumeLayout();
				}
				if (this._printingState.IsSelectionEnabled)
				{
					base.IsSelectionEnabled = true;
				}
				CommandManager.RemovePreviewCanExecuteHandler(this, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				this._printingState.XpsDocumentWriter.WritingCompleted -= this.HandlePrintCompleted;
				this._printingState.XpsDocumentWriter.WritingCancelled -= this.HandlePrintCancelled;
				((FlowDocument)base.Document).PagePadding = this._printingState.PagePadding;
				base.Document.DocumentPaginator.PageSize = this._printingState.PageSize;
				this._printingState = null;
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x06007332 RID: 29490 RVA: 0x002E20AC File Offset: 0x002E10AC
		private void ApplyZoom()
		{
			ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
			for (int i = 0; i < pageViews.Count; i++)
			{
				pageViews[i].SetPageZoom(this.Zoom / 100.0);
			}
		}

		// Token: 0x06007333 RID: 29491 RVA: 0x002E20ED File Offset: 0x002E10ED
		private void ToggleFindToolBar(bool enable)
		{
			Invariant.Assert(enable == (this.FindToolBar == null));
			DocumentViewerHelper.ToggleFindToolBar(this._findToolBarHost, new EventHandler(this.OnFindInvoked), enable);
		}

		// Token: 0x06007334 RID: 29492 RVA: 0x002E2118 File Offset: 0x002E1118
		private void OnFindInvoked(object sender, EventArgs e)
		{
			FindToolBar findToolBar = this.FindToolBar;
			if (findToolBar != null && base.TextEditor != null)
			{
				base.Focus();
				ITextRange textRange = base.Find(findToolBar);
				if (textRange != null && !textRange.IsEmpty)
				{
					if (textRange.Start is ContentPosition)
					{
						this._contentPosition = (ContentPosition)textRange.Start;
						int pageNumber = ((DynamicDocumentPaginator)base.Document.DocumentPaginator).GetPageNumber(this._contentPosition) + 1;
						this.OnBringIntoView(this, Rect.Empty, pageNumber);
						return;
					}
				}
				else
				{
					DocumentViewerHelper.ShowFindUnsuccessfulMessage(findToolBar);
				}
			}
		}

		// Token: 0x06007335 RID: 29493 RVA: 0x002E21A1 File Offset: 0x002E11A1
		private void ZoomChanged(double oldValue, double newValue)
		{
			if (!DoubleUtil.AreClose(oldValue, newValue))
			{
				this.UpdateCanIncreaseZoom();
				this.UpdateCanDecreaseZoom();
				this.ApplyZoom();
			}
		}

		// Token: 0x06007336 RID: 29494 RVA: 0x002E21BE File Offset: 0x002E11BE
		private void UpdateCanIncreaseZoom()
		{
			base.SetValue(FlowDocumentPageViewer.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(this.MaxZoom, this.Zoom)));
		}

		// Token: 0x06007337 RID: 29495 RVA: 0x002E21E1 File Offset: 0x002E11E1
		private void UpdateCanDecreaseZoom()
		{
			base.SetValue(FlowDocumentPageViewer.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(this.MinZoom, this.Zoom)));
		}

		// Token: 0x06007338 RID: 29496 RVA: 0x002E2204 File Offset: 0x002E1204
		private void MaxZoomChanged(double oldValue, double newValue)
		{
			base.CoerceValue(FlowDocumentPageViewer.ZoomProperty);
			this.UpdateCanIncreaseZoom();
		}

		// Token: 0x06007339 RID: 29497 RVA: 0x002E2217 File Offset: 0x002E1217
		private void MinZoomChanged(double oldValue, double newValue)
		{
			base.CoerceValue(FlowDocumentPageViewer.MaxZoomProperty);
			base.CoerceValue(FlowDocumentPageViewer.ZoomProperty);
			this.UpdateCanIncreaseZoom();
			this.UpdateCanDecreaseZoom();
		}

		// Token: 0x0600733A RID: 29498 RVA: 0x002E223C File Offset: 0x002E123C
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(FlowDocumentPageViewer.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(FlowDocumentPageViewer.CanExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentPageViewer), ApplicationCommands.Find, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentPageViewer), NavigationCommands.IncreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemPlus, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentPageViewer), NavigationCommands.DecreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemMinus, ModifierKeys.Control));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.PreviousPage, new KeyGesture(Key.Left)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.PreviousPage, new KeyGesture(Key.Up)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.PreviousPage, new KeyGesture(Key.Prior)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.NextPage, new KeyGesture(Key.Right)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.NextPage, new KeyGesture(Key.Down)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.NextPage, new KeyGesture(Key.Next)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.FirstPage, new KeyGesture(Key.Home)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.FirstPage, new KeyGesture(Key.Home, ModifierKeys.Control)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.LastPage, new KeyGesture(Key.End)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.LastPage, new KeyGesture(Key.End, ModifierKeys.Control)));
		}

		// Token: 0x0600733B RID: 29499 RVA: 0x002E2400 File Offset: 0x002E1400
		private static void CanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			FlowDocumentPageViewer flowDocumentPageViewer = target as FlowDocumentPageViewer;
			Invariant.Assert(flowDocumentPageViewer != null, "Target of QueryEnabledEvent must be FlowDocumentPageViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == ApplicationCommands.Find)
			{
				args.CanExecute = flowDocumentPageViewer.CanShowFindToolBar;
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x0600733C RID: 29500 RVA: 0x002E2454 File Offset: 0x002E1454
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			FlowDocumentPageViewer flowDocumentPageViewer = target as FlowDocumentPageViewer;
			Invariant.Assert(flowDocumentPageViewer != null, "Target of ExecuteEvent must be FlowDocumentPageViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				flowDocumentPageViewer.OnIncreaseZoomCommand();
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				flowDocumentPageViewer.OnDecreaseZoomCommand();
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				flowDocumentPageViewer.OnFindCommand();
				return;
			}
			Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
		}

		// Token: 0x0600733D RID: 29501 RVA: 0x002E24CC File Offset: 0x002E14CC
		private void PreviewCanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			FlowDocumentPageViewer flowDocumentPageViewer = target as FlowDocumentPageViewer;
			Invariant.Assert(flowDocumentPageViewer != null, "Target of PreviewCanExecuteRoutedEventHandler must be FlowDocumentPageViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (flowDocumentPageViewer._printingState != null && args.Command != ApplicationCommands.CancelPrint)
			{
				args.CanExecute = false;
				args.Handled = true;
			}
		}

		// Token: 0x0600733E RID: 29502 RVA: 0x002E251D File Offset: 0x002E151D
		private static void KeyDownHandler(object sender, KeyEventArgs e)
		{
			DocumentViewerHelper.KeyDownHelper(e, ((FlowDocumentPageViewer)sender)._findToolBarHost);
		}

		// Token: 0x0600733F RID: 29503 RVA: 0x002E2530 File Offset: 0x002E1530
		private static object CoerceZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			FlowDocumentPageViewer flowDocumentPageViewer = (FlowDocumentPageViewer)d;
			double value2 = (double)value;
			double maxZoom = flowDocumentPageViewer.MaxZoom;
			if (DoubleUtil.LessThan(maxZoom, value2))
			{
				return maxZoom;
			}
			double minZoom = flowDocumentPageViewer.MinZoom;
			if (DoubleUtil.GreaterThan(minZoom, value2))
			{
				return minZoom;
			}
			return value;
		}

		// Token: 0x06007340 RID: 29504 RVA: 0x002E2590 File Offset: 0x002E1590
		private static object CoerceMaxZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			double minZoom = ((FlowDocumentPageViewer)d).MinZoom;
			if (DoubleUtil.LessThan((double)value, minZoom))
			{
				return minZoom;
			}
			return value;
		}

		// Token: 0x06007341 RID: 29505 RVA: 0x002E25D3 File Offset: 0x002E15D3
		private static void ZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			((FlowDocumentPageViewer)d).ZoomChanged((double)e.OldValue, (double)e.NewValue);
		}

		// Token: 0x06007342 RID: 29506 RVA: 0x002E260C File Offset: 0x002E160C
		private static void MaxZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			((FlowDocumentPageViewer)d).MaxZoomChanged((double)e.OldValue, (double)e.NewValue);
		}

		// Token: 0x06007343 RID: 29507 RVA: 0x002E2645 File Offset: 0x002E1645
		private static void MinZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			((FlowDocumentPageViewer)d).MinZoomChanged((double)e.OldValue, (double)e.NewValue);
		}

		// Token: 0x06007344 RID: 29508 RVA: 0x002BA734 File Offset: 0x002B9734
		private static bool ZoomValidateValue(object o)
		{
			double num = (double)o;
			return !double.IsNaN(num) && !double.IsInfinity(num) && DoubleUtil.GreaterThan(num, 0.0);
		}

		// Token: 0x06007345 RID: 29509 RVA: 0x002E2680 File Offset: 0x002E1680
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlowDocumentPageViewer flowDocumentPageViewer = (FlowDocumentPageViewer)d;
			if (flowDocumentPageViewer.Selection != null)
			{
				CaretElement caretElement = flowDocumentPageViewer.Selection.CaretElement;
				if (caretElement != null)
				{
					caretElement.InvalidateVisual();
				}
			}
		}

		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x06007346 RID: 29510 RVA: 0x002E26B1 File Offset: 0x002E16B1
		private FindToolBar FindToolBar
		{
			get
			{
				if (this._findToolBarHost == null)
				{
					return null;
				}
				return this._findToolBarHost.Child as FindToolBar;
			}
		}

		// Token: 0x06007347 RID: 29511 RVA: 0x002E26D0 File Offset: 0x002E16D0
		CustomJournalStateInternal IJournalState.GetJournalState(JournalReason journalReason)
		{
			int contentPosition = -1;
			LogicalDirection contentPositionDirection = LogicalDirection.Forward;
			TextPointer textPointer = this.ContentPosition as TextPointer;
			if (textPointer != null)
			{
				contentPosition = textPointer.Offset;
				contentPositionDirection = textPointer.LogicalDirection;
			}
			return new FlowDocumentPageViewer.JournalState(contentPosition, contentPositionDirection, this.Zoom);
		}

		// Token: 0x06007348 RID: 29512 RVA: 0x002E270C File Offset: 0x002E170C
		void IJournalState.RestoreJournalState(CustomJournalStateInternal state)
		{
			FlowDocumentPageViewer.JournalState journalState = state as FlowDocumentPageViewer.JournalState;
			if (state != null)
			{
				base.SetCurrentValueInternal(FlowDocumentPageViewer.ZoomProperty, journalState.Zoom);
				if (journalState.ContentPosition != -1)
				{
					FlowDocument flowDocument = base.Document as FlowDocument;
					if (flowDocument != null)
					{
						TextContainer textContainer = flowDocument.StructuralCache.TextContainer;
						if (journalState.ContentPosition <= textContainer.SymbolCount)
						{
							TextPointer arg = textContainer.CreatePointerAtOffset(journalState.ContentPosition, journalState.ContentPositionDirection);
							base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.BringContentPositionIntoView), arg);
						}
					}
				}
			}
		}

		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x06007349 RID: 29513 RVA: 0x002E2798 File Offset: 0x002E1798
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return FlowDocumentPageViewer._dType;
			}
		}

		// Token: 0x040037AE RID: 14254
		public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(100.0, new PropertyChangedCallback(FlowDocumentPageViewer.ZoomChanged), new CoerceValueCallback(FlowDocumentPageViewer.CoerceZoom)), new ValidateValueCallback(FlowDocumentPageViewer.ZoomValidateValue));

		// Token: 0x040037AF RID: 14255
		public static readonly DependencyProperty MaxZoomProperty = DependencyProperty.Register("MaxZoom", typeof(double), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(200.0, new PropertyChangedCallback(FlowDocumentPageViewer.MaxZoomChanged), new CoerceValueCallback(FlowDocumentPageViewer.CoerceMaxZoom)), new ValidateValueCallback(FlowDocumentPageViewer.ZoomValidateValue));

		// Token: 0x040037B0 RID: 14256
		public static readonly DependencyProperty MinZoomProperty = DependencyProperty.Register("MinZoom", typeof(double), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(80.0, new PropertyChangedCallback(FlowDocumentPageViewer.MinZoomChanged)), new ValidateValueCallback(FlowDocumentPageViewer.ZoomValidateValue));

		// Token: 0x040037B1 RID: 14257
		public static readonly DependencyProperty ZoomIncrementProperty = DependencyProperty.Register("ZoomIncrement", typeof(double), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(10.0), new ValidateValueCallback(FlowDocumentPageViewer.ZoomValidateValue));

		// Token: 0x040037B2 RID: 14258
		protected static readonly DependencyPropertyKey CanIncreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanIncreaseZoom", typeof(bool), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(true));

		// Token: 0x040037B3 RID: 14259
		public static readonly DependencyProperty CanIncreaseZoomProperty = FlowDocumentPageViewer.CanIncreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x040037B4 RID: 14260
		protected static readonly DependencyPropertyKey CanDecreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanDecreaseZoom", typeof(bool), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(true));

		// Token: 0x040037B5 RID: 14261
		public static readonly DependencyProperty CanDecreaseZoomProperty = FlowDocumentPageViewer.CanDecreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x040037B6 RID: 14262
		public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(FlowDocumentPageViewer));

		// Token: 0x040037B7 RID: 14263
		public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(FlowDocumentPageViewer));

		// Token: 0x040037B8 RID: 14264
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(FlowDocumentPageViewer));

		// Token: 0x040037B9 RID: 14265
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = TextBoxBase.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(FlowDocumentPageViewer));

		// Token: 0x040037BA RID: 14266
		private Decorator _findToolBarHost;

		// Token: 0x040037BB RID: 14267
		private ContentPosition _contentPosition;

		// Token: 0x040037BC RID: 14268
		private FlowDocumentPrintingState _printingState;

		// Token: 0x040037BD RID: 14269
		private IDocumentPaginatorSource _oldDocument;

		// Token: 0x040037BE RID: 14270
		private object _bringContentPositionIntoViewToken = new object();

		// Token: 0x040037BF RID: 14271
		private const string _findToolBarHostTemplateName = "PART_FindToolBarHost";

		// Token: 0x040037C0 RID: 14272
		private static DependencyObjectType _dType;

		// Token: 0x02000C1D RID: 3101
		[Serializable]
		private class JournalState : CustomJournalStateInternal
		{
			// Token: 0x06009089 RID: 37001 RVA: 0x00346C2F File Offset: 0x00345C2F
			public JournalState(int contentPosition, LogicalDirection contentPositionDirection, double zoom)
			{
				this.ContentPosition = contentPosition;
				this.ContentPositionDirection = contentPositionDirection;
				this.Zoom = zoom;
			}

			// Token: 0x04004B25 RID: 19237
			public int ContentPosition;

			// Token: 0x04004B26 RID: 19238
			public LogicalDirection ContentPositionDirection;

			// Token: 0x04004B27 RID: 19239
			public double Zoom;
		}
	}
}
