using System;

namespace System.Windows.Controls
{
	// Token: 0x02000743 RID: 1859
	public class DataGridCellClipboardEventArgs : EventArgs
	{
		// Token: 0x0600647F RID: 25727 RVA: 0x002A8CDA File Offset: 0x002A7CDA
		public DataGridCellClipboardEventArgs(object item, DataGridColumn column, object content)
		{
			this._item = item;
			this._column = column;
			this._content = content;
		}

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x06006480 RID: 25728 RVA: 0x002A8CF7 File Offset: 0x002A7CF7
		// (set) Token: 0x06006481 RID: 25729 RVA: 0x002A8CFF File Offset: 0x002A7CFF
		public object Content
		{
			get
			{
				return this._content;
			}
			set
			{
				this._content = value;
			}
		}

		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x06006482 RID: 25730 RVA: 0x002A8D08 File Offset: 0x002A7D08
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x06006483 RID: 25731 RVA: 0x002A8D10 File Offset: 0x002A7D10
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x04003349 RID: 13129
		private object _content;

		// Token: 0x0400334A RID: 13130
		private object _item;

		// Token: 0x0400334B RID: 13131
		private DataGridColumn _column;
	}
}
