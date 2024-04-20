using System;

namespace System.Windows.Controls
{
	// Token: 0x0200074E RID: 1870
	public class DataGridColumnEventArgs : EventArgs
	{
		// Token: 0x060065D9 RID: 26073 RVA: 0x002AFEB6 File Offset: 0x002AEEB6
		public DataGridColumnEventArgs(DataGridColumn column)
		{
			this._column = column;
		}

		// Token: 0x17001785 RID: 6021
		// (get) Token: 0x060065DA RID: 26074 RVA: 0x002AFEC5 File Offset: 0x002AEEC5
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x04003398 RID: 13208
		private DataGridColumn _column;
	}
}
