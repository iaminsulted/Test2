using System;

namespace System.Windows.Documents
{
	// Token: 0x02000630 RID: 1584
	internal interface ITextSelection : ITextRange
	{
		// Token: 0x06004E9F RID: 20127
		void SetCaretToPosition(ITextPointer caretPosition, LogicalDirection direction, bool allowStopAtLineEnd, bool allowStopNearSpace);

		// Token: 0x06004EA0 RID: 20128
		void ExtendToPosition(ITextPointer textPosition);

		// Token: 0x06004EA1 RID: 20129
		bool ExtendToNextInsertionPosition(LogicalDirection direction);

		// Token: 0x06004EA2 RID: 20130
		bool Contains(Point point);

		// Token: 0x06004EA3 RID: 20131
		void OnDetach();

		// Token: 0x06004EA4 RID: 20132
		void UpdateCaretAndHighlight();

		// Token: 0x06004EA5 RID: 20133
		void OnTextViewUpdated();

		// Token: 0x06004EA6 RID: 20134
		void DetachFromVisualTree();

		// Token: 0x06004EA7 RID: 20135
		void RefreshCaret();

		// Token: 0x06004EA8 RID: 20136
		void OnInterimSelectionChanged(bool interimSelection);

		// Token: 0x06004EA9 RID: 20137
		void SetSelectionByMouse(ITextPointer cursorPosition, Point cursorMousePoint);

		// Token: 0x06004EAA RID: 20138
		void ExtendSelectionByMouse(ITextPointer cursorPosition, bool forceWordSelection, bool forceParagraphSelection);

		// Token: 0x06004EAB RID: 20139
		bool ExtendToNextTableRow(LogicalDirection direction);

		// Token: 0x06004EAC RID: 20140
		void OnCaretNavigation();

		// Token: 0x06004EAD RID: 20141
		void ValidateLayout();

		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06004EAE RID: 20142
		TextEditor TextEditor { get; }

		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06004EAF RID: 20143
		ITextView TextView { get; }

		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06004EB0 RID: 20144
		bool IsInterimSelection { get; }

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06004EB1 RID: 20145
		ITextPointer AnchorPosition { get; }

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06004EB2 RID: 20146
		ITextPointer MovingPosition { get; }

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06004EB3 RID: 20147
		CaretElement CaretElement { get; }

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06004EB4 RID: 20148
		bool CoversEntireContent { get; }
	}
}
