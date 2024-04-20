using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007D8 RID: 2008
	[Localizability(LocalizationCategory.Ignore)]
	[DefaultEvent("ValueChanged")]
	[DefaultProperty("Value")]
	[TemplatePart(Name = "PART_Track", Type = typeof(Track))]
	[TemplatePart(Name = "PART_SelectionRange", Type = typeof(FrameworkElement))]
	public class Slider : RangeBase
	{
		// Token: 0x0600734B RID: 29515 RVA: 0x002E27A8 File Offset: 0x002E17A8
		static Slider()
		{
			Slider.InitializeCommands();
			RangeBase.MinimumProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
			RangeBase.MaximumProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
			RangeBase.ValueProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
			EventManager.RegisterClassHandler(typeof(Slider), Thumb.DragStartedEvent, new DragStartedEventHandler(Slider.OnThumbDragStarted));
			EventManager.RegisterClassHandler(typeof(Slider), Thumb.DragDeltaEvent, new DragDeltaEventHandler(Slider.OnThumbDragDelta));
			EventManager.RegisterClassHandler(typeof(Slider), Thumb.DragCompletedEvent, new DragCompletedEventHandler(Slider.OnThumbDragCompleted));
			EventManager.RegisterClassHandler(typeof(Slider), Mouse.MouseDownEvent, new MouseButtonEventHandler(Slider._OnMouseLeftButtonDown), true);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(typeof(Slider)));
			Slider._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Slider));
			ControlsTraceLogger.AddControl(TelemetryControls.Slider);
		}

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x0600734C RID: 29516 RVA: 0x002E2C3B File Offset: 0x002E1C3B
		public static RoutedCommand IncreaseLarge
		{
			get
			{
				return Slider._increaseLargeCommand;
			}
		}

		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x0600734D RID: 29517 RVA: 0x002E2C42 File Offset: 0x002E1C42
		public static RoutedCommand DecreaseLarge
		{
			get
			{
				return Slider._decreaseLargeCommand;
			}
		}

		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x0600734E RID: 29518 RVA: 0x002E2C49 File Offset: 0x002E1C49
		public static RoutedCommand IncreaseSmall
		{
			get
			{
				return Slider._increaseSmallCommand;
			}
		}

		// Token: 0x17001AB9 RID: 6841
		// (get) Token: 0x0600734F RID: 29519 RVA: 0x002E2C50 File Offset: 0x002E1C50
		public static RoutedCommand DecreaseSmall
		{
			get
			{
				return Slider._decreaseSmallCommand;
			}
		}

		// Token: 0x17001ABA RID: 6842
		// (get) Token: 0x06007350 RID: 29520 RVA: 0x002E2C57 File Offset: 0x002E1C57
		public static RoutedCommand MinimizeValue
		{
			get
			{
				return Slider._minimizeValueCommand;
			}
		}

		// Token: 0x17001ABB RID: 6843
		// (get) Token: 0x06007351 RID: 29521 RVA: 0x002E2C5E File Offset: 0x002E1C5E
		public static RoutedCommand MaximizeValue
		{
			get
			{
				return Slider._maximizeValueCommand;
			}
		}

		// Token: 0x06007352 RID: 29522 RVA: 0x002E2C68 File Offset: 0x002E1C68
		private static void InitializeCommands()
		{
			Slider._increaseLargeCommand = new RoutedCommand("IncreaseLarge", typeof(Slider));
			Slider._decreaseLargeCommand = new RoutedCommand("DecreaseLarge", typeof(Slider));
			Slider._increaseSmallCommand = new RoutedCommand("IncreaseSmall", typeof(Slider));
			Slider._decreaseSmallCommand = new RoutedCommand("DecreaseSmall", typeof(Slider));
			Slider._minimizeValueCommand = new RoutedCommand("MinimizeValue", typeof(Slider));
			Slider._maximizeValueCommand = new RoutedCommand("MaximizeValue", typeof(Slider));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._increaseLargeCommand, new ExecutedRoutedEventHandler(Slider.OnIncreaseLargeCommand), new Slider.SliderGesture(Key.Prior, Key.Next, false));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._decreaseLargeCommand, new ExecutedRoutedEventHandler(Slider.OnDecreaseLargeCommand), new Slider.SliderGesture(Key.Next, Key.Prior, false));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._increaseSmallCommand, new ExecutedRoutedEventHandler(Slider.OnIncreaseSmallCommand), new Slider.SliderGesture(Key.Up, Key.Down, false), new Slider.SliderGesture(Key.Right, Key.Left, true));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._decreaseSmallCommand, new ExecutedRoutedEventHandler(Slider.OnDecreaseSmallCommand), new Slider.SliderGesture(Key.Down, Key.Up, false), new Slider.SliderGesture(Key.Left, Key.Right, true));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._minimizeValueCommand, new ExecutedRoutedEventHandler(Slider.OnMinimizeValueCommand), Key.Home);
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._maximizeValueCommand, new ExecutedRoutedEventHandler(Slider.OnMaximizeValueCommand), Key.End);
		}

		// Token: 0x06007353 RID: 29523 RVA: 0x002E2E0C File Offset: 0x002E1E0C
		private static void OnIncreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnIncreaseSmall();
			}
		}

		// Token: 0x06007354 RID: 29524 RVA: 0x002E2E2C File Offset: 0x002E1E2C
		private static void OnDecreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnDecreaseSmall();
			}
		}

		// Token: 0x06007355 RID: 29525 RVA: 0x002E2E4C File Offset: 0x002E1E4C
		private static void OnMaximizeValueCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnMaximizeValue();
			}
		}

		// Token: 0x06007356 RID: 29526 RVA: 0x002E2E6C File Offset: 0x002E1E6C
		private static void OnMinimizeValueCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnMinimizeValue();
			}
		}

		// Token: 0x06007357 RID: 29527 RVA: 0x002E2E8C File Offset: 0x002E1E8C
		private static void OnIncreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnIncreaseLarge();
			}
		}

		// Token: 0x06007358 RID: 29528 RVA: 0x002E2EAC File Offset: 0x002E1EAC
		private static void OnDecreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnDecreaseLarge();
			}
		}

		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x06007359 RID: 29529 RVA: 0x002E2EC9 File Offset: 0x002E1EC9
		// (set) Token: 0x0600735A RID: 29530 RVA: 0x002E2EDB File Offset: 0x002E1EDB
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(Slider.OrientationProperty);
			}
			set
			{
				base.SetValue(Slider.OrientationProperty, value);
			}
		}

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x0600735B RID: 29531 RVA: 0x002E2EEE File Offset: 0x002E1EEE
		// (set) Token: 0x0600735C RID: 29532 RVA: 0x002E2F00 File Offset: 0x002E1F00
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsDirectionReversed
		{
			get
			{
				return (bool)base.GetValue(Slider.IsDirectionReversedProperty);
			}
			set
			{
				base.SetValue(Slider.IsDirectionReversedProperty, value);
			}
		}

		// Token: 0x17001ABE RID: 6846
		// (get) Token: 0x0600735D RID: 29533 RVA: 0x002E2F0E File Offset: 0x002E1F0E
		// (set) Token: 0x0600735E RID: 29534 RVA: 0x002E2F20 File Offset: 0x002E1F20
		[Bindable(true)]
		[Category("Behavior")]
		public int Delay
		{
			get
			{
				return (int)base.GetValue(Slider.DelayProperty);
			}
			set
			{
				base.SetValue(Slider.DelayProperty, value);
			}
		}

		// Token: 0x17001ABF RID: 6847
		// (get) Token: 0x0600735F RID: 29535 RVA: 0x002E2F33 File Offset: 0x002E1F33
		// (set) Token: 0x06007360 RID: 29536 RVA: 0x002E2F45 File Offset: 0x002E1F45
		[Bindable(true)]
		[Category("Behavior")]
		public int Interval
		{
			get
			{
				return (int)base.GetValue(Slider.IntervalProperty);
			}
			set
			{
				base.SetValue(Slider.IntervalProperty, value);
			}
		}

		// Token: 0x17001AC0 RID: 6848
		// (get) Token: 0x06007361 RID: 29537 RVA: 0x002E2F58 File Offset: 0x002E1F58
		// (set) Token: 0x06007362 RID: 29538 RVA: 0x002E2F6A File Offset: 0x002E1F6A
		[Bindable(true)]
		[Category("Behavior")]
		public AutoToolTipPlacement AutoToolTipPlacement
		{
			get
			{
				return (AutoToolTipPlacement)base.GetValue(Slider.AutoToolTipPlacementProperty);
			}
			set
			{
				base.SetValue(Slider.AutoToolTipPlacementProperty, value);
			}
		}

		// Token: 0x06007363 RID: 29539 RVA: 0x002E2F80 File Offset: 0x002E1F80
		private static bool IsValidAutoToolTipPlacement(object o)
		{
			AutoToolTipPlacement autoToolTipPlacement = (AutoToolTipPlacement)o;
			return autoToolTipPlacement == AutoToolTipPlacement.None || autoToolTipPlacement == AutoToolTipPlacement.TopLeft || autoToolTipPlacement == AutoToolTipPlacement.BottomRight;
		}

		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x06007364 RID: 29540 RVA: 0x002E2FA1 File Offset: 0x002E1FA1
		// (set) Token: 0x06007365 RID: 29541 RVA: 0x002E2FB3 File Offset: 0x002E1FB3
		[Bindable(true)]
		[Category("Appearance")]
		public int AutoToolTipPrecision
		{
			get
			{
				return (int)base.GetValue(Slider.AutoToolTipPrecisionProperty);
			}
			set
			{
				base.SetValue(Slider.AutoToolTipPrecisionProperty, value);
			}
		}

		// Token: 0x06007366 RID: 29542 RVA: 0x002A6F55 File Offset: 0x002A5F55
		private static bool IsValidAutoToolTipPrecision(object o)
		{
			return (int)o >= 0;
		}

		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x06007367 RID: 29543 RVA: 0x002E2FC6 File Offset: 0x002E1FC6
		// (set) Token: 0x06007368 RID: 29544 RVA: 0x002E2FD8 File Offset: 0x002E1FD8
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsSnapToTickEnabled
		{
			get
			{
				return (bool)base.GetValue(Slider.IsSnapToTickEnabledProperty);
			}
			set
			{
				base.SetValue(Slider.IsSnapToTickEnabledProperty, value);
			}
		}

		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x06007369 RID: 29545 RVA: 0x002E2FE6 File Offset: 0x002E1FE6
		// (set) Token: 0x0600736A RID: 29546 RVA: 0x002E2FF8 File Offset: 0x002E1FF8
		[Bindable(true)]
		[Category("Appearance")]
		public TickPlacement TickPlacement
		{
			get
			{
				return (TickPlacement)base.GetValue(Slider.TickPlacementProperty);
			}
			set
			{
				base.SetValue(Slider.TickPlacementProperty, value);
			}
		}

		// Token: 0x0600736B RID: 29547 RVA: 0x002E300C File Offset: 0x002E200C
		private static bool IsValidTickPlacement(object o)
		{
			TickPlacement tickPlacement = (TickPlacement)o;
			return tickPlacement == TickPlacement.None || tickPlacement == TickPlacement.TopLeft || tickPlacement == TickPlacement.BottomRight || tickPlacement == TickPlacement.Both;
		}

		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x0600736C RID: 29548 RVA: 0x002E3031 File Offset: 0x002E2031
		// (set) Token: 0x0600736D RID: 29549 RVA: 0x002E3043 File Offset: 0x002E2043
		[Bindable(true)]
		[Category("Appearance")]
		public double TickFrequency
		{
			get
			{
				return (double)base.GetValue(Slider.TickFrequencyProperty);
			}
			set
			{
				base.SetValue(Slider.TickFrequencyProperty, value);
			}
		}

		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x0600736E RID: 29550 RVA: 0x002E3056 File Offset: 0x002E2056
		// (set) Token: 0x0600736F RID: 29551 RVA: 0x002E3068 File Offset: 0x002E2068
		[Bindable(true)]
		[Category("Appearance")]
		public DoubleCollection Ticks
		{
			get
			{
				return (DoubleCollection)base.GetValue(Slider.TicksProperty);
			}
			set
			{
				base.SetValue(Slider.TicksProperty, value);
			}
		}

		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x06007370 RID: 29552 RVA: 0x002E3076 File Offset: 0x002E2076
		// (set) Token: 0x06007371 RID: 29553 RVA: 0x002E3088 File Offset: 0x002E2088
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelectionRangeEnabled
		{
			get
			{
				return (bool)base.GetValue(Slider.IsSelectionRangeEnabledProperty);
			}
			set
			{
				base.SetValue(Slider.IsSelectionRangeEnabledProperty, value);
			}
		}

		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x06007372 RID: 29554 RVA: 0x002E3096 File Offset: 0x002E2096
		// (set) Token: 0x06007373 RID: 29555 RVA: 0x002E30A8 File Offset: 0x002E20A8
		[Bindable(true)]
		[Category("Appearance")]
		public double SelectionStart
		{
			get
			{
				return (double)base.GetValue(Slider.SelectionStartProperty);
			}
			set
			{
				base.SetValue(Slider.SelectionStartProperty, value);
			}
		}

		// Token: 0x06007374 RID: 29556 RVA: 0x002E30BB File Offset: 0x002E20BB
		private static void OnSelectionStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Slider slider = (Slider)d;
			double num = (double)e.OldValue;
			double num2 = (double)e.NewValue;
			slider.CoerceValue(Slider.SelectionEndProperty);
			slider.UpdateSelectionRangeElementPositionAndSize();
		}

		// Token: 0x06007375 RID: 29557 RVA: 0x002E30F0 File Offset: 0x002E20F0
		private static object CoerceSelectionStart(DependencyObject d, object value)
		{
			Slider slider = (Slider)d;
			double num = (double)value;
			double minimum = slider.Minimum;
			double maximum = slider.Maximum;
			if (num < minimum)
			{
				return minimum;
			}
			if (num > maximum)
			{
				return maximum;
			}
			return value;
		}

		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x06007376 RID: 29558 RVA: 0x002E312E File Offset: 0x002E212E
		// (set) Token: 0x06007377 RID: 29559 RVA: 0x002E3140 File Offset: 0x002E2140
		[Bindable(true)]
		[Category("Appearance")]
		public double SelectionEnd
		{
			get
			{
				return (double)base.GetValue(Slider.SelectionEndProperty);
			}
			set
			{
				base.SetValue(Slider.SelectionEndProperty, value);
			}
		}

		// Token: 0x06007378 RID: 29560 RVA: 0x002E3153 File Offset: 0x002E2153
		private static void OnSelectionEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Slider)d).UpdateSelectionRangeElementPositionAndSize();
		}

		// Token: 0x06007379 RID: 29561 RVA: 0x002E3160 File Offset: 0x002E2160
		private static object CoerceSelectionEnd(DependencyObject d, object value)
		{
			Slider slider = (Slider)d;
			double num = (double)value;
			double selectionStart = slider.SelectionStart;
			double maximum = slider.Maximum;
			if (num < selectionStart)
			{
				return selectionStart;
			}
			if (num > maximum)
			{
				return maximum;
			}
			return value;
		}

		// Token: 0x0600737A RID: 29562 RVA: 0x002E319E File Offset: 0x002E219E
		private static object OnGetSelectionEnd(DependencyObject d)
		{
			return ((Slider)d).SelectionEnd;
		}

		// Token: 0x0600737B RID: 29563 RVA: 0x002E31B0 File Offset: 0x002E21B0
		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			base.CoerceValue(Slider.SelectionStartProperty);
		}

		// Token: 0x0600737C RID: 29564 RVA: 0x002E31BD File Offset: 0x002E21BD
		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			base.CoerceValue(Slider.SelectionStartProperty);
			base.CoerceValue(Slider.SelectionEndProperty);
		}

		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x0600737D RID: 29565 RVA: 0x002E31D5 File Offset: 0x002E21D5
		// (set) Token: 0x0600737E RID: 29566 RVA: 0x002E31E7 File Offset: 0x002E21E7
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsMoveToPointEnabled
		{
			get
			{
				return (bool)base.GetValue(Slider.IsMoveToPointEnabledProperty);
			}
			set
			{
				base.SetValue(Slider.IsMoveToPointEnabledProperty, value);
			}
		}

		// Token: 0x0600737F RID: 29567 RVA: 0x002E31F8 File Offset: 0x002E21F8
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (this.IsMoveToPointEnabled && this.Track != null && this.Track.Thumb != null && !this.Track.Thumb.IsMouseOver)
			{
				Point position = e.MouseDevice.GetPosition(this.Track);
				double num = this.Track.ValueFromPoint(position);
				if (Shape.IsDoubleFinite(num))
				{
					this.UpdateValue(num);
				}
				e.Handled = true;
			}
			base.OnPreviewMouseLeftButtonDown(e);
		}

		// Token: 0x06007380 RID: 29568 RVA: 0x002E3275 File Offset: 0x002E2275
		private static void OnThumbDragStarted(object sender, DragStartedEventArgs e)
		{
			(sender as Slider).OnThumbDragStarted(e);
		}

		// Token: 0x06007381 RID: 29569 RVA: 0x002E3283 File Offset: 0x002E2283
		private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			(sender as Slider).OnThumbDragDelta(e);
		}

		// Token: 0x06007382 RID: 29570 RVA: 0x002E3291 File Offset: 0x002E2291
		private static void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
		{
			(sender as Slider).OnThumbDragCompleted(e);
		}

		// Token: 0x06007383 RID: 29571 RVA: 0x002E32A0 File Offset: 0x002E22A0
		protected virtual void OnThumbDragStarted(DragStartedEventArgs e)
		{
			Thumb thumb = e.OriginalSource as Thumb;
			if (thumb == null || this.AutoToolTipPlacement == AutoToolTipPlacement.None)
			{
				return;
			}
			this._thumbOriginalToolTip = thumb.ToolTip;
			if (this._autoToolTip == null)
			{
				this._autoToolTip = new ToolTip();
				this._autoToolTip.Placement = PlacementMode.Custom;
				this._autoToolTip.PlacementTarget = thumb;
				this._autoToolTip.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.AutoToolTipCustomPlacementCallback);
			}
			thumb.ToolTip = this._autoToolTip;
			this._autoToolTip.Content = this.GetAutoToolTipNumber();
			this._autoToolTip.IsOpen = true;
			((Popup)this._autoToolTip.Parent).Reposition();
		}

		// Token: 0x06007384 RID: 29572 RVA: 0x002E3354 File Offset: 0x002E2354
		protected virtual void OnThumbDragDelta(DragDeltaEventArgs e)
		{
			Thumb thumb = e.OriginalSource as Thumb;
			if (this.Track != null && thumb == this.Track.Thumb)
			{
				double num = base.Value + this.Track.ValueFromDistance(e.HorizontalChange, e.VerticalChange);
				if (Shape.IsDoubleFinite(num))
				{
					this.UpdateValue(num);
				}
				if (this.AutoToolTipPlacement != AutoToolTipPlacement.None)
				{
					if (this._autoToolTip == null)
					{
						this._autoToolTip = new ToolTip();
					}
					this._autoToolTip.Content = this.GetAutoToolTipNumber();
					if (thumb.ToolTip != this._autoToolTip)
					{
						thumb.ToolTip = this._autoToolTip;
					}
					if (!this._autoToolTip.IsOpen)
					{
						this._autoToolTip.IsOpen = true;
					}
					((Popup)this._autoToolTip.Parent).Reposition();
				}
			}
		}

		// Token: 0x06007385 RID: 29573 RVA: 0x002E3430 File Offset: 0x002E2430
		private string GetAutoToolTipNumber()
		{
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
			numberFormatInfo.NumberDecimalDigits = this.AutoToolTipPrecision;
			return base.Value.ToString("N", numberFormatInfo);
		}

		// Token: 0x06007386 RID: 29574 RVA: 0x002E3470 File Offset: 0x002E2470
		protected virtual void OnThumbDragCompleted(DragCompletedEventArgs e)
		{
			Thumb thumb = e.OriginalSource as Thumb;
			if (thumb == null || this.AutoToolTipPlacement == AutoToolTipPlacement.None)
			{
				return;
			}
			if (this._autoToolTip != null)
			{
				this._autoToolTip.IsOpen = false;
			}
			thumb.ToolTip = this._thumbOriginalToolTip;
		}

		// Token: 0x06007387 RID: 29575 RVA: 0x002E34B8 File Offset: 0x002E24B8
		private CustomPopupPlacement[] AutoToolTipCustomPlacementCallback(Size popupSize, Size targetSize, Point offset)
		{
			AutoToolTipPlacement autoToolTipPlacement = this.AutoToolTipPlacement;
			if (autoToolTipPlacement != AutoToolTipPlacement.TopLeft)
			{
				if (autoToolTipPlacement != AutoToolTipPlacement.BottomRight)
				{
					return new CustomPopupPlacement[0];
				}
				if (this.Orientation == Orientation.Horizontal)
				{
					return new CustomPopupPlacement[]
					{
						new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height), PopupPrimaryAxis.Horizontal)
					};
				}
				return new CustomPopupPlacement[]
				{
					new CustomPopupPlacement(new Point(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
				};
			}
			else
			{
				if (this.Orientation == Orientation.Horizontal)
				{
					return new CustomPopupPlacement[]
					{
						new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, -popupSize.Height), PopupPrimaryAxis.Horizontal)
					};
				}
				return new CustomPopupPlacement[]
				{
					new CustomPopupPlacement(new Point(-popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
				};
			}
		}

		// Token: 0x06007388 RID: 29576 RVA: 0x002E35D8 File Offset: 0x002E25D8
		private void UpdateSelectionRangeElementPositionAndSize()
		{
			Size renderSize = new Size(0.0, 0.0);
			Size size = new Size(0.0, 0.0);
			if (this.Track == null || DoubleUtil.LessThan(this.SelectionEnd, this.SelectionStart))
			{
				return;
			}
			renderSize = this.Track.RenderSize;
			size = ((this.Track.Thumb != null) ? this.Track.Thumb.RenderSize : new Size(0.0, 0.0));
			double num = base.Maximum - base.Minimum;
			FrameworkElement selectionRangeElement = this.SelectionRangeElement;
			if (selectionRangeElement == null)
			{
				return;
			}
			if (this.Orientation == Orientation.Horizontal)
			{
				double num2;
				if (DoubleUtil.AreClose(num, 0.0) || DoubleUtil.AreClose(renderSize.Width, size.Width))
				{
					num2 = 0.0;
				}
				else
				{
					num2 = Math.Max(0.0, (renderSize.Width - size.Width) / num);
				}
				selectionRangeElement.Width = (this.SelectionEnd - this.SelectionStart) * num2;
				if (this.IsDirectionReversed)
				{
					Canvas.SetLeft(selectionRangeElement, size.Width * 0.5 + Math.Max(base.Maximum - this.SelectionEnd, 0.0) * num2);
					return;
				}
				Canvas.SetLeft(selectionRangeElement, size.Width * 0.5 + Math.Max(this.SelectionStart - base.Minimum, 0.0) * num2);
				return;
			}
			else
			{
				double num2;
				if (DoubleUtil.AreClose(num, 0.0) || DoubleUtil.AreClose(renderSize.Height, size.Height))
				{
					num2 = 0.0;
				}
				else
				{
					num2 = Math.Max(0.0, (renderSize.Height - size.Height) / num);
				}
				selectionRangeElement.Height = (this.SelectionEnd - this.SelectionStart) * num2;
				if (this.IsDirectionReversed)
				{
					Canvas.SetTop(selectionRangeElement, size.Height * 0.5 + Math.Max(this.SelectionStart - base.Minimum, 0.0) * num2);
					return;
				}
				Canvas.SetTop(selectionRangeElement, size.Height * 0.5 + Math.Max(base.Maximum - this.SelectionEnd, 0.0) * num2);
				return;
			}
		}

		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x06007389 RID: 29577 RVA: 0x002E3857 File Offset: 0x002E2857
		// (set) Token: 0x0600738A RID: 29578 RVA: 0x002E385F File Offset: 0x002E285F
		internal Track Track
		{
			get
			{
				return this._track;
			}
			set
			{
				this._track = value;
			}
		}

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x0600738B RID: 29579 RVA: 0x002E3868 File Offset: 0x002E2868
		// (set) Token: 0x0600738C RID: 29580 RVA: 0x002E3870 File Offset: 0x002E2870
		internal FrameworkElement SelectionRangeElement
		{
			get
			{
				return this._selectionRangeElement;
			}
			set
			{
				this._selectionRangeElement = value;
			}
		}

		// Token: 0x0600738D RID: 29581 RVA: 0x002E387C File Offset: 0x002E287C
		private double SnapToTick(double value)
		{
			if (this.IsSnapToTickEnabled)
			{
				double num = base.Minimum;
				double num2 = base.Maximum;
				DoubleCollection doubleCollection = null;
				bool flag;
				if (base.GetValueSource(Slider.TicksProperty, null, out flag) != BaseValueSourceInternal.Default || flag)
				{
					doubleCollection = this.Ticks;
				}
				if (doubleCollection != null && doubleCollection.Count > 0)
				{
					for (int i = 0; i < doubleCollection.Count; i++)
					{
						double num3 = doubleCollection[i];
						if (DoubleUtil.AreClose(num3, value))
						{
							return value;
						}
						if (DoubleUtil.LessThan(num3, value) && DoubleUtil.GreaterThan(num3, num))
						{
							num = num3;
						}
						else if (DoubleUtil.GreaterThan(num3, value) && DoubleUtil.LessThan(num3, num2))
						{
							num2 = num3;
						}
					}
				}
				else if (DoubleUtil.GreaterThan(this.TickFrequency, 0.0))
				{
					num = base.Minimum + Math.Round((value - base.Minimum) / this.TickFrequency) * this.TickFrequency;
					num2 = Math.Min(base.Maximum, num + this.TickFrequency);
				}
				value = (DoubleUtil.GreaterThanOrClose(value, (num + num2) * 0.5) ? num2 : num);
			}
			return value;
		}

		// Token: 0x0600738E RID: 29582 RVA: 0x002E3998 File Offset: 0x002E2998
		private void MoveToNextTick(double direction)
		{
			if (direction != 0.0)
			{
				double value = base.Value;
				double num = this.SnapToTick(Math.Max(base.Minimum, Math.Min(base.Maximum, value + direction)));
				bool flag = direction > 0.0;
				if (num == value && (!flag || value != base.Maximum) && (flag || value != base.Minimum))
				{
					DoubleCollection doubleCollection = null;
					bool flag2;
					if (base.GetValueSource(Slider.TicksProperty, null, out flag2) != BaseValueSourceInternal.Default || flag2)
					{
						doubleCollection = this.Ticks;
					}
					if (doubleCollection != null && doubleCollection.Count > 0)
					{
						for (int i = 0; i < doubleCollection.Count; i++)
						{
							double num2 = doubleCollection[i];
							if ((flag && DoubleUtil.GreaterThan(num2, value) && (DoubleUtil.LessThan(num2, num) || num == value)) || (!flag && DoubleUtil.LessThan(num2, value) && (DoubleUtil.GreaterThan(num2, num) || num == value)))
							{
								num = num2;
							}
						}
					}
					else if (DoubleUtil.GreaterThan(this.TickFrequency, 0.0))
					{
						double num3 = Math.Round((value - base.Minimum) / this.TickFrequency);
						if (flag)
						{
							num3 += 1.0;
						}
						else
						{
							num3 -= 1.0;
						}
						num = base.Minimum + num3 * this.TickFrequency;
					}
				}
				if (num != value)
				{
					base.SetCurrentValueInternal(RangeBase.ValueProperty, num);
				}
			}
		}

		// Token: 0x0600738F RID: 29583 RVA: 0x002E3B08 File Offset: 0x002E2B08
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new SliderAutomationPeer(this);
		}

		// Token: 0x06007390 RID: 29584 RVA: 0x002E3B10 File Offset: 0x002E2B10
		private static void _OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton != MouseButton.Left)
			{
				return;
			}
			Slider slider = (Slider)sender;
			if (!slider.IsKeyboardFocusWithin)
			{
				e.Handled = (slider.Focus() || e.Handled);
			}
		}

		// Token: 0x06007391 RID: 29585 RVA: 0x002E3B4C File Offset: 0x002E2B4C
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size result = base.ArrangeOverride(finalSize);
			this.UpdateSelectionRangeElementPositionAndSize();
			return result;
		}

		// Token: 0x06007392 RID: 29586 RVA: 0x002E3B5B File Offset: 0x002E2B5B
		protected override void OnValueChanged(double oldValue, double newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			this.UpdateSelectionRangeElementPositionAndSize();
		}

		// Token: 0x06007393 RID: 29587 RVA: 0x002E3B6C File Offset: 0x002E2B6C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.SelectionRangeElement = (base.GetTemplateChild("PART_SelectionRange") as FrameworkElement);
			this.Track = (base.GetTemplateChild("PART_Track") as Track);
			if (this._autoToolTip != null)
			{
				this._autoToolTip.PlacementTarget = ((this.Track != null) ? this.Track.Thumb : null);
			}
		}

		// Token: 0x06007394 RID: 29588 RVA: 0x002E3BD4 File Offset: 0x002E2BD4
		protected virtual void OnIncreaseLarge()
		{
			this.MoveToNextTick(base.LargeChange);
		}

		// Token: 0x06007395 RID: 29589 RVA: 0x002E3BE2 File Offset: 0x002E2BE2
		protected virtual void OnDecreaseLarge()
		{
			this.MoveToNextTick(-base.LargeChange);
		}

		// Token: 0x06007396 RID: 29590 RVA: 0x002E3BF1 File Offset: 0x002E2BF1
		protected virtual void OnIncreaseSmall()
		{
			this.MoveToNextTick(base.SmallChange);
		}

		// Token: 0x06007397 RID: 29591 RVA: 0x002E3BFF File Offset: 0x002E2BFF
		protected virtual void OnDecreaseSmall()
		{
			this.MoveToNextTick(-base.SmallChange);
		}

		// Token: 0x06007398 RID: 29592 RVA: 0x002E3C0E File Offset: 0x002E2C0E
		protected virtual void OnMaximizeValue()
		{
			base.SetCurrentValueInternal(RangeBase.ValueProperty, base.Maximum);
		}

		// Token: 0x06007399 RID: 29593 RVA: 0x002E3C26 File Offset: 0x002E2C26
		protected virtual void OnMinimizeValue()
		{
			base.SetCurrentValueInternal(RangeBase.ValueProperty, base.Minimum);
		}

		// Token: 0x0600739A RID: 29594 RVA: 0x002E3C40 File Offset: 0x002E2C40
		private void UpdateValue(double value)
		{
			double num = this.SnapToTick(value);
			if (num != base.Value)
			{
				base.SetCurrentValueInternal(RangeBase.ValueProperty, Math.Max(base.Minimum, Math.Min(base.Maximum, num)));
			}
		}

		// Token: 0x0600739B RID: 29595 RVA: 0x002E3C88 File Offset: 0x002E2C88
		private static bool IsValidDoubleValue(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && !double.IsInfinity(num);
		}

		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x0600739C RID: 29596 RVA: 0x002E3CAF File Offset: 0x002E2CAF
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Slider._dType;
			}
		}

		// Token: 0x040037C1 RID: 14273
		private static RoutedCommand _increaseLargeCommand = null;

		// Token: 0x040037C2 RID: 14274
		private static RoutedCommand _increaseSmallCommand = null;

		// Token: 0x040037C3 RID: 14275
		private static RoutedCommand _decreaseLargeCommand = null;

		// Token: 0x040037C4 RID: 14276
		private static RoutedCommand _decreaseSmallCommand = null;

		// Token: 0x040037C5 RID: 14277
		private static RoutedCommand _minimizeValueCommand = null;

		// Token: 0x040037C6 RID: 14278
		private static RoutedCommand _maximizeValueCommand = null;

		// Token: 0x040037C7 RID: 14279
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Slider), new FrameworkPropertyMetadata(Orientation.Horizontal), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		// Token: 0x040037C8 RID: 14280
		public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register("IsDirectionReversed", typeof(bool), typeof(Slider), new FrameworkPropertyMetadata(false));

		// Token: 0x040037C9 RID: 14281
		public static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof(Slider), new FrameworkPropertyMetadata(RepeatButton.GetKeyboardDelay()));

		// Token: 0x040037CA RID: 14282
		public static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof(Slider), new FrameworkPropertyMetadata(RepeatButton.GetKeyboardSpeed()));

		// Token: 0x040037CB RID: 14283
		public static readonly DependencyProperty AutoToolTipPlacementProperty = DependencyProperty.Register("AutoToolTipPlacement", typeof(AutoToolTipPlacement), typeof(Slider), new FrameworkPropertyMetadata(AutoToolTipPlacement.None), new ValidateValueCallback(Slider.IsValidAutoToolTipPlacement));

		// Token: 0x040037CC RID: 14284
		public static readonly DependencyProperty AutoToolTipPrecisionProperty = DependencyProperty.Register("AutoToolTipPrecision", typeof(int), typeof(Slider), new FrameworkPropertyMetadata(0), new ValidateValueCallback(Slider.IsValidAutoToolTipPrecision));

		// Token: 0x040037CD RID: 14285
		public static readonly DependencyProperty IsSnapToTickEnabledProperty = DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(Slider), new FrameworkPropertyMetadata(false));

		// Token: 0x040037CE RID: 14286
		public static readonly DependencyProperty TickPlacementProperty = DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(Slider), new FrameworkPropertyMetadata(TickPlacement.None), new ValidateValueCallback(Slider.IsValidTickPlacement));

		// Token: 0x040037CF RID: 14287
		public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register("TickFrequency", typeof(double), typeof(Slider), new FrameworkPropertyMetadata(1.0), new ValidateValueCallback(Slider.IsValidDoubleValue));

		// Token: 0x040037D0 RID: 14288
		public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks", typeof(DoubleCollection), typeof(Slider), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(DoubleCollection.Empty)));

		// Token: 0x040037D1 RID: 14289
		public static readonly DependencyProperty IsSelectionRangeEnabledProperty = DependencyProperty.Register("IsSelectionRangeEnabled", typeof(bool), typeof(Slider), new FrameworkPropertyMetadata(false));

		// Token: 0x040037D2 RID: 14290
		public static readonly DependencyProperty SelectionStartProperty = DependencyProperty.Register("SelectionStart", typeof(double), typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Slider.OnSelectionStartChanged), new CoerceValueCallback(Slider.CoerceSelectionStart)), new ValidateValueCallback(Slider.IsValidDoubleValue));

		// Token: 0x040037D3 RID: 14291
		public static readonly DependencyProperty SelectionEndProperty = DependencyProperty.Register("SelectionEnd", typeof(double), typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Slider.OnSelectionEndChanged), new CoerceValueCallback(Slider.CoerceSelectionEnd)), new ValidateValueCallback(Slider.IsValidDoubleValue));

		// Token: 0x040037D4 RID: 14292
		public static readonly DependencyProperty IsMoveToPointEnabledProperty = DependencyProperty.Register("IsMoveToPointEnabled", typeof(bool), typeof(Slider), new FrameworkPropertyMetadata(false));

		// Token: 0x040037D5 RID: 14293
		private const string TrackName = "PART_Track";

		// Token: 0x040037D6 RID: 14294
		private const string SelectionRangeElementName = "PART_SelectionRange";

		// Token: 0x040037D7 RID: 14295
		private FrameworkElement _selectionRangeElement;

		// Token: 0x040037D8 RID: 14296
		private Track _track;

		// Token: 0x040037D9 RID: 14297
		private ToolTip _autoToolTip;

		// Token: 0x040037DA RID: 14298
		private object _thumbOriginalToolTip;

		// Token: 0x040037DB RID: 14299
		private static DependencyObjectType _dType;

		// Token: 0x02000C1E RID: 3102
		private class SliderGesture : InputGesture
		{
			// Token: 0x0600908A RID: 37002 RVA: 0x00346C4C File Offset: 0x00345C4C
			public SliderGesture(Key normal, Key inverted, bool forHorizontal)
			{
				this._normal = normal;
				this._inverted = inverted;
				this._forHorizontal = forHorizontal;
			}

			// Token: 0x0600908B RID: 37003 RVA: 0x00346C6C File Offset: 0x00345C6C
			public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
			{
				KeyEventArgs keyEventArgs = inputEventArgs as KeyEventArgs;
				Slider slider = targetElement as Slider;
				if (keyEventArgs != null && slider != null && Keyboard.Modifiers == ModifierKeys.None)
				{
					if (this._normal == keyEventArgs.RealKey)
					{
						return !this.IsInverted(slider);
					}
					if (this._inverted == keyEventArgs.RealKey)
					{
						return this.IsInverted(slider);
					}
				}
				return false;
			}

			// Token: 0x0600908C RID: 37004 RVA: 0x00346CC4 File Offset: 0x00345CC4
			private bool IsInverted(Slider slider)
			{
				if (this._forHorizontal)
				{
					return slider.IsDirectionReversed != (slider.FlowDirection == FlowDirection.RightToLeft);
				}
				return slider.IsDirectionReversed;
			}

			// Token: 0x04004B28 RID: 19240
			private Key _normal;

			// Token: 0x04004B29 RID: 19241
			private Key _inverted;

			// Token: 0x04004B2A RID: 19242
			private bool _forHorizontal;
		}
	}
}
