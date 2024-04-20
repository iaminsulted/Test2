using System;

namespace System.Windows.Documents
{
	// Token: 0x0200062E RID: 1582
	internal interface ITextPointer
	{
		// Token: 0x06004E47 RID: 20039
		ITextPointer CreatePointer();

		// Token: 0x06004E48 RID: 20040
		StaticTextPointer CreateStaticPointer();

		// Token: 0x06004E49 RID: 20041
		ITextPointer CreatePointer(int offset);

		// Token: 0x06004E4A RID: 20042
		ITextPointer CreatePointer(LogicalDirection gravity);

		// Token: 0x06004E4B RID: 20043
		ITextPointer CreatePointer(int offset, LogicalDirection gravity);

		// Token: 0x06004E4C RID: 20044
		void SetLogicalDirection(LogicalDirection direction);

		// Token: 0x06004E4D RID: 20045
		int CompareTo(ITextPointer position);

		// Token: 0x06004E4E RID: 20046
		int CompareTo(StaticTextPointer position);

		// Token: 0x06004E4F RID: 20047
		bool HasEqualScope(ITextPointer position);

		// Token: 0x06004E50 RID: 20048
		TextPointerContext GetPointerContext(LogicalDirection direction);

		// Token: 0x06004E51 RID: 20049
		int GetOffsetToPosition(ITextPointer position);

		// Token: 0x06004E52 RID: 20050
		int GetTextRunLength(LogicalDirection direction);

		// Token: 0x06004E53 RID: 20051
		string GetTextInRun(LogicalDirection direction);

		// Token: 0x06004E54 RID: 20052
		int GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count);

		// Token: 0x06004E55 RID: 20053
		object GetAdjacentElement(LogicalDirection direction);

		// Token: 0x06004E56 RID: 20054
		void MoveToPosition(ITextPointer position);

		// Token: 0x06004E57 RID: 20055
		int MoveByOffset(int offset);

		// Token: 0x06004E58 RID: 20056
		bool MoveToNextContextPosition(LogicalDirection direction);

		// Token: 0x06004E59 RID: 20057
		ITextPointer GetNextContextPosition(LogicalDirection direction);

		// Token: 0x06004E5A RID: 20058
		bool MoveToInsertionPosition(LogicalDirection direction);

		// Token: 0x06004E5B RID: 20059
		ITextPointer GetInsertionPosition(LogicalDirection direction);

		// Token: 0x06004E5C RID: 20060
		ITextPointer GetFormatNormalizedPosition(LogicalDirection direction);

		// Token: 0x06004E5D RID: 20061
		bool MoveToNextInsertionPosition(LogicalDirection direction);

		// Token: 0x06004E5E RID: 20062
		ITextPointer GetNextInsertionPosition(LogicalDirection direction);

		// Token: 0x06004E5F RID: 20063
		void MoveToElementEdge(ElementEdge edge);

		// Token: 0x06004E60 RID: 20064
		int MoveToLineBoundary(int count);

		// Token: 0x06004E61 RID: 20065
		Rect GetCharacterRect(LogicalDirection direction);

		// Token: 0x06004E62 RID: 20066
		void Freeze();

		// Token: 0x06004E63 RID: 20067
		ITextPointer GetFrozenPointer(LogicalDirection logicalDirection);

		// Token: 0x06004E64 RID: 20068
		void InsertTextInRun(string textData);

		// Token: 0x06004E65 RID: 20069
		void DeleteContentToPosition(ITextPointer limit);

		// Token: 0x06004E66 RID: 20070
		Type GetElementType(LogicalDirection direction);

		// Token: 0x06004E67 RID: 20071
		object GetValue(DependencyProperty formattingProperty);

		// Token: 0x06004E68 RID: 20072
		object ReadLocalValue(DependencyProperty formattingProperty);

		// Token: 0x06004E69 RID: 20073
		LocalValueEnumerator GetLocalValueEnumerator();

		// Token: 0x06004E6A RID: 20074
		bool ValidateLayout();

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06004E6B RID: 20075
		ITextContainer TextContainer { get; }

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06004E6C RID: 20076
		bool HasValidLayout { get; }

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06004E6D RID: 20077
		bool IsAtCaretUnitBoundary { get; }

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06004E6E RID: 20078
		LogicalDirection LogicalDirection { get; }

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06004E6F RID: 20079
		Type ParentType { get; }

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06004E70 RID: 20080
		bool IsAtInsertionPosition { get; }

		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06004E71 RID: 20081
		bool IsFrozen { get; }

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06004E72 RID: 20082
		int Offset { get; }

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06004E73 RID: 20083
		int CharOffset { get; }
	}
}
