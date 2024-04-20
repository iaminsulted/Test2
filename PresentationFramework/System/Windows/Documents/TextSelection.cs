using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006C0 RID: 1728
	public sealed class TextSelection : TextRange, ITextSelection, ITextRange
	{
		// Token: 0x0600597D RID: 22909 RVA: 0x0027C338 File Offset: 0x0027B338
		internal TextSelection(TextEditor textEditor) : base(textEditor.TextContainer.Start, textEditor.TextContainer.Start)
		{
			Invariant.Assert(textEditor.UiScope != null);
			this._textEditor = textEditor;
			this.SetActivePositions(((ITextRange)this).Start, ((ITextRange)this).End);
			((ITextSelection)this).UpdateCaretAndHighlight();
		}

		// Token: 0x0600597E RID: 22910 RVA: 0x0027C390 File Offset: 0x0027B390
		void ITextRange.Select(ITextPointer anchorPosition, ITextPointer movingPosition)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.Select(this, anchorPosition, movingPosition);
				this.SetActivePositions(anchorPosition, movingPosition);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x0600597F RID: 22911 RVA: 0x0027C3CC File Offset: 0x0027B3CC
		void ITextRange.SelectWord(ITextPointer position)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.SelectWord(this, position);
				this.SetActivePositions(((ITextRange)this).Start, ((ITextRange)this).End);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06005980 RID: 22912 RVA: 0x0027C414 File Offset: 0x0027B414
		void ITextRange.SelectParagraph(ITextPointer position)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.SelectParagraph(this, position);
				this.SetActivePositions(position, ((ITextRange)this).End);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06005981 RID: 22913 RVA: 0x0027C458 File Offset: 0x0027B458
		void ITextRange.ApplyTypingHeuristics(bool overType)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.ApplyInitialTypingHeuristics(this);
				if (!base.IsEmpty && this._textEditor.AcceptsRichContent)
				{
					this.SpringloadCurrentFormatting();
				}
				TextRangeBase.ApplyFinalTypingHeuristics(this, overType);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06005982 RID: 22914 RVA: 0x0027C4AC File Offset: 0x0027B4AC
		object ITextRange.GetPropertyValue(DependencyProperty formattingProperty)
		{
			object result;
			if (base.IsEmpty && TextSchema.IsCharacterProperty(formattingProperty))
			{
				result = this.GetCurrentValue(formattingProperty);
			}
			else
			{
				result = TextRangeBase.GetPropertyValue(this, formattingProperty);
			}
			return result;
		}

		// Token: 0x170014BE RID: 5310
		// (get) Token: 0x06005983 RID: 22915 RVA: 0x00271584 File Offset: 0x00270584
		// (set) Token: 0x06005984 RID: 22916 RVA: 0x0027C4DC File Offset: 0x0027B4DC
		bool ITextRange._IsChanged
		{
			get
			{
				return base._IsChanged;
			}
			set
			{
				if (!base._IsChanged && value)
				{
					if (this.TextStore != null)
					{
						this.TextStore.OnSelectionChange();
					}
					if (this.ImmComposition != null)
					{
						this.ImmComposition.OnSelectionChange();
					}
				}
				base._IsChanged = value;
			}
		}

		// Token: 0x06005985 RID: 22917 RVA: 0x0027C518 File Offset: 0x0027B518
		void ITextRange.NotifyChanged(bool disableScroll, bool skipEvents)
		{
			if (this.TextStore != null)
			{
				this.TextStore.OnSelectionChanged();
			}
			if (this.ImmComposition != null)
			{
				this.ImmComposition.OnSelectionChanged();
			}
			if (!skipEvents)
			{
				TextRangeBase.NotifyChanged(this, disableScroll);
			}
			if (!disableScroll)
			{
				ITextPointer movingPosition = ((ITextSelection)this).MovingPosition;
				if (this.TextView != null && this.TextView.IsValid && !this.TextView.Contains(movingPosition))
				{
					movingPosition.ValidateLayout();
				}
			}
			this.UpdateCaretState(disableScroll ? CaretScrollMethod.None : CaretScrollMethod.Simple);
		}

		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x06005986 RID: 22918 RVA: 0x002714E8 File Offset: 0x002704E8
		// (set) Token: 0x06005987 RID: 22919 RVA: 0x0027C598 File Offset: 0x0027B598
		string ITextRange.Text
		{
			get
			{
				return TextRangeBase.GetText(this);
			}
			set
			{
				TextRangeBase.BeginChange(this);
				try
				{
					TextRangeBase.SetText(this, value);
					if (base.IsEmpty)
					{
						((ITextSelection)this).SetCaretToPosition(((ITextRange)this).End, LogicalDirection.Forward, false, false);
					}
					Invariant.Assert(!base.IsTableCellRange);
					this.SetActivePositions(((ITextRange)this).Start, ((ITextRange)this).End);
				}
				finally
				{
					TextRangeBase.EndChange(this);
				}
			}
		}

		// Token: 0x06005988 RID: 22920 RVA: 0x0027C604 File Offset: 0x0027B604
		void ITextSelection.UpdateCaretAndHighlight()
		{
			FrameworkElement uiScope = this.UiScope;
			FrameworkElement ownerElement = CaretElement.GetOwnerElement(uiScope);
			bool flag = false;
			bool isBlinkEnabled = false;
			bool flag2 = false;
			if (uiScope.IsEnabled && this.TextView != null)
			{
				if (uiScope.IsKeyboardFocused)
				{
					flag = true;
					isBlinkEnabled = true;
					flag2 = true;
				}
				else if (uiScope.IsFocused && ((TextSelection.IsRootElement(FocusManager.GetFocusScope(uiScope)) && this.IsFocusWithinRoot()) || this._textEditor.IsContextMenuOpen))
				{
					flag = true;
					isBlinkEnabled = false;
					flag2 = true;
				}
				else if (!base.IsEmpty && (bool)ownerElement.GetValue(TextBoxBase.IsInactiveSelectionHighlightEnabledProperty))
				{
					flag = true;
					isBlinkEnabled = false;
					flag2 = false;
				}
			}
			ownerElement.SetValue(TextBoxBase.IsSelectionActivePropertyKey, flag2);
			if (flag)
			{
				if (flag2)
				{
					this.SetThreadSelection();
				}
				this.EnsureCaret(isBlinkEnabled, flag2, CaretScrollMethod.None);
				this.Highlight();
				return;
			}
			this.ClearThreadSelection();
			this.DetachCaretFromVisualTree();
			this.Unhighlight();
		}

		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x06005989 RID: 22921 RVA: 0x0027C6DC File Offset: 0x0027B6DC
		ITextPointer ITextSelection.AnchorPosition
		{
			get
			{
				Invariant.Assert(base.IsEmpty || this._anchorPosition != null);
				Invariant.Assert(this._anchorPosition == null || this._anchorPosition.IsFrozen);
				if (!base.IsEmpty)
				{
					return this._anchorPosition;
				}
				return ((ITextRange)this).Start;
			}
		}

		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x0600598A RID: 22922 RVA: 0x0027C734 File Offset: 0x0027B734
		ITextPointer ITextSelection.MovingPosition
		{
			get
			{
				ITextPointer textPointer;
				if (base.IsEmpty)
				{
					textPointer = ((ITextRange)this).Start;
				}
				else
				{
					switch (this._movingPositionEdge)
					{
					case TextSelection.MovingEdge.Start:
						textPointer = ((ITextRange)this).Start;
						goto IL_92;
					case TextSelection.MovingEdge.StartInner:
						textPointer = ((ITextRange)this).TextSegments[0].End;
						goto IL_92;
					case TextSelection.MovingEdge.EndInner:
						textPointer = ((ITextRange)this).TextSegments[((ITextRange)this).TextSegments.Count - 1].Start;
						goto IL_92;
					case TextSelection.MovingEdge.End:
						textPointer = ((ITextRange)this).End;
						goto IL_92;
					}
					Invariant.Assert(false, "MovingEdge should never be None with non-empty TextSelection!");
					textPointer = null;
					IL_92:
					textPointer = textPointer.GetFrozenPointer(this._movingPositionDirection);
				}
				return textPointer;
			}
		}

		// Token: 0x0600598B RID: 22923 RVA: 0x0027C7E4 File Offset: 0x0027B7E4
		void ITextSelection.SetCaretToPosition(ITextPointer caretPosition, LogicalDirection direction, bool allowStopAtLineEnd, bool allowStopNearSpace)
		{
			caretPosition = caretPosition.CreatePointer(direction);
			caretPosition.MoveToInsertionPosition(direction);
			ITextPointer position = caretPosition.CreatePointer((direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward);
			if (!allowStopAtLineEnd && ((TextPointerBase.IsAtLineWrappingPosition(caretPosition, this.TextView) && TextPointerBase.IsAtLineWrappingPosition(position, this.TextView)) || TextPointerBase.IsNextToPlainLineBreak(caretPosition, LogicalDirection.Backward) || TextSchema.IsBreak(caretPosition.GetElementType(LogicalDirection.Backward))))
			{
				caretPosition.SetLogicalDirection(LogicalDirection.Forward);
			}
			else if ((caretPosition.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text || caretPosition.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text) && !allowStopNearSpace)
			{
				char[] array = new char[1];
				if (caretPosition.GetPointerContext(direction) == TextPointerContext.Text && caretPosition.GetTextInRun(direction, array, 0, 1) == 1 && char.IsWhiteSpace(array[0]))
				{
					LogicalDirection logicalDirection = (direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward;
					FlowDirection flowDirection = (FlowDirection)caretPosition.GetValue(FrameworkElement.FlowDirectionProperty);
					if (caretPosition.MoveToInsertionPosition(logicalDirection) && flowDirection == (FlowDirection)caretPosition.GetValue(FrameworkElement.FlowDirectionProperty) && (caretPosition.GetPointerContext(logicalDirection) != TextPointerContext.Text || caretPosition.GetTextInRun(logicalDirection, array, 0, 1) != 1 || !char.IsWhiteSpace(array[0])))
					{
						direction = logicalDirection;
						caretPosition.SetLogicalDirection(direction);
					}
				}
			}
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.Select(this, caretPosition, caretPosition);
				Invariant.Assert(((ITextRange)this).Start.LogicalDirection == caretPosition.LogicalDirection);
				Invariant.Assert(base.IsEmpty);
				this.SetActivePositions(null, null);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x0600598C RID: 22924 RVA: 0x0027C94C File Offset: 0x0027B94C
		void ITextSelection.ExtendToPosition(ITextPointer position)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				ITextPointer anchorPosition = ((ITextSelection)this).AnchorPosition;
				TextRangeBase.Select(this, anchorPosition, position);
				this.SetActivePositions(anchorPosition, position);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x0600598D RID: 22925 RVA: 0x0027C990 File Offset: 0x0027B990
		bool ITextSelection.ExtendToNextInsertionPosition(LogicalDirection direction)
		{
			bool result = false;
			TextRangeBase.BeginChange(this);
			try
			{
				ITextPointer anchorPosition = ((ITextSelection)this).AnchorPosition;
				ITextPointer movingPosition = ((ITextSelection)this).MovingPosition;
				ITextPointer textPointer;
				if (base.IsTableCellRange)
				{
					textPointer = TextRangeEditTables.GetNextTableCellRangeInsertionPosition(this, direction);
				}
				else if (movingPosition is TextPointer && TextPointerBase.IsAtRowEnd(movingPosition))
				{
					textPointer = TextRangeEditTables.GetNextRowEndMovingPosition(this, direction);
				}
				else if (movingPosition is TextPointer && TextRangeEditTables.MovingPositionCrossesCellBoundary(this))
				{
					textPointer = TextRangeEditTables.GetNextRowStartMovingPosition(this, direction);
				}
				else
				{
					textPointer = this.GetNextTextSegmentInsertionPosition(direction);
				}
				if (textPointer == null && direction == LogicalDirection.Forward && movingPosition.CompareTo(movingPosition.TextContainer.End) != 0)
				{
					textPointer = movingPosition.TextContainer.End;
				}
				if (textPointer != null)
				{
					result = true;
					TextRangeBase.Select(this, anchorPosition, textPointer);
					LogicalDirection logicalDirection = (anchorPosition.CompareTo(textPointer) <= 0) ? LogicalDirection.Backward : LogicalDirection.Forward;
					textPointer = textPointer.GetFrozenPointer(logicalDirection);
					this.SetActivePositions(anchorPosition, textPointer);
				}
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x0600598E RID: 22926 RVA: 0x0027CA70 File Offset: 0x0027BA70
		private ITextPointer GetNextTextSegmentInsertionPosition(LogicalDirection direction)
		{
			return ((ITextSelection)this).MovingPosition.GetNextInsertionPosition(direction);
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x0027CA80 File Offset: 0x0027BA80
		bool ITextSelection.Contains(Point point)
		{
			if (((ITextRange)this).IsEmpty)
			{
				return false;
			}
			if (this.TextView == null || !this.TextView.IsValid)
			{
				return false;
			}
			bool flag = false;
			ITextPointer textPointer = this.TextView.GetTextPositionFromPoint(point, false);
			if (textPointer != null && ((ITextRange)this).Contains(textPointer))
			{
				textPointer = textPointer.GetNextInsertionPosition(textPointer.LogicalDirection);
				if (textPointer != null && ((ITextRange)this).Contains(textPointer))
				{
					flag = true;
				}
			}
			if (!flag && this._caretElement != null && this._caretElement.SelectionGeometry != null && this._caretElement.SelectionGeometry.FillContains(point))
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06005990 RID: 22928 RVA: 0x0027CB18 File Offset: 0x0027BB18
		void ITextSelection.OnDetach()
		{
			((ITextSelection)this).UpdateCaretAndHighlight();
			if (this._highlightLayer != null && ((ITextRange)this).Start.TextContainer.Highlights.GetLayer(typeof(TextSelection)) == this._highlightLayer)
			{
				((ITextRange)this).Start.TextContainer.Highlights.RemoveLayer(this._highlightLayer);
			}
			this._highlightLayer = null;
			this._textEditor = null;
		}

		// Token: 0x06005991 RID: 22929 RVA: 0x0027CB88 File Offset: 0x0027BB88
		void ITextSelection.OnTextViewUpdated()
		{
			if (this.UiScope.IsKeyboardFocused || this.UiScope.IsFocused)
			{
				CaretElement caretElement = this._caretElement;
				if (caretElement != null)
				{
					caretElement.OnTextViewUpdated();
				}
			}
			if (this._pendingUpdateCaretStateCallback)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.UpdateCaretStateWorker), null);
			}
		}

		// Token: 0x06005992 RID: 22930 RVA: 0x0027CBE0 File Offset: 0x0027BBE0
		void ITextSelection.DetachFromVisualTree()
		{
			this.DetachCaretFromVisualTree();
		}

		// Token: 0x06005993 RID: 22931 RVA: 0x0027CBE8 File Offset: 0x0027BBE8
		void ITextSelection.RefreshCaret()
		{
			TextSelection.RefreshCaret(this._textEditor, this._textEditor.Selection);
		}

		// Token: 0x06005994 RID: 22932 RVA: 0x0027CC00 File Offset: 0x0027BC00
		void ITextSelection.OnInterimSelectionChanged(bool interimSelection)
		{
			this.UpdateCaretState(CaretScrollMethod.None);
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x0027CC0C File Offset: 0x0027BC0C
		void ITextSelection.SetSelectionByMouse(ITextPointer cursorPosition, Point cursorMousePoint)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
			{
				((ITextSelection)this).ExtendSelectionByMouse(cursorPosition, false, false);
				return;
			}
			this.MoveSelectionByMouse(cursorPosition, cursorMousePoint);
		}

		// Token: 0x06005996 RID: 22934 RVA: 0x0027CC38 File Offset: 0x0027BC38
		void ITextSelection.ExtendSelectionByMouse(ITextPointer cursorPosition, bool forceWordSelection, bool forceParagraphSelection)
		{
			if (forceParagraphSelection || (this._previousCursorPosition != null && cursorPosition.CompareTo(this._previousCursorPosition) == 0))
			{
				return;
			}
			((ITextRange)this).BeginChange();
			try
			{
				if (this.BeginMouseSelectionProcess(cursorPosition))
				{
					ITextPointer anchorPosition = ((ITextSelection)this).AnchorPosition;
					TextSegment textSegment;
					TextSegment textSegment2;
					this.IdentifyWordsOnSelectionEnds(anchorPosition, cursorPosition, forceWordSelection, out textSegment, out textSegment2);
					ITextPointer frozenPointer;
					ITextPointer frozenPointer2;
					if (textSegment.Start.CompareTo(textSegment2.Start) <= 0)
					{
						frozenPointer = textSegment.Start.GetFrozenPointer(LogicalDirection.Forward);
						frozenPointer2 = textSegment2.End.GetFrozenPointer(LogicalDirection.Backward);
					}
					else
					{
						frozenPointer = textSegment.End.GetFrozenPointer(LogicalDirection.Backward);
						frozenPointer2 = textSegment2.Start.GetFrozenPointer(LogicalDirection.Forward);
					}
					TextRangeBase.Select(this, frozenPointer, frozenPointer2, true);
					this.SetActivePositions(anchorPosition, frozenPointer2);
					this._previousCursorPosition = cursorPosition.CreatePointer();
					Invariant.Assert(((ITextRange)this).Contains(((ITextSelection)this).AnchorPosition));
				}
			}
			finally
			{
				((ITextRange)this).EndChange();
			}
		}

		// Token: 0x06005997 RID: 22935 RVA: 0x0027CD28 File Offset: 0x0027BD28
		private bool BeginMouseSelectionProcess(ITextPointer cursorPosition)
		{
			if (this._previousCursorPosition == null)
			{
				this._anchorWordRangeHasBeenCrossedOnce = false;
				this._allowWordExpansionOnAnchorEnd = true;
				this._reenterPosition = null;
				if (base.GetUIElementSelected() != null)
				{
					this._previousCursorPosition = cursorPosition.CreatePointer();
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005998 RID: 22936 RVA: 0x0027CD60 File Offset: 0x0027BD60
		private void IdentifyWordsOnSelectionEnds(ITextPointer anchorPosition, ITextPointer cursorPosition, bool forceWordSelection, out TextSegment anchorWordRange, out TextSegment cursorWordRange)
		{
			if (forceWordSelection)
			{
				anchorWordRange = TextPointerBase.GetWordRange(anchorPosition);
				cursorWordRange = TextPointerBase.GetWordRange(cursorPosition, cursorPosition.LogicalDirection);
				return;
			}
			if (!this._textEditor.AutoWordSelection || ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Control) > ModifierKeys.None))
			{
				anchorWordRange = new TextSegment(anchorPosition, anchorPosition);
				cursorWordRange = new TextSegment(cursorPosition, cursorPosition);
				return;
			}
			anchorWordRange = TextPointerBase.GetWordRange(anchorPosition);
			if (this._previousCursorPosition != null && ((anchorPosition.CompareTo(cursorPosition) < 0 && cursorPosition.CompareTo(this._previousCursorPosition) < 0) || (this._previousCursorPosition.CompareTo(cursorPosition) < 0 && cursorPosition.CompareTo(anchorPosition) < 0)))
			{
				this._reenterPosition = cursorPosition.CreatePointer();
				if (this._anchorWordRangeHasBeenCrossedOnce && anchorWordRange.Contains(cursorPosition))
				{
					this._allowWordExpansionOnAnchorEnd = false;
				}
			}
			else if (this._reenterPosition != null && !TextPointerBase.GetWordRange(this._reenterPosition).Contains(cursorPosition))
			{
				this._reenterPosition = null;
			}
			if (anchorWordRange.Contains(cursorPosition) || anchorWordRange.Contains(cursorPosition.GetInsertionPosition(LogicalDirection.Forward)) || anchorWordRange.Contains(cursorPosition.GetInsertionPosition(LogicalDirection.Backward)))
			{
				anchorWordRange = new TextSegment(anchorPosition, anchorPosition);
				cursorWordRange = new TextSegment(cursorPosition, cursorPosition);
				return;
			}
			this._anchorWordRangeHasBeenCrossedOnce = true;
			if (!this._allowWordExpansionOnAnchorEnd || TextPointerBase.IsAtWordBoundary(anchorPosition, LogicalDirection.Forward))
			{
				anchorWordRange = new TextSegment(anchorPosition, anchorPosition);
			}
			if (TextPointerBase.IsAfterLastParagraph(cursorPosition) || TextPointerBase.IsAtWordBoundary(cursorPosition, LogicalDirection.Forward))
			{
				cursorWordRange = new TextSegment(cursorPosition, cursorPosition);
				return;
			}
			if (this._reenterPosition == null)
			{
				cursorWordRange = TextPointerBase.GetWordRange(cursorPosition, cursorPosition.LogicalDirection);
				return;
			}
			cursorWordRange = new TextSegment(cursorPosition, cursorPosition);
		}

		// Token: 0x06005999 RID: 22937 RVA: 0x0027CF20 File Offset: 0x0027BF20
		bool ITextSelection.ExtendToNextTableRow(LogicalDirection direction)
		{
			if (!base.IsTableCellRange)
			{
				return false;
			}
			Invariant.Assert(!base.IsEmpty);
			Invariant.Assert(this._anchorPosition != null);
			Invariant.Assert(this._movingPositionEdge != TextSelection.MovingEdge.None);
			TableCell tableCell;
			TableCell tableCell2;
			if (!TextRangeEditTables.IsTableCellRange((TextPointer)this._anchorPosition, (TextPointer)((ITextSelection)this).MovingPosition, false, out tableCell, out tableCell2))
			{
				return false;
			}
			Invariant.Assert(tableCell != null && tableCell2 != null);
			TableRowGroup rowGroup = tableCell2.Row.RowGroup;
			TableCell tableCell3 = null;
			if (direction == LogicalDirection.Forward)
			{
				int i = tableCell2.Row.Index + tableCell2.RowSpan;
				if (i < rowGroup.Rows.Count)
				{
					tableCell3 = TextSelection.FindCellAtColumnIndex(rowGroup.Rows[i].Cells, tableCell2.ColumnIndex);
				}
			}
			else
			{
				for (int i = tableCell2.Row.Index - 1; i >= 0; i--)
				{
					tableCell3 = TextSelection.FindCellAtColumnIndex(rowGroup.Rows[i].Cells, tableCell2.ColumnIndex);
					if (tableCell3 != null)
					{
						break;
					}
				}
			}
			if (tableCell3 != null)
			{
				ITextPointer textPointer = tableCell3.ContentEnd.CreatePointer();
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
				TextRangeBase.Select(this, this._anchorPosition, textPointer);
				this.SetActivePositions(this._anchorPosition, textPointer);
				return true;
			}
			return false;
		}

		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x0600599A RID: 22938 RVA: 0x0027D05F File Offset: 0x0027C05F
		internal bool IsInterimSelection
		{
			get
			{
				return this.TextStore != null && this.TextStore.IsInterimSelection;
			}
		}

		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x0600599B RID: 22939 RVA: 0x0027D076 File Offset: 0x0027C076
		bool ITextSelection.IsInterimSelection
		{
			get
			{
				return this.IsInterimSelection;
			}
		}

		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x0600599C RID: 22940 RVA: 0x0027D07E File Offset: 0x0027C07E
		internal TextPointer AnchorPosition
		{
			get
			{
				return (TextPointer)((ITextSelection)this).AnchorPosition;
			}
		}

		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x0600599D RID: 22941 RVA: 0x0027D08B File Offset: 0x0027C08B
		internal TextPointer MovingPosition
		{
			get
			{
				return (TextPointer)((ITextSelection)this).MovingPosition;
			}
		}

		// Token: 0x0600599E RID: 22942 RVA: 0x0027D098 File Offset: 0x0027C098
		internal void SetCaretToPosition(TextPointer caretPosition, LogicalDirection direction, bool allowStopAtLineEnd, bool allowStopNearSpace)
		{
			((ITextSelection)this).SetCaretToPosition(caretPosition, direction, allowStopAtLineEnd, allowStopNearSpace);
		}

		// Token: 0x0600599F RID: 22943 RVA: 0x0027D0A5 File Offset: 0x0027C0A5
		internal bool ExtendToNextInsertionPosition(LogicalDirection direction)
		{
			return ((ITextSelection)this).ExtendToNextInsertionPosition(direction);
		}

		// Token: 0x060059A0 RID: 22944 RVA: 0x0027D0B0 File Offset: 0x0027C0B0
		internal static void OnInputLanguageChanged(CultureInfo cultureInfo)
		{
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (TextSelection.IsBidiInputLanguage(cultureInfo))
			{
				threadLocalStore.Bidi = true;
			}
			else
			{
				threadLocalStore.Bidi = false;
			}
			if (threadLocalStore.FocusedTextSelection != null)
			{
				((ITextSelection)threadLocalStore.FocusedTextSelection).RefreshCaret();
			}
		}

		// Token: 0x060059A1 RID: 22945 RVA: 0x0027D0EE File Offset: 0x0027C0EE
		internal bool Contains(Point point)
		{
			return ((ITextSelection)this).Contains(point);
		}

		// Token: 0x060059A2 RID: 22946 RVA: 0x0027D0F8 File Offset: 0x0027C0F8
		internal override void InsertEmbeddedUIElementVirtual(FrameworkElement embeddedElement)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				base.InsertEmbeddedUIElementVirtual(embeddedElement);
				this.ClearSpringloadFormatting();
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x060059A3 RID: 22947 RVA: 0x0027D134 File Offset: 0x0027C134
		internal override void ApplyPropertyToTextVirtual(DependencyProperty formattingProperty, object value, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			if (!TextSchema.IsParagraphProperty(formattingProperty) && !TextSchema.IsCharacterProperty(formattingProperty))
			{
				return;
			}
			if (!base.IsEmpty || !TextSchema.IsCharacterProperty(formattingProperty) || applyToParagraphs || formattingProperty == FrameworkElement.FlowDirectionProperty)
			{
				base.ApplyPropertyToTextVirtual(formattingProperty, value, applyToParagraphs, propertyValueAction);
				this.ClearSpringloadFormatting();
				return;
			}
			TextSegment autoWord = TextRangeBase.GetAutoWord(this);
			if (autoWord.IsNull)
			{
				if (this._springloadFormatting == null)
				{
					this._springloadFormatting = new DependencyObject();
				}
				this._springloadFormatting.SetValue(formattingProperty, value);
				return;
			}
			new TextRange(autoWord.Start, autoWord.End).ApplyPropertyValue(formattingProperty, value);
		}

		// Token: 0x060059A4 RID: 22948 RVA: 0x0027D1CC File Offset: 0x0027C1CC
		internal override void ClearAllPropertiesVirtual()
		{
			if (base.IsEmpty)
			{
				this.ClearSpringloadFormatting();
				return;
			}
			TextRangeBase.BeginChange(this);
			try
			{
				base.ClearAllPropertiesVirtual();
				this.ClearSpringloadFormatting();
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x060059A5 RID: 22949 RVA: 0x0027D214 File Offset: 0x0027C214
		internal override void SetXmlVirtual(TextElement fragment)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				base.SetXmlVirtual(fragment);
				this.ClearSpringloadFormatting();
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x060059A6 RID: 22950 RVA: 0x0027D250 File Offset: 0x0027C250
		internal override void LoadVirtual(Stream stream, string dataFormat)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				base.LoadVirtual(stream, dataFormat);
				this.ClearSpringloadFormatting();
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x0027D28C File Offset: 0x0027C28C
		internal override Table InsertTableVirtual(int rowCount, int columnCount)
		{
			Table result;
			using (base.DeclareChangeBlock())
			{
				Table table = base.InsertTableVirtual(rowCount, columnCount);
				if (table != null)
				{
					TextPointer contentStart = table.RowGroups[0].Rows[0].Cells[0].ContentStart;
					this.SetCaretToPosition(contentStart, LogicalDirection.Backward, false, false);
				}
				result = table;
			}
			return result;
		}

		// Token: 0x060059A8 RID: 22952 RVA: 0x0027D300 File Offset: 0x0027C300
		internal object GetCurrentValue(DependencyProperty formattingProperty)
		{
			object obj = DependencyProperty.UnsetValue;
			if (((ITextRange)this).Start is TextPointer && this._springloadFormatting != null && base.IsEmpty)
			{
				obj = this._springloadFormatting.ReadLocalValue(formattingProperty);
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = base.Start.Parent.GetValue(formattingProperty);
				}
			}
			if (obj == DependencyProperty.UnsetValue)
			{
				obj = this.PropertyPosition.GetValue(formattingProperty);
			}
			return obj;
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x0027D370 File Offset: 0x0027C370
		internal void SpringloadCurrentFormatting()
		{
			if (((ITextRange)this).Start is TextPointer)
			{
				TextPointer textPointer = base.Start;
				Inline nonMergeableInlineAncestor = textPointer.GetNonMergeableInlineAncestor();
				if (nonMergeableInlineAncestor != null && base.End.GetNonMergeableInlineAncestor() != nonMergeableInlineAncestor)
				{
					textPointer = nonMergeableInlineAncestor.ElementEnd;
				}
				if (this._springloadFormatting == null)
				{
					this.SpringloadCurrentFormatting(textPointer.Parent);
				}
			}
		}

		// Token: 0x060059AA RID: 22954 RVA: 0x0027D3C4 File Offset: 0x0027C3C4
		private void SpringloadCurrentFormatting(DependencyObject parent)
		{
			this._springloadFormatting = new DependencyObject();
			if (parent == null)
			{
				return;
			}
			DependencyProperty[] inheritableProperties = TextSchema.GetInheritableProperties(typeof(Inline));
			DependencyProperty[] noninheritableProperties = TextSchema.GetNoninheritableProperties(typeof(Span));
			DependencyObject dependencyObject = parent;
			while (dependencyObject is Inline)
			{
				if (((TextElementEditingBehaviorAttribute)Attribute.GetCustomAttribute(dependencyObject.GetType(), typeof(TextElementEditingBehaviorAttribute))).IsTypographicOnly)
				{
					for (int i = 0; i < inheritableProperties.Length; i++)
					{
						if (this._springloadFormatting.ReadLocalValue(inheritableProperties[i]) == DependencyProperty.UnsetValue && inheritableProperties[i] != FrameworkElement.LanguageProperty && inheritableProperties[i] != FrameworkElement.FlowDirectionProperty && DependencyPropertyHelper.GetValueSource(dependencyObject, inheritableProperties[i]).BaseValueSource != BaseValueSource.Inherited)
						{
							object value = parent.GetValue(inheritableProperties[i]);
							this._springloadFormatting.SetValue(inheritableProperties[i], value);
						}
					}
					for (int j = 0; j < noninheritableProperties.Length; j++)
					{
						if (this._springloadFormatting.ReadLocalValue(noninheritableProperties[j]) == DependencyProperty.UnsetValue && noninheritableProperties[j] != TextElement.TextEffectsProperty && DependencyPropertyHelper.GetValueSource(dependencyObject, noninheritableProperties[j]).BaseValueSource != BaseValueSource.Inherited)
						{
							object value2 = parent.GetValue(noninheritableProperties[j]);
							this._springloadFormatting.SetValue(noninheritableProperties[j], value2);
						}
					}
				}
				dependencyObject = ((TextElement)dependencyObject).Parent;
			}
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x0027D50F File Offset: 0x0027C50F
		internal void ClearSpringloadFormatting()
		{
			if (((ITextRange)this).Start is TextPointer)
			{
				this._springloadFormatting = null;
				((ITextSelection)this).RefreshCaret();
			}
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x0027D52C File Offset: 0x0027C52C
		internal void ApplySpringloadFormatting()
		{
			if (!(((ITextRange)this).Start is TextPointer))
			{
				return;
			}
			if (base.IsEmpty)
			{
				return;
			}
			if (this._springloadFormatting != null)
			{
				Invariant.Assert(base.Start.LogicalDirection == LogicalDirection.Backward);
				Invariant.Assert(base.End.LogicalDirection == LogicalDirection.Forward);
				LocalValueEnumerator localValueEnumerator = this._springloadFormatting.GetLocalValueEnumerator();
				while (!base.IsEmpty && localValueEnumerator.MoveNext())
				{
					LocalValueEntry localValueEntry = localValueEnumerator.Current;
					Invariant.Assert(TextSchema.IsCharacterProperty(localValueEntry.Property));
					base.ApplyPropertyValue(localValueEntry.Property, localValueEntry.Value);
				}
				this.ClearSpringloadFormatting();
			}
		}

		// Token: 0x060059AD RID: 22957 RVA: 0x0027D5D0 File Offset: 0x0027C5D0
		internal void UpdateCaretState(CaretScrollMethod caretScrollMethod)
		{
			Invariant.Assert(caretScrollMethod > CaretScrollMethod.Unset);
			if (this._pendingCaretNavigation)
			{
				caretScrollMethod = CaretScrollMethod.Navigation;
				this._pendingCaretNavigation = false;
			}
			if (this._caretScrollMethod != CaretScrollMethod.Unset)
			{
				if (caretScrollMethod != CaretScrollMethod.None)
				{
					this._caretScrollMethod = caretScrollMethod;
				}
				return;
			}
			this._caretScrollMethod = caretScrollMethod;
			if (this._textEditor.TextView != null && this._textEditor.TextView.IsValid)
			{
				this.UpdateCaretStateWorker(null);
				return;
			}
			this._pendingUpdateCaretStateCallback = true;
		}

		// Token: 0x060059AE RID: 22958 RVA: 0x0027D644 File Offset: 0x0027C644
		internal static Brush GetCaretBrush(TextEditor textEditor)
		{
			Brush brush = (Brush)textEditor.UiScope.GetValue(TextBoxBase.CaretBrushProperty);
			if (brush != null)
			{
				return brush;
			}
			object obj = textEditor.UiScope.GetValue(Panel.BackgroundProperty);
			Color color;
			if (obj != null && obj != DependencyProperty.UnsetValue && obj is SolidColorBrush)
			{
				color = ((SolidColorBrush)obj).Color;
			}
			else
			{
				color = SystemColors.WindowColor;
			}
			ITextSelection selection = textEditor.Selection;
			if (selection is TextSelection)
			{
				obj = ((TextSelection)selection).GetCurrentValue(TextElement.BackgroundProperty);
				if (obj != null && obj != DependencyProperty.UnsetValue && obj is SolidColorBrush)
				{
					color = ((SolidColorBrush)obj).Color;
				}
			}
			byte r = ~color.R;
			byte g = ~color.G;
			byte b = ~color.B;
			brush = new SolidColorBrush(Color.FromRgb(r, g, b));
			brush.Freeze();
			return brush;
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x0027D718 File Offset: 0x0027C718
		internal static bool IsBidiInputLanguageInstalled()
		{
			bool result = false;
			int keyboardLayoutList = SafeNativeMethods.GetKeyboardLayoutList(0, null);
			if (keyboardLayoutList > 0)
			{
				IntPtr[] array = new IntPtr[keyboardLayoutList];
				keyboardLayoutList = SafeNativeMethods.GetKeyboardLayoutList(keyboardLayoutList, array);
				int num = 0;
				while (num < array.Length && num < keyboardLayoutList)
				{
					if (TextSelection.IsBidiInputLanguage(new CultureInfo((int)((short)((int)array[num])))))
					{
						result = true;
						break;
					}
					num++;
				}
			}
			return result;
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x0027D76E File Offset: 0x0027C76E
		void ITextSelection.ValidateLayout()
		{
			((ITextSelection)this).MovingPosition.ValidateLayout();
		}

		// Token: 0x170014C6 RID: 5318
		// (get) Token: 0x060059B1 RID: 22961 RVA: 0x0027D77C File Offset: 0x0027C77C
		internal CaretElement CaretElement
		{
			get
			{
				return this._caretElement;
			}
		}

		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x060059B2 RID: 22962 RVA: 0x0027D784 File Offset: 0x0027C784
		CaretElement ITextSelection.CaretElement
		{
			get
			{
				return this.CaretElement;
			}
		}

		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x060059B3 RID: 22963 RVA: 0x0027D78C File Offset: 0x0027C78C
		bool ITextSelection.CoversEntireContent
		{
			get
			{
				return ((ITextRange)this).Start.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text && ((ITextRange)this).End.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text && ((ITextRange)this).Start.GetNextInsertionPosition(LogicalDirection.Backward) == null && ((ITextRange)this).End.GetNextInsertionPosition(LogicalDirection.Forward) == null;
			}
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x0027D7D8 File Offset: 0x0027C7D8
		private void SetThreadSelection()
		{
			TextEditor._ThreadLocalStore.FocusedTextSelection = this;
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x0027D7E5 File Offset: 0x0027C7E5
		private void ClearThreadSelection()
		{
			if (TextEditor._ThreadLocalStore.FocusedTextSelection == this)
			{
				TextEditor._ThreadLocalStore.FocusedTextSelection = null;
			}
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x0027D800 File Offset: 0x0027C800
		private void Highlight()
		{
			ITextContainer textContainer = ((ITextRange)this).Start.TextContainer;
			if (FrameworkAppContextSwitches.UseAdornerForTextboxSelectionRendering && (textContainer is TextContainer || textContainer is PasswordTextContainer))
			{
				return;
			}
			if (this._highlightLayer == null)
			{
				this._highlightLayer = new TextSelectionHighlightLayer(this);
			}
			if (textContainer.Highlights.GetLayer(typeof(TextSelection)) == null)
			{
				textContainer.Highlights.AddLayer(this._highlightLayer);
			}
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x0027D870 File Offset: 0x0027C870
		private void Unhighlight()
		{
			ITextContainer textContainer = ((ITextRange)this).Start.TextContainer;
			TextSelectionHighlightLayer textSelectionHighlightLayer = textContainer.Highlights.GetLayer(typeof(TextSelection)) as TextSelectionHighlightLayer;
			if (textSelectionHighlightLayer != null)
			{
				textContainer.Highlights.RemoveLayer(textSelectionHighlightLayer);
				Invariant.Assert(textContainer.Highlights.GetLayer(typeof(TextSelection)) == null);
			}
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x0027D8D0 File Offset: 0x0027C8D0
		private void SetActivePositions(ITextPointer anchorPosition, ITextPointer movingPosition)
		{
			this._previousCursorPosition = null;
			if (base.IsEmpty)
			{
				this._anchorPosition = null;
				this._movingPositionEdge = TextSelection.MovingEdge.None;
				return;
			}
			Invariant.Assert(anchorPosition != null);
			this._anchorPosition = anchorPosition.GetInsertionPosition(anchorPosition.LogicalDirection);
			if (this._anchorPosition.CompareTo(((ITextRange)this).Start) < 0)
			{
				this._anchorPosition = ((ITextRange)this).Start.GetFrozenPointer(this._anchorPosition.LogicalDirection);
			}
			else if (this._anchorPosition.CompareTo(((ITextRange)this).End) > 0)
			{
				this._anchorPosition = ((ITextRange)this).End.GetFrozenPointer(this._anchorPosition.LogicalDirection);
			}
			this._movingPositionEdge = this.ConvertToMovingEdge(anchorPosition, movingPosition);
			this._movingPositionDirection = movingPosition.LogicalDirection;
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x0027D994 File Offset: 0x0027C994
		private TextSelection.MovingEdge ConvertToMovingEdge(ITextPointer anchorPosition, ITextPointer movingPosition)
		{
			TextSelection.MovingEdge result;
			if (((ITextRange)this).IsEmpty)
			{
				result = TextSelection.MovingEdge.None;
			}
			else if (((ITextRange)this).TextSegments.Count < 2)
			{
				result = ((anchorPosition.CompareTo(movingPosition) <= 0) ? TextSelection.MovingEdge.End : TextSelection.MovingEdge.Start);
			}
			else if (movingPosition.CompareTo(((ITextRange)this).Start) == 0)
			{
				result = TextSelection.MovingEdge.Start;
			}
			else if (movingPosition.CompareTo(((ITextRange)this).End) == 0)
			{
				result = TextSelection.MovingEdge.End;
			}
			else if (movingPosition.CompareTo(((ITextRange)this).TextSegments[0].End) == 0)
			{
				result = TextSelection.MovingEdge.StartInner;
			}
			else if (movingPosition.CompareTo(((ITextRange)this).TextSegments[((ITextRange)this).TextSegments.Count - 1].Start) == 0)
			{
				result = TextSelection.MovingEdge.EndInner;
			}
			else
			{
				result = ((anchorPosition.CompareTo(movingPosition) <= 0) ? TextSelection.MovingEdge.End : TextSelection.MovingEdge.Start);
			}
			return result;
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x0027DA54 File Offset: 0x0027CA54
		private void MoveSelectionByMouse(ITextPointer cursorPosition, Point cursorMousePoint)
		{
			if (this.TextView == null)
			{
				return;
			}
			Invariant.Assert(this.TextView.IsValid);
			ITextPointer textPointer = null;
			if (cursorPosition.GetPointerContext(cursorPosition.LogicalDirection) == TextPointerContext.EmbeddedElement)
			{
				Rect rectangleFromTextPosition = this.TextView.GetRectangleFromTextPosition(cursorPosition);
				if (!this._textEditor.IsReadOnly && this.ShouldSelectEmbeddedObject(cursorPosition, cursorMousePoint, rectangleFromTextPosition))
				{
					textPointer = cursorPosition.GetNextContextPosition(cursorPosition.LogicalDirection);
				}
			}
			if (textPointer == null)
			{
				((ITextSelection)this).SetCaretToPosition(cursorPosition, cursorPosition.LogicalDirection, true, false);
				return;
			}
			((ITextRange)this).Select(cursorPosition, textPointer);
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x0027DADC File Offset: 0x0027CADC
		private bool ShouldSelectEmbeddedObject(ITextPointer cursorPosition, Point cursorMousePoint, Rect objectEdgeRect)
		{
			if (!objectEdgeRect.IsEmpty && cursorMousePoint.Y >= objectEdgeRect.Y && cursorMousePoint.Y < objectEdgeRect.Y + objectEdgeRect.Height)
			{
				bool flag = (FlowDirection)this.TextView.RenderScope.GetValue(Block.FlowDirectionProperty) != FlowDirection.LeftToRight;
				FlowDirection flowDirection = (FlowDirection)cursorPosition.GetValue(Block.FlowDirectionProperty);
				if (!flag)
				{
					if (flowDirection == FlowDirection.LeftToRight && ((cursorPosition.LogicalDirection == LogicalDirection.Forward && objectEdgeRect.X < cursorMousePoint.X) || (cursorPosition.LogicalDirection == LogicalDirection.Backward && cursorMousePoint.X < objectEdgeRect.X)))
					{
						return true;
					}
					if (flowDirection == FlowDirection.RightToLeft && ((cursorPosition.LogicalDirection == LogicalDirection.Forward && objectEdgeRect.X > cursorMousePoint.X) || (cursorPosition.LogicalDirection == LogicalDirection.Backward && cursorMousePoint.X > objectEdgeRect.X)))
					{
						return true;
					}
				}
				else
				{
					if (flowDirection == FlowDirection.LeftToRight && ((cursorPosition.LogicalDirection == LogicalDirection.Forward && objectEdgeRect.X > cursorMousePoint.X) || (cursorPosition.LogicalDirection == LogicalDirection.Backward && cursorMousePoint.X > objectEdgeRect.X)))
					{
						return true;
					}
					if (flowDirection == FlowDirection.RightToLeft && ((cursorPosition.LogicalDirection == LogicalDirection.Forward && objectEdgeRect.X < cursorMousePoint.X) || (cursorPosition.LogicalDirection == LogicalDirection.Backward && cursorMousePoint.X < objectEdgeRect.X)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x0027DC30 File Offset: 0x0027CC30
		private static void RefreshCaret(TextEditor textEditor, ITextSelection textSelection)
		{
			if (textSelection == null || textSelection.CaretElement == null)
			{
				return;
			}
			object currentValue = ((TextSelection)textSelection).GetCurrentValue(TextElement.FontStyleProperty);
			bool italic = textEditor.AcceptsRichContent && currentValue != DependencyProperty.UnsetValue && (FontStyle)currentValue == FontStyles.Italic;
			textSelection.CaretElement.RefreshCaret(italic);
		}

		// Token: 0x060059BD RID: 22973 RVA: 0x0027DC8A File Offset: 0x0027CC8A
		internal void OnCaretNavigation()
		{
			this._pendingCaretNavigation = true;
		}

		// Token: 0x060059BE RID: 22974 RVA: 0x0027DC93 File Offset: 0x0027CC93
		void ITextSelection.OnCaretNavigation()
		{
			this.OnCaretNavigation();
		}

		// Token: 0x060059BF RID: 22975 RVA: 0x0027DC9C File Offset: 0x0027CC9C
		private object UpdateCaretStateWorker(object o)
		{
			this._pendingUpdateCaretStateCallback = false;
			if (this._textEditor == null)
			{
				return null;
			}
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			CaretScrollMethod caretScrollMethod = this._caretScrollMethod;
			this._caretScrollMethod = CaretScrollMethod.Unset;
			CaretElement caretElement = this._caretElement;
			if (caretElement == null)
			{
				return null;
			}
			if (threadLocalStore.FocusedTextSelection == null)
			{
				if (!base.IsEmpty)
				{
					caretElement.Hide();
				}
				return null;
			}
			if (this._textEditor.TextView == null || !this._textEditor.TextView.IsValid)
			{
				return null;
			}
			if (!this.VerifyAdornerLayerExists())
			{
				caretElement.Hide();
			}
			ITextPointer textPointer = TextSelection.IdentifyCaretPosition(this);
			if (textPointer.HasValidLayout)
			{
				bool italic = false;
				bool visible = base.IsEmpty && (!this._textEditor.IsReadOnly || this._textEditor.IsReadOnlyCaretVisible);
				Rect caretRectangle;
				if (!this.IsInterimSelection)
				{
					caretRectangle = TextSelection.CalculateCaretRectangle(this, textPointer);
					if (base.IsEmpty)
					{
						object propertyValue = base.GetPropertyValue(TextElement.FontStyleProperty);
						italic = (this._textEditor.AcceptsRichContent && propertyValue != DependencyProperty.UnsetValue && (FontStyle)propertyValue == FontStyles.Italic);
					}
				}
				else
				{
					caretRectangle = TextSelection.CalculateInterimCaretRectangle(this);
					visible = true;
				}
				Brush caretBrush = TextSelection.GetCaretBrush(this._textEditor);
				double scrollToOriginPosition = TextSelection.CalculateScrollToOriginPosition(this._textEditor, textPointer, caretRectangle.X);
				caretElement.Update(visible, caretRectangle, caretBrush, 1.0, italic, caretScrollMethod, scrollToOriginPosition);
			}
			if (this.TextView.IsValid && !this.TextView.RendersOwnSelection)
			{
				caretElement.UpdateSelection();
			}
			return null;
		}

		// Token: 0x060059C0 RID: 22976 RVA: 0x0027DE1C File Offset: 0x0027CE1C
		private static ITextPointer IdentifyCaretPosition(ITextSelection currentTextSelection)
		{
			ITextPointer textPointer = currentTextSelection.MovingPosition;
			if (!currentTextSelection.IsEmpty && ((textPointer.LogicalDirection == LogicalDirection.Backward && textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart) || TextPointerBase.IsAfterLastParagraph(textPointer)))
			{
				textPointer = textPointer.CreatePointer();
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
				textPointer.SetLogicalDirection(LogicalDirection.Forward);
			}
			if (textPointer.LogicalDirection == LogicalDirection.Backward && textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && (textPointer.GetNextInsertionPosition(LogicalDirection.Backward) == null || TextPointerBase.IsNextToAnyBreak(textPointer, LogicalDirection.Backward)))
			{
				textPointer = textPointer.CreatePointer();
				textPointer.SetLogicalDirection(LogicalDirection.Forward);
			}
			return textPointer;
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x0027DE9C File Offset: 0x0027CE9C
		private static Rect CalculateCaretRectangle(ITextSelection currentTextSelection, ITextPointer caretPosition)
		{
			Transform transform;
			Rect rect = currentTextSelection.TextView.GetRawRectangleFromTextPosition(caretPosition, out transform);
			if (rect.IsEmpty)
			{
				return Rect.Empty;
			}
			rect = transform.TransformBounds(rect);
			rect.Width = 0.0;
			if (currentTextSelection.IsEmpty)
			{
				double num = (double)currentTextSelection.GetPropertyValue(TextElement.FontSizeProperty);
				double num2 = ((FontFamily)currentTextSelection.GetPropertyValue(TextElement.FontFamilyProperty)).LineSpacing * num;
				if (num2 < rect.Height)
				{
					rect.Y += rect.Height - num2;
					rect.Height = num2;
				}
				if (!transform.IsIdentity)
				{
					Point inPoint = new Point(rect.X, rect.Y);
					Point inPoint2 = new Point(rect.X, rect.Y + rect.Height);
					transform.TryTransform(inPoint, out inPoint);
					transform.TryTransform(inPoint2, out inPoint2);
					rect.Y += rect.Height - Math.Abs(inPoint2.Y - inPoint.Y);
					rect.Height = Math.Abs(inPoint2.Y - inPoint.Y);
				}
			}
			return rect;
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x0027DFD8 File Offset: 0x0027CFD8
		private static Rect CalculateInterimCaretRectangle(ITextSelection focusedTextSelection)
		{
			Rect rectangleFromTextPosition;
			Rect rectangleFromTextPosition2;
			if ((FlowDirection)focusedTextSelection.Start.GetValue(FrameworkElement.FlowDirectionProperty) != FlowDirection.RightToLeft)
			{
				ITextPointer textPointer = focusedTextSelection.Start.CreatePointer(LogicalDirection.Forward);
				rectangleFromTextPosition = focusedTextSelection.TextView.GetRectangleFromTextPosition(textPointer);
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
				textPointer.SetLogicalDirection(LogicalDirection.Backward);
				rectangleFromTextPosition2 = focusedTextSelection.TextView.GetRectangleFromTextPosition(textPointer);
			}
			else
			{
				ITextPointer textPointer = focusedTextSelection.End.CreatePointer(LogicalDirection.Backward);
				rectangleFromTextPosition = focusedTextSelection.TextView.GetRectangleFromTextPosition(textPointer);
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
				textPointer.SetLogicalDirection(LogicalDirection.Forward);
				rectangleFromTextPosition2 = focusedTextSelection.TextView.GetRectangleFromTextPosition(textPointer);
			}
			if (!rectangleFromTextPosition.IsEmpty && !rectangleFromTextPosition2.IsEmpty && rectangleFromTextPosition2.Left > rectangleFromTextPosition.Left)
			{
				rectangleFromTextPosition.Width = rectangleFromTextPosition2.Left - rectangleFromTextPosition.Left;
			}
			return rectangleFromTextPosition;
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x0027E0A4 File Offset: 0x0027D0A4
		private static double CalculateScrollToOriginPosition(TextEditor textEditor, ITextPointer caretPosition, double horizontalCaretPosition)
		{
			double num = double.NaN;
			if (textEditor.UiScope is TextBoxBase)
			{
				double viewportWidth = ((TextBoxBase)textEditor.UiScope).ViewportWidth;
				double extentWidth = ((TextBoxBase)textEditor.UiScope).ExtentWidth;
				if (viewportWidth != 0.0 && extentWidth != 0.0 && viewportWidth < extentWidth)
				{
					bool flag = false;
					if (horizontalCaretPosition < 0.0 || horizontalCaretPosition >= viewportWidth)
					{
						flag = true;
					}
					if (flag)
					{
						num = 0.0;
						FlowDirection flowDirection = (FlowDirection)textEditor.UiScope.GetValue(FrameworkElement.FlowDirectionProperty);
						Block block = (caretPosition is TextPointer) ? ((TextPointer)caretPosition).ParagraphOrBlockUIContainer : null;
						if (block != null)
						{
							FlowDirection flowDirection2 = block.FlowDirection;
							if (flowDirection != flowDirection2)
							{
								num = extentWidth;
							}
						}
						num -= ((TextBoxBase)textEditor.UiScope).HorizontalOffset;
					}
				}
			}
			return num;
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x0027E188 File Offset: 0x0027D188
		private CaretElement EnsureCaret(bool isBlinkEnabled, bool isSelectionActive, CaretScrollMethod scrollMethod)
		{
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (this._caretElement == null)
			{
				this._caretElement = new CaretElement(this._textEditor, isBlinkEnabled);
				this._caretElement.IsSelectionActive = isSelectionActive;
				if (TextSelection.IsBidiInputLanguage(InputLanguageManager.Current.CurrentInputLanguage))
				{
					TextEditor._ThreadLocalStore.Bidi = true;
				}
				else
				{
					TextEditor._ThreadLocalStore.Bidi = false;
				}
			}
			else
			{
				this._caretElement.IsSelectionActive = isSelectionActive;
				this._caretElement.SetBlinking(isBlinkEnabled);
			}
			this.UpdateCaretState(scrollMethod);
			return this._caretElement;
		}

		// Token: 0x060059C5 RID: 22981 RVA: 0x0027E214 File Offset: 0x0027D214
		private bool VerifyAdornerLayerExists()
		{
			DependencyObject dependencyObject = this.TextView.RenderScope;
			while (dependencyObject != this._textEditor.UiScope && dependencyObject != null)
			{
				if (dependencyObject is AdornerDecorator || dependencyObject is ScrollContentPresenter)
				{
					return true;
				}
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			}
			return false;
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x0027E25C File Offset: 0x0027D25C
		private static bool IsBidiInputLanguage(CultureInfo cultureInfo)
		{
			bool result = false;
			string text = new string(new char[16]);
			if (UnsafeNativeMethods.GetLocaleInfoW(cultureInfo.LCID, 88, text, 16) != 0 && (text[7] & 'ࠀ') != '\0')
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x0027E29C File Offset: 0x0027D29C
		private static TableCell FindCellAtColumnIndex(TableCellCollection cells, int columnIndex)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				TableCell tableCell = cells[i];
				int columnIndex2 = tableCell.ColumnIndex;
				int num = columnIndex2 + tableCell.ColumnSpan - 1;
				if (columnIndex2 <= columnIndex && columnIndex <= num)
				{
					return tableCell;
				}
			}
			return null;
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x0027E2DD File Offset: 0x0027D2DD
		private static bool IsRootElement(DependencyObject element)
		{
			return TextSelection.GetParentElement(element) == null;
		}

		// Token: 0x060059C9 RID: 22985 RVA: 0x0027E2E8 File Offset: 0x0027D2E8
		private bool IsFocusWithinRoot()
		{
			DependencyObject dependencyObject = this.UiScope;
			for (DependencyObject dependencyObject2 = this.UiScope; dependencyObject2 != null; dependencyObject2 = TextSelection.GetParentElement(dependencyObject))
			{
				dependencyObject = dependencyObject2;
			}
			return dependencyObject is UIElement && ((UIElement)dependencyObject).IsKeyboardFocusWithin;
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x0027E32C File Offset: 0x0027D32C
		private static DependencyObject GetParentElement(DependencyObject element)
		{
			DependencyObject dependencyObject;
			if (element is FrameworkElement || element is FrameworkContentElement)
			{
				dependencyObject = LogicalTreeHelper.GetParent(element);
				if (dependencyObject == null && element is FrameworkElement)
				{
					dependencyObject = ((FrameworkElement)element).TemplatedParent;
					if (dependencyObject == null && element is Visual)
					{
						dependencyObject = VisualTreeHelper.GetParent(element);
					}
				}
			}
			else if (element is Visual)
			{
				dependencyObject = VisualTreeHelper.GetParent(element);
			}
			else
			{
				dependencyObject = null;
			}
			return dependencyObject;
		}

		// Token: 0x060059CB RID: 22987 RVA: 0x0027E38F File Offset: 0x0027D38F
		private void DetachCaretFromVisualTree()
		{
			if (this._caretElement != null)
			{
				this._caretElement.DetachFromView();
				this._caretElement = null;
			}
		}

		// Token: 0x170014C9 RID: 5321
		// (get) Token: 0x060059CC RID: 22988 RVA: 0x0027E3AB File Offset: 0x0027D3AB
		TextEditor ITextSelection.TextEditor
		{
			get
			{
				return this._textEditor;
			}
		}

		// Token: 0x170014CA RID: 5322
		// (get) Token: 0x060059CD RID: 22989 RVA: 0x0027E3B3 File Offset: 0x0027D3B3
		ITextView ITextSelection.TextView
		{
			get
			{
				return this._textEditor.TextView;
			}
		}

		// Token: 0x170014CB RID: 5323
		// (get) Token: 0x060059CE RID: 22990 RVA: 0x0027E3C0 File Offset: 0x0027D3C0
		private ITextView TextView
		{
			get
			{
				return ((ITextSelection)this).TextView;
			}
		}

		// Token: 0x170014CC RID: 5324
		// (get) Token: 0x060059CF RID: 22991 RVA: 0x0027E3C8 File Offset: 0x0027D3C8
		private TextStore TextStore
		{
			get
			{
				return this._textEditor.TextStore;
			}
		}

		// Token: 0x170014CD RID: 5325
		// (get) Token: 0x060059D0 RID: 22992 RVA: 0x0027E3D5 File Offset: 0x0027D3D5
		private ImmComposition ImmComposition
		{
			get
			{
				return this._textEditor.ImmComposition;
			}
		}

		// Token: 0x170014CE RID: 5326
		// (get) Token: 0x060059D1 RID: 22993 RVA: 0x0027E3E2 File Offset: 0x0027D3E2
		private FrameworkElement UiScope
		{
			get
			{
				return this._textEditor.UiScope;
			}
		}

		// Token: 0x170014CF RID: 5327
		// (get) Token: 0x060059D2 RID: 22994 RVA: 0x0027E3F0 File Offset: 0x0027D3F0
		private ITextPointer PropertyPosition
		{
			get
			{
				ITextPointer textPointer = null;
				if (!((ITextRange)this).IsEmpty)
				{
					textPointer = TextPointerBase.GetFollowingNonMergeableInlineContentStart(((ITextRange)this).Start);
				}
				if (textPointer == null)
				{
					textPointer = ((ITextRange)this).Start;
				}
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x04003005 RID: 12293
		private TextEditor _textEditor;

		// Token: 0x04003006 RID: 12294
		private TextSelectionHighlightLayer _highlightLayer;

		// Token: 0x04003007 RID: 12295
		private DependencyObject _springloadFormatting;

		// Token: 0x04003008 RID: 12296
		private ITextPointer _anchorPosition;

		// Token: 0x04003009 RID: 12297
		private TextSelection.MovingEdge _movingPositionEdge;

		// Token: 0x0400300A RID: 12298
		private LogicalDirection _movingPositionDirection;

		// Token: 0x0400300B RID: 12299
		private ITextPointer _previousCursorPosition;

		// Token: 0x0400300C RID: 12300
		private ITextPointer _reenterPosition;

		// Token: 0x0400300D RID: 12301
		private bool _anchorWordRangeHasBeenCrossedOnce;

		// Token: 0x0400300E RID: 12302
		private bool _allowWordExpansionOnAnchorEnd;

		// Token: 0x0400300F RID: 12303
		private const int FONTSIGNATURE_SIZE = 16;

		// Token: 0x04003010 RID: 12304
		private const int FONTSIGNATURE_BIDI_INDEX = 7;

		// Token: 0x04003011 RID: 12305
		private const int FONTSIGNATURE_BIDI = 2048;

		// Token: 0x04003012 RID: 12306
		private CaretScrollMethod _caretScrollMethod;

		// Token: 0x04003013 RID: 12307
		private bool _pendingCaretNavigation;

		// Token: 0x04003014 RID: 12308
		private CaretElement _caretElement;

		// Token: 0x04003015 RID: 12309
		private bool _pendingUpdateCaretStateCallback;

		// Token: 0x02000B71 RID: 2929
		private enum MovingEdge
		{
			// Token: 0x040048E9 RID: 18665
			Start,
			// Token: 0x040048EA RID: 18666
			StartInner,
			// Token: 0x040048EB RID: 18667
			EndInner,
			// Token: 0x040048EC RID: 18668
			End,
			// Token: 0x040048ED RID: 18669
			None
		}
	}
}
