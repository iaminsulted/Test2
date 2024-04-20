using System;

namespace System.Windows.Controls
{
	// Token: 0x02000728 RID: 1832
	public class CalendarModeChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x06006088 RID: 24712 RVA: 0x00299DAE File Offset: 0x00298DAE
		public CalendarModeChangedEventArgs(CalendarMode oldMode, CalendarMode newMode)
		{
			this.OldMode = oldMode;
			this.NewMode = newMode;
		}

		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x06006089 RID: 24713 RVA: 0x00299DC4 File Offset: 0x00298DC4
		// (set) Token: 0x0600608A RID: 24714 RVA: 0x00299DCC File Offset: 0x00298DCC
		public CalendarMode NewMode { get; private set; }

		// Token: 0x17001650 RID: 5712
		// (get) Token: 0x0600608B RID: 24715 RVA: 0x00299DD5 File Offset: 0x00298DD5
		// (set) Token: 0x0600608C RID: 24716 RVA: 0x00299DDD File Offset: 0x00298DDD
		public CalendarMode OldMode { get; private set; }
	}
}
