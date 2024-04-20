using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020001DE RID: 478
	internal sealed class TextParagraphResult : ParagraphResult
	{
		// Token: 0x060010E8 RID: 4328 RVA: 0x001420CB File Offset: 0x001410CB
		internal TextParagraphResult(TextParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0014213F File Offset: 0x0014113F
		internal Rect GetRectangleFromTextPosition(ITextPointer position)
		{
			return ((TextParaClient)this._paraClient).GetRectangleFromTextPosition(position);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00142152 File Offset: 0x00141152
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, double paragraphTopSpace, Rect visibleRect)
		{
			return ((TextParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, paragraphTopSpace, visibleRect);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00142169 File Offset: 0x00141169
		internal bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			return ((TextParaClient)this._paraClient).IsAtCaretUnitBoundary(position);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0014217C File Offset: 0x0014117C
		internal ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			return ((TextParaClient)this._paraClient).GetNextCaretUnitPosition(position, direction);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00142190 File Offset: 0x00141190
		internal ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			return ((TextParaClient)this._paraClient).GetBackspaceCaretUnitPosition(position);
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x001421A3 File Offset: 0x001411A3
		internal void GetGlyphRuns(List<GlyphRun> glyphRuns, ITextPointer start, ITextPointer end)
		{
			((TextParaClient)this._paraClient).GetGlyphRuns(glyphRuns, start, end);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x001421B8 File Offset: 0x001411B8
		internal override bool Contains(ITextPointer position, bool strict)
		{
			bool flag = base.Contains(position, strict);
			if (!flag && strict)
			{
				flag = (position.CompareTo(base.EndPosition) == 0);
			}
			return flag;
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x001421E7 File Offset: 0x001411E7
		internal ReadOnlyCollection<LineResult> Lines
		{
			get
			{
				if (this._lines == null)
				{
					this._lines = ((TextParaClient)this._paraClient).GetLineResults();
				}
				Invariant.Assert(this._lines != null, "Lines collection is null");
				return this._lines;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00142220 File Offset: 0x00141220
		internal ReadOnlyCollection<ParagraphResult> Floaters
		{
			get
			{
				if (this._floaters == null)
				{
					this._floaters = ((TextParaClient)this._paraClient).GetFloaters();
				}
				return this._floaters;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x00142246 File Offset: 0x00141246
		internal ReadOnlyCollection<ParagraphResult> Figures
		{
			get
			{
				if (this._figures == null)
				{
					this._figures = ((TextParaClient)this._paraClient).GetFigures();
				}
				return this._figures;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0014226C File Offset: 0x0014126C
		internal override bool HasTextContent
		{
			get
			{
				return this.Lines.Count > 0 && !this.ContainsOnlyFloatingElements;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00142288 File Offset: 0x00141288
		private bool ContainsOnlyFloatingElements
		{
			get
			{
				bool result = false;
				TextParagraph textParagraph = this._paraClient.Paragraph as TextParagraph;
				Invariant.Assert(textParagraph != null);
				if (textParagraph.HasFiguresOrFloaters())
				{
					if (this.Lines.Count == 0)
					{
						result = true;
					}
					else if (this.Lines.Count == 1 && textParagraph.GetLastDcpAttachedObjectBeforeLine(0) + textParagraph.ParagraphStartCharacterPosition == textParagraph.ParagraphEndCharacterPosition)
					{
						result = true;
					}
				}
				return result;
			}
		}

		// Token: 0x04000AE6 RID: 2790
		private ReadOnlyCollection<LineResult> _lines;

		// Token: 0x04000AE7 RID: 2791
		private ReadOnlyCollection<ParagraphResult> _floaters;

		// Token: 0x04000AE8 RID: 2792
		private ReadOnlyCollection<ParagraphResult> _figures;
	}
}
