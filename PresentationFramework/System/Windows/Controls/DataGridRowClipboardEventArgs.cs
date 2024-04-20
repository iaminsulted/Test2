using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Controls
{
	// Token: 0x02000761 RID: 1889
	public class DataGridRowClipboardEventArgs : EventArgs
	{
		// Token: 0x060066E8 RID: 26344 RVA: 0x002B35B6 File Offset: 0x002B25B6
		public DataGridRowClipboardEventArgs(object item, int startColumnDisplayIndex, int endColumnDisplayIndex, bool isColumnHeadersRow)
		{
			this._item = item;
			this._startColumnDisplayIndex = startColumnDisplayIndex;
			this._endColumnDisplayIndex = endColumnDisplayIndex;
			this._isColumnHeadersRow = isColumnHeadersRow;
		}

		// Token: 0x060066E9 RID: 26345 RVA: 0x002B35E2 File Offset: 0x002B25E2
		internal DataGridRowClipboardEventArgs(object item, int startColumnDisplayIndex, int endColumnDisplayIndex, bool isColumnHeadersRow, int rowIndexHint) : this(item, startColumnDisplayIndex, endColumnDisplayIndex, isColumnHeadersRow)
		{
			this._rowIndexHint = rowIndexHint;
		}

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x060066EA RID: 26346 RVA: 0x002B35F7 File Offset: 0x002B25F7
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x060066EB RID: 26347 RVA: 0x002B35FF File Offset: 0x002B25FF
		public List<DataGridClipboardCellContent> ClipboardRowContent
		{
			get
			{
				if (this._clipboardRowContent == null)
				{
					this._clipboardRowContent = new List<DataGridClipboardCellContent>();
				}
				return this._clipboardRowContent;
			}
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x002B361C File Offset: 0x002B261C
		public string FormatClipboardCellValues(string format)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.ClipboardRowContent.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridClipboardHelper.FormatCell(this.ClipboardRowContent[i].Content, i == 0, i == count - 1, stringBuilder, format);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x060066ED RID: 26349 RVA: 0x002B3672 File Offset: 0x002B2672
		public int StartColumnDisplayIndex
		{
			get
			{
				return this._startColumnDisplayIndex;
			}
		}

		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x060066EE RID: 26350 RVA: 0x002B367A File Offset: 0x002B267A
		public int EndColumnDisplayIndex
		{
			get
			{
				return this._endColumnDisplayIndex;
			}
		}

		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x060066EF RID: 26351 RVA: 0x002B3682 File Offset: 0x002B2682
		public bool IsColumnHeadersRow
		{
			get
			{
				return this._isColumnHeadersRow;
			}
		}

		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x060066F0 RID: 26352 RVA: 0x002B368A File Offset: 0x002B268A
		internal int RowIndexHint
		{
			get
			{
				return this._rowIndexHint;
			}
		}

		// Token: 0x0400340B RID: 13323
		private int _startColumnDisplayIndex;

		// Token: 0x0400340C RID: 13324
		private int _endColumnDisplayIndex;

		// Token: 0x0400340D RID: 13325
		private object _item;

		// Token: 0x0400340E RID: 13326
		private bool _isColumnHeadersRow;

		// Token: 0x0400340F RID: 13327
		private List<DataGridClipboardCellContent> _clipboardRowContent;

		// Token: 0x04003410 RID: 13328
		private int _rowIndexHint = -1;
	}
}
