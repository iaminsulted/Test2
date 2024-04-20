using System;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020005E2 RID: 1506
	internal class ColumnResizeUndoUnit : ParentUndoUnit
	{
		// Token: 0x060048C0 RID: 18624 RVA: 0x0022DC58 File Offset: 0x0022CC58
		internal ColumnResizeUndoUnit(TextPointer textPointerTable, int columnIndex, double[] columnWidths, double resizeAmount) : base("ColumnResize")
		{
			this._textContainer = textPointerTable.TextContainer;
			this._cpTable = this._textContainer.Start.GetOffsetToPosition(textPointerTable);
			this._columnWidths = columnWidths;
			this._columnIndex = columnIndex;
			this._resizeAmount = resizeAmount;
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x0022DCAC File Offset: 0x0022CCAC
		public override void Do()
		{
			UndoManager undoManager = base.TopContainer as UndoManager;
			IParentUndoUnit parentUndoUnit = null;
			TextPointer textPointer = new TextPointer(this._textContainer.Start, this._cpTable, LogicalDirection.Forward);
			Table table = (Table)textPointer.Parent;
			this._columnWidths[this._columnIndex] -= this._resizeAmount;
			if (this._columnIndex < table.ColumnCount - 1)
			{
				this._columnWidths[this._columnIndex + 1] += this._resizeAmount;
			}
			if (undoManager != null && undoManager.IsEnabled)
			{
				parentUndoUnit = new ColumnResizeUndoUnit(textPointer, this._columnIndex, this._columnWidths, -this._resizeAmount);
				undoManager.Open(parentUndoUnit);
			}
			TextRangeEditTables.EnsureTableColumnsAreFixedSize(table, this._columnWidths);
			if (parentUndoUnit != null)
			{
				undoManager.Close(parentUndoUnit, UndoCloseAction.Commit);
			}
		}

		// Token: 0x04002639 RID: 9785
		private TextContainer _textContainer;

		// Token: 0x0400263A RID: 9786
		private double[] _columnWidths;

		// Token: 0x0400263B RID: 9787
		private int _cpTable;

		// Token: 0x0400263C RID: 9788
		private int _columnIndex;

		// Token: 0x0400263D RID: 9789
		private double _resizeAmount;
	}
}
