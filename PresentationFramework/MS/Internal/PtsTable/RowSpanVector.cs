using System;
using System.Windows.Documents;

namespace MS.Internal.PtsTable
{
	// Token: 0x02000109 RID: 265
	internal sealed class RowSpanVector
	{
		// Token: 0x0600068D RID: 1677 RVA: 0x00108DB8 File Offset: 0x00107DB8
		internal RowSpanVector()
		{
			this._entries = new RowSpanVector.Entry[8];
			this._entries[0].Cell = null;
			this._entries[0].Start = 1073741823;
			this._entries[0].Range = 1073741823;
			this._entries[0].Ttl = int.MaxValue;
			this._size = 1;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00108E34 File Offset: 0x00107E34
		internal void Register(TableCell cell)
		{
			int columnIndex = cell.ColumnIndex;
			if (this._size == this._entries.Length)
			{
				this.InflateCapacity();
			}
			for (int i = this._size - 1; i >= this._index; i--)
			{
				this._entries[i + 1] = this._entries[i];
			}
			this._entries[this._index].Cell = cell;
			this._entries[this._index].Start = columnIndex;
			this._entries[this._index].Range = cell.ColumnSpan;
			this._entries[this._index].Ttl = cell.RowSpan - 1;
			this._size++;
			this._index++;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00108F14 File Offset: 0x00107F14
		internal void GetFirstAvailableRange(out int firstAvailableIndex, out int firstOccupiedIndex)
		{
			this._index = 0;
			firstAvailableIndex = 0;
			firstOccupiedIndex = this._entries[this._index].Start;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00108F38 File Offset: 0x00107F38
		internal void GetNextAvailableRange(out int firstAvailableIndex, out int firstOccupiedIndex)
		{
			firstAvailableIndex = this._entries[this._index].Start + this._entries[this._index].Range;
			RowSpanVector.Entry[] entries = this._entries;
			int index = this._index;
			entries[index].Ttl = entries[index].Ttl - 1;
			this._index++;
			firstOccupiedIndex = this._entries[this._index].Start;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00108FB8 File Offset: 0x00107FB8
		internal void GetSpanCells(out TableCell[] cells, out bool isLastRowOfAnySpan)
		{
			cells = RowSpanVector.s_noCells;
			isLastRowOfAnySpan = false;
			while (this._index < this._size)
			{
				RowSpanVector.Entry[] entries = this._entries;
				int index = this._index;
				entries[index].Ttl = entries[index].Ttl - 1;
				this._index++;
			}
			if (this._size > 1)
			{
				cells = new TableCell[this._size - 1];
				int num = 0;
				int num2 = 0;
				do
				{
					cells[num] = this._entries[num].Cell;
					if (this._entries[num].Ttl > 0)
					{
						if (num != num2)
						{
							this._entries[num2] = this._entries[num];
						}
						num2++;
					}
					num++;
				}
				while (num < this._size - 1);
				if (num != num2)
				{
					this._entries[num2] = this._entries[num];
					isLastRowOfAnySpan = true;
				}
				this._size = num2 + 1;
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x001090A7 File Offset: 0x001080A7
		internal bool Empty()
		{
			return this._size == 1;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x001090B4 File Offset: 0x001080B4
		private void InflateCapacity()
		{
			RowSpanVector.Entry[] array = new RowSpanVector.Entry[this._entries.Length * 2];
			Array.Copy(this._entries, array, this._entries.Length);
			this._entries = array;
		}

		// Token: 0x0400070A RID: 1802
		private RowSpanVector.Entry[] _entries;

		// Token: 0x0400070B RID: 1803
		private int _size;

		// Token: 0x0400070C RID: 1804
		private int _index;

		// Token: 0x0400070D RID: 1805
		private const int c_defaultCapacity = 8;

		// Token: 0x0400070E RID: 1806
		private static TableCell[] s_noCells = Array.Empty<TableCell>();

		// Token: 0x020008BF RID: 2239
		private struct Entry
		{
			// Token: 0x04003C2A RID: 15402
			internal TableCell Cell;

			// Token: 0x04003C2B RID: 15403
			internal int Start;

			// Token: 0x04003C2C RID: 15404
			internal int Range;

			// Token: 0x04003C2D RID: 15405
			internal int Ttl;
		}
	}
}
