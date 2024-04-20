using System;

namespace System.Windows.Controls
{
	// Token: 0x02000748 RID: 1864
	public struct DataGridClipboardCellContent
	{
		// Token: 0x060064F3 RID: 25843 RVA: 0x002AB66B File Offset: 0x002AA66B
		public DataGridClipboardCellContent(object item, DataGridColumn column, object content)
		{
			this._item = item;
			this._column = column;
			this._content = content;
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x060064F4 RID: 25844 RVA: 0x002AB682 File Offset: 0x002AA682
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x060064F5 RID: 25845 RVA: 0x002AB68A File Offset: 0x002AA68A
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x060064F6 RID: 25846 RVA: 0x002AB692 File Offset: 0x002AA692
		public object Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x002AB69C File Offset: 0x002AA69C
		public override bool Equals(object data)
		{
			if (data is DataGridClipboardCellContent)
			{
				DataGridClipboardCellContent dataGridClipboardCellContent = (DataGridClipboardCellContent)data;
				return this._column == dataGridClipboardCellContent._column && this._content == dataGridClipboardCellContent._content && this._item == dataGridClipboardCellContent._item;
			}
			return false;
		}

		// Token: 0x060064F8 RID: 25848 RVA: 0x002AB6E8 File Offset: 0x002AA6E8
		public override int GetHashCode()
		{
			return ((this._column == null) ? 0 : this._column.GetHashCode()) ^ ((this._content == null) ? 0 : this._content.GetHashCode()) ^ ((this._item == null) ? 0 : this._item.GetHashCode());
		}

		// Token: 0x060064F9 RID: 25849 RVA: 0x002AB739 File Offset: 0x002AA739
		public static bool operator ==(DataGridClipboardCellContent clipboardCellContent1, DataGridClipboardCellContent clipboardCellContent2)
		{
			return clipboardCellContent1._column == clipboardCellContent2._column && clipboardCellContent1._content == clipboardCellContent2._content && clipboardCellContent1._item == clipboardCellContent2._item;
		}

		// Token: 0x060064FA RID: 25850 RVA: 0x002AB767 File Offset: 0x002AA767
		public static bool operator !=(DataGridClipboardCellContent clipboardCellContent1, DataGridClipboardCellContent clipboardCellContent2)
		{
			return clipboardCellContent1._column != clipboardCellContent2._column || clipboardCellContent1._content != clipboardCellContent2._content || clipboardCellContent1._item != clipboardCellContent2._item;
		}

		// Token: 0x0400335D RID: 13149
		private object _item;

		// Token: 0x0400335E RID: 13150
		private DataGridColumn _column;

		// Token: 0x0400335F RID: 13151
		private object _content;
	}
}
