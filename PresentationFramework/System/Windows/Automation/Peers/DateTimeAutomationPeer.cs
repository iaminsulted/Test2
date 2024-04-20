using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000558 RID: 1368
	public sealed class DateTimeAutomationPeer : AutomationPeer, IGridItemProvider, ISelectionItemProvider, ITableItemProvider, IInvokeProvider, IVirtualizedItemProvider
	{
		// Token: 0x0600439A RID: 17306 RVA: 0x0021EC84 File Offset: 0x0021DC84
		internal DateTimeAutomationPeer(DateTime date, Calendar owningCalendar, CalendarMode buttonMode)
		{
			if (owningCalendar == null)
			{
				throw new ArgumentNullException("owningCalendar");
			}
			this.Date = date;
			this.ButtonMode = buttonMode;
			this.OwningCalendar = owningCalendar;
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x0600439B RID: 17307 RVA: 0x0021DD4D File Offset: 0x0021CD4D
		// (set) Token: 0x0600439C RID: 17308 RVA: 0x0021ECB0 File Offset: 0x0021DCB0
		internal override bool AncestorsInvalid
		{
			get
			{
				return base.AncestorsInvalid;
			}
			set
			{
				base.AncestorsInvalid = value;
				if (value)
				{
					return;
				}
				AutomationPeer wrapperPeer = this.WrapperPeer;
				if (wrapperPeer != null)
				{
					wrapperPeer.AncestorsInvalid = false;
				}
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x0600439D RID: 17309 RVA: 0x0021ECD9 File Offset: 0x0021DCD9
		// (set) Token: 0x0600439E RID: 17310 RVA: 0x0021ECE1 File Offset: 0x0021DCE1
		private Calendar OwningCalendar { get; set; }

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x0600439F RID: 17311 RVA: 0x0021ECEA File Offset: 0x0021DCEA
		// (set) Token: 0x060043A0 RID: 17312 RVA: 0x0021ECF2 File Offset: 0x0021DCF2
		internal DateTime Date { get; private set; }

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x060043A1 RID: 17313 RVA: 0x0021ECFB File Offset: 0x0021DCFB
		// (set) Token: 0x060043A2 RID: 17314 RVA: 0x0021ED03 File Offset: 0x0021DD03
		internal CalendarMode ButtonMode { get; private set; }

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x060043A3 RID: 17315 RVA: 0x0021ED0C File Offset: 0x0021DD0C
		internal bool IsDayButton
		{
			get
			{
				return this.ButtonMode == CalendarMode.Month;
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x060043A4 RID: 17316 RVA: 0x0021ED18 File Offset: 0x0021DD18
		private IRawElementProviderSimple OwningCalendarProvider
		{
			get
			{
				if (this.OwningCalendar != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar);
					if (automationPeer != null)
					{
						return base.ProviderFromPeer(automationPeer);
					}
				}
				return null;
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x060043A5 RID: 17317 RVA: 0x0021ED48 File Offset: 0x0021DD48
		internal Button OwningButton
		{
			get
			{
				if (this.OwningCalendar.DisplayMode != this.ButtonMode)
				{
					return null;
				}
				if (this.IsDayButton)
				{
					CalendarItem monthControl = this.OwningCalendar.MonthControl;
					if (monthControl == null)
					{
						return null;
					}
					return monthControl.GetCalendarDayButton(this.Date);
				}
				else
				{
					CalendarItem monthControl2 = this.OwningCalendar.MonthControl;
					if (monthControl2 == null)
					{
						return null;
					}
					return monthControl2.GetCalendarButton(this.Date, this.ButtonMode);
				}
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x060043A6 RID: 17318 RVA: 0x0021EDB4 File Offset: 0x0021DDB4
		internal FrameworkElementAutomationPeer WrapperPeer
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton != null)
				{
					return UIElementAutomationPeer.CreatePeerForElement(owningButton) as FrameworkElementAutomationPeer;
				}
				return null;
			}
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x0021EDD8 File Offset: 0x0021DDD8
		protected override string GetAcceleratorKeyCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAcceleratorKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x0021EE04 File Offset: 0x0021DE04
		protected override string GetAccessKeyCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAccessKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x0021EE30 File Offset: 0x0021DE30
		protected override string GetAutomationIdCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAutomationId();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x0021EE5C File Offset: 0x0021DE5C
		protected override Rect GetBoundingRectangleCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetBoundingRectangle();
			}
			this.ThrowElementNotAvailableException();
			return default(Rect);
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x0021EE8C File Offset: 0x0021DE8C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetChildren();
			}
			this.ThrowElementNotAvailableException();
			return null;
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0021EEB4 File Offset: 0x0021DEB4
		protected override string GetClassNameCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClassName();
			}
			if (!this.IsDayButton)
			{
				return "CalendarButton";
			}
			return "CalendarDayButton";
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x0021EEE8 File Offset: 0x0021DEE8
		protected override Point GetClickablePointCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClickablePoint();
			}
			this.ThrowElementNotAvailableException();
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x0021EF24 File Offset: 0x0021DF24
		protected override string GetHelpTextCore()
		{
			string text = DateTimeHelper.ToLongDateString(new DateTime?(this.Date), DateTimeHelper.GetCulture(this.OwningCalendar));
			if (this.IsDayButton && this.OwningCalendar.BlackoutDates.Contains(this.Date))
			{
				return string.Format(DateTimeHelper.GetCurrentDateFormat(), SR.Get("CalendarAutomationPeer_BlackoutDayHelpText"), text);
			}
			return text;
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x0021EF84 File Offset: 0x0021DF84
		protected override string GetItemStatusCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetItemStatus();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x0021EFB0 File Offset: 0x0021DFB0
		protected override string GetItemTypeCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetItemType();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x0021EFDC File Offset: 0x0021DFDC
		protected override AutomationPeer GetLabeledByCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetLabeledBy();
			}
			this.ThrowElementNotAvailableException();
			return null;
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x0021F004 File Offset: 0x0021E004
		protected override AutomationLiveSetting GetLiveSettingCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetLiveSetting();
			}
			this.ThrowElementNotAvailableException();
			return AutomationLiveSetting.Off;
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0021F029 File Offset: 0x0021E029
		protected override string GetLocalizedControlTypeCore()
		{
			if (!this.IsDayButton)
			{
				return SR.Get("CalendarAutomationPeer_CalendarButtonLocalizedControlType");
			}
			return SR.Get("CalendarAutomationPeer_DayButtonLocalizedControlType");
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x0021F048 File Offset: 0x0021E048
		protected override string GetNameCore()
		{
			string result = "";
			switch (this.ButtonMode)
			{
			case CalendarMode.Month:
				result = DateTimeHelper.ToLongDateString(new DateTime?(this.Date), DateTimeHelper.GetCulture(this.OwningCalendar));
				break;
			case CalendarMode.Year:
				result = DateTimeHelper.ToYearMonthPatternString(new DateTime?(this.Date), DateTimeHelper.GetCulture(this.OwningCalendar));
				break;
			case CalendarMode.Decade:
				result = DateTimeHelper.ToYearString(new DateTime?(this.Date), DateTimeHelper.GetCulture(this.OwningCalendar));
				break;
			}
			return result;
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x0021F0D0 File Offset: 0x0021E0D0
		protected override AutomationOrientation GetOrientationCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetOrientation();
			}
			this.ThrowElementNotAvailableException();
			return AutomationOrientation.None;
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0021F0F8 File Offset: 0x0021E0F8
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			Button owningButton = this.OwningButton;
			if (patternInterface <= PatternInterface.GridItem)
			{
				if (patternInterface == PatternInterface.Invoke || patternInterface == PatternInterface.GridItem)
				{
					if (owningButton != null)
					{
						result = this;
					}
				}
			}
			else if (patternInterface != PatternInterface.SelectionItem)
			{
				if (patternInterface != PatternInterface.TableItem)
				{
					if (patternInterface == PatternInterface.VirtualizedItem)
					{
						if (VirtualizedItemPatternIdentifiers.Pattern != null)
						{
							if (owningButton == null)
							{
								result = this;
							}
							else if (!this.IsItemInAutomationTree())
							{
								return this;
							}
						}
					}
				}
				else if (this.IsDayButton && owningButton != null)
				{
					result = this;
				}
			}
			else
			{
				result = this;
			}
			return result;
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0021F160 File Offset: 0x0021E160
		protected override int GetPositionInSetCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetPositionInSet();
			}
			this.ThrowElementNotAvailableException();
			return -1;
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x0021F188 File Offset: 0x0021E188
		protected override int GetSizeOfSetCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetSizeOfSet();
			}
			this.ThrowElementNotAvailableException();
			return -1;
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0021F1B0 File Offset: 0x0021E1B0
		internal override Rect GetVisibleBoundingRectCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer == null)
			{
				return base.GetBoundingRectangle();
			}
			return wrapperPeer.GetVisibleBoundingRect();
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x0021F1D4 File Offset: 0x0021E1D4
		protected override bool HasKeyboardFocusCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.HasKeyboardFocus();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x0021F1FC File Offset: 0x0021E1FC
		protected override bool IsContentElementCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsContentElement();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x0021F224 File Offset: 0x0021E224
		protected override bool IsControlElementCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsControlElement();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x0021F24C File Offset: 0x0021E24C
		protected override bool IsEnabledCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsEnabled();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x0021F274 File Offset: 0x0021E274
		protected override bool IsKeyboardFocusableCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsKeyboardFocusable();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x0021F29C File Offset: 0x0021E29C
		protected override bool IsOffscreenCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsOffscreen();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x0021F2C4 File Offset: 0x0021E2C4
		protected override bool IsPasswordCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsPassword();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x0021F2EC File Offset: 0x0021E2EC
		protected override bool IsRequiredForFormCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsRequiredForForm();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x0021F314 File Offset: 0x0021E314
		protected override void SetFocusCore()
		{
			UIElementAutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				wrapperPeer.SetFocus();
				return;
			}
			this.ThrowElementNotAvailableException();
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x060043C4 RID: 17348 RVA: 0x0021F338 File Offset: 0x0021E338
		int IGridItemProvider.Column
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton != null)
				{
					return (int)owningButton.GetValue(Grid.ColumnProperty);
				}
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x060043C5 RID: 17349 RVA: 0x0021F370 File Offset: 0x0021E370
		int IGridItemProvider.ColumnSpan
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton != null)
				{
					return (int)owningButton.GetValue(Grid.ColumnSpanProperty);
				}
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x060043C6 RID: 17350 RVA: 0x0021F3A7 File Offset: 0x0021E3A7
		IRawElementProviderSimple IGridItemProvider.ContainingGrid
		{
			get
			{
				return this.OwningCalendarProvider;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x060043C7 RID: 17351 RVA: 0x0021F3B0 File Offset: 0x0021E3B0
		int IGridItemProvider.Row
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton == null)
				{
					throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
				}
				if (this.IsDayButton)
				{
					return (int)owningButton.GetValue(Grid.RowProperty) - 1;
				}
				return (int)owningButton.GetValue(Grid.RowProperty);
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x060043C8 RID: 17352 RVA: 0x0021F404 File Offset: 0x0021E404
		int IGridItemProvider.RowSpan
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton == null)
				{
					throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
				}
				if (this.IsDayButton)
				{
					return (int)owningButton.GetValue(Grid.RowSpanProperty);
				}
				return 1;
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x060043C9 RID: 17353 RVA: 0x0021F445 File Offset: 0x0021E445
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return this.IsDayButton && this.OwningCalendar.SelectedDates.Contains(this.Date);
			}
		}

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x060043CA RID: 17354 RVA: 0x0021F3A7 File Offset: 0x0021E3A7
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return this.OwningCalendarProvider;
			}
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x0021F468 File Offset: 0x0021E468
		void ISelectionItemProvider.AddToSelection()
		{
			if (((ISelectionItemProvider)this).IsSelected)
			{
				return;
			}
			if (this.IsDayButton && this.EnsureSelection())
			{
				if (this.OwningCalendar.SelectionMode == CalendarSelectionMode.SingleDate)
				{
					this.OwningCalendar.SelectedDate = new DateTime?(this.Date);
					return;
				}
				this.OwningCalendar.SelectedDates.Add(this.Date);
			}
		}

		// Token: 0x060043CC RID: 17356 RVA: 0x0021F4C8 File Offset: 0x0021E4C8
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!((ISelectionItemProvider)this).IsSelected)
			{
				return;
			}
			if (this.IsDayButton)
			{
				this.OwningCalendar.SelectedDates.Remove(this.Date);
			}
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x0021F4F4 File Offset: 0x0021E4F4
		void ISelectionItemProvider.Select()
		{
			Button owningButton = this.OwningButton;
			if (this.IsDayButton)
			{
				if (this.EnsureSelection() && this.OwningCalendar.SelectionMode == CalendarSelectionMode.SingleDate)
				{
					this.OwningCalendar.SelectedDate = new DateTime?(this.Date);
					return;
				}
			}
			else if (owningButton != null && owningButton.IsEnabled)
			{
				owningButton.Focus();
			}
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x0021F550 File Offset: 0x0021E550
		IRawElementProviderSimple[] ITableItemProvider.GetColumnHeaderItems()
		{
			if (this.IsDayButton && this.OwningButton != null && this.OwningCalendar != null && this.OwningCalendarProvider != null)
			{
				IRawElementProviderSimple[] columnHeaders = ((ITableProvider)UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar)).GetColumnHeaders();
				if (columnHeaders != null)
				{
					int column = ((IGridItemProvider)this).Column;
					return new IRawElementProviderSimple[]
					{
						columnHeaders[column]
					};
				}
			}
			return null;
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x00109403 File Offset: 0x00108403
		IRawElementProviderSimple[] ITableItemProvider.GetRowHeaderItems()
		{
			return null;
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0021F5AC File Offset: 0x0021E5AC
		void IInvokeProvider.Invoke()
		{
			Button owningButton = this.OwningButton;
			if (owningButton == null || !base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				owningButton.AutomationButtonBaseClick();
				return null;
			}), null);
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x0021F5FB File Offset: 0x0021E5FB
		void IVirtualizedItemProvider.Realize()
		{
			if (this.OwningCalendar.DisplayMode != this.ButtonMode)
			{
				this.OwningCalendar.DisplayMode = this.ButtonMode;
			}
			this.OwningCalendar.DisplayDate = this.Date;
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsDataItemAutomationPeer()
		{
			return true;
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0021F634 File Offset: 0x0021E634
		internal override void AddToParentProxyWeakRefCache()
		{
			CalendarAutomationPeer calendarAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar) as CalendarAutomationPeer;
			if (calendarAutomationPeer != null)
			{
				calendarAutomationPeer.AddProxyToWeakRefStorage(base.ElementProxyWeakReference, this);
			}
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x0021F662 File Offset: 0x0021E662
		private bool EnsureSelection()
		{
			return !this.OwningCalendar.BlackoutDates.Contains(this.Date) && this.OwningCalendar.SelectionMode != CalendarSelectionMode.None;
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x0021DBC0 File Offset: 0x0021CBC0
		private bool IsItemInAutomationTree()
		{
			AutomationPeer parent = base.GetParent();
			return base.Index != -1 && parent != null && parent.Children != null && base.Index < parent.Children.Count && parent.Children[base.Index] == this;
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x0021F68D File Offset: 0x0021E68D
		private void ThrowElementNotAvailableException()
		{
			if (VirtualizedItemPatternIdentifiers.Pattern != null)
			{
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}
	}
}
