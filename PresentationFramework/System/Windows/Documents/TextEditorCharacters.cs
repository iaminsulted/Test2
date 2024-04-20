using System;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x0200069F RID: 1695
	internal static class TextEditorCharacters
	{
		// Token: 0x060055A3 RID: 21923 RVA: 0x00261F2C File Offset: 0x00260F2C
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorCharacters.OnQueryStatusNYI);
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ResetFormat, new ExecutedRoutedEventHandler(TextEditorCharacters.OnResetFormat), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+Space", "KeyResetFormatDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleBold, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleBold), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+B", "KeyToggleBoldDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleItalic, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleItalic), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+I", "KeyToggleItalicDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleUnderline, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleUnderline), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+U", "KeyToggleUnderlineDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleSubscript, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleSubscript), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+OemPlus", "KeyToggleSubscriptDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleSuperscript, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleSuperscript), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+Shift+OemPlus", "KeyToggleSuperscriptDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.IncreaseFontSize, new ExecutedRoutedEventHandler(TextEditorCharacters.OnIncreaseFontSize), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+OemCloseBrackets", "KeyIncreaseFontSizeDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DecreaseFontSize, new ExecutedRoutedEventHandler(TextEditorCharacters.OnDecreaseFontSize), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings("Ctrl+OemOpenBrackets", "KeyDecreaseFontSizeDisplayString"));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyFontSize, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyFontSize), canExecuteRoutedEventHandler, "KeyApplyFontSize", "KeyApplyFontSizeDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyFontFamily, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyFontFamily), canExecuteRoutedEventHandler, "KeyApplyFontFamily", "KeyApplyFontFamilyDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyForeground, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyForeground), canExecuteRoutedEventHandler, "KeyApplyForeground", "KeyApplyForegroundDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyBackground, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyBackground), canExecuteRoutedEventHandler, "KeyApplyBackground", "KeyApplyBackgroundDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleSpellCheck, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleSpellCheck), canExecuteRoutedEventHandler, "KeyToggleSpellCheck", "KeyToggleSpellCheckDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyInlineFlowDirectionRTL, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyInlineFlowDirectionRTL), new CanExecuteRoutedEventHandler(TextEditorCharacters.OnQueryStatusNYI));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyInlineFlowDirectionLTR, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyInlineFlowDirectionLTR), new CanExecuteRoutedEventHandler(TextEditorCharacters.OnQueryStatusNYI));
		}

		// Token: 0x060055A4 RID: 21924 RVA: 0x0026216E File Offset: 0x0026116E
		internal static void _OnApplyProperty(TextEditor This, DependencyProperty formattingProperty, object propertyValue)
		{
			TextEditorCharacters._OnApplyProperty(This, formattingProperty, propertyValue, false, PropertyValueAction.SetValue);
		}

		// Token: 0x060055A5 RID: 21925 RVA: 0x0026217A File Offset: 0x0026117A
		internal static void _OnApplyProperty(TextEditor This, DependencyProperty formattingProperty, object propertyValue, bool applyToParagraphs)
		{
			TextEditorCharacters._OnApplyProperty(This, formattingProperty, propertyValue, applyToParagraphs, PropertyValueAction.SetValue);
		}

		// Token: 0x060055A6 RID: 21926 RVA: 0x00262188 File Offset: 0x00261188
		internal static void _OnApplyProperty(TextEditor This, DependencyProperty formattingProperty, object propertyValue, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			if (This == null || !This._IsEnabled || This.IsReadOnly || !This.AcceptsRichContent || !(This.Selection is TextSelection))
			{
				return;
			}
			if (!TextSchema.IsParagraphProperty(formattingProperty) && !TextSchema.IsCharacterProperty(formattingProperty))
			{
				Invariant.Assert(false, "The property '" + formattingProperty.Name + "' is unknown to TextEditor");
				return;
			}
			TextSelection textSelection = (TextSelection)This.Selection;
			if (TextSchema.IsStructuralCharacterProperty(formattingProperty) && !TextRangeEdit.CanApplyStructuralInlineProperty(textSelection.Start, textSelection.End))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(This);
			TextEditorSelection._ClearSuggestedX(This);
			TextEditorTyping._BreakTypingSequence(This);
			textSelection.ApplyPropertyValue(formattingProperty, propertyValue, applyToParagraphs, propertyValueAction);
		}

		// Token: 0x060055A7 RID: 21927 RVA: 0x00262230 File Offset: 0x00261230
		private static void OnResetFormat(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection.Start is TextPointer))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextPointer start = (TextPointer)textEditor.Selection.Start;
				TextPointer end = (TextPointer)textEditor.Selection.End;
				if (textEditor.Selection.IsEmpty)
				{
					TextSegment autoWord = TextRangeBase.GetAutoWord(textEditor.Selection);
					if (autoWord.IsNull)
					{
						((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
						return;
					}
					start = (TextPointer)autoWord.Start;
					end = (TextPointer)autoWord.End;
				}
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextRangeEdit.CharacterResetFormatting(start, end);
			}
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x00262320 File Offset: 0x00261320
		private static void OnToggleBold(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			object currentValue = ((TextSelection)textEditor.Selection).GetCurrentValue(TextElement.FontWeightProperty);
			FontWeight fontWeight = (currentValue != DependencyProperty.UnsetValue && (FontWeight)currentValue == FontWeights.Bold) ? FontWeights.Normal : FontWeights.Bold;
			TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontWeightProperty, fontWeight);
		}

		// Token: 0x060055A9 RID: 21929 RVA: 0x002623B4 File Offset: 0x002613B4
		private static void OnToggleItalic(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			object currentValue = ((TextSelection)textEditor.Selection).GetCurrentValue(TextElement.FontStyleProperty);
			FontStyle fontStyle = (currentValue != DependencyProperty.UnsetValue && (FontStyle)currentValue == FontStyles.Italic) ? FontStyles.Normal : FontStyles.Italic;
			TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontStyleProperty, fontStyle);
			textEditor.Selection.RefreshCaret();
		}

		// Token: 0x060055AA RID: 21930 RVA: 0x00262450 File Offset: 0x00261450
		private static void OnToggleUnderline(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			object currentValue = ((TextSelection)textEditor.Selection).GetCurrentValue(Inline.TextDecorationsProperty);
			TextDecorationCollection textDecorationCollection = (currentValue != DependencyProperty.UnsetValue) ? ((TextDecorationCollection)currentValue) : null;
			TextDecorationCollection underline;
			if (!TextSchema.HasTextDecorations(textDecorationCollection))
			{
				underline = TextDecorations.Underline;
			}
			else if (!textDecorationCollection.TryRemove(TextDecorations.Underline, out underline))
			{
				underline.Add(TextDecorations.Underline);
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Inline.TextDecorationsProperty, underline);
		}

		// Token: 0x060055AB RID: 21931 RVA: 0x002624F4 File Offset: 0x002614F4
		private static void OnToggleSubscript(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			FontVariants fontVariants = (FontVariants)((TextSelection)textEditor.Selection).GetCurrentValue(Typography.VariantsProperty);
			fontVariants = ((fontVariants == FontVariants.Subscript) ? FontVariants.Normal : FontVariants.Subscript);
			TextEditorCharacters._OnApplyProperty(textEditor, Typography.VariantsProperty, fontVariants);
		}

		// Token: 0x060055AC RID: 21932 RVA: 0x0026256C File Offset: 0x0026156C
		private static void OnToggleSuperscript(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			FontVariants fontVariants = (FontVariants)((TextSelection)textEditor.Selection).GetCurrentValue(Typography.VariantsProperty);
			fontVariants = ((fontVariants == FontVariants.Superscript) ? FontVariants.Normal : FontVariants.Superscript);
			TextEditorCharacters._OnApplyProperty(textEditor, Typography.VariantsProperty, fontVariants);
		}

		// Token: 0x060055AD RID: 21933 RVA: 0x002625E4 File Offset: 0x002615E4
		private static void OnIncreaseFontSize(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (textEditor.Selection.IsEmpty)
			{
				double num = (double)((TextSelection)textEditor.Selection).GetCurrentValue(TextElement.FontSizeProperty);
				if (num == 0.0)
				{
					return;
				}
				if (num < 1638.0)
				{
					num += 0.75;
					if (num > 1638.0)
					{
						num = 1638.0;
					}
					TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontSizeProperty, num);
					return;
				}
			}
			else
			{
				TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontSizeProperty, 0.75, false, PropertyValueAction.IncreaseByAbsoluteValue);
			}
		}

		// Token: 0x060055AE RID: 21934 RVA: 0x002626B8 File Offset: 0x002616B8
		private static void OnDecreaseFontSize(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (textEditor.Selection.IsEmpty)
			{
				double num = (double)((TextSelection)textEditor.Selection).GetCurrentValue(TextElement.FontSizeProperty);
				if (num == 0.0)
				{
					return;
				}
				if (num > 0.75)
				{
					num -= 0.75;
					if (num < 0.75)
					{
						num = 0.75;
					}
					TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontSizeProperty, num);
					return;
				}
			}
			else
			{
				TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontSizeProperty, 0.75, false, PropertyValueAction.DecreaseByAbsoluteValue);
			}
		}

		// Token: 0x060055AF RID: 21935 RVA: 0x0026278B File Offset: 0x0026178B
		private static void OnApplyFontSize(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Parameter == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(TextEditor._GetTextEditor(target), TextElement.FontSizeProperty, args.Parameter);
		}

		// Token: 0x060055B0 RID: 21936 RVA: 0x002627AC File Offset: 0x002617AC
		private static void OnApplyFontFamily(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Parameter == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(TextEditor._GetTextEditor(target), TextElement.FontFamilyProperty, args.Parameter);
		}

		// Token: 0x060055B1 RID: 21937 RVA: 0x002627CD File Offset: 0x002617CD
		private static void OnApplyForeground(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Parameter == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(TextEditor._GetTextEditor(target), TextElement.ForegroundProperty, args.Parameter);
		}

		// Token: 0x060055B2 RID: 21938 RVA: 0x002627EE File Offset: 0x002617EE
		private static void OnApplyBackground(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Parameter == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(TextEditor._GetTextEditor(target), TextElement.BackgroundProperty, args.Parameter);
		}

		// Token: 0x060055B3 RID: 21939 RVA: 0x00262810 File Offset: 0x00261810
		private static void OnToggleSpellCheck(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			textEditor.IsSpellCheckEnabled = !textEditor.IsSpellCheckEnabled;
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x00262847 File Offset: 0x00261847
		private static void OnApplyInlineFlowDirectionRTL(object target, ExecutedRoutedEventArgs args)
		{
			TextEditorCharacters._OnApplyProperty(TextEditor._GetTextEditor(target), Inline.FlowDirectionProperty, FlowDirection.RightToLeft);
		}

		// Token: 0x060055B5 RID: 21941 RVA: 0x0026285F File Offset: 0x0026185F
		private static void OnApplyInlineFlowDirectionLTR(object target, ExecutedRoutedEventArgs args)
		{
			TextEditorCharacters._OnApplyProperty(TextEditor._GetTextEditor(target), Inline.FlowDirectionProperty, FlowDirection.LeftToRight);
		}

		// Token: 0x060055B6 RID: 21942 RVA: 0x00262877 File Offset: 0x00261877
		private static void OnQueryStatusNYI(object target, CanExecuteRoutedEventArgs args)
		{
			if (TextEditor._GetTextEditor(target) == null)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x04002F45 RID: 12101
		internal const double OneFontPoint = 0.75;

		// Token: 0x04002F46 RID: 12102
		internal const double MaxFontPoint = 1638.0;

		// Token: 0x04002F47 RID: 12103
		private const string KeyDecreaseFontSize = "Ctrl+OemOpenBrackets";

		// Token: 0x04002F48 RID: 12104
		private const string KeyIncreaseFontSize = "Ctrl+OemCloseBrackets";

		// Token: 0x04002F49 RID: 12105
		private const string KeyResetFormat = "Ctrl+Space";

		// Token: 0x04002F4A RID: 12106
		private const string KeyToggleBold = "Ctrl+B";

		// Token: 0x04002F4B RID: 12107
		private const string KeyToggleItalic = "Ctrl+I";

		// Token: 0x04002F4C RID: 12108
		private const string KeyToggleSubscript = "Ctrl+OemPlus";

		// Token: 0x04002F4D RID: 12109
		private const string KeyToggleSuperscript = "Ctrl+Shift+OemPlus";

		// Token: 0x04002F4E RID: 12110
		private const string KeyToggleUnderline = "Ctrl+U";
	}
}
