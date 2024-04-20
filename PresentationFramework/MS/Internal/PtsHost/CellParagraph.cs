using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000110 RID: 272
	internal sealed class CellParagraph : SubpageParagraph
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x0010A5F5 File Offset: 0x001095F5
		internal CellParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
			this._isInterruptible = false;
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0010A606 File Offset: 0x00109606
		internal TableCell Cell
		{
			get
			{
				return (TableCell)base.Element;
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0010A614 File Offset: 0x00109614
		internal void FormatCellFinite(TableParaClient tableParaClient, IntPtr pfsbrkcellIn, IntPtr pfsFtnRejector, int fEmptyOK, uint fswdirTable, int dvrAvailable, out PTS.FSFMTR pfmtr, out IntPtr ppfscell, out IntPtr pfsbrkcellOut, out int dvrUsed)
		{
			CellParaClient cellParaClient = new CellParaClient(this, tableParaClient);
			Size subpageSize = new Size(cellParaClient.CalculateCellWidth(tableParaClient), Math.Max(TextDpi.FromTextDpi(dvrAvailable), 0.0));
			cellParaClient.FormatCellFinite(subpageSize, pfsbrkcellIn, PTS.ToBoolean(fEmptyOK), fswdirTable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA.fsksuppresshardbreakbeforefirstparaNone, out pfmtr, out dvrUsed, out pfsbrkcellOut);
			ppfscell = cellParaClient.Handle;
			if (pfmtr.kstop == PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace)
			{
				cellParaClient.Dispose();
				ppfscell = IntPtr.Zero;
				dvrUsed = 0;
			}
			if (dvrAvailable < dvrUsed)
			{
				if (PTS.ToBoolean(fEmptyOK))
				{
					if (cellParaClient != null)
					{
						cellParaClient.Dispose();
					}
					if (pfsbrkcellOut != IntPtr.Zero)
					{
						PTS.Validate(PTS.FsDestroySubpageBreakRecord(cellParaClient.PtsContext.Context, pfsbrkcellOut), cellParaClient.PtsContext);
						pfsbrkcellOut = IntPtr.Zero;
					}
					ppfscell = IntPtr.Zero;
					pfmtr.kstop = PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace;
					dvrUsed = 0;
					return;
				}
				pfmtr.fForcedProgress = 1;
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0010A6F4 File Offset: 0x001096F4
		internal void FormatCellBottomless(TableParaClient tableParaClient, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out IntPtr ppfscell, out int dvrUsed)
		{
			CellParaClient cellParaClient = new CellParaClient(this, tableParaClient);
			cellParaClient.FormatCellBottomless(fswdirTable, cellParaClient.CalculateCellWidth(tableParaClient), out fmtrbl, out dvrUsed);
			ppfscell = cellParaClient.Handle;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0010A723 File Offset: 0x00109723
		internal void UpdateBottomlessCell(CellParaClient cellParaClient, TableParaClient tableParaClient, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out int dvrUsed)
		{
			cellParaClient.UpdateBottomlessCell(fswdirTable, cellParaClient.CalculateCellWidth(tableParaClient), out fmtrbl, out dvrUsed);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0010A737 File Offset: 0x00109737
		internal void SetCellHeight(CellParaClient cellParaClient, TableParaClient tableParaClient, IntPtr subpageBreakRecord, int fBrokenHere, uint fswdirTable, int dvrActual)
		{
			cellParaClient.ArrangeHeight = TextDpi.FromTextDpi(dvrActual);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0010A746 File Offset: 0x00109746
		internal void UpdGetCellChange(out int fWidthChanged, out PTS.FSKCHANGE fskchCell)
		{
			fWidthChanged = 1;
			fskchCell = PTS.FSKCHANGE.fskchNew;
		}
	}
}
