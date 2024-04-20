using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020001DF RID: 479
	internal sealed class TableParagraphResult : ParagraphResult
	{
		// Token: 0x060010F5 RID: 4341 RVA: 0x001420CB File Offset: 0x001410CB
		internal TableParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x001422F1 File Offset: 0x001412F1
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPoint(Point point, bool snapToText)
		{
			return ((TableParaClient)this._paraClient).GetParagraphsFromPoint(point, snapToText);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00142305 File Offset: 0x00141305
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPosition(ITextPointer position)
		{
			return ((TableParaClient)this._paraClient).GetParagraphsFromPosition(position);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00142318 File Offset: 0x00141318
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			return ((TableParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0014232D File Offset: 0x0014132D
		internal CellParaClient GetCellParaClientFromPosition(ITextPointer position)
		{
			return ((TableParaClient)this._paraClient).GetCellParaClientFromPosition(position);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00142340 File Offset: 0x00141340
		internal CellParaClient GetCellAbove(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			return ((TableParaClient)this._paraClient).GetCellAbove(suggestedX, rowGroupIndex, rowIndex);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00142355 File Offset: 0x00141355
		internal CellParaClient GetCellBelow(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			return ((TableParaClient)this._paraClient).GetCellBelow(suggestedX, rowGroupIndex, rowIndex);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0014236A File Offset: 0x0014136A
		internal CellInfo GetCellInfoFromPoint(Point point)
		{
			return ((TableParaClient)this._paraClient).GetCellInfoFromPoint(point);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0014237D File Offset: 0x0014137D
		internal Rect GetRectangleFromRowEndPosition(ITextPointer position)
		{
			return ((TableParaClient)this._paraClient).GetRectangleFromRowEndPosition(position);
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x00142390 File Offset: 0x00141390
		internal ReadOnlyCollection<ParagraphResult> Paragraphs
		{
			get
			{
				if (this._paragraphs == null)
				{
					this._paragraphs = ((TableParaClient)this._paraClient).GetChildrenParagraphResults(out this._hasTextContent);
				}
				Invariant.Assert(this._paragraphs != null, "Paragraph collection is empty");
				return this._paragraphs;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x001423CF File Offset: 0x001413CF
		internal override bool HasTextContent
		{
			get
			{
				if (this._paragraphs == null)
				{
					ReadOnlyCollection<ParagraphResult> paragraphs = this.Paragraphs;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x04000AE9 RID: 2793
		private ReadOnlyCollection<ParagraphResult> _paragraphs;
	}
}
