using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000850 RID: 2128
	[Localizability(LocalizationCategory.NeverLocalize)]
	[TemplatePart(Name = "PART_Track", Type = typeof(Track))]
	public class ScrollBar : RangeBase
	{
		// Token: 0x1400015A RID: 346
		// (add) Token: 0x06007CE3 RID: 31971 RVA: 0x0031114A File Offset: 0x0031014A
		// (remove) Token: 0x06007CE4 RID: 31972 RVA: 0x00311158 File Offset: 0x00310158
		[Category("Behavior")]
		public event ScrollEventHandler Scroll
		{
			add
			{
				base.AddHandler(ScrollBar.ScrollEvent, value);
			}
			remove
			{
				base.RemoveHandler(ScrollBar.ScrollEvent, value);
			}
		}

		// Token: 0x17001CD7 RID: 7383
		// (get) Token: 0x06007CE5 RID: 31973 RVA: 0x00311166 File Offset: 0x00310166
		// (set) Token: 0x06007CE6 RID: 31974 RVA: 0x00311178 File Offset: 0x00310178
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(ScrollBar.OrientationProperty);
			}
			set
			{
				base.SetValue(ScrollBar.OrientationProperty, value);
			}
		}

		// Token: 0x17001CD8 RID: 7384
		// (get) Token: 0x06007CE7 RID: 31975 RVA: 0x0031118B File Offset: 0x0031018B
		// (set) Token: 0x06007CE8 RID: 31976 RVA: 0x0031119D File Offset: 0x0031019D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double ViewportSize
		{
			get
			{
				return (double)base.GetValue(ScrollBar.ViewportSizeProperty);
			}
			set
			{
				base.SetValue(ScrollBar.ViewportSizeProperty, value);
			}
		}

		// Token: 0x17001CD9 RID: 7385
		// (get) Token: 0x06007CE9 RID: 31977 RVA: 0x003111B0 File Offset: 0x003101B0
		public Track Track
		{
			get
			{
				return this._track;
			}
		}

		// Token: 0x06007CEA RID: 31978 RVA: 0x003111B8 File Offset: 0x003101B8
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ScrollBarAutomationPeer(this);
		}

		// Token: 0x06007CEB RID: 31979 RVA: 0x003111C0 File Offset: 0x003101C0
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			this._thumbOffset = default(Vector);
			if (this.Track != null && this.Track.IsMouseOver && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
			{
				Point position = e.MouseDevice.GetPosition(this.Track);
				double num = this.Track.ValueFromPoint(position);
				if (Shape.IsDoubleFinite(num))
				{
					this.ChangeValue(num, false);
				}
				if (this.Track.Thumb != null && this.Track.Thumb.IsMouseOver)
				{
					Point position2 = e.MouseDevice.GetPosition(this.Track.Thumb);
					this._thumbOffset = position2 - new Point(this.Track.Thumb.ActualWidth * 0.5, this.Track.Thumb.ActualHeight * 0.5);
				}
				else
				{
					e.Handled = true;
				}
			}
			base.OnPreviewMouseLeftButtonDown(e);
		}

		// Token: 0x06007CEC RID: 31980 RVA: 0x003112C0 File Offset: 0x003102C0
		protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
		{
			if (this.Track != null)
			{
				this._latestRightButtonClickPoint = e.MouseDevice.GetPosition(this.Track);
			}
			else
			{
				this._latestRightButtonClickPoint = new Point(-1.0, -1.0);
			}
			base.OnPreviewMouseRightButtonUp(e);
		}

		// Token: 0x17001CDA RID: 7386
		// (get) Token: 0x06007CED RID: 31981 RVA: 0x00311312 File Offset: 0x00310312
		protected override bool IsEnabledCore
		{
			get
			{
				return base.IsEnabledCore && this._canScroll;
			}
		}

		// Token: 0x06007CEE RID: 31982 RVA: 0x00311324 File Offset: 0x00310324
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._track = (base.GetTemplateChild("PART_Track") as Track);
		}

		// Token: 0x06007CEF RID: 31983 RVA: 0x00311344 File Offset: 0x00310344
		private static void OnThumbDragStarted(object sender, DragStartedEventArgs e)
		{
			ScrollBar scrollBar = sender as ScrollBar;
			if (scrollBar == null)
			{
				return;
			}
			scrollBar._hasScrolled = false;
			scrollBar._previousValue = scrollBar.Value;
		}

		// Token: 0x06007CF0 RID: 31984 RVA: 0x00311370 File Offset: 0x00310370
		private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			ScrollBar scrollBar = sender as ScrollBar;
			if (scrollBar == null)
			{
				return;
			}
			scrollBar.UpdateValue(e.HorizontalChange + scrollBar._thumbOffset.X, e.VerticalChange + scrollBar._thumbOffset.Y);
		}

		// Token: 0x06007CF1 RID: 31985 RVA: 0x003113B4 File Offset: 0x003103B4
		private void UpdateValue(double horizontalDragDelta, double verticalDragDelta)
		{
			if (this.Track != null)
			{
				double num = this.Track.ValueFromDistance(horizontalDragDelta, verticalDragDelta);
				if (Shape.IsDoubleFinite(num) && !DoubleUtil.IsZero(num))
				{
					double value = base.Value;
					double num2 = value + num;
					double value2;
					if (this.Orientation == Orientation.Horizontal)
					{
						value2 = Math.Abs(verticalDragDelta);
					}
					else
					{
						value2 = Math.Abs(horizontalDragDelta);
					}
					if (DoubleUtil.GreaterThan(value2, 150.0))
					{
						num2 = this._previousValue;
					}
					if (!DoubleUtil.AreClose(value, num2))
					{
						this._hasScrolled = true;
						this.ChangeValue(num2, true);
						this.RaiseScrollEvent(ScrollEventType.ThumbTrack);
					}
				}
			}
		}

		// Token: 0x06007CF2 RID: 31986 RVA: 0x00311444 File Offset: 0x00310444
		private static void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
		{
			((ScrollBar)sender).OnThumbDragCompleted(e);
		}

		// Token: 0x06007CF3 RID: 31987 RVA: 0x00311452 File Offset: 0x00310452
		private void OnThumbDragCompleted(DragCompletedEventArgs e)
		{
			if (this._hasScrolled)
			{
				this.FinishDrag();
				this.RaiseScrollEvent(ScrollEventType.EndScroll);
			}
		}

		// Token: 0x17001CDB RID: 7387
		// (get) Token: 0x06007CF4 RID: 31988 RVA: 0x0031146C File Offset: 0x0031046C
		private IInputElement CommandTarget
		{
			get
			{
				IInputElement inputElement = base.TemplatedParent as IInputElement;
				if (inputElement == null)
				{
					inputElement = this;
				}
				return inputElement;
			}
		}

		// Token: 0x06007CF5 RID: 31989 RVA: 0x0031148C File Offset: 0x0031048C
		private void FinishDrag()
		{
			double value = base.Value;
			IInputElement commandTarget = this.CommandTarget;
			if (((this.Orientation == Orientation.Horizontal) ? ScrollBar.DeferScrollToHorizontalOffsetCommand : ScrollBar.DeferScrollToVerticalOffsetCommand).CanExecute(value, commandTarget))
			{
				this.ChangeValue(value, false);
			}
		}

		// Token: 0x06007CF6 RID: 31990 RVA: 0x003114D4 File Offset: 0x003104D4
		private void ChangeValue(double newValue, bool defer)
		{
			newValue = Math.Min(Math.Max(newValue, base.Minimum), base.Maximum);
			if (this.IsStandalone)
			{
				base.Value = newValue;
				return;
			}
			IInputElement commandTarget = this.CommandTarget;
			RoutedCommand routedCommand = null;
			bool flag = this.Orientation == Orientation.Horizontal;
			if (defer)
			{
				routedCommand = (flag ? ScrollBar.DeferScrollToHorizontalOffsetCommand : ScrollBar.DeferScrollToVerticalOffsetCommand);
				if (routedCommand.CanExecute(newValue, commandTarget))
				{
					routedCommand.Execute(newValue, commandTarget);
				}
				else
				{
					routedCommand = null;
				}
			}
			if (routedCommand == null)
			{
				routedCommand = (flag ? ScrollBar.ScrollToHorizontalOffsetCommand : ScrollBar.ScrollToVerticalOffsetCommand);
				if (routedCommand.CanExecute(newValue, commandTarget))
				{
					routedCommand.Execute(newValue, commandTarget);
				}
			}
		}

		// Token: 0x06007CF7 RID: 31991 RVA: 0x00311580 File Offset: 0x00310580
		internal void ScrollToLastMousePoint()
		{
			Point point = new Point(-1.0, -1.0);
			if (this.Track != null && this._latestRightButtonClickPoint != point)
			{
				double num = this.Track.ValueFromPoint(this._latestRightButtonClickPoint);
				if (Shape.IsDoubleFinite(num))
				{
					this.ChangeValue(num, false);
					this._latestRightButtonClickPoint = point;
					this.RaiseScrollEvent(ScrollEventType.ThumbPosition);
				}
			}
		}

		// Token: 0x06007CF8 RID: 31992 RVA: 0x003115F4 File Offset: 0x003105F4
		internal void RaiseScrollEvent(ScrollEventType scrollEventType)
		{
			base.RaiseEvent(new ScrollEventArgs(scrollEventType, base.Value)
			{
				Source = this
			});
		}

		// Token: 0x06007CF9 RID: 31993 RVA: 0x0031161C File Offset: 0x0031061C
		private static void OnScrollCommand(object target, ExecutedRoutedEventArgs args)
		{
			ScrollBar scrollBar = (ScrollBar)target;
			if (args.Command == ScrollBar.ScrollHereCommand)
			{
				scrollBar.ScrollToLastMousePoint();
			}
			if (scrollBar.IsStandalone)
			{
				if (scrollBar.Orientation == Orientation.Vertical)
				{
					if (args.Command == ScrollBar.LineUpCommand)
					{
						scrollBar.LineUp();
						return;
					}
					if (args.Command == ScrollBar.LineDownCommand)
					{
						scrollBar.LineDown();
						return;
					}
					if (args.Command == ScrollBar.PageUpCommand)
					{
						scrollBar.PageUp();
						return;
					}
					if (args.Command == ScrollBar.PageDownCommand)
					{
						scrollBar.PageDown();
						return;
					}
					if (args.Command == ScrollBar.ScrollToTopCommand)
					{
						scrollBar.ScrollToTop();
						return;
					}
					if (args.Command == ScrollBar.ScrollToBottomCommand)
					{
						scrollBar.ScrollToBottom();
						return;
					}
				}
				else
				{
					if (args.Command == ScrollBar.LineLeftCommand)
					{
						scrollBar.LineLeft();
						return;
					}
					if (args.Command == ScrollBar.LineRightCommand)
					{
						scrollBar.LineRight();
						return;
					}
					if (args.Command == ScrollBar.PageLeftCommand)
					{
						scrollBar.PageLeft();
						return;
					}
					if (args.Command == ScrollBar.PageRightCommand)
					{
						scrollBar.PageRight();
						return;
					}
					if (args.Command == ScrollBar.ScrollToLeftEndCommand)
					{
						scrollBar.ScrollToLeftEnd();
						return;
					}
					if (args.Command == ScrollBar.ScrollToRightEndCommand)
					{
						scrollBar.ScrollToRightEnd();
					}
				}
			}
		}

		// Token: 0x06007CFA RID: 31994 RVA: 0x00311748 File Offset: 0x00310748
		private void SmallDecrement()
		{
			double num = Math.Max(base.Value - base.SmallChange, base.Minimum);
			if (base.Value != num)
			{
				base.Value = num;
				this.RaiseScrollEvent(ScrollEventType.SmallDecrement);
			}
		}

		// Token: 0x06007CFB RID: 31995 RVA: 0x00311788 File Offset: 0x00310788
		private void SmallIncrement()
		{
			double num = Math.Min(base.Value + base.SmallChange, base.Maximum);
			if (base.Value != num)
			{
				base.Value = num;
				this.RaiseScrollEvent(ScrollEventType.SmallIncrement);
			}
		}

		// Token: 0x06007CFC RID: 31996 RVA: 0x003117C8 File Offset: 0x003107C8
		private void LargeDecrement()
		{
			double num = Math.Max(base.Value - base.LargeChange, base.Minimum);
			if (base.Value != num)
			{
				base.Value = num;
				this.RaiseScrollEvent(ScrollEventType.LargeDecrement);
			}
		}

		// Token: 0x06007CFD RID: 31997 RVA: 0x00311808 File Offset: 0x00310808
		private void LargeIncrement()
		{
			double num = Math.Min(base.Value + base.LargeChange, base.Maximum);
			if (base.Value != num)
			{
				base.Value = num;
				this.RaiseScrollEvent(ScrollEventType.LargeIncrement);
			}
		}

		// Token: 0x06007CFE RID: 31998 RVA: 0x00311845 File Offset: 0x00310845
		private void ToMinimum()
		{
			if (base.Value != base.Minimum)
			{
				base.Value = base.Minimum;
				this.RaiseScrollEvent(ScrollEventType.First);
			}
		}

		// Token: 0x06007CFF RID: 31999 RVA: 0x00311868 File Offset: 0x00310868
		private void ToMaximum()
		{
			if (base.Value != base.Maximum)
			{
				base.Value = base.Maximum;
				this.RaiseScrollEvent(ScrollEventType.Last);
			}
		}

		// Token: 0x06007D00 RID: 32000 RVA: 0x0031188B File Offset: 0x0031088B
		private void LineUp()
		{
			this.SmallDecrement();
		}

		// Token: 0x06007D01 RID: 32001 RVA: 0x00311893 File Offset: 0x00310893
		private void LineDown()
		{
			this.SmallIncrement();
		}

		// Token: 0x06007D02 RID: 32002 RVA: 0x0031189B File Offset: 0x0031089B
		private void PageUp()
		{
			this.LargeDecrement();
		}

		// Token: 0x06007D03 RID: 32003 RVA: 0x003118A3 File Offset: 0x003108A3
		private void PageDown()
		{
			this.LargeIncrement();
		}

		// Token: 0x06007D04 RID: 32004 RVA: 0x003118AB File Offset: 0x003108AB
		private void ScrollToTop()
		{
			this.ToMinimum();
		}

		// Token: 0x06007D05 RID: 32005 RVA: 0x003118B3 File Offset: 0x003108B3
		private void ScrollToBottom()
		{
			this.ToMaximum();
		}

		// Token: 0x06007D06 RID: 32006 RVA: 0x0031188B File Offset: 0x0031088B
		private void LineLeft()
		{
			this.SmallDecrement();
		}

		// Token: 0x06007D07 RID: 32007 RVA: 0x00311893 File Offset: 0x00310893
		private void LineRight()
		{
			this.SmallIncrement();
		}

		// Token: 0x06007D08 RID: 32008 RVA: 0x0031189B File Offset: 0x0031089B
		private void PageLeft()
		{
			this.LargeDecrement();
		}

		// Token: 0x06007D09 RID: 32009 RVA: 0x003118A3 File Offset: 0x003108A3
		private void PageRight()
		{
			this.LargeIncrement();
		}

		// Token: 0x06007D0A RID: 32010 RVA: 0x003118AB File Offset: 0x003108AB
		private void ScrollToLeftEnd()
		{
			this.ToMinimum();
		}

		// Token: 0x06007D0B RID: 32011 RVA: 0x003118B3 File Offset: 0x003108B3
		private void ScrollToRightEnd()
		{
			this.ToMaximum();
		}

		// Token: 0x06007D0C RID: 32012 RVA: 0x003118BB File Offset: 0x003108BB
		private static void OnQueryScrollHereCommand(object target, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = (args.Command == ScrollBar.ScrollHereCommand);
		}

		// Token: 0x06007D0D RID: 32013 RVA: 0x003118D0 File Offset: 0x003108D0
		private static void OnQueryScrollCommand(object target, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = ((ScrollBar)target).IsStandalone;
		}

		// Token: 0x06007D0E RID: 32014 RVA: 0x003118E4 File Offset: 0x003108E4
		static ScrollBar()
		{
			ScrollBar.ScrollEvent = EventManager.RegisterRoutedEvent("Scroll", RoutingStrategy.Bubble, typeof(ScrollEventHandler), typeof(ScrollBar));
			ScrollBar.OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ScrollBar), new FrameworkPropertyMetadata(Orientation.Vertical), new ValidateValueCallback(ScrollBar.IsValidOrientation));
			ScrollBar.ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(double), typeof(ScrollBar), new FrameworkPropertyMetadata(0.0), new ValidateValueCallback(Shape.IsDoubleFiniteNonNegative));
			ScrollBar.LineUpCommand = new RoutedCommand("LineUp", typeof(ScrollBar));
			ScrollBar.LineDownCommand = new RoutedCommand("LineDown", typeof(ScrollBar));
			ScrollBar.LineLeftCommand = new RoutedCommand("LineLeft", typeof(ScrollBar));
			ScrollBar.LineRightCommand = new RoutedCommand("LineRight", typeof(ScrollBar));
			ScrollBar.PageUpCommand = new RoutedCommand("PageUp", typeof(ScrollBar));
			ScrollBar.PageDownCommand = new RoutedCommand("PageDown", typeof(ScrollBar));
			ScrollBar.PageLeftCommand = new RoutedCommand("PageLeft", typeof(ScrollBar));
			ScrollBar.PageRightCommand = new RoutedCommand("PageRight", typeof(ScrollBar));
			ScrollBar.ScrollToEndCommand = new RoutedCommand("ScrollToEnd", typeof(ScrollBar));
			ScrollBar.ScrollToHomeCommand = new RoutedCommand("ScrollToHome", typeof(ScrollBar));
			ScrollBar.ScrollToRightEndCommand = new RoutedCommand("ScrollToRightEnd", typeof(ScrollBar));
			ScrollBar.ScrollToLeftEndCommand = new RoutedCommand("ScrollToLeftEnd", typeof(ScrollBar));
			ScrollBar.ScrollToTopCommand = new RoutedCommand("ScrollToTop", typeof(ScrollBar));
			ScrollBar.ScrollToBottomCommand = new RoutedCommand("ScrollToBottom", typeof(ScrollBar));
			ScrollBar.ScrollToHorizontalOffsetCommand = new RoutedCommand("ScrollToHorizontalOffset", typeof(ScrollBar));
			ScrollBar.ScrollToVerticalOffsetCommand = new RoutedCommand("ScrollToVerticalOffset", typeof(ScrollBar));
			ScrollBar.DeferScrollToHorizontalOffsetCommand = new RoutedCommand("DeferScrollToToHorizontalOffset", typeof(ScrollBar));
			ScrollBar.DeferScrollToVerticalOffsetCommand = new RoutedCommand("DeferScrollToVerticalOffset", typeof(ScrollBar));
			ScrollBar.ScrollHereCommand = new RoutedCommand("ScrollHere", typeof(ScrollBar));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(typeof(ScrollBar)));
			ScrollBar._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ScrollBar));
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(ScrollBar.OnScrollCommand);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(ScrollBar.OnQueryScrollCommand);
			UIElement.FocusableProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			EventManager.RegisterClassHandler(typeof(ScrollBar), Thumb.DragStartedEvent, new DragStartedEventHandler(ScrollBar.OnThumbDragStarted));
			EventManager.RegisterClassHandler(typeof(ScrollBar), Thumb.DragDeltaEvent, new DragDeltaEventHandler(ScrollBar.OnThumbDragDelta));
			EventManager.RegisterClassHandler(typeof(ScrollBar), Thumb.DragCompletedEvent, new DragCompletedEventHandler(ScrollBar.OnThumbDragCompleted));
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollHereCommand, executedRoutedEventHandler, new CanExecuteRoutedEventHandler(ScrollBar.OnQueryScrollHereCommand));
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.LineUpCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Up);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.LineDownCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Down);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.PageUpCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Prior);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.PageDownCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Next);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollToTopCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Home, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollToBottomCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.End, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.LineLeftCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Left);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.LineRightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Right);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.PageLeftCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.PageRightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollToLeftEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Home);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollToRightEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.End);
			RangeBase.MaximumProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(ScrollBar.ViewChanged)));
			RangeBase.MinimumProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(ScrollBar.ViewChanged)));
			FrameworkElement.ContextMenuProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ScrollBar.CoerceContextMenu)));
			ControlsTraceLogger.AddControl(TelemetryControls.ScrollBar);
		}

		// Token: 0x06007D0F RID: 32015 RVA: 0x00311E00 File Offset: 0x00310E00
		private static void ViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollBar scrollBar = (ScrollBar)d;
			bool flag = scrollBar.Maximum > scrollBar.Minimum;
			if (flag != scrollBar._canScroll)
			{
				scrollBar._canScroll = flag;
				scrollBar.CoerceValue(UIElement.IsEnabledProperty);
			}
		}

		// Token: 0x06007D10 RID: 32016 RVA: 0x002DBFC8 File Offset: 0x002DAFC8
		internal static bool IsValidOrientation(object o)
		{
			Orientation orientation = (Orientation)o;
			return orientation == Orientation.Horizontal || orientation == Orientation.Vertical;
		}

		// Token: 0x17001CDC RID: 7388
		// (get) Token: 0x06007D11 RID: 32017 RVA: 0x00311E3E File Offset: 0x00310E3E
		// (set) Token: 0x06007D12 RID: 32018 RVA: 0x00311E46 File Offset: 0x00310E46
		internal bool IsStandalone
		{
			get
			{
				return this._isStandalone;
			}
			set
			{
				this._isStandalone = value;
			}
		}

		// Token: 0x17001CDD RID: 7389
		// (get) Token: 0x06007D13 RID: 32019 RVA: 0x00311E4F File Offset: 0x00310E4F
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ScrollBar._dType;
			}
		}

		// Token: 0x06007D14 RID: 32020 RVA: 0x00311E58 File Offset: 0x00310E58
		private static object CoerceContextMenu(DependencyObject o, object value)
		{
			ScrollBar scrollBar = (ScrollBar)o;
			bool flag;
			if (!scrollBar._openingContextMenu || scrollBar.GetValueSource(FrameworkElement.ContextMenuProperty, null, out flag) != BaseValueSourceInternal.Default || flag)
			{
				return value;
			}
			if (scrollBar.Orientation == Orientation.Vertical)
			{
				return ScrollBar.VerticalContextMenu;
			}
			if (scrollBar.FlowDirection == FlowDirection.LeftToRight)
			{
				return ScrollBar.HorizontalContextMenuLTR;
			}
			return ScrollBar.HorizontalContextMenuRTL;
		}

		// Token: 0x06007D15 RID: 32021 RVA: 0x00311EAC File Offset: 0x00310EAC
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			if (!e.Handled)
			{
				this._openingContextMenu = true;
				base.CoerceValue(FrameworkElement.ContextMenuProperty);
			}
		}

		// Token: 0x06007D16 RID: 32022 RVA: 0x00311ECF File Offset: 0x00310ECF
		protected override void OnContextMenuClosing(ContextMenuEventArgs e)
		{
			base.OnContextMenuClosing(e);
			this._openingContextMenu = false;
			base.CoerceValue(FrameworkElement.ContextMenuProperty);
		}

		// Token: 0x17001CDE RID: 7390
		// (get) Token: 0x06007D17 RID: 32023 RVA: 0x00311EEC File Offset: 0x00310EEC
		private static ContextMenu VerticalContextMenu
		{
			get
			{
				return new ContextMenu
				{
					Items = 
					{
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollHere", "ScrollHere", ScrollBar.ScrollHereCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_Top", "Top", ScrollBar.ScrollToTopCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_Bottom", "Bottom", ScrollBar.ScrollToBottomCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageUp", "PageUp", ScrollBar.PageUpCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageDown", "PageDown", ScrollBar.PageDownCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollUp", "ScrollUp", ScrollBar.LineUpCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollDown", "ScrollDown", ScrollBar.LineDownCommand)
					}
				};
			}
		}

		// Token: 0x17001CDF RID: 7391
		// (get) Token: 0x06007D18 RID: 32024 RVA: 0x00312014 File Offset: 0x00311014
		private static ContextMenu HorizontalContextMenuLTR
		{
			get
			{
				return new ContextMenu
				{
					Items = 
					{
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollHere", "ScrollHere", ScrollBar.ScrollHereCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_LeftEdge", "LeftEdge", ScrollBar.ScrollToLeftEndCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_RightEdge", "RightEdge", ScrollBar.ScrollToRightEndCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageLeft", "PageLeft", ScrollBar.PageLeftCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageRight", "PageRight", ScrollBar.PageRightCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollLeft", "ScrollLeft", ScrollBar.LineLeftCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollRight", "ScrollRight", ScrollBar.LineRightCommand)
					}
				};
			}
		}

		// Token: 0x17001CE0 RID: 7392
		// (get) Token: 0x06007D19 RID: 32025 RVA: 0x0031213C File Offset: 0x0031113C
		private static ContextMenu HorizontalContextMenuRTL
		{
			get
			{
				return new ContextMenu
				{
					Items = 
					{
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollHere", "ScrollHere", ScrollBar.ScrollHereCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_LeftEdge", "LeftEdge", ScrollBar.ScrollToRightEndCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_RightEdge", "RightEdge", ScrollBar.ScrollToLeftEndCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageLeft", "PageLeft", ScrollBar.PageRightCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageRight", "PageRight", ScrollBar.PageLeftCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollLeft", "ScrollLeft", ScrollBar.LineRightCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollRight", "ScrollRight", ScrollBar.LineLeftCommand)
					}
				};
			}
		}

		// Token: 0x06007D1A RID: 32026 RVA: 0x00312264 File Offset: 0x00311264
		private static MenuItem CreateMenuItem(string name, string automationId, RoutedCommand command)
		{
			MenuItem menuItem = new MenuItem();
			menuItem.Header = SR.Get(name);
			menuItem.Command = command;
			AutomationProperties.SetAutomationId(menuItem, automationId);
			Binding binding = new Binding();
			binding.Path = new PropertyPath(ContextMenu.PlacementTargetProperty);
			binding.Mode = BindingMode.OneWay;
			binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ContextMenu), 1);
			menuItem.SetBinding(MenuItem.CommandTargetProperty, binding);
			return menuItem;
		}

		// Token: 0x17001CE1 RID: 7393
		// (get) Token: 0x06007D1B RID: 32027 RVA: 0x001FCA42 File Offset: 0x001FBA42
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x04003AAC RID: 15020
		public static readonly DependencyProperty OrientationProperty;

		// Token: 0x04003AAD RID: 15021
		public static readonly DependencyProperty ViewportSizeProperty;

		// Token: 0x04003AAE RID: 15022
		public static readonly RoutedCommand LineUpCommand;

		// Token: 0x04003AAF RID: 15023
		public static readonly RoutedCommand LineDownCommand;

		// Token: 0x04003AB0 RID: 15024
		public static readonly RoutedCommand LineLeftCommand;

		// Token: 0x04003AB1 RID: 15025
		public static readonly RoutedCommand LineRightCommand;

		// Token: 0x04003AB2 RID: 15026
		public static readonly RoutedCommand PageUpCommand;

		// Token: 0x04003AB3 RID: 15027
		public static readonly RoutedCommand PageDownCommand;

		// Token: 0x04003AB4 RID: 15028
		public static readonly RoutedCommand PageLeftCommand;

		// Token: 0x04003AB5 RID: 15029
		public static readonly RoutedCommand PageRightCommand;

		// Token: 0x04003AB6 RID: 15030
		public static readonly RoutedCommand ScrollToEndCommand;

		// Token: 0x04003AB7 RID: 15031
		public static readonly RoutedCommand ScrollToHomeCommand;

		// Token: 0x04003AB8 RID: 15032
		public static readonly RoutedCommand ScrollToRightEndCommand;

		// Token: 0x04003AB9 RID: 15033
		public static readonly RoutedCommand ScrollToLeftEndCommand;

		// Token: 0x04003ABA RID: 15034
		public static readonly RoutedCommand ScrollToTopCommand;

		// Token: 0x04003ABB RID: 15035
		public static readonly RoutedCommand ScrollToBottomCommand;

		// Token: 0x04003ABC RID: 15036
		public static readonly RoutedCommand ScrollToHorizontalOffsetCommand;

		// Token: 0x04003ABD RID: 15037
		public static readonly RoutedCommand ScrollToVerticalOffsetCommand;

		// Token: 0x04003ABE RID: 15038
		public static readonly RoutedCommand DeferScrollToHorizontalOffsetCommand;

		// Token: 0x04003ABF RID: 15039
		public static readonly RoutedCommand DeferScrollToVerticalOffsetCommand;

		// Token: 0x04003AC0 RID: 15040
		public static readonly RoutedCommand ScrollHereCommand;

		// Token: 0x04003AC1 RID: 15041
		private const double MaxPerpendicularDelta = 150.0;

		// Token: 0x04003AC2 RID: 15042
		private const string TrackName = "PART_Track";

		// Token: 0x04003AC3 RID: 15043
		private Track _track;

		// Token: 0x04003AC4 RID: 15044
		private Point _latestRightButtonClickPoint = new Point(-1.0, -1.0);

		// Token: 0x04003AC5 RID: 15045
		private bool _canScroll = true;

		// Token: 0x04003AC6 RID: 15046
		private bool _hasScrolled;

		// Token: 0x04003AC7 RID: 15047
		private bool _isStandalone = true;

		// Token: 0x04003AC8 RID: 15048
		private bool _openingContextMenu;

		// Token: 0x04003AC9 RID: 15049
		private double _previousValue;

		// Token: 0x04003ACA RID: 15050
		private Vector _thumbOffset;

		// Token: 0x04003ACB RID: 15051
		private static DependencyObjectType _dType;
	}
}
