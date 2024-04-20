using System;

namespace System.Windows.Controls
{
	// Token: 0x02000765 RID: 1893
	public class DataGridRowEventArgs : EventArgs
	{
		// Token: 0x060066FB RID: 26363 RVA: 0x002B3701 File Offset: 0x002B2701
		public DataGridRowEventArgs(DataGridRow row)
		{
			this.Row = row;
		}

		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x060066FC RID: 26364 RVA: 0x002B3710 File Offset: 0x002B2710
		// (set) Token: 0x060066FD RID: 26365 RVA: 0x002B3718 File Offset: 0x002B2718
		public DataGridRow Row { get; private set; }
	}
}
