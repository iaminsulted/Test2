using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020001DD RID: 477
	internal sealed class ContainerParagraphResult : ParagraphResult
	{
		// Token: 0x060010E4 RID: 4324 RVA: 0x001420CB File Offset: 0x001410CB
		internal ContainerParagraphResult(ContainerParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x001420D4 File Offset: 0x001410D4
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			return ((ContainerParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060010E6 RID: 4326 RVA: 0x001420E9 File Offset: 0x001410E9
		internal ReadOnlyCollection<ParagraphResult> Paragraphs
		{
			get
			{
				if (this._paragraphs == null)
				{
					this._paragraphs = ((ContainerParaClient)this._paraClient).GetChildrenParagraphResults(out this._hasTextContent);
				}
				Invariant.Assert(this._paragraphs != null, "Paragraph collection is empty");
				return this._paragraphs;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x00142128 File Offset: 0x00141128
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

		// Token: 0x04000AE5 RID: 2789
		private ReadOnlyCollection<ParagraphResult> _paragraphs;
	}
}
