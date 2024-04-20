using System;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x0200062D RID: 1581
	internal interface ITextContainer
	{
		// Token: 0x06004E22 RID: 20002
		void BeginChange();

		// Token: 0x06004E23 RID: 20003
		void BeginChangeNoUndo();

		// Token: 0x06004E24 RID: 20004
		void EndChange();

		// Token: 0x06004E25 RID: 20005
		void EndChange(bool skipEvents);

		// Token: 0x06004E26 RID: 20006
		ITextPointer CreatePointerAtOffset(int offset, LogicalDirection direction);

		// Token: 0x06004E27 RID: 20007
		ITextPointer CreatePointerAtCharOffset(int charOffset, LogicalDirection direction);

		// Token: 0x06004E28 RID: 20008
		ITextPointer CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction);

		// Token: 0x06004E29 RID: 20009
		StaticTextPointer CreateStaticPointerAtOffset(int offset);

		// Token: 0x06004E2A RID: 20010
		TextPointerContext GetPointerContext(StaticTextPointer pointer, LogicalDirection direction);

		// Token: 0x06004E2B RID: 20011
		int GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2);

		// Token: 0x06004E2C RID: 20012
		int GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count);

		// Token: 0x06004E2D RID: 20013
		object GetAdjacentElement(StaticTextPointer position, LogicalDirection direction);

		// Token: 0x06004E2E RID: 20014
		DependencyObject GetParent(StaticTextPointer position);

		// Token: 0x06004E2F RID: 20015
		StaticTextPointer CreatePointer(StaticTextPointer position, int offset);

		// Token: 0x06004E30 RID: 20016
		StaticTextPointer GetNextContextPosition(StaticTextPointer position, LogicalDirection direction);

		// Token: 0x06004E31 RID: 20017
		int CompareTo(StaticTextPointer position1, StaticTextPointer position2);

		// Token: 0x06004E32 RID: 20018
		int CompareTo(StaticTextPointer position1, ITextPointer position2);

		// Token: 0x06004E33 RID: 20019
		object GetValue(StaticTextPointer position, DependencyProperty formattingProperty);

		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x06004E34 RID: 20020
		bool IsReadOnly { get; }

		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x06004E35 RID: 20021
		ITextPointer Start { get; }

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x06004E36 RID: 20022
		ITextPointer End { get; }

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06004E37 RID: 20023
		DependencyObject Parent { get; }

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06004E38 RID: 20024
		Highlights Highlights { get; }

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06004E39 RID: 20025
		// (set) Token: 0x06004E3A RID: 20026
		ITextSelection TextSelection { get; set; }

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x06004E3B RID: 20027
		UndoManager UndoManager { get; }

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x06004E3C RID: 20028
		// (set) Token: 0x06004E3D RID: 20029
		ITextView TextView { get; set; }

		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06004E3E RID: 20030
		int SymbolCount { get; }

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x06004E3F RID: 20031
		int IMECharCount { get; }

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x06004E40 RID: 20032
		uint Generation { get; }

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06004E41 RID: 20033
		// (remove) Token: 0x06004E42 RID: 20034
		event EventHandler Changing;

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06004E43 RID: 20035
		// (remove) Token: 0x06004E44 RID: 20036
		event TextContainerChangeEventHandler Change;

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06004E45 RID: 20037
		// (remove) Token: 0x06004E46 RID: 20038
		event TextContainerChangedEventHandler Changed;
	}
}
