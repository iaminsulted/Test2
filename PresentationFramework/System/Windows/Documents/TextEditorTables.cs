using System;
using System.Windows.Input;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020006A8 RID: 1704
	internal static class TextEditorTables
	{
		// Token: 0x0600564E RID: 22094 RVA: 0x0026875C File Offset: 0x0026775C
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(TextEditorTables.OnTableCommand);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorTables.OnQueryStatusNYI);
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.InsertTable, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+Shift+T", "KeyInsertTableDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.InsertRows, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+Shift+R", "KeyInsertRowsDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.InsertColumns, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+Shift+C", "KeyInsertColumnsDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DeleteRows, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyDeleteRows", "KeyDeleteRowsDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DeleteColumns, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+Shift+D", "KeyDeleteColumnsDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MergeCells, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+Shift+M", "KeyMergeCellsDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SplitCell, executedRoutedEventHandler, canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Alt+Ctrl+Shift+S", "KeySplitCellDisplayString"));
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x00268844 File Offset: 0x00267844
		private static void OnTableCommand(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			TextEditorSelection._ClearSuggestedX(textEditor);
			if (args.Command == EditingCommands.InsertTable)
			{
				((TextSelection)textEditor.Selection).InsertTable(4, 4);
				return;
			}
			if (args.Command == EditingCommands.InsertRows)
			{
				((TextSelection)textEditor.Selection).InsertRows(1);
				return;
			}
			if (args.Command == EditingCommands.InsertColumns)
			{
				((TextSelection)textEditor.Selection).InsertColumns(1);
				return;
			}
			if (args.Command == EditingCommands.DeleteRows)
			{
				((TextSelection)textEditor.Selection).DeleteRows();
				return;
			}
			if (args.Command == EditingCommands.DeleteColumns)
			{
				((TextSelection)textEditor.Selection).DeleteColumns();
				return;
			}
			if (args.Command == EditingCommands.MergeCells)
			{
				((TextSelection)textEditor.Selection).MergeCells();
				return;
			}
			if (args.Command == EditingCommands.SplitCell)
			{
				((TextSelection)textEditor.Selection).SplitCell(1000, 1000);
			}
		}

		// Token: 0x06005650 RID: 22096 RVA: 0x00262877 File Offset: 0x00261877
		private static void OnQueryStatusNYI(object target, CanExecuteRoutedEventArgs args)
		{
			if (TextEditor._GetTextEditor(target) == null)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x04002F88 RID: 12168
		private const string KeyDeleteColumns = "Alt+Ctrl+Shift+D";

		// Token: 0x04002F89 RID: 12169
		private const string KeyInsertColumns = "Alt+Ctrl+Shift+C";

		// Token: 0x04002F8A RID: 12170
		private const string KeyInsertRows = "Alt+Ctrl+Shift+R";

		// Token: 0x04002F8B RID: 12171
		private const string KeyInsertTable = "Alt+Ctrl+Shift+T";

		// Token: 0x04002F8C RID: 12172
		private const string KeyMergeCells = "Alt+Ctrl+Shift+M";

		// Token: 0x04002F8D RID: 12173
		private const string KeySplitCell = "Alt+Ctrl+Shift+S";
	}
}
