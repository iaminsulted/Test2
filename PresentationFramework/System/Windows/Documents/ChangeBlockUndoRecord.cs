using System;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020005E0 RID: 1504
	internal class ChangeBlockUndoRecord
	{
		// Token: 0x060048AE RID: 18606 RVA: 0x0022D998 File Offset: 0x0022C998
		internal ChangeBlockUndoRecord(ITextContainer textContainer, string actionDescription)
		{
			if (textContainer.UndoManager != null)
			{
				this._undoManager = textContainer.UndoManager;
				if (this._undoManager.IsEnabled)
				{
					if (textContainer.TextView != null)
					{
						if (this._undoManager.OpenedUnit == null)
						{
							if (textContainer.TextSelection != null)
							{
								this._parentUndoUnit = new TextParentUndoUnit(textContainer.TextSelection);
							}
							else
							{
								this._parentUndoUnit = new ParentUndoUnit(actionDescription);
							}
							this._undoManager.Open(this._parentUndoUnit);
							return;
						}
					}
					else
					{
						this._undoManager.Clear();
					}
				}
			}
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x0022DA28 File Offset: 0x0022CA28
		internal void OnEndChange()
		{
			if (this._parentUndoUnit != null)
			{
				IParentUndoUnit openedUnit;
				if (this._parentUndoUnit.Container is UndoManager)
				{
					openedUnit = ((UndoManager)this._parentUndoUnit.Container).OpenedUnit;
				}
				else
				{
					openedUnit = ((IParentUndoUnit)this._parentUndoUnit.Container).OpenedUnit;
				}
				if (openedUnit == this._parentUndoUnit)
				{
					if (this._parentUndoUnit is TextParentUndoUnit)
					{
						((TextParentUndoUnit)this._parentUndoUnit).RecordRedoSelectionState();
					}
					Invariant.Assert(this._undoManager != null);
					this._undoManager.Close(this._parentUndoUnit, (this._parentUndoUnit.LastUnit != null) ? UndoCloseAction.Commit : UndoCloseAction.Discard);
				}
			}
		}

		// Token: 0x04002630 RID: 9776
		private readonly UndoManager _undoManager;

		// Token: 0x04002631 RID: 9777
		private readonly IParentUndoUnit _parentUndoUnit;
	}
}
