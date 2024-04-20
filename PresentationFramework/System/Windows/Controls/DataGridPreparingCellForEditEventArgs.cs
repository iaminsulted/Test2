using System;

namespace System.Windows.Controls
{
	// Token: 0x0200075F RID: 1887
	public class DataGridPreparingCellForEditEventArgs : EventArgs
	{
		// Token: 0x0600668A RID: 26250 RVA: 0x002B20E5 File Offset: 0x002B10E5
		public DataGridPreparingCellForEditEventArgs(DataGridColumn column, DataGridRow row, RoutedEventArgs editingEventArgs, FrameworkElement editingElement)
		{
			this._dataGridColumn = column;
			this._dataGridRow = row;
			this._editingEventArgs = editingEventArgs;
			this._editingElement = editingElement;
		}

		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x0600668B RID: 26251 RVA: 0x002B210A File Offset: 0x002B110A
		public DataGridColumn Column
		{
			get
			{
				return this._dataGridColumn;
			}
		}

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x0600668C RID: 26252 RVA: 0x002B2112 File Offset: 0x002B1112
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x0600668D RID: 26253 RVA: 0x002B211A File Offset: 0x002B111A
		public RoutedEventArgs EditingEventArgs
		{
			get
			{
				return this._editingEventArgs;
			}
		}

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x0600668E RID: 26254 RVA: 0x002B2122 File Offset: 0x002B1122
		public FrameworkElement EditingElement
		{
			get
			{
				return this._editingElement;
			}
		}

		// Token: 0x040033E0 RID: 13280
		private DataGridColumn _dataGridColumn;

		// Token: 0x040033E1 RID: 13281
		private DataGridRow _dataGridRow;

		// Token: 0x040033E2 RID: 13282
		private RoutedEventArgs _editingEventArgs;

		// Token: 0x040033E3 RID: 13283
		private FrameworkElement _editingElement;
	}
}
