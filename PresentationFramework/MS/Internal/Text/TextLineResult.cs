using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;

namespace MS.Internal.Text
{
	// Token: 0x02000327 RID: 807
	internal sealed class TextLineResult : LineResult
	{
		// Token: 0x06001E11 RID: 7697 RVA: 0x0016F1C4 File Offset: 0x0016E1C4
		internal override ITextPointer GetTextPositionFromDistance(double distance)
		{
			return this._owner.GetTextPositionFromDistance(this._dcp, distance, this._layoutBox.Top, this._index);
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x00105F35 File Offset: 0x00104F35
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			return false;
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x00109403 File Offset: 0x00108403
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x00109403 File Offset: 0x00108403
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			return null;
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x00109403 File Offset: 0x00108403
		internal override ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			return null;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x0016F1F7 File Offset: 0x0016E1F7
		internal override ITextPointer GetContentEndPosition()
		{
			this.EnsureComplexData();
			return this._owner.TextContainer.CreatePointerAtOffset(this._dcp + this._cchContent, LogicalDirection.Backward);
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x0016F21D File Offset: 0x0016E21D
		internal override ITextPointer GetEllipsesPosition()
		{
			this.EnsureComplexData();
			if (this._cchEllipses != 0)
			{
				return this._owner.TextContainer.CreatePointerAtOffset(this._dcp + this._cch - this._cchEllipses, LogicalDirection.Forward);
			}
			return null;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0016F254 File Offset: 0x0016E254
		internal override int GetContentEndPositionCP()
		{
			this.EnsureComplexData();
			return this._dcp + this._cchContent;
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x0016F269 File Offset: 0x0016E269
		internal override int GetEllipsesPositionCP()
		{
			this.EnsureComplexData();
			return this._dcp + this._cch - this._cchEllipses;
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001E1A RID: 7706 RVA: 0x0016F285 File Offset: 0x0016E285
		internal override ITextPointer StartPosition
		{
			get
			{
				if (this._startPosition == null)
				{
					this._startPosition = this._owner.TextContainer.CreatePointerAtOffset(this._dcp, LogicalDirection.Forward);
				}
				return this._startPosition;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0016F2B2 File Offset: 0x0016E2B2
		internal override ITextPointer EndPosition
		{
			get
			{
				if (this._endPosition == null)
				{
					this._endPosition = this._owner.TextContainer.CreatePointerAtOffset(this._dcp + this._cch, LogicalDirection.Backward);
				}
				return this._endPosition;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001E1C RID: 7708 RVA: 0x0016F2E6 File Offset: 0x0016E2E6
		internal override int StartPositionCP
		{
			get
			{
				return this._dcp;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001E1D RID: 7709 RVA: 0x0016F2EE File Offset: 0x0016E2EE
		internal override int EndPositionCP
		{
			get
			{
				return this._dcp + this._cch;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001E1E RID: 7710 RVA: 0x0016F2FD File Offset: 0x0016E2FD
		internal override Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x0016F305 File Offset: 0x0016E305
		internal override double Baseline
		{
			get
			{
				return this._baseline;
			}
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0016F310 File Offset: 0x0016E310
		internal TextLineResult(TextBlock owner, int dcp, int cch, Rect layoutBox, double baseline, int index)
		{
			this._owner = owner;
			this._dcp = dcp;
			this._cch = cch;
			this._layoutBox = layoutBox;
			this._baseline = baseline;
			this._index = index;
			this._cchContent = (this._cchEllipses = -1);
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x0016F360 File Offset: 0x0016E360
		private void EnsureComplexData()
		{
			if (this._cchContent == -1)
			{
				this._owner.GetLineDetails(this._dcp, this._index, this._layoutBox.Top, out this._cchContent, out this._cchEllipses);
			}
		}

		// Token: 0x04000EEA RID: 3818
		private readonly TextBlock _owner;

		// Token: 0x04000EEB RID: 3819
		private readonly int _dcp;

		// Token: 0x04000EEC RID: 3820
		private readonly int _cch;

		// Token: 0x04000EED RID: 3821
		private readonly Rect _layoutBox;

		// Token: 0x04000EEE RID: 3822
		private int _index;

		// Token: 0x04000EEF RID: 3823
		private readonly double _baseline;

		// Token: 0x04000EF0 RID: 3824
		private ITextPointer _startPosition;

		// Token: 0x04000EF1 RID: 3825
		private ITextPointer _endPosition;

		// Token: 0x04000EF2 RID: 3826
		private int _cchContent;

		// Token: 0x04000EF3 RID: 3827
		private int _cchEllipses;
	}
}
