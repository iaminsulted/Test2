using System;

namespace System.Windows.Controls
{
	// Token: 0x02000740 RID: 1856
	public class DataGridBeginningEditEventArgs : EventArgs
	{
		// Token: 0x0600642C RID: 25644 RVA: 0x002A7AF1 File Offset: 0x002A6AF1
		public DataGridBeginningEditEventArgs(DataGridColumn column, DataGridRow row, RoutedEventArgs editingEventArgs)
		{
			this._dataGridColumn = column;
			this._dataGridRow = row;
			this._editingEventArgs = editingEventArgs;
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x0600642D RID: 25645 RVA: 0x002A7B0E File Offset: 0x002A6B0E
		// (set) Token: 0x0600642E RID: 25646 RVA: 0x002A7B16 File Offset: 0x002A6B16
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

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x0600642F RID: 25647 RVA: 0x002A7B1F File Offset: 0x002A6B1F
		public DataGridColumn Column
		{
			get
			{
				return this._dataGridColumn;
			}
		}

		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x06006430 RID: 25648 RVA: 0x002A7B27 File Offset: 0x002A6B27
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x06006431 RID: 25649 RVA: 0x002A7B2F File Offset: 0x002A6B2F
		public RoutedEventArgs EditingEventArgs
		{
			get
			{
				return this._editingEventArgs;
			}
		}

		// Token: 0x04003337 RID: 13111
		private bool _cancel;

		// Token: 0x04003338 RID: 13112
		private DataGridColumn _dataGridColumn;

		// Token: 0x04003339 RID: 13113
		private DataGridRow _dataGridRow;

		// Token: 0x0400333A RID: 13114
		private RoutedEventArgs _editingEventArgs;
	}
}
