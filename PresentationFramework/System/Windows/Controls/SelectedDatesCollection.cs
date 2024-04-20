using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace System.Windows.Controls
{
	// Token: 0x020007D1 RID: 2001
	public sealed class SelectedDatesCollection : ObservableCollection<DateTime>
	{
		// Token: 0x060072C7 RID: 29383 RVA: 0x002E03D7 File Offset: 0x002DF3D7
		public SelectedDatesCollection(Calendar owner)
		{
			this._dispatcherThread = Thread.CurrentThread;
			this._owner = owner;
			this._addedItems = new Collection<DateTime>();
			this._removedItems = new Collection<DateTime>();
		}

		// Token: 0x17001A9D RID: 6813
		// (get) Token: 0x060072C8 RID: 29384 RVA: 0x002E0408 File Offset: 0x002DF408
		internal DateTime? MinimumDate
		{
			get
			{
				if (base.Count < 1)
				{
					return null;
				}
				if (this._minimumDate == null)
				{
					DateTime dateTime = base[0];
					foreach (DateTime dateTime2 in this)
					{
						if (DateTime.Compare(dateTime2, dateTime) < 0)
						{
							dateTime = dateTime2;
						}
					}
					this._maximumDate = new DateTime?(dateTime);
				}
				return this._minimumDate;
			}
		}

		// Token: 0x17001A9E RID: 6814
		// (get) Token: 0x060072C9 RID: 29385 RVA: 0x002E0490 File Offset: 0x002DF490
		internal DateTime? MaximumDate
		{
			get
			{
				if (base.Count < 1)
				{
					return null;
				}
				if (this._maximumDate == null)
				{
					DateTime dateTime = base[0];
					foreach (DateTime dateTime2 in this)
					{
						if (DateTime.Compare(dateTime2, dateTime) > 0)
						{
							dateTime = dateTime2;
						}
					}
					this._maximumDate = new DateTime?(dateTime);
				}
				return this._maximumDate;
			}
		}

		// Token: 0x060072CA RID: 29386 RVA: 0x002E0518 File Offset: 0x002DF518
		public void AddRange(DateTime start, DateTime end)
		{
			this.BeginAddRange();
			if (this._owner.SelectionMode == CalendarSelectionMode.SingleRange && base.Count > 0)
			{
				this.ClearInternal();
			}
			foreach (DateTime item in SelectedDatesCollection.GetDaysInRange(start, end))
			{
				base.Add(item);
			}
			this.EndAddRange();
		}

		// Token: 0x060072CB RID: 29387 RVA: 0x002E0590 File Offset: 0x002DF590
		protected override void ClearItems()
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			this._owner.HoverStart = null;
			this.ClearInternal(true);
		}

		// Token: 0x060072CC RID: 29388 RVA: 0x002E05D0 File Offset: 0x002DF5D0
		protected override void InsertItem(int index, DateTime item)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (!base.Contains(item))
			{
				Collection<DateTime> collection = new Collection<DateTime>();
				bool flag = this.CheckSelectionMode();
				if (!Calendar.IsValidDateSelection(this._owner, item))
				{
					throw new ArgumentOutOfRangeException(SR.Get("Calendar_OnSelectedDateChanged_InvalidValue"));
				}
				if (flag)
				{
					index = 0;
				}
				base.InsertItem(index, item);
				this.UpdateMinMax(item);
				if (index == 0 && (this._owner.SelectedDate == null || DateTime.Compare(this._owner.SelectedDate.Value, item) != 0))
				{
					this._owner.SelectedDate = new DateTime?(item);
				}
				if (this._isAddingRange)
				{
					this._addedItems.Add(item);
					return;
				}
				collection.Add(item);
				this.RaiseSelectionChanged(this._removedItems, collection);
				this._removedItems.Clear();
				int num = DateTimeHelper.CompareYearMonth(item, this._owner.DisplayDateInternal);
				if (num < 2 && num > -2)
				{
					this._owner.UpdateCellItems();
					return;
				}
			}
		}

		// Token: 0x060072CD RID: 29389 RVA: 0x002E06EC File Offset: 0x002DF6EC
		protected override void RemoveItem(int index)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (index >= base.Count)
			{
				base.RemoveItem(index);
				this.ClearMinMax();
				return;
			}
			Collection<DateTime> addedItems = new Collection<DateTime>();
			Collection<DateTime> collection = new Collection<DateTime>();
			int num = DateTimeHelper.CompareYearMonth(base[index], this._owner.DisplayDateInternal);
			collection.Add(base[index]);
			base.RemoveItem(index);
			this.ClearMinMax();
			if (index == 0)
			{
				if (base.Count > 0)
				{
					this._owner.SelectedDate = new DateTime?(base[0]);
				}
				else
				{
					this._owner.SelectedDate = null;
				}
			}
			this.RaiseSelectionChanged(collection, addedItems);
			if (num < 2 && num > -2)
			{
				this._owner.UpdateCellItems();
			}
		}

		// Token: 0x060072CE RID: 29390 RVA: 0x002E07BC File Offset: 0x002DF7BC
		protected override void SetItem(int index, DateTime item)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (!base.Contains(item))
			{
				Collection<DateTime> collection = new Collection<DateTime>();
				Collection<DateTime> collection2 = new Collection<DateTime>();
				if (index >= base.Count)
				{
					base.SetItem(index, item);
					this.UpdateMinMax(item);
					return;
				}
				if (DateTime.Compare(base[index], item) != 0 && Calendar.IsValidDateSelection(this._owner, item))
				{
					collection2.Add(base[index]);
					base.SetItem(index, item);
					this.UpdateMinMax(item);
					collection.Add(item);
					if (index == 0 && (this._owner.SelectedDate == null || DateTime.Compare(this._owner.SelectedDate.Value, item) != 0))
					{
						this._owner.SelectedDate = new DateTime?(item);
					}
					this.RaiseSelectionChanged(collection2, collection);
					int num = DateTimeHelper.CompareYearMonth(item, this._owner.DisplayDateInternal);
					if (num < 2 && num > -2)
					{
						this._owner.UpdateCellItems();
					}
				}
			}
		}

		// Token: 0x060072CF RID: 29391 RVA: 0x002E08D0 File Offset: 0x002DF8D0
		internal void AddRangeInternal(DateTime start, DateTime end)
		{
			this.BeginAddRange();
			DateTime currentDate = start;
			foreach (DateTime dateTime in SelectedDatesCollection.GetDaysInRange(start, end))
			{
				if (Calendar.IsValidDateSelection(this._owner, dateTime))
				{
					base.Add(dateTime);
					currentDate = dateTime;
				}
				else if (this._owner.SelectionMode == CalendarSelectionMode.SingleRange)
				{
					this._owner.CurrentDate = currentDate;
					break;
				}
			}
			this.EndAddRange();
		}

		// Token: 0x060072D0 RID: 29392 RVA: 0x002E0960 File Offset: 0x002DF960
		internal void ClearInternal()
		{
			this.ClearInternal(false);
		}

		// Token: 0x060072D1 RID: 29393 RVA: 0x002E096C File Offset: 0x002DF96C
		internal void ClearInternal(bool fireChangeNotification)
		{
			if (base.Count > 0)
			{
				foreach (DateTime item in this)
				{
					this._removedItems.Add(item);
				}
				base.ClearItems();
				this.ClearMinMax();
				if (fireChangeNotification)
				{
					if (this._owner.SelectedDate != null)
					{
						this._owner.SelectedDate = null;
					}
					if (this._removedItems.Count > 0)
					{
						Collection<DateTime> addedItems = new Collection<DateTime>();
						this.RaiseSelectionChanged(this._removedItems, addedItems);
						this._removedItems.Clear();
					}
					this._owner.UpdateCellItems();
				}
			}
		}

		// Token: 0x060072D2 RID: 29394 RVA: 0x002E0A34 File Offset: 0x002DFA34
		internal void Toggle(DateTime date)
		{
			if (Calendar.IsValidDateSelection(this._owner, date))
			{
				CalendarSelectionMode selectionMode = this._owner.SelectionMode;
				if (selectionMode != CalendarSelectionMode.SingleDate)
				{
					if (selectionMode != CalendarSelectionMode.MultipleRange)
					{
						return;
					}
					if (!base.Remove(date))
					{
						base.Add(date);
					}
				}
				else
				{
					if (this._owner.SelectedDate == null || DateTimeHelper.CompareDays(this._owner.SelectedDate.Value, date) != 0)
					{
						this._owner.SelectedDate = new DateTime?(date);
						return;
					}
					this._owner.SelectedDate = null;
					return;
				}
			}
		}

		// Token: 0x060072D3 RID: 29395 RVA: 0x002E0ACF File Offset: 0x002DFACF
		private void RaiseSelectionChanged(IList removedItems, IList addedItems)
		{
			this._owner.OnSelectedDatesCollectionChanged(new CalendarSelectionChangedEventArgs(Calendar.SelectedDatesChangedEvent, removedItems, addedItems));
		}

		// Token: 0x060072D4 RID: 29396 RVA: 0x002E0AE8 File Offset: 0x002DFAE8
		private void BeginAddRange()
		{
			this._isAddingRange = true;
		}

		// Token: 0x060072D5 RID: 29397 RVA: 0x002E0AF1 File Offset: 0x002DFAF1
		private void EndAddRange()
		{
			this._isAddingRange = false;
			this.RaiseSelectionChanged(this._removedItems, this._addedItems);
			this._removedItems.Clear();
			this._addedItems.Clear();
			this._owner.UpdateCellItems();
		}

		// Token: 0x060072D6 RID: 29398 RVA: 0x002E0B30 File Offset: 0x002DFB30
		private bool CheckSelectionMode()
		{
			if (this._owner.SelectionMode == CalendarSelectionMode.None)
			{
				throw new InvalidOperationException(SR.Get("Calendar_OnSelectedDateChanged_InvalidOperation"));
			}
			if (this._owner.SelectionMode == CalendarSelectionMode.SingleDate && base.Count > 0)
			{
				throw new InvalidOperationException(SR.Get("Calendar_CheckSelectionMode_InvalidOperation"));
			}
			if (this._owner.SelectionMode == CalendarSelectionMode.SingleRange && !this._isAddingRange && base.Count > 0)
			{
				this.ClearInternal();
				return true;
			}
			return false;
		}

		// Token: 0x060072D7 RID: 29399 RVA: 0x002E0BA9 File Offset: 0x002DFBA9
		private bool IsValidThread()
		{
			return Thread.CurrentThread == this._dispatcherThread;
		}

		// Token: 0x060072D8 RID: 29400 RVA: 0x002E0BB8 File Offset: 0x002DFBB8
		private void UpdateMinMax(DateTime date)
		{
			if (this._maximumDate == null || date > this._maximumDate.Value)
			{
				this._maximumDate = new DateTime?(date);
			}
			if (this._minimumDate == null || date < this._minimumDate.Value)
			{
				this._minimumDate = new DateTime?(date);
			}
		}

		// Token: 0x060072D9 RID: 29401 RVA: 0x002E0C1D File Offset: 0x002DFC1D
		private void ClearMinMax()
		{
			this._maximumDate = null;
			this._minimumDate = null;
		}

		// Token: 0x060072DA RID: 29402 RVA: 0x002E0C37 File Offset: 0x002DFC37
		private static IEnumerable<DateTime> GetDaysInRange(DateTime start, DateTime end)
		{
			int increment = SelectedDatesCollection.GetDirection(start, end);
			DateTime? rangeStart = new DateTime?(start);
			do
			{
				yield return rangeStart.Value;
				rangeStart = DateTimeHelper.AddDays(rangeStart.Value, increment);
			}
			while (rangeStart != null && DateTime.Compare(end, rangeStart.Value) != -increment);
			yield break;
		}

		// Token: 0x060072DB RID: 29403 RVA: 0x002E0C4E File Offset: 0x002DFC4E
		private static int GetDirection(DateTime start, DateTime end)
		{
			if (DateTime.Compare(end, start) < 0)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x04003799 RID: 14233
		private Collection<DateTime> _addedItems;

		// Token: 0x0400379A RID: 14234
		private Collection<DateTime> _removedItems;

		// Token: 0x0400379B RID: 14235
		private Thread _dispatcherThread;

		// Token: 0x0400379C RID: 14236
		private bool _isAddingRange;

		// Token: 0x0400379D RID: 14237
		private Calendar _owner;

		// Token: 0x0400379E RID: 14238
		private DateTime? _maximumDate;

		// Token: 0x0400379F RID: 14239
		private DateTime? _minimumDate;
	}
}
