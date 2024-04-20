using System;

namespace System.Windows.Controls
{
	// Token: 0x02000725 RID: 1829
	internal class CalendarDateRangeChangingEventArgs : EventArgs
	{
		// Token: 0x06006084 RID: 24708 RVA: 0x00299D6E File Offset: 0x00298D6E
		public CalendarDateRangeChangingEventArgs(DateTime start, DateTime end)
		{
			this._start = start;
			this._end = end;
		}

		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x06006085 RID: 24709 RVA: 0x00299D84 File Offset: 0x00298D84
		public DateTime Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x06006086 RID: 24710 RVA: 0x00299D8C File Offset: 0x00298D8C
		public DateTime End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x04003225 RID: 12837
		private DateTime _start;

		// Token: 0x04003226 RID: 12838
		private DateTime _end;
	}
}
