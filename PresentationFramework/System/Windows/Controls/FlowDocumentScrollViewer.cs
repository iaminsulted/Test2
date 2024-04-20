using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Printing;
using System.Windows.Annotations;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps;
using MS.Internal;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.AppModel;
using MS.Internal.Commands;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x0200077E RID: 1918
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
	[TemplatePart(Name = "PART_FindToolBarHost", Type = typeof(Decorator))]
	[TemplatePart(Name = "PART_ToolBarHost", Type = typeof(Decorator))]
	[ContentProperty("Document")]
	public class FlowDocumentScrollViewer : Control, IAddChild, IServiceProvider, IJournalState
	{
		// Token: 0x060068EC RID: 26860 RVA: 0x002BA9FC File Offset: 0x002B99FC
		static FlowDocumentScrollViewer()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIFlowDocumentScrollViewer")));
			FlowDocumentScrollViewer._dType = DependencyObjectType.FromSystemTypeInternal(typeof(FlowDocumentScrollViewer));
			TextBoxBase.SelectionBrushProperty.OverrideMetadata(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(FlowDocumentScrollViewer.UpdateCaretElement)));
			TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(0.4, new PropertyChangedCallback(FlowDocumentScrollViewer.UpdateCaretElement)));
			FlowDocumentScrollViewer.CreateCommandBindings();
			EventManager.RegisterClassHandler(typeof(FlowDocumentScrollViewer), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(FlowDocumentScrollViewer.HandleRequestBringIntoView));
			EventManager.RegisterClassHandler(typeof(FlowDocumentScrollViewer), Keyboard.KeyDownEvent, new KeyEventHandler(FlowDocumentScrollViewer.KeyDownHandler), true);
		}

		// Token: 0x060068ED RID: 26861 RVA: 0x002BAD92 File Offset: 0x002B9D92
		public FlowDocumentScrollViewer()
		{
			AnnotationService.SetDataId(this, "FlowDocument");
		}

		// Token: 0x060068EE RID: 26862 RVA: 0x002BADA8 File Offset: 0x002B9DA8
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			this._findToolBarHost = (base.GetTemplateChild("PART_FindToolBarHost") as Decorator);
			this._toolBarHost = (base.GetTemplateChild("PART_ToolBarHost") as Decorator);
			if (this._toolBarHost != null)
			{
				this._toolBarHost.Visibility = (this.IsToolBarVisible ? Visibility.Visible : Visibility.Collapsed);
			}
			if (this._contentHost != null)
			{
				BindingOperations.ClearBinding(this._contentHost, FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty);
				BindingOperations.ClearBinding(this._contentHost, FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty);
				this._contentHost.ScrollChanged -= this.OnScrollChanged;
				this.RenderScope.Document = null;
				base.ClearValue(TextEditor.PageHeightProperty);
				this._contentHost.Content = null;
			}
			this._contentHost = (base.GetTemplateChild("PART_ContentHost") as ScrollViewer);
			if (this._contentHost != null)
			{
				if (this._contentHost.Content != null)
				{
					throw new NotSupportedException(SR.Get("FlowDocumentScrollViewerMarkedAsContentHostMustHaveNoContent"));
				}
				this._contentHost.ScrollChanged += this.OnScrollChanged;
				this.CreateTwoWayBinding(this._contentHost, FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty, "HorizontalScrollBarVisibility");
				this.CreateTwoWayBinding(this._contentHost, FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty, "VerticalScrollBarVisibility");
				this._contentHost.Focusable = false;
				this._contentHost.Content = new FlowDocumentView();
				this.RenderScope.Document = this.Document;
			}
			this.AttachTextEditor();
			this.ApplyZoom();
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x002BAF31 File Offset: 0x002B9F31
		public void Find()
		{
			this.OnFindCommand();
		}

		// Token: 0x060068F0 RID: 26864 RVA: 0x0013F419 File Offset: 0x0013E419
		public void Print()
		{
			this.OnPrintCommand();
		}

		// Token: 0x060068F1 RID: 26865 RVA: 0x0013F421 File Offset: 0x0013E421
		public void CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x002BAF39 File Offset: 0x002B9F39
		public void IncreaseZoom()
		{
			this.OnIncreaseZoomCommand();
		}

		// Token: 0x060068F3 RID: 26867 RVA: 0x002BAF41 File Offset: 0x002B9F41
		public void DecreaseZoom()
		{
			this.OnDecreaseZoomCommand();
		}

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x060068F4 RID: 26868 RVA: 0x002BAF49 File Offset: 0x002B9F49
		// (set) Token: 0x060068F5 RID: 26869 RVA: 0x002BAF5B File Offset: 0x002B9F5B
		public FlowDocument Document
		{
			get
			{
				return (FlowDocument)base.GetValue(FlowDocumentScrollViewer.DocumentProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.DocumentProperty, value);
			}
		}

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x060068F6 RID: 26870 RVA: 0x002BAF6C File Offset: 0x002B9F6C
		public TextSelection Selection
		{
			get
			{
				ITextSelection textSelection = null;
				FlowDocument document = this.Document;
				if (document != null)
				{
					textSelection = document.StructuralCache.TextContainer.TextSelection;
				}
				return textSelection as TextSelection;
			}
		}

		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x060068F7 RID: 26871 RVA: 0x002BAF9C File Offset: 0x002B9F9C
		// (set) Token: 0x060068F8 RID: 26872 RVA: 0x002BAFAE File Offset: 0x002B9FAE
		public double Zoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.ZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.ZoomProperty, value);
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x060068F9 RID: 26873 RVA: 0x002BAFC1 File Offset: 0x002B9FC1
		// (set) Token: 0x060068FA RID: 26874 RVA: 0x002BAFD3 File Offset: 0x002B9FD3
		public double MaxZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.MaxZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.MaxZoomProperty, value);
			}
		}

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x060068FB RID: 26875 RVA: 0x002BAFE6 File Offset: 0x002B9FE6
		// (set) Token: 0x060068FC RID: 26876 RVA: 0x002BAFF8 File Offset: 0x002B9FF8
		public double MinZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.MinZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.MinZoomProperty, value);
			}
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x060068FD RID: 26877 RVA: 0x002BB00B File Offset: 0x002BA00B
		// (set) Token: 0x060068FE RID: 26878 RVA: 0x002BB01D File Offset: 0x002BA01D
		public double ZoomIncrement
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.ZoomIncrementProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.ZoomIncrementProperty, value);
			}
		}

		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x060068FF RID: 26879 RVA: 0x002BB030 File Offset: 0x002BA030
		public bool CanIncreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.CanIncreaseZoomProperty);
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x06006900 RID: 26880 RVA: 0x002BB042 File Offset: 0x002BA042
		public bool CanDecreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.CanDecreaseZoomProperty);
			}
		}

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x06006901 RID: 26881 RVA: 0x002BB054 File Offset: 0x002BA054
		// (set) Token: 0x06006902 RID: 26882 RVA: 0x002BB066 File Offset: 0x002BA066
		public bool IsSelectionEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.IsSelectionEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.IsSelectionEnabledProperty, value);
			}
		}

		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x06006903 RID: 26883 RVA: 0x002BB074 File Offset: 0x002BA074
		// (set) Token: 0x06006904 RID: 26884 RVA: 0x002BB086 File Offset: 0x002BA086
		public bool IsToolBarVisible
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.IsToolBarVisibleProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.IsToolBarVisibleProperty, value);
			}
		}

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x06006905 RID: 26885 RVA: 0x002BB094 File Offset: 0x002BA094
		// (set) Token: 0x06006906 RID: 26886 RVA: 0x002BB0A6 File Offset: 0x002BA0A6
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty, value);
			}
		}

		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x06006907 RID: 26887 RVA: 0x002BB0B9 File Offset: 0x002BA0B9
		// (set) Token: 0x06006908 RID: 26888 RVA: 0x002BB0CB File Offset: 0x002BA0CB
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty, value);
			}
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x06006909 RID: 26889 RVA: 0x002BB0DE File Offset: 0x002BA0DE
		// (set) Token: 0x0600690A RID: 26890 RVA: 0x002BB0F0 File Offset: 0x002BA0F0
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(FlowDocumentScrollViewer.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.SelectionBrushProperty, value);
			}
		}

		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x0600690B RID: 26891 RVA: 0x002BB0FE File Offset: 0x002BA0FE
		// (set) Token: 0x0600690C RID: 26892 RVA: 0x002BB110 File Offset: 0x002BA110
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.SelectionOpacityProperty, value);
			}
		}

		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x0600690D RID: 26893 RVA: 0x002BB123 File Offset: 0x002BA123
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.IsSelectionActiveProperty);
			}
		}

		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x0600690E RID: 26894 RVA: 0x002BB135 File Offset: 0x002BA135
		// (set) Token: 0x0600690F RID: 26895 RVA: 0x002BB147 File Offset: 0x002BA147
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x002BB155 File Offset: 0x002BA155
		protected virtual void OnPrintCompleted()
		{
			this.ClearPrintingState();
		}

		// Token: 0x06006911 RID: 26897 RVA: 0x002BB15D File Offset: 0x002BA15D
		protected virtual void OnFindCommand()
		{
			if (this.CanShowFindToolBar)
			{
				this.ToggleFindToolBar(this.FindToolBar == null);
			}
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x002BB178 File Offset: 0x002BA178
		protected virtual void OnPrintCommand()
		{
			PrintDocumentImageableArea printDocumentImageableArea = null;
			if (this._printingState != null)
			{
				return;
			}
			if (this.Document == null)
			{
				this.OnPrintCompleted();
				return;
			}
			XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref printDocumentImageableArea);
			if (xpsDocumentWriter != null && printDocumentImageableArea != null)
			{
				if (this.RenderScope != null)
				{
					this.RenderScope.SuspendLayout();
				}
				FlowDocumentPaginator flowDocumentPaginator = ((IDocumentPaginatorSource)this.Document).DocumentPaginator as FlowDocumentPaginator;
				this._printingState = new FlowDocumentPrintingState();
				this._printingState.XpsDocumentWriter = xpsDocumentWriter;
				this._printingState.PageSize = flowDocumentPaginator.PageSize;
				this._printingState.PagePadding = this.Document.PagePadding;
				this._printingState.IsSelectionEnabled = this.IsSelectionEnabled;
				this._printingState.ColumnWidth = this.Document.ColumnWidth;
				CommandManager.InvalidateRequerySuggested();
				xpsDocumentWriter.WritingCompleted += this.HandlePrintCompleted;
				xpsDocumentWriter.WritingCancelled += this.HandlePrintCancelled;
				if (this._contentHost != null)
				{
					CommandManager.AddPreviewCanExecuteHandler(this._contentHost, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				if (this.IsSelectionEnabled)
				{
					base.SetCurrentValueInternal(FlowDocumentScrollViewer.IsSelectionEnabledProperty, BooleanBoxes.FalseBox);
				}
				flowDocumentPaginator.PageSize = new Size(printDocumentImageableArea.MediaSizeWidth, printDocumentImageableArea.MediaSizeHeight);
				Thickness thickness = this.Document.ComputePageMargin();
				this.Document.PagePadding = new Thickness(Math.Max(printDocumentImageableArea.OriginWidth, thickness.Left), Math.Max(printDocumentImageableArea.OriginHeight, thickness.Top), Math.Max(printDocumentImageableArea.MediaSizeWidth - (printDocumentImageableArea.OriginWidth + printDocumentImageableArea.ExtentWidth), thickness.Right), Math.Max(printDocumentImageableArea.MediaSizeHeight - (printDocumentImageableArea.OriginHeight + printDocumentImageableArea.ExtentHeight), thickness.Bottom));
				this.Document.ColumnWidth = double.PositiveInfinity;
				xpsDocumentWriter.WriteAsync(flowDocumentPaginator);
				return;
			}
			this.OnPrintCompleted();
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x002BB358 File Offset: 0x002BA358
		protected virtual void OnCancelPrintCommand()
		{
			if (this._printingState != null)
			{
				this._printingState.XpsDocumentWriter.CancelAsync();
			}
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x002BB372 File Offset: 0x002BA372
		protected virtual void OnIncreaseZoomCommand()
		{
			if (this.CanIncreaseZoom)
			{
				this.Zoom = Math.Min(this.Zoom + this.ZoomIncrement, this.MaxZoom);
			}
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x002BB39A File Offset: 0x002BA39A
		protected virtual void OnDecreaseZoomCommand()
		{
			if (this.CanDecreaseZoom)
			{
				this.Zoom = Math.Max(this.Zoom - this.ZoomIncrement, this.MinZoom);
			}
		}

		// Token: 0x06006916 RID: 26902 RVA: 0x002BB3C4 File Offset: 0x002BA3C4
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			Key key = e.Key;
			if (key != Key.Escape)
			{
				if (key == Key.F3)
				{
					if (this.CanShowFindToolBar)
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
				}
			}
			else if (this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
				e.Handled = true;
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x06006917 RID: 26903 RVA: 0x002BB45C File Offset: 0x002BA45C
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			if (this._contentHost != null)
			{
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					if (e.Delta > 0 && this.CanIncreaseZoom)
					{
						base.SetCurrentValueInternal(FlowDocumentScrollViewer.ZoomProperty, Math.Min(this.Zoom + this.ZoomIncrement, this.MaxZoom));
					}
					else if (e.Delta < 0 && this.CanDecreaseZoom)
					{
						base.SetCurrentValueInternal(FlowDocumentScrollViewer.ZoomProperty, Math.Max(this.Zoom - this.ZoomIncrement, this.MinZoom));
					}
				}
				else if (e.Delta < 0)
				{
					this._contentHost.LineDown();
				}
				else
				{
					this._contentHost.LineUp();
				}
				e.Handled = true;
			}
			if (!e.Handled)
			{
				base.OnMouseWheel(e);
			}
		}

		// Token: 0x06006918 RID: 26904 RVA: 0x002BB534 File Offset: 0x002BA534
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			DocumentViewerHelper.OnContextMenuOpening(this.Document, this, e);
		}

		// Token: 0x06006919 RID: 26905 RVA: 0x002BB54A File Offset: 0x002BA54A
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FlowDocumentScrollViewerAutomationPeer(this);
		}

		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x0600691A RID: 26906 RVA: 0x002BB552 File Offset: 0x002BA552
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (base.HasLogicalChildren && this.Document != null)
				{
					return new SingleChildEnumerator(this.Document);
				}
				return EmptyEnumerator.Instance;
			}
		}

		// Token: 0x0600691B RID: 26907 RVA: 0x002BB578 File Offset: 0x002BA578
		internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			DependencyObject document = this.Document;
			if (document != null && LogicalTreeHelper.GetParent(document) != this)
			{
				DependencyObject dependencyObject = route.PeekBranchNode() as DependencyObject;
				if (dependencyObject != null && DocumentViewerHelper.IsLogicalDescendent(dependencyObject, document))
				{
					FrameworkElement.AddIntermediateElementsToRoute(LogicalTreeHelper.GetParent(document), route, args, LogicalTreeHelper.GetParent(dependencyObject));
				}
			}
			return base.BuildRouteCore(route, args);
		}

		// Token: 0x0600691C RID: 26908 RVA: 0x002BB5CC File Offset: 0x002BA5CC
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool flag = true;
			DependencyObject document = this.Document;
			if (document != null && LogicalTreeHelper.GetParent(document) != this)
			{
				DependencyObject dependencyObject = (branchNodeStack.Count > 0) ? branchNodeStack.Peek() : null;
				if (dependencyObject != null && DocumentViewerHelper.IsLogicalDescendent(dependencyObject, document))
				{
					flag = FrameworkElement.InvalidateAutomationIntermediateElements(LogicalTreeHelper.GetParent(document), LogicalTreeHelper.GetParent(dependencyObject));
				}
			}
			return flag & base.InvalidateAutomationAncestorsCore(branchNodeStack, out continuePastCoreTree);
		}

		// Token: 0x0600691D RID: 26909 RVA: 0x002BB62C File Offset: 0x002BA62C
		internal object BringContentPositionIntoView(object arg)
		{
			ITextPointer textPointer = arg as ITextPointer;
			if (textPointer != null)
			{
				ITextView textView = this.GetTextView();
				if (textView != null && textView.IsValid && textView.RenderScope is IScrollInfo && textPointer.TextContainer == textView.TextContainer)
				{
					if (textView.Contains(textPointer))
					{
						Rect rectangleFromTextPosition = textView.GetRectangleFromTextPosition(textPointer);
						if (rectangleFromTextPosition != Rect.Empty)
						{
							IScrollInfo scrollInfo = (IScrollInfo)textView.RenderScope;
							scrollInfo.SetVerticalOffset(rectangleFromTextPosition.Top + scrollInfo.VerticalOffset);
						}
					}
					else
					{
						base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.BringContentPositionIntoView), textPointer);
					}
				}
			}
			return null;
		}

		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x0600691E RID: 26910 RVA: 0x002BB6CE File Offset: 0x002BA6CE
		internal ScrollViewer ScrollViewer
		{
			get
			{
				return this._contentHost;
			}
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x0600691F RID: 26911 RVA: 0x002BB6D6 File Offset: 0x002BA6D6
		internal bool CanShowFindToolBar
		{
			get
			{
				return this._findToolBarHost != null && this.Document != null && this._textEditor != null;
			}
		}

		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x06006920 RID: 26912 RVA: 0x002BB6F3 File Offset: 0x002BA6F3
		internal bool IsPrinting
		{
			get
			{
				return this._printingState != null;
			}
		}

		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x06006921 RID: 26913 RVA: 0x002BB700 File Offset: 0x002BA700
		internal TextPointer ContentPosition
		{
			get
			{
				TextPointer result = null;
				ITextView textView = this.GetTextView();
				if (textView != null && textView.IsValid && textView.RenderScope is IScrollInfo)
				{
					result = (textView.GetTextPositionFromPoint(default(Point), true) as TextPointer);
				}
				return result;
			}
		}

		// Token: 0x06006922 RID: 26914 RVA: 0x002BB748 File Offset: 0x002BA748
		private void ToggleFindToolBar(bool enable)
		{
			Invariant.Assert(enable == (this.FindToolBar == null));
			DocumentViewerHelper.ToggleFindToolBar(this._findToolBarHost, new EventHandler(this.OnFindInvoked), enable);
			if (!this.IsToolBarVisible && this._toolBarHost != null)
			{
				this._toolBarHost.Visibility = (enable ? Visibility.Visible : Visibility.Collapsed);
			}
		}

		// Token: 0x06006923 RID: 26915 RVA: 0x002BB7A0 File Offset: 0x002BA7A0
		private void ApplyZoom()
		{
			if (this.RenderScope != null)
			{
				this.RenderScope.LayoutTransform = new ScaleTransform(this.Zoom / 100.0, this.Zoom / 100.0);
			}
		}

		// Token: 0x06006924 RID: 26916 RVA: 0x002BB7DC File Offset: 0x002BA7DC
		private void AttachTextEditor()
		{
			AnnotationService service = AnnotationService.GetService(this);
			bool flag = false;
			if (service != null && service.IsEnabled)
			{
				flag = true;
				service.Disable();
			}
			if (this._textEditor != null)
			{
				this._textEditor.TextContainer.TextView = null;
				this._textEditor.OnDetach();
				this._textEditor = null;
			}
			ITextView textView = null;
			if (this.Document != null)
			{
				textView = this.GetTextView();
				this.Document.StructuralCache.TextContainer.TextView = textView;
			}
			if (this.IsSelectionEnabled && this.Document != null && this.RenderScope != null && this.Document.StructuralCache.TextContainer.TextSelection == null)
			{
				this._textEditor = new TextEditor(this.Document.StructuralCache.TextContainer, this, false);
				this._textEditor.IsReadOnly = !FlowDocumentScrollViewer.IsEditingEnabled;
				this._textEditor.TextView = textView;
			}
			if (service != null && flag)
			{
				service.Enable(service.Store);
			}
			if (this._textEditor == null && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x002BB8ED File Offset: 0x002BA8ED
		private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.OriginalSource == this.ScrollViewer && !DoubleUtil.IsZero(e.ViewportHeightChange))
			{
				base.SetValue(TextEditor.PageHeightProperty, e.ViewportHeight);
			}
		}

		// Token: 0x06006926 RID: 26918 RVA: 0x002BB920 File Offset: 0x002BA920
		private void HandlePrintCompleted(object sender, WritingCompletedEventArgs e)
		{
			this.OnPrintCompleted();
		}

		// Token: 0x06006927 RID: 26919 RVA: 0x002BB155 File Offset: 0x002BA155
		private void HandlePrintCancelled(object sender, WritingCancelledEventArgs e)
		{
			this.ClearPrintingState();
		}

		// Token: 0x06006928 RID: 26920 RVA: 0x002BB928 File Offset: 0x002BA928
		private void ClearPrintingState()
		{
			if (this._printingState != null)
			{
				if (this.RenderScope != null)
				{
					this.RenderScope.ResumeLayout();
				}
				if (this._printingState.IsSelectionEnabled)
				{
					base.SetCurrentValueInternal(FlowDocumentScrollViewer.IsSelectionEnabledProperty, BooleanBoxes.TrueBox);
				}
				if (this._contentHost != null)
				{
					CommandManager.RemovePreviewCanExecuteHandler(this._contentHost, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				this._printingState.XpsDocumentWriter.WritingCompleted -= this.HandlePrintCompleted;
				this._printingState.XpsDocumentWriter.WritingCancelled -= this.HandlePrintCancelled;
				this.Document.PagePadding = this._printingState.PagePadding;
				this.Document.ColumnWidth = this._printingState.ColumnWidth;
				((IDocumentPaginatorSource)this.Document).DocumentPaginator.PageSize = this._printingState.PageSize;
				this._printingState = null;
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x06006929 RID: 26921 RVA: 0x002BBA1C File Offset: 0x002BAA1C
		private void HandleRequestBringIntoView(RequestBringIntoViewEventArgs args)
		{
			Rect rect = Rect.Empty;
			if (args != null && args.TargetObject != null && this.Document != null)
			{
				DependencyObject document = this.Document;
				if (args.TargetObject == document)
				{
					if (this._contentHost != null)
					{
						this._contentHost.ScrollToHome();
					}
					args.Handled = true;
				}
				else if (args.TargetObject is UIElement)
				{
					UIElement uielement = (UIElement)args.TargetObject;
					if (this.RenderScope != null && this.RenderScope.IsAncestorOf(uielement))
					{
						rect = args.TargetRect;
						if (rect.IsEmpty)
						{
							rect = new Rect(uielement.RenderSize);
						}
						rect = FlowDocumentScrollViewer.MakeVisible(this.RenderScope, uielement, rect);
						if (!rect.IsEmpty)
						{
							rect = this.RenderScope.TransformToAncestor(this).TransformBounds(rect);
						}
						args.Handled = true;
					}
				}
				else if (args.TargetObject is ContentElement)
				{
					DependencyObject dependencyObject = args.TargetObject;
					while (dependencyObject != null && dependencyObject != document)
					{
						dependencyObject = LogicalTreeHelper.GetParent(dependencyObject);
					}
					if (dependencyObject != null)
					{
						IContentHost icontentHost = this.GetIContentHost();
						if (icontentHost != null)
						{
							ReadOnlyCollection<Rect> rectangles = icontentHost.GetRectangles((ContentElement)args.TargetObject);
							if (rectangles.Count > 0)
							{
								rect = FlowDocumentScrollViewer.MakeVisible(this.RenderScope, (Visual)icontentHost, rectangles[0]);
								if (!rect.IsEmpty)
								{
									rect = this.RenderScope.TransformToAncestor(this).TransformBounds(rect);
								}
							}
						}
						args.Handled = true;
					}
				}
				if (args.Handled)
				{
					if (rect.IsEmpty)
					{
						base.BringIntoView();
						return;
					}
					base.BringIntoView(rect);
				}
			}
		}

		// Token: 0x0600692A RID: 26922 RVA: 0x002BBBBC File Offset: 0x002BABBC
		private void DocumentChanged(FlowDocument oldDocument, FlowDocument newDocument)
		{
			if (newDocument != null && newDocument.StructuralCache.TextContainer != null && newDocument.StructuralCache.TextContainer.TextSelection != null)
			{
				throw new ArgumentException(SR.Get("FlowDocumentScrollViewerDocumentBelongsToAnotherFlowDocumentScrollViewerAlready"));
			}
			if (oldDocument != null)
			{
				if (this._documentAsLogicalChild)
				{
					base.RemoveLogicalChild(oldDocument);
				}
				if (this.RenderScope != null)
				{
					this.RenderScope.Document = null;
				}
				oldDocument.ClearValue(PathNode.HiddenParentProperty);
				oldDocument.StructuralCache.ClearUpdateInfo(true);
			}
			if (newDocument != null && LogicalTreeHelper.GetParent(newDocument) != null)
			{
				ContentOperations.SetParent(newDocument, this);
				this._documentAsLogicalChild = false;
			}
			else
			{
				this._documentAsLogicalChild = true;
			}
			if (newDocument != null)
			{
				if (this.RenderScope != null)
				{
					this.RenderScope.Document = newDocument;
				}
				if (this._documentAsLogicalChild)
				{
					base.AddLogicalChild(newDocument);
				}
				newDocument.SetValue(PathNode.HiddenParentProperty, this);
				newDocument.StructuralCache.ClearUpdateInfo(true);
			}
			this.AttachTextEditor();
			if (!this.CanShowFindToolBar && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			FlowDocumentScrollViewerAutomationPeer flowDocumentScrollViewerAutomationPeer = UIElementAutomationPeer.FromElement(this) as FlowDocumentScrollViewerAutomationPeer;
			if (flowDocumentScrollViewerAutomationPeer != null)
			{
				flowDocumentScrollViewerAutomationPeer.InvalidatePeer();
			}
		}

		// Token: 0x0600692B RID: 26923 RVA: 0x002BBCCC File Offset: 0x002BACCC
		private ITextView GetTextView()
		{
			ITextView result = null;
			if (this.RenderScope != null)
			{
				result = (ITextView)((IServiceProvider)this.RenderScope).GetService(typeof(ITextView));
			}
			return result;
		}

		// Token: 0x0600692C RID: 26924 RVA: 0x002BBD00 File Offset: 0x002BAD00
		private IContentHost GetIContentHost()
		{
			IContentHost result = null;
			if (this.RenderScope != null && VisualTreeHelper.GetChildrenCount(this.RenderScope) > 0)
			{
				result = (VisualTreeHelper.GetChild(this.RenderScope, 0) as IContentHost);
			}
			return result;
		}

		// Token: 0x0600692D RID: 26925 RVA: 0x002B9AE0 File Offset: 0x002B8AE0
		private void CreateTwoWayBinding(FrameworkElement fe, DependencyProperty dp, string propertyPath)
		{
			fe.SetBinding(dp, new Binding(propertyPath)
			{
				Mode = BindingMode.TwoWay,
				Source = this
			});
		}

		// Token: 0x0600692E RID: 26926 RVA: 0x002BBD38 File Offset: 0x002BAD38
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(FlowDocumentScrollViewer.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(FlowDocumentScrollViewer.CanExecuteRoutedEventHandler);
			FlowDocumentScrollViewer._commandLineDown = new RoutedUICommand(string.Empty, "FDSV_LineDown", typeof(FlowDocumentScrollViewer));
			FlowDocumentScrollViewer._commandLineUp = new RoutedUICommand(string.Empty, "FDSV_LineUp", typeof(FlowDocumentScrollViewer));
			FlowDocumentScrollViewer._commandLineLeft = new RoutedUICommand(string.Empty, "FDSV_LineLeft", typeof(FlowDocumentScrollViewer));
			FlowDocumentScrollViewer._commandLineRight = new RoutedUICommand(string.Empty, "FDSV_LineRight", typeof(FlowDocumentScrollViewer));
			TextEditor.RegisterCommandHandlers(typeof(FlowDocumentScrollViewer), true, !FlowDocumentScrollViewer.IsEditingEnabled, true);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), ApplicationCommands.Find, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), ApplicationCommands.Print, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), ApplicationCommands.CancelPrint, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.PreviousPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Prior);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.NextPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Next);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.FirstPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Home), new KeyGesture(Key.Home, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.LastPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.End), new KeyGesture(Key.End, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.IncreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemPlus, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.DecreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemMinus, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), FlowDocumentScrollViewer._commandLineDown, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Down);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), FlowDocumentScrollViewer._commandLineUp, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Up);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), FlowDocumentScrollViewer._commandLineLeft, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Left);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), FlowDocumentScrollViewer._commandLineRight, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Right);
		}

		// Token: 0x0600692F RID: 26927 RVA: 0x002BBF50 File Offset: 0x002BAF50
		private static void CanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = target as FlowDocumentScrollViewer;
			Invariant.Assert(flowDocumentScrollViewer != null, "Target of QueryEnabledEvent must be FlowDocumentScrollViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (flowDocumentScrollViewer._printingState != null)
			{
				args.CanExecute = (args.Command == ApplicationCommands.CancelPrint);
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				args.CanExecute = flowDocumentScrollViewer.CanShowFindToolBar;
				return;
			}
			if (args.Command == ApplicationCommands.Print)
			{
				args.CanExecute = (flowDocumentScrollViewer.Document != null);
				return;
			}
			if (args.Command == ApplicationCommands.CancelPrint)
			{
				args.CanExecute = false;
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x002BBFF0 File Offset: 0x002BAFF0
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = target as FlowDocumentScrollViewer;
			Invariant.Assert(flowDocumentScrollViewer != null, "Target of ExecuteEvent must be FlowDocumentScrollViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == ApplicationCommands.Find)
			{
				flowDocumentScrollViewer.OnFindCommand();
				return;
			}
			if (args.Command == ApplicationCommands.Print)
			{
				flowDocumentScrollViewer.OnPrintCommand();
				return;
			}
			if (args.Command == ApplicationCommands.CancelPrint)
			{
				flowDocumentScrollViewer.OnCancelPrintCommand();
				return;
			}
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				flowDocumentScrollViewer.OnIncreaseZoomCommand();
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				flowDocumentScrollViewer.OnDecreaseZoomCommand();
				return;
			}
			if (args.Command == FlowDocumentScrollViewer._commandLineDown)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.LineDown();
					return;
				}
			}
			else if (args.Command == FlowDocumentScrollViewer._commandLineUp)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.LineUp();
					return;
				}
			}
			else if (args.Command == FlowDocumentScrollViewer._commandLineLeft)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.LineLeft();
					return;
				}
			}
			else if (args.Command == FlowDocumentScrollViewer._commandLineRight)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.LineRight();
					return;
				}
			}
			else if (args.Command == NavigationCommands.NextPage)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.PageDown();
					return;
				}
			}
			else if (args.Command == NavigationCommands.PreviousPage)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.PageUp();
					return;
				}
			}
			else if (args.Command == NavigationCommands.FirstPage)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.ScrollToHome();
					return;
				}
			}
			else if (args.Command == NavigationCommands.LastPage)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.ScrollToEnd();
					return;
				}
			}
			else
			{
				Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
			}
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x002BC1A4 File Offset: 0x002BB1A4
		private void OnFindInvoked(object sender, EventArgs e)
		{
			FindToolBar findToolBar = this.FindToolBar;
			if (findToolBar != null && this._textEditor != null)
			{
				base.Focus();
				ITextRange textRange = DocumentViewerHelper.Find(findToolBar, this._textEditor, this._textEditor.TextView, this._textEditor.TextView);
				if (textRange == null || textRange.IsEmpty)
				{
					DocumentViewerHelper.ShowFindUnsuccessfulMessage(findToolBar);
				}
			}
		}

		// Token: 0x06006932 RID: 26930 RVA: 0x002BC1FE File Offset: 0x002BB1FE
		private void PreviewCanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			Invariant.Assert(target is ScrollViewer, "Target of PreviewCanExecuteRoutedEventHandler must be ScrollViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (this._printingState != null)
			{
				args.CanExecute = false;
				args.Handled = true;
			}
		}

		// Token: 0x06006933 RID: 26931 RVA: 0x002BC237 File Offset: 0x002BB237
		private static void KeyDownHandler(object sender, KeyEventArgs e)
		{
			DocumentViewerHelper.KeyDownHelper(e, ((FlowDocumentScrollViewer)sender)._findToolBarHost);
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x002BC24C File Offset: 0x002BB24C
		private static Rect MakeVisible(IScrollInfo scrollInfo, Visual visual, Rect rectangle)
		{
			Rect result;
			if (scrollInfo.GetType() == typeof(ScrollContentPresenter))
			{
				result = ((ScrollContentPresenter)scrollInfo).MakeVisible(visual, rectangle, false);
			}
			else
			{
				result = scrollInfo.MakeVisible(visual, rectangle);
			}
			return result;
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x002BC28B File Offset: 0x002BB28B
		private static void DocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			((FlowDocumentScrollViewer)d).DocumentChanged((FlowDocument)e.OldValue, (FlowDocument)e.NewValue);
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x002BC2CC File Offset: 0x002BB2CC
		private static void ZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			if (!DoubleUtil.AreClose((double)e.OldValue, (double)e.NewValue))
			{
				flowDocumentScrollViewer.SetValue(FlowDocumentScrollViewer.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(flowDocumentScrollViewer.MaxZoom, flowDocumentScrollViewer.Zoom)));
				flowDocumentScrollViewer.SetValue(FlowDocumentScrollViewer.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(flowDocumentScrollViewer.MinZoom, flowDocumentScrollViewer.Zoom)));
				flowDocumentScrollViewer.ApplyZoom();
			}
		}

		// Token: 0x06006937 RID: 26935 RVA: 0x002BC35C File Offset: 0x002BB35C
		private static object CoerceZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			double value2 = (double)value;
			double maxZoom = flowDocumentScrollViewer.MaxZoom;
			if (DoubleUtil.LessThan(maxZoom, value2))
			{
				return maxZoom;
			}
			double minZoom = flowDocumentScrollViewer.MinZoom;
			if (DoubleUtil.GreaterThan(minZoom, value2))
			{
				return minZoom;
			}
			return value;
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x002BC3BC File Offset: 0x002BB3BC
		private static void MaxZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			flowDocumentScrollViewer.CoerceValue(FlowDocumentScrollViewer.ZoomProperty);
			flowDocumentScrollViewer.SetValue(FlowDocumentScrollViewer.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(flowDocumentScrollViewer.MaxZoom, flowDocumentScrollViewer.Zoom)));
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x002BC410 File Offset: 0x002BB410
		private static object CoerceMaxZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			double minZoom = ((FlowDocumentScrollViewer)d).MinZoom;
			if ((double)value >= minZoom)
			{
				return value;
			}
			return minZoom;
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x002BC450 File Offset: 0x002BB450
		private static void MinZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			flowDocumentScrollViewer.CoerceValue(FlowDocumentScrollViewer.MaxZoomProperty);
			flowDocumentScrollViewer.CoerceValue(FlowDocumentScrollViewer.ZoomProperty);
			flowDocumentScrollViewer.SetValue(FlowDocumentScrollViewer.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(flowDocumentScrollViewer.MinZoom, flowDocumentScrollViewer.Zoom)));
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x002BA734 File Offset: 0x002B9734
		private static bool ZoomValidateValue(object o)
		{
			double num = (double)o;
			return !double.IsNaN(num) && !double.IsInfinity(num) && DoubleUtil.GreaterThan(num, 0.0);
		}

		// Token: 0x0600693C RID: 26940 RVA: 0x002BC4AF File Offset: 0x002BB4AF
		private static void HandleRequestBringIntoView(object sender, RequestBringIntoViewEventArgs args)
		{
			if (sender != null && sender is FlowDocumentScrollViewer)
			{
				((FlowDocumentScrollViewer)sender).HandleRequestBringIntoView(args);
			}
		}

		// Token: 0x0600693D RID: 26941 RVA: 0x002BC4C8 File Offset: 0x002BB4C8
		private static void IsSelectionEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			((FlowDocumentScrollViewer)d).AttachTextEditor();
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x002BC4EC File Offset: 0x002BB4EC
		private static void IsToolBarVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			if (flowDocumentScrollViewer._toolBarHost != null)
			{
				flowDocumentScrollViewer._toolBarHost.Visibility = (((bool)e.NewValue) ? Visibility.Visible : Visibility.Collapsed);
			}
		}

		// Token: 0x0600693F RID: 26943 RVA: 0x002BC53C File Offset: 0x002BB53C
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			if (flowDocumentScrollViewer.Selection != null)
			{
				CaretElement caretElement = flowDocumentScrollViewer.Selection.CaretElement;
				if (caretElement != null)
				{
					caretElement.InvalidateVisual();
				}
			}
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x06006940 RID: 26944 RVA: 0x002BC56D File Offset: 0x002BB56D
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

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x06006941 RID: 26945 RVA: 0x002BC589 File Offset: 0x002BB589
		private FlowDocumentView RenderScope
		{
			get
			{
				if (this._contentHost == null)
				{
					return null;
				}
				return this._contentHost.Content as FlowDocumentView;
			}
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x002BC5A8 File Offset: 0x002BB5A8
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.Document != null)
			{
				throw new ArgumentException(SR.Get("FlowDocumentScrollViewerCanHaveOnlyOneChild"));
			}
			if (!(value is FlowDocument))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(FlowDocument)
				}), "value");
			}
			this.Document = (value as FlowDocument);
		}

		// Token: 0x06006943 RID: 26947 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06006944 RID: 26948 RVA: 0x002BC620 File Offset: 0x002BB620
		object IServiceProvider.GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				result = this.GetTextView();
			}
			else if ((serviceType == typeof(TextContainer) || serviceType == typeof(ITextContainer)) && this.Document != null)
			{
				result = ((IServiceProvider)this.Document).GetService(serviceType);
			}
			return result;
		}

		// Token: 0x06006945 RID: 26949 RVA: 0x002BC698 File Offset: 0x002BB698
		CustomJournalStateInternal IJournalState.GetJournalState(JournalReason journalReason)
		{
			int contentPosition = -1;
			LogicalDirection contentPositionDirection = LogicalDirection.Forward;
			TextPointer contentPosition2 = this.ContentPosition;
			if (contentPosition2 != null)
			{
				contentPosition = contentPosition2.Offset;
				contentPositionDirection = contentPosition2.LogicalDirection;
			}
			return new FlowDocumentScrollViewer.JournalState(contentPosition, contentPositionDirection, this.Zoom);
		}

		// Token: 0x06006946 RID: 26950 RVA: 0x002BC6D0 File Offset: 0x002BB6D0
		void IJournalState.RestoreJournalState(CustomJournalStateInternal state)
		{
			FlowDocumentScrollViewer.JournalState journalState = state as FlowDocumentScrollViewer.JournalState;
			if (state != null)
			{
				this.Zoom = journalState.Zoom;
				if (journalState.ContentPosition != -1)
				{
					FlowDocument document = this.Document;
					if (document != null)
					{
						TextContainer textContainer = document.StructuralCache.TextContainer;
						if (journalState.ContentPosition <= textContainer.SymbolCount)
						{
							TextPointer arg = textContainer.CreatePointerAtOffset(journalState.ContentPosition, journalState.ContentPositionDirection);
							base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.BringContentPositionIntoView), arg);
						}
					}
				}
			}
		}

		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x06006947 RID: 26951 RVA: 0x002BC74D File Offset: 0x002BB74D
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return FlowDocumentScrollViewer._dType;
			}
		}

		// Token: 0x040034E3 RID: 13539
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(FlowDocumentScrollViewer.DocumentChanged)));

		// Token: 0x040034E4 RID: 13540
		public static readonly DependencyProperty ZoomProperty = FlowDocumentPageViewer.ZoomProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(100.0, new PropertyChangedCallback(FlowDocumentScrollViewer.ZoomChanged), new CoerceValueCallback(FlowDocumentScrollViewer.CoerceZoom)));

		// Token: 0x040034E5 RID: 13541
		public static readonly DependencyProperty MaxZoomProperty = FlowDocumentPageViewer.MaxZoomProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(200.0, new PropertyChangedCallback(FlowDocumentScrollViewer.MaxZoomChanged), new CoerceValueCallback(FlowDocumentScrollViewer.CoerceMaxZoom)));

		// Token: 0x040034E6 RID: 13542
		public static readonly DependencyProperty MinZoomProperty = FlowDocumentPageViewer.MinZoomProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(80.0, new PropertyChangedCallback(FlowDocumentScrollViewer.MinZoomChanged)));

		// Token: 0x040034E7 RID: 13543
		public static readonly DependencyProperty ZoomIncrementProperty = FlowDocumentPageViewer.ZoomIncrementProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		// Token: 0x040034E8 RID: 13544
		private static readonly DependencyPropertyKey CanIncreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanIncreaseZoom", typeof(bool), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		// Token: 0x040034E9 RID: 13545
		public static readonly DependencyProperty CanIncreaseZoomProperty = FlowDocumentScrollViewer.CanIncreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x040034EA RID: 13546
		private static readonly DependencyPropertyKey CanDecreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanDecreaseZoom", typeof(bool), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		// Token: 0x040034EB RID: 13547
		public static readonly DependencyProperty CanDecreaseZoomProperty = FlowDocumentScrollViewer.CanDecreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x040034EC RID: 13548
		public static readonly DependencyProperty IsSelectionEnabledProperty = DependencyProperty.Register("IsSelectionEnabled", typeof(bool), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentScrollViewer.IsSelectionEnabledChanged)));

		// Token: 0x040034ED RID: 13549
		public static readonly DependencyProperty IsToolBarVisibleProperty = DependencyProperty.Register("IsToolBarVisible", typeof(bool), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(FlowDocumentScrollViewer.IsToolBarVisibleChanged)));

		// Token: 0x040034EE RID: 13550
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));

		// Token: 0x040034EF RID: 13551
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Visible));

		// Token: 0x040034F0 RID: 13552
		public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		// Token: 0x040034F1 RID: 13553
		public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		// Token: 0x040034F2 RID: 13554
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		// Token: 0x040034F3 RID: 13555
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = TextBoxBase.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		// Token: 0x040034F4 RID: 13556
		private TextEditor _textEditor;

		// Token: 0x040034F5 RID: 13557
		private Decorator _findToolBarHost;

		// Token: 0x040034F6 RID: 13558
		private Decorator _toolBarHost;

		// Token: 0x040034F7 RID: 13559
		private ScrollViewer _contentHost;

		// Token: 0x040034F8 RID: 13560
		private bool _documentAsLogicalChild;

		// Token: 0x040034F9 RID: 13561
		private FlowDocumentPrintingState _printingState;

		// Token: 0x040034FA RID: 13562
		private const string _contentHostTemplateName = "PART_ContentHost";

		// Token: 0x040034FB RID: 13563
		private const string _findToolBarHostTemplateName = "PART_FindToolBarHost";

		// Token: 0x040034FC RID: 13564
		private const string _toolBarHostTemplateName = "PART_ToolBarHost";

		// Token: 0x040034FD RID: 13565
		private static bool IsEditingEnabled = false;

		// Token: 0x040034FE RID: 13566
		private static RoutedUICommand _commandLineDown;

		// Token: 0x040034FF RID: 13567
		private static RoutedUICommand _commandLineUp;

		// Token: 0x04003500 RID: 13568
		private static RoutedUICommand _commandLineLeft;

		// Token: 0x04003501 RID: 13569
		private static RoutedUICommand _commandLineRight;

		// Token: 0x04003502 RID: 13570
		private static DependencyObjectType _dType;

		// Token: 0x02000BD5 RID: 3029
		[Serializable]
		private class JournalState : CustomJournalStateInternal
		{
			// Token: 0x06008F92 RID: 36754 RVA: 0x003449AD File Offset: 0x003439AD
			public JournalState(int contentPosition, LogicalDirection contentPositionDirection, double zoom)
			{
				this.ContentPosition = contentPosition;
				this.ContentPositionDirection = contentPositionDirection;
				this.Zoom = zoom;
			}

			// Token: 0x04004A14 RID: 18964
			public int ContentPosition;

			// Token: 0x04004A15 RID: 18965
			public LogicalDirection ContentPositionDirection;

			// Token: 0x04004A16 RID: 18966
			public double Zoom;
		}
	}
}
