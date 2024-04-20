using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000826 RID: 2086
	[TemplatePart(Name = "PART_DisabledVisual", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_HeaderButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_PreviousButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_NextButton", Type = typeof(Button))]
	[TemplatePart(Name = "DayTitleTemplate", Type = typeof(DataTemplate))]
	[TemplatePart(Name = "PART_MonthView", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_YearView", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_Root", Type = typeof(FrameworkElement))]
	public sealed class CalendarItem : Control
	{
		// Token: 0x060079B5 RID: 31157 RVA: 0x00304134 File Offset: 0x00303134
		static CalendarItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(typeof(CalendarItem)));
			UIElement.FocusableProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(false));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(CalendarItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		// Token: 0x17001C34 RID: 7220
		// (get) Token: 0x060079B7 RID: 31159 RVA: 0x003041F9 File Offset: 0x003031F9
		internal Grid MonthView
		{
			get
			{
				return this._monthView;
			}
		}

		// Token: 0x17001C35 RID: 7221
		// (get) Token: 0x060079B8 RID: 31160 RVA: 0x00304201 File Offset: 0x00303201
		// (set) Token: 0x060079B9 RID: 31161 RVA: 0x00304209 File Offset: 0x00303209
		internal Calendar Owner { get; set; }

		// Token: 0x17001C36 RID: 7222
		// (get) Token: 0x060079BA RID: 31162 RVA: 0x00304212 File Offset: 0x00303212
		internal Grid YearView
		{
			get
			{
				return this._yearView;
			}
		}

		// Token: 0x17001C37 RID: 7223
		// (get) Token: 0x060079BB RID: 31163 RVA: 0x0030421A File Offset: 0x0030321A
		private CalendarMode DisplayMode
		{
			get
			{
				if (this.Owner == null)
				{
					return CalendarMode.Month;
				}
				return this.Owner.DisplayMode;
			}
		}

		// Token: 0x17001C38 RID: 7224
		// (get) Token: 0x060079BC RID: 31164 RVA: 0x00304231 File Offset: 0x00303231
		internal Button HeaderButton
		{
			get
			{
				return this._headerButton;
			}
		}

		// Token: 0x17001C39 RID: 7225
		// (get) Token: 0x060079BD RID: 31165 RVA: 0x00304239 File Offset: 0x00303239
		internal Button NextButton
		{
			get
			{
				return this._nextButton;
			}
		}

		// Token: 0x17001C3A RID: 7226
		// (get) Token: 0x060079BE RID: 31166 RVA: 0x00304241 File Offset: 0x00303241
		internal Button PreviousButton
		{
			get
			{
				return this._previousButton;
			}
		}

		// Token: 0x17001C3B RID: 7227
		// (get) Token: 0x060079BF RID: 31167 RVA: 0x00304249 File Offset: 0x00303249
		private DateTime DisplayDate
		{
			get
			{
				if (this.Owner == null)
				{
					return DateTime.Today;
				}
				return this.Owner.DisplayDate;
			}
		}

		// Token: 0x060079C0 RID: 31168 RVA: 0x00304264 File Offset: 0x00303264
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this._previousButton != null)
			{
				this._previousButton.Click -= this.PreviousButton_Click;
			}
			if (this._nextButton != null)
			{
				this._nextButton.Click -= this.NextButton_Click;
			}
			if (this._headerButton != null)
			{
				this._headerButton.Click -= this.HeaderButton_Click;
			}
			this._monthView = (base.GetTemplateChild("PART_MonthView") as Grid);
			this._yearView = (base.GetTemplateChild("PART_YearView") as Grid);
			this._previousButton = (base.GetTemplateChild("PART_PreviousButton") as Button);
			this._nextButton = (base.GetTemplateChild("PART_NextButton") as Button);
			this._headerButton = (base.GetTemplateChild("PART_HeaderButton") as Button);
			this._disabledVisual = (base.GetTemplateChild("PART_DisabledVisual") as FrameworkElement);
			this._dayTitleTemplate = null;
			if (base.Template != null && base.Template.Resources.Contains(CalendarItem.DayTitleTemplateResourceKey))
			{
				this._dayTitleTemplate = (base.Template.Resources[CalendarItem.DayTitleTemplateResourceKey] as DataTemplate);
			}
			if (this._previousButton != null)
			{
				if (this._previousButton.Content == null)
				{
					this._previousButton.Content = SR.Get("Calendar_PreviousButtonName");
				}
				this._previousButton.Click += this.PreviousButton_Click;
			}
			if (this._nextButton != null)
			{
				if (this._nextButton.Content == null)
				{
					this._nextButton.Content = SR.Get("Calendar_NextButtonName");
				}
				this._nextButton.Click += this.NextButton_Click;
			}
			if (this._headerButton != null)
			{
				this._headerButton.Click += this.HeaderButton_Click;
			}
			this.PopulateGrids();
			if (this.Owner == null)
			{
				this.UpdateMonthMode();
				return;
			}
			switch (this.Owner.DisplayMode)
			{
			case CalendarMode.Month:
				this.UpdateMonthMode();
				return;
			case CalendarMode.Year:
				this.UpdateYearMode();
				return;
			case CalendarMode.Decade:
				this.UpdateDecadeMode();
				return;
			default:
				return;
			}
		}

		// Token: 0x060079C1 RID: 31169 RVA: 0x00304487 File Offset: 0x00303487
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x060079C2 RID: 31170 RVA: 0x003044B4 File Offset: 0x003034B4
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			if (base.IsMouseCaptured)
			{
				base.ReleaseMouseCapture();
			}
			this._isMonthPressed = false;
			this._isDayPressed = false;
			if (!e.Handled && this.Owner.DisplayMode == CalendarMode.Month && this.Owner.HoverEnd != null)
			{
				this.FinishSelection(this.Owner.HoverEnd.Value);
			}
		}

		// Token: 0x060079C3 RID: 31171 RVA: 0x00304527 File Offset: 0x00303527
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			if (!base.IsMouseCaptured)
			{
				this._isDayPressed = false;
				this._isMonthPressed = false;
			}
		}

		// Token: 0x060079C4 RID: 31172 RVA: 0x00304548 File Offset: 0x00303548
		internal void UpdateDecadeMode()
		{
			DateTime selectedYear;
			if (this.Owner != null)
			{
				selectedYear = this.Owner.DisplayYear;
			}
			else
			{
				selectedYear = DateTime.Today;
			}
			int decadeForDecadeMode = this.GetDecadeForDecadeMode(selectedYear);
			int num = decadeForDecadeMode + 9;
			this.SetDecadeModeHeaderButton(decadeForDecadeMode);
			this.SetDecadeModePreviousButton(decadeForDecadeMode);
			this.SetDecadeModeNextButton(num);
			if (this._yearView != null)
			{
				this.SetYearButtons(decadeForDecadeMode, num);
			}
		}

		// Token: 0x060079C5 RID: 31173 RVA: 0x003045A3 File Offset: 0x003035A3
		internal void UpdateMonthMode()
		{
			this.SetMonthModeHeaderButton();
			this.SetMonthModePreviousButton();
			this.SetMonthModeNextButton();
			if (this._monthView != null)
			{
				this.SetMonthModeDayTitles();
				this.SetMonthModeCalendarDayButtons();
				this.AddMonthModeHighlight();
			}
		}

		// Token: 0x060079C6 RID: 31174 RVA: 0x003045D1 File Offset: 0x003035D1
		internal void UpdateYearMode()
		{
			this.SetYearModeHeaderButton();
			this.SetYearModePreviousButton();
			this.SetYearModeNextButton();
			if (this._yearView != null)
			{
				this.SetYearModeMonthButtons();
			}
		}

		// Token: 0x060079C7 RID: 31175 RVA: 0x003045F3 File Offset: 0x003035F3
		internal IEnumerable<CalendarDayButton> GetCalendarDayButtons()
		{
			int count = 49;
			if (this.MonthView != null)
			{
				UIElementCollection dayButtonsHost = this.MonthView.Children;
				int num;
				for (int childIndex = 7; childIndex < count; childIndex = num + 1)
				{
					CalendarDayButton calendarDayButton = dayButtonsHost[childIndex] as CalendarDayButton;
					if (calendarDayButton != null)
					{
						yield return calendarDayButton;
					}
					num = childIndex;
				}
				dayButtonsHost = null;
			}
			yield break;
		}

		// Token: 0x060079C8 RID: 31176 RVA: 0x00304604 File Offset: 0x00303604
		internal CalendarDayButton GetFocusedCalendarDayButton()
		{
			foreach (CalendarDayButton calendarDayButton in this.GetCalendarDayButtons())
			{
				if (calendarDayButton != null && calendarDayButton.IsFocused)
				{
					return calendarDayButton;
				}
			}
			return null;
		}

		// Token: 0x060079C9 RID: 31177 RVA: 0x0030465C File Offset: 0x0030365C
		internal CalendarDayButton GetCalendarDayButton(DateTime date)
		{
			foreach (CalendarDayButton calendarDayButton in this.GetCalendarDayButtons())
			{
				if (calendarDayButton != null && calendarDayButton.DataContext is DateTime && DateTimeHelper.CompareDays(date, (DateTime)calendarDayButton.DataContext) == 0)
				{
					return calendarDayButton;
				}
			}
			return null;
		}

		// Token: 0x060079CA RID: 31178 RVA: 0x003046CC File Offset: 0x003036CC
		internal CalendarButton GetCalendarButton(DateTime date, CalendarMode mode)
		{
			foreach (CalendarButton calendarButton in this.GetCalendarButtons())
			{
				if (calendarButton != null && calendarButton.DataContext is DateTime)
				{
					if (mode == CalendarMode.Year)
					{
						if (DateTimeHelper.CompareYearMonth(date, (DateTime)calendarButton.DataContext) == 0)
						{
							return calendarButton;
						}
					}
					else if (date.Year == ((DateTime)calendarButton.DataContext).Year)
					{
						return calendarButton;
					}
				}
			}
			return null;
		}

		// Token: 0x060079CB RID: 31179 RVA: 0x00304760 File Offset: 0x00303760
		internal CalendarButton GetFocusedCalendarButton()
		{
			foreach (CalendarButton calendarButton in this.GetCalendarButtons())
			{
				if (calendarButton != null && calendarButton.IsFocused)
				{
					return calendarButton;
				}
			}
			return null;
		}

		// Token: 0x060079CC RID: 31180 RVA: 0x003047B8 File Offset: 0x003037B8
		private IEnumerable<CalendarButton> GetCalendarButtons()
		{
			foreach (object obj in this.YearView.Children)
			{
				CalendarButton calendarButton = ((UIElement)obj) as CalendarButton;
				if (calendarButton != null)
				{
					yield return calendarButton;
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060079CD RID: 31181 RVA: 0x003047C8 File Offset: 0x003037C8
		internal void FocusDate(DateTime date)
		{
			FrameworkElement frameworkElement = null;
			CalendarMode displayMode = this.DisplayMode;
			if (displayMode != CalendarMode.Month)
			{
				if (displayMode - CalendarMode.Year <= 1)
				{
					frameworkElement = this.GetCalendarButton(date, this.DisplayMode);
				}
			}
			else
			{
				frameworkElement = this.GetCalendarDayButton(date);
			}
			if (frameworkElement != null && !frameworkElement.IsFocused)
			{
				frameworkElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
			}
		}

		// Token: 0x060079CE RID: 31182 RVA: 0x0030481C File Offset: 0x0030381C
		private int GetDecadeForDecadeMode(DateTime selectedYear)
		{
			int num = DateTimeHelper.DecadeOfDate(selectedYear);
			if (this._isMonthPressed && this._yearView != null)
			{
				UIElementCollection children = this._yearView.Children;
				int count = children.Count;
				if (count > 0)
				{
					CalendarButton calendarButton = children[0] as CalendarButton;
					if (calendarButton != null && calendarButton.DataContext is DateTime && ((DateTime)calendarButton.DataContext).Year == selectedYear.Year)
					{
						return num + 10;
					}
				}
				if (count > 1)
				{
					CalendarButton calendarButton2 = children[count - 1] as CalendarButton;
					if (calendarButton2 != null && calendarButton2.DataContext is DateTime && ((DateTime)calendarButton2.DataContext).Year == selectedYear.Year)
					{
						return num - 10;
					}
				}
			}
			return num;
		}

		// Token: 0x060079CF RID: 31183 RVA: 0x003048E8 File Offset: 0x003038E8
		private void EndDrag(bool ctrl, DateTime selectedDate)
		{
			if (this.Owner != null)
			{
				this.Owner.CurrentDate = selectedDate;
				if (this.Owner.HoverStart != null)
				{
					if (ctrl && DateTime.Compare(this.Owner.HoverStart.Value, selectedDate) == 0 && (this.Owner.SelectionMode == CalendarSelectionMode.SingleDate || this.Owner.SelectionMode == CalendarSelectionMode.MultipleRange))
					{
						this.Owner.SelectedDates.Toggle(selectedDate);
					}
					else
					{
						this.Owner.SelectedDates.AddRangeInternal(this.Owner.HoverStart.Value, selectedDate);
					}
					this.Owner.OnDayClick(selectedDate);
				}
			}
		}

		// Token: 0x060079D0 RID: 31184 RVA: 0x0030499D File Offset: 0x0030399D
		private void CellOrMonth_PreviewKeyDown(object sender, RoutedEventArgs e)
		{
			if (this.Owner == null)
			{
				return;
			}
			this.Owner.OnDayOrMonthPreviewKeyDown(e);
		}

		// Token: 0x060079D1 RID: 31185 RVA: 0x003049B4 File Offset: 0x003039B4
		private void Cell_Clicked(object sender, RoutedEventArgs e)
		{
			if (this.Owner == null)
			{
				return;
			}
			CalendarDayButton calendarDayButton = sender as CalendarDayButton;
			if (!(calendarDayButton.DataContext is DateTime))
			{
				return;
			}
			if (!calendarDayButton.IsBlackedOut)
			{
				DateTime dateTime = (DateTime)calendarDayButton.DataContext;
				bool flag;
				bool flag2;
				CalendarKeyboardHelper.GetMetaKeyState(out flag, out flag2);
				switch (this.Owner.SelectionMode)
				{
				case CalendarSelectionMode.SingleDate:
					if (!flag)
					{
						this.Owner.SelectedDate = new DateTime?(dateTime);
					}
					else
					{
						this.Owner.SelectedDates.Toggle(dateTime);
					}
					break;
				case CalendarSelectionMode.SingleRange:
				{
					DateTime? dateTime2 = new DateTime?(this.Owner.CurrentDate);
					this.Owner.SelectedDates.ClearInternal(true);
					if (flag2 && dateTime2 != null)
					{
						this.Owner.SelectedDates.AddRangeInternal(dateTime2.Value, dateTime);
					}
					else
					{
						this.Owner.SelectedDate = new DateTime?(dateTime);
						this.Owner.HoverStart = null;
						this.Owner.HoverEnd = null;
					}
					break;
				}
				case CalendarSelectionMode.MultipleRange:
					if (!flag)
					{
						this.Owner.SelectedDates.ClearInternal(true);
					}
					if (flag2)
					{
						this.Owner.SelectedDates.AddRangeInternal(this.Owner.CurrentDate, dateTime);
					}
					else if (!flag)
					{
						this.Owner.SelectedDate = new DateTime?(dateTime);
					}
					else
					{
						this.Owner.SelectedDates.Toggle(dateTime);
						this.Owner.HoverStart = null;
						this.Owner.HoverEnd = null;
					}
					break;
				}
				this.Owner.OnDayClick(dateTime);
			}
		}

		// Token: 0x060079D2 RID: 31186 RVA: 0x00304B78 File Offset: 0x00303B78
		private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			CalendarDayButton calendarDayButton = sender as CalendarDayButton;
			if (calendarDayButton == null)
			{
				return;
			}
			if (this.Owner == null || !(calendarDayButton.DataContext is DateTime))
			{
				return;
			}
			if (calendarDayButton.IsBlackedOut)
			{
				Calendar owner = this.Owner;
				DateTime? dateTime = null;
				owner.HoverStart = dateTime;
				return;
			}
			this._isDayPressed = true;
			Mouse.Capture(this, CaptureMode.SubTree);
			calendarDayButton.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
			bool flag;
			bool flag2;
			CalendarKeyboardHelper.GetMetaKeyState(out flag, out flag2);
			DateTime dateTime2 = (DateTime)calendarDayButton.DataContext;
			switch (this.Owner.SelectionMode)
			{
			case CalendarSelectionMode.SingleDate:
				this.Owner.DatePickerDisplayDateFlag = true;
				if (!flag)
				{
					this.Owner.SelectedDate = new DateTime?(dateTime2);
				}
				else
				{
					this.Owner.SelectedDates.Toggle(dateTime2);
				}
				break;
			case CalendarSelectionMode.SingleRange:
				this.Owner.SelectedDates.ClearInternal();
				if (flag2)
				{
					DateTime? dateTime = this.Owner.HoverStart;
					if (dateTime == null)
					{
						Calendar owner2 = this.Owner;
						Calendar owner3 = this.Owner;
						dateTime = new DateTime?(this.Owner.CurrentDate);
						owner3.HoverEnd = dateTime;
						owner2.HoverStart = dateTime;
					}
				}
				else
				{
					Calendar owner4 = this.Owner;
					Calendar owner5 = this.Owner;
					DateTime? dateTime = new DateTime?(dateTime2);
					owner5.HoverEnd = dateTime;
					owner4.HoverStart = dateTime;
				}
				break;
			case CalendarSelectionMode.MultipleRange:
				if (!flag)
				{
					this.Owner.SelectedDates.ClearInternal();
				}
				if (flag2)
				{
					DateTime? dateTime = this.Owner.HoverStart;
					if (dateTime == null)
					{
						Calendar owner6 = this.Owner;
						Calendar owner7 = this.Owner;
						dateTime = new DateTime?(this.Owner.CurrentDate);
						owner7.HoverEnd = dateTime;
						owner6.HoverStart = dateTime;
					}
				}
				else
				{
					Calendar owner8 = this.Owner;
					Calendar owner9 = this.Owner;
					DateTime? dateTime = new DateTime?(dateTime2);
					owner9.HoverEnd = dateTime;
					owner8.HoverStart = dateTime;
				}
				break;
			}
			this.Owner.CurrentDate = dateTime2;
			this.Owner.UpdateCellItems();
		}

		// Token: 0x060079D3 RID: 31187 RVA: 0x00304D68 File Offset: 0x00303D68
		private void Cell_MouseEnter(object sender, MouseEventArgs e)
		{
			CalendarDayButton calendarDayButton = sender as CalendarDayButton;
			if (calendarDayButton == null)
			{
				return;
			}
			if (calendarDayButton.IsBlackedOut)
			{
				return;
			}
			if (e.LeftButton == MouseButtonState.Pressed && this._isDayPressed)
			{
				calendarDayButton.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
				if (this.Owner == null || !(calendarDayButton.DataContext is DateTime))
				{
					return;
				}
				DateTime dateTime = (DateTime)calendarDayButton.DataContext;
				if (this.Owner.SelectionMode == CalendarSelectionMode.SingleDate)
				{
					this.Owner.DatePickerDisplayDateFlag = true;
					Calendar owner = this.Owner;
					Calendar owner2 = this.Owner;
					DateTime? dateTime2 = null;
					owner2.HoverEnd = dateTime2;
					owner.HoverStart = dateTime2;
					if (this.Owner.SelectedDates.Count == 0)
					{
						this.Owner.SelectedDates.Add(dateTime);
						return;
					}
					this.Owner.SelectedDates[0] = dateTime;
					return;
				}
				else
				{
					this.Owner.HoverEnd = new DateTime?(dateTime);
					this.Owner.CurrentDate = dateTime;
					this.Owner.UpdateCellItems();
				}
			}
		}

		// Token: 0x060079D4 RID: 31188 RVA: 0x00304E68 File Offset: 0x00303E68
		private void Cell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			CalendarDayButton calendarDayButton = sender as CalendarDayButton;
			if (calendarDayButton == null)
			{
				return;
			}
			if (this.Owner == null)
			{
				return;
			}
			if (!calendarDayButton.IsBlackedOut)
			{
				this.Owner.OnDayButtonMouseUp(e);
			}
			if (!(calendarDayButton.DataContext is DateTime))
			{
				return;
			}
			this.FinishSelection((DateTime)calendarDayButton.DataContext);
			e.Handled = true;
		}

		// Token: 0x060079D5 RID: 31189 RVA: 0x00304EC4 File Offset: 0x00303EC4
		private void FinishSelection(DateTime selectedDate)
		{
			bool ctrl;
			bool flag;
			CalendarKeyboardHelper.GetMetaKeyState(out ctrl, out flag);
			if (this.Owner.SelectionMode == CalendarSelectionMode.None || this.Owner.SelectionMode == CalendarSelectionMode.SingleDate)
			{
				this.Owner.OnDayClick(selectedDate);
				return;
			}
			if (this.Owner.HoverStart == null)
			{
				CalendarDayButton calendarDayButton = this.GetCalendarDayButton(selectedDate);
				if (calendarDayButton != null && calendarDayButton.IsInactive && calendarDayButton.IsBlackedOut)
				{
					this.Owner.OnDayClick(selectedDate);
				}
				return;
			}
			CalendarSelectionMode selectionMode = this.Owner.SelectionMode;
			if (selectionMode == CalendarSelectionMode.SingleRange)
			{
				this.Owner.SelectedDates.ClearInternal();
				this.EndDrag(ctrl, selectedDate);
				return;
			}
			if (selectionMode != CalendarSelectionMode.MultipleRange)
			{
				return;
			}
			this.EndDrag(ctrl, selectedDate);
		}

		// Token: 0x060079D6 RID: 31190 RVA: 0x00304F7C File Offset: 0x00303F7C
		private void Month_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			CalendarButton calendarButton = sender as CalendarButton;
			if (calendarButton != null)
			{
				this._isMonthPressed = true;
				Mouse.Capture(this, CaptureMode.SubTree);
				if (this.Owner != null)
				{
					this.Owner.OnCalendarButtonPressed(calendarButton, false);
				}
			}
		}

		// Token: 0x060079D7 RID: 31191 RVA: 0x00304FB8 File Offset: 0x00303FB8
		private void Month_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			CalendarButton calendarButton = sender as CalendarButton;
			if (calendarButton != null && this.Owner != null)
			{
				this.Owner.OnCalendarButtonPressed(calendarButton, true);
			}
		}

		// Token: 0x060079D8 RID: 31192 RVA: 0x00304FE4 File Offset: 0x00303FE4
		private void Month_MouseEnter(object sender, MouseEventArgs e)
		{
			CalendarButton calendarButton = sender as CalendarButton;
			if (calendarButton != null && this._isMonthPressed && this.Owner != null)
			{
				this.Owner.OnCalendarButtonPressed(calendarButton, false);
			}
		}

		// Token: 0x060079D9 RID: 31193 RVA: 0x00305018 File Offset: 0x00304018
		private void Month_Clicked(object sender, RoutedEventArgs e)
		{
			CalendarButton calendarButton = sender as CalendarButton;
			if (calendarButton != null)
			{
				this.Owner.OnCalendarButtonPressed(calendarButton, true);
			}
		}

		// Token: 0x060079DA RID: 31194 RVA: 0x0030503C File Offset: 0x0030403C
		private void HeaderButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Owner != null)
			{
				if (this.Owner.DisplayMode == CalendarMode.Month)
				{
					this.Owner.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Year);
				}
				else
				{
					this.Owner.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Decade);
				}
				this.FocusDate(this.DisplayDate);
			}
		}

		// Token: 0x060079DB RID: 31195 RVA: 0x00305098 File Offset: 0x00304098
		private void PreviousButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Owner != null)
			{
				this.Owner.OnPreviousClick();
			}
		}

		// Token: 0x060079DC RID: 31196 RVA: 0x003050AD File Offset: 0x003040AD
		private void NextButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Owner != null)
			{
				this.Owner.OnNextClick();
			}
		}

		// Token: 0x060079DD RID: 31197 RVA: 0x003050C4 File Offset: 0x003040C4
		private void PopulateGrids()
		{
			if (this._monthView != null)
			{
				for (int i = 0; i < 7; i++)
				{
					FrameworkElement frameworkElement = (this._dayTitleTemplate != null) ? ((FrameworkElement)this._dayTitleTemplate.LoadContent()) : new ContentControl();
					frameworkElement.SetValue(Grid.RowProperty, 0);
					frameworkElement.SetValue(Grid.ColumnProperty, i);
					this._monthView.Children.Add(frameworkElement);
				}
				for (int j = 1; j < 7; j++)
				{
					for (int k = 0; k < 7; k++)
					{
						CalendarDayButton calendarDayButton = new CalendarDayButton();
						calendarDayButton.Owner = this.Owner;
						calendarDayButton.SetValue(Grid.RowProperty, j);
						calendarDayButton.SetValue(Grid.ColumnProperty, k);
						calendarDayButton.SetBinding(FrameworkElement.StyleProperty, this.GetOwnerBinding("CalendarDayButtonStyle"));
						calendarDayButton.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Cell_MouseLeftButtonDown), true);
						calendarDayButton.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.Cell_MouseLeftButtonUp), true);
						calendarDayButton.AddHandler(UIElement.MouseEnterEvent, new MouseEventHandler(this.Cell_MouseEnter), true);
						calendarDayButton.Click += this.Cell_Clicked;
						calendarDayButton.AddHandler(UIElement.PreviewKeyDownEvent, new RoutedEventHandler(this.CellOrMonth_PreviewKeyDown), true);
						this._monthView.Children.Add(calendarDayButton);
					}
				}
			}
			if (this._yearView != null)
			{
				int num = 0;
				for (int l = 0; l < 3; l++)
				{
					for (int m = 0; m < 4; m++)
					{
						CalendarButton calendarButton = new CalendarButton();
						calendarButton.Owner = this.Owner;
						calendarButton.SetValue(Grid.RowProperty, l);
						calendarButton.SetValue(Grid.ColumnProperty, m);
						calendarButton.SetBinding(FrameworkElement.StyleProperty, this.GetOwnerBinding("CalendarButtonStyle"));
						calendarButton.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Month_MouseLeftButtonDown), true);
						calendarButton.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.Month_MouseLeftButtonUp), true);
						calendarButton.AddHandler(UIElement.MouseEnterEvent, new MouseEventHandler(this.Month_MouseEnter), true);
						calendarButton.AddHandler(UIElement.PreviewKeyDownEvent, new RoutedEventHandler(this.CellOrMonth_PreviewKeyDown), true);
						calendarButton.Click += this.Month_Clicked;
						this._yearView.Children.Add(calendarButton);
						num++;
					}
				}
			}
		}

		// Token: 0x060079DE RID: 31198 RVA: 0x00305358 File Offset: 0x00304358
		private void SetMonthModeDayTitles()
		{
			if (this._monthView != null)
			{
				string[] shortestDayNames = DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)).ShortestDayNames;
				for (int i = 0; i < 7; i++)
				{
					FrameworkElement frameworkElement = this._monthView.Children[i] as FrameworkElement;
					if (frameworkElement != null && shortestDayNames != null && shortestDayNames.Length != 0)
					{
						if (this.Owner != null)
						{
							frameworkElement.DataContext = shortestDayNames[(int)((i + this.Owner.FirstDayOfWeek) % (DayOfWeek)shortestDayNames.Length)];
						}
						else
						{
							frameworkElement.DataContext = shortestDayNames[(int)((i + DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)).FirstDayOfWeek) % (DayOfWeek)shortestDayNames.Length)];
						}
					}
				}
			}
		}

		// Token: 0x060079DF RID: 31199 RVA: 0x003053EC File Offset: 0x003043EC
		private void SetMonthModeCalendarDayButtons()
		{
			DateTime dateTime = DateTimeHelper.DiscardDayTime(this.DisplayDate);
			int numberOfDisplayedDaysFromPreviousMonth = this.GetNumberOfDisplayedDaysFromPreviousMonth(dateTime);
			bool flag = DateTimeHelper.CompareYearMonth(dateTime, DateTime.MinValue) <= 0;
			bool flag2 = DateTimeHelper.CompareYearMonth(dateTime, DateTime.MaxValue) >= 0;
			int daysInMonth = this._calendar.GetDaysInMonth(dateTime.Year, dateTime.Month);
			CultureInfo culture = DateTimeHelper.GetCulture(this);
			int num = 49;
			for (int i = 7; i < num; i++)
			{
				CalendarDayButton calendarDayButton = this._monthView.Children[i] as CalendarDayButton;
				int num2 = i - numberOfDisplayedDaysFromPreviousMonth - 7;
				if ((!flag || num2 >= 0) && (!flag2 || num2 < daysInMonth))
				{
					DateTime dateTime2 = this._calendar.AddDays(dateTime, num2);
					this.SetMonthModeDayButtonState(calendarDayButton, new DateTime?(dateTime2));
					calendarDayButton.DataContext = dateTime2;
					calendarDayButton.SetContentInternal(DateTimeHelper.ToDayString(new DateTime?(dateTime2), culture));
				}
				else
				{
					this.SetMonthModeDayButtonState(calendarDayButton, null);
					calendarDayButton.DataContext = null;
					calendarDayButton.SetContentInternal(DateTimeHelper.ToDayString(null, culture));
				}
			}
		}

		// Token: 0x060079E0 RID: 31200 RVA: 0x00305518 File Offset: 0x00304518
		private void SetMonthModeDayButtonState(CalendarDayButton childButton, DateTime? dateToAdd)
		{
			if (this.Owner != null)
			{
				if (dateToAdd != null)
				{
					childButton.Visibility = Visibility.Visible;
					if (DateTimeHelper.CompareDays(dateToAdd.Value, this.Owner.DisplayDateStartInternal) < 0 || DateTimeHelper.CompareDays(dateToAdd.Value, this.Owner.DisplayDateEndInternal) > 0)
					{
						childButton.IsEnabled = false;
						childButton.Visibility = Visibility.Hidden;
						return;
					}
					childButton.IsEnabled = true;
					childButton.SetValue(CalendarDayButton.IsBlackedOutPropertyKey, this.Owner.BlackoutDates.Contains(dateToAdd.Value));
					childButton.SetValue(CalendarDayButton.IsInactivePropertyKey, DateTimeHelper.CompareYearMonth(dateToAdd.Value, this.Owner.DisplayDateInternal) != 0);
					if (DateTimeHelper.CompareDays(dateToAdd.Value, DateTime.Today) == 0)
					{
						childButton.SetValue(CalendarDayButton.IsTodayPropertyKey, true);
					}
					else
					{
						childButton.SetValue(CalendarDayButton.IsTodayPropertyKey, false);
					}
					childButton.NotifyNeedsVisualStateUpdate();
					bool flag = false;
					foreach (DateTime dt in this.Owner.SelectedDates)
					{
						flag |= (DateTimeHelper.CompareDays(dateToAdd.Value, dt) == 0);
					}
					childButton.SetValue(CalendarDayButton.IsSelectedPropertyKey, flag);
					return;
				}
				else
				{
					childButton.Visibility = Visibility.Hidden;
					childButton.IsEnabled = false;
					childButton.SetValue(CalendarDayButton.IsBlackedOutPropertyKey, false);
					childButton.SetValue(CalendarDayButton.IsInactivePropertyKey, true);
					childButton.SetValue(CalendarDayButton.IsTodayPropertyKey, false);
					childButton.SetValue(CalendarDayButton.IsSelectedPropertyKey, false);
				}
			}
		}

		// Token: 0x060079E1 RID: 31201 RVA: 0x003056A8 File Offset: 0x003046A8
		private void AddMonthModeHighlight()
		{
			Calendar owner = this.Owner;
			if (owner == null)
			{
				return;
			}
			if (owner.HoverStart != null && owner.HoverEnd != null)
			{
				DateTime value = owner.HoverEnd.Value;
				DateTime value2 = owner.HoverEnd.Value;
				int num = DateTimeHelper.CompareDays(owner.HoverEnd.Value, owner.HoverStart.Value);
				if (num < 0)
				{
					value2 = owner.HoverStart.Value;
				}
				else
				{
					value = owner.HoverStart.Value;
				}
				int num2 = 49;
				for (int i = 7; i < num2; i++)
				{
					CalendarDayButton calendarDayButton = this._monthView.Children[i] as CalendarDayButton;
					if (calendarDayButton.DataContext is DateTime)
					{
						DateTime date = (DateTime)calendarDayButton.DataContext;
						calendarDayButton.SetValue(CalendarDayButton.IsHighlightedPropertyKey, num != 0 && DateTimeHelper.InRange(date, value, value2));
					}
					else
					{
						calendarDayButton.SetValue(CalendarDayButton.IsHighlightedPropertyKey, false);
					}
				}
				return;
			}
			int num3 = 49;
			for (int j = 7; j < num3; j++)
			{
				(this._monthView.Children[j] as CalendarDayButton).SetValue(CalendarDayButton.IsHighlightedPropertyKey, false);
			}
		}

		// Token: 0x060079E2 RID: 31202 RVA: 0x003057F9 File Offset: 0x003047F9
		private void SetMonthModeHeaderButton()
		{
			if (this._headerButton != null)
			{
				this._headerButton.Content = DateTimeHelper.ToYearMonthPatternString(new DateTime?(this.DisplayDate), DateTimeHelper.GetCulture(this));
				if (this.Owner != null)
				{
					this._headerButton.IsEnabled = true;
				}
			}
		}

		// Token: 0x060079E3 RID: 31203 RVA: 0x00305838 File Offset: 0x00304838
		private void SetMonthModeNextButton()
		{
			if (this.Owner != null && this._nextButton != null)
			{
				DateTime dateTime = DateTimeHelper.DiscardDayTime(this.DisplayDate);
				if (DateTimeHelper.CompareYearMonth(dateTime, DateTime.MaxValue) == 0)
				{
					this._nextButton.IsEnabled = false;
					return;
				}
				DateTime dt = this._calendar.AddMonths(dateTime, 1);
				this._nextButton.IsEnabled = (DateTimeHelper.CompareDays(this.Owner.DisplayDateEndInternal, dt) > -1);
			}
		}

		// Token: 0x060079E4 RID: 31204 RVA: 0x003058A8 File Offset: 0x003048A8
		private void SetMonthModePreviousButton()
		{
			if (this.Owner != null && this._previousButton != null)
			{
				DateTime dt = DateTimeHelper.DiscardDayTime(this.DisplayDate);
				this._previousButton.IsEnabled = (DateTimeHelper.CompareDays(this.Owner.DisplayDateStartInternal, dt) < 0);
			}
		}

		// Token: 0x060079E5 RID: 31205 RVA: 0x003058F0 File Offset: 0x003048F0
		private void SetYearButtons(int decade, int decadeEnd)
		{
			int num = -1;
			foreach (object obj in this._yearView.Children)
			{
				CalendarButton calendarButton = obj as CalendarButton;
				int num2 = decade + num;
				if (num2 <= DateTime.MaxValue.Year && num2 >= DateTime.MinValue.Year)
				{
					DateTime dateTime = new DateTime(num2, 1, 1);
					calendarButton.DataContext = dateTime;
					calendarButton.SetContentInternal(DateTimeHelper.ToYearString(new DateTime?(dateTime), DateTimeHelper.GetCulture(this)));
					calendarButton.Visibility = Visibility.Visible;
					if (this.Owner != null)
					{
						calendarButton.HasSelectedDays = (this.Owner.DisplayDate.Year == num2);
						if (num2 < this.Owner.DisplayDateStartInternal.Year || num2 > this.Owner.DisplayDateEndInternal.Year)
						{
							calendarButton.IsEnabled = false;
							calendarButton.Opacity = 0.0;
						}
						else
						{
							calendarButton.IsEnabled = true;
							calendarButton.Opacity = 1.0;
						}
					}
					calendarButton.IsInactive = (num2 < decade || num2 > decadeEnd);
				}
				else
				{
					calendarButton.DataContext = null;
					calendarButton.IsEnabled = false;
					calendarButton.Opacity = 0.0;
				}
				num++;
			}
		}

		// Token: 0x060079E6 RID: 31206 RVA: 0x00305A70 File Offset: 0x00304A70
		private void SetYearModeMonthButtons()
		{
			int num = 0;
			foreach (object obj in this._yearView.Children)
			{
				CalendarButton calendarButton = obj as CalendarButton;
				DateTime dateTime = new DateTime(this.DisplayDate.Year, num + 1, 1);
				calendarButton.DataContext = dateTime;
				calendarButton.SetContentInternal(DateTimeHelper.ToAbbreviatedMonthString(new DateTime?(dateTime), DateTimeHelper.GetCulture(this)));
				calendarButton.Visibility = Visibility.Visible;
				if (this.Owner != null)
				{
					calendarButton.HasSelectedDays = (DateTimeHelper.CompareYearMonth(dateTime, this.Owner.DisplayDateInternal) == 0);
					if (DateTimeHelper.CompareYearMonth(dateTime, this.Owner.DisplayDateStartInternal) < 0 || DateTimeHelper.CompareYearMonth(dateTime, this.Owner.DisplayDateEndInternal) > 0)
					{
						calendarButton.IsEnabled = false;
						calendarButton.Opacity = 0.0;
					}
					else
					{
						calendarButton.IsEnabled = true;
						calendarButton.Opacity = 1.0;
					}
				}
				calendarButton.IsInactive = false;
				num++;
			}
		}

		// Token: 0x060079E7 RID: 31207 RVA: 0x00305B9C File Offset: 0x00304B9C
		private void SetYearModeHeaderButton()
		{
			if (this._headerButton != null)
			{
				this._headerButton.IsEnabled = true;
				this._headerButton.Content = DateTimeHelper.ToYearString(new DateTime?(this.DisplayDate), DateTimeHelper.GetCulture(this));
			}
		}

		// Token: 0x060079E8 RID: 31208 RVA: 0x00305BD4 File Offset: 0x00304BD4
		private void SetYearModeNextButton()
		{
			if (this.Owner != null && this._nextButton != null)
			{
				this._nextButton.IsEnabled = (this.Owner.DisplayDateEndInternal.Year != this.DisplayDate.Year);
			}
		}

		// Token: 0x060079E9 RID: 31209 RVA: 0x00305C24 File Offset: 0x00304C24
		private void SetYearModePreviousButton()
		{
			if (this.Owner != null && this._previousButton != null)
			{
				this._previousButton.IsEnabled = (this.Owner.DisplayDateStartInternal.Year != this.DisplayDate.Year);
			}
		}

		// Token: 0x060079EA RID: 31210 RVA: 0x00305C72 File Offset: 0x00304C72
		private void SetDecadeModeHeaderButton(int decade)
		{
			if (this._headerButton != null)
			{
				this._headerButton.Content = DateTimeHelper.ToDecadeRangeString(decade, this);
				this._headerButton.IsEnabled = false;
			}
		}

		// Token: 0x060079EB RID: 31211 RVA: 0x00305C9C File Offset: 0x00304C9C
		private void SetDecadeModeNextButton(int decadeEnd)
		{
			if (this.Owner != null && this._nextButton != null)
			{
				this._nextButton.IsEnabled = (this.Owner.DisplayDateEndInternal.Year > decadeEnd);
			}
		}

		// Token: 0x060079EC RID: 31212 RVA: 0x00305CDC File Offset: 0x00304CDC
		private void SetDecadeModePreviousButton(int decade)
		{
			if (this.Owner != null && this._previousButton != null)
			{
				this._previousButton.IsEnabled = (decade > this.Owner.DisplayDateStartInternal.Year);
			}
		}

		// Token: 0x060079ED RID: 31213 RVA: 0x00305D1C File Offset: 0x00304D1C
		private int GetNumberOfDisplayedDaysFromPreviousMonth(DateTime firstOfMonth)
		{
			DayOfWeek dayOfWeek = this._calendar.GetDayOfWeek(firstOfMonth);
			int num;
			if (this.Owner != null)
			{
				num = (dayOfWeek - this.Owner.FirstDayOfWeek + 7) % 7;
			}
			else
			{
				num = (dayOfWeek - DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)).FirstDayOfWeek + 7) % 7;
			}
			if (num == 0)
			{
				return 7;
			}
			return num;
		}

		// Token: 0x060079EE RID: 31214 RVA: 0x00305D6F File Offset: 0x00304D6F
		private BindingBase GetOwnerBinding(string propertyName)
		{
			return new Binding(propertyName)
			{
				Source = this.Owner
			};
		}

		// Token: 0x17001C3C RID: 7228
		// (get) Token: 0x060079EF RID: 31215 RVA: 0x00305D83 File Offset: 0x00304D83
		public static ComponentResourceKey DayTitleTemplateResourceKey
		{
			get
			{
				if (CalendarItem._dayTitleTemplateResourceKey == null)
				{
					CalendarItem._dayTitleTemplateResourceKey = new ComponentResourceKey(typeof(CalendarItem), "DayTitleTemplate");
				}
				return CalendarItem._dayTitleTemplateResourceKey;
			}
		}

		// Token: 0x040039C0 RID: 14784
		private const string ElementRoot = "PART_Root";

		// Token: 0x040039C1 RID: 14785
		private const string ElementHeaderButton = "PART_HeaderButton";

		// Token: 0x040039C2 RID: 14786
		private const string ElementPreviousButton = "PART_PreviousButton";

		// Token: 0x040039C3 RID: 14787
		private const string ElementNextButton = "PART_NextButton";

		// Token: 0x040039C4 RID: 14788
		private const string ElementDayTitleTemplate = "DayTitleTemplate";

		// Token: 0x040039C5 RID: 14789
		private const string ElementMonthView = "PART_MonthView";

		// Token: 0x040039C6 RID: 14790
		private const string ElementYearView = "PART_YearView";

		// Token: 0x040039C7 RID: 14791
		private const string ElementDisabledVisual = "PART_DisabledVisual";

		// Token: 0x040039C8 RID: 14792
		private const int COLS = 7;

		// Token: 0x040039C9 RID: 14793
		private const int ROWS = 7;

		// Token: 0x040039CA RID: 14794
		private const int YEAR_COLS = 4;

		// Token: 0x040039CB RID: 14795
		private const int YEAR_ROWS = 3;

		// Token: 0x040039CC RID: 14796
		private const int NUMBER_OF_DAYS_IN_WEEK = 7;

		// Token: 0x040039CD RID: 14797
		private static ComponentResourceKey _dayTitleTemplateResourceKey;

		// Token: 0x040039CE RID: 14798
		private Calendar _calendar = new GregorianCalendar();

		// Token: 0x040039CF RID: 14799
		private DataTemplate _dayTitleTemplate;

		// Token: 0x040039D0 RID: 14800
		private FrameworkElement _disabledVisual;

		// Token: 0x040039D1 RID: 14801
		private Button _headerButton;

		// Token: 0x040039D2 RID: 14802
		private Grid _monthView;

		// Token: 0x040039D3 RID: 14803
		private Button _nextButton;

		// Token: 0x040039D4 RID: 14804
		private Button _previousButton;

		// Token: 0x040039D5 RID: 14805
		private Grid _yearView;

		// Token: 0x040039D6 RID: 14806
		private bool _isMonthPressed;

		// Token: 0x040039D7 RID: 14807
		private bool _isDayPressed;
	}
}
