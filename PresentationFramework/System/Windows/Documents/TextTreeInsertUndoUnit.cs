using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006CC RID: 1740
	internal class TextTreeInsertUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06005AB0 RID: 23216 RVA: 0x00282F57 File Offset: 0x00281F57
		internal TextTreeInsertUndoUnit(TextContainer tree, int symbolOffset, int symbolCount) : base(tree, symbolOffset)
		{
			Invariant.Assert(symbolCount > 0, "Creating no-op insert undo unit!");
			this._symbolCount = symbolCount;
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x00282F78 File Offset: 0x00281F78
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer startPosition = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			TextPointer endPosition = new TextPointer(base.TextContainer, base.SymbolOffset + this._symbolCount, LogicalDirection.Forward);
			base.TextContainer.DeleteContentInternal(startPosition, endPosition);
		}

		// Token: 0x0400305F RID: 12383
		private readonly int _symbolCount;
	}
}
