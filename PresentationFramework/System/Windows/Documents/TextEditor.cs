using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x0200069E RID: 1694
	internal class TextEditor
	{
		// Token: 0x0600553E RID: 21822 RVA: 0x002608A0 File Offset: 0x0025F8A0
		internal TextEditor(ITextContainer textContainer, FrameworkElement uiScope, bool isUndoEnabled)
		{
			Invariant.Assert(uiScope != null);
			this._acceptsRichContent = true;
			this._textContainer = textContainer;
			this._uiScope = uiScope;
			if (isUndoEnabled && this._textContainer is TextContainer)
			{
				((TextContainer)this._textContainer).EnableUndo(this._uiScope);
			}
			this._selection = new TextSelection(this);
			textContainer.TextSelection = this._selection;
			this._dragDropProcess = new TextEditorDragDrop._DragDropProcess(this);
			this._cursor = Cursors.IBeam;
			TextEditorTyping._AddInputLanguageChangedEventHandler(this);
			this.TextContainer.Changed += this.OnTextContainerChanged;
			this._uiScope.IsEnabledChanged += TextEditor.OnIsEnabledChanged;
			this._uiScope.SetValue(TextEditor.InstanceProperty, this);
			if ((bool)this._uiScope.GetValue(SpellCheck.IsEnabledProperty))
			{
				this.SetSpellCheckEnabled(true);
				this.SetCustomDictionaries(true);
			}
			if (!TextServicesLoader.ServicesInstalled)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0600553F RID: 21823 RVA: 0x002609A0 File Offset: 0x0025F9A0
		~TextEditor()
		{
			this.DetachTextStore(true);
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x002609D0 File Offset: 0x0025F9D0
		internal void OnDetach()
		{
			Invariant.Assert(this._textContainer != null);
			this.SetSpellCheckEnabled(false);
			if (UndoManager.GetUndoManager(this._uiScope) != null)
			{
				if (this._textContainer is TextContainer)
				{
					((TextContainer)this._textContainer).DisableUndo(this._uiScope);
				}
				else
				{
					UndoManager.DetachUndoManager(this._uiScope);
				}
			}
			this._textContainer.TextSelection = null;
			TextEditorTyping._RemoveInputLanguageChangedEventHandler(this);
			this._textContainer.Changed -= this.OnTextContainerChanged;
			this._uiScope.IsEnabledChanged -= TextEditor.OnIsEnabledChanged;
			this._pendingTextStoreInit = false;
			this.DetachTextStore(false);
			if (this._immCompositionForDetach != null)
			{
				ImmComposition immComposition;
				if (this._immCompositionForDetach.TryGetTarget(out immComposition))
				{
					immComposition.OnDetach(this);
				}
				this._immComposition = null;
				this._immCompositionForDetach = null;
			}
			this.TextView = null;
			this._selection.OnDetach();
			this._selection = null;
			this._uiScope.ClearValue(TextEditor.InstanceProperty);
			this._uiScope = null;
			this._textContainer = null;
		}

		// Token: 0x06005541 RID: 21825 RVA: 0x00260AE0 File Offset: 0x0025FAE0
		private void DetachTextStore(bool finalizer)
		{
			if (this._textstore != null)
			{
				this._textstore.OnDetach(finalizer);
				this._textstore = null;
			}
			if (this._weakThis != null)
			{
				this._weakThis.StopListening();
				this._weakThis = null;
			}
			if (!finalizer)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06005542 RID: 21826 RVA: 0x00260B20 File Offset: 0x0025FB20
		internal void SetSpellCheckEnabled(bool value)
		{
			value = (value && !this.IsReadOnly && this._IsEnabled);
			if (value && this._speller == null)
			{
				this._speller = new Speller(this);
				return;
			}
			if (!value && this._speller != null)
			{
				this._speller.Detach();
				this._speller = null;
			}
		}

		// Token: 0x06005543 RID: 21827 RVA: 0x00260B78 File Offset: 0x0025FB78
		internal void SetCustomDictionaries(bool add)
		{
			TextBoxBase textBoxBase = this._uiScope as TextBoxBase;
			if (textBoxBase == null)
			{
				return;
			}
			if (this._speller != null)
			{
				CustomDictionarySources dictionaryLocations = (CustomDictionarySources)SpellCheck.GetCustomDictionaries(textBoxBase);
				this._speller.SetCustomDictionaries(dictionaryLocations, add);
			}
		}

		// Token: 0x06005544 RID: 21828 RVA: 0x00260BB6 File Offset: 0x0025FBB6
		internal void SetSpellingReform(SpellingReform spellingReform)
		{
			if (this._speller != null)
			{
				this._speller.SetSpellingReform(spellingReform);
			}
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x00260BCC File Offset: 0x0025FBCC
		internal static ITextView GetTextView(UIElement scope)
		{
			IServiceProvider serviceProvider = scope as IServiceProvider;
			if (serviceProvider == null)
			{
				return null;
			}
			return serviceProvider.GetService(typeof(ITextView)) as ITextView;
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x00260BFC File Offset: 0x0025FBFC
		internal static ITextSelection GetTextSelection(FrameworkElement frameworkElement)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(frameworkElement);
			if (textEditor != null)
			{
				return textEditor.Selection;
			}
			return null;
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x00260C1C File Offset: 0x0025FC1C
		internal static void RegisterCommandHandlers(Type controlType, bool acceptsRichContent, bool readOnly, bool registerEventListeners)
		{
			Invariant.Assert(TextEditor._registeredEditingTypes != null);
			ArrayList registeredEditingTypes = TextEditor._registeredEditingTypes;
			lock (registeredEditingTypes)
			{
				for (int i = 0; i < TextEditor._registeredEditingTypes.Count; i++)
				{
					if (((Type)TextEditor._registeredEditingTypes[i]).IsAssignableFrom(controlType))
					{
						return;
					}
					if (controlType.IsAssignableFrom((Type)TextEditor._registeredEditingTypes[i]))
					{
						throw new InvalidOperationException(SR.Get("TextEditorCanNotRegisterCommandHandler", new object[]
						{
							((Type)TextEditor._registeredEditingTypes[i]).Name,
							controlType.Name
						}));
					}
				}
				TextEditor._registeredEditingTypes.Add(controlType);
			}
			TextEditorMouse._RegisterClassHandlers(controlType, registerEventListeners);
			if (!readOnly)
			{
				TextEditorTyping._RegisterClassHandlers(controlType, registerEventListeners);
			}
			TextEditorDragDrop._RegisterClassHandlers(controlType, readOnly, registerEventListeners);
			TextEditorCopyPaste._RegisterClassHandlers(controlType, acceptsRichContent, readOnly, registerEventListeners);
			TextEditorSelection._RegisterClassHandlers(controlType, registerEventListeners);
			if (!readOnly)
			{
				TextEditorParagraphs._RegisterClassHandlers(controlType, acceptsRichContent, registerEventListeners);
			}
			TextEditorContextMenu._RegisterClassHandlers(controlType, registerEventListeners);
			if (!readOnly)
			{
				TextEditorSpelling._RegisterClassHandlers(controlType, registerEventListeners);
			}
			if (acceptsRichContent && !readOnly)
			{
				TextEditorCharacters._RegisterClassHandlers(controlType, registerEventListeners);
				TextEditorLists._RegisterClassHandlers(controlType, registerEventListeners);
				if (TextEditor._isTableEditingEnabled)
				{
					TextEditorTables._RegisterClassHandlers(controlType, registerEventListeners);
				}
			}
			if (registerEventListeners)
			{
				EventManager.RegisterClassHandler(controlType, Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(TextEditor.OnGotKeyboardFocus));
				EventManager.RegisterClassHandler(controlType, Keyboard.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(TextEditor.OnLostKeyboardFocus));
				EventManager.RegisterClassHandler(controlType, UIElement.LostFocusEvent, new RoutedEventHandler(TextEditor.OnLostFocus));
			}
			if (!readOnly)
			{
				CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Undo, new ExecutedRoutedEventHandler(TextEditor.OnUndo), new CanExecuteRoutedEventHandler(TextEditor.OnQueryStatusUndo), KeyGesture.CreateFromResourceStrings("Ctrl+Z", SR.Get("KeyUndoDisplayString")), KeyGesture.CreateFromResourceStrings("Alt+Backspace", SR.Get("KeyAltUndoDisplayString")));
				CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Redo, new ExecutedRoutedEventHandler(TextEditor.OnRedo), new CanExecuteRoutedEventHandler(TextEditor.OnQueryStatusRedo), KeyGesture.CreateFromResourceStrings("Ctrl+Y", "KeyRedoDisplayString"));
			}
		}

		// Token: 0x06005548 RID: 21832 RVA: 0x00260E20 File Offset: 0x0025FE20
		internal SpellingError GetSpellingErrorAtPosition(ITextPointer position, LogicalDirection direction)
		{
			return TextEditorSpelling.GetSpellingErrorAtPosition(this, position, direction);
		}

		// Token: 0x06005549 RID: 21833 RVA: 0x00260E2A File Offset: 0x0025FE2A
		internal SpellingError GetSpellingErrorAtSelection()
		{
			return TextEditorSpelling.GetSpellingErrorAtSelection(this);
		}

		// Token: 0x0600554A RID: 21834 RVA: 0x00260E32 File Offset: 0x0025FE32
		internal ITextPointer GetNextSpellingErrorPosition(ITextPointer position, LogicalDirection direction)
		{
			return TextEditorSpelling.GetNextSpellingErrorPosition(this, position, direction);
		}

		// Token: 0x0600554B RID: 21835 RVA: 0x00260E3C File Offset: 0x0025FE3C
		internal void SetText(ITextRange range, string text, CultureInfo cultureInfo)
		{
			range.Text = text;
			if (range is TextRange)
			{
				this.MarkCultureProperty((TextRange)range, cultureInfo);
			}
		}

		// Token: 0x0600554C RID: 21836 RVA: 0x00260E5A File Offset: 0x0025FE5A
		internal void SetSelectedText(string text, CultureInfo cultureInfo)
		{
			this.SetText(this.Selection, text, cultureInfo);
			((TextSelection)this.Selection).ApplySpringloadFormatting();
			TextEditorSelection._ClearSuggestedX(this);
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x00260E80 File Offset: 0x0025FE80
		internal void MarkCultureProperty(TextRange range, CultureInfo inputCultureInfo)
		{
			Invariant.Assert(this.UiScope != null);
			if (!this.AcceptsRichContent)
			{
				return;
			}
			XmlLanguage xmlLanguage = (XmlLanguage)((ITextPointer)range.Start).GetValue(FrameworkElement.LanguageProperty);
			Invariant.Assert(xmlLanguage != null);
			if (!string.Equals(inputCultureInfo.IetfLanguageTag, xmlLanguage.IetfLanguageTag, StringComparison.OrdinalIgnoreCase))
			{
				range.ApplyPropertyValue(FrameworkElement.LanguageProperty, XmlLanguage.GetLanguage(inputCultureInfo.IetfLanguageTag));
			}
			FlowDirection flowDirection;
			if (inputCultureInfo.TextInfo.IsRightToLeft)
			{
				flowDirection = FlowDirection.RightToLeft;
			}
			else
			{
				flowDirection = FlowDirection.LeftToRight;
			}
			if ((FlowDirection)((ITextPointer)range.Start).GetValue(FrameworkElement.FlowDirectionProperty) != flowDirection)
			{
				range.ApplyPropertyValue(FrameworkElement.FlowDirectionProperty, flowDirection);
			}
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x00260F2C File Offset: 0x0025FF2C
		internal void RequestExtendSelection(Point point)
		{
			if (this._mouseSelectionState == null)
			{
				this._mouseSelectionState = new TextEditor.MouseSelectionState();
				this._mouseSelectionState.Timer = new DispatcherTimer(DispatcherPriority.Normal);
				this._mouseSelectionState.Timer.Tick += this.HandleMouseSelectionTick;
				this._mouseSelectionState.Timer.Interval = TimeSpan.FromMilliseconds((double)Math.Max(SystemParameters.MenuShowDelay, 200));
				this._mouseSelectionState.Timer.Start();
				this._mouseSelectionState.Point = point;
				this.HandleMouseSelectionTick(this._mouseSelectionState.Timer, EventArgs.Empty);
				return;
			}
			this._mouseSelectionState.Point = point;
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x00260FE1 File Offset: 0x0025FFE1
		internal void CancelExtendSelection()
		{
			if (this._mouseSelectionState != null)
			{
				this._mouseSelectionState.Timer.Stop();
				this._mouseSelectionState.Timer.Tick -= this.HandleMouseSelectionTick;
				this._mouseSelectionState = null;
			}
		}

		// Token: 0x06005550 RID: 21840 RVA: 0x00261020 File Offset: 0x00260020
		internal void CloseToolTip()
		{
			PopupControlService popupControlService = PopupControlService.Current;
			if (popupControlService.CurrentToolTip != null && popupControlService.CurrentToolTip.IsOpen && popupControlService.CurrentToolTip.PlacementTarget == this._uiScope)
			{
				popupControlService.CurrentToolTip.IsOpen = false;
			}
		}

		// Token: 0x06005551 RID: 21841 RVA: 0x00261068 File Offset: 0x00260068
		internal void Undo()
		{
			TextEditorTyping._FlushPendingInputItems(this);
			this.CompleteComposition();
			this._undoState = UndoState.Undo;
			bool coversEntireContent = this.Selection.CoversEntireContent;
			try
			{
				this._selection.BeginChangeNoUndo();
				try
				{
					UndoManager undoManager = this._GetUndoManager();
					if (undoManager != null && undoManager.UndoCount > undoManager.MinUndoStackCount)
					{
						undoManager.Undo(1);
					}
					TextEditorSelection._ClearSuggestedX(this);
					TextEditorTyping._BreakTypingSequence(this);
					if (this._selection is TextSelection)
					{
						((TextSelection)this._selection).ClearSpringloadFormatting();
					}
				}
				finally
				{
					this._selection.EndChange();
				}
			}
			finally
			{
				this._undoState = UndoState.Normal;
			}
			if (coversEntireContent)
			{
				this.Selection.ValidateLayout();
			}
		}

		// Token: 0x06005552 RID: 21842 RVA: 0x00261128 File Offset: 0x00260128
		internal void Redo()
		{
			TextEditorTyping._FlushPendingInputItems(this);
			this._undoState = UndoState.Redo;
			bool coversEntireContent = this.Selection.CoversEntireContent;
			try
			{
				this._selection.BeginChangeNoUndo();
				try
				{
					UndoManager undoManager = this._GetUndoManager();
					if (undoManager != null && undoManager.RedoCount > 0)
					{
						undoManager.Redo(1);
					}
					TextEditorSelection._ClearSuggestedX(this);
					TextEditorTyping._BreakTypingSequence(this);
					if (this._selection is TextSelection)
					{
						((TextSelection)this._selection).ClearSpringloadFormatting();
					}
				}
				finally
				{
					this._selection.EndChange();
				}
			}
			finally
			{
				this._undoState = UndoState.Normal;
			}
			if (coversEntireContent)
			{
				this.Selection.ValidateLayout();
			}
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x002611DC File Offset: 0x002601DC
		internal void OnPreviewKeyDown(KeyEventArgs e)
		{
			TextEditorTyping.OnPreviewKeyDown(this._uiScope, e);
		}

		// Token: 0x06005554 RID: 21844 RVA: 0x002611EA File Offset: 0x002601EA
		internal void OnKeyDown(KeyEventArgs e)
		{
			TextEditorTyping.OnKeyDown(this._uiScope, e);
		}

		// Token: 0x06005555 RID: 21845 RVA: 0x002611F8 File Offset: 0x002601F8
		internal void OnKeyUp(KeyEventArgs e)
		{
			TextEditorTyping.OnKeyUp(this._uiScope, e);
		}

		// Token: 0x06005556 RID: 21846 RVA: 0x00261206 File Offset: 0x00260206
		internal void OnTextInput(TextCompositionEventArgs e)
		{
			TextEditorTyping.OnTextInput(this._uiScope, e);
		}

		// Token: 0x06005557 RID: 21847 RVA: 0x00261214 File Offset: 0x00260214
		internal void OnMouseDown(MouseButtonEventArgs e)
		{
			TextEditorMouse.OnMouseDown(this._uiScope, e);
		}

		// Token: 0x06005558 RID: 21848 RVA: 0x00261222 File Offset: 0x00260222
		internal void OnMouseMove(MouseEventArgs e)
		{
			TextEditorMouse.OnMouseMove(this._uiScope, e);
		}

		// Token: 0x06005559 RID: 21849 RVA: 0x00261230 File Offset: 0x00260230
		internal void OnMouseUp(MouseButtonEventArgs e)
		{
			TextEditorMouse.OnMouseUp(this._uiScope, e);
		}

		// Token: 0x0600555A RID: 21850 RVA: 0x0026123E File Offset: 0x0026023E
		internal void OnQueryCursor(QueryCursorEventArgs e)
		{
			TextEditorMouse.OnQueryCursor(this._uiScope, e);
		}

		// Token: 0x0600555B RID: 21851 RVA: 0x0026124C File Offset: 0x0026024C
		internal void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
			TextEditorDragDrop.OnQueryContinueDrag(this._uiScope, e);
		}

		// Token: 0x0600555C RID: 21852 RVA: 0x0026125A File Offset: 0x0026025A
		internal void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			TextEditorDragDrop.OnGiveFeedback(this._uiScope, e);
		}

		// Token: 0x0600555D RID: 21853 RVA: 0x00261268 File Offset: 0x00260268
		internal void OnDragEnter(DragEventArgs e)
		{
			TextEditorDragDrop.OnDragEnter(this._uiScope, e);
		}

		// Token: 0x0600555E RID: 21854 RVA: 0x00261276 File Offset: 0x00260276
		internal void OnDragOver(DragEventArgs e)
		{
			TextEditorDragDrop.OnDragOver(this._uiScope, e);
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x00261284 File Offset: 0x00260284
		internal void OnDragLeave(DragEventArgs e)
		{
			TextEditorDragDrop.OnDragLeave(this._uiScope, e);
		}

		// Token: 0x06005560 RID: 21856 RVA: 0x00261292 File Offset: 0x00260292
		internal void OnDrop(DragEventArgs e)
		{
			TextEditorDragDrop.OnDrop(this._uiScope, e);
		}

		// Token: 0x06005561 RID: 21857 RVA: 0x002612A0 File Offset: 0x002602A0
		internal void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			TextEditorContextMenu.OnContextMenuOpening(this._uiScope, e);
		}

		// Token: 0x06005562 RID: 21858 RVA: 0x002612AE File Offset: 0x002602AE
		internal void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			TextEditor.OnGotKeyboardFocus(this._uiScope, e);
		}

		// Token: 0x06005563 RID: 21859 RVA: 0x002612BC File Offset: 0x002602BC
		internal void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			TextEditor.OnLostKeyboardFocus(this._uiScope, e);
		}

		// Token: 0x06005564 RID: 21860 RVA: 0x002612CA File Offset: 0x002602CA
		internal void OnLostFocus(RoutedEventArgs e)
		{
			TextEditor.OnLostFocus(this._uiScope, e);
		}

		// Token: 0x17001423 RID: 5155
		// (get) Token: 0x06005565 RID: 21861 RVA: 0x002612D8 File Offset: 0x002602D8
		internal ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x06005566 RID: 21862 RVA: 0x002612E0 File Offset: 0x002602E0
		internal FrameworkElement UiScope
		{
			get
			{
				return this._uiScope;
			}
		}

		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x06005567 RID: 21863 RVA: 0x002612E8 File Offset: 0x002602E8
		// (set) Token: 0x06005568 RID: 21864 RVA: 0x002612F0 File Offset: 0x002602F0
		internal ITextView TextView
		{
			get
			{
				return this._textView;
			}
			set
			{
				if (value != this._textView)
				{
					if (this._textView != null)
					{
						this._textView.Updated -= this.OnTextViewUpdated;
						this._textView = null;
						this._selection.UpdateCaretAndHighlight();
					}
					if (value != null)
					{
						this._textView = value;
						this._textView.Updated += this.OnTextViewUpdated;
						this._selection.UpdateCaretAndHighlight();
					}
				}
			}
		}

		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x06005569 RID: 21865 RVA: 0x00261363 File Offset: 0x00260363
		internal ITextSelection Selection
		{
			get
			{
				return this._selection;
			}
		}

		// Token: 0x17001427 RID: 5159
		// (get) Token: 0x0600556A RID: 21866 RVA: 0x0026136B File Offset: 0x0026036B
		internal TextStore TextStore
		{
			get
			{
				return this._textstore;
			}
		}

		// Token: 0x17001428 RID: 5160
		// (get) Token: 0x0600556B RID: 21867 RVA: 0x00261373 File Offset: 0x00260373
		internal ImmComposition ImmComposition
		{
			get
			{
				if (!TextEditor._immEnabled)
				{
					return null;
				}
				return this._immComposition;
			}
		}

		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x0600556C RID: 21868 RVA: 0x00261384 File Offset: 0x00260384
		internal bool AcceptsReturn
		{
			get
			{
				return this._uiScope == null || (bool)this._uiScope.GetValue(KeyboardNavigation.AcceptsReturnProperty);
			}
		}

		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x0600556D RID: 21869 RVA: 0x002613A5 File Offset: 0x002603A5
		// (set) Token: 0x0600556E RID: 21870 RVA: 0x002613C6 File Offset: 0x002603C6
		internal bool AcceptsTab
		{
			get
			{
				return this._uiScope == null || (bool)this._uiScope.GetValue(TextBoxBase.AcceptsTabProperty);
			}
			set
			{
				Invariant.Assert(this._uiScope != null);
				if (this.AcceptsTab != value)
				{
					this._uiScope.SetValue(TextBoxBase.AcceptsTabProperty, value);
				}
			}
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x0600556F RID: 21871 RVA: 0x002613F0 File Offset: 0x002603F0
		// (set) Token: 0x06005570 RID: 21872 RVA: 0x0026141B File Offset: 0x0026041B
		internal bool IsReadOnly
		{
			get
			{
				return this._isReadOnly || (this._uiScope != null && (bool)this._uiScope.GetValue(TextEditor.IsReadOnlyProperty));
			}
			set
			{
				this._isReadOnly = value;
			}
		}

		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x06005571 RID: 21873 RVA: 0x00261424 File Offset: 0x00260424
		// (set) Token: 0x06005572 RID: 21874 RVA: 0x00261445 File Offset: 0x00260445
		internal bool IsSpellCheckEnabled
		{
			get
			{
				return this._uiScope != null && (bool)this._uiScope.GetValue(SpellCheck.IsEnabledProperty);
			}
			set
			{
				Invariant.Assert(this._uiScope != null);
				this._uiScope.SetValue(SpellCheck.IsEnabledProperty, value);
			}
		}

		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x06005573 RID: 21875 RVA: 0x00261466 File Offset: 0x00260466
		// (set) Token: 0x06005574 RID: 21876 RVA: 0x0026146E File Offset: 0x0026046E
		internal bool AcceptsRichContent
		{
			get
			{
				return this._acceptsRichContent;
			}
			set
			{
				this._acceptsRichContent = value;
			}
		}

		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x06005575 RID: 21877 RVA: 0x00261477 File Offset: 0x00260477
		internal bool AllowOvertype
		{
			get
			{
				return this._uiScope == null || (bool)this._uiScope.GetValue(TextEditor.AllowOvertypeProperty);
			}
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x06005576 RID: 21878 RVA: 0x00261498 File Offset: 0x00260498
		internal int MaxLength
		{
			get
			{
				if (this._uiScope != null)
				{
					return (int)this._uiScope.GetValue(TextBox.MaxLengthProperty);
				}
				return 0;
			}
		}

		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06005577 RID: 21879 RVA: 0x002614B9 File Offset: 0x002604B9
		internal CharacterCasing CharacterCasing
		{
			get
			{
				if (this._uiScope != null)
				{
					return (CharacterCasing)this._uiScope.GetValue(TextBox.CharacterCasingProperty);
				}
				return CharacterCasing.Normal;
			}
		}

		// Token: 0x17001431 RID: 5169
		// (get) Token: 0x06005578 RID: 21880 RVA: 0x002614DA File Offset: 0x002604DA
		internal bool AutoWordSelection
		{
			get
			{
				return this._uiScope != null && (bool)this._uiScope.GetValue(TextBoxBase.AutoWordSelectionProperty);
			}
		}

		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x06005579 RID: 21881 RVA: 0x002614FB File Offset: 0x002604FB
		internal bool IsReadOnlyCaretVisible
		{
			get
			{
				return this._uiScope != null && (bool)this._uiScope.GetValue(TextBoxBase.IsReadOnlyCaretVisibleProperty);
			}
		}

		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x0600557A RID: 21882 RVA: 0x0026151C File Offset: 0x0026051C
		internal UndoState UndoState
		{
			get
			{
				return this._undoState;
			}
		}

		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x0600557B RID: 21883 RVA: 0x00261524 File Offset: 0x00260524
		// (set) Token: 0x0600557C RID: 21884 RVA: 0x0026152C File Offset: 0x0026052C
		internal bool IsContextMenuOpen
		{
			get
			{
				return this._isContextMenuOpen;
			}
			set
			{
				this._isContextMenuOpen = value;
			}
		}

		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x0600557D RID: 21885 RVA: 0x00261535 File Offset: 0x00260535
		internal Speller Speller
		{
			get
			{
				return this._speller;
			}
		}

		// Token: 0x0600557E RID: 21886 RVA: 0x0026153D File Offset: 0x0026053D
		internal static TextEditor _GetTextEditor(object element)
		{
			if (!(element is DependencyObject))
			{
				return null;
			}
			return ((DependencyObject)element).ReadLocalValue(TextEditor.InstanceProperty) as TextEditor;
		}

		// Token: 0x0600557F RID: 21887 RVA: 0x00261560 File Offset: 0x00260560
		internal UndoManager _GetUndoManager()
		{
			UndoManager result = null;
			if (this.TextContainer is TextContainer)
			{
				result = ((TextContainer)this.TextContainer).UndoManager;
			}
			return result;
		}

		// Token: 0x06005580 RID: 21888 RVA: 0x0026158E File Offset: 0x0026058E
		internal string _FilterText(string textData, ITextRange range)
		{
			return this._FilterText(textData, range.Start.GetOffsetToPosition(range.End));
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x002615A8 File Offset: 0x002605A8
		internal string _FilterText(string textData, int charsToReplaceCount)
		{
			return this._FilterText(textData, charsToReplaceCount, true);
		}

		// Token: 0x06005582 RID: 21890 RVA: 0x002615B3 File Offset: 0x002605B3
		internal string _FilterText(string textData, ITextRange range, bool filterMaxLength)
		{
			return this._FilterText(textData, range.Start.GetOffsetToPosition(range.End), filterMaxLength);
		}

		// Token: 0x06005583 RID: 21891 RVA: 0x002615D0 File Offset: 0x002605D0
		internal string _FilterText(string textData, int charsToReplaceCount, bool filterMaxLength)
		{
			if (!this.AcceptsRichContent)
			{
				if (filterMaxLength && this.MaxLength > 0)
				{
					ITextContainer textContainer = this.TextContainer;
					int num = textContainer.SymbolCount - charsToReplaceCount;
					int num2 = Math.Max(0, this.MaxLength - num);
					if (num2 == 0)
					{
						return string.Empty;
					}
					if (textData.Length > num2)
					{
						int num3 = num2;
						if (this.IsBadSplitPosition(textData, num3))
						{
							num3--;
						}
						textData = textData.Substring(0, num3);
					}
					if (textData.Length == num2 && char.IsHighSurrogate(textData, num2 - 1))
					{
						textData = textData.Substring(0, num2 - 1);
					}
					if (!string.IsNullOrEmpty(textData) && char.IsLowSurrogate(textData, 0))
					{
						string textInRun = textContainer.TextSelection.AnchorPosition.GetTextInRun(LogicalDirection.Backward);
						if (string.IsNullOrEmpty(textInRun) || !char.IsHighSurrogate(textInRun, textInRun.Length - 1))
						{
							return string.Empty;
						}
					}
				}
				if (string.IsNullOrEmpty(textData))
				{
					return textData;
				}
				if (this.CharacterCasing == CharacterCasing.Upper)
				{
					textData = textData.ToUpper(InputLanguageManager.Current.CurrentInputLanguage);
				}
				else if (this.CharacterCasing == CharacterCasing.Lower)
				{
					textData = textData.ToLower(InputLanguageManager.Current.CurrentInputLanguage);
				}
				if (!this.AcceptsReturn)
				{
					int num4 = textData.IndexOf(Environment.NewLine, StringComparison.Ordinal);
					if (num4 >= 0)
					{
						textData = textData.Substring(0, num4);
					}
					num4 = textData.IndexOfAny(TextPointerBase.NextLineCharacters);
					if (num4 >= 0)
					{
						textData = textData.Substring(0, num4);
					}
				}
				if (!this.AcceptsTab)
				{
					textData = textData.Replace('\t', ' ');
				}
			}
			return textData;
		}

		// Token: 0x06005584 RID: 21892 RVA: 0x00261744 File Offset: 0x00260744
		internal bool _IsSourceInScope(object source)
		{
			return source == this.UiScope || (source is FrameworkElement && ((FrameworkElement)source).TemplatedParent == this.UiScope);
		}

		// Token: 0x06005585 RID: 21893 RVA: 0x0026176F File Offset: 0x0026076F
		internal void CompleteComposition()
		{
			if (this.TextStore != null)
			{
				this.TextStore.CompleteComposition();
			}
			if (this.ImmComposition != null)
			{
				this.ImmComposition.CompleteComposition();
			}
		}

		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x06005586 RID: 21894 RVA: 0x00261797 File Offset: 0x00260797
		internal bool _IsEnabled
		{
			get
			{
				return this._uiScope != null && this._uiScope.IsEnabled;
			}
		}

		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x06005587 RID: 21895 RVA: 0x002617AE File Offset: 0x002607AE
		// (set) Token: 0x06005588 RID: 21896 RVA: 0x002617B6 File Offset: 0x002607B6
		internal bool _OvertypeMode
		{
			get
			{
				return this._overtypeMode;
			}
			set
			{
				this._overtypeMode = value;
			}
		}

		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x06005589 RID: 21897 RVA: 0x002617C0 File Offset: 0x002607C0
		internal FrameworkElement _Scroller
		{
			get
			{
				FrameworkElement frameworkElement = (this.TextView == null) ? null : (this.TextView.RenderScope as FrameworkElement);
				while (frameworkElement != null && frameworkElement != this.UiScope)
				{
					frameworkElement = (FrameworkElement.GetFrameworkParent(frameworkElement) as FrameworkElement);
					if (frameworkElement is ScrollViewer || frameworkElement is ScrollContentPresenter)
					{
						return frameworkElement;
					}
				}
				return null;
			}
		}

		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x0600558A RID: 21898 RVA: 0x00261818 File Offset: 0x00260818
		internal static TextEditorThreadLocalStore _ThreadLocalStore
		{
			get
			{
				TextEditorThreadLocalStore textEditorThreadLocalStore = (TextEditorThreadLocalStore)Thread.GetData(TextEditor._threadLocalStoreSlot);
				if (textEditorThreadLocalStore == null)
				{
					textEditorThreadLocalStore = new TextEditorThreadLocalStore();
					Thread.SetData(TextEditor._threadLocalStoreSlot, textEditorThreadLocalStore);
				}
				return textEditorThreadLocalStore;
			}
		}

		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x0600558B RID: 21899 RVA: 0x0026184A File Offset: 0x0026084A
		internal long _ContentChangeCounter
		{
			get
			{
				return this._contentChangeCounter;
			}
		}

		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x0600558C RID: 21900 RVA: 0x00261852 File Offset: 0x00260852
		// (set) Token: 0x0600558D RID: 21901 RVA: 0x00261859 File Offset: 0x00260859
		internal static bool IsTableEditingEnabled
		{
			get
			{
				return TextEditor._isTableEditingEnabled;
			}
			set
			{
				TextEditor._isTableEditingEnabled = value;
			}
		}

		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x0600558E RID: 21902 RVA: 0x00261861 File Offset: 0x00260861
		// (set) Token: 0x0600558F RID: 21903 RVA: 0x00261869 File Offset: 0x00260869
		internal ITextPointer _NextLineAdvanceMovingPosition
		{
			get
			{
				return this._nextLineAdvanceMovingPosition;
			}
			set
			{
				this._nextLineAdvanceMovingPosition = value;
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x06005590 RID: 21904 RVA: 0x00261872 File Offset: 0x00260872
		// (set) Token: 0x06005591 RID: 21905 RVA: 0x0026187A File Offset: 0x0026087A
		internal bool _IsNextLineAdvanceMovingPositionAtDocumentHead
		{
			get
			{
				return this._isNextLineAdvanceMovingPositionAtDocumentHead;
			}
			set
			{
				this._isNextLineAdvanceMovingPositionAtDocumentHead = value;
			}
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x00261883 File Offset: 0x00260883
		private bool IsBadSplitPosition(string text, int position)
		{
			return (text[position - 1] == '\r' && text[position] == '\n') || (char.IsHighSurrogate(text, position - 1) && char.IsLowSurrogate(text, position));
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x002618B4 File Offset: 0x002608B4
		private void HandleMouseSelectionTick(object sender, EventArgs e)
		{
			if (this._mouseSelectionState != null && !this._mouseSelectionState.BringIntoViewInProgress && this.TextView != null && this.TextView.IsValid && TextEditorSelection.IsPaginated(this.TextView))
			{
				this._mouseSelectionState.BringIntoViewInProgress = true;
				this.TextView.BringPointIntoViewCompleted += this.HandleBringPointIntoViewCompleted;
				this.TextView.BringPointIntoViewAsync(this._mouseSelectionState.Point, this);
			}
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x00261934 File Offset: 0x00260934
		private void HandleBringPointIntoViewCompleted(object sender, BringPointIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringPointIntoViewCompleted -= this.HandleBringPointIntoViewCompleted;
			if (this._mouseSelectionState == null)
			{
				return;
			}
			this._mouseSelectionState.BringIntoViewInProgress = false;
			if (e == null || e.Cancelled || e.Error != null)
			{
				this.CancelExtendSelection();
				return;
			}
			Invariant.Assert(e.UserState == this && this.TextView == sender);
			ITextPointer textPointer = e.Position;
			if (textPointer != null)
			{
				if (textPointer.GetNextInsertionPosition(LogicalDirection.Forward) == null && textPointer.ParentType != null)
				{
					Rect characterRect = textPointer.GetCharacterRect(LogicalDirection.Backward);
					if (e.Point.X > characterRect.X + characterRect.Width)
					{
						textPointer = this.TextContainer.End;
					}
				}
				this.Selection.ExtendSelectionByMouse(textPointer, this._forceWordSelection, this._forceParagraphSelection);
				return;
			}
			this.CancelExtendSelection();
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x00261A2C File Offset: 0x00260A2C
		private object InitTextStore(object o)
		{
			if (!this._pendingTextStoreInit)
			{
				return null;
			}
			if (this._textContainer is TextContainer && TextServicesHost.Current != null)
			{
				UnsafeNativeMethods.ITfThreadMgr tfThreadMgr = TextServicesLoader.Load();
				if (tfThreadMgr != null)
				{
					if (this._textstore == null)
					{
						this._textstore = new TextStore(this);
						this._weakThis = new TextEditor.TextEditorShutDownListener(this);
					}
					this._textstore.OnAttach();
					Marshal.ReleaseComObject(tfThreadMgr);
				}
			}
			this._pendingTextStoreInit = false;
			return null;
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x00261A9A File Offset: 0x00260A9A
		private void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			this._contentChangeCounter += 1L;
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x00261AAC File Offset: 0x00260AAC
		private void OnTextViewUpdated(object sender, EventArgs e)
		{
			this._selection.OnTextViewUpdated();
			this.UiScope.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnTextViewUpdatedWorker), EventArgs.Empty);
			if (!this._textStoreInitStarted)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.InitTextStore), null);
				this._pendingTextStoreInit = true;
				this._textStoreInitStarted = true;
			}
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x00261B17 File Offset: 0x00260B17
		private object OnTextViewUpdatedWorker(object o)
		{
			if (this.TextView == null)
			{
				return null;
			}
			if (this._textstore != null)
			{
				this._textstore.OnLayoutUpdated();
			}
			if (TextEditor._immEnabled && this._immComposition != null)
			{
				this._immComposition.OnLayoutUpdated();
			}
			return null;
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x00261B54 File Offset: 0x00260B54
		private static void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			textEditor._selection.UpdateCaretAndHighlight();
			textEditor.SetSpellCheckEnabled(textEditor.IsSpellCheckEnabled);
			textEditor.SetCustomDictionaries(textEditor.IsSpellCheckEnabled);
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x00261B94 File Offset: 0x00260B94
		private static void OnIsReadOnlyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			TextEditor textEditor = TextEditor._GetTextEditor(frameworkElement);
			if (textEditor == null)
			{
				return;
			}
			textEditor.SetSpellCheckEnabled(textEditor.IsSpellCheckEnabled);
			if ((bool)e.NewValue && textEditor._textstore != null)
			{
				textEditor._textstore.CompleteCompositionAsync();
			}
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x00261BE4 File Offset: 0x00260BE4
		private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender != e.NewFocus)
			{
				return;
			}
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor._textstore != null)
			{
				textEditor._textstore.OnGotFocus();
			}
			if (TextEditor._immEnabled)
			{
				textEditor._immComposition = ImmComposition.GetImmComposition(textEditor._uiScope);
				if (textEditor._immComposition != null)
				{
					textEditor._immCompositionForDetach = new WeakReference<ImmComposition>(textEditor._immComposition);
					textEditor._immComposition.OnGotFocus(textEditor);
				}
				else
				{
					textEditor._immCompositionForDetach = null;
				}
			}
			textEditor._selection.RefreshCaret();
			textEditor._selection.UpdateCaretAndHighlight();
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x00261C84 File Offset: 0x00260C84
		private static void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender != e.OldFocus)
			{
				return;
			}
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			textEditor._selection.UpdateCaretAndHighlight();
			if (textEditor._textstore != null)
			{
				textEditor._textstore.OnLostFocus();
			}
			if (TextEditor._immEnabled && textEditor._immComposition != null)
			{
				textEditor._immComposition.OnLostFocus();
				textEditor._immComposition = null;
			}
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x00261CF4 File Offset: 0x00260CF4
		private static void OnLostFocus(object sender, RoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorTyping._ShowCursor();
			if (!textEditor._IsEnabled)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			TextEditorTyping._BreakTypingSequence(textEditor);
			if (textEditor._tableColResizeInfo != null)
			{
				textEditor._tableColResizeInfo.DisposeAdorner();
				textEditor._tableColResizeInfo = null;
			}
			textEditor._selection.UpdateCaretAndHighlight();
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x00261D50 File Offset: 0x00260D50
		private static void OnUndo(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)target);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.IsReadOnly)
			{
				return;
			}
			textEditor.Undo();
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x00261D88 File Offset: 0x00260D88
		private static void OnRedo(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)target);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.IsReadOnly)
			{
				return;
			}
			textEditor.Redo();
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x00261DC0 File Offset: 0x00260DC0
		private static void OnQueryStatusUndo(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			UndoManager undoManager = textEditor._GetUndoManager();
			if (undoManager != null && undoManager.UndoCount > undoManager.MinUndoStackCount)
			{
				args.CanExecute = true;
			}
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x00261DFC File Offset: 0x00260DFC
		private static void OnQueryStatusRedo(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			UndoManager undoManager = textEditor._GetUndoManager();
			if (undoManager != null && undoManager.RedoCount > 0)
			{
				args.CanExecute = true;
			}
		}

		// Token: 0x04002F1D RID: 12061
		internal static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(TextEditor), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(TextEditor.OnIsReadOnlyChanged)));

		// Token: 0x04002F1E RID: 12062
		internal static readonly DependencyProperty AllowOvertypeProperty = DependencyProperty.RegisterAttached("AllowOvertype", typeof(bool), typeof(TextEditor), new FrameworkPropertyMetadata(true));

		// Token: 0x04002F1F RID: 12063
		internal static readonly DependencyProperty PageHeightProperty = DependencyProperty.RegisterAttached("PageHeight", typeof(double), typeof(TextEditor), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04002F20 RID: 12064
		private static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached("Instance", typeof(TextEditor), typeof(TextEditor), new FrameworkPropertyMetadata(null));

		// Token: 0x04002F21 RID: 12065
		internal Dispatcher _dispatcher;

		// Token: 0x04002F22 RID: 12066
		private bool _isReadOnly;

		// Token: 0x04002F23 RID: 12067
		private static ArrayList _registeredEditingTypes = new ArrayList(4);

		// Token: 0x04002F24 RID: 12068
		private ITextContainer _textContainer;

		// Token: 0x04002F25 RID: 12069
		private long _contentChangeCounter;

		// Token: 0x04002F26 RID: 12070
		private FrameworkElement _uiScope;

		// Token: 0x04002F27 RID: 12071
		private ITextView _textView;

		// Token: 0x04002F28 RID: 12072
		private ITextSelection _selection;

		// Token: 0x04002F29 RID: 12073
		private bool _overtypeMode;

		// Token: 0x04002F2A RID: 12074
		internal double _suggestedX;

		// Token: 0x04002F2B RID: 12075
		private TextStore _textstore;

		// Token: 0x04002F2C RID: 12076
		private TextEditor.TextEditorShutDownListener _weakThis;

		// Token: 0x04002F2D RID: 12077
		private Speller _speller;

		// Token: 0x04002F2E RID: 12078
		private bool _textStoreInitStarted;

		// Token: 0x04002F2F RID: 12079
		private bool _pendingTextStoreInit;

		// Token: 0x04002F30 RID: 12080
		internal Cursor _cursor;

		// Token: 0x04002F31 RID: 12081
		internal IParentUndoUnit _typingUndoUnit;

		// Token: 0x04002F32 RID: 12082
		internal TextEditorDragDrop._DragDropProcess _dragDropProcess;

		// Token: 0x04002F33 RID: 12083
		internal bool _forceWordSelection;

		// Token: 0x04002F34 RID: 12084
		internal bool _forceParagraphSelection;

		// Token: 0x04002F35 RID: 12085
		internal TextRangeEditTables.TableColumnResizeInfo _tableColResizeInfo;

		// Token: 0x04002F36 RID: 12086
		private UndoState _undoState;

		// Token: 0x04002F37 RID: 12087
		private bool _acceptsRichContent;

		// Token: 0x04002F38 RID: 12088
		private static bool _immEnabled = SafeSystemMetrics.IsImmEnabled;

		// Token: 0x04002F39 RID: 12089
		private ImmComposition _immComposition;

		// Token: 0x04002F3A RID: 12090
		private WeakReference<ImmComposition> _immCompositionForDetach;

		// Token: 0x04002F3B RID: 12091
		private static LocalDataStoreSlot _threadLocalStoreSlot = Thread.AllocateDataSlot();

		// Token: 0x04002F3C RID: 12092
		internal bool _mouseCapturingInProgress;

		// Token: 0x04002F3D RID: 12093
		private TextEditor.MouseSelectionState _mouseSelectionState;

		// Token: 0x04002F3E RID: 12094
		private bool _isContextMenuOpen;

		// Token: 0x04002F3F RID: 12095
		private static bool _isTableEditingEnabled;

		// Token: 0x04002F40 RID: 12096
		private ITextPointer _nextLineAdvanceMovingPosition;

		// Token: 0x04002F41 RID: 12097
		internal bool _isNextLineAdvanceMovingPositionAtDocumentHead;

		// Token: 0x04002F42 RID: 12098
		private const string KeyAltUndo = "Alt+Backspace";

		// Token: 0x04002F43 RID: 12099
		private const string KeyRedo = "Ctrl+Y";

		// Token: 0x04002F44 RID: 12100
		private const string KeyUndo = "Ctrl+Z";

		// Token: 0x02000B60 RID: 2912
		private sealed class TextEditorShutDownListener : ShutDownListener
		{
			// Token: 0x06008DC3 RID: 36291 RVA: 0x0033EF75 File Offset: 0x0033DF75
			public TextEditorShutDownListener(TextEditor target) : base(target, ShutDownEvents.DomainUnload | ShutDownEvents.DispatcherShutdown)
			{
			}

			// Token: 0x06008DC4 RID: 36292 RVA: 0x0033EF7F File Offset: 0x0033DF7F
			internal override void OnShutDown(object target, object sender, EventArgs e)
			{
				((TextEditor)target).DetachTextStore(false);
			}
		}

		// Token: 0x02000B61 RID: 2913
		private class MouseSelectionState
		{
			// Token: 0x040048BC RID: 18620
			internal DispatcherTimer Timer;

			// Token: 0x040048BD RID: 18621
			internal Point Point;

			// Token: 0x040048BE RID: 18622
			internal bool BringIntoViewInProgress;
		}
	}
}
