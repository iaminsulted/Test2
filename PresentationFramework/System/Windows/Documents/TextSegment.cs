using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006BF RID: 1727
	internal struct TextSegment
	{
		// Token: 0x06005976 RID: 22902 RVA: 0x0027C25C File Offset: 0x0027B25C
		internal TextSegment(ITextPointer startPosition, ITextPointer endPosition)
		{
			this = new TextSegment(startPosition, endPosition, false);
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x0027C268 File Offset: 0x0027B268
		internal TextSegment(ITextPointer startPosition, ITextPointer endPosition, bool preserveLogicalDirection)
		{
			ValidationHelper.VerifyPositionPair(startPosition, endPosition);
			if (startPosition.CompareTo(endPosition) == 0)
			{
				this._start = startPosition.GetFrozenPointer(startPosition.LogicalDirection);
				this._end = this._start;
				return;
			}
			Invariant.Assert(startPosition.CompareTo(endPosition) < 0);
			this._start = startPosition.GetFrozenPointer(preserveLogicalDirection ? startPosition.LogicalDirection : LogicalDirection.Backward);
			this._end = endPosition.GetFrozenPointer(preserveLogicalDirection ? endPosition.LogicalDirection : LogicalDirection.Forward);
		}

		// Token: 0x06005978 RID: 22904 RVA: 0x0027C2E3 File Offset: 0x0027B2E3
		internal bool Contains(ITextPointer position)
		{
			return !this.IsNull && this._start.CompareTo(position) <= 0 && position.CompareTo(this._end) <= 0;
		}

		// Token: 0x170014BB RID: 5307
		// (get) Token: 0x06005979 RID: 22905 RVA: 0x0027C310 File Offset: 0x0027B310
		internal ITextPointer Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x170014BC RID: 5308
		// (get) Token: 0x0600597A RID: 22906 RVA: 0x0027C318 File Offset: 0x0027B318
		internal ITextPointer End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x170014BD RID: 5309
		// (get) Token: 0x0600597B RID: 22907 RVA: 0x0027C320 File Offset: 0x0027B320
		internal bool IsNull
		{
			get
			{
				return this._start == null || this._end == null;
			}
		}

		// Token: 0x04003002 RID: 12290
		internal static readonly TextSegment Null;

		// Token: 0x04003003 RID: 12291
		private readonly ITextPointer _start;

		// Token: 0x04003004 RID: 12292
		private readonly ITextPointer _end;
	}
}
