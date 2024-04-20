using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000546 RID: 1350
	public sealed class CalendarAutomationPeer : FrameworkElementAutomationPeer, IGridProvider, IMultipleViewProvider, ISelectionProvider, ITableProvider, IItemContainerProvider
	{
		// Token: 0x060042A3 RID: 17059 RVA: 0x0021BCE6 File Offset: 0x0021ACE6
		public CalendarAutomationPeer(Calendar owner) : base(owner)
		{
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x060042A4 RID: 17060 RVA: 0x0021BD05 File Offset: 0x0021AD05
		private Calendar OwningCalendar
		{
			get
			{
				return base.Owner as Calendar;
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x060042A5 RID: 17061 RVA: 0x0021BD14 File Offset: 0x0021AD14
		private Grid OwningGrid
		{
			get
			{
				if (this.OwningCalendar == null || this.OwningCalendar.MonthControl == null)
				{
					return null;
				}
				if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
				{
					return this.OwningCalendar.MonthControl.MonthView;
				}
				return this.OwningCalendar.MonthControl.YearView;
			}
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x0021BD66 File Offset: 0x0021AD66
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.Grid)
			{
				if (patternInterface != PatternInterface.Selection && patternInterface != PatternInterface.Grid)
				{
					goto IL_27;
				}
			}
			else if (patternInterface != PatternInterface.MultipleView && patternInterface != PatternInterface.Table && patternInterface != PatternInterface.ItemContainer)
			{
				goto IL_27;
			}
			if (this.OwningGrid != null)
			{
				return this;
			}
			IL_27:
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Calendar;
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x0021BD98 File Offset: 0x0021AD98
		protected override List<AutomationPeer> GetChildrenCore()
		{
			if (this.OwningCalendar.MonthControl == null)
			{
				return null;
			}
			List<AutomationPeer> list = new List<AutomationPeer>();
			Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer> dictionary = new Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer>();
			AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar.MonthControl.PreviousButton);
			if (automationPeer != null)
			{
				list.Add(automationPeer);
			}
			automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar.MonthControl.HeaderButton);
			if (automationPeer != null)
			{
				list.Add(automationPeer);
			}
			automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar.MonthControl.NextButton);
			if (automationPeer != null)
			{
				list.Add(automationPeer);
			}
			foreach (object obj in this.OwningGrid.Children)
			{
				UIElement uielement = (UIElement)obj;
				int num = (int)uielement.GetValue(Grid.RowProperty);
				if (this.OwningCalendar.DisplayMode == CalendarMode.Month && num == 0)
				{
					AutomationPeer automationPeer2 = UIElementAutomationPeer.CreatePeerForElement(uielement);
					if (automationPeer2 != null)
					{
						list.Add(automationPeer2);
					}
				}
				else
				{
					Button button = uielement as Button;
					if (button != null && button.DataContext is DateTime)
					{
						DateTime date = (DateTime)button.DataContext;
						DateTimeAutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer(date, this.OwningCalendar.DisplayMode, false);
						list.Add(orCreateDateTimeAutomationPeer);
						DateTimeCalendarModePair key = new DateTimeCalendarModePair(date, this.OwningCalendar.DisplayMode);
						dictionary.Add(key, orCreateDateTimeAutomationPeer);
					}
				}
			}
			this.DateTimePeers = dictionary;
			return list;
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x0021BF34 File Offset: 0x0021AF34
		protected override void SetFocusCore()
		{
			Calendar owningCalendar = this.OwningCalendar;
			if (owningCalendar.Focusable)
			{
				if (!owningCalendar.Focus())
				{
					DateTime date;
					if (owningCalendar.SelectedDate != null && DateTimeHelper.CompareYearMonth(owningCalendar.SelectedDate.Value, owningCalendar.DisplayDateInternal) == 0)
					{
						date = owningCalendar.SelectedDate.Value;
					}
					else
					{
						date = owningCalendar.DisplayDate;
					}
					FrameworkElement owningButton = this.GetOrCreateDateTimeAutomationPeer(date, owningCalendar.DisplayMode, false).OwningButton;
					if (owningButton == null || !owningButton.IsKeyboardFocused)
					{
						throw new InvalidOperationException(SR.Get("SetFocusFailed"));
					}
				}
				return;
			}
			throw new InvalidOperationException(SR.Get("SetFocusFailed"));
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x0021BFDD File Offset: 0x0021AFDD
		private DateTimeAutomationPeer GetOrCreateDateTimeAutomationPeer(DateTime date, CalendarMode buttonMode)
		{
			return this.GetOrCreateDateTimeAutomationPeer(date, buttonMode, true);
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x0021BFE8 File Offset: 0x0021AFE8
		private DateTimeAutomationPeer GetOrCreateDateTimeAutomationPeer(DateTime date, CalendarMode buttonMode, bool addParentInfo)
		{
			DateTimeCalendarModePair dateTimeCalendarModePair = new DateTimeCalendarModePair(date, buttonMode);
			DateTimeAutomationPeer dateTimeAutomationPeer = null;
			this.DateTimePeers.TryGetValue(dateTimeCalendarModePair, out dateTimeAutomationPeer);
			if (dateTimeAutomationPeer == null)
			{
				dateTimeAutomationPeer = this.GetPeerFromWeakRefStorage(dateTimeCalendarModePair);
				if (dateTimeAutomationPeer != null && !addParentInfo)
				{
					dateTimeAutomationPeer.AncestorsInvalid = false;
					dateTimeAutomationPeer.ChildrenValid = false;
				}
			}
			if (dateTimeAutomationPeer == null)
			{
				dateTimeAutomationPeer = new DateTimeAutomationPeer(date, this.OwningCalendar, buttonMode);
				if (addParentInfo && dateTimeAutomationPeer != null)
				{
					dateTimeAutomationPeer.TrySetParentInfo(this);
				}
			}
			AutomationPeer wrapperPeer = dateTimeAutomationPeer.WrapperPeer;
			if (wrapperPeer != null)
			{
				wrapperPeer.EventsSource = dateTimeAutomationPeer;
			}
			return dateTimeAutomationPeer;
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x0021C060 File Offset: 0x0021B060
		private DateTimeAutomationPeer GetPeerFromWeakRefStorage(DateTimeCalendarModePair dateTimeCalendarModePairKey)
		{
			DateTimeAutomationPeer dateTimeAutomationPeer = null;
			WeakReference weakReference = null;
			this.WeakRefElementProxyStorage.TryGetValue(dateTimeCalendarModePairKey, out weakReference);
			if (weakReference != null)
			{
				ElementProxy elementProxy = weakReference.Target as ElementProxy;
				if (elementProxy != null)
				{
					dateTimeAutomationPeer = (base.PeerFromProvider(elementProxy) as DateTimeAutomationPeer);
					if (dateTimeAutomationPeer == null)
					{
						this.WeakRefElementProxyStorage.Remove(dateTimeCalendarModePairKey);
					}
				}
				else
				{
					this.WeakRefElementProxyStorage.Remove(dateTimeCalendarModePairKey);
				}
			}
			return dateTimeAutomationPeer;
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x0021C0C0 File Offset: 0x0021B0C0
		internal void AddProxyToWeakRefStorage(WeakReference wr, DateTimeAutomationPeer dateTimePeer)
		{
			DateTimeCalendarModePair dateTimeCalendarModePair = new DateTimeCalendarModePair(dateTimePeer.Date, dateTimePeer.ButtonMode);
			if (this.GetPeerFromWeakRefStorage(dateTimeCalendarModePair) == null)
			{
				this.WeakRefElementProxyStorage.Add(dateTimeCalendarModePair, wr);
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x0021C0F8 File Offset: 0x0021B0F8
		internal void RaiseSelectionEvents(SelectionChangedEventArgs e)
		{
			int count = this.OwningCalendar.SelectedDates.Count;
			int count2 = e.AddedItems.Count;
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) && count == 1 && count2 == 1)
			{
				DateTimeAutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer((DateTime)e.AddedItems[0], CalendarMode.Month);
				if (orCreateDateTimeAutomationPeer != null)
				{
					orCreateDateTimeAutomationPeer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
				}
			}
			else if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection))
			{
				foreach (object obj in e.AddedItems)
				{
					DateTime date = (DateTime)obj;
					DateTimeAutomationPeer orCreateDateTimeAutomationPeer2 = this.GetOrCreateDateTimeAutomationPeer(date, CalendarMode.Month);
					if (orCreateDateTimeAutomationPeer2 != null)
					{
						orCreateDateTimeAutomationPeer2.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
					}
				}
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				foreach (object obj2 in e.RemovedItems)
				{
					DateTime date2 = (DateTime)obj2;
					DateTimeAutomationPeer orCreateDateTimeAutomationPeer3 = this.GetOrCreateDateTimeAutomationPeer(date2, CalendarMode.Month);
					if (orCreateDateTimeAutomationPeer3 != null)
					{
						orCreateDateTimeAutomationPeer3.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
					}
				}
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x060042B0 RID: 17072 RVA: 0x0021C224 File Offset: 0x0021B224
		int IGridProvider.ColumnCount
		{
			get
			{
				if (this.OwningGrid != null)
				{
					return this.OwningGrid.ColumnDefinitions.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x060042B1 RID: 17073 RVA: 0x0021C240 File Offset: 0x0021B240
		int IGridProvider.RowCount
		{
			get
			{
				if (this.OwningGrid == null)
				{
					return 0;
				}
				if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
				{
					return Math.Max(0, this.OwningGrid.RowDefinitions.Count - 1);
				}
				return this.OwningGrid.RowDefinitions.Count;
			}
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x0021C290 File Offset: 0x0021B290
		IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
		{
			if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
			{
				row++;
			}
			if (this.OwningGrid != null && row >= 0 && row < this.OwningGrid.RowDefinitions.Count && column >= 0 && column < this.OwningGrid.ColumnDefinitions.Count)
			{
				foreach (object obj in this.OwningGrid.Children)
				{
					UIElement uielement = (UIElement)obj;
					int num = (int)uielement.GetValue(Grid.RowProperty);
					int num2 = (int)uielement.GetValue(Grid.ColumnProperty);
					if (num == row && num2 == column)
					{
						object dataContext = (uielement as FrameworkElement).DataContext;
						if (dataContext is DateTime)
						{
							DateTime date = (DateTime)dataContext;
							AutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer(date, this.OwningCalendar.DisplayMode);
							return base.ProviderFromPeer(orCreateDateTimeAutomationPeer);
						}
					}
				}
			}
			return null;
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x060042B3 RID: 17075 RVA: 0x0021C3B0 File Offset: 0x0021B3B0
		int IMultipleViewProvider.CurrentView
		{
			get
			{
				return (int)this.OwningCalendar.DisplayMode;
			}
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x0021C3BD File Offset: 0x0021B3BD
		int[] IMultipleViewProvider.GetSupportedViews()
		{
			return new int[]
			{
				0,
				1,
				2
			};
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x0021C3D1 File Offset: 0x0021B3D1
		string IMultipleViewProvider.GetViewName(int viewId)
		{
			switch (viewId)
			{
			case 0:
				return SR.Get("CalendarAutomationPeer_MonthMode");
			case 1:
				return SR.Get("CalendarAutomationPeer_YearMode");
			case 2:
				return SR.Get("CalendarAutomationPeer_DecadeMode");
			default:
				return string.Empty;
			}
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x0021C40D File Offset: 0x0021B40D
		void IMultipleViewProvider.SetCurrentView(int viewId)
		{
			this.OwningCalendar.DisplayMode = (CalendarMode)viewId;
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x060042B7 RID: 17079 RVA: 0x0021C41B File Offset: 0x0021B41B
		bool ISelectionProvider.CanSelectMultiple
		{
			get
			{
				return this.OwningCalendar.SelectionMode == CalendarSelectionMode.SingleRange || this.OwningCalendar.SelectionMode == CalendarSelectionMode.MultipleRange;
			}
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x060042B8 RID: 17080 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x0021C43C File Offset: 0x0021B43C
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
			foreach (DateTime date in this.OwningCalendar.SelectedDates)
			{
				AutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer(date, CalendarMode.Month);
				list.Add(base.ProviderFromPeer(orCreateDateTimeAutomationPeer));
			}
			if (list.Count > 0)
			{
				return list.ToArray();
			}
			return null;
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x0021C4B4 File Offset: 0x0021B4B4
		IRawElementProviderSimple IItemContainerProvider.FindItemByProperty(IRawElementProviderSimple startAfterProvider, int propertyId, object value)
		{
			DateTimeAutomationPeer dateTimeAutomationPeer = null;
			if (startAfterProvider != null)
			{
				dateTimeAutomationPeer = (base.PeerFromProvider(startAfterProvider) as DateTimeAutomationPeer);
				if (dateTimeAutomationPeer == null)
				{
					throw new InvalidOperationException(SR.Get("InavalidStartItem"));
				}
			}
			DateTime? dateTime = null;
			CalendarMode calendarMode;
			if (propertyId == SelectionItemPatternIdentifiers.IsSelectedProperty.Id)
			{
				calendarMode = CalendarMode.Month;
				dateTime = this.GetNextSelectedDate(dateTimeAutomationPeer, (bool)value);
			}
			else if (propertyId == AutomationElementIdentifiers.NameProperty.Id)
			{
				DateTimeFormatInfo currentDateFormat = DateTimeHelper.GetCurrentDateFormat();
				DateTime value2;
				if (DateTime.TryParse(value as string, currentDateFormat, DateTimeStyles.None, out value2))
				{
					dateTime = new DateTime?(value2);
				}
				if (dateTime == null || (dateTimeAutomationPeer != null && dateTime <= dateTimeAutomationPeer.Date))
				{
					throw new InvalidOperationException(SR.Get("CalendarNamePropertyValueNotValid"));
				}
				calendarMode = ((dateTimeAutomationPeer != null) ? dateTimeAutomationPeer.ButtonMode : this.OwningCalendar.DisplayMode);
			}
			else
			{
				if (propertyId != 0 && propertyId != AutomationElementIdentifiers.ControlTypeProperty.Id)
				{
					throw new ArgumentException(SR.Get("PropertyNotSupported"));
				}
				if (propertyId == AutomationElementIdentifiers.ControlTypeProperty.Id && (int)value != ControlType.Button.Id)
				{
					return null;
				}
				calendarMode = ((dateTimeAutomationPeer != null) ? dateTimeAutomationPeer.ButtonMode : this.OwningCalendar.DisplayMode);
				dateTime = this.GetNextDate(dateTimeAutomationPeer, calendarMode);
			}
			if (dateTime != null)
			{
				AutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer(dateTime.Value, calendarMode);
				if (orCreateDateTimeAutomationPeer != null)
				{
					return base.ProviderFromPeer(orCreateDateTimeAutomationPeer);
				}
			}
			return null;
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x0021C628 File Offset: 0x0021B628
		private DateTime? GetNextDate(DateTimeAutomationPeer currentDatePeer, CalendarMode currentMode)
		{
			DateTime? result = null;
			DateTime dateTime = (currentDatePeer != null) ? currentDatePeer.Date : this.OwningCalendar.DisplayDate;
			if (currentMode == CalendarMode.Month)
			{
				result = new DateTime?(dateTime.AddDays(1.0));
			}
			else if (currentMode == CalendarMode.Year)
			{
				result = new DateTime?(dateTime.AddMonths(1));
			}
			else if (currentMode == CalendarMode.Decade)
			{
				result = new DateTime?(dateTime.AddYears(1));
			}
			return result;
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x0021C69C File Offset: 0x0021B69C
		private DateTime? GetNextSelectedDate(DateTimeAutomationPeer currentDatePeer, bool isSelected)
		{
			DateTime dateTime = (currentDatePeer != null) ? currentDatePeer.Date : this.OwningCalendar.DisplayDate;
			if (isSelected)
			{
				if (this.OwningCalendar.SelectedDates.MaximumDate == null || this.OwningCalendar.SelectedDates.MaximumDate <= dateTime)
				{
					return null;
				}
				if (this.OwningCalendar.SelectedDates.MinimumDate != null && dateTime < this.OwningCalendar.SelectedDates.MinimumDate)
				{
					return this.OwningCalendar.SelectedDates.MinimumDate;
				}
			}
			do
			{
				dateTime = dateTime.AddDays(1.0);
			}
			while (this.OwningCalendar.SelectedDates.Contains(dateTime) != isSelected);
			return new DateTime?(dateTime);
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x060042BD RID: 17085 RVA: 0x00105F35 File Offset: 0x00104F35
		RowOrColumnMajor ITableProvider.RowOrColumnMajor
		{
			get
			{
				return RowOrColumnMajor.RowMajor;
			}
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x0021C79C File Offset: 0x0021B79C
		IRawElementProviderSimple[] ITableProvider.GetColumnHeaders()
		{
			if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
				foreach (object obj in this.OwningGrid.Children)
				{
					UIElement uielement = (UIElement)obj;
					if ((int)uielement.GetValue(Grid.RowProperty) == 0)
					{
						AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(uielement);
						if (automationPeer != null)
						{
							list.Add(base.ProviderFromPeer(automationPeer));
						}
					}
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x00109403 File Offset: 0x00108403
		IRawElementProviderSimple[] ITableProvider.GetRowHeaders()
		{
			return null;
		}

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x060042C0 RID: 17088 RVA: 0x0021C844 File Offset: 0x0021B844
		// (set) Token: 0x060042C1 RID: 17089 RVA: 0x0021C84C File Offset: 0x0021B84C
		private Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer> DateTimePeers
		{
			get
			{
				return this._dataChildren;
			}
			set
			{
				this._dataChildren = value;
			}
		}

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x060042C2 RID: 17090 RVA: 0x0021C855 File Offset: 0x0021B855
		private Dictionary<DateTimeCalendarModePair, WeakReference> WeakRefElementProxyStorage
		{
			get
			{
				return this._weakRefElementProxyStorage;
			}
		}

		// Token: 0x04002511 RID: 9489
		private Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer> _dataChildren = new Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer>();

		// Token: 0x04002512 RID: 9490
		private Dictionary<DateTimeCalendarModePair, WeakReference> _weakRefElementProxyStorage = new Dictionary<DateTimeCalendarModePair, WeakReference>();
	}
}
