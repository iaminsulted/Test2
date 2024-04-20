using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000721 RID: 1825
	[TemplatePart(Name = "PART_Root", Type = typeof(Panel))]
	[TemplatePart(Name = "PART_CalendarItem", Type = typeof(CalendarItem))]
	public class Calendar : Control
	{
		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06005FF1 RID: 24561 RVA: 0x002978CB File Offset: 0x002968CB
		// (remove) Token: 0x06005FF2 RID: 24562 RVA: 0x002978D9 File Offset: 0x002968D9
		public event EventHandler<SelectionChangedEventArgs> SelectedDatesChanged
		{
			add
			{
				base.AddHandler(Calendar.SelectedDatesChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Calendar.SelectedDatesChangedEvent, value);
			}
		}

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06005FF3 RID: 24563 RVA: 0x002978E8 File Offset: 0x002968E8
		// (remove) Token: 0x06005FF4 RID: 24564 RVA: 0x00297920 File Offset: 0x00296920
		public event EventHandler<CalendarDateChangedEventArgs> DisplayDateChanged;

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06005FF5 RID: 24565 RVA: 0x00297958 File Offset: 0x00296958
		// (remove) Token: 0x06005FF6 RID: 24566 RVA: 0x00297990 File Offset: 0x00296990
		public event EventHandler<CalendarModeChangedEventArgs> DisplayModeChanged;

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06005FF7 RID: 24567 RVA: 0x002979C8 File Offset: 0x002969C8
		// (remove) Token: 0x06005FF8 RID: 24568 RVA: 0x00297A00 File Offset: 0x00296A00
		public event EventHandler<EventArgs> SelectionModeChanged;

		// Token: 0x06005FF9 RID: 24569 RVA: 0x00297A38 File Offset: 0x00296A38
		static Calendar()
		{
			Calendar.SelectedDatesChangedEvent = EventManager.RegisterRoutedEvent("SelectedDatesChanged", RoutingStrategy.Direct, typeof(EventHandler<SelectionChangedEventArgs>), typeof(Calendar));
			Calendar.CalendarButtonStyleProperty = DependencyProperty.Register("CalendarButtonStyle", typeof(Style), typeof(Calendar));
			Calendar.CalendarDayButtonStyleProperty = DependencyProperty.Register("CalendarDayButtonStyle", typeof(Style), typeof(Calendar));
			Calendar.CalendarItemStyleProperty = DependencyProperty.Register("CalendarItemStyle", typeof(Style), typeof(Calendar));
			Calendar.DisplayDateProperty = DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(Calendar), new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnDisplayDateChanged), new CoerceValueCallback(Calendar.CoerceDisplayDate)));
			Calendar.DisplayDateEndProperty = DependencyProperty.Register("DisplayDateEnd", typeof(DateTime?), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnDisplayDateEndChanged), new CoerceValueCallback(Calendar.CoerceDisplayDateEnd)));
			Calendar.DisplayDateStartProperty = DependencyProperty.Register("DisplayDateStart", typeof(DateTime?), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnDisplayDateStartChanged), new CoerceValueCallback(Calendar.CoerceDisplayDateStart)));
			Calendar.DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(CalendarMode), typeof(Calendar), new FrameworkPropertyMetadata(CalendarMode.Month, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnDisplayModePropertyChanged)), new ValidateValueCallback(Calendar.IsValidDisplayMode));
			Calendar.FirstDayOfWeekProperty = DependencyProperty.Register("FirstDayOfWeek", typeof(DayOfWeek), typeof(Calendar), new FrameworkPropertyMetadata(DateTimeHelper.GetCurrentDateFormat().FirstDayOfWeek, new PropertyChangedCallback(Calendar.OnFirstDayOfWeekChanged)), new ValidateValueCallback(Calendar.IsValidFirstDayOfWeek));
			Calendar.IsTodayHighlightedProperty = DependencyProperty.Register("IsTodayHighlighted", typeof(bool), typeof(Calendar), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(Calendar.OnIsTodayHighlightedChanged)));
			Calendar.SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnSelectedDateChanged)));
			Calendar.SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(CalendarSelectionMode), typeof(Calendar), new FrameworkPropertyMetadata(CalendarSelectionMode.SingleDate, new PropertyChangedCallback(Calendar.OnSelectionModeChanged)), new ValidateValueCallback(Calendar.IsValidSelectionMode));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			FrameworkElement.LanguageProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(new PropertyChangedCallback(Calendar.OnLanguageChanged)));
			EventManager.RegisterClassHandler(typeof(Calendar), UIElement.GotFocusEvent, new RoutedEventHandler(Calendar.OnGotFocus));
			ControlsTraceLogger.AddControl(TelemetryControls.Calendar);
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x00297DAE File Offset: 0x00296DAE
		public Calendar()
		{
			this._blackoutDates = new CalendarBlackoutDatesCollection(this);
			this._selectedDates = new SelectedDatesCollection(this);
			base.SetCurrentValueInternal(Calendar.DisplayDateProperty, DateTime.Today);
		}

		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x06005FFB RID: 24571 RVA: 0x00297DE3 File Offset: 0x00296DE3
		public CalendarBlackoutDatesCollection BlackoutDates
		{
			get
			{
				return this._blackoutDates;
			}
		}

		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x06005FFC RID: 24572 RVA: 0x00297DEB File Offset: 0x00296DEB
		// (set) Token: 0x06005FFD RID: 24573 RVA: 0x00297DFD File Offset: 0x00296DFD
		public Style CalendarButtonStyle
		{
			get
			{
				return (Style)base.GetValue(Calendar.CalendarButtonStyleProperty);
			}
			set
			{
				base.SetValue(Calendar.CalendarButtonStyleProperty, value);
			}
		}

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x06005FFE RID: 24574 RVA: 0x00297E0B File Offset: 0x00296E0B
		// (set) Token: 0x06005FFF RID: 24575 RVA: 0x00297E1D File Offset: 0x00296E1D
		public Style CalendarDayButtonStyle
		{
			get
			{
				return (Style)base.GetValue(Calendar.CalendarDayButtonStyleProperty);
			}
			set
			{
				base.SetValue(Calendar.CalendarDayButtonStyleProperty, value);
			}
		}

		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x06006000 RID: 24576 RVA: 0x00297E2B File Offset: 0x00296E2B
		// (set) Token: 0x06006001 RID: 24577 RVA: 0x00297E3D File Offset: 0x00296E3D
		public Style CalendarItemStyle
		{
			get
			{
				return (Style)base.GetValue(Calendar.CalendarItemStyleProperty);
			}
			set
			{
				base.SetValue(Calendar.CalendarItemStyleProperty, value);
			}
		}

		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x06006002 RID: 24578 RVA: 0x00297E4B File Offset: 0x00296E4B
		// (set) Token: 0x06006003 RID: 24579 RVA: 0x00297E5D File Offset: 0x00296E5D
		public DateTime DisplayDate
		{
			get
			{
				return (DateTime)base.GetValue(Calendar.DisplayDateProperty);
			}
			set
			{
				base.SetValue(Calendar.DisplayDateProperty, value);
			}
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x00297E70 File Offset: 0x00296E70
		private static void OnDisplayDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			calendar.DisplayDateInternal = DateTimeHelper.DiscardDayTime((DateTime)e.NewValue);
			calendar.UpdateCellItems();
			calendar.OnDisplayDateChanged(new CalendarDateChangedEventArgs(new DateTime?((DateTime)e.OldValue), new DateTime?((DateTime)e.NewValue)));
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x00297ECC File Offset: 0x00296ECC
		private static object CoerceDisplayDate(DependencyObject d, object value)
		{
			Calendar calendar = d as Calendar;
			DateTime t = (DateTime)value;
			if (calendar.DisplayDateStart != null && t < calendar.DisplayDateStart.Value)
			{
				value = calendar.DisplayDateStart.Value;
			}
			else if (calendar.DisplayDateEnd != null && t > calendar.DisplayDateEnd.Value)
			{
				value = calendar.DisplayDateEnd.Value;
			}
			return value;
		}

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x06006006 RID: 24582 RVA: 0x00297F60 File Offset: 0x00296F60
		// (set) Token: 0x06006007 RID: 24583 RVA: 0x00297F72 File Offset: 0x00296F72
		public DateTime? DisplayDateEnd
		{
			get
			{
				return (DateTime?)base.GetValue(Calendar.DisplayDateEndProperty);
			}
			set
			{
				base.SetValue(Calendar.DisplayDateEndProperty, value);
			}
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x00297F85 File Offset: 0x00296F85
		private static void OnDisplayDateEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			calendar.CoerceValue(Calendar.DisplayDateProperty);
			calendar.UpdateCellItems();
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x00297FA0 File Offset: 0x00296FA0
		private static object CoerceDisplayDateEnd(DependencyObject d, object value)
		{
			Calendar calendar = d as Calendar;
			DateTime? dateTime = (DateTime?)value;
			if (dateTime != null)
			{
				if (calendar.DisplayDateStart != null && dateTime.Value < calendar.DisplayDateStart.Value)
				{
					value = calendar.DisplayDateStart;
				}
				DateTime? maximumDate = calendar.SelectedDates.MaximumDate;
				if (maximumDate != null && dateTime.Value < maximumDate.Value)
				{
					value = maximumDate;
				}
			}
			return value;
		}

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x0600600A RID: 24586 RVA: 0x00298030 File Offset: 0x00297030
		// (set) Token: 0x0600600B RID: 24587 RVA: 0x00298042 File Offset: 0x00297042
		public DateTime? DisplayDateStart
		{
			get
			{
				return (DateTime?)base.GetValue(Calendar.DisplayDateStartProperty);
			}
			set
			{
				base.SetValue(Calendar.DisplayDateStartProperty, value);
			}
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x00298055 File Offset: 0x00297055
		private static void OnDisplayDateStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			calendar.CoerceValue(Calendar.DisplayDateEndProperty);
			calendar.CoerceValue(Calendar.DisplayDateProperty);
			calendar.UpdateCellItems();
		}

		// Token: 0x0600600D RID: 24589 RVA: 0x00298078 File Offset: 0x00297078
		private static object CoerceDisplayDateStart(DependencyObject d, object value)
		{
			Calendar calendar = d as Calendar;
			DateTime? dateTime = (DateTime?)value;
			if (dateTime != null)
			{
				DateTime? minimumDate = calendar.SelectedDates.MinimumDate;
				if (minimumDate != null && dateTime.Value > minimumDate.Value)
				{
					value = minimumDate;
				}
			}
			return value;
		}

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x0600600E RID: 24590 RVA: 0x002980CF File Offset: 0x002970CF
		// (set) Token: 0x0600600F RID: 24591 RVA: 0x002980E1 File Offset: 0x002970E1
		public CalendarMode DisplayMode
		{
			get
			{
				return (CalendarMode)base.GetValue(Calendar.DisplayModeProperty);
			}
			set
			{
				base.SetValue(Calendar.DisplayModeProperty, value);
			}
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x002980F4 File Offset: 0x002970F4
		private static void OnDisplayModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			CalendarMode calendarMode = (CalendarMode)e.NewValue;
			CalendarMode calendarMode2 = (CalendarMode)e.OldValue;
			CalendarItem monthControl = calendar.MonthControl;
			if (calendarMode != CalendarMode.Month)
			{
				if (calendarMode - CalendarMode.Year <= 1)
				{
					if (calendarMode2 == CalendarMode.Month)
					{
						calendar.SetCurrentValueInternal(Calendar.DisplayDateProperty, calendar.CurrentDate);
					}
					calendar.UpdateCellItems();
				}
			}
			else
			{
				if (calendarMode2 == CalendarMode.Year || calendarMode2 == CalendarMode.Decade)
				{
					Calendar calendar2 = calendar;
					Calendar calendar3 = calendar;
					DateTime? dateTime = null;
					calendar3.HoverEnd = dateTime;
					calendar2.HoverStart = dateTime;
					calendar.CurrentDate = calendar.DisplayDate;
				}
				calendar.UpdateCellItems();
			}
			calendar.OnDisplayModeChanged(new CalendarModeChangedEventArgs((CalendarMode)e.OldValue, calendarMode));
		}

		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x06006011 RID: 24593 RVA: 0x0029819D File Offset: 0x0029719D
		// (set) Token: 0x06006012 RID: 24594 RVA: 0x002981AF File Offset: 0x002971AF
		public DayOfWeek FirstDayOfWeek
		{
			get
			{
				return (DayOfWeek)base.GetValue(Calendar.FirstDayOfWeekProperty);
			}
			set
			{
				base.SetValue(Calendar.FirstDayOfWeekProperty, value);
			}
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x002981C2 File Offset: 0x002971C2
		private static void OnFirstDayOfWeekChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Calendar).UpdateCellItems();
		}

		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x06006014 RID: 24596 RVA: 0x002981CF File Offset: 0x002971CF
		// (set) Token: 0x06006015 RID: 24597 RVA: 0x002981E1 File Offset: 0x002971E1
		public bool IsTodayHighlighted
		{
			get
			{
				return (bool)base.GetValue(Calendar.IsTodayHighlightedProperty);
			}
			set
			{
				base.SetValue(Calendar.IsTodayHighlightedProperty, value);
			}
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x002981F0 File Offset: 0x002971F0
		private static void OnIsTodayHighlightedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			int num = DateTimeHelper.CompareYearMonth(calendar.DisplayDateInternal, DateTime.Today);
			if (num > -2 && num < 2)
			{
				calendar.UpdateCellItems();
			}
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x00298224 File Offset: 0x00297224
		private static void OnLanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			if (DependencyPropertyHelper.GetValueSource(d, Calendar.FirstDayOfWeekProperty).BaseValueSource == BaseValueSource.Default)
			{
				calendar.SetCurrentValueInternal(Calendar.FirstDayOfWeekProperty, DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(calendar)).FirstDayOfWeek);
				calendar.UpdateCellItems();
			}
		}

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x06006018 RID: 24600 RVA: 0x00298274 File Offset: 0x00297274
		// (set) Token: 0x06006019 RID: 24601 RVA: 0x00298286 File Offset: 0x00297286
		public DateTime? SelectedDate
		{
			get
			{
				return (DateTime?)base.GetValue(Calendar.SelectedDateProperty);
			}
			set
			{
				base.SetValue(Calendar.SelectedDateProperty, value);
			}
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x0029829C File Offset: 0x0029729C
		private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			if (calendar.SelectionMode == CalendarSelectionMode.None && e.NewValue != null)
			{
				throw new InvalidOperationException(SR.Get("Calendar_OnSelectedDateChanged_InvalidOperation"));
			}
			DateTime? dateTime = (DateTime?)e.NewValue;
			if (!Calendar.IsValidDateSelection(calendar, dateTime))
			{
				throw new ArgumentOutOfRangeException("d", SR.Get("Calendar_OnSelectedDateChanged_InvalidValue"));
			}
			if (dateTime == null)
			{
				calendar.SelectedDates.ClearInternal(true);
			}
			else if (dateTime != null && (calendar.SelectedDates.Count <= 0 || !(calendar.SelectedDates[0] == dateTime.Value)))
			{
				calendar.SelectedDates.ClearInternal();
				calendar.SelectedDates.Add(dateTime.Value);
			}
			if (calendar.SelectionMode == CalendarSelectionMode.SingleDate)
			{
				if (dateTime != null)
				{
					calendar.CurrentDate = dateTime.Value;
				}
				calendar.UpdateCellItems();
				return;
			}
		}

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x0600601B RID: 24603 RVA: 0x00298392 File Offset: 0x00297392
		public SelectedDatesCollection SelectedDates
		{
			get
			{
				return this._selectedDates;
			}
		}

		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x0600601C RID: 24604 RVA: 0x0029839A File Offset: 0x0029739A
		// (set) Token: 0x0600601D RID: 24605 RVA: 0x002983AC File Offset: 0x002973AC
		public CalendarSelectionMode SelectionMode
		{
			get
			{
				return (CalendarSelectionMode)base.GetValue(Calendar.SelectionModeProperty);
			}
			set
			{
				base.SetValue(Calendar.SelectionModeProperty, value);
			}
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x002983C0 File Offset: 0x002973C0
		private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			DateTime? dateTime = null;
			calendar.HoverEnd = dateTime;
			calendar.HoverStart = dateTime;
			calendar.SelectedDates.ClearInternal(true);
			calendar.OnSelectionModeChanged(EventArgs.Empty);
		}

		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x0600601F RID: 24607 RVA: 0x00298400 File Offset: 0x00297400
		// (remove) Token: 0x06006020 RID: 24608 RVA: 0x00298438 File Offset: 0x00297438
		internal event MouseButtonEventHandler DayButtonMouseUp;

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06006021 RID: 24609 RVA: 0x00298470 File Offset: 0x00297470
		// (remove) Token: 0x06006022 RID: 24610 RVA: 0x002984A8 File Offset: 0x002974A8
		internal event RoutedEventHandler DayOrMonthPreviewKeyDown;

		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x06006023 RID: 24611 RVA: 0x002984DD File Offset: 0x002974DD
		// (set) Token: 0x06006024 RID: 24612 RVA: 0x002984E5 File Offset: 0x002974E5
		internal bool DatePickerDisplayDateFlag { get; set; }

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x06006025 RID: 24613 RVA: 0x002984EE File Offset: 0x002974EE
		// (set) Token: 0x06006026 RID: 24614 RVA: 0x002984F6 File Offset: 0x002974F6
		internal DateTime DisplayDateInternal { get; private set; }

		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x06006027 RID: 24615 RVA: 0x00298500 File Offset: 0x00297500
		internal DateTime DisplayDateEndInternal
		{
			get
			{
				return this.DisplayDateEnd.GetValueOrDefault(DateTime.MaxValue);
			}
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x06006028 RID: 24616 RVA: 0x00298520 File Offset: 0x00297520
		internal DateTime DisplayDateStartInternal
		{
			get
			{
				return this.DisplayDateStart.GetValueOrDefault(DateTime.MinValue);
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x06006029 RID: 24617 RVA: 0x00298540 File Offset: 0x00297540
		// (set) Token: 0x0600602A RID: 24618 RVA: 0x00298553 File Offset: 0x00297553
		internal DateTime CurrentDate
		{
			get
			{
				return this._currentDate.GetValueOrDefault(this.DisplayDateInternal);
			}
			set
			{
				this._currentDate = new DateTime?(value);
			}
		}

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x0600602B RID: 24619 RVA: 0x00298564 File Offset: 0x00297564
		// (set) Token: 0x0600602C RID: 24620 RVA: 0x0029858A File Offset: 0x0029758A
		internal DateTime? HoverStart
		{
			get
			{
				if (this.SelectionMode != CalendarSelectionMode.None)
				{
					return this._hoverStart;
				}
				return null;
			}
			set
			{
				this._hoverStart = value;
			}
		}

		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x0600602D RID: 24621 RVA: 0x00298594 File Offset: 0x00297594
		// (set) Token: 0x0600602E RID: 24622 RVA: 0x002985BA File Offset: 0x002975BA
		internal DateTime? HoverEnd
		{
			get
			{
				if (this.SelectionMode != CalendarSelectionMode.None)
				{
					return this._hoverEnd;
				}
				return null;
			}
			set
			{
				this._hoverEnd = value;
			}
		}

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x0600602F RID: 24623 RVA: 0x002985C3 File Offset: 0x002975C3
		internal CalendarItem MonthControl
		{
			get
			{
				return this._monthControl;
			}
		}

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x06006030 RID: 24624 RVA: 0x002985CB File Offset: 0x002975CB
		internal DateTime DisplayMonth
		{
			get
			{
				return DateTimeHelper.DiscardDayTime(this.DisplayDate);
			}
		}

		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x06006031 RID: 24625 RVA: 0x002985D8 File Offset: 0x002975D8
		internal DateTime DisplayYear
		{
			get
			{
				return new DateTime(this.DisplayDate.Year, 1, 1);
			}
		}

		// Token: 0x06006032 RID: 24626 RVA: 0x002985FC File Offset: 0x002975FC
		public override void OnApplyTemplate()
		{
			if (this._monthControl != null)
			{
				this._monthControl.Owner = null;
			}
			base.OnApplyTemplate();
			this._monthControl = (base.GetTemplateChild("PART_CalendarItem") as CalendarItem);
			if (this._monthControl != null)
			{
				this._monthControl.Owner = this;
			}
			this.CurrentDate = this.DisplayDate;
			this.UpdateCellItems();
		}

		// Token: 0x06006033 RID: 24627 RVA: 0x00298660 File Offset: 0x00297660
		public override string ToString()
		{
			if (this.SelectedDate != null)
			{
				return this.SelectedDate.Value.ToString(DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)));
			}
			return string.Empty;
		}

		// Token: 0x06006034 RID: 24628 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSelectedDatesChanged(SelectionChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06006035 RID: 24629 RVA: 0x002986A4 File Offset: 0x002976A4
		protected virtual void OnDisplayDateChanged(CalendarDateChangedEventArgs e)
		{
			EventHandler<CalendarDateChangedEventArgs> displayDateChanged = this.DisplayDateChanged;
			if (displayDateChanged != null)
			{
				displayDateChanged(this, e);
			}
		}

		// Token: 0x06006036 RID: 24630 RVA: 0x002986C4 File Offset: 0x002976C4
		protected virtual void OnDisplayModeChanged(CalendarModeChangedEventArgs e)
		{
			EventHandler<CalendarModeChangedEventArgs> displayModeChanged = this.DisplayModeChanged;
			if (displayModeChanged != null)
			{
				displayModeChanged(this, e);
			}
		}

		// Token: 0x06006037 RID: 24631 RVA: 0x002986E4 File Offset: 0x002976E4
		protected virtual void OnSelectionModeChanged(EventArgs e)
		{
			EventHandler<EventArgs> selectionModeChanged = this.SelectionModeChanged;
			if (selectionModeChanged != null)
			{
				selectionModeChanged(this, e);
			}
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x00298703 File Offset: 0x00297703
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new CalendarAutomationPeer(this);
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x0029870B File Offset: 0x0029770B
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = this.ProcessCalendarKey(e);
			}
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x00298722 File Offset: 0x00297722
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (!e.Handled && (e.Key == Key.LeftShift || e.Key == Key.RightShift))
			{
				this.ProcessShiftKeyUp();
			}
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x00298748 File Offset: 0x00297748
		internal CalendarDayButton FindDayButtonFromDay(DateTime day)
		{
			if (this.MonthControl != null)
			{
				foreach (CalendarDayButton calendarDayButton in this.MonthControl.GetCalendarDayButtons())
				{
					if (calendarDayButton.DataContext is DateTime && DateTimeHelper.CompareDays((DateTime)calendarDayButton.DataContext, day) == 0)
					{
						return calendarDayButton;
					}
				}
			}
			return null;
		}

		// Token: 0x0600603C RID: 24636 RVA: 0x002987C4 File Offset: 0x002977C4
		internal static bool IsValidDateSelection(Calendar cal, object value)
		{
			return value == null || !cal.BlackoutDates.Contains((DateTime)value);
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x002987E0 File Offset: 0x002977E0
		internal void OnDayButtonMouseUp(MouseButtonEventArgs e)
		{
			MouseButtonEventHandler dayButtonMouseUp = this.DayButtonMouseUp;
			if (dayButtonMouseUp != null)
			{
				dayButtonMouseUp(this, e);
			}
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x00298800 File Offset: 0x00297800
		internal void OnDayOrMonthPreviewKeyDown(RoutedEventArgs e)
		{
			RoutedEventHandler dayOrMonthPreviewKeyDown = this.DayOrMonthPreviewKeyDown;
			if (dayOrMonthPreviewKeyDown != null)
			{
				dayOrMonthPreviewKeyDown(this, e);
			}
		}

		// Token: 0x0600603F RID: 24639 RVA: 0x0029881F File Offset: 0x0029781F
		internal void OnDayClick(DateTime selectedDate)
		{
			if (this.SelectionMode == CalendarSelectionMode.None)
			{
				this.CurrentDate = selectedDate;
			}
			if (DateTimeHelper.CompareYearMonth(selectedDate, this.DisplayDateInternal) != 0)
			{
				this.MoveDisplayTo(new DateTime?(selectedDate));
				return;
			}
			this.UpdateCellItems();
			this.FocusDate(selectedDate);
		}

		// Token: 0x06006040 RID: 24640 RVA: 0x0029885C File Offset: 0x0029785C
		internal void OnCalendarButtonPressed(CalendarButton b, bool switchDisplayMode)
		{
			if (b.DataContext is DateTime)
			{
				DateTime yearMonth = (DateTime)b.DataContext;
				DateTime? dateTime = null;
				CalendarMode calendarMode = CalendarMode.Month;
				switch (this.DisplayMode)
				{
				case CalendarMode.Year:
					dateTime = DateTimeHelper.SetYearMonth(this.DisplayDate, yearMonth);
					calendarMode = CalendarMode.Month;
					break;
				case CalendarMode.Decade:
					dateTime = DateTimeHelper.SetYear(this.DisplayDate, yearMonth.Year);
					calendarMode = CalendarMode.Year;
					break;
				}
				if (dateTime != null)
				{
					this.DisplayDate = dateTime.Value;
					if (switchDisplayMode)
					{
						base.SetCurrentValueInternal(Calendar.DisplayModeProperty, calendarMode);
						this.FocusDate((this.DisplayMode == CalendarMode.Month) ? this.CurrentDate : this.DisplayDate);
					}
				}
			}
		}

		// Token: 0x06006041 RID: 24641 RVA: 0x00298918 File Offset: 0x00297918
		private DateTime? GetDateOffset(DateTime date, int offset, CalendarMode displayMode)
		{
			DateTime? result = null;
			switch (displayMode)
			{
			case CalendarMode.Month:
				result = DateTimeHelper.AddMonths(date, offset);
				break;
			case CalendarMode.Year:
				result = DateTimeHelper.AddYears(date, offset);
				break;
			case CalendarMode.Decade:
				result = DateTimeHelper.AddYears(this.DisplayDate, offset * 10);
				break;
			}
			return result;
		}

		// Token: 0x06006042 RID: 24642 RVA: 0x00298968 File Offset: 0x00297968
		private void MoveDisplayTo(DateTime? date)
		{
			if (date != null)
			{
				DateTime date2 = date.Value.Date;
				CalendarMode displayMode = this.DisplayMode;
				if (displayMode != CalendarMode.Month)
				{
					if (displayMode - CalendarMode.Year <= 1)
					{
						base.SetCurrentValueInternal(Calendar.DisplayDateProperty, date2);
						this.UpdateCellItems();
					}
				}
				else
				{
					base.SetCurrentValueInternal(Calendar.DisplayDateProperty, DateTimeHelper.DiscardDayTime(date2));
					this.CurrentDate = date2;
					this.UpdateCellItems();
				}
				this.FocusDate(date2);
			}
		}

		// Token: 0x06006043 RID: 24643 RVA: 0x002989E4 File Offset: 0x002979E4
		internal void OnNextClick()
		{
			DateTime? dateOffset = this.GetDateOffset(this.DisplayDate, 1, this.DisplayMode);
			if (dateOffset != null)
			{
				this.MoveDisplayTo(new DateTime?(DateTimeHelper.DiscardDayTime(dateOffset.Value)));
			}
		}

		// Token: 0x06006044 RID: 24644 RVA: 0x00298A28 File Offset: 0x00297A28
		internal void OnPreviousClick()
		{
			DateTime? dateOffset = this.GetDateOffset(this.DisplayDate, -1, this.DisplayMode);
			if (dateOffset != null)
			{
				this.MoveDisplayTo(new DateTime?(DateTimeHelper.DiscardDayTime(dateOffset.Value)));
			}
		}

		// Token: 0x06006045 RID: 24645 RVA: 0x00298A6C File Offset: 0x00297A6C
		internal void OnSelectedDatesCollectionChanged(SelectionChangedEventArgs e)
		{
			if (Calendar.IsSelectionChanged(e))
			{
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
				{
					CalendarAutomationPeer calendarAutomationPeer = UIElementAutomationPeer.FromElement(this) as CalendarAutomationPeer;
					if (calendarAutomationPeer != null)
					{
						calendarAutomationPeer.RaiseSelectionEvents(e);
					}
				}
				this.CoerceFromSelection();
				this.OnSelectedDatesChanged(e);
			}
		}

		// Token: 0x06006046 RID: 24646 RVA: 0x00298ABC File Offset: 0x00297ABC
		internal void UpdateCellItems()
		{
			CalendarItem monthControl = this.MonthControl;
			if (monthControl != null)
			{
				switch (this.DisplayMode)
				{
				case CalendarMode.Month:
					monthControl.UpdateMonthMode();
					return;
				case CalendarMode.Year:
					monthControl.UpdateYearMode();
					return;
				case CalendarMode.Decade:
					monthControl.UpdateDecadeMode();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x00298B01 File Offset: 0x00297B01
		private void CoerceFromSelection()
		{
			base.CoerceValue(Calendar.DisplayDateStartProperty);
			base.CoerceValue(Calendar.DisplayDateEndProperty);
			base.CoerceValue(Calendar.DisplayDateProperty);
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x00298B24 File Offset: 0x00297B24
		private void AddKeyboardSelection()
		{
			if (this.HoverStart != null)
			{
				this.SelectedDates.ClearInternal();
				this.SelectedDates.AddRange(this.HoverStart.Value, this.CurrentDate);
			}
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x00298B6C File Offset: 0x00297B6C
		private static bool IsSelectionChanged(SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count != e.RemovedItems.Count)
			{
				return true;
			}
			foreach (object obj in e.AddedItems)
			{
				DateTime dateTime = (DateTime)obj;
				if (!e.RemovedItems.Contains(dateTime))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x00298BF4 File Offset: 0x00297BF4
		private static bool IsValidDisplayMode(object value)
		{
			CalendarMode calendarMode = (CalendarMode)value;
			return calendarMode == CalendarMode.Month || calendarMode == CalendarMode.Year || calendarMode == CalendarMode.Decade;
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x00298C18 File Offset: 0x00297C18
		internal static bool IsValidFirstDayOfWeek(object value)
		{
			DayOfWeek dayOfWeek = (DayOfWeek)value;
			return dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Monday || dayOfWeek == DayOfWeek.Tuesday || dayOfWeek == DayOfWeek.Wednesday || dayOfWeek == DayOfWeek.Thursday || dayOfWeek == DayOfWeek.Friday || dayOfWeek == DayOfWeek.Saturday;
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x00298C4C File Offset: 0x00297C4C
		private static bool IsValidKeyboardSelection(Calendar cal, object value)
		{
			return value == null || (!cal.BlackoutDates.Contains((DateTime)value) && DateTime.Compare((DateTime)value, cal.DisplayDateStartInternal) >= 0 && DateTime.Compare((DateTime)value, cal.DisplayDateEndInternal) <= 0);
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x00298CA0 File Offset: 0x00297CA0
		private static bool IsValidSelectionMode(object value)
		{
			CalendarSelectionMode calendarSelectionMode = (CalendarSelectionMode)value;
			return calendarSelectionMode == CalendarSelectionMode.SingleDate || calendarSelectionMode == CalendarSelectionMode.SingleRange || calendarSelectionMode == CalendarSelectionMode.MultipleRange || calendarSelectionMode == CalendarSelectionMode.None;
		}

		// Token: 0x0600604E RID: 24654 RVA: 0x00298CC5 File Offset: 0x00297CC5
		private void OnSelectedMonthChanged(DateTime? selectedMonth)
		{
			if (selectedMonth != null)
			{
				base.SetCurrentValueInternal(Calendar.DisplayDateProperty, selectedMonth.Value);
				this.UpdateCellItems();
				this.FocusDate(selectedMonth.Value);
			}
		}

		// Token: 0x0600604F RID: 24655 RVA: 0x00298CC5 File Offset: 0x00297CC5
		private void OnSelectedYearChanged(DateTime? selectedYear)
		{
			if (selectedYear != null)
			{
				base.SetCurrentValueInternal(Calendar.DisplayDateProperty, selectedYear.Value);
				this.UpdateCellItems();
				this.FocusDate(selectedYear.Value);
			}
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x00298CFA File Offset: 0x00297CFA
		internal void FocusDate(DateTime date)
		{
			if (this.MonthControl != null)
			{
				this.MonthControl.FocusDate(date);
			}
		}

		// Token: 0x06006051 RID: 24657 RVA: 0x00298D10 File Offset: 0x00297D10
		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			Calendar calendar = (Calendar)sender;
			if (!e.Handled && e.OriginalSource == calendar)
			{
				if (calendar.SelectedDate != null && DateTimeHelper.CompareYearMonth(calendar.SelectedDate.Value, calendar.DisplayDateInternal) == 0)
				{
					calendar.FocusDate(calendar.SelectedDate.Value);
				}
				else
				{
					calendar.FocusDate(calendar.DisplayDate);
				}
				e.Handled = true;
			}
		}

		// Token: 0x06006052 RID: 24658 RVA: 0x00298D8C File Offset: 0x00297D8C
		private bool ProcessCalendarKey(KeyEventArgs e)
		{
			if (this.DisplayMode == CalendarMode.Month)
			{
				CalendarDayButton calendarDayButton = (this.MonthControl != null) ? this.MonthControl.GetCalendarDayButton(this.CurrentDate) : null;
				if (DateTimeHelper.CompareYearMonth(this.CurrentDate, this.DisplayDateInternal) != 0 && calendarDayButton != null && !calendarDayButton.IsInactive)
				{
					return false;
				}
			}
			bool ctrl;
			bool shift;
			CalendarKeyboardHelper.GetMetaKeyState(out ctrl, out shift);
			Key key = e.Key;
			if (key != Key.Return)
			{
				switch (key)
				{
				case Key.Space:
					break;
				case Key.Prior:
					this.ProcessPageUpKey(shift);
					return true;
				case Key.Next:
					this.ProcessPageDownKey(shift);
					return true;
				case Key.End:
					this.ProcessEndKey(shift);
					return true;
				case Key.Home:
					this.ProcessHomeKey(shift);
					return true;
				case Key.Left:
					this.ProcessLeftKey(shift);
					return true;
				case Key.Up:
					this.ProcessUpKey(ctrl, shift);
					return true;
				case Key.Right:
					this.ProcessRightKey(shift);
					return true;
				case Key.Down:
					this.ProcessDownKey(ctrl, shift);
					return true;
				default:
					return false;
				}
			}
			return this.ProcessEnterKey();
		}

		// Token: 0x06006053 RID: 24659 RVA: 0x00298E74 File Offset: 0x00297E74
		private void ProcessDownKey(bool ctrl, bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
				if (!ctrl || shift)
				{
					DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddDays(this.CurrentDate, 7), 1);
					this.ProcessSelection(shift, nonBlackoutDate);
					return;
				}
				break;
			case CalendarMode.Year:
			{
				if (ctrl)
				{
					base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Month);
					this.FocusDate(this.DisplayDate);
					return;
				}
				DateTime? selectedMonth = DateTimeHelper.AddMonths(this.DisplayDate, 4);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				if (ctrl)
				{
					base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Year);
					this.FocusDate(this.DisplayDate);
					return;
				}
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, 4);
				this.OnSelectedYearChanged(selectedYear);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06006054 RID: 24660 RVA: 0x00298F34 File Offset: 0x00297F34
		private void ProcessEndKey(bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? lastSelectedDate = new DateTime?(new DateTime(this.DisplayDateInternal.Year, this.DisplayDateInternal.Month, 1));
				if (DateTimeHelper.CompareYearMonth(DateTime.MaxValue, lastSelectedDate.Value) > 0)
				{
					lastSelectedDate = new DateTime?(DateTimeHelper.AddMonths(lastSelectedDate.Value, 1).Value);
					lastSelectedDate = new DateTime?(DateTimeHelper.AddDays(lastSelectedDate.Value, -1).Value);
				}
				else
				{
					lastSelectedDate = new DateTime?(DateTime.MaxValue);
				}
				this.ProcessSelection(shift, lastSelectedDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime value = new DateTime(this.DisplayDate.Year, 12, 1);
				this.OnSelectedMonthChanged(new DateTime?(value));
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = new DateTime?(new DateTime(DateTimeHelper.EndOfDecade(this.DisplayDate), 1, 1));
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06006055 RID: 24661 RVA: 0x00299030 File Offset: 0x00298030
		private bool ProcessEnterKey()
		{
			CalendarMode displayMode = this.DisplayMode;
			if (displayMode == CalendarMode.Year)
			{
				base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Month);
				this.FocusDate(this.DisplayDate);
				return true;
			}
			if (displayMode != CalendarMode.Decade)
			{
				return false;
			}
			base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Year);
			this.FocusDate(this.DisplayDate);
			return true;
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x00299090 File Offset: 0x00298090
		private void ProcessHomeKey(bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? lastSelectedDate = new DateTime?(new DateTime(this.DisplayDateInternal.Year, this.DisplayDateInternal.Month, 1));
				this.ProcessSelection(shift, lastSelectedDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime value = new DateTime(this.DisplayDate.Year, 1, 1);
				this.OnSelectedMonthChanged(new DateTime?(value));
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = new DateTime?(new DateTime(DateTimeHelper.DecadeOfDate(this.DisplayDate), 1, 1));
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06006057 RID: 24663 RVA: 0x00299130 File Offset: 0x00298130
		private void ProcessLeftKey(bool shift)
		{
			int num = (!base.IsRightToLeft) ? -1 : 1;
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddDays(this.CurrentDate, num), num);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime? selectedMonth = DateTimeHelper.AddMonths(this.DisplayDate, num);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, num);
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06006058 RID: 24664 RVA: 0x002991B4 File Offset: 0x002981B4
		private void ProcessPageDownKey(bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddMonths(this.CurrentDate, 1), 1);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime? selectedMonth = DateTimeHelper.AddYears(this.DisplayDate, 1);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, 10);
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06006059 RID: 24665 RVA: 0x00299228 File Offset: 0x00298228
		private void ProcessPageUpKey(bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddMonths(this.CurrentDate, -1), -1);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime? selectedMonth = DateTimeHelper.AddYears(this.DisplayDate, -1);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, -10);
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x0600605A RID: 24666 RVA: 0x0029929C File Offset: 0x0029829C
		private void ProcessRightKey(bool shift)
		{
			int num = (!base.IsRightToLeft) ? 1 : -1;
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddDays(this.CurrentDate, num), num);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime? selectedMonth = DateTimeHelper.AddMonths(this.DisplayDate, num);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, num);
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x0600605B RID: 24667 RVA: 0x00299320 File Offset: 0x00298320
		private void ProcessSelection(bool shift, DateTime? lastSelectedDate)
		{
			if (this.SelectionMode == CalendarSelectionMode.None && lastSelectedDate != null)
			{
				this.OnDayClick(lastSelectedDate.Value);
				return;
			}
			if (lastSelectedDate != null && Calendar.IsValidKeyboardSelection(this, lastSelectedDate.Value))
			{
				if (this.SelectionMode == CalendarSelectionMode.SingleRange || this.SelectionMode == CalendarSelectionMode.MultipleRange)
				{
					this.SelectedDates.ClearInternal();
					if (shift)
					{
						this._isShiftPressed = true;
						DateTime? dateTime = this.HoverStart;
						if (dateTime == null)
						{
							dateTime = new DateTime?(this.CurrentDate);
							this.HoverEnd = dateTime;
							this.HoverStart = dateTime;
						}
						dateTime = this.HoverStart;
						CalendarDateRange range;
						if (DateTime.Compare(dateTime.Value, lastSelectedDate.Value) < 0)
						{
							dateTime = this.HoverStart;
							range = new CalendarDateRange(dateTime.Value, lastSelectedDate.Value);
						}
						else
						{
							DateTime value = lastSelectedDate.Value;
							dateTime = this.HoverStart;
							range = new CalendarDateRange(value, dateTime.Value);
						}
						if (!this.BlackoutDates.ContainsAny(range))
						{
							this._currentDate = lastSelectedDate;
							this.HoverEnd = lastSelectedDate;
						}
						this.OnDayClick(this.CurrentDate);
					}
					else
					{
						DateTime? dateTime = new DateTime?(this.CurrentDate = lastSelectedDate.Value);
						this.HoverEnd = dateTime;
						this.HoverStart = dateTime;
						this.AddKeyboardSelection();
						this.OnDayClick(lastSelectedDate.Value);
					}
				}
				else
				{
					this.CurrentDate = lastSelectedDate.Value;
					DateTime? dateTime = null;
					this.HoverEnd = dateTime;
					this.HoverStart = dateTime;
					if (this.SelectedDates.Count > 0)
					{
						this.SelectedDates[0] = lastSelectedDate.Value;
					}
					else
					{
						this.SelectedDates.Add(lastSelectedDate.Value);
					}
					this.OnDayClick(lastSelectedDate.Value);
				}
				this.UpdateCellItems();
			}
		}

		// Token: 0x0600605C RID: 24668 RVA: 0x002994F0 File Offset: 0x002984F0
		private void ProcessShiftKeyUp()
		{
			if (this._isShiftPressed && (this.SelectionMode == CalendarSelectionMode.SingleRange || this.SelectionMode == CalendarSelectionMode.MultipleRange))
			{
				this.AddKeyboardSelection();
				this._isShiftPressed = false;
				DateTime? dateTime = null;
				this.HoverEnd = dateTime;
				this.HoverStart = dateTime;
			}
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x0029953C File Offset: 0x0029853C
		private void ProcessUpKey(bool ctrl, bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				if (ctrl)
				{
					base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Year);
					this.FocusDate(this.DisplayDate);
					return;
				}
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddDays(this.CurrentDate, -7), -1);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				if (ctrl)
				{
					base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Decade);
					this.FocusDate(this.DisplayDate);
					return;
				}
				DateTime? selectedMonth = DateTimeHelper.AddMonths(this.DisplayDate, -4);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
				if (!ctrl)
				{
					DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, -4);
					this.OnSelectedYearChanged(selectedYear);
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x040031FC RID: 12796
		private const string ElementRoot = "PART_Root";

		// Token: 0x040031FD RID: 12797
		private const string ElementMonth = "PART_CalendarItem";

		// Token: 0x040031FE RID: 12798
		private const int COLS = 7;

		// Token: 0x040031FF RID: 12799
		private const int ROWS = 7;

		// Token: 0x04003200 RID: 12800
		private const int YEAR_ROWS = 3;

		// Token: 0x04003201 RID: 12801
		private const int YEAR_COLS = 4;

		// Token: 0x04003202 RID: 12802
		private const int YEARS_PER_DECADE = 10;

		// Token: 0x04003203 RID: 12803
		private DateTime? _hoverStart;

		// Token: 0x04003204 RID: 12804
		private DateTime? _hoverEnd;

		// Token: 0x04003205 RID: 12805
		private bool _isShiftPressed;

		// Token: 0x04003206 RID: 12806
		private DateTime? _currentDate;

		// Token: 0x04003207 RID: 12807
		private CalendarItem _monthControl;

		// Token: 0x04003208 RID: 12808
		private CalendarBlackoutDatesCollection _blackoutDates;

		// Token: 0x04003209 RID: 12809
		private SelectedDatesCollection _selectedDates;

		// Token: 0x0400320E RID: 12814
		public static readonly DependencyProperty CalendarButtonStyleProperty;

		// Token: 0x0400320F RID: 12815
		public static readonly DependencyProperty CalendarDayButtonStyleProperty;

		// Token: 0x04003210 RID: 12816
		public static readonly DependencyProperty CalendarItemStyleProperty;

		// Token: 0x04003211 RID: 12817
		public static readonly DependencyProperty DisplayDateProperty;

		// Token: 0x04003212 RID: 12818
		public static readonly DependencyProperty DisplayDateEndProperty;

		// Token: 0x04003213 RID: 12819
		public static readonly DependencyProperty DisplayDateStartProperty;

		// Token: 0x04003214 RID: 12820
		public static readonly DependencyProperty DisplayModeProperty;

		// Token: 0x04003215 RID: 12821
		public static readonly DependencyProperty FirstDayOfWeekProperty;

		// Token: 0x04003216 RID: 12822
		public static readonly DependencyProperty IsTodayHighlightedProperty;

		// Token: 0x04003217 RID: 12823
		public static readonly DependencyProperty SelectedDateProperty;

		// Token: 0x04003218 RID: 12824
		public static readonly DependencyProperty SelectionModeProperty;
	}
}
