using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace System.Windows.Controls
{
	// Token: 0x02000722 RID: 1826
	public sealed class CalendarBlackoutDatesCollection : ObservableCollection<CalendarDateRange>
	{
		// Token: 0x0600605E RID: 24670 RVA: 0x002995F6 File Offset: 0x002985F6
		public CalendarBlackoutDatesCollection(Calendar owner)
		{
			this._owner = owner;
			this._dispatcherThread = Thread.CurrentThread;
		}

		// Token: 0x0600605F RID: 24671 RVA: 0x00299610 File Offset: 0x00298610
		public void AddDatesInPast()
		{
			base.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1.0)));
		}

		// Token: 0x06006060 RID: 24672 RVA: 0x00299643 File Offset: 0x00298643
		public bool Contains(DateTime date)
		{
			return this.GetContainingDateRange(date) != null;
		}

		// Token: 0x06006061 RID: 24673 RVA: 0x00299650 File Offset: 0x00298650
		public bool Contains(DateTime start, DateTime end)
		{
			int count = base.Count;
			DateTime value;
			DateTime value2;
			if (DateTime.Compare(end, start) > -1)
			{
				value = DateTimeHelper.DiscardTime(new DateTime?(start)).Value;
				value2 = DateTimeHelper.DiscardTime(new DateTime?(end)).Value;
			}
			else
			{
				value = DateTimeHelper.DiscardTime(new DateTime?(end)).Value;
				value2 = DateTimeHelper.DiscardTime(new DateTime?(start)).Value;
			}
			for (int i = 0; i < count; i++)
			{
				if (DateTime.Compare(base[i].Start, value) == 0 && DateTime.Compare(base[i].End, value2) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006062 RID: 24674 RVA: 0x00299700 File Offset: 0x00298700
		public bool ContainsAny(CalendarDateRange range)
		{
			using (IEnumerator<CalendarDateRange> enumerator = base.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ContainsAny(range))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006063 RID: 24675 RVA: 0x00299750 File Offset: 0x00298750
		internal DateTime? GetNonBlackoutDate(DateTime? requestedDate, int dayInterval)
		{
			DateTime? result = requestedDate;
			if (requestedDate == null)
			{
				return null;
			}
			CalendarDateRange containingDateRange;
			if ((containingDateRange = this.GetContainingDateRange(result.Value)) == null)
			{
				return requestedDate;
			}
			do
			{
				if (dayInterval > 0)
				{
					result = DateTimeHelper.AddDays(containingDateRange.End, dayInterval);
				}
				else
				{
					result = DateTimeHelper.AddDays(containingDateRange.Start, dayInterval);
				}
			}
			while (result != null && (containingDateRange = this.GetContainingDateRange(result.Value)) != null);
			return result;
		}

		// Token: 0x06006064 RID: 24676 RVA: 0x002997C4 File Offset: 0x002987C4
		protected override void ClearItems()
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			foreach (CalendarDateRange item in base.Items)
			{
				this.UnRegisterItem(item);
			}
			base.ClearItems();
			this._owner.UpdateCellItems();
		}

		// Token: 0x06006065 RID: 24677 RVA: 0x0029983C File Offset: 0x0029883C
		protected override void InsertItem(int index, CalendarDateRange item)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (this.IsValid(item))
			{
				this.RegisterItem(item);
				base.InsertItem(index, item);
				this._owner.UpdateCellItems();
				return;
			}
			throw new ArgumentOutOfRangeException(SR.Get("Calendar_UnSelectableDates"));
		}

		// Token: 0x06006066 RID: 24678 RVA: 0x00299894 File Offset: 0x00298894
		protected override void RemoveItem(int index)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (index >= 0 && index < base.Count)
			{
				this.UnRegisterItem(base.Items[index]);
			}
			base.RemoveItem(index);
			this._owner.UpdateCellItems();
		}

		// Token: 0x06006067 RID: 24679 RVA: 0x002998EC File Offset: 0x002988EC
		protected override void SetItem(int index, CalendarDateRange item)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (this.IsValid(item))
			{
				CalendarDateRange item2 = null;
				if (index >= 0 && index < base.Count)
				{
					item2 = base.Items[index];
				}
				base.SetItem(index, item);
				this.UnRegisterItem(item2);
				this.RegisterItem(base.Items[index]);
				this._owner.UpdateCellItems();
				return;
			}
			throw new ArgumentOutOfRangeException(SR.Get("Calendar_UnSelectableDates"));
		}

		// Token: 0x06006068 RID: 24680 RVA: 0x00299972 File Offset: 0x00298972
		private void RegisterItem(CalendarDateRange item)
		{
			if (item != null)
			{
				item.Changing += this.Item_Changing;
				item.PropertyChanged += this.Item_PropertyChanged;
			}
		}

		// Token: 0x06006069 RID: 24681 RVA: 0x0029999B File Offset: 0x0029899B
		private void UnRegisterItem(CalendarDateRange item)
		{
			if (item != null)
			{
				item.Changing -= this.Item_Changing;
				item.PropertyChanged -= this.Item_PropertyChanged;
			}
		}

		// Token: 0x0600606A RID: 24682 RVA: 0x002999C4 File Offset: 0x002989C4
		private void Item_Changing(object sender, CalendarDateRangeChangingEventArgs e)
		{
			if (sender is CalendarDateRange && !this.IsValid(e.Start, e.End))
			{
				throw new ArgumentOutOfRangeException(SR.Get("Calendar_UnSelectableDates"));
			}
		}

		// Token: 0x0600606B RID: 24683 RVA: 0x002999F2 File Offset: 0x002989F2
		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sender is CalendarDateRange)
			{
				this._owner.UpdateCellItems();
			}
		}

		// Token: 0x0600606C RID: 24684 RVA: 0x00299A07 File Offset: 0x00298A07
		private bool IsValid(CalendarDateRange item)
		{
			return this.IsValid(item.Start, item.End);
		}

		// Token: 0x0600606D RID: 24685 RVA: 0x00299A1C File Offset: 0x00298A1C
		private bool IsValid(DateTime start, DateTime end)
		{
			using (IEnumerator<DateTime> enumerator = this._owner.SelectedDates.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (DateTimeHelper.InRange((enumerator.Current as DateTime?).Value, start, end))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600606E RID: 24686 RVA: 0x00299A90 File Offset: 0x00298A90
		private bool IsValidThread()
		{
			return Thread.CurrentThread == this._dispatcherThread;
		}

		// Token: 0x0600606F RID: 24687 RVA: 0x00299AA0 File Offset: 0x00298AA0
		private CalendarDateRange GetContainingDateRange(DateTime date)
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (DateTimeHelper.InRange(date, base[i]))
				{
					return base[i];
				}
			}
			return null;
		}

		// Token: 0x0400321D RID: 12829
		private Thread _dispatcherThread;

		// Token: 0x0400321E RID: 12830
		private Calendar _owner;
	}
}
