using System;

namespace System.Windows.Controls
{
	// Token: 0x02000723 RID: 1827
	public class CalendarDateChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x06006070 RID: 24688 RVA: 0x00299AD6 File Offset: 0x00298AD6
		internal CalendarDateChangedEventArgs(DateTime? removedDate, DateTime? addedDate)
		{
			this.RemovedDate = removedDate;
			this.AddedDate = addedDate;
		}

		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x06006071 RID: 24689 RVA: 0x00299AEC File Offset: 0x00298AEC
		// (set) Token: 0x06006072 RID: 24690 RVA: 0x00299AF4 File Offset: 0x00298AF4
		public DateTime? AddedDate { get; private set; }

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x06006073 RID: 24691 RVA: 0x00299AFD File Offset: 0x00298AFD
		// (set) Token: 0x06006074 RID: 24692 RVA: 0x00299B05 File Offset: 0x00298B05
		public DateTime? RemovedDate { get; private set; }
	}
}
