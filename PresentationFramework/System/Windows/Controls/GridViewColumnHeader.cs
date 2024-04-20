using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x0200078A RID: 1930
	[TemplatePart(Name = "PART_FloatingHeaderCanvas", Type = typeof(Canvas))]
	[TemplatePart(Name = "PART_HeaderGripper", Type = typeof(Thumb))]
	public class GridViewColumnHeader : ButtonBase
	{
		// Token: 0x06006ABD RID: 27325 RVA: 0x002C2A60 File Offset: 0x002C1A60
		static GridViewColumnHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(typeof(GridViewColumnHeader)));
			GridViewColumnHeader._dType = DependencyObjectType.FromSystemTypeInternal(typeof(GridViewColumnHeader));
			UIElement.FocusableProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
			ContentControl.ContentTemplateProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
			ContentControl.ContentTemplateSelectorProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
			FrameworkElement.ContextMenuProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
			FrameworkElement.ToolTipProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
		}

		// Token: 0x06006ABE RID: 27326 RVA: 0x002C2BF8 File Offset: 0x002C1BF8
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			GridViewColumnHeaderRole role = this.Role;
			if (role == GridViewColumnHeaderRole.Normal)
			{
				this.HookupGripperEvents();
				return;
			}
			if (role == GridViewColumnHeaderRole.Floating)
			{
				this._floatingHeaderCanvas = (base.GetTemplateChild("PART_FloatingHeaderCanvas") as Canvas);
				this.UpdateFloatingHeaderCanvas();
			}
		}

		// Token: 0x170018AD RID: 6317
		// (get) Token: 0x06006ABF RID: 27327 RVA: 0x002C2C3C File Offset: 0x002C1C3C
		public GridViewColumn Column
		{
			get
			{
				return (GridViewColumn)base.GetValue(GridViewColumnHeader.ColumnProperty);
			}
		}

		// Token: 0x170018AE RID: 6318
		// (get) Token: 0x06006AC0 RID: 27328 RVA: 0x002C2C4E File Offset: 0x002C1C4E
		[Category("Behavior")]
		public GridViewColumnHeaderRole Role
		{
			get
			{
				return (GridViewColumnHeaderRole)base.GetValue(GridViewColumnHeader.RoleProperty);
			}
		}

		// Token: 0x06006AC1 RID: 27329 RVA: 0x002C2C60 File Offset: 0x002C1C60
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			e.Handled = false;
			if (base.ClickMode == ClickMode.Hover && base.IsMouseCaptured)
			{
				base.ReleaseMouseCapture();
			}
		}

		// Token: 0x06006AC2 RID: 27330 RVA: 0x002C2C87 File Offset: 0x002C1C87
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			e.Handled = false;
			if (base.ClickMode == ClickMode.Hover && e.ButtonState == MouseButtonState.Pressed)
			{
				base.CaptureMouse();
			}
		}

		// Token: 0x06006AC3 RID: 27331 RVA: 0x002C2CB0 File Offset: 0x002C1CB0
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (base.ClickMode != ClickMode.Hover && base.IsMouseCaptured && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
			{
				base.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.TrueBox);
			}
			e.Handled = false;
		}

		// Token: 0x06006AC4 RID: 27332 RVA: 0x002C2CEE File Offset: 0x002C1CEE
		protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			this.CheckWidthForPreviousHeaderGripper();
		}

		// Token: 0x06006AC5 RID: 27333 RVA: 0x002C2CFD File Offset: 0x002C1CFD
		protected override void OnClick()
		{
			if (!this.SuppressClickEvent && (this.IsAccessKeyOrAutomation || !this.IsMouseOutside()))
			{
				this.IsAccessKeyOrAutomation = false;
				this.ClickImplement();
				this.MakeParentGotFocus();
			}
		}

		// Token: 0x06006AC6 RID: 27334 RVA: 0x002C2D2A File Offset: 0x002C1D2A
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			this.IsAccessKeyOrAutomation = true;
			base.OnAccessKey(e);
		}

		// Token: 0x06006AC7 RID: 27335 RVA: 0x002C2D3C File Offset: 0x002C1D3C
		protected internal override bool ShouldSerializeProperty(DependencyProperty dp)
		{
			if (this.IsInternalGenerated)
			{
				return false;
			}
			GridViewColumnHeader.Flags flags;
			GridViewColumnHeader.Flags flags2;
			GridViewColumnHeader.PropertyToFlags(dp, out flags, out flags2);
			return (flags == GridViewColumnHeader.Flags.None || this.GetFlag(flags)) && base.ShouldSerializeProperty(dp);
		}

		// Token: 0x06006AC8 RID: 27336 RVA: 0x002C2D72 File Offset: 0x002C1D72
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		// Token: 0x06006AC9 RID: 27337 RVA: 0x002C2D72 File Offset: 0x002C1D72
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		// Token: 0x06006ACA RID: 27338 RVA: 0x002C2D83 File Offset: 0x002C1D83
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (base.ClickMode == ClickMode.Hover && base.IsMouseCaptured)
			{
				base.ReleaseMouseCapture();
			}
		}

		// Token: 0x06006ACB RID: 27339 RVA: 0x002C2DA3 File Offset: 0x002C1DA3
		internal void AutomationClick()
		{
			this.IsAccessKeyOrAutomation = true;
			this.OnClick();
		}

		// Token: 0x06006ACC RID: 27340 RVA: 0x002C2DB2 File Offset: 0x002C1DB2
		internal void OnColumnHeaderKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && this._headerGripper != null && this._headerGripper.IsDragging)
			{
				this._headerGripper.CancelDrag();
				e.Handled = true;
			}
		}

		// Token: 0x06006ACD RID: 27341 RVA: 0x002C2DE8 File Offset: 0x002C1DE8
		internal void CheckWidthForPreviousHeaderGripper()
		{
			bool hide = false;
			if (this._headerGripper != null)
			{
				hide = DoubleUtil.LessThan(base.ActualWidth, this._headerGripper.Width);
			}
			if (this._previousHeader != null)
			{
				this._previousHeader.HideGripperRightHalf(hide);
			}
			this.UpdateGripperCursor();
		}

		// Token: 0x06006ACE RID: 27342 RVA: 0x002C2E30 File Offset: 0x002C1E30
		internal void ResetFloatingHeaderCanvasBackground()
		{
			if (this._floatingHeaderCanvas != null)
			{
				this._floatingHeaderCanvas.Background = null;
			}
		}

		// Token: 0x06006ACF RID: 27343 RVA: 0x002C2E48 File Offset: 0x002C1E48
		internal void UpdateProperty(DependencyProperty dp, object value)
		{
			GridViewColumnHeader.Flags flag = GridViewColumnHeader.Flags.None;
			if (!this.IsInternalGenerated)
			{
				GridViewColumnHeader.Flags flag2;
				GridViewColumnHeader.PropertyToFlags(dp, out flag2, out flag);
				if (this.GetFlag(flag2))
				{
					return;
				}
				this.SetFlag(flag, true);
			}
			if (value != null)
			{
				base.SetValue(dp, value);
			}
			else
			{
				base.ClearValue(dp);
			}
			this.SetFlag(flag, false);
		}

		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x06006AD0 RID: 27344 RVA: 0x002C2E97 File Offset: 0x002C1E97
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return GridViewColumnHeader._dType;
			}
		}

		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x06006AD1 RID: 27345 RVA: 0x002C2E9E File Offset: 0x002C1E9E
		// (set) Token: 0x06006AD2 RID: 27346 RVA: 0x002C2EA6 File Offset: 0x002C1EA6
		internal GridViewColumnHeader PreviousVisualHeader
		{
			get
			{
				return this._previousHeader;
			}
			set
			{
				this._previousHeader = value;
			}
		}

		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x06006AD3 RID: 27347 RVA: 0x002C2EAF File Offset: 0x002C1EAF
		// (set) Token: 0x06006AD4 RID: 27348 RVA: 0x002C2EBC File Offset: 0x002C1EBC
		internal bool SuppressClickEvent
		{
			get
			{
				return this.GetFlag(GridViewColumnHeader.Flags.SuppressClickEvent);
			}
			set
			{
				this.SetFlag(GridViewColumnHeader.Flags.SuppressClickEvent, value);
			}
		}

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x06006AD5 RID: 27349 RVA: 0x002C2ECA File Offset: 0x002C1ECA
		// (set) Token: 0x06006AD6 RID: 27350 RVA: 0x002C2ED2 File Offset: 0x002C1ED2
		internal GridViewColumnHeader FloatSourceHeader
		{
			get
			{
				return this._srcHeader;
			}
			set
			{
				this._srcHeader = value;
			}
		}

		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x06006AD7 RID: 27351 RVA: 0x002C2EDB File Offset: 0x002C1EDB
		// (set) Token: 0x06006AD8 RID: 27352 RVA: 0x002C2EE8 File Offset: 0x002C1EE8
		internal bool IsInternalGenerated
		{
			get
			{
				return this.GetFlag(GridViewColumnHeader.Flags.IsInternalGenerated);
			}
			set
			{
				this.SetFlag(GridViewColumnHeader.Flags.IsInternalGenerated, value);
			}
		}

		// Token: 0x06006AD9 RID: 27353 RVA: 0x002C2EF6 File Offset: 0x002C1EF6
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GridViewColumnHeaderAutomationPeer(this);
		}

		// Token: 0x06006ADA RID: 27354 RVA: 0x002C2F00 File Offset: 0x002C1F00
		private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumnHeader gridViewColumnHeader = (GridViewColumnHeader)d;
			if (!gridViewColumnHeader.IsInternalGenerated)
			{
				GridViewColumnHeader.Flags flag;
				GridViewColumnHeader.Flags flag2;
				GridViewColumnHeader.PropertyToFlags(e.Property, out flag, out flag2);
				if (!gridViewColumnHeader.GetFlag(flag2))
				{
					if (e.NewValueSource == BaseValueSourceInternal.Local)
					{
						gridViewColumnHeader.SetFlag(flag, true);
						return;
					}
					gridViewColumnHeader.SetFlag(flag, false);
					GridViewHeaderRowPresenter gridViewHeaderRowPresenter = gridViewColumnHeader.Parent as GridViewHeaderRowPresenter;
					if (gridViewHeaderRowPresenter != null)
					{
						gridViewHeaderRowPresenter.UpdateHeaderProperty(gridViewColumnHeader, e.Property);
					}
				}
			}
		}

		// Token: 0x06006ADB RID: 27355 RVA: 0x002C2F70 File Offset: 0x002C1F70
		private static void PropertyToFlags(DependencyProperty dp, out GridViewColumnHeader.Flags flag, out GridViewColumnHeader.Flags ignoreFlag)
		{
			if (dp == FrameworkElement.StyleProperty)
			{
				flag = GridViewColumnHeader.Flags.StyleSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreStyle;
				return;
			}
			if (dp == ContentControl.ContentTemplateProperty)
			{
				flag = GridViewColumnHeader.Flags.ContentTemplateSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreContentTemplate;
				return;
			}
			if (dp == ContentControl.ContentTemplateSelectorProperty)
			{
				flag = GridViewColumnHeader.Flags.ContentTemplateSelectorSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreContentTemplateSelector;
				return;
			}
			if (dp == ContentControl.ContentStringFormatProperty)
			{
				flag = GridViewColumnHeader.Flags.ContentStringFormatSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreContentStringFormat;
				return;
			}
			if (dp == FrameworkElement.ContextMenuProperty)
			{
				flag = GridViewColumnHeader.Flags.ContextMenuSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreContextMenu;
				return;
			}
			if (dp == FrameworkElement.ToolTipProperty)
			{
				flag = GridViewColumnHeader.Flags.ToolTipSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreToolTip;
				return;
			}
			flag = (ignoreFlag = GridViewColumnHeader.Flags.None);
		}

		// Token: 0x06006ADC RID: 27356 RVA: 0x002C2FF8 File Offset: 0x002C1FF8
		private void HideGripperRightHalf(bool hide)
		{
			if (this._headerGripper != null)
			{
				FrameworkElement frameworkElement = this._headerGripper.Parent as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.ClipToBounds = hide;
				}
			}
		}

		// Token: 0x06006ADD RID: 27357 RVA: 0x002C3028 File Offset: 0x002C2028
		private void OnColumnHeaderGripperDragStarted(object sender, DragStartedEventArgs e)
		{
			this.MakeParentGotFocus();
			this._originalWidth = this.ColumnActualWidth;
			e.Handled = true;
		}

		// Token: 0x06006ADE RID: 27358 RVA: 0x002C3044 File Offset: 0x002C2044
		private void MakeParentGotFocus()
		{
			GridViewHeaderRowPresenter gridViewHeaderRowPresenter = base.Parent as GridViewHeaderRowPresenter;
			if (gridViewHeaderRowPresenter != null)
			{
				gridViewHeaderRowPresenter.MakeParentItemsControlGotFocus();
			}
		}

		// Token: 0x06006ADF RID: 27359 RVA: 0x002C3068 File Offset: 0x002C2068
		private void OnColumnHeaderResize(object sender, DragDeltaEventArgs e)
		{
			double num = this.ColumnActualWidth + e.HorizontalChange;
			if (DoubleUtil.LessThanOrClose(num, 0.0))
			{
				num = 0.0;
			}
			this.UpdateColumnHeaderWidth(num);
			e.Handled = true;
		}

		// Token: 0x06006AE0 RID: 27360 RVA: 0x002C30AC File Offset: 0x002C20AC
		private void OnColumnHeaderGripperDragCompleted(object sender, DragCompletedEventArgs e)
		{
			if (e.Canceled)
			{
				this.UpdateColumnHeaderWidth(this._originalWidth);
			}
			this.UpdateGripperCursor();
			e.Handled = true;
		}

		// Token: 0x06006AE1 RID: 27361 RVA: 0x002C30D0 File Offset: 0x002C20D0
		private void HookupGripperEvents()
		{
			this.UnhookGripperEvents();
			this._headerGripper = (base.GetTemplateChild("PART_HeaderGripper") as Thumb);
			if (this._headerGripper != null)
			{
				this._headerGripper.DragStarted += this.OnColumnHeaderGripperDragStarted;
				this._headerGripper.DragDelta += this.OnColumnHeaderResize;
				this._headerGripper.DragCompleted += this.OnColumnHeaderGripperDragCompleted;
				this._headerGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this._headerGripper.MouseEnter += this.OnGripperMouseEnterLeave;
				this._headerGripper.MouseLeave += this.OnGripperMouseEnterLeave;
				this._headerGripper.Cursor = this.SplitCursor;
			}
		}

		// Token: 0x06006AE2 RID: 27362 RVA: 0x002C31A0 File Offset: 0x002C21A0
		private void OnGripperDoubleClicked(object sender, MouseButtonEventArgs e)
		{
			if (this.Column != null)
			{
				if (double.IsNaN(this.Column.Width))
				{
					this.Column.Width = this.Column.ActualWidth;
				}
				this.Column.Width = double.NaN;
				e.Handled = true;
			}
		}

		// Token: 0x06006AE3 RID: 27363 RVA: 0x002C31F8 File Offset: 0x002C21F8
		private void UnhookGripperEvents()
		{
			if (this._headerGripper != null)
			{
				this._headerGripper.DragStarted -= this.OnColumnHeaderGripperDragStarted;
				this._headerGripper.DragDelta -= this.OnColumnHeaderResize;
				this._headerGripper.DragCompleted -= this.OnColumnHeaderGripperDragCompleted;
				this._headerGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._headerGripper.MouseEnter -= this.OnGripperMouseEnterLeave;
				this._headerGripper.MouseLeave -= this.OnGripperMouseEnterLeave;
				this._headerGripper = null;
			}
		}

		// Token: 0x06006AE4 RID: 27364 RVA: 0x002C32A4 File Offset: 0x002C22A4
		private Cursor GetCursor(int cursorID)
		{
			Invariant.Assert(cursorID == 100 || cursorID == 101, "incorrect cursor type");
			Cursor result = null;
			Stream stream = null;
			Assembly assembly = base.GetType().Assembly;
			if (cursorID == 100)
			{
				stream = assembly.GetManifestResourceStream("split.cur");
			}
			else if (cursorID == 101)
			{
				stream = assembly.GetManifestResourceStream("splitopen.cur");
			}
			if (stream != null)
			{
				result = new Cursor(stream);
			}
			return result;
		}

		// Token: 0x06006AE5 RID: 27365 RVA: 0x002C3308 File Offset: 0x002C2308
		private void UpdateGripperCursor()
		{
			if (this._headerGripper != null && !this._headerGripper.IsDragging)
			{
				Cursor cursor;
				if (DoubleUtil.IsZero(base.ActualWidth))
				{
					cursor = this.SplitOpenCursor;
				}
				else
				{
					cursor = this.SplitCursor;
				}
				if (cursor != null)
				{
					this._headerGripper.Cursor = cursor;
				}
			}
		}

		// Token: 0x06006AE6 RID: 27366 RVA: 0x002C3356 File Offset: 0x002C2356
		private void UpdateColumnHeaderWidth(double width)
		{
			if (this.Column != null)
			{
				this.Column.Width = width;
				return;
			}
			base.Width = width;
		}

		// Token: 0x06006AE7 RID: 27367 RVA: 0x002C3374 File Offset: 0x002C2374
		private bool IsMouseOutside()
		{
			Point position = Mouse.PrimaryDevice.GetPosition(this);
			return position.X < 0.0 || position.X > base.ActualWidth || position.Y < 0.0 || position.Y > base.ActualHeight;
		}

		// Token: 0x06006AE8 RID: 27368 RVA: 0x002C33D4 File Offset: 0x002C23D4
		private void ClickImplement()
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
			base.OnClick();
		}

		// Token: 0x06006AE9 RID: 27369 RVA: 0x002C3400 File Offset: 0x002C2400
		private bool GetFlag(GridViewColumnHeader.Flags flag)
		{
			return (this._flags & flag) == flag;
		}

		// Token: 0x06006AEA RID: 27370 RVA: 0x002C340D File Offset: 0x002C240D
		private void SetFlag(GridViewColumnHeader.Flags flag, bool set)
		{
			if (set)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x06006AEB RID: 27371 RVA: 0x002C3430 File Offset: 0x002C2430
		private void UpdateFloatingHeaderCanvas()
		{
			if (this._floatingHeaderCanvas != null && this.FloatSourceHeader != null)
			{
				Vector offset = VisualTreeHelper.GetOffset(this.FloatSourceHeader);
				VisualBrush visualBrush = new VisualBrush(this.FloatSourceHeader);
				visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
				visualBrush.Viewbox = new Rect(offset.X, offset.Y, this.FloatSourceHeader.ActualWidth, this.FloatSourceHeader.ActualHeight);
				this._floatingHeaderCanvas.Background = visualBrush;
				this.FloatSourceHeader = null;
			}
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x002C34B0 File Offset: 0x002C24B0
		private bool HandleIsMouseOverChanged()
		{
			if (base.ClickMode == ClickMode.Hover)
			{
				if (base.IsMouseOver && (this._headerGripper == null || !this._headerGripper.IsMouseOver))
				{
					base.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.Box(true));
					this.OnClick();
				}
				else
				{
					base.ClearValue(ButtonBase.IsPressedPropertyKey);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06006AED RID: 27373 RVA: 0x002C350A File Offset: 0x002C250A
		private void OnGripperMouseEnterLeave(object sender, MouseEventArgs e)
		{
			this.HandleIsMouseOverChanged();
		}

		// Token: 0x170018B4 RID: 6324
		// (get) Token: 0x06006AEE RID: 27374 RVA: 0x002C3513 File Offset: 0x002C2513
		private Cursor SplitCursor
		{
			get
			{
				if (GridViewColumnHeader._splitCursorCache == null)
				{
					GridViewColumnHeader._splitCursorCache = this.GetCursor(100);
				}
				return GridViewColumnHeader._splitCursorCache;
			}
		}

		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x06006AEF RID: 27375 RVA: 0x002C352E File Offset: 0x002C252E
		private Cursor SplitOpenCursor
		{
			get
			{
				if (GridViewColumnHeader._splitOpenCursorCache == null)
				{
					GridViewColumnHeader._splitOpenCursorCache = this.GetCursor(101);
				}
				return GridViewColumnHeader._splitOpenCursorCache;
			}
		}

		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x06006AF0 RID: 27376 RVA: 0x002C3549 File Offset: 0x002C2549
		// (set) Token: 0x06006AF1 RID: 27377 RVA: 0x002C3556 File Offset: 0x002C2556
		private bool IsAccessKeyOrAutomation
		{
			get
			{
				return this.GetFlag(GridViewColumnHeader.Flags.IsAccessKeyOrAutomation);
			}
			set
			{
				this.SetFlag(GridViewColumnHeader.Flags.IsAccessKeyOrAutomation, value);
			}
		}

		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x06006AF2 RID: 27378 RVA: 0x002C3564 File Offset: 0x002C2564
		private double ColumnActualWidth
		{
			get
			{
				if (this.Column == null)
				{
					return base.ActualWidth;
				}
				return this.Column.ActualWidth;
			}
		}

		// Token: 0x0400356A RID: 13674
		internal static readonly DependencyPropertyKey ColumnPropertyKey = DependencyProperty.RegisterReadOnly("Column", typeof(GridViewColumn), typeof(GridViewColumnHeader), null);

		// Token: 0x0400356B RID: 13675
		public static readonly DependencyProperty ColumnProperty = GridViewColumnHeader.ColumnPropertyKey.DependencyProperty;

		// Token: 0x0400356C RID: 13676
		internal static readonly DependencyPropertyKey RolePropertyKey = DependencyProperty.RegisterReadOnly("Role", typeof(GridViewColumnHeaderRole), typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(GridViewColumnHeaderRole.Normal));

		// Token: 0x0400356D RID: 13677
		public static readonly DependencyProperty RoleProperty = GridViewColumnHeader.RolePropertyKey.DependencyProperty;

		// Token: 0x0400356E RID: 13678
		private static DependencyObjectType _dType;

		// Token: 0x0400356F RID: 13679
		private GridViewColumnHeader _previousHeader;

		// Token: 0x04003570 RID: 13680
		private static Cursor _splitCursorCache = null;

		// Token: 0x04003571 RID: 13681
		private static Cursor _splitOpenCursorCache = null;

		// Token: 0x04003572 RID: 13682
		private GridViewColumnHeader.Flags _flags;

		// Token: 0x04003573 RID: 13683
		private Thumb _headerGripper;

		// Token: 0x04003574 RID: 13684
		private double _originalWidth;

		// Token: 0x04003575 RID: 13685
		private Canvas _floatingHeaderCanvas;

		// Token: 0x04003576 RID: 13686
		private GridViewColumnHeader _srcHeader;

		// Token: 0x04003577 RID: 13687
		private const int c_SPLIT = 100;

		// Token: 0x04003578 RID: 13688
		private const int c_SPLITOPEN = 101;

		// Token: 0x04003579 RID: 13689
		private const string HeaderGripperTemplateName = "PART_HeaderGripper";

		// Token: 0x0400357A RID: 13690
		private const string FloatingHeaderCanvasTemplateName = "PART_FloatingHeaderCanvas";

		// Token: 0x02000BEF RID: 3055
		[Flags]
		private enum Flags
		{
			// Token: 0x04004A7A RID: 19066
			None = 0,
			// Token: 0x04004A7B RID: 19067
			StyleSetByUser = 1,
			// Token: 0x04004A7C RID: 19068
			IgnoreStyle = 2,
			// Token: 0x04004A7D RID: 19069
			ContentTemplateSetByUser = 4,
			// Token: 0x04004A7E RID: 19070
			IgnoreContentTemplate = 8,
			// Token: 0x04004A7F RID: 19071
			ContentTemplateSelectorSetByUser = 16,
			// Token: 0x04004A80 RID: 19072
			IgnoreContentTemplateSelector = 32,
			// Token: 0x04004A81 RID: 19073
			ContextMenuSetByUser = 64,
			// Token: 0x04004A82 RID: 19074
			IgnoreContextMenu = 128,
			// Token: 0x04004A83 RID: 19075
			ToolTipSetByUser = 256,
			// Token: 0x04004A84 RID: 19076
			IgnoreToolTip = 512,
			// Token: 0x04004A85 RID: 19077
			SuppressClickEvent = 1024,
			// Token: 0x04004A86 RID: 19078
			IsInternalGenerated = 2048,
			// Token: 0x04004A87 RID: 19079
			IsAccessKeyOrAutomation = 4096,
			// Token: 0x04004A88 RID: 19080
			ContentStringFormatSetByUser = 8192,
			// Token: 0x04004A89 RID: 19081
			IgnoreContentStringFormat = 16384
		}
	}
}
