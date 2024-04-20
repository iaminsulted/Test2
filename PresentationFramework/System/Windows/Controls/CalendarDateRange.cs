using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	// Token: 0x02000724 RID: 1828
	public sealed class CalendarDateRange : INotifyPropertyChanged
	{
		// Token: 0x06006075 RID: 24693 RVA: 0x00299B0E File Offset: 0x00298B0E
		public CalendarDateRange() : this(DateTime.MinValue, DateTime.MaxValue)
		{
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x00299B20 File Offset: 0x00298B20
		public CalendarDateRange(DateTime day) : this(day, day)
		{
		}

		// Token: 0x06006077 RID: 24695 RVA: 0x00299B2A File Offset: 0x00298B2A
		public CalendarDateRange(DateTime start, DateTime end)
		{
			this._start = start;
			this._end = end;
		}

		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06006078 RID: 24696 RVA: 0x00299B40 File Offset: 0x00298B40
		// (remove) Token: 0x06006079 RID: 24697 RVA: 0x00299B78 File Offset: 0x00298B78
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x0600607A RID: 24698 RVA: 0x00299BAD File Offset: 0x00298BAD
		// (set) Token: 0x0600607B RID: 24699 RVA: 0x00299BC0 File Offset: 0x00298BC0
		public DateTime End
		{
			get
			{
				return CalendarDateRange.CoerceEnd(this._start, this._end);
			}
			set
			{
				DateTime dateTime = CalendarDateRange.CoerceEnd(this._start, value);
				if (dateTime != this.End)
				{
					this.OnChanging(new CalendarDateRangeChangingEventArgs(this._start, dateTime));
					this._end = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("End"));
				}
			}
		}

		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x0600607C RID: 24700 RVA: 0x00299C11 File Offset: 0x00298C11
		// (set) Token: 0x0600607D RID: 24701 RVA: 0x00299C1C File Offset: 0x00298C1C
		public DateTime Start
		{
			get
			{
				return this._start;
			}
			set
			{
				if (this._start != value)
				{
					DateTime end = this.End;
					DateTime dateTime = CalendarDateRange.CoerceEnd(value, this._end);
					this.OnChanging(new CalendarDateRangeChangingEventArgs(value, dateTime));
					this._start = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Start"));
					if (dateTime != end)
					{
						this.OnPropertyChanged(new PropertyChangedEventArgs("End"));
					}
				}
			}
		}

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x0600607E RID: 24702 RVA: 0x00299C88 File Offset: 0x00298C88
		// (remove) Token: 0x0600607F RID: 24703 RVA: 0x00299CC0 File Offset: 0x00298CC0
		internal event EventHandler<CalendarDateRangeChangingEventArgs> Changing;

		// Token: 0x06006080 RID: 24704 RVA: 0x00299CF5 File Offset: 0x00298CF5
		internal bool ContainsAny(CalendarDateRange range)
		{
			return range.End >= this.Start && this.End >= range.Start;
		}

		// Token: 0x06006081 RID: 24705 RVA: 0x00299D20 File Offset: 0x00298D20
		private void OnChanging(CalendarDateRangeChangingEventArgs e)
		{
			EventHandler<CalendarDateRangeChangingEventArgs> changing = this.Changing;
			if (changing != null)
			{
				changing(this, e);
			}
		}

		// Token: 0x06006082 RID: 24706 RVA: 0x00299D40 File Offset: 0x00298D40
		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, e);
			}
		}

		// Token: 0x06006083 RID: 24707 RVA: 0x00299D5F File Offset: 0x00298D5F
		private static DateTime CoerceEnd(DateTime start, DateTime end)
		{
			if (DateTime.Compare(start, end) > 0)
			{
				return start;
			}
			return end;
		}

		// Token: 0x04003221 RID: 12833
		private DateTime _end;

		// Token: 0x04003222 RID: 12834
		private DateTime _start;
	}
}
