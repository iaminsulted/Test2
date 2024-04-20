using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006CB RID: 1739
	internal class TextTreeInsertElementUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06005AAE RID: 23214 RVA: 0x00282ECD File Offset: 0x00281ECD
		internal TextTreeInsertElementUndoUnit(TextContainer tree, int symbolOffset, bool deep) : base(tree, symbolOffset)
		{
			this._deep = deep;
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x00282EE0 File Offset: 0x00281EE0
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer textPointer = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			Invariant.Assert(textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart, "TextTree undo unit out of sync with TextTree.");
			TextElement adjacentElementFromOuterPosition = textPointer.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
			if (this._deep)
			{
				TextPointer endPosition = new TextPointer(base.TextContainer, adjacentElementFromOuterPosition.TextElementNode, ElementEdge.AfterEnd);
				base.TextContainer.DeleteContentInternal(textPointer, endPosition);
				return;
			}
			base.TextContainer.ExtractElementInternal(adjacentElementFromOuterPosition);
		}

		// Token: 0x0400305E RID: 12382
		private readonly bool _deep;
	}
}
