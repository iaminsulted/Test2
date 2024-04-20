using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020006A7 RID: 1703
	internal static class TextEditorSpelling
	{
		// Token: 0x06005643 RID: 22083 RVA: 0x0026821C File Offset: 0x0026721C
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.CorrectSpellingError, new ExecutedRoutedEventHandler(TextEditorSpelling.OnCorrectSpellingError), new CanExecuteRoutedEventHandler(TextEditorSpelling.OnQueryStatusSpellingError));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.IgnoreSpellingError, new ExecutedRoutedEventHandler(TextEditorSpelling.OnIgnoreSpellingError), new CanExecuteRoutedEventHandler(TextEditorSpelling.OnQueryStatusSpellingError));
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x0026826F File Offset: 0x0026726F
		internal static SpellingError GetSpellingErrorAtPosition(TextEditor This, ITextPointer position, LogicalDirection direction)
		{
			if (This.Speller != null)
			{
				return This.Speller.GetError(position, direction, true);
			}
			return null;
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x0026828C File Offset: 0x0026728C
		internal static SpellingError GetSpellingErrorAtSelection(TextEditor This)
		{
			if (This.Speller == null)
			{
				return null;
			}
			if (TextEditorSpelling.IsSelectionIgnoringErrors(This.Selection))
			{
				return null;
			}
			LogicalDirection logicalDirection = This.Selection.IsEmpty ? This.Selection.Start.LogicalDirection : LogicalDirection.Forward;
			char c;
			ITextPointer textPointer = TextEditorSpelling.GetNextTextPosition(This.Selection.Start, null, logicalDirection, out c);
			if (textPointer == null)
			{
				logicalDirection = ((logicalDirection == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward);
				textPointer = TextEditorSpelling.GetNextTextPosition(This.Selection.Start, null, logicalDirection, out c);
			}
			else if (char.IsWhiteSpace(c))
			{
				if (This.Selection.IsEmpty)
				{
					logicalDirection = ((logicalDirection == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward);
					textPointer = TextEditorSpelling.GetNextTextPosition(This.Selection.Start, null, logicalDirection, out c);
				}
				else
				{
					logicalDirection = LogicalDirection.Forward;
					textPointer = TextEditorSpelling.GetNextNonWhiteSpacePosition(This.Selection.Start, This.Selection.End);
					if (textPointer == null)
					{
						logicalDirection = LogicalDirection.Backward;
						textPointer = TextEditorSpelling.GetNextTextPosition(This.Selection.Start, null, logicalDirection, out c);
					}
				}
			}
			if (textPointer != null)
			{
				return This.Speller.GetError(textPointer, logicalDirection, false);
			}
			return null;
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x0026838B File Offset: 0x0026738B
		internal static ITextPointer GetNextSpellingErrorPosition(TextEditor This, ITextPointer position, LogicalDirection direction)
		{
			if (This.Speller != null)
			{
				return This.Speller.GetNextSpellingErrorPosition(position, direction);
			}
			return null;
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x002683A4 File Offset: 0x002673A4
		private static void OnCorrectSpellingError(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			string text = args.Parameter as string;
			if (text == null)
			{
				return;
			}
			SpellingError spellingErrorAtSelection = TextEditorSpelling.GetSpellingErrorAtSelection(textEditor);
			if (spellingErrorAtSelection == null)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer textPointer;
				ITextPointer position;
				ITextPointer textPointer2;
				if (TextEditorSpelling.IsErrorAtNonMergeableInlineEdge(spellingErrorAtSelection, out textPointer, out position) && textPointer is TextPointer)
				{
					((TextPointer)textPointer).DeleteTextInRun(textPointer.GetOffsetToPosition(position));
					textPointer.InsertTextInRun(text);
					textPointer2 = textPointer.CreatePointer(text.Length, LogicalDirection.Forward);
				}
				else
				{
					textEditor.Selection.Select(spellingErrorAtSelection.Start, spellingErrorAtSelection.End);
					if (textEditor.AcceptsRichContent)
					{
						((TextSelection)textEditor.Selection).SpringloadCurrentFormatting();
					}
					XmlLanguage xmlLanguage = (XmlLanguage)spellingErrorAtSelection.Start.GetValue(FrameworkElement.LanguageProperty);
					textEditor.SetSelectedText(text, xmlLanguage.GetSpecificCulture());
					textPointer2 = textEditor.Selection.End;
				}
				textEditor.Selection.Select(textPointer2, textPointer2);
			}
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x002684B4 File Offset: 0x002674B4
		private static bool IsErrorAtNonMergeableInlineEdge(SpellingError spellingError, out ITextPointer textStart, out ITextPointer textEnd)
		{
			bool result = false;
			textStart = spellingError.Start.CreatePointer(LogicalDirection.Backward);
			while (textStart.CompareTo(spellingError.End) < 0 && textStart.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
			{
				textStart.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			textEnd = spellingError.End.CreatePointer();
			while (textEnd.CompareTo(spellingError.Start) > 0 && textEnd.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text)
			{
				textEnd.MoveToNextContextPosition(LogicalDirection.Backward);
			}
			if (textStart.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text || textStart.CompareTo(spellingError.End) == 0)
			{
				return false;
			}
			Invariant.Assert(textEnd.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text && textEnd.CompareTo(spellingError.Start) != 0);
			if ((TextPointerBase.IsAtNonMergeableInlineStart(textStart) || TextPointerBase.IsAtNonMergeableInlineEnd(textEnd)) && typeof(Run).IsAssignableFrom(textStart.ParentType) && textStart.HasEqualScope(textEnd))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06005649 RID: 22089 RVA: 0x002685A4 File Offset: 0x002675A4
		private static void OnIgnoreSpellingError(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			SpellingError spellingErrorAtSelection = TextEditorSpelling.GetSpellingErrorAtSelection(textEditor);
			if (spellingErrorAtSelection == null)
			{
				return;
			}
			spellingErrorAtSelection.IgnoreAll();
		}

		// Token: 0x0600564A RID: 22090 RVA: 0x002685D0 File Offset: 0x002675D0
		private static void OnQueryStatusSpellingError(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			SpellingError spellingErrorAtSelection = TextEditorSpelling.GetSpellingErrorAtSelection(textEditor);
			args.CanExecute = (spellingErrorAtSelection != null);
		}

		// Token: 0x0600564B RID: 22091 RVA: 0x002685FC File Offset: 0x002675FC
		private static ITextPointer GetNextTextPosition(ITextPointer position, ITextPointer limit, LogicalDirection direction, out char character)
		{
			bool flag = false;
			character = '\0';
			while (position != null && !flag && (limit == null || position.CompareTo(limit) < 0))
			{
				switch (position.GetPointerContext(direction))
				{
				case TextPointerContext.Text:
				{
					char[] array = new char[1];
					position.GetTextInRun(direction, array, 0, 1);
					character = array[0];
					flag = true;
					continue;
				}
				case TextPointerContext.ElementStart:
				case TextPointerContext.ElementEnd:
					if (TextSchema.IsFormattingType(position.GetElementType(direction)))
					{
						position = position.CreatePointer(1);
						continue;
					}
					position = null;
					continue;
				}
				position = null;
			}
			return position;
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x00268684 File Offset: 0x00267684
		private static ITextPointer GetNextNonWhiteSpacePosition(ITextPointer position, ITextPointer limit)
		{
			Invariant.Assert(limit != null);
			while (position.CompareTo(limit) != 0)
			{
				char c;
				position = TextEditorSpelling.GetNextTextPosition(position, limit, LogicalDirection.Forward, out c);
				if (position == null || !char.IsWhiteSpace(c))
				{
					return position;
				}
				position = position.CreatePointer(1);
			}
			return null;
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x002686CC File Offset: 0x002676CC
		private static bool IsSelectionIgnoringErrors(ITextSelection selection)
		{
			bool flag = false;
			if (selection.Start is TextPointer)
			{
				flag = (((TextPointer)selection.Start).ParentBlock != ((TextPointer)selection.End).ParentBlock);
			}
			if (!flag)
			{
				flag = (selection.Start.GetOffsetToPosition(selection.End) >= 256);
			}
			if (!flag)
			{
				string text = selection.Text;
				int num = 0;
				while (num < text.Length && !flag)
				{
					flag = TextPointerBase.IsCharUnicodeNewLine(text[num]);
					num++;
				}
			}
			return flag;
		}
	}
}
