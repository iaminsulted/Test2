using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000631 RID: 1585
	internal interface ITextView
	{
		// Token: 0x06004EB5 RID: 20149
		ITextPointer GetTextPositionFromPoint(Point point, bool snapToText);

		// Token: 0x06004EB6 RID: 20150
		Rect GetRectangleFromTextPosition(ITextPointer position);

		// Token: 0x06004EB7 RID: 20151
		Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform);

		// Token: 0x06004EB8 RID: 20152
		Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition);

		// Token: 0x06004EB9 RID: 20153
		ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved);

		// Token: 0x06004EBA RID: 20154
		ITextPointer GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved);

		// Token: 0x06004EBB RID: 20155
		bool IsAtCaretUnitBoundary(ITextPointer position);

		// Token: 0x06004EBC RID: 20156
		ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction);

		// Token: 0x06004EBD RID: 20157
		ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position);

		// Token: 0x06004EBE RID: 20158
		TextSegment GetLineRange(ITextPointer position);

		// Token: 0x06004EBF RID: 20159
		ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end);

		// Token: 0x06004EC0 RID: 20160
		bool Contains(ITextPointer position);

		// Token: 0x06004EC1 RID: 20161
		void BringPositionIntoViewAsync(ITextPointer position, object userState);

		// Token: 0x06004EC2 RID: 20162
		void BringPointIntoViewAsync(Point point, object userState);

		// Token: 0x06004EC3 RID: 20163
		void BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState);

		// Token: 0x06004EC4 RID: 20164
		void BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState);

		// Token: 0x06004EC5 RID: 20165
		void CancelAsync(object userState);

		// Token: 0x06004EC6 RID: 20166
		bool Validate();

		// Token: 0x06004EC7 RID: 20167
		bool Validate(Point point);

		// Token: 0x06004EC8 RID: 20168
		bool Validate(ITextPointer position);

		// Token: 0x06004EC9 RID: 20169
		void ThrottleBackgroundTasksForUserInput();

		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06004ECA RID: 20170
		UIElement RenderScope { get; }

		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06004ECB RID: 20171
		ITextContainer TextContainer { get; }

		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x06004ECC RID: 20172
		bool IsValid { get; }

		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x06004ECD RID: 20173
		bool RendersOwnSelection { get; }

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x06004ECE RID: 20174
		ReadOnlyCollection<TextSegment> TextSegments { get; }

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06004ECF RID: 20175
		// (remove) Token: 0x06004ED0 RID: 20176
		event BringPositionIntoViewCompletedEventHandler BringPositionIntoViewCompleted;

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06004ED1 RID: 20177
		// (remove) Token: 0x06004ED2 RID: 20178
		event BringPointIntoViewCompletedEventHandler BringPointIntoViewCompleted;

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06004ED3 RID: 20179
		// (remove) Token: 0x06004ED4 RID: 20180
		event BringLineIntoViewCompletedEventHandler BringLineIntoViewCompleted;

		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x06004ED5 RID: 20181
		// (remove) Token: 0x06004ED6 RID: 20182
		event BringPageIntoViewCompletedEventHandler BringPageIntoViewCompleted;

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x06004ED7 RID: 20183
		// (remove) Token: 0x06004ED8 RID: 20184
		event EventHandler Updated;
	}
}
