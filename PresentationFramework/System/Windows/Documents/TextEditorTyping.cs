using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Internal.Interop;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006AA RID: 1706
	internal static class TextEditorTyping
	{
		// Token: 0x06005660 RID: 22112 RVA: 0x002689EC File Offset: 0x002679EC
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			if (registerEventListeners)
			{
				EventManager.RegisterClassHandler(controlType, Keyboard.PreviewKeyDownEvent, new KeyEventHandler(TextEditorTyping.OnPreviewKeyDown));
				EventManager.RegisterClassHandler(controlType, Keyboard.KeyDownEvent, new KeyEventHandler(TextEditorTyping.OnKeyDown));
				EventManager.RegisterClassHandler(controlType, Keyboard.KeyUpEvent, new KeyEventHandler(TextEditorTyping.OnKeyUp));
				EventManager.RegisterClassHandler(controlType, TextCompositionManager.TextInputEvent, new TextCompositionEventHandler(TextEditorTyping.OnTextInput));
			}
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(TextEditorTyping.OnEnterBreak);
			ExecutedRoutedEventHandler executedRoutedEventHandler2 = new ExecutedRoutedEventHandler(TextEditorTyping.OnSpace);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusNYI);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler2 = new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusEnterBreak);
			EventManager.RegisterClassHandler(controlType, Mouse.MouseMoveEvent, new MouseEventHandler(TextEditorTyping.OnMouseMove), true);
			EventManager.RegisterClassHandler(controlType, Mouse.MouseLeaveEvent, new MouseEventHandler(TextEditorTyping.OnMouseLeave), true);
			CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.CorrectionList, new ExecutedRoutedEventHandler(TextEditorTyping.OnCorrectionList), new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusCorrectionList), "KeyCorrectionList", "KeyCorrectionListDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleInsert, new ExecutedRoutedEventHandler(TextEditorTyping.OnToggleInsert), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Insert", "KeyToggleInsertDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.Delete, new ExecutedRoutedEventHandler(TextEditorTyping.OnDelete), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Delete", "KeyDeleteDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DeleteNextWord, new ExecutedRoutedEventHandler(TextEditorTyping.OnDeleteNextWord), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+Delete", "KeyDeleteNextWordDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DeletePreviousWord, new ExecutedRoutedEventHandler(TextEditorTyping.OnDeletePreviousWord), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+Backspace", "KeyDeletePreviousWordDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.EnterParagraphBreak, executedRoutedEventHandler, canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Enter", "KeyEnterParagraphBreakDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.EnterLineBreak, executedRoutedEventHandler, canExecuteRoutedEventHandler2, KeyGesture.CreateFromResourceStrings("Shift+Enter", "KeyEnterLineBreakDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.TabForward, new ExecutedRoutedEventHandler(TextEditorTyping.OnTabForward), new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusTabForward), KeyGesture.CreateFromResourceStrings("Tab", "KeyTabForwardDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.TabBackward, new ExecutedRoutedEventHandler(TextEditorTyping.OnTabBackward), new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusTabBackward), KeyGesture.CreateFromResourceStrings("Shift+Tab", "KeyTabBackwardDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.Space, executedRoutedEventHandler2, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Space", "KeySpaceDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ShiftSpace, executedRoutedEventHandler2, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Shift+Space", "KeyShiftSpaceDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.Backspace, new ExecutedRoutedEventHandler(TextEditorTyping.OnBackspace), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Backspace", SR.Get("KeyBackspaceDisplayString")), KeyGesture.CreateFromResourceStrings("Shift+Backspace", SR.Get("KeyShiftBackspaceDisplayString")));
		}

		// Token: 0x06005661 RID: 22113 RVA: 0x00268C9C File Offset: 0x00267C9C
		internal static void _AddInputLanguageChangedEventHandler(TextEditor This)
		{
			Invariant.Assert(This._dispatcher == null);
			This._dispatcher = Dispatcher.CurrentDispatcher;
			Invariant.Assert(This._dispatcher != null);
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (threadLocalStore.InputLanguageChangeEventHandlerCount == 0)
			{
				InputLanguageManager.Current.InputLanguageChanged += TextEditorTyping.OnInputLanguageChanged;
				Dispatcher.CurrentDispatcher.ShutdownFinished += TextEditorTyping.OnDispatcherShutdownFinished;
			}
			int inputLanguageChangeEventHandlerCount = threadLocalStore.InputLanguageChangeEventHandlerCount;
			threadLocalStore.InputLanguageChangeEventHandlerCount = inputLanguageChangeEventHandlerCount + 1;
		}

		// Token: 0x06005662 RID: 22114 RVA: 0x00268D18 File Offset: 0x00267D18
		internal static void _RemoveInputLanguageChangedEventHandler(TextEditor This)
		{
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			int inputLanguageChangeEventHandlerCount = threadLocalStore.InputLanguageChangeEventHandlerCount;
			threadLocalStore.InputLanguageChangeEventHandlerCount = inputLanguageChangeEventHandlerCount - 1;
			if (threadLocalStore.InputLanguageChangeEventHandlerCount == 0)
			{
				InputLanguageManager.Current.InputLanguageChanged -= TextEditorTyping.OnInputLanguageChanged;
				Dispatcher.CurrentDispatcher.ShutdownFinished -= TextEditorTyping.OnDispatcherShutdownFinished;
			}
		}

		// Token: 0x06005663 RID: 22115 RVA: 0x00268D6D File Offset: 0x00267D6D
		internal static void _BreakTypingSequence(TextEditor This)
		{
			This._typingUndoUnit = null;
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x00268D78 File Offset: 0x00267D78
		internal static void _FlushPendingInputItems(TextEditor This)
		{
			if (This.TextView != null)
			{
				This.TextView.ThrottleBackgroundTasksForUserInput();
			}
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (threadLocalStore.PendingInputItems != null)
			{
				try
				{
					for (int i = 0; i < threadLocalStore.PendingInputItems.Count; i++)
					{
						((TextEditorTyping.InputItem)threadLocalStore.PendingInputItems[i]).Do();
						threadLocalStore.PureControlShift = false;
					}
				}
				finally
				{
					threadLocalStore.PendingInputItems.Clear();
				}
			}
			threadLocalStore.PureControlShift = false;
		}

		// Token: 0x06005665 RID: 22117 RVA: 0x00268E00 File Offset: 0x00267E00
		internal static void _ShowCursor()
		{
			if (TextEditor._ThreadLocalStore.HideCursor)
			{
				TextEditor._ThreadLocalStore.HideCursor = false;
				SafeNativeMethods.ShowCursor(true);
			}
		}

		// Token: 0x06005666 RID: 22118 RVA: 0x00268E20 File Offset: 0x00267E20
		internal static void OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.ImeProcessed)
			{
				return;
			}
			RichTextBox richTextBox = sender as RichTextBox;
			if (richTextBox == null)
			{
				return;
			}
			TextEditor textEditor = richTextBox.TextEditor;
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			if (e.IsRepeat)
			{
				return;
			}
			if (textEditor.TextStore == null || textEditor.TextStore.IsComposing)
			{
				return;
			}
			if (richTextBox.Selection.IsEmpty)
			{
				return;
			}
			textEditor.SetText(textEditor.Selection, string.Empty, InputLanguageManager.Current.CurrentInputLanguage);
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x00268EB8 File Offset: 0x00267EB8
		internal static void OnKeyDown(object sender, KeyEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			if (e.IsRepeat)
			{
				return;
			}
			textEditor.CloseToolTip();
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			threadLocalStore.PureControlShift = false;
			if (textEditor.TextView != null && !textEditor.UiScope.IsMouseCaptured)
			{
				if ((e.Key == Key.RightShift || e.Key == Key.LeftShift) && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.None)
				{
					threadLocalStore.PureControlShift = true;
					return;
				}
				if ((e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl) && (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.None)
				{
					threadLocalStore.PureControlShift = true;
					return;
				}
				if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)
				{
					TextEditorTyping.UpdateHyperlinkCursor(textEditor);
				}
			}
		}

		// Token: 0x06005668 RID: 22120 RVA: 0x00268FBC File Offset: 0x00267FBC
		internal static void OnKeyUp(object sender, KeyEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			Key key = e.Key;
			if (key - Key.LeftShift > 1)
			{
				if (key - Key.LeftCtrl > 1)
				{
					return;
				}
				TextEditorTyping.UpdateHyperlinkCursor(textEditor);
			}
			else if (TextEditor._ThreadLocalStore.PureControlShift && (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.None)
			{
				TextEditorTyping.ScheduleInput(textEditor, new TextEditorTyping.KeyUpInputItem(textEditor, e.Key, e.KeyboardDevice.Modifiers));
				return;
			}
		}

		// Token: 0x06005669 RID: 22121 RVA: 0x00269050 File Offset: 0x00268050
		internal static void OnTextInput(object sender, TextCompositionEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			FrameworkTextComposition frameworkTextComposition = e.TextComposition as FrameworkTextComposition;
			if (frameworkTextComposition == null && (e.Text == null || e.Text.Length == 0))
			{
				return;
			}
			e.Handled = true;
			if (textEditor.TextView != null)
			{
				textEditor.TextView.ThrottleBackgroundTasksForUserInput();
			}
			if (frameworkTextComposition != null)
			{
				if (frameworkTextComposition.Owner == textEditor.TextStore)
				{
					textEditor.TextStore.UpdateCompositionText(frameworkTextComposition);
					return;
				}
				if (frameworkTextComposition.Owner == textEditor.ImmComposition)
				{
					textEditor.ImmComposition.UpdateCompositionText(frameworkTextComposition);
					return;
				}
			}
			else
			{
				KeyboardDevice keyboardDevice = e.Device as KeyboardDevice;
				TextEditorTyping.ScheduleInput(textEditor, new TextEditorTyping.TextInputItem(textEditor, e.Text, keyboardDevice != null && keyboardDevice.IsKeyToggled(Key.Insert)));
			}
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x0026912C File Offset: 0x0026812C
		private static void OnQueryStatusCorrectionList(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			if (textEditor.TextStore != null)
			{
				args.CanExecute = textEditor.TextStore.QueryRangeOrReconvertSelection(false);
				return;
			}
			args.CanExecute = false;
		}

		// Token: 0x0600566B RID: 22123 RVA: 0x00269168 File Offset: 0x00268168
		private static void OnCorrectionList(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			if (textEditor.TextStore != null)
			{
				textEditor.TextStore.QueryRangeOrReconvertSelection(true);
			}
		}

		// Token: 0x0600566C RID: 22124 RVA: 0x00269198 File Offset: 0x00268198
		private static void OnToggleInsert(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			textEditor._OvertypeMode = !textEditor._OvertypeMode;
			if (TextServicesLoader.ServicesInstalled && textEditor.TextStore != null && TextServicesHost.Current != null)
			{
				if (textEditor._OvertypeMode)
				{
					IInputElement inputElement = target as IInputElement;
					if (inputElement != null)
					{
						PresentationSource.AddSourceChangedHandler(inputElement, new SourceChangedEventHandler(TextEditorTyping.OnSourceChanged));
					}
					TextServicesHost.StartTransitoryExtension(textEditor.TextStore);
					return;
				}
				IInputElement inputElement2 = target as IInputElement;
				if (inputElement2 != null)
				{
					PresentationSource.RemoveSourceChangedHandler(inputElement2, new SourceChangedEventHandler(TextEditorTyping.OnSourceChanged));
				}
				TextServicesHost.StopTransitoryExtension(textEditor.TextStore);
			}
		}

		// Token: 0x0600566D RID: 22125 RVA: 0x0026923C File Offset: 0x0026823C
		private static void OnSourceChanged(object sender, SourceChangedEventArgs args)
		{
			TextEditorTyping.OnToggleInsert(sender, null);
		}

		// Token: 0x0600566E RID: 22126 RVA: 0x00269248 File Offset: 0x00268248
		private static void OnDelete(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
			TextEditorSelection._ClearSuggestedX(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer end = textEditor.Selection.End;
				if (textEditor.Selection.IsEmpty)
				{
					ITextPointer nextInsertionPosition = end.GetNextInsertionPosition(LogicalDirection.Forward);
					if (nextInsertionPosition == null)
					{
						return;
					}
					if (TextPointerBase.IsAtRowEnd(nextInsertionPosition))
					{
						return;
					}
					if (end is TextPointer && !TextEditorTyping.IsAtListItemStart(nextInsertionPosition) && TextEditorTyping.HandleDeleteWhenStructuralBoundaryIsCrossed(textEditor, (TextPointer)end, (TextPointer)nextInsertionPosition))
					{
						return;
					}
					textEditor.Selection.ExtendToNextInsertionPosition(LogicalDirection.Forward);
				}
				textEditor.Selection.Text = string.Empty;
			}
		}

		// Token: 0x0600566F RID: 22127 RVA: 0x00269338 File Offset: 0x00268338
		private static void OnBackspace(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			TextEditorSelection._ClearSuggestedX(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer textPointer = textEditor.Selection.Start;
				ITextPointer textPointer2 = null;
				if (textEditor.Selection.IsEmpty)
				{
					if (textEditor.AcceptsRichContent && TextEditorTyping.IsAtListItemStart(textPointer))
					{
						TextRangeEditLists.ConvertListItemsToParagraphs((TextRange)textEditor.Selection);
					}
					else if (textEditor.AcceptsRichContent && (TextEditorTyping.IsAtListItemChildStart(textPointer, false) || TextEditorTyping.IsAtIndentedParagraphOrBlockUIContainerStart(textEditor.Selection.Start)))
					{
						TextEditorLists.DecreaseIndentation(textEditor);
					}
					else
					{
						ITextPointer nextInsertionPosition = textPointer.GetNextInsertionPosition(LogicalDirection.Backward);
						if (nextInsertionPosition == null)
						{
							((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
							return;
						}
						if (TextPointerBase.IsAtRowEnd(nextInsertionPosition))
						{
							((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
							return;
						}
						if (textPointer is TextPointer && TextEditorTyping.HandleDeleteWhenStructuralBoundaryIsCrossed(textEditor, (TextPointer)textPointer, (TextPointer)nextInsertionPosition))
						{
							return;
						}
						textPointer = textPointer.GetFrozenPointer(LogicalDirection.Backward);
						if (textEditor.TextView != null && textPointer.HasValidLayout && textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text)
						{
							textPointer2 = textEditor.TextView.GetBackspaceCaretUnitPosition(textPointer);
							Invariant.Assert(textPointer2 != null);
							if (textPointer2.CompareTo(textPointer) == 0)
							{
								textEditor.Selection.ExtendToNextInsertionPosition(LogicalDirection.Backward);
								textPointer2 = null;
							}
							else if (textPointer2.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text)
							{
								textEditor.Selection.Select(textEditor.Selection.End, textPointer2);
								textPointer2 = null;
							}
						}
						else
						{
							textEditor.Selection.ExtendToNextInsertionPosition(LogicalDirection.Backward);
						}
					}
				}
				if (textEditor.AcceptsRichContent)
				{
					((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
					((TextSelection)textEditor.Selection).SpringloadCurrentFormatting();
				}
				if (textPointer2 != null)
				{
					Invariant.Assert(textPointer2.CompareTo(textPointer) < 0);
					textPointer2.DeleteContentToPosition(textPointer);
				}
				else
				{
					textEditor.Selection.Text = string.Empty;
					textPointer = textEditor.Selection.Start;
				}
				textEditor.Selection.SetCaretToPosition(textPointer, LogicalDirection.Backward, false, true);
			}
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x00269574 File Offset: 0x00268574
		private static bool HandleDeleteWhenStructuralBoundaryIsCrossed(TextEditor This, TextPointer position, TextPointer deletePosition)
		{
			if (!TextRangeEditTables.IsTableStructureCrossed(position, deletePosition) && !TextEditorTyping.IsBlockUIContainerBoundaryCrossed(position, deletePosition) && !TextPointerBase.IsAtRowEnd(position))
			{
				return false;
			}
			LogicalDirection logicalDirection = (position.CompareTo(deletePosition) < 0) ? LogicalDirection.Forward : LogicalDirection.Backward;
			Block paragraphOrBlockUIContainer = position.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer != null)
			{
				if (logicalDirection == LogicalDirection.Forward)
				{
					if ((paragraphOrBlockUIContainer.NextBlock != null && paragraphOrBlockUIContainer is Paragraph && Paragraph.HasNoTextContent((Paragraph)paragraphOrBlockUIContainer)) || (paragraphOrBlockUIContainer is BlockUIContainer && paragraphOrBlockUIContainer.IsEmpty))
					{
						paragraphOrBlockUIContainer.RepositionWithContent(null);
					}
				}
				else if ((paragraphOrBlockUIContainer.PreviousBlock != null && paragraphOrBlockUIContainer is Paragraph && Paragraph.HasNoTextContent((Paragraph)paragraphOrBlockUIContainer)) || (paragraphOrBlockUIContainer is BlockUIContainer && paragraphOrBlockUIContainer.IsEmpty))
				{
					paragraphOrBlockUIContainer.RepositionWithContent(null);
				}
			}
			This.Selection.SetCaretToPosition(deletePosition, logicalDirection, false, true);
			if (logicalDirection == LogicalDirection.Backward)
			{
				((TextSelection)This.Selection).ClearSpringloadFormatting();
			}
			return true;
		}

		// Token: 0x06005671 RID: 22129 RVA: 0x00269648 File Offset: 0x00268648
		private static bool IsAtIndentedParagraphOrBlockUIContainerStart(ITextPointer position)
		{
			if (position is TextPointer && TextPointerBase.IsAtParagraphOrBlockUIContainerStart(position))
			{
				Block paragraphOrBlockUIContainer = ((TextPointer)position).ParagraphOrBlockUIContainer;
				if (paragraphOrBlockUIContainer != null)
				{
					FlowDirection flowDirection = paragraphOrBlockUIContainer.FlowDirection;
					Thickness margin = paragraphOrBlockUIContainer.Margin;
					return (flowDirection == FlowDirection.LeftToRight && margin.Left > 0.0) || (flowDirection == FlowDirection.RightToLeft && margin.Right > 0.0) || (paragraphOrBlockUIContainer is Paragraph && ((Paragraph)paragraphOrBlockUIContainer).TextIndent > 0.0);
				}
			}
			return false;
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x002696D4 File Offset: 0x002686D4
		private static bool IsAtListItemStart(ITextPointer position)
		{
			if (typeof(ListItem).IsAssignableFrom(position.ParentType) && position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				return true;
			}
			while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				Type parentType = position.ParentType;
				if (TextSchema.IsBlock(parentType))
				{
					if (TextSchema.IsParagraphOrBlockUIContainer(parentType))
					{
						position = position.GetNextContextPosition(LogicalDirection.Backward);
						if (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && typeof(ListItem).IsAssignableFrom(position.ParentType))
						{
							return true;
						}
					}
					return false;
				}
				position = position.GetNextContextPosition(LogicalDirection.Backward);
			}
			return false;
		}

		// Token: 0x06005673 RID: 22131 RVA: 0x00269768 File Offset: 0x00268768
		private static bool IsAtListItemChildStart(ITextPointer position, bool emptyChildOnly)
		{
			if (position.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.ElementStart)
			{
				return false;
			}
			if (emptyChildOnly && position.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.ElementEnd)
			{
				return false;
			}
			ITextPointer textPointer = position.CreatePointer();
			while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && typeof(Inline).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			}
			if (textPointer.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.ElementStart || !TextSchema.IsParagraphOrBlockUIContainer(textPointer.ParentType))
			{
				return false;
			}
			textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			return typeof(ListItem).IsAssignableFrom(textPointer.ParentType);
		}

		// Token: 0x06005674 RID: 22132 RVA: 0x002697F6 File Offset: 0x002687F6
		private static bool IsBlockUIContainerBoundaryCrossed(TextPointer position1, TextPointer position2)
		{
			return (position1.Parent is BlockUIContainer || position2.Parent is BlockUIContainer) && position1.Parent != position2.Parent;
		}

		// Token: 0x06005675 RID: 22133 RVA: 0x00269828 File Offset: 0x00268828
		private static void OnDeleteNextWord(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.Selection.IsTableCellRange)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer textPointer = textEditor.Selection.End.CreatePointer();
			if (textEditor.Selection.IsEmpty)
			{
				TextPointerBase.MoveToNextWordBoundary(textPointer, LogicalDirection.Forward);
			}
			if (TextRangeEditTables.IsTableStructureCrossed(textEditor.Selection.Start, textPointer))
			{
				return;
			}
			ITextRange textRange = new TextRange(textEditor.Selection.Start, textPointer);
			if (textRange.IsTableCellRange)
			{
				return;
			}
			if (!textRange.IsEmpty)
			{
				using (textEditor.Selection.DeclareChangeBlock())
				{
					if (textEditor.AcceptsRichContent)
					{
						((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
					}
					textEditor.Selection.Select(textRange.Start, textRange.End);
					textEditor.Selection.Text = string.Empty;
				}
			}
		}

		// Token: 0x06005676 RID: 22134 RVA: 0x00269928 File Offset: 0x00268928
		private static void OnDeletePreviousWord(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.Selection.IsTableCellRange)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer textPointer = textEditor.Selection.Start.CreatePointer();
			if (textEditor.Selection.IsEmpty)
			{
				TextPointerBase.MoveToNextWordBoundary(textPointer, LogicalDirection.Backward);
			}
			if (TextRangeEditTables.IsTableStructureCrossed(textPointer, textEditor.Selection.Start))
			{
				return;
			}
			ITextRange textRange = new TextRange(textPointer, textEditor.Selection.End);
			if (textRange.IsTableCellRange)
			{
				return;
			}
			if (!textRange.IsEmpty)
			{
				using (textEditor.Selection.DeclareChangeBlock())
				{
					if (textEditor.AcceptsRichContent)
					{
						((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
						textEditor.Selection.Select(textRange.Start, textRange.End);
						((TextSelection)textEditor.Selection).SpringloadCurrentFormatting();
					}
					else
					{
						textEditor.Selection.Select(textRange.Start, textRange.End);
					}
					textEditor.Selection.Text = string.Empty;
				}
			}
		}

		// Token: 0x06005677 RID: 22135 RVA: 0x00269A54 File Offset: 0x00268A54
		private static void OnQueryStatusEnterBreak(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				args.ContinueRouting = true;
				return;
			}
			if (textEditor.Selection.IsTableCellRange || !textEditor.AcceptsReturn)
			{
				args.ContinueRouting = true;
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x00269AA8 File Offset: 0x00268AA8
		private static void OnEnterBreak(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.Selection.IsTableCellRange || !textEditor.AcceptsReturn || !textEditor.UiScope.IsKeyboardFocused)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				bool flag;
				if (textEditor.AcceptsRichContent && textEditor.Selection.Start is TextPointer)
				{
					flag = TextEditorTyping.HandleEnterBreakForRichText(textEditor, args.Command);
				}
				else
				{
					flag = TextEditorTyping.HandleEnterBreakForPlainText(textEditor);
				}
				if (flag)
				{
					textEditor.Selection.SetCaretToPosition(textEditor.Selection.End, LogicalDirection.Forward, false, false);
					TextEditorSelection._ClearSuggestedX(textEditor);
				}
			}
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x00269B74 File Offset: 0x00268B74
		private static bool HandleEnterBreakForRichText(TextEditor This, ICommand command)
		{
			bool flag = true;
			((TextSelection)This.Selection).SpringloadCurrentFormatting();
			if (!This.Selection.IsEmpty)
			{
				This.Selection.Text = string.Empty;
			}
			if (!TextEditorTyping.HandleEnterBreakWhenStructuralBoundaryIsCrossed(This, command))
			{
				TextPointer textPointer = ((TextSelection)This.Selection).End;
				if (command == EditingCommands.EnterParagraphBreak)
				{
					if (textPointer.HasNonMergeableInlineAncestor && !TextPointerBase.IsPositionAtNonMergeableInlineBoundary(textPointer))
					{
						flag = false;
					}
					else
					{
						textPointer = TextRangeEdit.InsertParagraphBreak(textPointer, true);
					}
				}
				else if (command == EditingCommands.EnterLineBreak)
				{
					textPointer = textPointer.InsertLineBreak();
				}
				if (flag)
				{
					This.Selection.Select(textPointer, textPointer);
				}
			}
			return flag;
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x00269C10 File Offset: 0x00268C10
		private static bool HandleEnterBreakForPlainText(TextEditor This)
		{
			bool result = true;
			if (This._FilterText(Environment.NewLine, This.Selection) != string.Empty)
			{
				This.Selection.Text = Environment.NewLine;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x00269C54 File Offset: 0x00268C54
		private static bool HandleEnterBreakWhenStructuralBoundaryIsCrossed(TextEditor This, ICommand command)
		{
			Invariant.Assert(This.Selection.Start is TextPointer);
			TextPointer textPointer = (TextPointer)This.Selection.Start;
			bool result = true;
			if (TextPointerBase.IsAtRowEnd(textPointer))
			{
				TextRange textRange = ((TextSelection)This.Selection).InsertRows(1);
				This.Selection.SetCaretToPosition(textRange.Start, LogicalDirection.Forward, false, false);
			}
			else if (This.Selection.IsEmpty && (TextPointerBase.IsInEmptyListItem(textPointer) || TextEditorTyping.IsAtListItemChildStart(textPointer, true)) && command == EditingCommands.EnterParagraphBreak)
			{
				TextEditorLists.DecreaseIndentation(This);
			}
			else if (TextPointerBase.IsBeforeFirstTable(textPointer) || TextPointerBase.IsAtBlockUIContainerStart(textPointer))
			{
				TextRangeEditTables.EnsureInsertionPosition(textPointer);
			}
			else if (TextPointerBase.IsAtBlockUIContainerEnd(textPointer))
			{
				TextPointer textPointer2 = TextRangeEditTables.EnsureInsertionPosition(textPointer);
				This.Selection.Select(textPointer2, textPointer2);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x00269D24 File Offset: 0x00268D24
		private static void OnFlowDirectionCommand(TextEditor This, Key key)
		{
			using (This.Selection.DeclareChangeBlock())
			{
				if (key == Key.LeftShift)
				{
					if (This.AcceptsRichContent && This.Selection is TextSelection)
					{
						((TextSelection)This.Selection).ApplyPropertyValue(FlowDocument.FlowDirectionProperty, FlowDirection.LeftToRight, true);
					}
					else
					{
						Invariant.Assert(This.UiScope != null);
						UIElementPropertyUndoUnit.Add(This.TextContainer, This.UiScope, FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight);
						This.UiScope.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight);
					}
				}
				else
				{
					Invariant.Assert(key == Key.RightShift);
					if (This.AcceptsRichContent && This.Selection is TextSelection)
					{
						((TextSelection)This.Selection).ApplyPropertyValue(FlowDocument.FlowDirectionProperty, FlowDirection.RightToLeft, true);
					}
					else
					{
						Invariant.Assert(This.UiScope != null);
						UIElementPropertyUndoUnit.Add(This.TextContainer, This.UiScope, FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft);
						This.UiScope.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft);
					}
				}
				((TextSelection)This.Selection).UpdateCaretState(CaretScrollMethod.Simple);
			}
		}

		// Token: 0x0600567D RID: 22141 RVA: 0x00269E68 File Offset: 0x00268E68
		private static void OnSpace(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			if (textEditor.TextStore != null && textEditor.TextStore.IsComposing)
			{
				return;
			}
			if (textEditor.ImmComposition != null && textEditor.ImmComposition.IsComposition)
			{
				return;
			}
			e.Handled = true;
			if (textEditor.TextView != null)
			{
				textEditor.TextView.ThrottleBackgroundTasksForUserInput();
			}
			TextEditorTyping.ScheduleInput(textEditor, new TextEditorTyping.TextInputItem(textEditor, " ", !textEditor._OvertypeMode));
		}

		// Token: 0x0600567E RID: 22142 RVA: 0x00269F00 File Offset: 0x00268F00
		private static void OnQueryStatusTabForward(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor != null && textEditor.AcceptsTab)
			{
				args.CanExecute = true;
				return;
			}
			args.ContinueRouting = true;
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x00269F00 File Offset: 0x00268F00
		private static void OnQueryStatusTabBackward(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor != null && textEditor.AcceptsTab)
			{
				args.CanExecute = true;
				return;
			}
			args.ContinueRouting = true;
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x00269F30 File Offset: 0x00268F30
		private static void OnTabForward(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.UiScope.IsKeyboardFocused)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (TextEditorTyping.HandleTabInTables(textEditor, LogicalDirection.Forward))
			{
				return;
			}
			if (textEditor.AcceptsRichContent && (!textEditor.Selection.IsEmpty || TextPointerBase.IsAtParagraphOrBlockUIContainerStart(textEditor.Selection.Start)) && EditingCommands.IncreaseIndentation.CanExecute(null, (IInputElement)sender))
			{
				EditingCommands.IncreaseIndentation.Execute(null, (IInputElement)sender);
				return;
			}
			TextEditorTyping.DoTextInput(textEditor, "\t", !textEditor._OvertypeMode, true);
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x00269FD8 File Offset: 0x00268FD8
		private static void OnTabBackward(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.UiScope.IsKeyboardFocused)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (TextEditorTyping.HandleTabInTables(textEditor, LogicalDirection.Backward))
			{
				return;
			}
			if (textEditor.AcceptsRichContent && (!textEditor.Selection.IsEmpty || TextPointerBase.IsAtParagraphOrBlockUIContainerStart(textEditor.Selection.Start)) && EditingCommands.DecreaseIndentation.CanExecute(null, (IInputElement)sender))
			{
				EditingCommands.DecreaseIndentation.Execute(null, (IInputElement)sender);
				return;
			}
			TextEditorTyping.DoTextInput(textEditor, "\t", !textEditor._OvertypeMode, true);
		}

		// Token: 0x06005682 RID: 22146 RVA: 0x0026A080 File Offset: 0x00269080
		private static bool HandleTabInTables(TextEditor This, LogicalDirection direction)
		{
			if (!This.AcceptsRichContent)
			{
				return false;
			}
			if (This.Selection.IsTableCellRange)
			{
				This.Selection.SetCaretToPosition(This.Selection.Start, LogicalDirection.Backward, false, false);
				return true;
			}
			if (This.Selection.IsEmpty && TextPointerBase.IsAtRowEnd(This.Selection.End))
			{
				TableCell tableCell = null;
				TableRow tableRow = ((TextPointer)This.Selection.End).Parent as TableRow;
				Invariant.Assert(tableRow != null);
				TableRowGroup rowGroup = tableRow.RowGroup;
				int num = rowGroup.Rows.IndexOf(tableRow);
				if (direction == LogicalDirection.Forward)
				{
					if (num + 1 < rowGroup.Rows.Count)
					{
						tableCell = rowGroup.Rows[num + 1].Cells[0];
					}
				}
				else if (num > 0)
				{
					tableCell = rowGroup.Rows[num - 1].Cells[rowGroup.Rows[num - 1].Cells.Count - 1];
				}
				if (tableCell != null)
				{
					This.Selection.Select(tableCell.ContentStart, tableCell.ContentEnd);
				}
				return true;
			}
			TextElement textElement = ((TextPointer)This.Selection.Start).Parent as TextElement;
			while (textElement != null && !(textElement is TableCell))
			{
				textElement = (textElement.Parent as TextElement);
			}
			if (textElement is TableCell)
			{
				TableCell tableCell2 = (TableCell)textElement;
				TableRow row = tableCell2.Row;
				TableRowGroup rowGroup2 = row.RowGroup;
				int num2 = row.Cells.IndexOf(tableCell2);
				int num3 = rowGroup2.Rows.IndexOf(row);
				if (direction == LogicalDirection.Forward)
				{
					if (num2 + 1 < row.Cells.Count)
					{
						tableCell2 = row.Cells[num2 + 1];
					}
					else if (num3 + 1 < rowGroup2.Rows.Count)
					{
						tableCell2 = rowGroup2.Rows[num3 + 1].Cells[0];
					}
				}
				else if (num2 > 0)
				{
					tableCell2 = row.Cells[num2 - 1];
				}
				else if (num3 > 0)
				{
					tableCell2 = rowGroup2.Rows[num3 - 1].Cells[rowGroup2.Rows[num3 - 1].Cells.Count - 1];
				}
				Invariant.Assert(tableCell2 != null);
				This.Selection.Select(tableCell2.ContentStart, tableCell2.ContentEnd);
				return true;
			}
			return false;
		}

		// Token: 0x06005683 RID: 22147 RVA: 0x0026A2FC File Offset: 0x002692FC
		private static void DoTextInput(TextEditor This, string textData, bool isInsertKeyToggled, bool acceptControlCharacters)
		{
			TextEditorTyping.HideCursor(This);
			if (!acceptControlCharacters)
			{
				for (int i = 0; i < textData.Length; i++)
				{
					if (char.IsControl(textData[i]))
					{
						textData = textData.Remove(i--, 1);
					}
				}
			}
			string text = This._FilterText(textData, This.Selection);
			if (text.Length == 0)
			{
				return;
			}
			TextEditorTyping.OpenTypingUndoUnit(This);
			UndoCloseAction closeAction = UndoCloseAction.Rollback;
			try
			{
				using (This.Selection.DeclareChangeBlock())
				{
					This.Selection.ApplyTypingHeuristics(This.AllowOvertype && This._OvertypeMode && text != "\t");
					This.SetSelectedText(text, InputLanguageManager.Current.CurrentInputLanguage);
					ITextPointer caretPosition = This.Selection.End.CreatePointer(LogicalDirection.Backward);
					This.Selection.SetCaretToPosition(caretPosition, LogicalDirection.Backward, true, true);
					closeAction = UndoCloseAction.Commit;
				}
			}
			finally
			{
				TextEditorTyping.CloseTypingUndoUnit(This, closeAction);
			}
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x0026A3FC File Offset: 0x002693FC
		private static void ScheduleInput(TextEditor This, TextEditorTyping.InputItem item)
		{
			if (!This.AcceptsRichContent || TextEditorTyping.IsMouseInputPending(This))
			{
				TextEditorTyping._FlushPendingInputItems(This);
				item.Do();
				return;
			}
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (threadLocalStore.PendingInputItems == null)
			{
				threadLocalStore.PendingInputItems = new ArrayList(1);
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(TextEditorTyping.BackgroundInputCallback), This);
			}
			threadLocalStore.PendingInputItems.Add(item);
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x0026A468 File Offset: 0x00269468
		private static bool IsMouseInputPending(TextEditor This)
		{
			bool result = false;
			IWin32Window win32Window = PresentationSource.CriticalFromVisual(This.UiScope) as IWin32Window;
			if (win32Window != null)
			{
				IntPtr intPtr = IntPtr.Zero;
				intPtr = win32Window.Handle;
				if (intPtr != (IntPtr)0)
				{
					MSG msg = default(MSG);
					result = UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(null, intPtr), WindowMessage.WM_MOUSEMOVE, WindowMessage.WM_MOUSEHWHEEL, 0);
				}
			}
			return result;
		}

		// Token: 0x06005686 RID: 22150 RVA: 0x0026A4CC File Offset: 0x002694CC
		private static object BackgroundInputCallback(object This)
		{
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			Invariant.Assert(This is TextEditor);
			Invariant.Assert(threadLocalStore.PendingInputItems != null);
			try
			{
				TextEditorTyping._FlushPendingInputItems((TextEditor)This);
			}
			finally
			{
				threadLocalStore.PendingInputItems = null;
			}
			return null;
		}

		// Token: 0x06005687 RID: 22151 RVA: 0x0026A524 File Offset: 0x00269524
		private static void OnDispatcherShutdownFinished(object sender, EventArgs args)
		{
			Dispatcher.CurrentDispatcher.ShutdownFinished -= TextEditorTyping.OnDispatcherShutdownFinished;
			InputLanguageManager.Current.InputLanguageChanged -= TextEditorTyping.OnInputLanguageChanged;
			TextEditor._ThreadLocalStore.InputLanguageChangeEventHandlerCount = 0;
		}

		// Token: 0x06005688 RID: 22152 RVA: 0x0026A55D File Offset: 0x0026955D
		private static void OnInputLanguageChanged(object sender, InputLanguageEventArgs e)
		{
			TextSelection.OnInputLanguageChanged(e.NewLanguage);
		}

		// Token: 0x06005689 RID: 22153 RVA: 0x0026A56C File Offset: 0x0026956C
		private static void OpenTypingUndoUnit(TextEditor This)
		{
			UndoManager undoManager = This._GetUndoManager();
			if (undoManager != null && undoManager.IsEnabled)
			{
				if (This._typingUndoUnit != null && undoManager.LastUnit == This._typingUndoUnit && !This._typingUndoUnit.Locked)
				{
					undoManager.Reopen(This._typingUndoUnit);
					return;
				}
				This._typingUndoUnit = new TextParentUndoUnit(This.Selection);
				undoManager.Open(This._typingUndoUnit);
			}
		}

		// Token: 0x0600568A RID: 22154 RVA: 0x0026A5D8 File Offset: 0x002695D8
		private static void CloseTypingUndoUnit(TextEditor This, UndoCloseAction closeAction)
		{
			UndoManager undoManager = This._GetUndoManager();
			if (undoManager != null && undoManager.IsEnabled)
			{
				if (This._typingUndoUnit != null && undoManager.LastUnit == This._typingUndoUnit && !This._typingUndoUnit.Locked)
				{
					if (This._typingUndoUnit is TextParentUndoUnit)
					{
						((TextParentUndoUnit)This._typingUndoUnit).RecordRedoSelectionState();
					}
					undoManager.Close(This._typingUndoUnit, closeAction);
					return;
				}
			}
			else
			{
				This._typingUndoUnit = null;
			}
		}

		// Token: 0x0600568B RID: 22155 RVA: 0x00262877 File Offset: 0x00261877
		private static void OnQueryStatusNYI(object target, CanExecuteRoutedEventArgs args)
		{
			if (TextEditor._GetTextEditor(target) == null)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x0026A64C File Offset: 0x0026964C
		private static void OnMouseMove(object sender, MouseEventArgs e)
		{
			TextEditorTyping._ShowCursor();
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x0026A64C File Offset: 0x0026964C
		private static void OnMouseLeave(object sender, MouseEventArgs e)
		{
			TextEditorTyping._ShowCursor();
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x0026A653 File Offset: 0x00269653
		private static void HideCursor(TextEditor This)
		{
			if (!TextEditor._ThreadLocalStore.HideCursor && SystemParameters.MouseVanish && This.UiScope.IsMouseOver)
			{
				TextEditor._ThreadLocalStore.HideCursor = true;
				SafeNativeMethods.ShowCursor(false);
			}
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x0026A688 File Offset: 0x00269688
		private static void UpdateHyperlinkCursor(TextEditor This)
		{
			if (This.UiScope is RichTextBox && This.TextView != null && This.TextView.IsValid)
			{
				TextPointer textPointer = (TextPointer)This.TextView.GetTextPositionFromPoint(Mouse.GetPosition(This.TextView.RenderScope), false);
				if (textPointer != null && textPointer.Parent is TextElement && TextSchema.HasHyperlinkAncestor((TextElement)textPointer.Parent))
				{
					Mouse.UpdateCursor();
				}
			}
		}

		// Token: 0x04002F95 RID: 12181
		private const string KeyBackspace = "Backspace";

		// Token: 0x04002F96 RID: 12182
		private const string KeyDelete = "Delete";

		// Token: 0x04002F97 RID: 12183
		private const string KeyDeleteNextWord = "Ctrl+Delete";

		// Token: 0x04002F98 RID: 12184
		private const string KeyDeletePreviousWord = "Ctrl+Backspace";

		// Token: 0x04002F99 RID: 12185
		private const string KeyEnterLineBreak = "Shift+Enter";

		// Token: 0x04002F9A RID: 12186
		private const string KeyEnterParagraphBreak = "Enter";

		// Token: 0x04002F9B RID: 12187
		private const string KeyShiftBackspace = "Shift+Backspace";

		// Token: 0x04002F9C RID: 12188
		private const string KeyShiftSpace = "Shift+Space";

		// Token: 0x04002F9D RID: 12189
		private const string KeySpace = "Space";

		// Token: 0x04002F9E RID: 12190
		private const string KeyTabBackward = "Shift+Tab";

		// Token: 0x04002F9F RID: 12191
		private const string KeyTabForward = "Tab";

		// Token: 0x04002FA0 RID: 12192
		private const string KeyToggleInsert = "Insert";

		// Token: 0x02000B66 RID: 2918
		private abstract class InputItem
		{
			// Token: 0x06008DE6 RID: 36326 RVA: 0x0033FD50 File Offset: 0x0033ED50
			internal InputItem(TextEditor textEditor)
			{
				this._textEditor = textEditor;
			}

			// Token: 0x06008DE7 RID: 36327
			internal abstract void Do();

			// Token: 0x17001EFE RID: 7934
			// (get) Token: 0x06008DE8 RID: 36328 RVA: 0x0033FD5F File Offset: 0x0033ED5F
			protected TextEditor TextEditor
			{
				get
				{
					return this._textEditor;
				}
			}

			// Token: 0x040048C7 RID: 18631
			private TextEditor _textEditor;
		}

		// Token: 0x02000B67 RID: 2919
		private class TextInputItem : TextEditorTyping.InputItem
		{
			// Token: 0x06008DE9 RID: 36329 RVA: 0x0033FD67 File Offset: 0x0033ED67
			internal TextInputItem(TextEditor textEditor, string text, bool isInsertKeyToggled) : base(textEditor)
			{
				this._text = text;
				this._isInsertKeyToggled = isInsertKeyToggled;
			}

			// Token: 0x06008DEA RID: 36330 RVA: 0x0033FD7E File Offset: 0x0033ED7E
			internal override void Do()
			{
				if (base.TextEditor.UiScope == null)
				{
					return;
				}
				TextEditorTyping.DoTextInput(base.TextEditor, this._text, this._isInsertKeyToggled, false);
			}

			// Token: 0x040048C8 RID: 18632
			private readonly string _text;

			// Token: 0x040048C9 RID: 18633
			private readonly bool _isInsertKeyToggled;
		}

		// Token: 0x02000B68 RID: 2920
		private class KeyUpInputItem : TextEditorTyping.InputItem
		{
			// Token: 0x06008DEB RID: 36331 RVA: 0x0033FDA6 File Offset: 0x0033EDA6
			internal KeyUpInputItem(TextEditor textEditor, Key key, ModifierKeys modifiers) : base(textEditor)
			{
				this._key = key;
				this._modifiers = modifiers;
			}

			// Token: 0x06008DEC RID: 36332 RVA: 0x0033FDC0 File Offset: 0x0033EDC0
			internal override void Do()
			{
				if (base.TextEditor.UiScope == null)
				{
					return;
				}
				Key key = this._key;
				if (key != Key.LeftShift)
				{
					if (key == Key.RightShift)
					{
						if (TextSelection.IsBidiInputLanguageInstalled())
						{
							TextEditorTyping.OnFlowDirectionCommand(base.TextEditor, this._key);
							return;
						}
					}
					else
					{
						Invariant.Assert(false, "Unexpected key value!");
					}
					return;
				}
				TextEditorTyping.OnFlowDirectionCommand(base.TextEditor, this._key);
			}

			// Token: 0x040048CA RID: 18634
			private readonly Key _key;

			// Token: 0x040048CB RID: 18635
			private readonly ModifierKeys _modifiers;
		}
	}
}
