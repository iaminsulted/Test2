using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020006A6 RID: 1702
	internal static class TextEditorSelection
	{
		// Token: 0x06005606 RID: 22022 RVA: 0x0026552C File Offset: 0x0026452C
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(TextEditorSelection.OnNYICommand);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorSelection.OnQueryStatusCaretNavigation);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler2 = new CanExecuteRoutedEventHandler(TextEditorSelection.OnQueryStatusKeyboardSelection);
			CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.SelectAll, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectAll), canExecuteRoutedEventHandler2, "Ctrl+A", "KeySelectAllDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveRightByCharacter, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveRightByCharacter), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Right", "KeyMoveRightByCharacterDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveLeftByCharacter, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveLeftByCharacter), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Left", "KeyMoveLeftByCharacterDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveRightByWord, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveRightByWord), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+Right", "KeyMoveRightByWordDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveLeftByWord, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveLeftByWord), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+Left", "KeyMoveLeftByWordDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveDownByLine, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveDownByLine), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Down", "KeyMoveDownByLineDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveUpByLine, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveUpByLine), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Up", "KeyMoveUpByLineDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveDownByParagraph, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveDownByParagraph), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+Down", "KeyMoveDownByParagraphDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveUpByParagraph, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveUpByParagraph), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+Up", "KeyMoveUpByParagraphDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveDownByPage, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveDownByPage), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("PageDown", "KeyMoveDownByPageDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveUpByPage, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveUpByPage), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("PageUp", "KeyMoveUpByPageDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToLineStart, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveToLineStart), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Home", "KeyMoveToLineStartDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToLineEnd, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveToLineEnd), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("End", "KeyMoveToLineEndDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToColumnStart, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+PageUp", "KeyMoveToColumnStartDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToColumnEnd, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+PageDown", "KeyMoveToColumnEndDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToWindowTop, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+PageUp", "KeyMoveToWindowTopDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToWindowBottom, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+PageDown", "KeyMoveToWindowBottomDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToDocumentStart, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveToDocumentStart), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+Home", "KeyMoveToDocumentStartDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToDocumentEnd, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveToDocumentEnd), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+End", "KeyMoveToDocumentEndDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectRightByCharacter, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectRightByCharacter), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Shift+Right", "KeySelectRightByCharacterDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectLeftByCharacter, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectLeftByCharacter), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Shift+Left", "KeySelectLeftByCharacterDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectRightByWord, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectRightByWord), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Ctrl+Shift+Right", "KeySelectRightByWordDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectLeftByWord, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectLeftByWord), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Ctrl+Shift+Left", "KeySelectLeftByWordDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectDownByLine, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectDownByLine), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Shift+Down", "KeySelectDownByLineDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectUpByLine, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectUpByLine), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Shift+Up", "KeySelectUpByLineDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectDownByParagraph, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectDownByParagraph), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Ctrl+Shift+Down", "KeySelectDownByParagraphDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectUpByParagraph, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectUpByParagraph), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Ctrl+Shift+Up", "KeySelectUpByParagraphDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectDownByPage, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectDownByPage), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Shift+PageDown", "KeySelectDownByPageDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectUpByPage, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectUpByPage), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Shift+PageUp", "KeySelectUpByPageDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToLineStart, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectToLineStart), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Shift+Home", "KeySelectToLineStartDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToLineEnd, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectToLineEnd), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Shift+End", "KeySelectToLineEndDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToColumnStart, executedRoutedEventHandler, canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Alt+Shift+PageUp", "KeySelectToColumnStartDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToColumnEnd, executedRoutedEventHandler, canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Alt+Shift+PageDown", "KeySelectToColumnEndDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToWindowTop, executedRoutedEventHandler, canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+Shift+PageUp", "KeySelectToWindowTopDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToWindowBottom, executedRoutedEventHandler, canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+Shift+PageDown", "KeySelectToWindowBottomDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToDocumentStart, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectToDocumentStart), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Ctrl+Shift+Home", "KeySelectToDocumentStartDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToDocumentEnd, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectToDocumentEnd), canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Ctrl+Shift+End", "KeySelectToDocumentEndDisplayString"));
		}

		// Token: 0x06005607 RID: 22023 RVA: 0x00265AA6 File Offset: 0x00264AA6
		internal static void _ClearSuggestedX(TextEditor This)
		{
			This._suggestedX = double.NaN;
			This._NextLineAdvanceMovingPosition = null;
		}

		// Token: 0x06005608 RID: 22024 RVA: 0x00265AC0 File Offset: 0x00264AC0
		internal static TextSegment GetNormalizedLineRange(ITextView textView, ITextPointer position)
		{
			TextSegment lineRange = textView.GetLineRange(position);
			if (!lineRange.IsNull)
			{
				ITextRange textRange = new TextRange(lineRange.Start, lineRange.End);
				return new TextSegment(textRange.Start, textRange.End);
			}
			if (!typeof(BlockUIContainer).IsAssignableFrom(position.ParentType))
			{
				return lineRange;
			}
			ITextPointer textPointer = position.CreatePointer(LogicalDirection.Forward);
			textPointer.MoveToElementEdge(ElementEdge.AfterStart);
			ITextPointer textPointer2 = position.CreatePointer(LogicalDirection.Backward);
			textPointer2.MoveToElementEdge(ElementEdge.BeforeEnd);
			lineRange = new TextSegment(textPointer, textPointer2);
			return lineRange;
		}

		// Token: 0x06005609 RID: 22025 RVA: 0x00265B45 File Offset: 0x00264B45
		internal static bool IsPaginated(ITextView textview)
		{
			return !(textview is TextBoxView);
		}

		// Token: 0x0600560A RID: 22026 RVA: 0x00265B54 File Offset: 0x00264B54
		private static void OnSelectAll(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock(true))
			{
				textEditor.Selection.Select(textEditor.TextContainer.Start, textEditor.TextContainer.End);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x0600560B RID: 22027 RVA: 0x00265BE4 File Offset: 0x00264BE4
		private static void OnMoveRightByCharacter(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Backward : LogicalDirection.Forward;
			TextEditorSelection.MoveToCharacterLogicalDirection(textEditor, direction, false);
		}

		// Token: 0x0600560C RID: 22028 RVA: 0x00265C28 File Offset: 0x00264C28
		private static void OnMoveLeftByCharacter(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Forward : LogicalDirection.Backward;
			TextEditorSelection.MoveToCharacterLogicalDirection(textEditor, direction, false);
		}

		// Token: 0x0600560D RID: 22029 RVA: 0x00265C6C File Offset: 0x00264C6C
		private static void OnMoveRightByWord(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Backward : LogicalDirection.Forward;
			TextEditorSelection.NavigateWordLogicalDirection(textEditor, direction);
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x00265CB0 File Offset: 0x00264CB0
		private static void OnMoveLeftByWord(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Forward : LogicalDirection.Backward;
			TextEditorSelection.NavigateWordLogicalDirection(textEditor, direction);
		}

		// Token: 0x0600560F RID: 22031 RVA: 0x00265CF4 File Offset: 0x00264CF4
		private static void OnMoveDownByLine(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.Selection.End.ValidateLayout())
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer endInner = TextEditorSelection.GetEndInner(textEditor);
					textEditor.Selection.SetCaretToPosition(endInner, endInner.LogicalDirection, true, true);
					TextEditorSelection._ClearSuggestedX(textEditor);
				}
				Invariant.Assert(textEditor.Selection.IsEmpty);
				TextEditorSelection.AdjustCaretAtTableRowEnd(textEditor);
				ITextPointer textPointer;
				double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
				if (textPointer != null)
				{
					double num;
					int num2;
					ITextPointer positionAtNextLine = textEditor.TextView.GetPositionAtNextLine(textEditor.Selection.MovingPosition, suggestedX, 1, out num, out num2);
					Invariant.Assert(positionAtNextLine != null);
					if (num2 != 0)
					{
						TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, num);
						textEditor.Selection.SetCaretToPosition(positionAtNextLine, positionAtNextLine.LogicalDirection, true, true);
					}
					else if (TextPointerBase.IsInAnchoredBlock(textPointer))
					{
						ITextPointer positionAtLineEnd = TextEditorSelection.GetPositionAtLineEnd(textPointer);
						ITextPointer nextInsertionPosition = positionAtLineEnd.GetNextInsertionPosition(LogicalDirection.Forward);
						textEditor.Selection.SetCaretToPosition((nextInsertionPosition != null) ? nextInsertionPosition : positionAtLineEnd, textPointer.LogicalDirection, true, true);
					}
					else if (TextEditorSelection.IsPaginated(textEditor.TextView))
					{
						textEditor.TextView.BringLineIntoViewCompleted += TextEditorSelection.HandleMoveByLineCompleted;
						textEditor.TextView.BringLineIntoViewAsync(positionAtNextLine, num, 1, textEditor);
					}
					TextEditorTyping._BreakTypingSequence(textEditor);
					TextEditorSelection.ClearSpringloadFormatting(textEditor);
				}
			}
		}

		// Token: 0x06005610 RID: 22032 RVA: 0x00265E90 File Offset: 0x00264E90
		private static void OnMoveUpByLine(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.Selection.Start.ValidateLayout())
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer startInner = TextEditorSelection.GetStartInner(textEditor);
					textEditor.Selection.SetCaretToPosition(startInner, startInner.LogicalDirection, true, true);
					TextEditorSelection._ClearSuggestedX(textEditor);
				}
				Invariant.Assert(textEditor.Selection.IsEmpty);
				TextEditorSelection.AdjustCaretAtTableRowEnd(textEditor);
				ITextPointer textPointer;
				double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
				if (textPointer != null)
				{
					double num;
					int num2;
					ITextPointer positionAtNextLine = textEditor.TextView.GetPositionAtNextLine(textEditor.Selection.MovingPosition, suggestedX, -1, out num, out num2);
					Invariant.Assert(positionAtNextLine != null);
					if (num2 != 0)
					{
						TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, num);
						textEditor.Selection.SetCaretToPosition(positionAtNextLine, positionAtNextLine.LogicalDirection, true, true);
					}
					else if (TextPointerBase.IsInAnchoredBlock(textPointer))
					{
						ITextPointer positionAtLineStart = TextEditorSelection.GetPositionAtLineStart(textPointer);
						ITextPointer nextInsertionPosition = positionAtLineStart.GetNextInsertionPosition(LogicalDirection.Backward);
						textEditor.Selection.SetCaretToPosition((nextInsertionPosition != null) ? nextInsertionPosition : positionAtLineStart, textPointer.LogicalDirection, true, true);
					}
					else if (TextEditorSelection.IsPaginated(textEditor.TextView))
					{
						textEditor.TextView.BringLineIntoViewCompleted += TextEditorSelection.HandleMoveByLineCompleted;
						textEditor.TextView.BringLineIntoViewAsync(positionAtNextLine, num, -1, textEditor);
					}
					TextEditorTyping._BreakTypingSequence(textEditor);
					TextEditorSelection.ClearSpringloadFormatting(textEditor);
				}
			}
		}

		// Token: 0x06005611 RID: 22033 RVA: 0x0026602C File Offset: 0x0026502C
		private static void OnMoveDownByParagraph(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer endInner = TextEditorSelection.GetEndInner(textEditor);
					textEditor.Selection.SetCaretToPosition(endInner, endInner.LogicalDirection, false, false);
				}
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				ITextRange textRange = new TextRange(textPointer, textPointer);
				textRange.SelectParagraph(textPointer);
				textPointer.MoveToPosition(textRange.End);
				if (textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward))
				{
					textRange.SelectParagraph(textPointer);
					textEditor.Selection.SetCaretToPosition(textRange.Start, LogicalDirection.Backward, false, false);
				}
				else
				{
					textEditor.Selection.SetCaretToPosition(textRange.End, LogicalDirection.Backward, false, false);
				}
			}
		}

		// Token: 0x06005612 RID: 22034 RVA: 0x0026612C File Offset: 0x0026512C
		private static void OnMoveUpByParagraph(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer startInner = TextEditorSelection.GetStartInner(textEditor);
					textEditor.Selection.SetCaretToPosition(startInner, startInner.LogicalDirection, false, false);
				}
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				ITextRange textRange = new TextRange(textPointer, textPointer);
				textRange.SelectParagraph(textPointer);
				if (textEditor.Selection.Start.CompareTo(textRange.Start) > 0)
				{
					textEditor.Selection.SetCaretToPosition(textRange.Start, LogicalDirection.Backward, false, false);
				}
				else
				{
					textPointer.MoveToPosition(textRange.Start);
					if (textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward))
					{
						textRange.SelectParagraph(textPointer);
						textEditor.Selection.SetCaretToPosition(textRange.Start, LogicalDirection.Backward, false, false);
					}
				}
			}
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x00266244 File Offset: 0x00265244
		private static void OnMoveDownByPage(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.Selection.End.ValidateLayout())
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer endInner = TextEditorSelection.GetEndInner(textEditor);
					textEditor.Selection.SetCaretToPosition(endInner, endInner.LogicalDirection, true, true);
				}
				ITextPointer textPointer;
				double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
				if (textPointer != null)
				{
					double num = (double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
					if (num == 0.0)
					{
						if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							double suggestedYFromPosition = TextEditorSelection.GetSuggestedYFromPosition(textEditor, textPointer);
							Point suggestedOffset;
							int num2;
							ITextPointer textPointer2 = textEditor.TextView.GetPositionAtNextPage(textPointer, new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), suggestedYFromPosition), 1, out suggestedOffset, out num2);
							double x = suggestedOffset.X;
							Invariant.Assert(textPointer2 != null);
							if (num2 != 0)
							{
								TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, x);
								textEditor.Selection.SetCaretToPosition(textPointer2, textPointer2.LogicalDirection, true, false);
							}
							else if (TextEditorSelection.IsPaginated(textEditor.TextView))
							{
								textEditor.TextView.BringPageIntoViewCompleted += TextEditorSelection.HandleMoveByPageCompleted;
								textEditor.TextView.BringPageIntoViewAsync(textPointer2, suggestedOffset, 1, textEditor);
							}
						}
					}
					else
					{
						Rect rectangleFromTextPosition = textEditor.TextView.GetRectangleFromTextPosition(textPointer);
						Point point = new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), rectangleFromTextPosition.Top + num);
						ITextPointer textPointer2 = textEditor.TextView.GetTextPositionFromPoint(point, true);
						if (textPointer2 == null)
						{
							return;
						}
						if (textPointer2.CompareTo(textPointer) <= 0)
						{
							textPointer2 = textEditor.TextContainer.End;
							TextEditorSelection._ClearSuggestedX(textEditor);
						}
						ScrollBar.PageDownCommand.Execute(null, textEditor.TextView.RenderScope);
						textEditor.TextView.RenderScope.UpdateLayout();
						textEditor.Selection.SetCaretToPosition(textPointer2, textPointer2.LogicalDirection, true, false);
					}
					TextEditorTyping._BreakTypingSequence(textEditor);
					TextEditorSelection.ClearSpringloadFormatting(textEditor);
				}
			}
		}

		// Token: 0x06005614 RID: 22036 RVA: 0x00266480 File Offset: 0x00265480
		private static void OnMoveUpByPage(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.Selection.Start.ValidateLayout())
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer startInner = TextEditorSelection.GetStartInner(textEditor);
					textEditor.Selection.SetCaretToPosition(startInner, startInner.LogicalDirection, true, true);
				}
				ITextPointer textPointer;
				double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
				if (textPointer != null)
				{
					double num = (double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
					if (num == 0.0)
					{
						if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							double suggestedYFromPosition = TextEditorSelection.GetSuggestedYFromPosition(textEditor, textPointer);
							Point suggestedOffset;
							int num2;
							ITextPointer textPointer2 = textEditor.TextView.GetPositionAtNextPage(textPointer, new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), suggestedYFromPosition), -1, out suggestedOffset, out num2);
							double x = suggestedOffset.X;
							Invariant.Assert(textPointer2 != null);
							if (num2 != 0)
							{
								TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, x);
								textEditor.Selection.SetCaretToPosition(textPointer2, textPointer2.LogicalDirection, true, false);
							}
							else if (TextEditorSelection.IsPaginated(textEditor.TextView))
							{
								textEditor.TextView.BringPageIntoViewCompleted += TextEditorSelection.HandleMoveByPageCompleted;
								textEditor.TextView.BringPageIntoViewAsync(textPointer2, suggestedOffset, -1, textEditor);
							}
						}
					}
					else
					{
						Rect rectangleFromTextPosition = textEditor.TextView.GetRectangleFromTextPosition(textPointer);
						Point point = new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), rectangleFromTextPosition.Bottom - num);
						ITextPointer textPointer2 = textEditor.TextView.GetTextPositionFromPoint(point, true);
						if (textPointer2 == null)
						{
							return;
						}
						if (textPointer2.CompareTo(textPointer) >= 0)
						{
							textPointer2 = textEditor.TextContainer.Start;
							TextEditorSelection._ClearSuggestedX(textEditor);
						}
						ScrollBar.PageUpCommand.Execute(null, textEditor.TextView.RenderScope);
						textEditor.TextView.RenderScope.UpdateLayout();
						textEditor.Selection.SetCaretToPosition(textPointer2, textPointer2.LogicalDirection, true, false);
					}
					TextEditorTyping._BreakTypingSequence(textEditor);
					TextEditorSelection.ClearSpringloadFormatting(textEditor);
				}
			}
		}

		// Token: 0x06005615 RID: 22037 RVA: 0x002666BC File Offset: 0x002656BC
		private static void OnMoveToLineStart(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer startInner = TextEditorSelection.GetStartInner(textEditor);
			if (!startInner.ValidateLayout())
			{
				return;
			}
			TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, startInner);
			if (normalizedLineRange.IsNull)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer frozenPointer = normalizedLineRange.Start.GetFrozenPointer(LogicalDirection.Forward);
				textEditor.Selection.SetCaretToPosition(frozenPointer, LogicalDirection.Forward, true, true);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x00266770 File Offset: 0x00265770
		private static void OnMoveToLineEnd(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer endInner = TextEditorSelection.GetEndInner(textEditor);
			if (!endInner.ValidateLayout())
			{
				return;
			}
			TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, endInner);
			if (normalizedLineRange.IsNull)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				LogicalDirection logicalDirection = TextPointerBase.IsNextToPlainLineBreak(normalizedLineRange.End, LogicalDirection.Backward) ? LogicalDirection.Forward : LogicalDirection.Backward;
				ITextPointer frozenPointer = normalizedLineRange.End.GetFrozenPointer(logicalDirection);
				textEditor.Selection.SetCaretToPosition(frozenPointer, logicalDirection, true, true);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005617 RID: 22039 RVA: 0x0026683C File Offset: 0x0026583C
		private static void OnMoveToDocumentStart(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				textEditor.Selection.SetCaretToPosition(textEditor.TextContainer.Start, LogicalDirection.Forward, false, false);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005618 RID: 22040 RVA: 0x002668C4 File Offset: 0x002658C4
		private static void OnMoveToDocumentEnd(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				textEditor.Selection.SetCaretToPosition(textEditor.TextContainer.End, LogicalDirection.Backward, false, false);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005619 RID: 22041 RVA: 0x0026694C File Offset: 0x0026594C
		private static void OnSelectRightByCharacter(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Backward : LogicalDirection.Forward;
			TextEditorSelection.MoveToCharacterLogicalDirection(textEditor, direction, true);
		}

		// Token: 0x0600561A RID: 22042 RVA: 0x00266990 File Offset: 0x00265990
		private static void OnSelectLeftByCharacter(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Forward : LogicalDirection.Backward;
			TextEditorSelection.MoveToCharacterLogicalDirection(textEditor, direction, true);
		}

		// Token: 0x0600561B RID: 22043 RVA: 0x002669D4 File Offset: 0x002659D4
		private static void OnSelectRightByWord(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Backward : LogicalDirection.Forward;
			TextEditorSelection.ExtendWordLogicalDirection(textEditor, direction);
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x00266A18 File Offset: 0x00265A18
		private static void OnSelectLeftByWord(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Forward : LogicalDirection.Backward;
			TextEditorSelection.ExtendWordLogicalDirection(textEditor, direction);
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x00266A5C File Offset: 0x00265A5C
		private static void OnSelectDownByLine(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.ExtendToNextTableRow(LogicalDirection.Forward))
				{
					ITextPointer textPointer;
					double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
					if (textPointer == null)
					{
						return;
					}
					if (textEditor._NextLineAdvanceMovingPosition != null && textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead)
					{
						TextEditorSelection.ExtendSelectionAndBringIntoView(textEditor._NextLineAdvanceMovingPosition, textEditor);
						textEditor._NextLineAdvanceMovingPosition = null;
					}
					else
					{
						ITextPointer textPointer2 = TextEditorSelection.AdjustPositionAtTableRowEnd(textPointer);
						double num;
						int num2;
						textPointer2 = textEditor.TextView.GetPositionAtNextLine(textPointer2, suggestedX, 1, out num, out num2);
						Invariant.Assert(textPointer2 != null);
						if (num2 != 0)
						{
							TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, num);
							TextEditorSelection.AdjustMovingPositionForSelectDownByLine(textEditor, textPointer2, textPointer, num);
						}
						else if (TextPointerBase.IsInAnchoredBlock(textPointer))
						{
							ITextPointer positionAtLineEnd = TextEditorSelection.GetPositionAtLineEnd(textPointer);
							ITextPointer nextInsertionPosition = positionAtLineEnd.GetNextInsertionPosition(LogicalDirection.Forward);
							TextEditorSelection.ExtendSelectionAndBringIntoView((nextInsertionPosition != null) ? nextInsertionPosition : positionAtLineEnd, textEditor);
						}
						else if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							textEditor.TextView.BringLineIntoViewCompleted += TextEditorSelection.HandleSelectByLineCompleted;
							textEditor.TextView.BringLineIntoViewAsync(textPointer2, num, 1, textEditor);
						}
						else
						{
							if (textEditor._NextLineAdvanceMovingPosition == null)
							{
								textEditor._NextLineAdvanceMovingPosition = textPointer;
								textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead = false;
							}
							TextEditorSelection.ExtendSelectionAndBringIntoView(TextEditorSelection.GetPositionAtLineEnd(textPointer2), textEditor);
						}
					}
				}
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x00266BE4 File Offset: 0x00265BE4
		private static void AdjustMovingPositionForSelectDownByLine(TextEditor This, ITextPointer newMovingPosition, ITextPointer originalMovingPosition, double suggestedX)
		{
			int num = newMovingPosition.CompareTo(originalMovingPosition);
			if (num > 0 || (num == 0 && newMovingPosition.LogicalDirection != originalMovingPosition.LogicalDirection))
			{
				if (TextPointerBase.IsNextToAnyBreak(newMovingPosition, LogicalDirection.Forward) || newMovingPosition.GetNextInsertionPosition(LogicalDirection.Forward) == null)
				{
					double absoluteXOffset = TextEditorSelection.GetAbsoluteXOffset(This.TextView, newMovingPosition);
					FlowDirection scopingParagraphFlowDirection = TextEditorSelection.GetScopingParagraphFlowDirection(newMovingPosition);
					FlowDirection flowDirection = This.UiScope.FlowDirection;
					if ((scopingParagraphFlowDirection == flowDirection && absoluteXOffset < suggestedX) || (scopingParagraphFlowDirection != flowDirection && absoluteXOffset > suggestedX))
					{
						newMovingPosition = newMovingPosition.GetInsertionPosition(LogicalDirection.Forward);
						newMovingPosition = newMovingPosition.GetNextInsertionPosition(LogicalDirection.Forward);
						if (newMovingPosition == null)
						{
							newMovingPosition = originalMovingPosition.TextContainer.End;
						}
						newMovingPosition = newMovingPosition.GetFrozenPointer(LogicalDirection.Backward);
					}
				}
				TextEditorSelection.ExtendSelectionAndBringIntoView(newMovingPosition, This);
				return;
			}
			if (This._NextLineAdvanceMovingPosition == null)
			{
				This._NextLineAdvanceMovingPosition = originalMovingPosition;
				This._IsNextLineAdvanceMovingPositionAtDocumentHead = false;
			}
			newMovingPosition = TextEditorSelection.GetPositionAtLineEnd(originalMovingPosition);
			if (newMovingPosition.GetNextInsertionPosition(LogicalDirection.Forward) == null)
			{
				newMovingPosition = newMovingPosition.TextContainer.End;
			}
			TextEditorSelection.ExtendSelectionAndBringIntoView(newMovingPosition, This);
		}

		// Token: 0x0600561F RID: 22047 RVA: 0x00266CC4 File Offset: 0x00265CC4
		private static void OnSelectUpByLine(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.ExtendToNextTableRow(LogicalDirection.Backward))
				{
					ITextPointer textPointer;
					double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
					if (textPointer == null)
					{
						return;
					}
					if (textEditor._NextLineAdvanceMovingPosition != null && !textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead)
					{
						TextEditorSelection.ExtendSelectionAndBringIntoView(textEditor._NextLineAdvanceMovingPosition, textEditor);
						textEditor._NextLineAdvanceMovingPosition = null;
					}
					else
					{
						ITextPointer textPointer2 = TextEditorSelection.AdjustPositionAtTableRowEnd(textPointer);
						double num;
						int num2;
						textPointer2 = textEditor.TextView.GetPositionAtNextLine(textPointer2, suggestedX, -1, out num, out num2);
						Invariant.Assert(textPointer2 != null);
						if (num2 != 0)
						{
							TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, num);
							int num3 = textPointer2.CompareTo(textPointer);
							if (num3 < 0 || (num3 == 0 && textPointer2.LogicalDirection != textPointer.LogicalDirection))
							{
								TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
							}
							else
							{
								TextEditorSelection.ExtendSelectionAndBringIntoView(TextEditorSelection.GetPositionAtLineStart(textPointer), textEditor);
							}
						}
						else if (TextPointerBase.IsInAnchoredBlock(textPointer))
						{
							ITextPointer positionAtLineStart = TextEditorSelection.GetPositionAtLineStart(textPointer);
							ITextPointer nextInsertionPosition = positionAtLineStart.GetNextInsertionPosition(LogicalDirection.Backward);
							TextEditorSelection.ExtendSelectionAndBringIntoView((nextInsertionPosition != null) ? nextInsertionPosition : positionAtLineStart, textEditor);
						}
						else if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							textEditor.TextView.BringLineIntoViewCompleted += TextEditorSelection.HandleSelectByLineCompleted;
							textEditor.TextView.BringLineIntoViewAsync(textPointer2, num, -1, textEditor);
						}
						else
						{
							if (textEditor._NextLineAdvanceMovingPosition == null)
							{
								textEditor._NextLineAdvanceMovingPosition = textPointer;
								textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead = true;
							}
							TextEditorSelection.ExtendSelectionAndBringIntoView(TextEditorSelection.GetPositionAtLineStart(textPointer2), textEditor);
						}
					}
				}
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x00266E7C File Offset: 0x00265E7C
		private static void OnSelectDownByParagraph(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				ITextRange textRange = new TextRange(textPointer, textPointer);
				textRange.SelectParagraph(textPointer);
				textPointer.MoveToPosition(textRange.End);
				if (textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward))
				{
					textRange.SelectParagraph(textPointer);
					TextEditorSelection.ExtendSelectionAndBringIntoView(textRange.Start, textEditor);
				}
				else
				{
					TextEditorSelection.ExtendSelectionAndBringIntoView(textRange.End, textEditor);
				}
			}
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x00266F40 File Offset: 0x00265F40
		private static void OnSelectUpByParagraph(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				ITextRange textRange = new TextRange(textPointer, textPointer);
				textRange.SelectParagraph(textPointer);
				if (textPointer.CompareTo(textRange.Start) > 0)
				{
					TextEditorSelection.ExtendSelectionAndBringIntoView(textRange.Start, textEditor);
				}
				else
				{
					textPointer.MoveToPosition(textRange.Start);
					if (textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward))
					{
						textRange.SelectParagraph(textPointer);
						TextEditorSelection.ExtendSelectionAndBringIntoView(textRange.Start, textEditor);
					}
				}
			}
		}

		// Token: 0x06005622 RID: 22050 RVA: 0x00267014 File Offset: 0x00266014
		private static void OnSelectDownByPage(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer textPointer;
			double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
			if (textPointer == null)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				double num = (double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
				if (num == 0.0)
				{
					if (TextEditorSelection.IsPaginated(textEditor.TextView))
					{
						double suggestedYFromPosition = TextEditorSelection.GetSuggestedYFromPosition(textEditor, textPointer);
						Point suggestedOffset;
						int num2;
						ITextPointer textPointer2 = textEditor.TextView.GetPositionAtNextPage(textPointer, new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), suggestedYFromPosition), 1, out suggestedOffset, out num2);
						double x = suggestedOffset.X;
						Invariant.Assert(textPointer2 != null);
						if (num2 != 0)
						{
							TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, x);
							TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
						}
						else if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							textEditor.TextView.BringPageIntoViewCompleted += TextEditorSelection.HandleSelectByPageCompleted;
							textEditor.TextView.BringPageIntoViewAsync(textPointer2, suggestedOffset, 1, textEditor);
						}
						else
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2.TextContainer.End, textEditor);
						}
					}
				}
				else
				{
					Rect rectangleFromTextPosition = textEditor.TextView.GetRectangleFromTextPosition(textPointer);
					Point point = new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), rectangleFromTextPosition.Top + num);
					ITextPointer textPointer2 = textEditor.TextView.GetTextPositionFromPoint(point, true);
					if (textPointer2 == null)
					{
						return;
					}
					if (textPointer2.CompareTo(textPointer) <= 0)
					{
						textPointer2 = textEditor.TextContainer.End;
					}
					TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
					ScrollBar.PageDownCommand.Execute(null, textEditor.TextView.RenderScope);
				}
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005623 RID: 22051 RVA: 0x002671EC File Offset: 0x002661EC
		private static void OnSelectUpByPage(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer textPointer;
			double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
			if (textPointer == null)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				double num = (double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
				if (num == 0.0)
				{
					if (TextEditorSelection.IsPaginated(textEditor.TextView))
					{
						double suggestedYFromPosition = TextEditorSelection.GetSuggestedYFromPosition(textEditor, textPointer);
						Point suggestedOffset;
						int num2;
						ITextPointer textPointer2 = textEditor.TextView.GetPositionAtNextPage(textPointer, new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), suggestedYFromPosition), -1, out suggestedOffset, out num2);
						double x = suggestedOffset.X;
						Invariant.Assert(textPointer2 != null);
						if (num2 != 0)
						{
							TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, x);
							TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
						}
						else if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							textEditor.TextView.BringPageIntoViewCompleted += TextEditorSelection.HandleSelectByPageCompleted;
							textEditor.TextView.BringPageIntoViewAsync(textPointer2, suggestedOffset, -1, textEditor);
						}
						else
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2.TextContainer.Start, textEditor);
						}
					}
				}
				else
				{
					Rect rectangleFromTextPosition = textEditor.TextView.GetRectangleFromTextPosition(textPointer);
					Point point = new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), rectangleFromTextPosition.Bottom - num);
					ITextPointer textPointer2 = textEditor.TextView.GetTextPositionFromPoint(point, true);
					if (textPointer2 == null)
					{
						return;
					}
					if (textPointer2.CompareTo(textPointer) >= 0)
					{
						textPointer2 = textEditor.TextContainer.Start;
					}
					TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
					ScrollBar.PageUpCommand.Execute(null, textEditor.TextView.RenderScope);
				}
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005624 RID: 22052 RVA: 0x002673C4 File Offset: 0x002663C4
		private static void OnSelectToLineStart(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer movingPositionInner = TextEditorSelection.GetMovingPositionInner(textEditor);
			if (!movingPositionInner.ValidateLayout())
			{
				return;
			}
			TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, movingPositionInner);
			if (normalizedLineRange.IsNull)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection.ExtendSelectionAndBringIntoView(normalizedLineRange.Start.CreatePointer(LogicalDirection.Forward), textEditor);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005625 RID: 22053 RVA: 0x0026746C File Offset: 0x0026646C
		private static void OnSelectToLineEnd(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer movingPositionInner = TextEditorSelection.GetMovingPositionInner(textEditor);
			if (!movingPositionInner.ValidateLayout())
			{
				return;
			}
			TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, movingPositionInner);
			if (normalizedLineRange.IsNull)
			{
				return;
			}
			if (normalizedLineRange.End.CompareTo(textEditor.Selection.End) < 0)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer textPointer = normalizedLineRange.End;
				if (TextPointerBase.IsNextToPlainLineBreak(textPointer, LogicalDirection.Forward) || TextPointerBase.IsNextToRichLineBreak(textPointer, LogicalDirection.Forward))
				{
					if (textEditor.Selection.AnchorPosition.ValidateLayout())
					{
						TextSegment normalizedLineRange2 = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, textEditor.Selection.AnchorPosition);
						if (!normalizedLineRange.IsNull && normalizedLineRange2.Start.CompareTo(textEditor.Selection.AnchorPosition) == 0)
						{
							textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
						}
					}
				}
				else if (TextPointerBase.IsNextToParagraphBreak(textPointer, LogicalDirection.Forward) && TextPointerBase.IsNextToParagraphBreak(textEditor.Selection.AnchorPosition, LogicalDirection.Backward))
				{
					ITextPointer nextInsertionPosition = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
					if (nextInsertionPosition == null)
					{
						textPointer = textPointer.TextContainer.End;
					}
					else
					{
						textPointer = nextInsertionPosition;
					}
				}
				textPointer = textPointer.GetFrozenPointer(LogicalDirection.Backward);
				TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer, textEditor);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005626 RID: 22054 RVA: 0x002675E8 File Offset: 0x002665E8
		private static void OnSelectToDocumentStart(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection.ExtendSelectionAndBringIntoView(textEditor.TextContainer.Start, textEditor);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x00267668 File Offset: 0x00266668
		private static void OnSelectToDocumentEnd(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection.ExtendSelectionAndBringIntoView(textEditor.TextContainer.End, textEditor);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x002676E8 File Offset: 0x002666E8
		private static void HandleMoveByLineCompleted(object sender, BringLineIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringLineIntoViewCompleted -= TextEditorSelection.HandleMoveByLineCompleted;
			if (e != null && !e.Cancelled && e.Error == null)
			{
				TextEditor textEditor = e.UserState as TextEditor;
				if (textEditor == null || !textEditor._IsEnabled)
				{
					return;
				}
				TextEditorTyping._FlushPendingInputItems(textEditor);
				using (textEditor.Selection.DeclareChangeBlock())
				{
					TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, e.NewSuggestedX);
					textEditor.Selection.SetCaretToPosition(e.NewPosition, e.NewPosition.LogicalDirection, true, true);
				}
			}
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x002677A0 File Offset: 0x002667A0
		private static void HandleMoveByPageCompleted(object sender, BringPageIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringPageIntoViewCompleted -= TextEditorSelection.HandleMoveByPageCompleted;
			if (e != null && !e.Cancelled && e.Error == null)
			{
				TextEditor textEditor = e.UserState as TextEditor;
				if (textEditor == null || !textEditor._IsEnabled)
				{
					return;
				}
				TextEditorTyping._FlushPendingInputItems(textEditor);
				using (textEditor.Selection.DeclareChangeBlock())
				{
					TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, e.NewSuggestedOffset.X);
					textEditor.Selection.SetCaretToPosition(e.NewPosition, e.NewPosition.LogicalDirection, true, true);
				}
			}
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x00267860 File Offset: 0x00266860
		private static void HandleSelectByLineCompleted(object sender, BringLineIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringLineIntoViewCompleted -= TextEditorSelection.HandleSelectByLineCompleted;
			if (e != null && !e.Cancelled && e.Error == null)
			{
				TextEditor textEditor = e.UserState as TextEditor;
				if (textEditor == null || !textEditor._IsEnabled)
				{
					return;
				}
				TextEditorTyping._FlushPendingInputItems(textEditor);
				using (textEditor.Selection.DeclareChangeBlock())
				{
					TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, e.NewSuggestedX);
					int num = e.NewPosition.CompareTo(e.Position);
					if (e.Count < 0)
					{
						if (num < 0 || (num == 0 && e.NewPosition.LogicalDirection != e.Position.LogicalDirection))
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition, textEditor);
						}
						else
						{
							if (textEditor._NextLineAdvanceMovingPosition == null)
							{
								textEditor._NextLineAdvanceMovingPosition = e.Position;
								textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead = true;
							}
							TextEditorSelection.ExtendSelectionAndBringIntoView(TextEditorSelection.GetPositionAtLineStart(e.NewPosition), textEditor);
						}
					}
					else
					{
						TextEditorSelection.AdjustMovingPositionForSelectDownByLine(textEditor, e.NewPosition, e.Position, e.NewSuggestedX);
					}
				}
			}
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x00267990 File Offset: 0x00266990
		private static void HandleSelectByPageCompleted(object sender, BringPageIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringPageIntoViewCompleted -= TextEditorSelection.HandleSelectByPageCompleted;
			if (e != null && !e.Cancelled && e.Error == null)
			{
				TextEditor textEditor = e.UserState as TextEditor;
				if (textEditor == null || !textEditor._IsEnabled)
				{
					return;
				}
				TextEditorTyping._FlushPendingInputItems(textEditor);
				using (textEditor.Selection.DeclareChangeBlock())
				{
					TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, e.NewSuggestedOffset.X);
					int num = e.NewPosition.CompareTo(e.Position);
					if (e.Count < 0)
					{
						if (num < 0)
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition, textEditor);
						}
						else
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition.TextContainer.Start, textEditor);
						}
					}
					else if (num > 0)
					{
						TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition, textEditor);
					}
					else
					{
						TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition.TextContainer.End, textEditor);
					}
				}
			}
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x00267AA8 File Offset: 0x00266AA8
		private static void OnQueryStatusKeyboardSelection(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x00267AD0 File Offset: 0x00266AD0
		private static void OnQueryStatusCaretNavigation(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void OnNYICommand(object source, ExecutedRoutedEventArgs args)
		{
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x00267B08 File Offset: 0x00266B08
		private static void ClearSpringloadFormatting(TextEditor This)
		{
			if (This.Selection is TextSelection)
			{
				((TextSelection)This.Selection).ClearSpringloadFormatting();
			}
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x00267B28 File Offset: 0x00266B28
		private static bool IsFlowDirectionRightToLeftThenTopToBottom(TextEditor textEditor)
		{
			Invariant.Assert(textEditor != null);
			ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
			while (TextSchema.IsFormattingType(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
			}
			return (FlowDirection)textPointer.GetValue(FlowDocument.FlowDirectionProperty) == FlowDirection.RightToLeft;
		}

		// Token: 0x06005631 RID: 22065 RVA: 0x00267B78 File Offset: 0x00266B78
		private static void MoveToCharacterLogicalDirection(TextEditor textEditor, LogicalDirection direction, bool extend)
		{
			Invariant.Assert(textEditor != null);
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (extend)
				{
					textEditor.Selection.ExtendToNextInsertionPosition(direction);
					TextEditorSelection.BringIntoView(textEditor.Selection.MovingPosition, textEditor);
				}
				else
				{
					ITextPointer textPointer = (direction == LogicalDirection.Forward) ? textEditor.Selection.End : textEditor.Selection.Start;
					if (textEditor.Selection.IsEmpty)
					{
						textPointer = textPointer.GetNextInsertionPosition(direction);
					}
					if (textPointer != null)
					{
						LogicalDirection direction2 = (direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward;
						textPointer = textPointer.GetInsertionPosition(direction2);
						textEditor.Selection.SetCaretToPosition(textPointer, direction2, false, false);
					}
				}
				textEditor.Selection.OnCaretNavigation();
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x00267C54 File Offset: 0x00266C54
		private static void NavigateWordLogicalDirection(TextEditor textEditor, LogicalDirection direction)
		{
			Invariant.Assert(textEditor != null);
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				if (direction == LogicalDirection.Forward)
				{
					if (!textEditor.Selection.IsEmpty && TextPointerBase.IsAtWordBoundary(textEditor.Selection.End, LogicalDirection.Forward))
					{
						textEditor.Selection.SetCaretToPosition(textEditor.Selection.End, LogicalDirection.Backward, false, false);
					}
					else
					{
						ITextPointer textPointer = textEditor.Selection.End.CreatePointer();
						TextPointerBase.MoveToNextWordBoundary(textPointer, LogicalDirection.Forward);
						textEditor.Selection.SetCaretToPosition(textPointer, LogicalDirection.Backward, false, false);
					}
				}
				else if (!textEditor.Selection.IsEmpty && TextPointerBase.IsAtWordBoundary(textEditor.Selection.Start, LogicalDirection.Forward))
				{
					textEditor.Selection.SetCaretToPosition(textEditor.Selection.Start, LogicalDirection.Forward, false, false);
				}
				else
				{
					ITextPointer textPointer2 = textEditor.Selection.Start.CreatePointer();
					TextPointerBase.MoveToNextWordBoundary(textPointer2, LogicalDirection.Backward);
					textEditor.Selection.SetCaretToPosition(textPointer2, LogicalDirection.Forward, false, false);
				}
				textEditor.Selection.OnCaretNavigation();
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x00267DA4 File Offset: 0x00266DA4
		private static void ExtendWordLogicalDirection(TextEditor textEditor, LogicalDirection direction)
		{
			Invariant.Assert(textEditor != null);
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				TextPointerBase.MoveToNextWordBoundary(textPointer, direction);
				textPointer.SetLogicalDirection((direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward);
				TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer, textEditor);
				textEditor.Selection.OnCaretNavigation();
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x00267E44 File Offset: 0x00266E44
		private static double GetSuggestedX(TextEditor This, out ITextPointer innerMovingPosition)
		{
			innerMovingPosition = TextEditorSelection.GetMovingPositionInner(This);
			if (!innerMovingPosition.ValidateLayout())
			{
				innerMovingPosition = null;
				return double.NaN;
			}
			if (double.IsNaN(This._suggestedX))
			{
				This._suggestedX = TextEditorSelection.GetAbsoluteXOffset(This.TextView, innerMovingPosition);
				if (This.Selection.MovingPosition.CompareTo(innerMovingPosition) > 0)
				{
					double num = (double)innerMovingPosition.GetValue(TextElement.FontSizeProperty) * 0.5;
					FlowDirection scopingParagraphFlowDirection = TextEditorSelection.GetScopingParagraphFlowDirection(innerMovingPosition);
					FlowDirection flowDirection = This.UiScope.FlowDirection;
					if (scopingParagraphFlowDirection != flowDirection)
					{
						num = -num;
					}
					This._suggestedX += num;
				}
			}
			return This._suggestedX;
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x00267EF0 File Offset: 0x00266EF0
		private static double GetSuggestedYFromPosition(TextEditor This, ITextPointer position)
		{
			double result = double.NaN;
			if (position != null)
			{
				result = This.TextView.GetRectangleFromTextPosition(position).Y;
			}
			return result;
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x00267F20 File Offset: 0x00266F20
		private static void UpdateSuggestedXOnColumnOrPageBoundary(TextEditor This, double newSuggestedX)
		{
			if (This._suggestedX != newSuggestedX)
			{
				This._suggestedX = newSuggestedX;
			}
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x00267F34 File Offset: 0x00266F34
		private static ITextPointer GetMovingPositionInner(TextEditor This)
		{
			ITextPointer textPointer = This.Selection.MovingPosition;
			if (!(textPointer is DocumentSequenceTextPointer) && !(textPointer is FixedTextPointer) && textPointer.LogicalDirection == LogicalDirection.Backward && This.Selection.Start.CompareTo(textPointer) < 0 && TextPointerBase.IsNextToAnyBreak(textPointer, LogicalDirection.Backward))
			{
				textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Backward);
				if (TextPointerBase.IsNextToPlainLineBreak(textPointer, LogicalDirection.Backward))
				{
					textPointer = textPointer.GetFrozenPointer(LogicalDirection.Forward);
				}
			}
			else if (TextPointerBase.IsAfterLastParagraph(textPointer))
			{
				textPointer = textPointer.GetInsertionPosition(textPointer.LogicalDirection);
			}
			return textPointer;
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x00267FB3 File Offset: 0x00266FB3
		private static ITextPointer GetStartInner(TextEditor This)
		{
			if (!This.Selection.IsEmpty)
			{
				return This.Selection.Start.GetFrozenPointer(LogicalDirection.Forward);
			}
			return This.Selection.Start;
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x00267FE0 File Offset: 0x00266FE0
		private static ITextPointer GetEndInner(TextEditor This)
		{
			ITextPointer textPointer = This.Selection.End;
			if (textPointer.CompareTo(This.Selection.MovingPosition) == 0)
			{
				textPointer = TextEditorSelection.GetMovingPositionInner(This);
			}
			return textPointer;
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x00268014 File Offset: 0x00267014
		private static ITextPointer GetPositionAtLineStart(ITextPointer position)
		{
			TextSegment lineRange = position.TextContainer.TextView.GetLineRange(position);
			if (!lineRange.IsNull)
			{
				return lineRange.Start;
			}
			return position;
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x00268048 File Offset: 0x00267048
		private static ITextPointer GetPositionAtLineEnd(ITextPointer position)
		{
			TextSegment lineRange = position.TextContainer.TextView.GetLineRange(position);
			if (!lineRange.IsNull)
			{
				return lineRange.End;
			}
			return position;
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x00268079 File Offset: 0x00267079
		private static void ExtendSelectionAndBringIntoView(ITextPointer position, TextEditor textEditor)
		{
			textEditor.Selection.ExtendToPosition(position);
			TextEditorSelection.BringIntoView(position, textEditor);
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x00268090 File Offset: 0x00267090
		private static void BringIntoView(ITextPointer position, TextEditor textEditor)
		{
			if ((double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty) == 0.0 && textEditor.TextView != null && textEditor.TextView.IsValid && !textEditor.TextView.Contains(position) && TextEditorSelection.IsPaginated(textEditor.TextView))
			{
				textEditor.TextView.BringPositionIntoViewAsync(position, textEditor);
			}
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x002680FC File Offset: 0x002670FC
		private static void AdjustCaretAtTableRowEnd(TextEditor This)
		{
			if (This.Selection.IsEmpty && TextPointerBase.IsAtRowEnd(This.Selection.Start))
			{
				ITextPointer nextInsertionPosition = This.Selection.Start.GetNextInsertionPosition(LogicalDirection.Backward);
				if (nextInsertionPosition != null)
				{
					This.Selection.SetCaretToPosition(nextInsertionPosition, LogicalDirection.Forward, false, false);
				}
			}
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x0026814C File Offset: 0x0026714C
		private static ITextPointer AdjustPositionAtTableRowEnd(ITextPointer position)
		{
			if (TextPointerBase.IsAtRowEnd(position))
			{
				ITextPointer nextInsertionPosition = position.GetNextInsertionPosition(LogicalDirection.Backward);
				if (nextInsertionPosition != null)
				{
					position = nextInsertionPosition;
				}
			}
			return position;
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x00268170 File Offset: 0x00267170
		private static FlowDirection GetScopingParagraphFlowDirection(ITextPointer position)
		{
			ITextPointer textPointer = position.CreatePointer();
			while (typeof(Inline).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			}
			return (FlowDirection)textPointer.GetValue(FrameworkElement.FlowDirectionProperty);
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x002681B4 File Offset: 0x002671B4
		private static double GetAbsoluteXOffset(ITextView textview, ITextPointer position)
		{
			double num = textview.GetRectangleFromTextPosition(position).X;
			if (textview is TextBoxView)
			{
				IScrollInfo scrollInfo = textview as IScrollInfo;
				if (scrollInfo != null)
				{
					num += scrollInfo.HorizontalOffset;
				}
			}
			return num;
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x002681F0 File Offset: 0x002671F0
		private static double GetViewportXOffset(ITextView textview, double suggestedX)
		{
			if (textview is TextBoxView)
			{
				IScrollInfo scrollInfo = textview as IScrollInfo;
				if (scrollInfo != null)
				{
					suggestedX -= scrollInfo.HorizontalOffset;
				}
			}
			return suggestedX;
		}

		// Token: 0x04002F63 RID: 12131
		private const string KeyMoveDownByLine = "Down";

		// Token: 0x04002F64 RID: 12132
		private const string KeyMoveDownByPage = "PageDown";

		// Token: 0x04002F65 RID: 12133
		private const string KeyMoveDownByParagraph = "Ctrl+Down";

		// Token: 0x04002F66 RID: 12134
		private const string KeyMoveLeftByCharacter = "Left";

		// Token: 0x04002F67 RID: 12135
		private const string KeyMoveLeftByWord = "Ctrl+Left";

		// Token: 0x04002F68 RID: 12136
		private const string KeyMoveRightByCharacter = "Right";

		// Token: 0x04002F69 RID: 12137
		private const string KeyMoveRightByWord = "Ctrl+Right";

		// Token: 0x04002F6A RID: 12138
		private const string KeyMoveToColumnEnd = "Alt+PageDown";

		// Token: 0x04002F6B RID: 12139
		private const string KeyMoveToColumnStart = "Alt+PageUp";

		// Token: 0x04002F6C RID: 12140
		private const string KeyMoveToDocumentEnd = "Ctrl+End";

		// Token: 0x04002F6D RID: 12141
		private const string KeyMoveToDocumentStart = "Ctrl+Home";

		// Token: 0x04002F6E RID: 12142
		private const string KeyMoveToLineEnd = "End";

		// Token: 0x04002F6F RID: 12143
		private const string KeyMoveToLineStart = "Home";

		// Token: 0x04002F70 RID: 12144
		private const string KeyMoveToWindowBottom = "Alt+Ctrl+PageDown";

		// Token: 0x04002F71 RID: 12145
		private const string KeyMoveToWindowTop = "Alt+Ctrl+PageUp";

		// Token: 0x04002F72 RID: 12146
		private const string KeyMoveUpByLine = "Up";

		// Token: 0x04002F73 RID: 12147
		private const string KeyMoveUpByPage = "PageUp";

		// Token: 0x04002F74 RID: 12148
		private const string KeyMoveUpByParagraph = "Ctrl+Up";

		// Token: 0x04002F75 RID: 12149
		private const string KeySelectAll = "Ctrl+A";

		// Token: 0x04002F76 RID: 12150
		private const string KeySelectDownByLine = "Shift+Down";

		// Token: 0x04002F77 RID: 12151
		private const string KeySelectDownByPage = "Shift+PageDown";

		// Token: 0x04002F78 RID: 12152
		private const string KeySelectDownByParagraph = "Ctrl+Shift+Down";

		// Token: 0x04002F79 RID: 12153
		private const string KeySelectLeftByCharacter = "Shift+Left";

		// Token: 0x04002F7A RID: 12154
		private const string KeySelectLeftByWord = "Ctrl+Shift+Left";

		// Token: 0x04002F7B RID: 12155
		private const string KeySelectRightByCharacter = "Shift+Right";

		// Token: 0x04002F7C RID: 12156
		private const string KeySelectRightByWord = "Ctrl+Shift+Right";

		// Token: 0x04002F7D RID: 12157
		private const string KeySelectToColumnEnd = "Alt+Shift+PageDown";

		// Token: 0x04002F7E RID: 12158
		private const string KeySelectToColumnStart = "Alt+Shift+PageUp";

		// Token: 0x04002F7F RID: 12159
		private const string KeySelectToDocumentEnd = "Ctrl+Shift+End";

		// Token: 0x04002F80 RID: 12160
		private const string KeySelectToDocumentStart = "Ctrl+Shift+Home";

		// Token: 0x04002F81 RID: 12161
		private const string KeySelectToLineEnd = "Shift+End";

		// Token: 0x04002F82 RID: 12162
		private const string KeySelectToLineStart = "Shift+Home";

		// Token: 0x04002F83 RID: 12163
		private const string KeySelectToWindowBottom = "Alt+Ctrl+Shift+PageDown";

		// Token: 0x04002F84 RID: 12164
		private const string KeySelectToWindowTop = "Alt+Ctrl+Shift+PageUp";

		// Token: 0x04002F85 RID: 12165
		private const string KeySelectUpByLine = "Shift+Up";

		// Token: 0x04002F86 RID: 12166
		private const string KeySelectUpByPage = "Shift+PageUp";

		// Token: 0x04002F87 RID: 12167
		private const string KeySelectUpByParagraph = "Ctrl+Shift+Up";
	}
}
