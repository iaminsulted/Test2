using System;
using System.Windows.Input;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020006A5 RID: 1701
	internal static class TextEditorParagraphs
	{
		// Token: 0x060055FB RID: 22011 RVA: 0x002652E8 File Offset: 0x002642E8
		internal static void _RegisterClassHandlers(Type controlType, bool acceptsRichContent, bool registerEventListeners)
		{
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorParagraphs.OnQueryStatusNYI);
			if (acceptsRichContent)
			{
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.AlignLeft, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnAlignLeft), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+L", "KeyAlignLeftDisplayString"));
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.AlignCenter, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnAlignCenter), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+E", "KeyAlignCenterDisplayString"));
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.AlignRight, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnAlignRight), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+R", "KeyAlignRightDisplayString"));
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.AlignJustify, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnAlignJustify), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+J", "KeyAlignJustifyDisplayString"));
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplySingleSpace, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplySingleSpace), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+1", "KeyApplySingleSpaceDisplayString"));
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyOneAndAHalfSpace, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplyOneAndAHalfSpace), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+5", "KeyApplyOneAndAHalfSpaceDisplayString"));
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyDoubleSpace, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplyDoubleSpace), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+2", "KeyApplyDoubleSpaceDisplayString"));
			}
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyParagraphFlowDirectionLTR, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplyParagraphFlowDirectionLTR), canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyParagraphFlowDirectionRTL, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplyParagraphFlowDirectionRTL), canExecuteRoutedEventHandler);
		}

		// Token: 0x060055FC RID: 22012 RVA: 0x0026544C File Offset: 0x0026444C
		private static void OnAlignLeft(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Block.TextAlignmentProperty, TextAlignment.Left, true);
		}

		// Token: 0x060055FD RID: 22013 RVA: 0x00265478 File Offset: 0x00264478
		private static void OnAlignCenter(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Block.TextAlignmentProperty, TextAlignment.Center, true);
		}

		// Token: 0x060055FE RID: 22014 RVA: 0x002654A4 File Offset: 0x002644A4
		private static void OnAlignRight(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Block.TextAlignmentProperty, TextAlignment.Right, true);
		}

		// Token: 0x060055FF RID: 22015 RVA: 0x002654D0 File Offset: 0x002644D0
		private static void OnAlignJustify(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Block.TextAlignmentProperty, TextAlignment.Justify, true);
		}

		// Token: 0x06005600 RID: 22016 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void OnApplySingleSpace(object sender, ExecutedRoutedEventArgs e)
		{
		}

		// Token: 0x06005601 RID: 22017 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void OnApplyOneAndAHalfSpace(object sender, ExecutedRoutedEventArgs e)
		{
		}

		// Token: 0x06005602 RID: 22018 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void OnApplyDoubleSpace(object sender, ExecutedRoutedEventArgs e)
		{
		}

		// Token: 0x06005603 RID: 22019 RVA: 0x002654FA File Offset: 0x002644FA
		private static void OnApplyParagraphFlowDirectionLTR(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditorCharacters._OnApplyProperty(TextEditor._GetTextEditor(sender), FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight, true);
		}

		// Token: 0x06005604 RID: 22020 RVA: 0x00265513 File Offset: 0x00264513
		private static void OnApplyParagraphFlowDirectionRTL(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditorCharacters._OnApplyProperty(TextEditor._GetTextEditor(sender), FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft, true);
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x00262877 File Offset: 0x00261877
		private static void OnQueryStatusNYI(object sender, CanExecuteRoutedEventArgs e)
		{
			if (TextEditor._GetTextEditor(sender) == null)
			{
				return;
			}
			e.CanExecute = true;
		}

		// Token: 0x04002F5C RID: 12124
		private const string KeyAlignCenter = "Ctrl+E";

		// Token: 0x04002F5D RID: 12125
		private const string KeyAlignJustify = "Ctrl+J";

		// Token: 0x04002F5E RID: 12126
		private const string KeyAlignLeft = "Ctrl+L";

		// Token: 0x04002F5F RID: 12127
		private const string KeyAlignRight = "Ctrl+R";

		// Token: 0x04002F60 RID: 12128
		private const string KeyApplyDoubleSpace = "Ctrl+2";

		// Token: 0x04002F61 RID: 12129
		private const string KeyApplyOneAndAHalfSpace = "Ctrl+5";

		// Token: 0x04002F62 RID: 12130
		private const string KeyApplySingleSpace = "Ctrl+1";
	}
}
