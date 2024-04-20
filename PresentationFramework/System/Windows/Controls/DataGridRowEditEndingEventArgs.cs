using System;

namespace System.Windows.Controls
{
	// Token: 0x02000764 RID: 1892
	public class DataGridRowEditEndingEventArgs : EventArgs
	{
		// Token: 0x060066F6 RID: 26358 RVA: 0x002B36CA File Offset: 0x002B26CA
		public DataGridRowEditEndingEventArgs(DataGridRow row, DataGridEditAction editAction)
		{
			this._dataGridRow = row;
			this._editAction = editAction;
		}

		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x060066F7 RID: 26359 RVA: 0x002B36E0 File Offset: 0x002B26E0
		// (set) Token: 0x060066F8 RID: 26360 RVA: 0x002B36E8 File Offset: 0x002B26E8
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x060066F9 RID: 26361 RVA: 0x002B36F1 File Offset: 0x002B26F1
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x060066FA RID: 26362 RVA: 0x002B36F9 File Offset: 0x002B26F9
		public DataGridEditAction EditAction
		{
			get
			{
				return this._editAction;
			}
		}

		// Token: 0x04003417 RID: 13335
		private bool _cancel;

		// Token: 0x04003418 RID: 13336
		private DataGridRow _dataGridRow;

		// Token: 0x04003419 RID: 13337
		private DataGridEditAction _editAction;
	}
}
