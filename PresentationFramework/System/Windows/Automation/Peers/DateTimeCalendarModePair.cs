using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000547 RID: 1351
	internal struct DateTimeCalendarModePair
	{
		// Token: 0x060042C3 RID: 17091 RVA: 0x0021C85D File Offset: 0x0021B85D
		internal DateTimeCalendarModePair(DateTime date, CalendarMode mode)
		{
			this.ButtonMode = mode;
			this.Date = date;
		}

		// Token: 0x04002513 RID: 9491
		private CalendarMode ButtonMode;

		// Token: 0x04002514 RID: 9492
		private DateTime Date;
	}
}
