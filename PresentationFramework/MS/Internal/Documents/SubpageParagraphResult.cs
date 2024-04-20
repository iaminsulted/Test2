using System;
using System.Collections.ObjectModel;
using System.Windows;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020001E1 RID: 481
	internal sealed class SubpageParagraphResult : ParagraphResult
	{
		// Token: 0x06001103 RID: 4355 RVA: 0x001420CB File Offset: 0x001410CB
		internal SubpageParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x00142467 File Offset: 0x00141467
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = ((SubpageParaClient)this._paraClient).GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Columns collection is null");
				}
				return this._columns;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x001424A6 File Offset: 0x001414A6
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

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x001424BD File Offset: 0x001414BD
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				if (this._floatingElements == null)
				{
					this._floatingElements = ((SubpageParaClient)this._paraClient).FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x001424F8 File Offset: 0x001414F8
		internal Vector ContentOffset
		{
			get
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this._paraClient.Paragraph.Element, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				return new Vector(base.LayoutBox.X + TextDpi.FromTextDpi(mbpInfo.BPLeft), base.LayoutBox.Y + TextDpi.FromTextDpi(mbpInfo.BPTop));
			}
		}

		// Token: 0x04000AEC RID: 2796
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x04000AED RID: 2797
		private ReadOnlyCollection<ParagraphResult> _floatingElements;
	}
}
