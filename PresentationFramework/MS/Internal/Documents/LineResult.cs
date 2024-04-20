using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020001D3 RID: 467
	internal abstract class LineResult
	{
		// Token: 0x0600106C RID: 4204
		internal abstract ITextPointer GetTextPositionFromDistance(double distance);

		// Token: 0x0600106D RID: 4205
		internal abstract bool IsAtCaretUnitBoundary(ITextPointer position);

		// Token: 0x0600106E RID: 4206
		internal abstract ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction);

		// Token: 0x0600106F RID: 4207
		internal abstract ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position);

		// Token: 0x06001070 RID: 4208
		internal abstract ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end);

		// Token: 0x06001071 RID: 4209
		internal abstract ITextPointer GetContentEndPosition();

		// Token: 0x06001072 RID: 4210
		internal abstract ITextPointer GetEllipsesPosition();

		// Token: 0x06001073 RID: 4211
		internal abstract int GetContentEndPositionCP();

		// Token: 0x06001074 RID: 4212
		internal abstract int GetEllipsesPositionCP();

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06001075 RID: 4213
		internal abstract ITextPointer StartPosition { get; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001076 RID: 4214
		internal abstract ITextPointer EndPosition { get; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001077 RID: 4215
		internal abstract int StartPositionCP { get; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001078 RID: 4216
		internal abstract int EndPositionCP { get; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001079 RID: 4217
		internal abstract Rect LayoutBox { get; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x0600107A RID: 4218
		internal abstract double Baseline { get; }
	}
}
