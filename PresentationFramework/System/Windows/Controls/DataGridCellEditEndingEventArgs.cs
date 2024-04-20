using System;

namespace System.Windows.Controls
{
	// Token: 0x02000744 RID: 1860
	public class DataGridCellEditEndingEventArgs : EventArgs
	{
		// Token: 0x06006484 RID: 25732 RVA: 0x002A8D18 File Offset: 0x002A7D18
		public DataGridCellEditEndingEventArgs(DataGridColumn column, DataGridRow row, FrameworkElement editingElement, DataGridEditAction editAction)
		{
			this._dataGridColumn = column;
			this._dataGridRow = row;
			this._editingElement = editingElement;
			this._editAction = editAction;
		}

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x06006485 RID: 25733 RVA: 0x002A8D3D File Offset: 0x002A7D3D
		// (set) Token: 0x06006486 RID: 25734 RVA: 0x002A8D45 File Offset: 0x002A7D45
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

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x06006487 RID: 25735 RVA: 0x002A8D4E File Offset: 0x002A7D4E
		public DataGridColumn Column
		{
			get
			{
				return this._dataGridColumn;
			}
		}

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x06006488 RID: 25736 RVA: 0x002A8D56 File Offset: 0x002A7D56
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x06006489 RID: 25737 RVA: 0x002A8D5E File Offset: 0x002A7D5E
		public FrameworkElement EditingElement
		{
			get
			{
				return this._editingElement;
			}
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x0600648A RID: 25738 RVA: 0x002A8D66 File Offset: 0x002A7D66
		public DataGridEditAction EditAction
		{
			get
			{
				return this._editAction;
			}
		}

		// Token: 0x0400334C RID: 13132
		private bool _cancel;

		// Token: 0x0400334D RID: 13133
		private DataGridColumn _dataGridColumn;

		// Token: 0x0400334E RID: 13134
		private DataGridRow _dataGridRow;

		// Token: 0x0400334F RID: 13135
		private FrameworkElement _editingElement;

		// Token: 0x04003350 RID: 13136
		private DataGridEditAction _editAction;
	}
}
