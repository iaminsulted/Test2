using System;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020006A3 RID: 1699
	internal static class TextEditorLists
	{
		// Token: 0x060055DF RID: 21983 RVA: 0x00264104 File Offset: 0x00263104
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.RemoveListMarkers, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusNYI), KeyGesture.CreateFromResourceStrings("Ctrl+Shift+R", "KeyRemoveListMarkersDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleBullets, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusNYI), KeyGesture.CreateFromResourceStrings("Ctrl+Shift+L", "KeyToggleBulletsDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleNumbering, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusNYI), KeyGesture.CreateFromResourceStrings("Ctrl+Shift+N", "KeyToggleNumberingDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.IncreaseIndentation, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusTab), KeyGesture.CreateFromResourceStrings("Ctrl+T", "KeyIncreaseIndentationDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DecreaseIndentation, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusTab), KeyGesture.CreateFromResourceStrings("Ctrl+Shift+T", "KeyDecreaseIndentationDisplayString"));
		}

		// Token: 0x060055E0 RID: 21984 RVA: 0x0026420C File Offset: 0x0026320C
		internal static void DecreaseIndentation(TextEditor This)
		{
			TextSelection textSelection = (TextSelection)This.Selection;
			ListItem listItem = TextPointerBase.GetListItem(textSelection.Start);
			ListItem immediateListItem = TextPointerBase.GetImmediateListItem(textSelection.Start);
			TextEditorLists.DecreaseIndentation(textSelection, listItem, immediateListItem);
		}

		// Token: 0x060055E1 RID: 21985 RVA: 0x00264244 File Offset: 0x00263244
		private static TextEditor IsEnabledNotReadOnlyIsTextSegment(object sender)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor != null && textEditor._IsEnabled && !textEditor.IsReadOnly && !textEditor.Selection.IsTableCellRange)
			{
				return textEditor;
			}
			return null;
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x0026427C File Offset: 0x0026327C
		private static void OnQueryStatusTab(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditorLists.IsEnabledNotReadOnlyIsTextSegment(sender);
			if (textEditor != null && textEditor.AcceptsTab)
			{
				args.CanExecute = true;
			}
		}

		// Token: 0x060055E3 RID: 21987 RVA: 0x00262877 File Offset: 0x00261877
		private static void OnQueryStatusNYI(object target, CanExecuteRoutedEventArgs args)
		{
			if (TextEditor._GetTextEditor(target) == null)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x060055E4 RID: 21988 RVA: 0x002642A4 File Offset: 0x002632A4
		private static void OnListCommand(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!TextRangeEditLists.IsListOperationApplicable((TextSelection)textEditor.Selection))
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextSelection textSelection = (TextSelection)textEditor.Selection;
				ListItem listItem = TextPointerBase.GetListItem(textSelection.Start);
				ListItem immediateListItem = TextPointerBase.GetImmediateListItem(textSelection.Start);
				List list = (listItem == null) ? null : ((List)listItem.Parent);
				TextEditorSelection._ClearSuggestedX(textEditor);
				if (args.Command == EditingCommands.ToggleBullets)
				{
					TextEditorLists.ToggleBullets(textSelection, listItem, immediateListItem, list);
				}
				else if (args.Command == EditingCommands.ToggleNumbering)
				{
					TextEditorLists.ToggleNumbering(textSelection, listItem, immediateListItem, list);
				}
				else if (args.Command == EditingCommands.RemoveListMarkers)
				{
					TextRangeEditLists.ConvertListItemsToParagraphs(textSelection);
				}
				else if (args.Command == EditingCommands.IncreaseIndentation)
				{
					TextEditorLists.IncreaseIndentation(textSelection, listItem, immediateListItem);
				}
				else if (args.Command == EditingCommands.DecreaseIndentation)
				{
					TextEditorLists.DecreaseIndentation(textSelection, listItem, immediateListItem);
				}
				else
				{
					Invariant.Assert(false);
				}
			}
		}

		// Token: 0x060055E5 RID: 21989 RVA: 0x002643E0 File Offset: 0x002633E0
		private static void ToggleBullets(TextSelection thisSelection, ListItem parentListItem, ListItem immediateListItem, List list)
		{
			if (immediateListItem != null && TextEditorLists.HasBulletMarker(list))
			{
				if (list.Parent is ListItem)
				{
					TextRangeEditLists.UnindentListItems(thisSelection);
					TextRangeEditLists.ConvertListItemsToParagraphs(thisSelection);
					return;
				}
				TextRangeEditLists.UnindentListItems(thisSelection);
				return;
			}
			else
			{
				if (immediateListItem != null)
				{
					list.MarkerStyle = TextMarkerStyle.Disc;
					return;
				}
				if (parentListItem != null)
				{
					TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Disc);
					TextRangeEditLists.IndentListItems(thisSelection);
					return;
				}
				TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Disc);
				return;
			}
		}

		// Token: 0x060055E6 RID: 21990 RVA: 0x00264440 File Offset: 0x00263440
		private static void ToggleNumbering(TextSelection thisSelection, ListItem parentListItem, ListItem immediateListItem, List list)
		{
			if (immediateListItem != null && TextEditorLists.HasNumericMarker(list))
			{
				if (list.Parent is ListItem)
				{
					TextRangeEditLists.UnindentListItems(thisSelection);
					TextRangeEditLists.ConvertListItemsToParagraphs(thisSelection);
					return;
				}
				TextRangeEditLists.UnindentListItems(thisSelection);
				return;
			}
			else
			{
				if (immediateListItem != null)
				{
					list.MarkerStyle = TextMarkerStyle.Decimal;
					return;
				}
				if (parentListItem != null)
				{
					TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Decimal);
					TextRangeEditLists.IndentListItems(thisSelection);
					return;
				}
				TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Decimal);
				return;
			}
		}

		// Token: 0x060055E7 RID: 21991 RVA: 0x002644A4 File Offset: 0x002634A4
		private static void IncreaseIndentation(TextSelection thisSelection, ListItem parentListItem, ListItem immediateListItem)
		{
			if (immediateListItem != null)
			{
				TextRangeEditLists.IndentListItems(thisSelection);
				return;
			}
			if (parentListItem != null)
			{
				TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Decimal);
				TextRangeEditLists.IndentListItems(thisSelection);
				return;
			}
			if (!thisSelection.IsEmpty)
			{
				TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.IncreaseByAbsoluteValue);
				return;
			}
			if (thisSelection.Start.ParagraphOrBlockUIContainer is BlockUIContainer)
			{
				TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.IncreaseByAbsoluteValue);
				return;
			}
			TextEditorLists.CreateImplicitParagraphIfNeededAndUpdateSelection(thisSelection);
			Paragraph paragraph = thisSelection.Start.Paragraph;
			Invariant.Assert(paragraph != null, "EnsureInsertionPosition must guarantee a position in text content");
			if (paragraph.TextIndent < 0.0)
			{
				TextRangeEdit.SetParagraphProperty(thisSelection.Start, thisSelection.End, Paragraph.TextIndentProperty, 0.0, PropertyValueAction.SetValue);
				return;
			}
			if (paragraph.TextIndent < 20.0)
			{
				TextRangeEdit.SetParagraphProperty(thisSelection.Start, thisSelection.End, Paragraph.TextIndentProperty, 20.0, PropertyValueAction.SetValue);
				return;
			}
			TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.IncreaseByAbsoluteValue);
		}

		// Token: 0x060055E8 RID: 21992 RVA: 0x002645AC File Offset: 0x002635AC
		private static void DecreaseIndentation(TextSelection thisSelection, ListItem parentListItem, ListItem immediateListItem)
		{
			if (immediateListItem != null)
			{
				TextRangeEditLists.UnindentListItems(thisSelection);
				return;
			}
			if (parentListItem != null)
			{
				TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Disc);
				TextRangeEditLists.UnindentListItems(thisSelection);
				return;
			}
			if (!thisSelection.IsEmpty)
			{
				TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.DecreaseByAbsoluteValue);
				return;
			}
			if (thisSelection.Start.ParagraphOrBlockUIContainer is BlockUIContainer)
			{
				TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.DecreaseByAbsoluteValue);
				return;
			}
			TextEditorLists.CreateImplicitParagraphIfNeededAndUpdateSelection(thisSelection);
			Paragraph paragraph = thisSelection.Start.Paragraph;
			Invariant.Assert(paragraph != null, "EnsureInsertionPosition must guarantee a position in text content");
			if (paragraph.TextIndent > 20.0)
			{
				TextRangeEdit.SetParagraphProperty(thisSelection.Start, thisSelection.End, Paragraph.TextIndentProperty, 20.0, PropertyValueAction.SetValue);
				return;
			}
			if (paragraph.TextIndent > 0.0)
			{
				TextRangeEdit.SetParagraphProperty(thisSelection.Start, thisSelection.End, Paragraph.TextIndentProperty, 0.0, PropertyValueAction.SetValue);
				return;
			}
			TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.DecreaseByAbsoluteValue);
		}

		// Token: 0x060055E9 RID: 21993 RVA: 0x002646B4 File Offset: 0x002636B4
		private static void CreateImplicitParagraphIfNeededAndUpdateSelection(TextSelection thisSelection)
		{
			TextPointer textPointer = thisSelection.Start;
			if (TextPointerBase.IsAtPotentialParagraphPosition(textPointer))
			{
				textPointer = TextRangeEditTables.EnsureInsertionPosition(textPointer);
				thisSelection.Select(textPointer, textPointer);
			}
		}

		// Token: 0x060055EA RID: 21994 RVA: 0x002646E0 File Offset: 0x002636E0
		private static bool HasBulletMarker(List list)
		{
			if (list == null)
			{
				return false;
			}
			TextMarkerStyle markerStyle = list.MarkerStyle;
			return TextMarkerStyle.Disc <= markerStyle && markerStyle <= TextMarkerStyle.Box;
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x00264708 File Offset: 0x00263708
		private static bool HasNumericMarker(List list)
		{
			if (list == null)
			{
				return false;
			}
			TextMarkerStyle markerStyle = list.MarkerStyle;
			return TextMarkerStyle.LowerRoman <= markerStyle && markerStyle <= TextMarkerStyle.Decimal;
		}

		// Token: 0x04002F56 RID: 12118
		private const string KeyDecreaseIndentation = "Ctrl+Shift+T";

		// Token: 0x04002F57 RID: 12119
		private const string KeyToggleBullets = "Ctrl+Shift+L";

		// Token: 0x04002F58 RID: 12120
		private const string KeyToggleNumbering = "Ctrl+Shift+N";

		// Token: 0x04002F59 RID: 12121
		private const string KeyRemoveListMarkers = "Ctrl+Shift+R";

		// Token: 0x04002F5A RID: 12122
		private const string KeyIncreaseIndentation = "Ctrl+T";
	}
}
