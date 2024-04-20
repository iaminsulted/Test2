using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Xml;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020006A1 RID: 1697
	internal static class TextEditorCopyPaste
	{
		// Token: 0x060055BE RID: 21950 RVA: 0x00262EB8 File Offset: 0x00261EB8
		internal static void _RegisterClassHandlers(Type controlType, bool acceptsRichContent, bool readOnly, bool registerEventListeners)
		{
			CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Copy, new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnCopy), new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusCopy), KeyGesture.CreateFromResourceStrings("Ctrl+C", SR.Get("KeyCopyDisplayString")), KeyGesture.CreateFromResourceStrings("Ctrl+Insert", SR.Get("KeyCtrlInsertDisplayString")));
			if (acceptsRichContent)
			{
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.CopyFormat, new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnCopyFormat), new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusCopyFormat), KeyGesture.CreateFromResourceStrings("Ctrl+Shift+C", "KeyCopyFormatDisplayString"));
			}
			if (!readOnly)
			{
				CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Cut, new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnCut), new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusCut), KeyGesture.CreateFromResourceStrings("Ctrl+X", SR.Get("KeyCutDisplayString")), KeyGesture.CreateFromResourceStrings("Shift+Delete", SR.Get("KeyShiftDeleteDisplayString")));
				ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnPaste);
				CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusPaste);
				InputGesture inputGesture = KeyGesture.CreateFromResourceStrings("Shift+Insert", SR.Get("KeyShiftInsertDisplayString"));
				CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Paste, executedRoutedEventHandler, canExecuteRoutedEventHandler, inputGesture);
				if (acceptsRichContent)
				{
					CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.PasteFormat, new ExecutedRoutedEventHandler(TextEditorCopyPaste.OnPasteFormat), new CanExecuteRoutedEventHandler(TextEditorCopyPaste.OnQueryStatusPasteFormat), "Ctrl+Shift+V", "KeyPasteFormatDisplayString");
				}
			}
		}

		// Token: 0x060055BF RID: 21951 RVA: 0x00263004 File Offset: 0x00262004
		internal static DataObject _CreateDataObject(TextEditor This, bool isDragDrop)
		{
			DataObject dataObject = new DataObject();
			string text = This.Selection.Text;
			if (text != string.Empty)
			{
				if (TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.Text))
				{
					dataObject.SetData(DataFormats.Text, text, false);
				}
				if (TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.UnicodeText))
				{
					dataObject.SetData(DataFormats.UnicodeText, text, false);
				}
			}
			if (This.AcceptsRichContent)
			{
				Stream stream = null;
				string text2 = WpfPayload.SaveRange(This.Selection, ref stream, false);
				if (text2.Length > 0)
				{
					if (stream != null && TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.XamlPackage))
					{
						dataObject.SetData(DataFormats.XamlPackage, stream);
					}
					if (TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.Rtf))
					{
						string text3 = TextEditorCopyPaste.ConvertXamlToRtf(text2, stream);
						if (text3 != string.Empty)
						{
							dataObject.SetData(DataFormats.Rtf, text3, true);
						}
					}
					Image image = This.Selection.GetUIElementSelected() as Image;
					if (image != null && image.Source is BitmapSource)
					{
						dataObject.SetImage((BitmapSource)image.Source);
					}
				}
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				TextRangeSerialization.WriteXaml(new XmlTextWriter(stringWriter), This.Selection, false, null);
				string text4 = stringWriter.ToString();
				if (text4.Length > 0 && TextEditorCopyPaste.ConfirmDataFormatSetting(This.UiScope, dataObject, DataFormats.Xaml))
				{
					dataObject.SetData(DataFormats.Xaml, text4, false);
				}
			}
			DataObjectCopyingEventArgs dataObjectCopyingEventArgs = new DataObjectCopyingEventArgs(dataObject, isDragDrop);
			This.UiScope.RaiseEvent(dataObjectCopyingEventArgs);
			if (dataObjectCopyingEventArgs.CommandCancelled)
			{
				dataObject = null;
			}
			return dataObject;
		}

		// Token: 0x060055C0 RID: 21952 RVA: 0x0026319C File Offset: 0x0026219C
		internal static bool _DoPaste(TextEditor This, IDataObject dataObject, bool isDragDrop)
		{
			Invariant.Assert(dataObject != null);
			bool result = false;
			string formatToApply = TextEditorCopyPaste.GetPasteApplyFormat(This, dataObject);
			DataObjectPastingEventArgs dataObjectPastingEventArgs;
			try
			{
				dataObjectPastingEventArgs = new DataObjectPastingEventArgs(dataObject, isDragDrop, formatToApply);
			}
			catch (ArgumentException)
			{
				return result;
			}
			This.UiScope.RaiseEvent(dataObjectPastingEventArgs);
			if (!dataObjectPastingEventArgs.CommandCancelled)
			{
				IDataObject dataObject2 = dataObjectPastingEventArgs.DataObject;
				formatToApply = dataObjectPastingEventArgs.FormatToApply;
				result = TextEditorCopyPaste.PasteContentData(This, dataObject, dataObject2, formatToApply);
			}
			return result;
		}

		// Token: 0x060055C1 RID: 21953 RVA: 0x00263210 File Offset: 0x00262210
		internal static string GetPasteApplyFormat(TextEditor This, IDataObject dataObject)
		{
			string result;
			if (This.AcceptsRichContent && dataObject.GetDataPresent(DataFormats.XamlPackage))
			{
				result = DataFormats.XamlPackage;
			}
			else if (This.AcceptsRichContent && dataObject.GetDataPresent(DataFormats.Xaml))
			{
				result = DataFormats.Xaml;
			}
			else if (This.AcceptsRichContent && dataObject.GetDataPresent(DataFormats.Rtf))
			{
				result = DataFormats.Rtf;
			}
			else if (dataObject.GetDataPresent(DataFormats.UnicodeText))
			{
				result = DataFormats.UnicodeText;
			}
			else if (dataObject.GetDataPresent(DataFormats.Text))
			{
				result = DataFormats.Text;
			}
			else if (This.AcceptsRichContent && dataObject is DataObject && ((DataObject)dataObject).ContainsImage())
			{
				result = DataFormats.Bitmap;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x002632D0 File Offset: 0x002622D0
		internal static void Cut(TextEditor This, bool userInitiated)
		{
			TextEditorTyping._FlushPendingInputItems(This);
			TextEditorTyping._BreakTypingSequence(This);
			if (This.Selection != null && !This.Selection.IsEmpty)
			{
				DataObject dataObject = TextEditorCopyPaste._CreateDataObject(This, false);
				if (dataObject != null)
				{
					try
					{
						Clipboard.CriticalSetDataObject(dataObject, true);
					}
					catch (ExternalException obj) when (!FrameworkCompatibilityPreferences.ShouldThrowOnCopyOrCutFailure)
					{
						return;
					}
					using (This.Selection.DeclareChangeBlock())
					{
						TextEditorSelection._ClearSuggestedX(This);
						This.Selection.Text = string.Empty;
						if (This.Selection is TextSelection)
						{
							((TextSelection)This.Selection).ClearSpringloadFormatting();
						}
					}
				}
			}
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x0026339C File Offset: 0x0026239C
		internal static void Copy(TextEditor This, bool userInitiated)
		{
			TextEditorTyping._FlushPendingInputItems(This);
			TextEditorTyping._BreakTypingSequence(This);
			if (This.Selection != null && !This.Selection.IsEmpty)
			{
				DataObject dataObject = TextEditorCopyPaste._CreateDataObject(This, false);
				if (dataObject != null)
				{
					try
					{
						Clipboard.CriticalSetDataObject(dataObject, true);
					}
					catch (ExternalException obj) when (!FrameworkCompatibilityPreferences.ShouldThrowOnCopyOrCutFailure)
					{
					}
				}
			}
		}

		// Token: 0x060055C4 RID: 21956 RVA: 0x0026340C File Offset: 0x0026240C
		internal static void Paste(TextEditor This)
		{
			if (This.Selection.IsTableCellRange)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(This);
			TextEditorTyping._BreakTypingSequence(This);
			IDataObject dataObject;
			try
			{
				dataObject = Clipboard.GetDataObject();
			}
			catch (ExternalException)
			{
				dataObject = null;
			}
			bool coversEntireContent = This.Selection.CoversEntireContent;
			if (dataObject != null)
			{
				using (This.Selection.DeclareChangeBlock())
				{
					TextEditorSelection._ClearSuggestedX(This);
					if (TextEditorCopyPaste._DoPaste(This, dataObject, false))
					{
						This.Selection.SetCaretToPosition(This.Selection.End, LogicalDirection.Backward, false, true);
						if (This.Selection is TextSelection)
						{
							((TextSelection)This.Selection).ClearSpringloadFormatting();
						}
					}
				}
			}
			if (coversEntireContent)
			{
				This.Selection.ValidateLayout();
			}
		}

		// Token: 0x060055C5 RID: 21957 RVA: 0x002634D8 File Offset: 0x002624D8
		internal static string ConvertXamlToRtf(string xamlContent, Stream wpfContainerMemory)
		{
			XamlRtfConverter xamlRtfConverter = new XamlRtfConverter();
			if (wpfContainerMemory != null)
			{
				xamlRtfConverter.WpfPayload = WpfPayload.OpenWpfPayload(wpfContainerMemory);
			}
			return xamlRtfConverter.ConvertXamlToRtf(xamlContent);
		}

		// Token: 0x060055C6 RID: 21958 RVA: 0x00263504 File Offset: 0x00262504
		internal static MemoryStream ConvertRtfToXaml(string rtfContent)
		{
			MemoryStream memoryStream = new MemoryStream();
			WpfPayload wpfPayload = WpfPayload.CreateWpfPayload(memoryStream);
			using (wpfPayload.Package)
			{
				using (Stream stream = wpfPayload.CreateXamlStream())
				{
					string text = new XamlRtfConverter
					{
						WpfPayload = wpfPayload
					}.ConvertRtfToXaml(rtfContent);
					if (text != string.Empty)
					{
						StreamWriter streamWriter = new StreamWriter(stream);
						using (streamWriter)
						{
							streamWriter.Write(text);
							return memoryStream;
						}
					}
					memoryStream = null;
				}
			}
			return memoryStream;
		}

		// Token: 0x060055C7 RID: 21959 RVA: 0x002635B4 File Offset: 0x002625B4
		private static void OnQueryStatusCut(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.UiScope is PasswordBox)
			{
				args.CanExecute = false;
				args.Handled = true;
				return;
			}
			args.CanExecute = !textEditor.Selection.IsEmpty;
			args.Handled = true;
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x00263614 File Offset: 0x00262614
		private static void OnCut(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.UiScope is PasswordBox)
			{
				return;
			}
			TextEditorCopyPaste.Cut(textEditor, args.UserInitiated);
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x00263658 File Offset: 0x00262658
		private static void OnQueryStatusCopy(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.UiScope is PasswordBox)
			{
				args.CanExecute = false;
				args.Handled = true;
				return;
			}
			args.CanExecute = !textEditor.Selection.IsEmpty;
			args.Handled = true;
		}

		// Token: 0x060055CA RID: 21962 RVA: 0x002636B0 File Offset: 0x002626B0
		private static void OnCopy(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.UiScope is PasswordBox)
			{
				return;
			}
			TextEditorCopyPaste.Copy(textEditor, args.UserInitiated);
		}

		// Token: 0x060055CB RID: 21963 RVA: 0x002636EC File Offset: 0x002626EC
		private static void OnQueryStatusPaste(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			args.Handled = true;
			try
			{
				string pasteApplyFormat = TextEditorCopyPaste.GetPasteApplyFormat(textEditor, Clipboard.GetDataObject());
				args.CanExecute = (pasteApplyFormat.Length > 0);
			}
			catch (ExternalException)
			{
				args.CanExecute = false;
			}
		}

		// Token: 0x060055CC RID: 21964 RVA: 0x00263754 File Offset: 0x00262754
		private static void OnPaste(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			TextEditorCopyPaste.Paste(textEditor);
		}

		// Token: 0x060055CD RID: 21965 RVA: 0x00263784 File Offset: 0x00262784
		private static void OnQueryStatusCopyFormat(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			args.CanExecute = false;
			args.Handled = true;
		}

		// Token: 0x060055CE RID: 21966 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void OnCopyFormat(object sender, ExecutedRoutedEventArgs args)
		{
		}

		// Token: 0x060055CF RID: 21967 RVA: 0x002637B4 File Offset: 0x002627B4
		private static void OnQueryStatusPasteFormat(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			args.CanExecute = false;
			args.Handled = true;
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void OnPasteFormat(object sender, ExecutedRoutedEventArgs args)
		{
		}

		// Token: 0x060055D1 RID: 21969 RVA: 0x002637EC File Offset: 0x002627EC
		private static bool PasteContentData(TextEditor This, IDataObject dataObject, IDataObject dataObjectToApply, string formatToApply)
		{
			if (formatToApply == DataFormats.Bitmap && dataObjectToApply is DataObject && This.AcceptsRichContent && This.Selection is TextSelection)
			{
				BitmapSource bitmapSource = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.Bitmap) as BitmapSource;
				if (bitmapSource != null)
				{
					MemoryStream data = WpfPayload.SaveImage(bitmapSource, "image/bmp");
					dataObjectToApply = new DataObject();
					formatToApply = DataFormats.XamlPackage;
					dataObjectToApply.SetData(DataFormats.XamlPackage, data);
				}
			}
			if (formatToApply == DataFormats.XamlPackage)
			{
				if (This.AcceptsRichContent && This.Selection is TextSelection)
				{
					MemoryStream memoryStream = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.XamlPackage) as MemoryStream;
					if (memoryStream != null)
					{
						object obj = WpfPayload.LoadElement(memoryStream);
						if ((obj is Section || obj is Span) && TextEditorCopyPaste.PasteTextElement(This, (TextElement)obj))
						{
							return true;
						}
						if (obj is FrameworkElement)
						{
							((TextSelection)This.Selection).InsertEmbeddedUIElement((FrameworkElement)obj);
							return true;
						}
					}
				}
				dataObjectToApply = dataObject;
				if (dataObjectToApply.GetDataPresent(DataFormats.Xaml))
				{
					formatToApply = DataFormats.Xaml;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.Rtf))
				{
					formatToApply = DataFormats.Rtf;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.UnicodeText))
				{
					formatToApply = DataFormats.UnicodeText;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.Text))
				{
					formatToApply = DataFormats.Text;
				}
			}
			if (formatToApply == DataFormats.Xaml)
			{
				if (This.AcceptsRichContent && This.Selection is TextSelection)
				{
					object pasteData = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.Xaml);
					if (pasteData != null && TextEditorCopyPaste.PasteXaml(This, pasteData.ToString()))
					{
						return true;
					}
				}
				dataObjectToApply = dataObject;
				if (dataObjectToApply.GetDataPresent(DataFormats.Rtf))
				{
					formatToApply = DataFormats.Rtf;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.UnicodeText))
				{
					formatToApply = DataFormats.UnicodeText;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.Text))
				{
					formatToApply = DataFormats.Text;
				}
			}
			if (formatToApply == DataFormats.Rtf)
			{
				if (This.AcceptsRichContent)
				{
					object pasteData2 = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.Rtf);
					if (pasteData2 != null)
					{
						MemoryStream memoryStream2 = TextEditorCopyPaste.ConvertRtfToXaml(pasteData2.ToString());
						if (memoryStream2 != null)
						{
							TextElement textElement = WpfPayload.LoadElement(memoryStream2) as TextElement;
							if ((textElement is Section || textElement is Span) && TextEditorCopyPaste.PasteTextElement(This, textElement))
							{
								return true;
							}
						}
					}
				}
				dataObjectToApply = dataObject;
				if (dataObjectToApply.GetDataPresent(DataFormats.UnicodeText))
				{
					formatToApply = DataFormats.UnicodeText;
				}
				else if (dataObjectToApply.GetDataPresent(DataFormats.Text))
				{
					formatToApply = DataFormats.Text;
				}
			}
			if (formatToApply == DataFormats.UnicodeText)
			{
				object pasteData3 = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.UnicodeText);
				if (pasteData3 != null)
				{
					return TextEditorCopyPaste.PastePlainText(This, pasteData3.ToString());
				}
				if (dataObjectToApply.GetDataPresent(DataFormats.Text))
				{
					formatToApply = DataFormats.Text;
					dataObjectToApply = dataObject;
				}
			}
			if (formatToApply == DataFormats.Text)
			{
				object pasteData4 = TextEditorCopyPaste.GetPasteData(dataObjectToApply, DataFormats.Text);
				if (pasteData4 != null && TextEditorCopyPaste.PastePlainText(This, pasteData4.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060055D2 RID: 21970 RVA: 0x00263AC8 File Offset: 0x00262AC8
		private static object GetPasteData(IDataObject dataObject, string dataFormat)
		{
			object result;
			try
			{
				result = dataObject.GetData(dataFormat, true);
			}
			catch (OutOfMemoryException)
			{
				result = null;
			}
			catch (ExternalException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060055D3 RID: 21971 RVA: 0x00263B08 File Offset: 0x00262B08
		private static bool PasteTextElement(TextEditor This, TextElement sectionOrSpan)
		{
			bool result = false;
			This.Selection.BeginChange();
			try
			{
				((TextRange)This.Selection).SetXmlVirtual(sectionOrSpan);
				TextRangeEditLists.MergeListsAroundNormalizedPosition((TextPointer)This.Selection.Start);
				TextRangeEditLists.MergeListsAroundNormalizedPosition((TextPointer)This.Selection.End);
				TextRangeEdit.MergeFlowDirection((TextPointer)This.Selection.Start);
				TextRangeEdit.MergeFlowDirection((TextPointer)This.Selection.End);
				result = true;
			}
			finally
			{
				This.Selection.EndChange();
			}
			return result;
		}

		// Token: 0x060055D4 RID: 21972 RVA: 0x00263BAC File Offset: 0x00262BAC
		private static bool PasteXaml(TextEditor This, string pasteXaml)
		{
			bool result;
			if (pasteXaml.Length == 0)
			{
				result = false;
			}
			else
			{
				try
				{
					TextElement textElement = XamlReader.Load(new XmlTextReader(new StringReader(pasteXaml)), true) as TextElement;
					result = (textElement != null && TextEditorCopyPaste.PasteTextElement(This, textElement));
				}
				catch (XamlParseException ex)
				{
					Invariant.Assert(ex != null);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060055D5 RID: 21973 RVA: 0x00263C0C File Offset: 0x00262C0C
		private static bool PastePlainText(TextEditor This, string pastedText)
		{
			pastedText = This._FilterText(pastedText, This.Selection);
			if (pastedText.Length > 0)
			{
				if (This.AcceptsRichContent && This.Selection.Start is TextPointer)
				{
					This.Selection.Text = string.Empty;
					TextPointer textPointer = TextRangeEditTables.EnsureInsertionPosition((TextPointer)This.Selection.Start);
					textPointer = textPointer.GetPositionAtOffset(0, LogicalDirection.Backward);
					TextPointer textPointer2 = textPointer.GetPositionAtOffset(0, LogicalDirection.Forward);
					int num = 0;
					for (int i = 0; i < pastedText.Length; i++)
					{
						if (pastedText[i] == '\r' || pastedText[i] == '\n')
						{
							textPointer2.InsertTextInRun(pastedText.Substring(num, i - num));
							if (!This.AcceptsReturn)
							{
								return true;
							}
							if (textPointer2.HasNonMergeableInlineAncestor)
							{
								textPointer2.InsertTextInRun(" ");
							}
							else
							{
								textPointer2 = textPointer2.InsertParagraphBreak();
							}
							if (pastedText[i] == '\r' && i + 1 < pastedText.Length && pastedText[i + 1] == '\n')
							{
								i++;
							}
							num = i + 1;
						}
					}
					textPointer2.InsertTextInRun(pastedText.Substring(num, pastedText.Length - num));
					This.Selection.Select(textPointer, textPointer2);
				}
				else
				{
					This.Selection.Text = pastedText;
				}
				return true;
			}
			return false;
		}

		// Token: 0x060055D6 RID: 21974 RVA: 0x00263D4C File Offset: 0x00262D4C
		private static bool ConfirmDataFormatSetting(FrameworkElement uiScope, IDataObject dataObject, string format)
		{
			DataObjectSettingDataEventArgs dataObjectSettingDataEventArgs = new DataObjectSettingDataEventArgs(dataObject, format);
			uiScope.RaiseEvent(dataObjectSettingDataEventArgs);
			return !dataObjectSettingDataEventArgs.CommandCancelled;
		}

		// Token: 0x04002F4F RID: 12111
		private const string KeyCopy = "Ctrl+C";

		// Token: 0x04002F50 RID: 12112
		private const string KeyCopyFormat = "Ctrl+Shift+C";

		// Token: 0x04002F51 RID: 12113
		private const string KeyCtrlInsert = "Ctrl+Insert";

		// Token: 0x04002F52 RID: 12114
		private const string KeyCut = "Ctrl+X";

		// Token: 0x04002F53 RID: 12115
		private const string KeyPasteFormat = "Ctrl+Shift+V";

		// Token: 0x04002F54 RID: 12116
		private const string KeyShiftDelete = "Shift+Delete";

		// Token: 0x04002F55 RID: 12117
		private const string KeyShiftInsert = "Shift+Insert";
	}
}
