using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200013C RID: 316
	internal sealed class RowParagraph : BaseParagraph
	{
		// Token: 0x06000991 RID: 2449 RVA: 0x0010AEA3 File Offset: 0x00109EA3
		internal RowParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0011E868 File Offset: 0x0011D868
		public override void Dispose()
		{
			if (this._cellParagraphs != null)
			{
				for (int i = 0; i < this._cellParagraphs.Length; i++)
				{
					this._cellParagraphs[i].Dispose();
				}
			}
			this._cellParagraphs = null;
			base.Dispose();
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0011E8B2 File Offset: 0x0011D8B2
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			Invariant.Assert(false);
			paraClientHandle = IntPtr.Zero;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0011E8C1 File Offset: 0x0011D8C1
		internal override void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			base.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			fskch = PTS.FSKCHANGE.fskchNew;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0011E8D0 File Offset: 0x0011D8D0
		internal void GetRowProperties(uint fswdirTable, out PTS.FSTABLEROWPROPS rowprops)
		{
			bool flag = this.Row.Index == this.Row.RowGroup.Rows.Count - 1;
			PTS.FSKROWHEIGHTRESTRICTION fskrowheight;
			int num;
			this.GetRowHeight(out fskrowheight, out num);
			rowprops = default(PTS.FSTABLEROWPROPS);
			rowprops.fskrowbreak = PTS.FSKROWBREAKRESTRICTION.fskrowbreakAnywhere;
			rowprops.fskrowheight = fskrowheight;
			rowprops.dvrRowHeightRestriction = 0;
			rowprops.dvrAboveRow = num;
			rowprops.dvrBelowRow = num;
			int num2 = TextDpi.ToTextDpi(this.Table.InternalCellSpacing);
			MbpInfo mbpInfo = MbpInfo.FromElement(this.Table, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (this.Row.Index == 0 && this.Table.IsFirstNonEmptyRowGroup(this.Row.RowGroup.Index))
			{
				rowprops.dvrAboveTopRow = mbpInfo.BPTop + num2 / 2;
			}
			else
			{
				rowprops.dvrAboveTopRow = num;
			}
			if (flag && this.Table.IsLastNonEmptyRowGroup(this.Row.RowGroup.Index))
			{
				rowprops.dvrBelowBottomRow = mbpInfo.BPBottom + num2 / 2;
			}
			else
			{
				rowprops.dvrBelowBottomRow = num;
			}
			rowprops.dvrAboveRowBreak = num2 / 2;
			rowprops.dvrBelowRowBreak = num2 / 2;
			rowprops.cCells = this.Row.FormatCellCount;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0011EA00 File Offset: 0x0011DA00
		internal void FInterruptFormattingTable(int dvr, out int fInterrupt)
		{
			fInterrupt = 0;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0011EA08 File Offset: 0x0011DA08
		internal unsafe void CalcHorizontalBBoxOfRow(int cCells, IntPtr* rgnmCell, IntPtr* rgpfsCell, out int urBBox, out int durBBox)
		{
			urBBox = 0;
			durBBox = 0;
			for (int i = 0; i < cCells; i++)
			{
				if (rgpfsCell[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] != IntPtr.Zero)
				{
					CellParaClient cellParaClient = base.PtsContext.HandleToObject(rgpfsCell[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]) as CellParaClient;
					PTS.ValidateHandle(cellParaClient);
					durBBox = TextDpi.ToTextDpi(cellParaClient.TableParaClient.TableDesiredWidth);
					return;
				}
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0011EA78 File Offset: 0x0011DA78
		internal unsafe void GetCells(int cCells, IntPtr* rgnmCell, PTS.FSTABLEKCELLMERGE* rgkcellmerge)
		{
			Invariant.Assert(cCells == this.Row.FormatCellCount);
			Invariant.Assert(cCells >= this.Row.Cells.Count);
			int num = 0;
			for (int i = 0; i < this.Row.Cells.Count; i++)
			{
				if (this.Row.Cells[i].RowSpan == 1)
				{
					rgnmCell[(IntPtr)num * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = this._cellParagraphs[i].Handle;
					rgkcellmerge[num] = PTS.FSTABLEKCELLMERGE.fskcellmergeNo;
					num++;
				}
			}
			Invariant.Assert(cCells == num + this._spannedCells.Length);
			if (this._spannedCells.Length != 0)
			{
				bool flag = this.Row.Index == this.Row.RowGroup.Rows.Count - 1;
				for (int j = 0; j < this._spannedCells.Length; j++)
				{
					TableCell cell = this._spannedCells[j].Cell;
					rgnmCell[(IntPtr)num * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = this._spannedCells[j].Handle;
					if (cell.RowIndex == this.Row.Index)
					{
						rgkcellmerge[num] = (flag ? PTS.FSTABLEKCELLMERGE.fskcellmergeNo : PTS.FSTABLEKCELLMERGE.fskcellmergeFirst);
					}
					else if (this.Row.Index - cell.RowIndex + 1 < cell.RowSpan)
					{
						rgkcellmerge[num] = (flag ? PTS.FSTABLEKCELLMERGE.fskcellmergeLast : PTS.FSTABLEKCELLMERGE.fskcellmergeMiddle);
					}
					else
					{
						rgkcellmerge[num] = PTS.FSTABLEKCELLMERGE.fskcellmergeLast;
					}
					num++;
				}
			}
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0011EBF0 File Offset: 0x0011DBF0
		internal void CalculateRowSpans()
		{
			RowParagraph previousRow = null;
			if (this.Row.Index != 0 && this.Previous != null)
			{
				previousRow = (RowParagraph)this.Previous;
			}
			Invariant.Assert(this._cellParagraphs == null);
			this._cellParagraphs = new CellParagraph[this.Row.Cells.Count];
			for (int i = 0; i < this.Row.Cells.Count; i++)
			{
				this._cellParagraphs[i] = new CellParagraph(this.Row.Cells[i], base.StructuralCache);
			}
			Invariant.Assert(this._spannedCells == null);
			if (this.Row.SpannedCells != null)
			{
				this._spannedCells = new CellParagraph[this.Row.SpannedCells.Length];
			}
			else
			{
				this._spannedCells = Array.Empty<CellParagraph>();
			}
			for (int j = 0; j < this._spannedCells.Length; j++)
			{
				this._spannedCells[j] = this.FindCellParagraphForCell(previousRow, this.Row.SpannedCells[j]);
			}
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0011ECF8 File Offset: 0x0011DCF8
		internal void GetRowHeight(out PTS.FSKROWHEIGHTRESTRICTION fskrowheight, out int dvrAboveBelow)
		{
			bool flag = this.Row.Index == this.Row.RowGroup.Rows.Count - 1;
			if (this.Row.HasRealCells || (flag && this._spannedCells.Length != 0))
			{
				fskrowheight = PTS.FSKROWHEIGHTRESTRICTION.fskrowheightNatural;
				dvrAboveBelow = TextDpi.ToTextDpi(this.Table.InternalCellSpacing / 2.0);
				return;
			}
			fskrowheight = PTS.FSKROWHEIGHTRESTRICTION.fskrowheightExactNoBreak;
			dvrAboveBelow = 0;
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0011ED69 File Offset: 0x0011DD69
		internal TableRow Row
		{
			get
			{
				return (TableRow)base.Element;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0011ED76 File Offset: 0x0011DD76
		internal Table Table
		{
			get
			{
				return this.Row.Table;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0011ED83 File Offset: 0x0011DD83
		internal CellParagraph[] Cells
		{
			get
			{
				return this._cellParagraphs;
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0011ED8C File Offset: 0x0011DD8C
		private CellParagraph FindCellParagraphForCell(RowParagraph previousRow, TableCell cell)
		{
			for (int i = 0; i < this._cellParagraphs.Length; i++)
			{
				if (this._cellParagraphs[i].Cell == cell)
				{
					return this._cellParagraphs[i];
				}
			}
			if (previousRow != null)
			{
				for (int j = 0; j < previousRow._spannedCells.Length; j++)
				{
					if (previousRow._spannedCells[j].Cell == cell)
					{
						return previousRow._spannedCells[j];
					}
				}
			}
			Invariant.Assert(false, "Structural integrity for table not correct.");
			return null;
		}

		// Token: 0x040007E9 RID: 2025
		private CellParagraph[] _cellParagraphs;

		// Token: 0x040007EA RID: 2026
		private CellParagraph[] _spannedCells;
	}
}
