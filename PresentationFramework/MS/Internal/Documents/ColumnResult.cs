using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020001B7 RID: 439
	internal sealed class ColumnResult
	{
		// Token: 0x06000E40 RID: 3648 RVA: 0x00138958 File Offset: 0x00137958
		internal ColumnResult(FlowDocumentPage page, ref PTS.FSTRACKDESCRIPTION trackDesc, Vector contentOffset)
		{
			this._page = page;
			this._columnHandle = trackDesc.pfstrack;
			this._layoutBox = new Rect(TextDpi.FromTextDpi(trackDesc.fsrc.u), TextDpi.FromTextDpi(trackDesc.fsrc.v), TextDpi.FromTextDpi(trackDesc.fsrc.du), TextDpi.FromTextDpi(trackDesc.fsrc.dv));
			this._layoutBox.X = this._layoutBox.X + contentOffset.X;
			this._layoutBox.Y = this._layoutBox.Y + contentOffset.Y;
			this._columnOffset = new Vector(TextDpi.FromTextDpi(trackDesc.fsrc.u), TextDpi.FromTextDpi(trackDesc.fsrc.v));
			this._hasTextContent = false;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00138A30 File Offset: 0x00137A30
		internal ColumnResult(BaseParaClient subpage, ref PTS.FSTRACKDESCRIPTION trackDesc, Vector contentOffset)
		{
			Invariant.Assert(subpage is SubpageParaClient || subpage is FigureParaClient || subpage is FloaterParaClient);
			this._subpage = subpage;
			this._columnHandle = trackDesc.pfstrack;
			this._layoutBox = new Rect(TextDpi.FromTextDpi(trackDesc.fsrc.u), TextDpi.FromTextDpi(trackDesc.fsrc.v), TextDpi.FromTextDpi(trackDesc.fsrc.du), TextDpi.FromTextDpi(trackDesc.fsrc.dv));
			this._layoutBox.X = this._layoutBox.X + contentOffset.X;
			this._layoutBox.Y = this._layoutBox.Y + contentOffset.Y;
			this._columnOffset = new Vector(TextDpi.FromTextDpi(trackDesc.fsrc.u), TextDpi.FromTextDpi(trackDesc.fsrc.v));
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00138B1F File Offset: 0x00137B1F
		internal bool Contains(ITextPointer position, bool strict)
		{
			this.EnsureTextContentRange();
			return this._contentRange.Contains(position, strict);
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x00138B34 File Offset: 0x00137B34
		internal ITextPointer StartPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.StartPosition;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x00138B47 File Offset: 0x00137B47
		internal ITextPointer EndPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.EndPosition;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x00138B5A File Offset: 0x00137B5A
		internal Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00138B64 File Offset: 0x00137B64
		internal ReadOnlyCollection<ParagraphResult> Paragraphs
		{
			get
			{
				if (this._paragraphs == null)
				{
					this._hasTextContent = false;
					if (this._page != null)
					{
						this._paragraphs = this._page.GetParagraphResultsFromColumn(this._columnHandle, this._columnOffset, out this._hasTextContent);
					}
					else if (this._subpage is FigureParaClient)
					{
						this._paragraphs = ((FigureParaClient)this._subpage).GetParagraphResultsFromColumn(this._columnHandle, this._columnOffset, out this._hasTextContent);
					}
					else if (this._subpage is FloaterParaClient)
					{
						this._paragraphs = ((FloaterParaClient)this._subpage).GetParagraphResultsFromColumn(this._columnHandle, this._columnOffset, out this._hasTextContent);
					}
					else if (this._subpage is SubpageParaClient)
					{
						this._paragraphs = ((SubpageParaClient)this._subpage).GetParagraphResultsFromColumn(this._columnHandle, this._columnOffset, out this._hasTextContent);
					}
					else
					{
						Invariant.Assert(false, "Expecting Subpage, Figure or Floater ParaClient");
					}
				}
				return this._paragraphs;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x00138C69 File Offset: 0x00137C69
		internal bool HasTextContent
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

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x00138C80 File Offset: 0x00137C80
		internal TextContentRange TextContentRange
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange;
			}
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00138C90 File Offset: 0x00137C90
		private void EnsureTextContentRange()
		{
			if (this._contentRange == null)
			{
				if (this._page != null)
				{
					this._contentRange = this._page.GetTextContentRangeFromColumn(this._columnHandle);
				}
				else if (this._subpage is FigureParaClient)
				{
					this._contentRange = ((FigureParaClient)this._subpage).GetTextContentRangeFromColumn(this._columnHandle);
				}
				else if (this._subpage is FloaterParaClient)
				{
					this._contentRange = ((FloaterParaClient)this._subpage).GetTextContentRangeFromColumn(this._columnHandle);
				}
				else if (this._subpage is SubpageParaClient)
				{
					this._contentRange = ((SubpageParaClient)this._subpage).GetTextContentRangeFromColumn(this._columnHandle);
				}
				else
				{
					Invariant.Assert(false, "Expecting Subpage, Figure or Floater ParaClient");
				}
				Invariant.Assert(this._contentRange != null);
			}
		}

		// Token: 0x04000A46 RID: 2630
		private readonly FlowDocumentPage _page;

		// Token: 0x04000A47 RID: 2631
		private readonly BaseParaClient _subpage;

		// Token: 0x04000A48 RID: 2632
		private readonly IntPtr _columnHandle;

		// Token: 0x04000A49 RID: 2633
		private readonly Rect _layoutBox;

		// Token: 0x04000A4A RID: 2634
		private readonly Vector _columnOffset;

		// Token: 0x04000A4B RID: 2635
		private TextContentRange _contentRange;

		// Token: 0x04000A4C RID: 2636
		private ReadOnlyCollection<ParagraphResult> _paragraphs;

		// Token: 0x04000A4D RID: 2637
		private bool _hasTextContent;
	}
}
