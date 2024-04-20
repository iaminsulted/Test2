using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200010F RID: 271
	internal class CellInfo
	{
		// Token: 0x060006E6 RID: 1766 RVA: 0x0010A45C File Offset: 0x0010945C
		internal CellInfo(TableParaClient tpc, CellParaClient cpc)
		{
			this._rectTable = new Rect(TextDpi.FromTextDpi(tpc.Rect.u), TextDpi.FromTextDpi(tpc.Rect.v), TextDpi.FromTextDpi(tpc.Rect.du), TextDpi.FromTextDpi(tpc.Rect.dv));
			this._rectCell = new Rect(TextDpi.FromTextDpi(cpc.Rect.u), TextDpi.FromTextDpi(cpc.Rect.v), TextDpi.FromTextDpi(cpc.Rect.du), TextDpi.FromTextDpi(cpc.Rect.dv));
			this._autofitWidth = tpc.AutofitWidth;
			this._columnWidths = new double[tpc.CalculatedColumns.Length];
			for (int i = 0; i < tpc.CalculatedColumns.Length; i++)
			{
				this._columnWidths[i] = tpc.CalculatedColumns[i].DurWidth;
			}
			this._cell = cpc.Cell;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0010A55C File Offset: 0x0010955C
		internal void Adjust(Point ptAdjust)
		{
			this._rectTable.X = this._rectTable.X + ptAdjust.X;
			this._rectTable.Y = this._rectTable.Y + ptAdjust.Y;
			this._rectCell.X = this._rectCell.X + ptAdjust.X;
			this._rectCell.Y = this._rectCell.Y + ptAdjust.Y;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0010A5CD File Offset: 0x001095CD
		internal TableCell Cell
		{
			get
			{
				return this._cell;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0010A5D5 File Offset: 0x001095D5
		internal double[] TableColumnWidths
		{
			get
			{
				return this._columnWidths;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0010A5DD File Offset: 0x001095DD
		internal double TableAutofitWidth
		{
			get
			{
				return this._autofitWidth;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0010A5E5 File Offset: 0x001095E5
		internal Rect TableArea
		{
			get
			{
				return this._rectTable;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0010A5ED File Offset: 0x001095ED
		internal Rect CellArea
		{
			get
			{
				return this._rectCell;
			}
		}

		// Token: 0x0400072F RID: 1839
		private Rect _rectCell;

		// Token: 0x04000730 RID: 1840
		private Rect _rectTable;

		// Token: 0x04000731 RID: 1841
		private TableCell _cell;

		// Token: 0x04000732 RID: 1842
		private double[] _columnWidths;

		// Token: 0x04000733 RID: 1843
		private double _autofitWidth;
	}
}
