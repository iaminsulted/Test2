using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x0200067E RID: 1662
	internal class ColumnStateArray : ArrayList
	{
		// Token: 0x0600520F RID: 21007 RVA: 0x0024BDD2 File Offset: 0x0024ADD2
		internal ColumnStateArray() : base(20)
		{
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x00251ABB File Offset: 0x00250ABB
		internal ColumnState EntryAt(int i)
		{
			return (ColumnState)this[i];
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x00251ACC File Offset: 0x00250ACC
		internal int GetMinUnfilledRowIndex()
		{
			int num = -1;
			for (int i = 0; i < this.Count; i++)
			{
				ColumnState columnState = this.EntryAt(i);
				if (!columnState.IsFilled && (num < 0 || num > columnState.Row.Index) && !columnState.Row.FormatState.RowFormat.IsVMerge)
				{
					num = columnState.Row.Index;
				}
			}
			return num;
		}
	}
}
