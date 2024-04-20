using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200014D RID: 333
	internal sealed class TextParaLineResult : LineResult
	{
		// Token: 0x06000ABD RID: 2749 RVA: 0x0012AFD6 File Offset: 0x00129FD6
		internal override ITextPointer GetTextPositionFromDistance(double distance)
		{
			return this._owner.GetTextPositionFromDistance(this._dcp, distance);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00105F35 File Offset: 0x00104F35
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			return false;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00109403 File Offset: 0x00108403
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00109403 File Offset: 0x00108403
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			return null;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00109403 File Offset: 0x00108403
		internal override ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			return null;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0012AFEA File Offset: 0x00129FEA
		internal override ITextPointer GetContentEndPosition()
		{
			this.EnsureComplexData();
			return this._owner.GetTextPosition(this._dcp + this._cchContent, LogicalDirection.Backward);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0012B00B File Offset: 0x0012A00B
		internal override ITextPointer GetEllipsesPosition()
		{
			this.EnsureComplexData();
			if (this._cchEllipses != 0)
			{
				return this._owner.GetTextPosition(this._dcp + this._cch - this._cchEllipses, LogicalDirection.Forward);
			}
			return null;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0012B03D File Offset: 0x0012A03D
		internal override int GetContentEndPositionCP()
		{
			this.EnsureComplexData();
			return this._dcp + this._cchContent;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0012B052 File Offset: 0x0012A052
		internal override int GetEllipsesPositionCP()
		{
			this.EnsureComplexData();
			return this._dcp + this._cch - this._cchEllipses;
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0012B06E File Offset: 0x0012A06E
		internal override ITextPointer StartPosition
		{
			get
			{
				if (this._startPosition == null)
				{
					this._startPosition = this._owner.GetTextPosition(this._dcp, LogicalDirection.Forward);
				}
				return this._startPosition;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0012B096 File Offset: 0x0012A096
		internal override ITextPointer EndPosition
		{
			get
			{
				if (this._endPosition == null)
				{
					this._endPosition = this._owner.GetTextPosition(this._dcp + this._cch, LogicalDirection.Backward);
				}
				return this._endPosition;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0012B0C5 File Offset: 0x0012A0C5
		internal override int StartPositionCP
		{
			get
			{
				return this._dcp + this._owner.Paragraph.ParagraphStartCharacterPosition;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0012B0DE File Offset: 0x0012A0DE
		internal override int EndPositionCP
		{
			get
			{
				return this._dcp + this._cch + this._owner.Paragraph.ParagraphStartCharacterPosition;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0012B0FE File Offset: 0x0012A0FE
		internal override Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0012B106 File Offset: 0x0012A106
		internal override double Baseline
		{
			get
			{
				return this._baseline;
			}
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0012B110 File Offset: 0x0012A110
		internal TextParaLineResult(TextParaClient owner, int dcp, int cch, Rect layoutBox, double baseline)
		{
			this._owner = owner;
			this._dcp = dcp;
			this._cch = cch;
			this._layoutBox = layoutBox;
			this._baseline = baseline;
			this._cchContent = (this._cchEllipses = -1);
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x0012B158 File Offset: 0x0012A158
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x0012B167 File Offset: 0x0012A167
		internal int DcpLast
		{
			get
			{
				return this._dcp + this._cch;
			}
			set
			{
				this._cch = value - this._dcp;
			}
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0012B177 File Offset: 0x0012A177
		private void EnsureComplexData()
		{
			if (this._cchContent == -1)
			{
				this._owner.GetLineDetails(this._dcp, out this._cchContent, out this._cchEllipses);
			}
		}

		// Token: 0x0400081F RID: 2079
		private readonly TextParaClient _owner;

		// Token: 0x04000820 RID: 2080
		private int _dcp;

		// Token: 0x04000821 RID: 2081
		private int _cch;

		// Token: 0x04000822 RID: 2082
		private readonly Rect _layoutBox;

		// Token: 0x04000823 RID: 2083
		private readonly double _baseline;

		// Token: 0x04000824 RID: 2084
		private ITextPointer _startPosition;

		// Token: 0x04000825 RID: 2085
		private ITextPointer _endPosition;

		// Token: 0x04000826 RID: 2086
		private int _cchContent;

		// Token: 0x04000827 RID: 2087
		private int _cchEllipses;
	}
}
