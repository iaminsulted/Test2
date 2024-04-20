using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.PtsTable;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200010E RID: 270
	internal sealed class CellParaClient : SubpageParaClient
	{
		// Token: 0x060006D8 RID: 1752 RVA: 0x00109E20 File Offset: 0x00108E20
		internal CellParaClient(CellParagraph cellParagraph, TableParaClient tableParaClient) : base(cellParagraph)
		{
			this._tableParaClient = tableParaClient;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00109E30 File Offset: 0x00108E30
		internal void Arrange(int du, int dv, PTS.FSRECT rcTable, FlowDirection tableFlowDirection, PageContext pageContext)
		{
			CalculatedColumn[] calculatedColumns = this._tableParaClient.CalculatedColumns;
			double internalCellSpacing = this.Table.InternalCellSpacing;
			double num = -internalCellSpacing;
			int num2 = this.Cell.ColumnIndex + this.Cell.ColumnSpan - 1;
			do
			{
				num += calculatedColumns[num2].DurWidth + internalCellSpacing;
			}
			while (--num2 >= this.ColumnIndex);
			if (tableFlowDirection != base.PageFlowDirection)
			{
				PTS.FSRECT pageRect = pageContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.PageFlowDirection), ref pageRect, ref rcTable, PTS.FlowDirectionToFswdir(tableFlowDirection), out rcTable));
			}
			this._rect.u = du + rcTable.u;
			this._rect.v = dv + rcTable.v;
			this._rect.du = TextDpi.ToTextDpi(num);
			this._rect.dv = TextDpi.ToTextDpi(this._arrangeHeight);
			if (tableFlowDirection != base.PageFlowDirection)
			{
				PTS.FSRECT pageRect2 = pageContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(tableFlowDirection), ref pageRect2, ref this._rect, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out this._rect));
			}
			this._flowDirectionParent = tableFlowDirection;
			this._flowDirection = (FlowDirection)base.Paragraph.Element.GetValue(FrameworkElement.FlowDirectionProperty);
			this._pageContext = pageContext;
			this.OnArrange();
			if (this._paraHandle.Value != IntPtr.Zero)
			{
				PTS.Validate(PTS.FsClearUpdateInfoInSubpage(base.PtsContext.Context, this._paraHandle.Value), base.PtsContext);
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00109FC2 File Offset: 0x00108FC2
		internal void ValidateVisual()
		{
			this.ValidateVisual(PTS.FSKUPDATE.fskupdNew);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00109FCC File Offset: 0x00108FCC
		internal void FormatCellFinite(Size subpageSize, IntPtr breakRecordIn, bool isEmptyOk, uint fswdir, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out int dvrUsed, out IntPtr breakRecordOut)
		{
			if (this.CellParagraph.StructuralCache.DtrList != null && breakRecordIn != IntPtr.Zero)
			{
				this.CellParagraph.InvalidateStructure(TextContainerHelper.GetCPFromElement(this.CellParagraph.StructuralCache.TextContainer, this.CellParagraph.Element, ElementEdge.BeforeStart));
			}
			PTS.FSPAP fspap = default(PTS.FSPAP);
			this.CellParagraph.GetParaProperties(ref fspap);
			PTS.FSRECT fsrect = default(PTS.FSRECT);
			fsrect.u = (fsrect.v = 0);
			fsrect.du = TextDpi.ToTextDpi(subpageSize.Width);
			fsrect.dv = TextDpi.ToTextDpi(subpageSize.Height);
			bool condition = breakRecordIn != IntPtr.Zero;
			IntPtr value;
			PTS.FSBBOX fsbbox;
			IntPtr zero;
			PTS.FSKCLEAR fskclear;
			int num;
			this.CellParagraph.FormatParaFinite(this, breakRecordIn, PTS.FromBoolean(true), IntPtr.Zero, PTS.FromBoolean(isEmptyOk), PTS.FromBoolean(condition), fswdir, ref fsrect, null, PTS.FSKCLEAR.fskclearNone, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out value, out breakRecordOut, out dvrUsed, out fsbbox, out zero, out fskclear, out num);
			if (zero != IntPtr.Zero)
			{
				MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				dvrUsed += marginCollapsingState.Margin;
				marginCollapsingState.Dispose();
				zero = IntPtr.Zero;
			}
			this._paraHandle.Value = value;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0010A11C File Offset: 0x0010911C
		internal void FormatCellBottomless(uint fswdir, double width, out PTS.FSFMTRBL fmtrbl, out int dvrUsed)
		{
			if (this.CellParagraph.StructuralCache.DtrList != null)
			{
				this.CellParagraph.InvalidateStructure(TextContainerHelper.GetCPFromElement(this.CellParagraph.StructuralCache.TextContainer, this.CellParagraph.Element, ElementEdge.BeforeStart));
			}
			PTS.FSPAP fspap = default(PTS.FSPAP);
			this.CellParagraph.GetParaProperties(ref fspap);
			IntPtr value;
			PTS.FSBBOX fsbbox;
			IntPtr zero;
			PTS.FSKCLEAR fskclear;
			int num;
			int num2;
			this.CellParagraph.FormatParaBottomless(this, PTS.FromBoolean(false), fswdir, 0, TextDpi.ToTextDpi(width), 0, null, PTS.FSKCLEAR.fskclearNone, PTS.FromBoolean(true), out fmtrbl, out value, out dvrUsed, out fsbbox, out zero, out fskclear, out num, out num2);
			if (zero != IntPtr.Zero)
			{
				MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				dvrUsed += marginCollapsingState.Margin;
				marginCollapsingState.Dispose();
				zero = IntPtr.Zero;
			}
			this._paraHandle.Value = value;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0010A200 File Offset: 0x00109200
		internal void UpdateBottomlessCell(uint fswdir, double width, out PTS.FSFMTRBL fmtrbl, out int dvrUsed)
		{
			PTS.FSPAP fspap = default(PTS.FSPAP);
			this.CellParagraph.GetParaProperties(ref fspap);
			PTS.FSBBOX fsbbox;
			IntPtr zero;
			PTS.FSKCLEAR fskclear;
			int num;
			int num2;
			this.CellParagraph.UpdateBottomlessPara(this._paraHandle.Value, this, PTS.FromBoolean(false), fswdir, 0, TextDpi.ToTextDpi(width), 0, null, PTS.FSKCLEAR.fskclearNone, PTS.FromBoolean(true), out fmtrbl, out dvrUsed, out fsbbox, out zero, out fskclear, out num, out num2);
			if (zero != IntPtr.Zero)
			{
				MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				dvrUsed += marginCollapsingState.Margin;
				marginCollapsingState.Dispose();
				zero = IntPtr.Zero;
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0010A2A0 File Offset: 0x001092A0
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			Geometry geometry = null;
			if (endPosition.CompareTo(this.Cell.StaticElementEnd) >= 0)
			{
				geometry = new RectangleGeometry(this._rect.FromTextDpi());
			}
			else
			{
				SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)this.CreateParagraphResult();
				ReadOnlyCollection<ColumnResult> columns = subpageParagraphResult.Columns;
				Transform transform = new TranslateTransform(-TextDpi.FromTextDpi(base.ContentRect.u), -TextDpi.FromTextDpi(base.ContentRect.v));
				visibleRect = transform.TransformBounds(visibleRect);
				geometry = TextDocumentView.GetTightBoundingGeometryFromTextPositionsHelper(columns[0].Paragraphs, subpageParagraphResult.FloatingElements, startPosition, endPosition, 0.0, visibleRect);
				if (geometry != null)
				{
					Rect viewport = new Rect(0.0, 0.0, TextDpi.FromTextDpi(base.ContentRect.du), TextDpi.FromTextDpi(base.ContentRect.dv));
					CaretElement.ClipGeometryByViewport(ref geometry, viewport);
					transform = new TranslateTransform(TextDpi.FromTextDpi(base.ContentRect.u), TextDpi.FromTextDpi(base.ContentRect.v));
					CaretElement.AddTransformToGeometry(geometry, transform);
				}
			}
			return geometry;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0010A3B4 File Offset: 0x001093B4
		internal double CalculateCellWidth(TableParaClient tableParaClient)
		{
			CalculatedColumn[] calculatedColumns = tableParaClient.CalculatedColumns;
			double internalCellSpacing = this.Table.InternalCellSpacing;
			double num = -internalCellSpacing;
			int num2 = this.Cell.ColumnIndex + this.Cell.ColumnSpan - 1;
			do
			{
				num += calculatedColumns[num2].DurWidth + internalCellSpacing;
			}
			while (--num2 >= this.Cell.ColumnIndex);
			return num;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0010A415 File Offset: 0x00109415
		internal TableCell Cell
		{
			get
			{
				return this.CellParagraph.Cell;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0010A422 File Offset: 0x00109422
		internal Table Table
		{
			get
			{
				return this.Cell.Table;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0010A42F File Offset: 0x0010942F
		internal CellParagraph CellParagraph
		{
			get
			{
				return (CellParagraph)this._paragraph;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0010A43C File Offset: 0x0010943C
		internal int ColumnIndex
		{
			get
			{
				return this.Cell.ColumnIndex;
			}
		}

		// Token: 0x17000112 RID: 274
		// (set) Token: 0x060006E4 RID: 1764 RVA: 0x0010A449 File Offset: 0x00109449
		internal double ArrangeHeight
		{
			set
			{
				this._arrangeHeight = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0010A452 File Offset: 0x00109452
		internal TableParaClient TableParaClient
		{
			get
			{
				return this._tableParaClient;
			}
		}

		// Token: 0x0400072D RID: 1837
		private double _arrangeHeight;

		// Token: 0x0400072E RID: 1838
		private TableParaClient _tableParaClient;
	}
}
