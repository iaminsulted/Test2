using System;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020006B4 RID: 1716
	internal class TextParentUndoUnit : ParentUndoUnit
	{
		// Token: 0x06005732 RID: 22322 RVA: 0x0026DA84 File Offset: 0x0026CA84
		internal TextParentUndoUnit(ITextSelection selection) : this(selection, selection.AnchorPosition, selection.MovingPosition)
		{
		}

		// Token: 0x06005733 RID: 22323 RVA: 0x0026DA9C File Offset: 0x0026CA9C
		internal TextParentUndoUnit(ITextSelection selection, ITextPointer anchorPosition, ITextPointer movingPosition) : base(string.Empty)
		{
			this._selection = selection;
			this._undoAnchorPositionOffset = anchorPosition.Offset;
			this._undoAnchorPositionDirection = anchorPosition.LogicalDirection;
			this._undoMovingPositionOffset = movingPosition.Offset;
			this._undoMovingPositionDirection = movingPosition.LogicalDirection;
			this._redoAnchorPositionOffset = 0;
			this._redoMovingPositionOffset = 0;
		}

		// Token: 0x06005734 RID: 22324 RVA: 0x0026DAFC File Offset: 0x0026CAFC
		protected TextParentUndoUnit(TextParentUndoUnit undoUnit) : base(string.Empty)
		{
			this._selection = undoUnit._selection;
			this._undoAnchorPositionOffset = undoUnit._redoAnchorPositionOffset;
			this._undoAnchorPositionDirection = undoUnit._redoAnchorPositionDirection;
			this._undoMovingPositionOffset = undoUnit._redoMovingPositionOffset;
			this._undoMovingPositionDirection = undoUnit._redoMovingPositionDirection;
			this._redoAnchorPositionOffset = 0;
			this._redoMovingPositionOffset = 0;
		}

		// Token: 0x06005735 RID: 22325 RVA: 0x0026DB60 File Offset: 0x0026CB60
		public override void Do()
		{
			base.Do();
			ITextContainer textContainer = this._selection.Start.TextContainer;
			ITextPointer position = textContainer.CreatePointerAtOffset(this._undoAnchorPositionOffset, this._undoAnchorPositionDirection);
			ITextPointer position2 = textContainer.CreatePointerAtOffset(this._undoMovingPositionOffset, this._undoMovingPositionDirection);
			this._selection.Select(position, position2);
			this._redoUnit.RecordRedoSelectionState();
		}

		// Token: 0x06005736 RID: 22326 RVA: 0x0026DBC0 File Offset: 0x0026CBC0
		protected override IParentUndoUnit CreateParentUndoUnitForSelf()
		{
			this._redoUnit = this.CreateRedoUnit();
			return this._redoUnit;
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x0026DBD4 File Offset: 0x0026CBD4
		protected virtual TextParentUndoUnit CreateRedoUnit()
		{
			return new TextParentUndoUnit(this);
		}

		// Token: 0x06005738 RID: 22328 RVA: 0x0026DBDC File Offset: 0x0026CBDC
		protected void MergeRedoSelectionState(TextParentUndoUnit undoUnit)
		{
			this._redoAnchorPositionOffset = undoUnit._redoAnchorPositionOffset;
			this._redoAnchorPositionDirection = undoUnit._redoAnchorPositionDirection;
			this._redoMovingPositionOffset = undoUnit._redoMovingPositionOffset;
			this._redoMovingPositionDirection = undoUnit._redoMovingPositionDirection;
		}

		// Token: 0x06005739 RID: 22329 RVA: 0x0026DC0E File Offset: 0x0026CC0E
		internal void RecordRedoSelectionState()
		{
			this.RecordRedoSelectionState(this._selection.AnchorPosition, this._selection.MovingPosition);
		}

		// Token: 0x0600573A RID: 22330 RVA: 0x0026DC2C File Offset: 0x0026CC2C
		internal void RecordRedoSelectionState(ITextPointer anchorPosition, ITextPointer movingPosition)
		{
			this._redoAnchorPositionOffset = anchorPosition.Offset;
			this._redoAnchorPositionDirection = anchorPosition.LogicalDirection;
			this._redoMovingPositionOffset = movingPosition.Offset;
			this._redoMovingPositionDirection = movingPosition.LogicalDirection;
		}

		// Token: 0x04002FC9 RID: 12233
		private readonly ITextSelection _selection;

		// Token: 0x04002FCA RID: 12234
		private readonly int _undoAnchorPositionOffset;

		// Token: 0x04002FCB RID: 12235
		private readonly LogicalDirection _undoAnchorPositionDirection;

		// Token: 0x04002FCC RID: 12236
		private readonly int _undoMovingPositionOffset;

		// Token: 0x04002FCD RID: 12237
		private readonly LogicalDirection _undoMovingPositionDirection;

		// Token: 0x04002FCE RID: 12238
		private int _redoAnchorPositionOffset;

		// Token: 0x04002FCF RID: 12239
		private LogicalDirection _redoAnchorPositionDirection;

		// Token: 0x04002FD0 RID: 12240
		private int _redoMovingPositionOffset;

		// Token: 0x04002FD1 RID: 12241
		private LogicalDirection _redoMovingPositionDirection;

		// Token: 0x04002FD2 RID: 12242
		private TextParentUndoUnit _redoUnit;
	}
}
