using System;

namespace System.Windows.Documents
{
	// Token: 0x020006C9 RID: 1737
	internal class TextTreeExtractElementUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06005A86 RID: 23174 RVA: 0x00282D00 File Offset: 0x00281D00
		internal TextTreeExtractElementUndoUnit(TextContainer tree, TextTreeTextElementNode elementNode) : base(tree, elementNode.GetSymbolOffset(tree.Generation))
		{
			this._symbolCount = elementNode.SymbolCount;
			this._type = elementNode.TextElement.GetType();
			this._localValues = TextTreeUndoUnit.GetPropertyRecordArray(elementNode.TextElement);
			this._resources = elementNode.TextElement.Resources;
			if (elementNode.TextElement is Table)
			{
				this._columns = TextTreeDeleteContentUndoUnit.SaveColumns((Table)elementNode.TextElement);
			}
		}

		// Token: 0x06005A87 RID: 23175 RVA: 0x00282D84 File Offset: 0x00281D84
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer start = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			TextPointer textPointer = new TextPointer(base.TextContainer, base.SymbolOffset + this._symbolCount - 2, LogicalDirection.Forward);
			TextElement textElement = (TextElement)Activator.CreateInstance(this._type);
			textElement.Reposition(start, textPointer);
			textElement.Resources = this._resources;
			textPointer.MoveToNextContextPosition(LogicalDirection.Backward);
			base.TextContainer.SetValues(textPointer, TextTreeUndoUnit.ArrayToLocalValueEnumerator(this._localValues));
			if (textElement is Table)
			{
				TextTreeDeleteContentUndoUnit.RestoreColumns((Table)textElement, this._columns);
			}
		}

		// Token: 0x04003053 RID: 12371
		private readonly int _symbolCount;

		// Token: 0x04003054 RID: 12372
		private readonly Type _type;

		// Token: 0x04003055 RID: 12373
		private readonly PropertyRecord[] _localValues;

		// Token: 0x04003056 RID: 12374
		private readonly ResourceDictionary _resources;

		// Token: 0x04003057 RID: 12375
		private readonly TableColumn[] _columns;
	}
}
