using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006CF RID: 1743
	internal class TextTreePropertyUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06005AE8 RID: 23272 RVA: 0x00283335 File Offset: 0x00282335
		internal TextTreePropertyUndoUnit(TextContainer tree, int symbolOffset, PropertyRecord propertyRecord) : base(tree, symbolOffset)
		{
			this._propertyRecord = propertyRecord;
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x00283348 File Offset: 0x00282348
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer textPointer = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			Invariant.Assert(textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart, "TextTree undo unit out of sync with TextTree.");
			if (this._propertyRecord.Value != DependencyProperty.UnsetValue)
			{
				base.TextContainer.SetValue(textPointer, this._propertyRecord.Property, this._propertyRecord.Value);
				return;
			}
			textPointer.Parent.ClearValue(this._propertyRecord.Property);
		}

		// Token: 0x04003069 RID: 12393
		private readonly PropertyRecord _propertyRecord;
	}
}
