using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020001E2 RID: 482
	internal sealed class FigureParagraphResult : ParagraphResult
	{
		// Token: 0x06001108 RID: 4360 RVA: 0x001420CB File Offset: 0x001410CB
		internal FigureParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0014256E File Offset: 0x0014156E
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = ((FigureParaClient)this._paraClient).GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Columns collection is null");
				}
				return this._columns;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x001425AD File Offset: 0x001415AD
		internal override bool HasTextContent
		{
			get
			{
				if (this._columns == null)
				{
					ReadOnlyCollection<ColumnResult> columns = this.Columns;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x001425C4 File Offset: 0x001415C4
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				if (this._floatingElements == null)
				{
					this._floatingElements = ((FigureParaClient)this._paraClient).FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600110C RID: 4364 RVA: 0x001424F8 File Offset: 0x001414F8
		internal Vector ContentOffset
		{
			get
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this._paraClient.Paragraph.Element, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				return new Vector(base.LayoutBox.X + TextDpi.FromTextDpi(mbpInfo.BPLeft), base.LayoutBox.Y + TextDpi.FromTextDpi(mbpInfo.BPTop));
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00142600 File Offset: 0x00141600
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect, out bool success)
		{
			success = false;
			if (this.Contains(startPosition, true))
			{
				success = true;
				ITextPointer endPosition2 = (endPosition.CompareTo(base.EndPosition) < 0) ? endPosition : base.EndPosition;
				return ((FigureParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(this.Columns, this.FloatingElements, startPosition, endPosition2, visibleRect);
			}
			return null;
		}

		// Token: 0x04000AEE RID: 2798
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x04000AEF RID: 2799
		private ReadOnlyCollection<ParagraphResult> _floatingElements;
	}
}
