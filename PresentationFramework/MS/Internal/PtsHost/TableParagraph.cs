using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000149 RID: 329
	internal sealed class TableParagraph : BaseParagraph
	{
		// Token: 0x06000A3C RID: 2620 RVA: 0x001241FB File Offset: 0x001231FB
		internal TableParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
			this.Table.TableStructureChanged += this.TableStructureChanged;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0012421C File Offset: 0x0012321C
		public override void Dispose()
		{
			this.Table.TableStructureChanged -= this.TableStructureChanged;
			BaseParagraph baseParagraph = this._firstChild;
			while (baseParagraph != null)
			{
				BaseParagraph baseParagraph2 = baseParagraph;
				baseParagraph = baseParagraph.Next;
				baseParagraph2.Dispose();
				baseParagraph2.Next = null;
				baseParagraph2.Previous = null;
			}
			this._firstChild = null;
			base.Dispose();
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00124278 File Offset: 0x00123278
		internal override void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr)
		{
			if (suppressTopSpace && (base.StructuralCache.CurrentFormatContext.FinitePage || mcs == null))
			{
				dvr = 0;
				return;
			}
			MbpInfo mbp = MbpInfo.FromElement(this.Table, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState marginCollapsingState = null;
			MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbp, mcs, out marginCollapsingState, out dvr);
			if (marginCollapsingState != null)
			{
				dvr = marginCollapsingState.Margin;
				marginCollapsingState.Dispose();
				marginCollapsingState = null;
			}
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x001242E6 File Offset: 0x001232E6
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			fspap.idobj = PtsHost.TableParagraphId;
			fspap.fKeepWithNext = 0;
			fspap.fBreakPageBefore = 0;
			fspap.fBreakColumnBefore = 0;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00124308 File Offset: 0x00123308
		internal override void CreateParaclient(out IntPtr pfsparaclient)
		{
			TableParaClient tableParaClient = new TableParaClient(this);
			pfsparaclient = tableParaClient.Handle;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00124324 File Offset: 0x00123324
		internal void GetTableProperties(uint fswdirTrack, out PTS.FSTABLEOBJPROPS fstableobjprops)
		{
			fstableobjprops = default(PTS.FSTABLEOBJPROPS);
			fstableobjprops.fskclear = PTS.FSKCLEAR.fskclearNone;
			fstableobjprops.ktablealignment = PTS.FSKTABLEOBJALIGNMENT.fsktableobjAlignLeft;
			fstableobjprops.fFloat = 0;
			fstableobjprops.fskwr = PTS.FSKWRAP.fskwrBoth;
			fstableobjprops.fDelayNoProgress = 0;
			fstableobjprops.dvrCaptionTop = 0;
			fstableobjprops.dvrCaptionBottom = 0;
			fstableobjprops.durCaptionLeft = 0;
			fstableobjprops.durCaptionRight = 0;
			fstableobjprops.fswdirTable = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00124398 File Offset: 0x00123398
		internal void GetMCSClientAfterTable(uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr ppmcsclientOut)
		{
			ppmcsclientOut = IntPtr.Zero;
			MbpInfo mbp = MbpInfo.FromElement(this.Table, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState mcsCurrent = null;
			if (pmcsclientIn != IntPtr.Zero)
			{
				mcsCurrent = (base.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
			}
			MarginCollapsingState marginCollapsingState = null;
			int num;
			MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbp, mcsCurrent, out marginCollapsingState, out num);
			if (marginCollapsingState != null)
			{
				ppmcsclientOut = marginCollapsingState.Handle;
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00124407 File Offset: 0x00123407
		internal void GetFirstHeaderRow(int fRepeatedHeader, out int fFound, out IntPtr pnmFirstHeaderRow)
		{
			fFound = 0;
			pnmFirstHeaderRow = IntPtr.Zero;
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00124413 File Offset: 0x00123413
		internal void GetNextHeaderRow(int fRepeatedHeader, IntPtr nmHeaderRow, out int fFound, out IntPtr pnmNextHeaderRow)
		{
			fFound = 0;
			pnmNextHeaderRow = IntPtr.Zero;
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00124407 File Offset: 0x00123407
		internal void GetFirstFooterRow(int fRepeatedFooter, out int fFound, out IntPtr pnmFirstFooterRow)
		{
			fFound = 0;
			pnmFirstFooterRow = IntPtr.Zero;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00124413 File Offset: 0x00123413
		internal void GetNextFooterRow(int fRepeatedFooter, IntPtr nmFooterRow, out int fFound, out IntPtr pnmNextFooterRow)
		{
			fFound = 0;
			pnmNextFooterRow = IntPtr.Zero;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00124420 File Offset: 0x00123420
		internal void GetFirstRow(out int fFound, out IntPtr pnmFirstRow)
		{
			if (this._firstChild == null)
			{
				TableRow tableRow = null;
				int num = 0;
				while (num < this.Table.RowGroups.Count && tableRow == null)
				{
					TableRowGroup tableRowGroup = this.Table.RowGroups[num];
					if (tableRowGroup.Rows.Count > 0)
					{
						tableRow = tableRowGroup.Rows[0];
						Invariant.Assert(tableRow.Index != -1);
					}
					num++;
				}
				if (tableRow != null)
				{
					this._firstChild = new RowParagraph(tableRow, base.StructuralCache);
					((RowParagraph)this._firstChild).CalculateRowSpans();
				}
			}
			if (this._firstChild != null)
			{
				fFound = 1;
				pnmFirstRow = this._firstChild.Handle;
				return;
			}
			fFound = 0;
			pnmFirstRow = IntPtr.Zero;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x001244E0 File Offset: 0x001234E0
		internal void GetNextRow(IntPtr nmRow, out int fFound, out IntPtr pnmNextRow)
		{
			BaseParagraph baseParagraph = (RowParagraph)base.PtsContext.HandleToObject(nmRow);
			BaseParagraph baseParagraph2 = baseParagraph.Next;
			if (baseParagraph2 == null)
			{
				TableRow row = ((RowParagraph)baseParagraph).Row;
				TableRowGroup rowGroup = row.RowGroup;
				TableRow tableRow = null;
				int num = row.Index + 1;
				int num2 = rowGroup.Index + 1;
				if (num < rowGroup.Rows.Count)
				{
					tableRow = rowGroup.Rows[num];
				}
				while (tableRow == null && num2 != this.Table.RowGroups.Count)
				{
					TableRowCollection rows = this.Table.RowGroups[num2].Rows;
					if (rows.Count > 0)
					{
						tableRow = rows[0];
					}
					num2++;
				}
				if (tableRow != null)
				{
					baseParagraph2 = new RowParagraph(tableRow, base.StructuralCache);
					baseParagraph.Next = baseParagraph2;
					baseParagraph2.Previous = baseParagraph;
					((RowParagraph)baseParagraph2).CalculateRowSpans();
				}
			}
			if (baseParagraph2 != null)
			{
				fFound = 1;
				pnmNextRow = baseParagraph2.Handle;
				return;
			}
			fFound = 0;
			pnmNextRow = IntPtr.Zero;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x001245DD File Offset: 0x001235DD
		internal void UpdFChangeInHeaderFooter(out int fHeaderChanged, out int fFooterChanged, out int fRepeatedHeaderChanged, out int fRepeatedFooterChanged)
		{
			fHeaderChanged = 0;
			fRepeatedHeaderChanged = 0;
			fFooterChanged = 0;
			fRepeatedFooterChanged = 0;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x001245EC File Offset: 0x001235EC
		internal void UpdGetFirstChangeInTable(out int fFound, out int fChangeFirst, out IntPtr pnmRowBeforeChange)
		{
			fFound = 1;
			fChangeFirst = 1;
			pnmRowBeforeChange = IntPtr.Zero;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0011EA00 File Offset: 0x0011DA00
		internal void GetDistributionKind(uint fswdirTable, out PTS.FSKTABLEHEIGHTDISTRIBUTION tabledistr)
		{
			tabledistr = PTS.FSKTABLEHEIGHTDISTRIBUTION.fskdistributeUnchanged;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0011E8C1 File Offset: 0x0011D8C1
		internal override void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			base.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			fskch = PTS.FSKCHANGE.fskchNew;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x001245FC File Offset: 0x001235FC
		internal override bool InvalidateStructure(int startPosition)
		{
			bool result = true;
			for (RowParagraph rowParagraph = this._firstChild as RowParagraph; rowParagraph != null; rowParagraph = (rowParagraph.Next as RowParagraph))
			{
				if (!this.InvalidateRowStructure(rowParagraph, startPosition))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00124638 File Offset: 0x00123638
		internal override void InvalidateFormatCache()
		{
			for (RowParagraph rowParagraph = this._firstChild as RowParagraph; rowParagraph != null; rowParagraph = (rowParagraph.Next as RowParagraph))
			{
				this.InvalidateRowFormatCache(rowParagraph);
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x00124669 File Offset: 0x00123669
		internal Table Table
		{
			get
			{
				return (Table)base.Element;
			}
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00124678 File Offset: 0x00123678
		private bool InvalidateRowStructure(RowParagraph rowParagraph, int startPosition)
		{
			bool result = true;
			for (int i = 0; i < rowParagraph.Cells.Length; i++)
			{
				CellParagraph cellParagraph = rowParagraph.Cells[i];
				if (cellParagraph.ParagraphEndCharacterPosition < startPosition || !cellParagraph.InvalidateStructure(startPosition))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x001246B8 File Offset: 0x001236B8
		private void InvalidateRowFormatCache(RowParagraph rowParagraph)
		{
			for (int i = 0; i < rowParagraph.Cells.Length; i++)
			{
				rowParagraph.Cells[i].InvalidateFormatCache();
			}
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x001246E8 File Offset: 0x001236E8
		private void TableStructureChanged(object sender, EventArgs e)
		{
			for (BaseParagraph baseParagraph = this._firstChild; baseParagraph != null; baseParagraph = baseParagraph.Next)
			{
				baseParagraph.Dispose();
			}
			this._firstChild = null;
			int num = this.Table.SymbolCount - 2;
			if (num > 0)
			{
				DirtyTextRange dtr = new DirtyTextRange(this.Table.ContentStartOffset, num, num, false);
				base.StructuralCache.AddDirtyTextRange(dtr);
			}
			if (base.StructuralCache.FormattingOwner.Formatter != null)
			{
				base.StructuralCache.FormattingOwner.Formatter.OnContentInvalidated(true, this.Table.ContentStart, this.Table.ContentEnd);
			}
		}

		// Token: 0x04000816 RID: 2070
		private BaseParagraph _firstChild;
	}
}
