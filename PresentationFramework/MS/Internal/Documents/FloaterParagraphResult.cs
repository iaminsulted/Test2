using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020001E4 RID: 484
	internal sealed class FloaterParagraphResult : FloaterBaseParagraphResult
	{
		// Token: 0x0600110F RID: 4367 RVA: 0x00142659 File Offset: 0x00141659
		internal FloaterParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x00142662 File Offset: 0x00141662
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = ((FloaterParaClient)this._paraClient).GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Columns collection is null");
				}
				return this._columns;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x001426A1 File Offset: 0x001416A1
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

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x001426B8 File Offset: 0x001416B8
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				if (this._floatingElements == null)
				{
					this._floatingElements = ((FloaterParaClient)this._paraClient).FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x001424F8 File Offset: 0x001414F8
		internal Vector ContentOffset
		{
			get
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this._paraClient.Paragraph.Element, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				return new Vector(base.LayoutBox.X + TextDpi.FromTextDpi(mbpInfo.BPLeft), base.LayoutBox.Y + TextDpi.FromTextDpi(mbpInfo.BPTop));
			}
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x001426F4 File Offset: 0x001416F4
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect, out bool success)
		{
			success = false;
			if (this.Contains(startPosition, true))
			{
				success = true;
				ITextPointer endPosition2 = (endPosition.CompareTo(base.EndPosition) < 0) ? endPosition : base.EndPosition;
				return ((FloaterParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(this.Columns, this.FloatingElements, startPosition, endPosition2, visibleRect);
			}
			return null;
		}

		// Token: 0x04000AF0 RID: 2800
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x04000AF1 RID: 2801
		private ReadOnlyCollection<ParagraphResult> _floatingElements;
	}
}
