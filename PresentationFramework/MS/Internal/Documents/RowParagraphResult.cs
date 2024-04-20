using System;
using System.Collections.ObjectModel;
using System.Windows;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020001E0 RID: 480
	internal sealed class RowParagraphResult : ParagraphResult
	{
		// Token: 0x06001100 RID: 4352 RVA: 0x001423E6 File Offset: 0x001413E6
		internal RowParagraphResult(BaseParaClient paraClient, int index, Rect rowRect, RowParagraph rowParagraph) : base(paraClient, rowRect, rowParagraph.Element)
		{
			this._index = index;
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x00142400 File Offset: 0x00141400
		internal ReadOnlyCollection<ParagraphResult> CellParagraphs
		{
			get
			{
				if (this._cells == null)
				{
					this._cells = ((TableParaClient)this._paraClient).GetChildrenParagraphResultsForRow(this._index, out this._hasTextContent);
				}
				Invariant.Assert(this._cells != null, "Paragraph collection is empty");
				return this._cells;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001102 RID: 4354 RVA: 0x00142450 File Offset: 0x00141450
		internal override bool HasTextContent
		{
			get
			{
				if (this._cells == null)
				{
					ReadOnlyCollection<ParagraphResult> cellParagraphs = this.CellParagraphs;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x04000AEA RID: 2794
		private ReadOnlyCollection<ParagraphResult> _cells;

		// Token: 0x04000AEB RID: 2795
		private int _index;
	}
}
