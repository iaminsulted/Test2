using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x0200076D RID: 1901
	[TemplatePart(Name = "PART_Root", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_TextBox", Type = typeof(DatePickerTextBox))]
	[TemplatePart(Name = "PART_Button", Type = typeof(Button))]
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	public class DatePicker : Control
	{
		// Token: 0x1400010A RID: 266
		// (add) Token: 0x06006730 RID: 26416 RVA: 0x002B3FE8 File Offset: 0x002B2FE8
		// (remove) Token: 0x06006731 RID: 26417 RVA: 0x002B4020 File Offset: 0x002B3020
		public event RoutedEventHandler CalendarClosed;

		// Token: 0x1400010B RID: 267
		// (add) Token: 0x06006732 RID: 26418 RVA: 0x002B4058 File Offset: 0x002B3058
		// (remove) Token: 0x06006733 RID: 26419 RVA: 0x002B4090 File Offset: 0x002B3090
		public event RoutedEventHandler CalendarOpened;

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x06006734 RID: 26420 RVA: 0x002B40C8 File Offset: 0x002B30C8
		// (remove) Token: 0x06006735 RID: 26421 RVA: 0x002B4100 File Offset: 0x002B3100
		public event EventHandler<DatePickerDateValidationErrorEventArgs> DateValidationError;

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x06006736 RID: 26422 RVA: 0x002B4135 File Offset: 0x002B3135
		// (remove) Token: 0x06006737 RID: 26423 RVA: 0x002B4143 File Offset: 0x002B3143
		public event EventHandler<SelectionChangedEventArgs> SelectedDateChanged
		{
			add
			{
				base.AddHandler(DatePicker.SelectedDateChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DatePicker.SelectedDateChangedEvent, value);
			}
		}

		// Token: 0x06006738 RID: 26424 RVA: 0x002B4154 File Offset: 0x002B3154
		static DatePicker()
		{
			DatePicker.SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Direct, typeof(EventHandler<SelectionChangedEventArgs>), typeof(DatePicker));
			DatePicker.CalendarStyleProperty = DependencyProperty.Register("CalendarStyle", typeof(Style), typeof(DatePicker));
			DatePicker.DisplayDateProperty = DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(DatePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, new CoerceValueCallback(DatePicker.CoerceDisplayDate)));
			DatePicker.DisplayDateEndProperty = DependencyProperty.Register("DisplayDateEnd", typeof(DateTime?), typeof(DatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DatePicker.OnDisplayDateEndChanged), new CoerceValueCallback(DatePicker.CoerceDisplayDateEnd)));
			DatePicker.DisplayDateStartProperty = DependencyProperty.Register("DisplayDateStart", typeof(DateTime?), typeof(DatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DatePicker.OnDisplayDateStartChanged), new CoerceValueCallback(DatePicker.CoerceDisplayDateStart)));
			DatePicker.FirstDayOfWeekProperty = DependencyProperty.Register("FirstDayOfWeek", typeof(DayOfWeek), typeof(DatePicker), null, new ValidateValueCallback(Calendar.IsValidFirstDayOfWeek));
			DatePicker.IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DatePicker), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DatePicker.OnIsDropDownOpenChanged), new CoerceValueCallback(DatePicker.OnCoerceIsDropDownOpen)));
			DatePicker.IsTodayHighlightedProperty = DependencyProperty.Register("IsTodayHighlighted", typeof(bool), typeof(DatePicker));
			DatePicker.SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DatePicker.OnSelectedDateChanged), new CoerceValueCallback(DatePicker.CoerceSelectedDate)));
			DatePicker.SelectedDateFormatProperty = DependencyProperty.Register("SelectedDateFormat", typeof(DatePickerFormat), typeof(DatePicker), new FrameworkPropertyMetadata(DatePickerFormat.Long, new PropertyChangedCallback(DatePicker.OnSelectedDateFormatChanged)), new ValidateValueCallback(DatePicker.IsValidSelectedDateFormat));
			DatePicker.TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(DatePicker), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(DatePicker.OnTextChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePicker), new FrameworkPropertyMetadata(typeof(DatePicker)));
			EventManager.RegisterClassHandler(typeof(DatePicker), UIElement.GotFocusEvent, new RoutedEventHandler(DatePicker.OnGotFocus));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DatePicker), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(DatePicker), new FrameworkPropertyMetadata(false));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(DatePicker), new UIPropertyMetadata(new PropertyChangedCallback(DatePicker.OnIsEnabledChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.DatePicker);
		}

		// Token: 0x06006739 RID: 26425 RVA: 0x002B448C File Offset: 0x002B348C
		public DatePicker()
		{
			this.InitializeCalendar();
			this._defaultText = string.Empty;
			base.SetCurrentValueInternal(DatePicker.FirstDayOfWeekProperty, DateTimeHelper.GetCurrentDateFormat().FirstDayOfWeek);
			base.SetCurrentValueInternal(DatePicker.DisplayDateProperty, DateTime.Today);
		}

		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x0600673A RID: 26426 RVA: 0x002B44DF File Offset: 0x002B34DF
		public CalendarBlackoutDatesCollection BlackoutDates
		{
			get
			{
				return this._calendar.BlackoutDates;
			}
		}

		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x0600673B RID: 26427 RVA: 0x002B44EC File Offset: 0x002B34EC
		// (set) Token: 0x0600673C RID: 26428 RVA: 0x002B44FE File Offset: 0x002B34FE
		public Style CalendarStyle
		{
			get
			{
				return (Style)base.GetValue(DatePicker.CalendarStyleProperty);
			}
			set
			{
				base.SetValue(DatePicker.CalendarStyleProperty, value);
			}
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x0600673D RID: 26429 RVA: 0x002B450C File Offset: 0x002B350C
		// (set) Token: 0x0600673E RID: 26430 RVA: 0x002B451E File Offset: 0x002B351E
		public DateTime DisplayDate
		{
			get
			{
				return (DateTime)base.GetValue(DatePicker.DisplayDateProperty);
			}
			set
			{
				base.SetValue(DatePicker.DisplayDateProperty, value);
			}
		}

		// Token: 0x0600673F RID: 26431 RVA: 0x002B4531 File Offset: 0x002B3531
		private static object CoerceDisplayDate(DependencyObject d, object value)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker._calendar.DisplayDate = (DateTime)value;
			return datePicker._calendar.DisplayDate;
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x06006740 RID: 26432 RVA: 0x002B4559 File Offset: 0x002B3559
		// (set) Token: 0x06006741 RID: 26433 RVA: 0x002B456B File Offset: 0x002B356B
		public DateTime? DisplayDateEnd
		{
			get
			{
				return (DateTime?)base.GetValue(DatePicker.DisplayDateEndProperty);
			}
			set
			{
				base.SetValue(DatePicker.DisplayDateEndProperty, value);
			}
		}

		// Token: 0x06006742 RID: 26434 RVA: 0x002B457E File Offset: 0x002B357E
		private static void OnDisplayDateEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DatePicker).CoerceValue(DatePicker.DisplayDateProperty);
		}

		// Token: 0x06006743 RID: 26435 RVA: 0x002B4590 File Offset: 0x002B3590
		private static object CoerceDisplayDateEnd(DependencyObject d, object value)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker._calendar.DisplayDateEnd = (DateTime?)value;
			return datePicker._calendar.DisplayDateEnd;
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x06006744 RID: 26436 RVA: 0x002B45B8 File Offset: 0x002B35B8
		// (set) Token: 0x06006745 RID: 26437 RVA: 0x002B45CA File Offset: 0x002B35CA
		public DateTime? DisplayDateStart
		{
			get
			{
				return (DateTime?)base.GetValue(DatePicker.DisplayDateStartProperty);
			}
			set
			{
				base.SetValue(DatePicker.DisplayDateStartProperty, value);
			}
		}

		// Token: 0x06006746 RID: 26438 RVA: 0x002B45DD File Offset: 0x002B35DD
		private static void OnDisplayDateStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker.CoerceValue(DatePicker.DisplayDateEndProperty);
			datePicker.CoerceValue(DatePicker.DisplayDateProperty);
		}

		// Token: 0x06006747 RID: 26439 RVA: 0x002B45FA File Offset: 0x002B35FA
		private static object CoerceDisplayDateStart(DependencyObject d, object value)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker._calendar.DisplayDateStart = (DateTime?)value;
			return datePicker._calendar.DisplayDateStart;
		}

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x06006748 RID: 26440 RVA: 0x002B4622 File Offset: 0x002B3622
		// (set) Token: 0x06006749 RID: 26441 RVA: 0x002B4634 File Offset: 0x002B3634
		public DayOfWeek FirstDayOfWeek
		{
			get
			{
				return (DayOfWeek)base.GetValue(DatePicker.FirstDayOfWeekProperty);
			}
			set
			{
				base.SetValue(DatePicker.FirstDayOfWeekProperty, value);
			}
		}

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x0600674A RID: 26442 RVA: 0x002B4647 File Offset: 0x002B3647
		// (set) Token: 0x0600674B RID: 26443 RVA: 0x002B4659 File Offset: 0x002B3659
		public bool IsDropDownOpen
		{
			get
			{
				return (bool)base.GetValue(DatePicker.IsDropDownOpenProperty);
			}
			set
			{
				base.SetValue(DatePicker.IsDropDownOpenProperty, value);
			}
		}

		// Token: 0x0600674C RID: 26444 RVA: 0x002B4667 File Offset: 0x002B3667
		private static object OnCoerceIsDropDownOpen(DependencyObject d, object baseValue)
		{
			if (!(d as DatePicker).IsEnabled)
			{
				return false;
			}
			return baseValue;
		}

		// Token: 0x0600674D RID: 26445 RVA: 0x002B4680 File Offset: 0x002B3680
		private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker dp = d as DatePicker;
			bool flag = (bool)e.NewValue;
			if (dp._popUp != null && dp._popUp.IsOpen != flag)
			{
				dp._popUp.IsOpen = flag;
				if (flag)
				{
					dp._originalSelectedDate = dp.SelectedDate;
					dp.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate()
					{
						dp._calendar.Focus();
					}));
				}
			}
		}

		// Token: 0x0600674E RID: 26446 RVA: 0x002B4714 File Offset: 0x002B3714
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DatePicker).CoerceValue(DatePicker.IsDropDownOpenProperty);
			Control.OnVisualStatePropertyChanged(d, e);
		}

		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x0600674F RID: 26447 RVA: 0x002B472D File Offset: 0x002B372D
		// (set) Token: 0x06006750 RID: 26448 RVA: 0x002B473F File Offset: 0x002B373F
		public bool IsTodayHighlighted
		{
			get
			{
				return (bool)base.GetValue(DatePicker.IsTodayHighlightedProperty);
			}
			set
			{
				base.SetValue(DatePicker.IsTodayHighlightedProperty, value);
			}
		}

		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x06006751 RID: 26449 RVA: 0x002B474D File Offset: 0x002B374D
		// (set) Token: 0x06006752 RID: 26450 RVA: 0x002B475F File Offset: 0x002B375F
		public DateTime? SelectedDate
		{
			get
			{
				return (DateTime?)base.GetValue(DatePicker.SelectedDateProperty);
			}
			set
			{
				base.SetValue(DatePicker.SelectedDateProperty, value);
			}
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x002B4774 File Offset: 0x002B3774
		private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			Collection<DateTime> collection = new Collection<DateTime>();
			Collection<DateTime> collection2 = new Collection<DateTime>();
			datePicker.CoerceValue(DatePicker.DisplayDateStartProperty);
			datePicker.CoerceValue(DatePicker.DisplayDateEndProperty);
			datePicker.CoerceValue(DatePicker.DisplayDateProperty);
			DateTime? dateTime = (DateTime?)e.NewValue;
			DateTime? dateTime2 = (DateTime?)e.OldValue;
			if (datePicker.SelectedDate != null)
			{
				DateTime value = datePicker.SelectedDate.Value;
				datePicker.SetTextInternal(datePicker.DateTimeToString(value));
				if ((value.Month != datePicker.DisplayDate.Month || value.Year != datePicker.DisplayDate.Year) && !datePicker._calendar.DatePickerDisplayDateFlag)
				{
					datePicker.SetCurrentValueInternal(DatePicker.DisplayDateProperty, value);
				}
				datePicker._calendar.DatePickerDisplayDateFlag = false;
			}
			else
			{
				datePicker.SetWaterMarkText();
			}
			if (dateTime != null)
			{
				collection.Add(dateTime.Value);
			}
			if (dateTime2 != null)
			{
				collection2.Add(dateTime2.Value);
			}
			datePicker.OnSelectedDateChanged(new CalendarSelectionChangedEventArgs(DatePicker.SelectedDateChangedEvent, collection2, collection));
			DatePickerAutomationPeer datePickerAutomationPeer = UIElementAutomationPeer.FromElement(datePicker) as DatePickerAutomationPeer;
			if (datePickerAutomationPeer != null)
			{
				string newValue = (dateTime != null) ? datePicker.DateTimeToString(dateTime.Value) : "";
				string oldValue = (dateTime2 != null) ? datePicker.DateTimeToString(dateTime2.Value) : "";
				datePickerAutomationPeer.RaiseValuePropertyChangedEvent(oldValue, newValue);
			}
		}

		// Token: 0x06006754 RID: 26452 RVA: 0x002B48FB File Offset: 0x002B38FB
		private static object CoerceSelectedDate(DependencyObject d, object value)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker._calendar.SelectedDate = (DateTime?)value;
			return datePicker._calendar.SelectedDate;
		}

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x06006755 RID: 26453 RVA: 0x002B4923 File Offset: 0x002B3923
		// (set) Token: 0x06006756 RID: 26454 RVA: 0x002B4935 File Offset: 0x002B3935
		public DatePickerFormat SelectedDateFormat
		{
			get
			{
				return (DatePickerFormat)base.GetValue(DatePicker.SelectedDateFormatProperty);
			}
			set
			{
				base.SetValue(DatePicker.SelectedDateFormatProperty, value);
			}
		}

		// Token: 0x06006757 RID: 26455 RVA: 0x002B4948 File Offset: 0x002B3948
		private static void OnSelectedDateFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			if (datePicker._textBox != null)
			{
				if (string.IsNullOrEmpty(datePicker._textBox.Text))
				{
					datePicker.SetWaterMarkText();
					return;
				}
				DateTime? dateTime = datePicker.ParseText(datePicker._textBox.Text);
				if (dateTime != null)
				{
					datePicker.SetTextInternal(datePicker.DateTimeToString(dateTime.Value));
				}
			}
		}

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x06006758 RID: 26456 RVA: 0x002B49AB File Offset: 0x002B39AB
		// (set) Token: 0x06006759 RID: 26457 RVA: 0x002B49BD File Offset: 0x002B39BD
		public string Text
		{
			get
			{
				return (string)base.GetValue(DatePicker.TextProperty);
			}
			set
			{
				base.SetValue(DatePicker.TextProperty, value);
			}
		}

		// Token: 0x0600675A RID: 26458 RVA: 0x002B49CC File Offset: 0x002B39CC
		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			if (!datePicker.IsHandlerSuspended(DatePicker.TextProperty))
			{
				string text = e.NewValue as string;
				if (text != null)
				{
					if (datePicker._textBox != null)
					{
						datePicker._textBox.Text = text;
					}
					else
					{
						datePicker._defaultText = text;
					}
					datePicker.SetSelectedDate();
					return;
				}
				datePicker.SetValueNoCallback(DatePicker.SelectedDateProperty, null);
			}
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x002B4A2D File Offset: 0x002B3A2D
		private void SetTextInternal(string value)
		{
			base.SetCurrentValueInternal(DatePicker.TextProperty, value);
		}

		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x0600675C RID: 26460 RVA: 0x002B4A3B File Offset: 0x002B3A3B
		internal Calendar Calendar
		{
			get
			{
				return this._calendar;
			}
		}

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x0600675D RID: 26461 RVA: 0x002B4A43 File Offset: 0x002B3A43
		internal TextBox TextBox
		{
			get
			{
				return this._textBox;
			}
		}

		// Token: 0x0600675E RID: 26462 RVA: 0x002B4A4C File Offset: 0x002B3A4C
		public override void OnApplyTemplate()
		{
			if (this._popUp != null)
			{
				this._popUp.RemoveHandler(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(this.PopUp_PreviewMouseLeftButtonDown));
				this._popUp.Opened -= this.PopUp_Opened;
				this._popUp.Closed -= this.PopUp_Closed;
				this._popUp.Child = null;
			}
			if (this._dropDownButton != null)
			{
				this._dropDownButton.Click -= this.DropDownButton_Click;
				this._dropDownButton.RemoveHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(this.DropDownButton_MouseLeave));
			}
			if (this._textBox != null)
			{
				this._textBox.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.TextBox_KeyDown));
				this._textBox.RemoveHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(this.TextBox_TextChanged));
				this._textBox.RemoveHandler(UIElement.LostFocusEvent, new RoutedEventHandler(this.TextBox_LostFocus));
			}
			base.OnApplyTemplate();
			this._popUp = (base.GetTemplateChild("PART_Popup") as Popup);
			if (this._popUp != null)
			{
				this._popUp.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(this.PopUp_PreviewMouseLeftButtonDown));
				this._popUp.Opened += this.PopUp_Opened;
				this._popUp.Closed += this.PopUp_Closed;
				this._popUp.Child = this._calendar;
				if (this.IsDropDownOpen)
				{
					this._popUp.IsOpen = true;
				}
			}
			this._dropDownButton = (base.GetTemplateChild("PART_Button") as Button);
			if (this._dropDownButton != null)
			{
				this._dropDownButton.Click += this.DropDownButton_Click;
				this._dropDownButton.AddHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(this.DropDownButton_MouseLeave), true);
				if (this._dropDownButton.Content == null)
				{
					this._dropDownButton.Content = SR.Get("DatePicker_DropDownButtonName");
				}
			}
			this._textBox = (base.GetTemplateChild("PART_TextBox") as DatePickerTextBox);
			if (this.SelectedDate == null)
			{
				this.SetWaterMarkText();
			}
			if (this._textBox != null)
			{
				this._textBox.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.TextBox_KeyDown), true);
				this._textBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(this.TextBox_TextChanged), true);
				this._textBox.AddHandler(UIElement.LostFocusEvent, new RoutedEventHandler(this.TextBox_LostFocus), true);
				if (this.SelectedDate == null)
				{
					if (!string.IsNullOrEmpty(this._defaultText))
					{
						this._textBox.Text = this._defaultText;
						this.SetSelectedDate();
						return;
					}
				}
				else
				{
					this._textBox.Text = this.DateTimeToString(this.SelectedDate.Value);
				}
			}
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x002B4D38 File Offset: 0x002B3D38
		public override string ToString()
		{
			if (this.SelectedDate != null)
			{
				return this.SelectedDate.Value.ToString(DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)));
			}
			return string.Empty;
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x002B4D7C File Offset: 0x002B3D7C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x002B4DB9 File Offset: 0x002B3DB9
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DatePickerAutomationPeer(this);
		}

		// Token: 0x06006762 RID: 26466 RVA: 0x002B4DC4 File Offset: 0x002B3DC4
		protected virtual void OnCalendarClosed(RoutedEventArgs e)
		{
			RoutedEventHandler calendarClosed = this.CalendarClosed;
			if (calendarClosed != null)
			{
				calendarClosed(this, e);
			}
		}

		// Token: 0x06006763 RID: 26467 RVA: 0x002B4DE4 File Offset: 0x002B3DE4
		protected virtual void OnCalendarOpened(RoutedEventArgs e)
		{
			RoutedEventHandler calendarOpened = this.CalendarOpened;
			if (calendarOpened != null)
			{
				calendarOpened(this, e);
			}
		}

		// Token: 0x06006764 RID: 26468 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSelectedDateChanged(SelectionChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x002B4E04 File Offset: 0x002B3E04
		protected virtual void OnDateValidationError(DatePickerDateValidationErrorEventArgs e)
		{
			EventHandler<DatePickerDateValidationErrorEventArgs> dateValidationError = this.DateValidationError;
			if (dateValidationError != null)
			{
				dateValidationError(this, e);
			}
		}

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x06006766 RID: 26470 RVA: 0x002B4E23 File Offset: 0x002B3E23
		protected internal override bool HasEffectiveKeyboardFocus
		{
			get
			{
				if (this._textBox != null)
				{
					return this._textBox.HasEffectiveKeyboardFocus;
				}
				return base.HasEffectiveKeyboardFocus;
			}
		}

		// Token: 0x06006767 RID: 26471 RVA: 0x002B4E40 File Offset: 0x002B3E40
		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			DatePicker datePicker = (DatePicker)sender;
			if (!e.Handled && datePicker._textBox != null)
			{
				if (e.OriginalSource == datePicker)
				{
					datePicker._textBox.Focus();
					e.Handled = true;
					return;
				}
				if (e.OriginalSource == datePicker._textBox)
				{
					datePicker._textBox.SelectAll();
					e.Handled = true;
				}
			}
		}

		// Token: 0x06006768 RID: 26472 RVA: 0x002B4EA4 File Offset: 0x002B3EA4
		private void SetValueNoCallback(DependencyProperty property, object value)
		{
			this.SetIsHandlerSuspended(property, true);
			try
			{
				base.SetCurrentValue(property, value);
			}
			finally
			{
				this.SetIsHandlerSuspended(property, false);
			}
		}

		// Token: 0x06006769 RID: 26473 RVA: 0x002B4EDC File Offset: 0x002B3EDC
		private bool IsHandlerSuspended(DependencyProperty property)
		{
			return this._isHandlerSuspended != null && this._isHandlerSuspended.ContainsKey(property);
		}

		// Token: 0x0600676A RID: 26474 RVA: 0x002B4EF4 File Offset: 0x002B3EF4
		private void SetIsHandlerSuspended(DependencyProperty property, bool value)
		{
			if (value)
			{
				if (this._isHandlerSuspended == null)
				{
					this._isHandlerSuspended = new Dictionary<DependencyProperty, bool>(2);
				}
				this._isHandlerSuspended[property] = true;
				return;
			}
			if (this._isHandlerSuspended != null)
			{
				this._isHandlerSuspended.Remove(property);
			}
		}

		// Token: 0x0600676B RID: 26475 RVA: 0x002B4F30 File Offset: 0x002B3F30
		private void PopUp_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Popup popup = sender as Popup;
			if (popup != null && !popup.StaysOpen && this._dropDownButton != null && this._dropDownButton.InputHitTest(e.GetPosition(this._dropDownButton)) != null)
			{
				this._disablePopupReopen = true;
			}
		}

		// Token: 0x0600676C RID: 26476 RVA: 0x002B4F78 File Offset: 0x002B3F78
		private void PopUp_Opened(object sender, EventArgs e)
		{
			if (!this.IsDropDownOpen)
			{
				base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.TrueBox);
			}
			if (this._calendar != null)
			{
				this._calendar.DisplayMode = CalendarMode.Month;
				this._calendar.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
			}
			this.OnCalendarOpened(new RoutedEventArgs());
		}

		// Token: 0x0600676D RID: 26477 RVA: 0x002B4FCE File Offset: 0x002B3FCE
		private void PopUp_Closed(object sender, EventArgs e)
		{
			if (this.IsDropDownOpen)
			{
				base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
			}
			if (this._calendar.IsKeyboardFocusWithin)
			{
				this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
			}
			this.OnCalendarClosed(new RoutedEventArgs());
		}

		// Token: 0x0600676E RID: 26478 RVA: 0x002B500D File Offset: 0x002B400D
		private void Calendar_DayButtonMouseUp(object sender, MouseButtonEventArgs e)
		{
			base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x0600676F RID: 26479 RVA: 0x002B5020 File Offset: 0x002B4020
		private void CalendarDayOrMonthButton_PreviewKeyDown(object sender, RoutedEventArgs e)
		{
			Calendar calendar = sender as Calendar;
			KeyEventArgs keyEventArgs = (KeyEventArgs)e;
			if (keyEventArgs.Key == Key.Escape || ((keyEventArgs.Key == Key.Return || keyEventArgs.Key == Key.Space) && calendar.DisplayMode == CalendarMode.Month))
			{
				base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
				if (keyEventArgs.Key == Key.Escape)
				{
					base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, this._originalSelectedDate);
				}
			}
		}

		// Token: 0x06006770 RID: 26480 RVA: 0x002B5090 File Offset: 0x002B4090
		private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
		{
			DateTime? addedDate = e.AddedDate;
			DateTime displayDate = this.DisplayDate;
			if (addedDate == null || (addedDate != null && addedDate.GetValueOrDefault() != displayDate))
			{
				base.SetCurrentValueInternal(DatePicker.DisplayDateProperty, e.AddedDate.Value);
			}
		}

		// Token: 0x06006771 RID: 26481 RVA: 0x002B50F0 File Offset: 0x002B40F0
		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0 && this.SelectedDate != null && DateTime.Compare((DateTime)e.AddedItems[0], this.SelectedDate.Value) != 0)
			{
				base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, (DateTime?)e.AddedItems[0]);
				return;
			}
			if (e.AddedItems.Count == 0)
			{
				base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, null);
				return;
			}
			if (this.SelectedDate == null && e.AddedItems.Count > 0)
			{
				base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, (DateTime?)e.AddedItems[0]);
			}
		}

		// Token: 0x06006772 RID: 26482 RVA: 0x002B51BC File Offset: 0x002B41BC
		private string DateTimeToString(DateTime d)
		{
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this));
			DatePickerFormat selectedDateFormat = this.SelectedDateFormat;
			if (selectedDateFormat == DatePickerFormat.Long)
			{
				return string.Format(CultureInfo.CurrentCulture, d.ToString(dateFormat.LongDatePattern, dateFormat), Array.Empty<object>());
			}
			if (selectedDateFormat == DatePickerFormat.Short)
			{
				return string.Format(CultureInfo.CurrentCulture, d.ToString(dateFormat.ShortDatePattern, dateFormat), Array.Empty<object>());
			}
			return null;
		}

		// Token: 0x06006773 RID: 26483 RVA: 0x002B5220 File Offset: 0x002B4220
		private static DateTime DiscardDayTime(DateTime d)
		{
			int year = d.Year;
			int month = d.Month;
			return new DateTime(year, month, 1, 0, 0, 0);
		}

		// Token: 0x06006774 RID: 26484 RVA: 0x002B5248 File Offset: 0x002B4248
		private static DateTime? DiscardTime(DateTime? d)
		{
			if (d == null)
			{
				return null;
			}
			DateTime value = d.Value;
			int year = value.Year;
			int month = value.Month;
			int day = value.Day;
			return new DateTime?(new DateTime(year, month, day, 0, 0, 0));
		}

		// Token: 0x06006775 RID: 26485 RVA: 0x002B529A File Offset: 0x002B429A
		private void DropDownButton_Click(object sender, RoutedEventArgs e)
		{
			this.TogglePopUp();
		}

		// Token: 0x06006776 RID: 26486 RVA: 0x002B52A2 File Offset: 0x002B42A2
		private void DropDownButton_MouseLeave(object sender, MouseEventArgs e)
		{
			this._disablePopupReopen = false;
		}

		// Token: 0x06006777 RID: 26487 RVA: 0x002B52AC File Offset: 0x002B42AC
		private void TogglePopUp()
		{
			if (this.IsDropDownOpen)
			{
				base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
				return;
			}
			if (this._disablePopupReopen)
			{
				this._disablePopupReopen = false;
				return;
			}
			this.SetSelectedDate();
			base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.TrueBox);
		}

		// Token: 0x06006778 RID: 26488 RVA: 0x002B52F8 File Offset: 0x002B42F8
		private void InitializeCalendar()
		{
			this._calendar = new Calendar();
			this._calendar.DayButtonMouseUp += this.Calendar_DayButtonMouseUp;
			this._calendar.DisplayDateChanged += this.Calendar_DisplayDateChanged;
			this._calendar.SelectedDatesChanged += this.Calendar_SelectedDatesChanged;
			this._calendar.DayOrMonthPreviewKeyDown += this.CalendarDayOrMonthButton_PreviewKeyDown;
			this._calendar.HorizontalAlignment = HorizontalAlignment.Left;
			this._calendar.VerticalAlignment = VerticalAlignment.Top;
			this._calendar.SelectionMode = CalendarSelectionMode.SingleDate;
			this._calendar.SetBinding(Control.ForegroundProperty, this.GetDatePickerBinding(Control.ForegroundProperty));
			this._calendar.SetBinding(FrameworkElement.StyleProperty, this.GetDatePickerBinding(DatePicker.CalendarStyleProperty));
			this._calendar.SetBinding(Calendar.IsTodayHighlightedProperty, this.GetDatePickerBinding(DatePicker.IsTodayHighlightedProperty));
			this._calendar.SetBinding(Calendar.FirstDayOfWeekProperty, this.GetDatePickerBinding(DatePicker.FirstDayOfWeekProperty));
			this._calendar.SetBinding(FrameworkElement.FlowDirectionProperty, this.GetDatePickerBinding(FrameworkElement.FlowDirectionProperty));
			RenderOptions.SetClearTypeHint(this._calendar, ClearTypeHint.Enabled);
		}

		// Token: 0x06006779 RID: 26489 RVA: 0x002B5428 File Offset: 0x002B4428
		private BindingBase GetDatePickerBinding(DependencyProperty property)
		{
			return new Binding(property.Name)
			{
				Source = this
			};
		}

		// Token: 0x0600677A RID: 26490 RVA: 0x002B543C File Offset: 0x002B443C
		private static bool IsValidSelectedDateFormat(object value)
		{
			DatePickerFormat datePickerFormat = (DatePickerFormat)value;
			return datePickerFormat == DatePickerFormat.Long || datePickerFormat == DatePickerFormat.Short;
		}

		// Token: 0x0600677B RID: 26491 RVA: 0x002B545C File Offset: 0x002B445C
		private DateTime? ParseText(string text)
		{
			try
			{
				DateTime dateTime = DateTime.Parse(text, DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)));
				if (Calendar.IsValidDateSelection(this._calendar, dateTime))
				{
					return new DateTime?(dateTime);
				}
				DatePickerDateValidationErrorEventArgs datePickerDateValidationErrorEventArgs = new DatePickerDateValidationErrorEventArgs(new ArgumentOutOfRangeException("text", SR.Get("Calendar_OnSelectedDateChanged_InvalidValue")), text);
				this.OnDateValidationError(datePickerDateValidationErrorEventArgs);
				if (datePickerDateValidationErrorEventArgs.ThrowException)
				{
					throw datePickerDateValidationErrorEventArgs.Exception;
				}
			}
			catch (FormatException exception)
			{
				DatePickerDateValidationErrorEventArgs datePickerDateValidationErrorEventArgs2 = new DatePickerDateValidationErrorEventArgs(exception, text);
				this.OnDateValidationError(datePickerDateValidationErrorEventArgs2);
				if (datePickerDateValidationErrorEventArgs2.ThrowException && datePickerDateValidationErrorEventArgs2.Exception != null)
				{
					throw datePickerDateValidationErrorEventArgs2.Exception;
				}
			}
			return null;
		}

		// Token: 0x0600677C RID: 26492 RVA: 0x002B5510 File Offset: 0x002B4510
		private bool ProcessDatePickerKey(KeyEventArgs e)
		{
			Key key = e.Key;
			if (key == Key.Return)
			{
				this.SetSelectedDate();
				return true;
			}
			if (key == Key.System && e.SystemKey == Key.Down && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
			{
				this.TogglePopUp();
				return true;
			}
			return false;
		}

		// Token: 0x0600677D RID: 26493 RVA: 0x002B5558 File Offset: 0x002B4558
		private void SetSelectedDate()
		{
			if (this._textBox != null)
			{
				if (!string.IsNullOrEmpty(this._textBox.Text))
				{
					string text = this._textBox.Text;
					if (this.SelectedDate != null && string.Compare(this.DateTimeToString(this.SelectedDate.Value), text, StringComparison.Ordinal) == 0)
					{
						return;
					}
					DateTime? dateTime = this.SetTextBoxValue(text);
					if (!this.SelectedDate.Equals(dateTime))
					{
						base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, dateTime);
						base.SetCurrentValueInternal(DatePicker.DisplayDateProperty, dateTime);
						return;
					}
				}
				else if (this.SelectedDate != null)
				{
					base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, null);
					return;
				}
			}
			else
			{
				DateTime? dateTime2 = this.SetTextBoxValue(this._defaultText);
				if (!this.SelectedDate.Equals(dateTime2))
				{
					base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, dateTime2);
				}
			}
		}

		// Token: 0x0600677E RID: 26494 RVA: 0x002B565D File Offset: 0x002B465D
		private void SafeSetText(string s)
		{
			if (string.Compare(this.Text, s, StringComparison.Ordinal) != 0)
			{
				base.SetCurrentValueInternal(DatePicker.TextProperty, s);
			}
		}

		// Token: 0x0600677F RID: 26495 RVA: 0x002B567C File Offset: 0x002B467C
		private DateTime? SetTextBoxValue(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				this.SafeSetText(s);
				return this.SelectedDate;
			}
			DateTime? result = this.ParseText(s);
			if (result != null)
			{
				this.SafeSetText(this.DateTimeToString(result.Value));
				return result;
			}
			if (this.SelectedDate != null)
			{
				string s2 = this.DateTimeToString(this.SelectedDate.Value);
				this.SafeSetText(s2);
				return this.SelectedDate;
			}
			this.SetWaterMarkText();
			return null;
		}

		// Token: 0x06006780 RID: 26496 RVA: 0x002B5708 File Offset: 0x002B4708
		private void SetWaterMarkText()
		{
			if (this._textBox != null)
			{
				DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this));
				this.SetTextInternal(string.Empty);
				this._defaultText = string.Empty;
				DatePickerFormat selectedDateFormat = this.SelectedDateFormat;
				if (selectedDateFormat == DatePickerFormat.Long)
				{
					this._textBox.Watermark = string.Format(CultureInfo.CurrentCulture, SR.Get("DatePicker_WatermarkText"), dateFormat.LongDatePattern.ToString());
					return;
				}
				if (selectedDateFormat != DatePickerFormat.Short)
				{
					return;
				}
				this._textBox.Watermark = string.Format(CultureInfo.CurrentCulture, SR.Get("DatePicker_WatermarkText"), dateFormat.ShortDatePattern.ToString());
			}
		}

		// Token: 0x06006781 RID: 26497 RVA: 0x002B57A6 File Offset: 0x002B47A6
		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			this.SetSelectedDate();
		}

		// Token: 0x06006782 RID: 26498 RVA: 0x002B57AE File Offset: 0x002B47AE
		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = (this.ProcessDatePickerKey(e) || e.Handled);
		}

		// Token: 0x06006783 RID: 26499 RVA: 0x002B57C8 File Offset: 0x002B47C8
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.SetValueNoCallback(DatePicker.TextProperty, this._textBox.Text);
		}

		// Token: 0x0400342E RID: 13358
		private const string ElementRoot = "PART_Root";

		// Token: 0x0400342F RID: 13359
		private const string ElementTextBox = "PART_TextBox";

		// Token: 0x04003430 RID: 13360
		private const string ElementButton = "PART_Button";

		// Token: 0x04003431 RID: 13361
		private const string ElementPopup = "PART_Popup";

		// Token: 0x04003432 RID: 13362
		private Calendar _calendar;

		// Token: 0x04003433 RID: 13363
		private string _defaultText;

		// Token: 0x04003434 RID: 13364
		private ButtonBase _dropDownButton;

		// Token: 0x04003435 RID: 13365
		private Popup _popUp;

		// Token: 0x04003436 RID: 13366
		private bool _disablePopupReopen;

		// Token: 0x04003437 RID: 13367
		private DatePickerTextBox _textBox;

		// Token: 0x04003438 RID: 13368
		private IDictionary<DependencyProperty, bool> _isHandlerSuspended;

		// Token: 0x04003439 RID: 13369
		private DateTime? _originalSelectedDate;

		// Token: 0x0400343E RID: 13374
		public static readonly DependencyProperty CalendarStyleProperty;

		// Token: 0x0400343F RID: 13375
		public static readonly DependencyProperty DisplayDateProperty;

		// Token: 0x04003440 RID: 13376
		public static readonly DependencyProperty DisplayDateEndProperty;

		// Token: 0x04003441 RID: 13377
		public static readonly DependencyProperty DisplayDateStartProperty;

		// Token: 0x04003442 RID: 13378
		public static readonly DependencyProperty FirstDayOfWeekProperty;

		// Token: 0x04003443 RID: 13379
		public static readonly DependencyProperty IsDropDownOpenProperty;

		// Token: 0x04003444 RID: 13380
		public static readonly DependencyProperty IsTodayHighlightedProperty;

		// Token: 0x04003445 RID: 13381
		public static readonly DependencyProperty SelectedDateProperty;

		// Token: 0x04003446 RID: 13382
		public static readonly DependencyProperty SelectedDateFormatProperty;

		// Token: 0x04003447 RID: 13383
		public static readonly DependencyProperty TextProperty;
	}
}
