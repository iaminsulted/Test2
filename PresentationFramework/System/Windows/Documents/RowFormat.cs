using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x0200066F RID: 1647
	internal class RowFormat
	{
		// Token: 0x06005143 RID: 20803 RVA: 0x0024EDD4 File Offset: 0x0024DDD4
		internal RowFormat()
		{
			this._rowCellFormat = new CellFormat();
			this._widthA = new CellWidth();
			this._widthB = new CellWidth();
			this._widthRow = new CellWidth();
			this._cellFormats = new ArrayList();
			this._dir = DirState.DirLTR;
			this._nTrgaph = -1L;
			this._nTrleft = 0L;
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x0024EE38 File Offset: 0x0024DE38
		internal RowFormat(RowFormat ri)
		{
			this._rowCellFormat = new CellFormat(ri.RowCellFormat);
			this._cellFormats = new ArrayList();
			this._widthA = new CellWidth(ri.WidthA);
			this._widthB = new CellWidth(ri.WidthB);
			this._widthRow = new CellWidth(ri.WidthRow);
			this._nTrgaph = ri.Trgaph;
			this._dir = ri.Dir;
			this._nTrleft = ri._nTrleft;
			for (int i = 0; i < ri.CellCount; i++)
			{
				this._cellFormats.Add(new CellFormat(ri.NthCellFormat(i)));
			}
		}

		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x06005145 RID: 20805 RVA: 0x0024EEE7 File Offset: 0x0024DEE7
		internal CellFormat RowCellFormat
		{
			get
			{
				return this._rowCellFormat;
			}
		}

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x06005146 RID: 20806 RVA: 0x0024EEEF File Offset: 0x0024DEEF
		internal int CellCount
		{
			get
			{
				return this._cellFormats.Count;
			}
		}

		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x06005147 RID: 20807 RVA: 0x0024EEFC File Offset: 0x0024DEFC
		internal CellFormat TopCellFormat
		{
			get
			{
				if (this.CellCount <= 0)
				{
					return null;
				}
				return this.NthCellFormat(this.CellCount - 1);
			}
		}

		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x06005148 RID: 20808 RVA: 0x0024EF17 File Offset: 0x0024DF17
		internal CellWidth WidthA
		{
			get
			{
				return this._widthA;
			}
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x06005149 RID: 20809 RVA: 0x0024EF1F File Offset: 0x0024DF1F
		internal CellWidth WidthB
		{
			get
			{
				return this._widthB;
			}
		}

		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x0600514A RID: 20810 RVA: 0x0024EF27 File Offset: 0x0024DF27
		internal CellWidth WidthRow
		{
			get
			{
				return this._widthRow;
			}
		}

		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x0600514B RID: 20811 RVA: 0x0024EF2F File Offset: 0x0024DF2F
		// (set) Token: 0x0600514C RID: 20812 RVA: 0x0024EF37 File Offset: 0x0024DF37
		internal long Trgaph
		{
			get
			{
				return this._nTrgaph;
			}
			set
			{
				this._nTrgaph = value;
			}
		}

		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x0600514D RID: 20813 RVA: 0x0024EF40 File Offset: 0x0024DF40
		// (set) Token: 0x0600514E RID: 20814 RVA: 0x0024EF48 File Offset: 0x0024DF48
		internal long Trleft
		{
			get
			{
				return this._nTrleft;
			}
			set
			{
				this._nTrleft = value;
			}
		}

		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x0600514F RID: 20815 RVA: 0x0024EF51 File Offset: 0x0024DF51
		// (set) Token: 0x06005150 RID: 20816 RVA: 0x0024EF59 File Offset: 0x0024DF59
		internal DirState Dir
		{
			get
			{
				return this._dir;
			}
			set
			{
				this._dir = value;
			}
		}

		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x06005151 RID: 20817 RVA: 0x0024EF64 File Offset: 0x0024DF64
		internal bool IsVMerge
		{
			get
			{
				for (int i = 0; i < this.CellCount; i++)
				{
					if (this.NthCellFormat(i).IsVMerge)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x0024EF93 File Offset: 0x0024DF93
		internal CellFormat NthCellFormat(int n)
		{
			if (n < 0 || n >= this.CellCount)
			{
				return this.RowCellFormat;
			}
			return (CellFormat)this._cellFormats[n];
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x0024EFBA File Offset: 0x0024DFBA
		internal CellFormat NextCellFormat()
		{
			this._cellFormats.Add(new CellFormat(this.RowCellFormat));
			return this.TopCellFormat;
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x0024EFD9 File Offset: 0x0024DFD9
		internal CellFormat CurrentCellFormat()
		{
			if (this.CellCount == 0 || !this.TopCellFormat.IsPending)
			{
				return this.NextCellFormat();
			}
			return this.TopCellFormat;
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x0024F000 File Offset: 0x0024E000
		internal void CanonicalizeWidthsFromRTF()
		{
			if (this.CellCount == 0)
			{
				return;
			}
			CellFormat cellFormat = null;
			long num = this.Trleft;
			for (int i = 0; i < this.CellCount; i++)
			{
				CellFormat cellFormat2 = this.NthCellFormat(i);
				if (!cellFormat2.IsHMerge)
				{
					if (cellFormat2.IsHMergeFirst)
					{
						for (int j = i + 1; j < this.CellCount; j++)
						{
							CellFormat cellFormat3 = this.NthCellFormat(j);
							if (!cellFormat3.IsHMerge)
							{
								break;
							}
							cellFormat2.CellX = cellFormat3.CellX;
						}
					}
					if (cellFormat2.Width.Value == 0L && cellFormat2.IsCellXSet)
					{
						cellFormat2.Width.Type = WidthType.WidthTwips;
						cellFormat2.Width.Value = ((cellFormat == null) ? (cellFormat2.CellX - this.Trleft) : (cellFormat2.CellX - cellFormat.CellX));
					}
					else if (cellFormat2.Width.Value > 0L && !cellFormat2.IsCellXSet)
					{
						num += cellFormat2.Width.Value;
						cellFormat2.CellX = num;
					}
					cellFormat = cellFormat2;
				}
			}
			num = this.NthCellFormat(0).CellX;
			for (int k = 1; k < this.CellCount; k++)
			{
				CellFormat cellFormat4 = this.NthCellFormat(k);
				if (cellFormat4.CellX < num)
				{
					cellFormat4.CellX = num + 1L;
				}
				num = cellFormat4.CellX;
			}
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x0024F14C File Offset: 0x0024E14C
		internal void CanonicalizeWidthsFromXaml()
		{
			long num = this.Trleft;
			for (int i = 0; i < this.CellCount; i++)
			{
				CellFormat cellFormat = this.NthCellFormat(i);
				if (cellFormat.Width.Type == WidthType.WidthTwips)
				{
					num += cellFormat.Width.Value;
				}
				else
				{
					num += 1440L;
				}
				cellFormat.CellX = num;
			}
		}

		// Token: 0x04002E59 RID: 11865
		private CellFormat _rowCellFormat;

		// Token: 0x04002E5A RID: 11866
		private CellWidth _widthA;

		// Token: 0x04002E5B RID: 11867
		private CellWidth _widthB;

		// Token: 0x04002E5C RID: 11868
		private CellWidth _widthRow;

		// Token: 0x04002E5D RID: 11869
		private ArrayList _cellFormats;

		// Token: 0x04002E5E RID: 11870
		private long _nTrgaph;

		// Token: 0x04002E5F RID: 11871
		private long _nTrleft;

		// Token: 0x04002E60 RID: 11872
		private DirState _dir;
	}
}
