using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Commands;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x0200077C RID: 1916
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(Decorator))]
	[TemplatePart(Name = "PART_FindToolBarHost", Type = typeof(Decorator))]
	[ContentProperty("Document")]
	public class FlowDocumentReader : Control, IAddChild, IJournalState
	{
		// Token: 0x0600687C RID: 26748 RVA: 0x002B8C94 File Offset: 0x002B7C94
		static FlowDocumentReader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIFlowDocumentReader")));
			FlowDocumentReader._dType = DependencyObjectType.FromSystemTypeInternal(typeof(FlowDocumentReader));
			TextBoxBase.SelectionBrushProperty.OverrideMetadata(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(new PropertyChangedCallback(FlowDocumentReader.UpdateCaretElement)));
			TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(0.4, new PropertyChangedCallback(FlowDocumentReader.UpdateCaretElement)));
			FlowDocumentReader.CreateCommandBindings();
			EventManager.RegisterClassHandler(typeof(FlowDocumentReader), Keyboard.KeyDownEvent, new KeyEventHandler(FlowDocumentReader.KeyDownHandler), true);
		}

		// Token: 0x0600687E RID: 26750 RVA: 0x002B91C0 File Offset: 0x002B81C0
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.CurrentViewer != null)
			{
				this.DetachViewer(this.CurrentViewer);
				this._contentHost.Child = null;
			}
			this._contentHost = (base.GetTemplateChild("PART_ContentHost") as Decorator);
			if (this._contentHost != null)
			{
				if (this._contentHost.Child != null)
				{
					throw new NotSupportedException(SR.Get("FlowDocumentReaderDecoratorMarkedAsContentHostMustHaveNoContent"));
				}
				this.SwitchViewingModeCore(this.ViewingMode);
			}
			if (this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			this._findToolBarHost = (base.GetTemplateChild("PART_FindToolBarHost") as Decorator);
			this._findButton = (base.GetTemplateChild("FindButton") as ToggleButton);
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x002B9278 File Offset: 0x002B8278
		public bool CanGoToPage(int pageNumber)
		{
			bool result = false;
			if (this.CurrentViewer != null)
			{
				result = this.CurrentViewer.CanGoToPage(pageNumber);
			}
			return result;
		}

		// Token: 0x06006880 RID: 26752 RVA: 0x002B929D File Offset: 0x002B829D
		public void Find()
		{
			this.OnFindCommand();
		}

		// Token: 0x06006881 RID: 26753 RVA: 0x002B92A5 File Offset: 0x002B82A5
		public void Print()
		{
			this.OnPrintCommand();
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x002B92AD File Offset: 0x002B82AD
		public void CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		// Token: 0x06006883 RID: 26755 RVA: 0x002B92B5 File Offset: 0x002B82B5
		public void IncreaseZoom()
		{
			this.OnIncreaseZoomCommand();
		}

		// Token: 0x06006884 RID: 26756 RVA: 0x002B92BD File Offset: 0x002B82BD
		public void DecreaseZoom()
		{
			this.OnDecreaseZoomCommand();
		}

		// Token: 0x06006885 RID: 26757 RVA: 0x002B92C5 File Offset: 0x002B82C5
		public void SwitchViewingMode(FlowDocumentReaderViewingMode viewingMode)
		{
			this.OnSwitchViewingModeCommand(viewingMode);
		}

		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x06006886 RID: 26758 RVA: 0x002B92CE File Offset: 0x002B82CE
		// (set) Token: 0x06006887 RID: 26759 RVA: 0x002B92E0 File Offset: 0x002B82E0
		public FlowDocumentReaderViewingMode ViewingMode
		{
			get
			{
				return (FlowDocumentReaderViewingMode)base.GetValue(FlowDocumentReader.ViewingModeProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.ViewingModeProperty, value);
			}
		}

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x06006888 RID: 26760 RVA: 0x002B92F4 File Offset: 0x002B82F4
		public TextSelection Selection
		{
			get
			{
				TextSelection result = null;
				if (this._contentHost != null)
				{
					IFlowDocumentViewer flowDocumentViewer = this._contentHost.Child as IFlowDocumentViewer;
					if (flowDocumentViewer != null)
					{
						result = (flowDocumentViewer.TextSelection as TextSelection);
					}
				}
				return result;
			}
		}

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x06006889 RID: 26761 RVA: 0x002B932C File Offset: 0x002B832C
		// (set) Token: 0x0600688A RID: 26762 RVA: 0x002B933E File Offset: 0x002B833E
		public bool IsPageViewEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsPageViewEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsPageViewEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x0600688B RID: 26763 RVA: 0x002B9351 File Offset: 0x002B8351
		// (set) Token: 0x0600688C RID: 26764 RVA: 0x002B9363 File Offset: 0x002B8363
		public bool IsTwoPageViewEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsTwoPageViewEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsTwoPageViewEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x0600688D RID: 26765 RVA: 0x002B9376 File Offset: 0x002B8376
		// (set) Token: 0x0600688E RID: 26766 RVA: 0x002B9388 File Offset: 0x002B8388
		public bool IsScrollViewEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsScrollViewEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsScrollViewEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x0600688F RID: 26767 RVA: 0x002B939B File Offset: 0x002B839B
		public int PageCount
		{
			get
			{
				return (int)base.GetValue(FlowDocumentReader.PageCountProperty);
			}
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x06006890 RID: 26768 RVA: 0x002B93AD File Offset: 0x002B83AD
		public int PageNumber
		{
			get
			{
				return (int)base.GetValue(FlowDocumentReader.PageNumberProperty);
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x06006891 RID: 26769 RVA: 0x002B93BF File Offset: 0x002B83BF
		public bool CanGoToPreviousPage
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.CanGoToPreviousPageProperty);
			}
		}

		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x06006892 RID: 26770 RVA: 0x002B93D1 File Offset: 0x002B83D1
		public bool CanGoToNextPage
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.CanGoToNextPageProperty);
			}
		}

		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x06006893 RID: 26771 RVA: 0x002B93E3 File Offset: 0x002B83E3
		// (set) Token: 0x06006894 RID: 26772 RVA: 0x002B93F5 File Offset: 0x002B83F5
		public bool IsFindEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsFindEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsFindEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x06006895 RID: 26773 RVA: 0x002B9408 File Offset: 0x002B8408
		// (set) Token: 0x06006896 RID: 26774 RVA: 0x002B941A File Offset: 0x002B841A
		public bool IsPrintEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsPrintEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsPrintEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x06006897 RID: 26775 RVA: 0x002B942D File Offset: 0x002B842D
		// (set) Token: 0x06006898 RID: 26776 RVA: 0x002B943F File Offset: 0x002B843F
		public FlowDocument Document
		{
			get
			{
				return (FlowDocument)base.GetValue(FlowDocumentReader.DocumentProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.DocumentProperty, value);
			}
		}

		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x06006899 RID: 26777 RVA: 0x002B944D File Offset: 0x002B844D
		// (set) Token: 0x0600689A RID: 26778 RVA: 0x002B945F File Offset: 0x002B845F
		public double Zoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.ZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.ZoomProperty, value);
			}
		}

		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x0600689B RID: 26779 RVA: 0x002B9472 File Offset: 0x002B8472
		// (set) Token: 0x0600689C RID: 26780 RVA: 0x002B9484 File Offset: 0x002B8484
		public double MaxZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.MaxZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.MaxZoomProperty, value);
			}
		}

		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x0600689D RID: 26781 RVA: 0x002B9497 File Offset: 0x002B8497
		// (set) Token: 0x0600689E RID: 26782 RVA: 0x002B94A9 File Offset: 0x002B84A9
		public double MinZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.MinZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.MinZoomProperty, value);
			}
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x0600689F RID: 26783 RVA: 0x002B94BC File Offset: 0x002B84BC
		// (set) Token: 0x060068A0 RID: 26784 RVA: 0x002B94CE File Offset: 0x002B84CE
		public double ZoomIncrement
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.ZoomIncrementProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.ZoomIncrementProperty, value);
			}
		}

		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x060068A1 RID: 26785 RVA: 0x002B94E1 File Offset: 0x002B84E1
		public bool CanIncreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.CanIncreaseZoomProperty);
			}
		}

		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x060068A2 RID: 26786 RVA: 0x002B94F3 File Offset: 0x002B84F3
		public bool CanDecreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.CanDecreaseZoomProperty);
			}
		}

		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x060068A3 RID: 26787 RVA: 0x002B9505 File Offset: 0x002B8505
		// (set) Token: 0x060068A4 RID: 26788 RVA: 0x002B9517 File Offset: 0x002B8517
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(FlowDocumentReader.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.SelectionBrushProperty, value);
			}
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x060068A5 RID: 26789 RVA: 0x002B9525 File Offset: 0x002B8525
		// (set) Token: 0x060068A6 RID: 26790 RVA: 0x002B9537 File Offset: 0x002B8537
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(FlowDocumentReader.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.SelectionOpacityProperty, value);
			}
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x060068A7 RID: 26791 RVA: 0x002B954A File Offset: 0x002B854A
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsSelectionActiveProperty);
			}
		}

		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x060068A8 RID: 26792 RVA: 0x002B955C File Offset: 0x002B855C
		// (set) Token: 0x060068A9 RID: 26793 RVA: 0x002B956E File Offset: 0x002B856E
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentReader.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentReader.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		// Token: 0x060068AA RID: 26794 RVA: 0x002B957C File Offset: 0x002B857C
		protected virtual void OnPrintCompleted()
		{
			if (this._printInProgress)
			{
				this._printInProgress = false;
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x060068AB RID: 26795 RVA: 0x002B9592 File Offset: 0x002B8592
		protected virtual void OnFindCommand()
		{
			if (this.CanShowFindToolBar)
			{
				this.ToggleFindToolBar(this.FindToolBar == null);
			}
		}

		// Token: 0x060068AC RID: 26796 RVA: 0x002B95AB File Offset: 0x002B85AB
		protected virtual void OnPrintCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.Print();
			}
		}

		// Token: 0x060068AD RID: 26797 RVA: 0x002B95C0 File Offset: 0x002B85C0
		protected virtual void OnCancelPrintCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.CancelPrint();
			}
		}

		// Token: 0x060068AE RID: 26798 RVA: 0x002B95D5 File Offset: 0x002B85D5
		protected virtual void OnIncreaseZoomCommand()
		{
			if (this.CanIncreaseZoom)
			{
				base.SetCurrentValueInternal(FlowDocumentReader.ZoomProperty, Math.Min(this.Zoom + this.ZoomIncrement, this.MaxZoom));
			}
		}

		// Token: 0x060068AF RID: 26799 RVA: 0x002B9607 File Offset: 0x002B8607
		protected virtual void OnDecreaseZoomCommand()
		{
			if (this.CanDecreaseZoom)
			{
				base.SetCurrentValueInternal(FlowDocumentReader.ZoomProperty, Math.Max(this.Zoom - this.ZoomIncrement, this.MinZoom));
			}
		}

		// Token: 0x060068B0 RID: 26800 RVA: 0x002B9639 File Offset: 0x002B8639
		protected virtual void OnSwitchViewingModeCommand(FlowDocumentReaderViewingMode viewingMode)
		{
			this.SwitchViewingModeCore(viewingMode);
		}

		// Token: 0x060068B1 RID: 26801 RVA: 0x002B9642 File Offset: 0x002B8642
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if (base.IsInitialized && !this.CanSwitchToViewingMode(this.ViewingMode))
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderViewingModeEnabledConflict"));
			}
		}

		// Token: 0x060068B2 RID: 26802 RVA: 0x002B9671 File Offset: 0x002B8671
		protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
		{
			FlowDocument document = this.Document;
			if (document == null)
			{
				return;
			}
			document.SetDpi(newDpiScaleInfo);
		}

		// Token: 0x060068B3 RID: 26803 RVA: 0x002B9684 File Offset: 0x002B8684
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FlowDocumentReaderAutomationPeer(this);
		}

		// Token: 0x060068B4 RID: 26804 RVA: 0x002B968C File Offset: 0x002B868C
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (base.IsKeyboardFocusWithin && this.CurrentViewer != null && !this.IsFocusWithinDocument())
			{
				((FrameworkElement)this.CurrentViewer).Focus();
			}
		}

		// Token: 0x060068B5 RID: 26805 RVA: 0x002B96C0 File Offset: 0x002B86C0
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

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x060068B6 RID: 26806 RVA: 0x002B9757 File Offset: 0x002B8757
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

		// Token: 0x060068B7 RID: 26807 RVA: 0x002B977A File Offset: 0x002B877A
		internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			return base.BuildRouteCoreHelper(route, args, false);
		}

		// Token: 0x060068B8 RID: 26808 RVA: 0x002B9788 File Offset: 0x002B8788
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool shouldInvalidateIntermediateElements = false;
			return base.InvalidateAutomationAncestorsCoreHelper(branchNodeStack, out continuePastCoreTree, shouldInvalidateIntermediateElements);
		}

		// Token: 0x060068B9 RID: 26809 RVA: 0x002B97A0 File Offset: 0x002B87A0
		protected virtual void SwitchViewingModeCore(FlowDocumentReaderViewingMode viewingMode)
		{
			ITextSelection textSelection = null;
			ContentPosition contentPosition = null;
			DependencyObject dependencyObject = null;
			if (this._contentHost != null)
			{
				bool isKeyboardFocusWithin = base.IsKeyboardFocusWithin;
				IFlowDocumentViewer flowDocumentViewer = this._contentHost.Child as IFlowDocumentViewer;
				if (flowDocumentViewer != null)
				{
					if (isKeyboardFocusWithin && this.IsFocusWithinDocument())
					{
						dependencyObject = (Keyboard.FocusedElement as DependencyObject);
					}
					textSelection = flowDocumentViewer.TextSelection;
					contentPosition = flowDocumentViewer.ContentPosition;
					this.DetachViewer(flowDocumentViewer);
				}
				flowDocumentViewer = this.GetViewerFromMode(viewingMode);
				FrameworkElement frameworkElement = (FrameworkElement)flowDocumentViewer;
				if (flowDocumentViewer != null)
				{
					this._contentHost.Child = frameworkElement;
					this.AttachViewer(flowDocumentViewer);
					flowDocumentViewer.TextSelection = textSelection;
					flowDocumentViewer.ContentPosition = contentPosition;
					if (isKeyboardFocusWithin)
					{
						if (dependencyObject is UIElement)
						{
							((UIElement)dependencyObject).Focus();
						}
						else if (dependencyObject is ContentElement)
						{
							((ContentElement)dependencyObject).Focus();
						}
						else
						{
							frameworkElement.Focus();
						}
					}
				}
				this.UpdateReadOnlyProperties(true, true);
			}
		}

		// Token: 0x060068BA RID: 26810 RVA: 0x002B9880 File Offset: 0x002B8880
		private bool IsFocusWithinDocument()
		{
			DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
			while (dependencyObject != null && dependencyObject != this.Document)
			{
				FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
				if (frameworkElement != null && frameworkElement.TemplatedParent != null)
				{
					dependencyObject = frameworkElement.TemplatedParent;
				}
				else
				{
					dependencyObject = LogicalTreeHelper.GetParent(dependencyObject);
				}
			}
			return dependencyObject != null;
		}

		// Token: 0x060068BB RID: 26811 RVA: 0x002B98CC File Offset: 0x002B88CC
		private void DocumentChanged(FlowDocument oldDocument, FlowDocument newDocument)
		{
			if (oldDocument != null && this._documentAsLogicalChild)
			{
				base.RemoveLogicalChild(oldDocument);
			}
			if (base.TemplatedParent != null && newDocument != null && LogicalTreeHelper.GetParent(newDocument) != null)
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
				newDocument.SetDpi(base.GetDpi());
				if (this._documentAsLogicalChild)
				{
					base.AddLogicalChild(newDocument);
				}
			}
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.SetDocument(newDocument);
			}
			this.UpdateReadOnlyProperties(true, true);
			if (!this.CanShowFindToolBar && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			FlowDocumentReaderAutomationPeer flowDocumentReaderAutomationPeer = UIElementAutomationPeer.FromElement(this) as FlowDocumentReaderAutomationPeer;
			if (flowDocumentReaderAutomationPeer != null)
			{
				flowDocumentReaderAutomationPeer.InvalidatePeer();
			}
		}

		// Token: 0x060068BC RID: 26812 RVA: 0x002B997C File Offset: 0x002B897C
		private void DetachViewer(IFlowDocumentViewer viewer)
		{
			Invariant.Assert(viewer != null && viewer is FrameworkElement);
			FrameworkElement target = (FrameworkElement)viewer;
			BindingOperations.ClearBinding(target, FlowDocumentReader.ZoomProperty);
			BindingOperations.ClearBinding(target, FlowDocumentReader.MaxZoomProperty);
			BindingOperations.ClearBinding(target, FlowDocumentReader.MinZoomProperty);
			BindingOperations.ClearBinding(target, FlowDocumentReader.ZoomIncrementProperty);
			viewer.PageCountChanged -= this.OnPageCountChanged;
			viewer.PageNumberChanged -= this.OnPageNumberChanged;
			viewer.PrintStarted -= this.OnViewerPrintStarted;
			viewer.PrintCompleted -= this.OnViewerPrintCompleted;
			viewer.SetDocument(null);
		}

		// Token: 0x060068BD RID: 26813 RVA: 0x002B9A20 File Offset: 0x002B8A20
		private void AttachViewer(IFlowDocumentViewer viewer)
		{
			Invariant.Assert(viewer != null && viewer is FrameworkElement);
			FrameworkElement fe = (FrameworkElement)viewer;
			viewer.SetDocument(this.Document);
			viewer.PageCountChanged += this.OnPageCountChanged;
			viewer.PageNumberChanged += this.OnPageNumberChanged;
			viewer.PrintStarted += this.OnViewerPrintStarted;
			viewer.PrintCompleted += this.OnViewerPrintCompleted;
			this.CreateTwoWayBinding(fe, FlowDocumentReader.ZoomProperty, "Zoom");
			this.CreateTwoWayBinding(fe, FlowDocumentReader.MaxZoomProperty, "MaxZoom");
			this.CreateTwoWayBinding(fe, FlowDocumentReader.MinZoomProperty, "MinZoom");
			this.CreateTwoWayBinding(fe, FlowDocumentReader.ZoomIncrementProperty, "ZoomIncrement");
		}

		// Token: 0x060068BE RID: 26814 RVA: 0x002B9AE0 File Offset: 0x002B8AE0
		private void CreateTwoWayBinding(FrameworkElement fe, DependencyProperty dp, string propertyPath)
		{
			fe.SetBinding(dp, new Binding(propertyPath)
			{
				Mode = BindingMode.TwoWay,
				Source = this
			});
		}

		// Token: 0x060068BF RID: 26815 RVA: 0x002B9B0C File Offset: 0x002B8B0C
		private bool CanSwitchToViewingMode(FlowDocumentReaderViewingMode mode)
		{
			bool result = false;
			switch (mode)
			{
			case FlowDocumentReaderViewingMode.Page:
				result = this.IsPageViewEnabled;
				break;
			case FlowDocumentReaderViewingMode.TwoPage:
				result = this.IsTwoPageViewEnabled;
				break;
			case FlowDocumentReaderViewingMode.Scroll:
				result = this.IsScrollViewEnabled;
				break;
			}
			return result;
		}

		// Token: 0x060068C0 RID: 26816 RVA: 0x002B9B4C File Offset: 0x002B8B4C
		private IFlowDocumentViewer GetViewerFromMode(FlowDocumentReaderViewingMode mode)
		{
			IFlowDocumentViewer result = null;
			switch (mode)
			{
			case FlowDocumentReaderViewingMode.Page:
				if (this._pageViewer == null)
				{
					this._pageViewer = new ReaderPageViewer();
					this._pageViewer.SetResourceReference(FrameworkElement.StyleProperty, FlowDocumentReader.PageViewStyleKey);
					this._pageViewer.Name = "PageViewer";
					CommandManager.AddPreviewCanExecuteHandler(this._pageViewer, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				result = this._pageViewer;
				break;
			case FlowDocumentReaderViewingMode.TwoPage:
				if (this._twoPageViewer == null)
				{
					this._twoPageViewer = new ReaderTwoPageViewer();
					this._twoPageViewer.SetResourceReference(FrameworkElement.StyleProperty, FlowDocumentReader.TwoPageViewStyleKey);
					this._twoPageViewer.Name = "TwoPageViewer";
					CommandManager.AddPreviewCanExecuteHandler(this._twoPageViewer, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				result = this._twoPageViewer;
				break;
			case FlowDocumentReaderViewingMode.Scroll:
				if (this._scrollViewer == null)
				{
					this._scrollViewer = new ReaderScrollViewer();
					this._scrollViewer.SetResourceReference(FrameworkElement.StyleProperty, FlowDocumentReader.ScrollViewStyleKey);
					this._scrollViewer.Name = "ScrollViewer";
					CommandManager.AddPreviewCanExecuteHandler(this._scrollViewer, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				result = this._scrollViewer;
				break;
			}
			return result;
		}

		// Token: 0x060068C1 RID: 26817 RVA: 0x002B9C7C File Offset: 0x002B8C7C
		private void UpdateReadOnlyProperties(bool pageCountChanged, bool pageNumberChanged)
		{
			if (pageCountChanged)
			{
				base.SetValue(FlowDocumentReader.PageCountPropertyKey, (this.CurrentViewer != null) ? this.CurrentViewer.PageCount : 0);
			}
			if (pageNumberChanged)
			{
				base.SetValue(FlowDocumentReader.PageNumberPropertyKey, (this.CurrentViewer != null) ? this.CurrentViewer.PageNumber : 0);
				base.SetValue(FlowDocumentReader.CanGoToPreviousPagePropertyKey, this.CurrentViewer != null && this.CurrentViewer.CanGoToPreviousPage);
			}
			if (pageCountChanged || pageNumberChanged)
			{
				base.SetValue(FlowDocumentReader.CanGoToNextPagePropertyKey, this.CurrentViewer != null && this.CurrentViewer.CanGoToNextPage);
			}
		}

		// Token: 0x060068C2 RID: 26818 RVA: 0x002B9D22 File Offset: 0x002B8D22
		private void OnPageCountChanged(object sender, EventArgs e)
		{
			Invariant.Assert(this.CurrentViewer != null && sender == this.CurrentViewer);
			this.UpdateReadOnlyProperties(true, false);
		}

		// Token: 0x060068C3 RID: 26819 RVA: 0x002B9D45 File Offset: 0x002B8D45
		private void OnPageNumberChanged(object sender, EventArgs e)
		{
			Invariant.Assert(this.CurrentViewer != null && sender == this.CurrentViewer);
			this.UpdateReadOnlyProperties(false, true);
		}

		// Token: 0x060068C4 RID: 26820 RVA: 0x002B9D68 File Offset: 0x002B8D68
		private void OnViewerPrintStarted(object sender, EventArgs e)
		{
			Invariant.Assert(this.CurrentViewer != null && sender == this.CurrentViewer);
			this._printInProgress = true;
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x060068C5 RID: 26821 RVA: 0x002B9D8F File Offset: 0x002B8D8F
		private void OnViewerPrintCompleted(object sender, EventArgs e)
		{
			Invariant.Assert(this.CurrentViewer != null && sender == this.CurrentViewer);
			this.OnPrintCompleted();
		}

		// Token: 0x060068C6 RID: 26822 RVA: 0x002B9DB0 File Offset: 0x002B8DB0
		private bool ConvertToViewingMode(object value, out FlowDocumentReaderViewingMode mode)
		{
			bool result;
			if (value is FlowDocumentReaderViewingMode)
			{
				mode = (FlowDocumentReaderViewingMode)value;
				result = true;
			}
			else if (value is string)
			{
				string a = (string)value;
				if (a == FlowDocumentReaderViewingMode.Page.ToString())
				{
					mode = FlowDocumentReaderViewingMode.Page;
					result = true;
				}
				else if (a == FlowDocumentReaderViewingMode.TwoPage.ToString())
				{
					mode = FlowDocumentReaderViewingMode.TwoPage;
					result = true;
				}
				else if (a == FlowDocumentReaderViewingMode.Scroll.ToString())
				{
					mode = FlowDocumentReaderViewingMode.Scroll;
					result = true;
				}
				else
				{
					mode = FlowDocumentReaderViewingMode.Page;
					result = false;
				}
			}
			else
			{
				mode = FlowDocumentReaderViewingMode.Page;
				result = false;
			}
			return result;
		}

		// Token: 0x060068C7 RID: 26823 RVA: 0x002B9E48 File Offset: 0x002B8E48
		private void ToggleFindToolBar(bool enable)
		{
			Invariant.Assert(enable == (this.FindToolBar == null));
			if (this._findButton != null && this._findButton.IsChecked != null && this._findButton.IsChecked.Value != enable)
			{
				this._findButton.IsChecked = new bool?(enable);
			}
			DocumentViewerHelper.ToggleFindToolBar(this._findToolBarHost, new EventHandler(this.OnFindInvoked), enable);
		}

		// Token: 0x060068C8 RID: 26824 RVA: 0x002B9EC4 File Offset: 0x002B8EC4
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(FlowDocumentReader.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(FlowDocumentReader.CanExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), FlowDocumentReader.SwitchViewingModeCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+M", "KeySwitchViewingModeDisplayString"));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), ApplicationCommands.Find, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), ApplicationCommands.Print, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), ApplicationCommands.CancelPrint, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.PreviousPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.NextPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.FirstPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.LastPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.IncreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemPlus, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentReader), NavigationCommands.DecreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemMinus, ModifierKeys.Control));
		}

		// Token: 0x060068C9 RID: 26825 RVA: 0x002B9FEC File Offset: 0x002B8FEC
		private static void CanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			FlowDocumentReader flowDocumentReader = target as FlowDocumentReader;
			Invariant.Assert(flowDocumentReader != null, "Target of CanExecuteRoutedEventHandler must be FlowDocumentReader.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (flowDocumentReader._printInProgress)
			{
				args.CanExecute = (args.Command == ApplicationCommands.CancelPrint);
				return;
			}
			if (args.Command == FlowDocumentReader.SwitchViewingModeCommand)
			{
				FlowDocumentReaderViewingMode mode;
				if (flowDocumentReader.ConvertToViewingMode(args.Parameter, out mode))
				{
					args.CanExecute = flowDocumentReader.CanSwitchToViewingMode(mode);
					return;
				}
				args.CanExecute = (args.Parameter == null);
				return;
			}
			else
			{
				if (args.Command == ApplicationCommands.Find)
				{
					args.CanExecute = flowDocumentReader.CanShowFindToolBar;
					return;
				}
				if (args.Command == ApplicationCommands.Print)
				{
					args.CanExecute = (flowDocumentReader.Document != null && flowDocumentReader.IsPrintEnabled);
					return;
				}
				if (args.Command == ApplicationCommands.CancelPrint)
				{
					args.CanExecute = false;
					return;
				}
				args.CanExecute = true;
				return;
			}
		}

		// Token: 0x060068CA RID: 26826 RVA: 0x002BA0D4 File Offset: 0x002B90D4
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			FlowDocumentReader flowDocumentReader = target as FlowDocumentReader;
			Invariant.Assert(flowDocumentReader != null, "Target of ExecutedRoutedEventHandler must be FlowDocumentReader.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == FlowDocumentReader.SwitchViewingModeCommand)
			{
				flowDocumentReader.TrySwitchViewingMode(args.Parameter);
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				flowDocumentReader.OnFindCommand();
				return;
			}
			if (args.Command == ApplicationCommands.Print)
			{
				flowDocumentReader.OnPrintCommand();
				return;
			}
			if (args.Command == ApplicationCommands.CancelPrint)
			{
				flowDocumentReader.OnCancelPrintCommand();
				return;
			}
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				flowDocumentReader.OnIncreaseZoomCommand();
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				flowDocumentReader.OnDecreaseZoomCommand();
				return;
			}
			if (args.Command == NavigationCommands.PreviousPage)
			{
				flowDocumentReader.OnPreviousPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.NextPage)
			{
				flowDocumentReader.OnNextPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.FirstPage)
			{
				flowDocumentReader.OnFirstPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.LastPage)
			{
				flowDocumentReader.OnLastPageCommand();
				return;
			}
			Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
		}

		// Token: 0x060068CB RID: 26827 RVA: 0x002BA1E0 File Offset: 0x002B91E0
		private void TrySwitchViewingMode(object parameter)
		{
			FlowDocumentReaderViewingMode flowDocumentReaderViewingMode;
			if (!this.ConvertToViewingMode(parameter, out flowDocumentReaderViewingMode))
			{
				if (parameter != null)
				{
					return;
				}
				flowDocumentReaderViewingMode = (this.ViewingMode + 1) % (FlowDocumentReaderViewingMode)3;
			}
			while (!this.CanSwitchToViewingMode(flowDocumentReaderViewingMode))
			{
				flowDocumentReaderViewingMode = (flowDocumentReaderViewingMode + 1) % (FlowDocumentReaderViewingMode)3;
			}
			base.SetCurrentValueInternal(FlowDocumentReader.ViewingModeProperty, flowDocumentReaderViewingMode);
		}

		// Token: 0x060068CC RID: 26828 RVA: 0x002BA229 File Offset: 0x002B9229
		private void OnPreviousPageCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.PreviousPage();
			}
		}

		// Token: 0x060068CD RID: 26829 RVA: 0x002BA23E File Offset: 0x002B923E
		private void OnNextPageCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.NextPage();
			}
		}

		// Token: 0x060068CE RID: 26830 RVA: 0x002BA253 File Offset: 0x002B9253
		private void OnFirstPageCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.FirstPage();
			}
		}

		// Token: 0x060068CF RID: 26831 RVA: 0x002BA268 File Offset: 0x002B9268
		private void OnLastPageCommand()
		{
			if (this.CurrentViewer != null)
			{
				this.CurrentViewer.LastPage();
			}
		}

		// Token: 0x060068D0 RID: 26832 RVA: 0x002BA280 File Offset: 0x002B9280
		private void OnFindInvoked(object sender, EventArgs e)
		{
			TextEditor textEditor = this.TextEditor;
			FindToolBar findToolBar = this.FindToolBar;
			if (findToolBar != null && textEditor != null)
			{
				if (this.CurrentViewer != null && this.CurrentViewer is UIElement)
				{
					((UIElement)this.CurrentViewer).Focus();
				}
				ITextRange textRange = DocumentViewerHelper.Find(findToolBar, textEditor, textEditor.TextView, textEditor.TextView);
				if (textRange != null && !textRange.IsEmpty)
				{
					if (this.CurrentViewer != null)
					{
						this.CurrentViewer.ShowFindResult(textRange);
						return;
					}
				}
				else
				{
					DocumentViewerHelper.ShowFindUnsuccessfulMessage(findToolBar);
				}
			}
		}

		// Token: 0x060068D1 RID: 26833 RVA: 0x002BA304 File Offset: 0x002B9304
		private void PreviewCanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			if (args.Command == ApplicationCommands.Find)
			{
				args.CanExecute = false;
				args.Handled = true;
				return;
			}
			if (args.Command == ApplicationCommands.Print)
			{
				args.CanExecute = this.IsPrintEnabled;
				args.Handled = !this.IsPrintEnabled;
			}
		}

		// Token: 0x060068D2 RID: 26834 RVA: 0x002BA355 File Offset: 0x002B9355
		private static void KeyDownHandler(object sender, KeyEventArgs e)
		{
			DocumentViewerHelper.KeyDownHelper(e, ((FlowDocumentReader)sender)._findToolBarHost);
		}

		// Token: 0x060068D3 RID: 26835 RVA: 0x002BA368 File Offset: 0x002B9368
		private static void ViewingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (flowDocumentReader.CanSwitchToViewingMode((FlowDocumentReaderViewingMode)e.NewValue))
			{
				flowDocumentReader.SwitchViewingModeCore((FlowDocumentReaderViewingMode)e.NewValue);
			}
			else if (flowDocumentReader.IsInitialized)
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderViewingModeEnabledConflict"));
			}
			FlowDocumentReaderAutomationPeer flowDocumentReaderAutomationPeer = UIElementAutomationPeer.FromElement(flowDocumentReader) as FlowDocumentReaderAutomationPeer;
			if (flowDocumentReaderAutomationPeer != null)
			{
				flowDocumentReaderAutomationPeer.RaiseCurrentViewChangedEvent((FlowDocumentReaderViewingMode)e.NewValue, (FlowDocumentReaderViewingMode)e.OldValue);
			}
		}

		// Token: 0x060068D4 RID: 26836 RVA: 0x002BA400 File Offset: 0x002B9400
		private static bool IsValidViewingMode(object o)
		{
			FlowDocumentReaderViewingMode flowDocumentReaderViewingMode = (FlowDocumentReaderViewingMode)o;
			return flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.Page || flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.TwoPage || flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.Scroll;
		}

		// Token: 0x060068D5 RID: 26837 RVA: 0x002BA424 File Offset: 0x002B9424
		private static void ViewingModeEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (!flowDocumentReader.IsPageViewEnabled && !flowDocumentReader.IsTwoPageViewEnabled && !flowDocumentReader.IsScrollViewEnabled)
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderCannotDisableAllViewingModes"));
			}
			if (flowDocumentReader.IsInitialized && !flowDocumentReader.CanSwitchToViewingMode(flowDocumentReader.ViewingMode))
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderViewingModeEnabledConflict"));
			}
			FlowDocumentReaderAutomationPeer flowDocumentReaderAutomationPeer = UIElementAutomationPeer.FromElement(flowDocumentReader) as FlowDocumentReaderAutomationPeer;
			if (flowDocumentReaderAutomationPeer != null)
			{
				flowDocumentReaderAutomationPeer.RaiseSupportedViewsChangedEvent(e);
			}
		}

		// Token: 0x060068D6 RID: 26838 RVA: 0x002BA4B0 File Offset: 0x002B94B0
		private static void IsFindEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (!flowDocumentReader.CanShowFindToolBar && flowDocumentReader.FindToolBar != null)
			{
				flowDocumentReader.ToggleFindToolBar(false);
			}
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x060068D7 RID: 26839 RVA: 0x002BA4F4 File Offset: 0x002B94F4
		private static void IsPrintEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x060068D8 RID: 26840 RVA: 0x002BA516 File Offset: 0x002B9516
		private static void DocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			((FlowDocumentReader)d).DocumentChanged((FlowDocument)e.OldValue, (FlowDocument)e.NewValue);
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x060068D9 RID: 26841 RVA: 0x002BA554 File Offset: 0x002B9554
		private static void ZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (!DoubleUtil.AreClose((double)e.OldValue, (double)e.NewValue))
			{
				flowDocumentReader.SetValue(FlowDocumentReader.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(flowDocumentReader.MaxZoom, flowDocumentReader.Zoom)));
				flowDocumentReader.SetValue(FlowDocumentReader.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(flowDocumentReader.MinZoom, flowDocumentReader.Zoom)));
			}
		}

		// Token: 0x060068DA RID: 26842 RVA: 0x002BA5E0 File Offset: 0x002B95E0
		private static object CoerceZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			double value2 = (double)value;
			double maxZoom = flowDocumentReader.MaxZoom;
			if (DoubleUtil.LessThan(maxZoom, value2))
			{
				return maxZoom;
			}
			double minZoom = flowDocumentReader.MinZoom;
			if (DoubleUtil.GreaterThan(minZoom, value2))
			{
				return minZoom;
			}
			return value;
		}

		// Token: 0x060068DB RID: 26843 RVA: 0x002BA640 File Offset: 0x002B9640
		private static void MaxZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			flowDocumentReader.CoerceValue(FlowDocumentReader.ZoomProperty);
			flowDocumentReader.SetValue(FlowDocumentReader.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(flowDocumentReader.MaxZoom, flowDocumentReader.Zoom)));
		}

		// Token: 0x060068DC RID: 26844 RVA: 0x002BA694 File Offset: 0x002B9694
		private static object CoerceMaxZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			double minZoom = ((FlowDocumentReader)d).MinZoom;
			if ((double)value >= minZoom)
			{
				return value;
			}
			return minZoom;
		}

		// Token: 0x060068DD RID: 26845 RVA: 0x002BA6D4 File Offset: 0x002B96D4
		private static void MinZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentReader);
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			flowDocumentReader.CoerceValue(FlowDocumentReader.MaxZoomProperty);
			flowDocumentReader.CoerceValue(FlowDocumentReader.ZoomProperty);
			flowDocumentReader.SetValue(FlowDocumentReader.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(flowDocumentReader.MinZoom, flowDocumentReader.Zoom)));
		}

		// Token: 0x060068DE RID: 26846 RVA: 0x002BA734 File Offset: 0x002B9734
		private static bool ZoomValidateValue(object o)
		{
			double num = (double)o;
			return !double.IsNaN(num) && !double.IsInfinity(num) && DoubleUtil.GreaterThan(num, 0.0);
		}

		// Token: 0x060068DF RID: 26847 RVA: 0x002BA76C File Offset: 0x002B976C
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlowDocumentReader flowDocumentReader = (FlowDocumentReader)d;
			if (flowDocumentReader.Selection != null)
			{
				CaretElement caretElement = flowDocumentReader.Selection.CaretElement;
				if (caretElement != null)
				{
					caretElement.InvalidateVisual();
				}
			}
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x060068E0 RID: 26848 RVA: 0x002BA79D File Offset: 0x002B979D
		private bool CanShowFindToolBar
		{
			get
			{
				return this._findToolBarHost != null && this.IsFindEnabled && this.Document != null;
			}
		}

		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x060068E1 RID: 26849 RVA: 0x002BA7BC File Offset: 0x002B97BC
		private TextEditor TextEditor
		{
			get
			{
				TextEditor result = null;
				IFlowDocumentViewer currentViewer = this.CurrentViewer;
				if (currentViewer != null && currentViewer.TextSelection != null)
				{
					result = currentViewer.TextSelection.TextEditor;
				}
				return result;
			}
		}

		// Token: 0x1700183D RID: 6205
		// (get) Token: 0x060068E2 RID: 26850 RVA: 0x002BA7EA File Offset: 0x002B97EA
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

		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x060068E3 RID: 26851 RVA: 0x002BA806 File Offset: 0x002B9806
		private IFlowDocumentViewer CurrentViewer
		{
			get
			{
				if (this._contentHost != null)
				{
					return (IFlowDocumentViewer)this._contentHost.Child;
				}
				return null;
			}
		}

		// Token: 0x060068E4 RID: 26852 RVA: 0x002BA824 File Offset: 0x002B9824
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.Document != null)
			{
				throw new ArgumentException(SR.Get("FlowDocumentReaderCanHaveOnlyOneChild"));
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

		// Token: 0x060068E5 RID: 26853 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x060068E6 RID: 26854 RVA: 0x002BA89C File Offset: 0x002B989C
		CustomJournalStateInternal IJournalState.GetJournalState(JournalReason journalReason)
		{
			int contentPosition = -1;
			LogicalDirection contentPositionDirection = LogicalDirection.Forward;
			IFlowDocumentViewer currentViewer = this.CurrentViewer;
			if (currentViewer != null)
			{
				TextPointer textPointer = currentViewer.ContentPosition as TextPointer;
				if (textPointer != null)
				{
					contentPosition = textPointer.Offset;
					contentPositionDirection = textPointer.LogicalDirection;
				}
			}
			return new FlowDocumentReader.JournalState(contentPosition, contentPositionDirection, this.Zoom, this.ViewingMode);
		}

		// Token: 0x060068E7 RID: 26855 RVA: 0x002BA8E8 File Offset: 0x002B98E8
		void IJournalState.RestoreJournalState(CustomJournalStateInternal state)
		{
			FlowDocumentReader.JournalState journalState = state as FlowDocumentReader.JournalState;
			if (state != null)
			{
				base.SetCurrentValueInternal(FlowDocumentReader.ZoomProperty, journalState.Zoom);
				base.SetCurrentValueInternal(FlowDocumentReader.ViewingModeProperty, journalState.ViewingMode);
				if (journalState.ContentPosition != -1)
				{
					IFlowDocumentViewer currentViewer = this.CurrentViewer;
					FlowDocument document = this.Document;
					if (currentViewer != null && document != null)
					{
						TextContainer textContainer = document.StructuralCache.TextContainer;
						if (journalState.ContentPosition <= textContainer.SymbolCount)
						{
							TextPointer contentPosition = textContainer.CreatePointerAtOffset(journalState.ContentPosition, journalState.ContentPositionDirection);
							currentViewer.ContentPosition = contentPosition;
						}
					}
				}
			}
		}

		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x060068E8 RID: 26856 RVA: 0x002BA97E File Offset: 0x002B997E
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return FlowDocumentReader._dType;
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x060068E9 RID: 26857 RVA: 0x002BA985 File Offset: 0x002B9985
		private static ResourceKey PageViewStyleKey
		{
			get
			{
				if (FlowDocumentReader._pageViewStyleKey == null)
				{
					FlowDocumentReader._pageViewStyleKey = new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIPageViewStyleKey");
				}
				return FlowDocumentReader._pageViewStyleKey;
			}
		}

		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x060068EA RID: 26858 RVA: 0x002BA9AC File Offset: 0x002B99AC
		private static ResourceKey TwoPageViewStyleKey
		{
			get
			{
				if (FlowDocumentReader._twoPageViewStyleKey == null)
				{
					FlowDocumentReader._twoPageViewStyleKey = new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUITwoPageViewStyleKey");
				}
				return FlowDocumentReader._twoPageViewStyleKey;
			}
		}

		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x060068EB RID: 26859 RVA: 0x002BA9D3 File Offset: 0x002B99D3
		private static ResourceKey ScrollViewStyleKey
		{
			get
			{
				if (FlowDocumentReader._scrollViewStyleKey == null)
				{
					FlowDocumentReader._scrollViewStyleKey = new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIScrollViewStyleKey");
				}
				return FlowDocumentReader._scrollViewStyleKey;
			}
		}

		// Token: 0x040034B2 RID: 13490
		public static readonly DependencyProperty ViewingModeProperty = DependencyProperty.Register("ViewingMode", typeof(FlowDocumentReaderViewingMode), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(FlowDocumentReaderViewingMode.Page, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocumentReader.ViewingModeChanged)), new ValidateValueCallback(FlowDocumentReader.IsValidViewingMode));

		// Token: 0x040034B3 RID: 13491
		public static readonly DependencyProperty IsPageViewEnabledProperty = DependencyProperty.Register("IsPageViewEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.ViewingModeEnabledChanged)));

		// Token: 0x040034B4 RID: 13492
		public static readonly DependencyProperty IsTwoPageViewEnabledProperty = DependencyProperty.Register("IsTwoPageViewEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.ViewingModeEnabledChanged)));

		// Token: 0x040034B5 RID: 13493
		public static readonly DependencyProperty IsScrollViewEnabledProperty = DependencyProperty.Register("IsScrollViewEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.ViewingModeEnabledChanged)));

		// Token: 0x040034B6 RID: 13494
		private static readonly DependencyPropertyKey PageCountPropertyKey = DependencyProperty.RegisterReadOnly("PageCount", typeof(int), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(0));

		// Token: 0x040034B7 RID: 13495
		public static readonly DependencyProperty PageCountProperty = FlowDocumentReader.PageCountPropertyKey.DependencyProperty;

		// Token: 0x040034B8 RID: 13496
		private static readonly DependencyPropertyKey PageNumberPropertyKey = DependencyProperty.RegisterReadOnly("PageNumber", typeof(int), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(0));

		// Token: 0x040034B9 RID: 13497
		public static readonly DependencyProperty PageNumberProperty = FlowDocumentReader.PageNumberPropertyKey.DependencyProperty;

		// Token: 0x040034BA RID: 13498
		private static readonly DependencyPropertyKey CanGoToPreviousPagePropertyKey = DependencyProperty.RegisterReadOnly("CanGoToPreviousPage", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x040034BB RID: 13499
		public static readonly DependencyProperty CanGoToPreviousPageProperty = FlowDocumentReader.CanGoToPreviousPagePropertyKey.DependencyProperty;

		// Token: 0x040034BC RID: 13500
		private static readonly DependencyPropertyKey CanGoToNextPagePropertyKey = DependencyProperty.RegisterReadOnly("CanGoToNextPage", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x040034BD RID: 13501
		public static readonly DependencyProperty CanGoToNextPageProperty = FlowDocumentReader.CanGoToNextPagePropertyKey.DependencyProperty;

		// Token: 0x040034BE RID: 13502
		public static readonly DependencyProperty IsFindEnabledProperty = DependencyProperty.Register("IsFindEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.IsFindEnabledChanged)));

		// Token: 0x040034BF RID: 13503
		public static readonly DependencyProperty IsPrintEnabledProperty = DependencyProperty.Register("IsPrintEnabled", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentReader.IsPrintEnabledChanged)));

		// Token: 0x040034C0 RID: 13504
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(FlowDocumentReader.DocumentChanged)));

		// Token: 0x040034C1 RID: 13505
		public static readonly DependencyProperty ZoomProperty = FlowDocumentPageViewer.ZoomProperty.AddOwner(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(100.0, new PropertyChangedCallback(FlowDocumentReader.ZoomChanged), new CoerceValueCallback(FlowDocumentReader.CoerceZoom)));

		// Token: 0x040034C2 RID: 13506
		public static readonly DependencyProperty MaxZoomProperty = FlowDocumentPageViewer.MaxZoomProperty.AddOwner(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(200.0, new PropertyChangedCallback(FlowDocumentReader.MaxZoomChanged), new CoerceValueCallback(FlowDocumentReader.CoerceMaxZoom)));

		// Token: 0x040034C3 RID: 13507
		public static readonly DependencyProperty MinZoomProperty = FlowDocumentPageViewer.MinZoomProperty.AddOwner(typeof(FlowDocumentReader), new FrameworkPropertyMetadata(80.0, new PropertyChangedCallback(FlowDocumentReader.MinZoomChanged)));

		// Token: 0x040034C4 RID: 13508
		public static readonly DependencyProperty ZoomIncrementProperty = FlowDocumentPageViewer.ZoomIncrementProperty.AddOwner(typeof(FlowDocumentReader));

		// Token: 0x040034C5 RID: 13509
		private static readonly DependencyPropertyKey CanIncreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanIncreaseZoom", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		// Token: 0x040034C6 RID: 13510
		public static readonly DependencyProperty CanIncreaseZoomProperty = FlowDocumentReader.CanIncreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x040034C7 RID: 13511
		private static readonly DependencyPropertyKey CanDecreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanDecreaseZoom", typeof(bool), typeof(FlowDocumentReader), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		// Token: 0x040034C8 RID: 13512
		public static readonly DependencyProperty CanDecreaseZoomProperty = FlowDocumentReader.CanDecreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x040034C9 RID: 13513
		public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(FlowDocumentReader));

		// Token: 0x040034CA RID: 13514
		public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(FlowDocumentReader));

		// Token: 0x040034CB RID: 13515
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(FlowDocumentReader));

		// Token: 0x040034CC RID: 13516
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = TextBoxBase.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(FlowDocumentReader));

		// Token: 0x040034CD RID: 13517
		public static readonly RoutedUICommand SwitchViewingModeCommand = new RoutedUICommand("_Switch ViewingMode", "SwitchViewingMode", typeof(FlowDocumentReader), null);

		// Token: 0x040034CE RID: 13518
		private Decorator _contentHost;

		// Token: 0x040034CF RID: 13519
		private Decorator _findToolBarHost;

		// Token: 0x040034D0 RID: 13520
		private ToggleButton _findButton;

		// Token: 0x040034D1 RID: 13521
		private ReaderPageViewer _pageViewer;

		// Token: 0x040034D2 RID: 13522
		private ReaderTwoPageViewer _twoPageViewer;

		// Token: 0x040034D3 RID: 13523
		private ReaderScrollViewer _scrollViewer;

		// Token: 0x040034D4 RID: 13524
		private bool _documentAsLogicalChild;

		// Token: 0x040034D5 RID: 13525
		private bool _printInProgress;

		// Token: 0x040034D6 RID: 13526
		private const string _contentHostTemplateName = "PART_ContentHost";

		// Token: 0x040034D7 RID: 13527
		private const string _findToolBarHostTemplateName = "PART_FindToolBarHost";

		// Token: 0x040034D8 RID: 13528
		private const string _findButtonTemplateName = "FindButton";

		// Token: 0x040034D9 RID: 13529
		private const string KeySwitchViewingMode = "Ctrl+M";

		// Token: 0x040034DA RID: 13530
		private const string Switch_ViewingMode = "_Switch ViewingMode";

		// Token: 0x040034DB RID: 13531
		private static DependencyObjectType _dType;

		// Token: 0x040034DC RID: 13532
		private static ComponentResourceKey _pageViewStyleKey;

		// Token: 0x040034DD RID: 13533
		private static ComponentResourceKey _twoPageViewStyleKey;

		// Token: 0x040034DE RID: 13534
		private static ComponentResourceKey _scrollViewStyleKey;

		// Token: 0x02000BD4 RID: 3028
		[Serializable]
		private class JournalState : CustomJournalStateInternal
		{
			// Token: 0x06008F91 RID: 36753 RVA: 0x00344988 File Offset: 0x00343988
			public JournalState(int contentPosition, LogicalDirection contentPositionDirection, double zoom, FlowDocumentReaderViewingMode viewingMode)
			{
				this.ContentPosition = contentPosition;
				this.ContentPositionDirection = contentPositionDirection;
				this.Zoom = zoom;
				this.ViewingMode = viewingMode;
			}

			// Token: 0x04004A10 RID: 18960
			public int ContentPosition;

			// Token: 0x04004A11 RID: 18961
			public LogicalDirection ContentPositionDirection;

			// Token: 0x04004A12 RID: 18962
			public double Zoom;

			// Token: 0x04004A13 RID: 18963
			public FlowDocumentReaderViewingMode ViewingMode;
		}
	}
}
