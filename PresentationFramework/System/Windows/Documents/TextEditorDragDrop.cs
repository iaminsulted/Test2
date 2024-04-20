using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006A2 RID: 1698
	internal static class TextEditorDragDrop
	{
		// Token: 0x060055D7 RID: 21975 RVA: 0x00263D74 File Offset: 0x00262D74
		internal static void _RegisterClassHandlers(Type controlType, bool readOnly, bool registerEventListeners)
		{
			if (!readOnly)
			{
				EventManager.RegisterClassHandler(controlType, DragDrop.DropEvent, new DragEventHandler(TextEditorDragDrop.OnClearState), true);
				EventManager.RegisterClassHandler(controlType, DragDrop.DragLeaveEvent, new DragEventHandler(TextEditorDragDrop.OnClearState), true);
			}
			if (registerEventListeners)
			{
				EventManager.RegisterClassHandler(controlType, DragDrop.QueryContinueDragEvent, new QueryContinueDragEventHandler(TextEditorDragDrop.OnQueryContinueDrag));
				EventManager.RegisterClassHandler(controlType, DragDrop.GiveFeedbackEvent, new GiveFeedbackEventHandler(TextEditorDragDrop.OnGiveFeedback));
				EventManager.RegisterClassHandler(controlType, DragDrop.DragEnterEvent, new DragEventHandler(TextEditorDragDrop.OnDragEnter));
				EventManager.RegisterClassHandler(controlType, DragDrop.DragOverEvent, new DragEventHandler(TextEditorDragDrop.OnDragOver));
				EventManager.RegisterClassHandler(controlType, DragDrop.DragLeaveEvent, new DragEventHandler(TextEditorDragDrop.OnDragLeave));
				if (!readOnly)
				{
					EventManager.RegisterClassHandler(controlType, DragDrop.DropEvent, new DragEventHandler(TextEditorDragDrop.OnDrop));
				}
			}
		}

		// Token: 0x060055D8 RID: 21976 RVA: 0x00263E48 File Offset: 0x00262E48
		internal static void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			e.Handled = true;
			e.Action = DragAction.Continue;
			bool flag = (e.KeyStates & DragDropKeyStates.LeftMouseButton) == DragDropKeyStates.None;
			if (e.EscapePressed)
			{
				e.Action = DragAction.Cancel;
				return;
			}
			if (flag)
			{
				e.Action = DragAction.Drop;
			}
		}

		// Token: 0x060055D9 RID: 21977 RVA: 0x00263EA0 File Offset: 0x00262EA0
		internal static void OnGiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			e.UseDefaultCursors = true;
			e.Handled = true;
		}

		// Token: 0x060055DA RID: 21978 RVA: 0x00263ED0 File Offset: 0x00262ED0
		internal static void OnDragEnter(object sender, DragEventArgs e)
		{
			e.Handled = true;
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (!textEditor._IsEnabled || textEditor.TextView == null || textEditor.TextView.RenderScope == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (e.Data == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (TextEditorCopyPaste.GetPasteApplyFormat(textEditor, e.Data) == string.Empty)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.TextView.Validate(e.GetPosition(textEditor.TextView.RenderScope)))
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			textEditor._dragDropProcess.TargetOnDragEnter(e);
		}

		// Token: 0x060055DB RID: 21979 RVA: 0x00263F84 File Offset: 0x00262F84
		internal static void OnDragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (!textEditor._IsEnabled || textEditor.TextView == null || textEditor.TextView.RenderScope == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (e.Data == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (TextEditorCopyPaste.GetPasteApplyFormat(textEditor, e.Data) == string.Empty)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.TextView.Validate(e.GetPosition(textEditor.TextView.RenderScope)))
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			textEditor._dragDropProcess.TargetOnDragOver(e);
		}

		// Token: 0x060055DC RID: 21980 RVA: 0x00264038 File Offset: 0x00263038
		internal static void OnDragLeave(object sender, DragEventArgs e)
		{
			e.Handled = true;
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			textEditor.TextView.Validate(e.GetPosition(textEditor.TextView.RenderScope));
		}

		// Token: 0x060055DD RID: 21981 RVA: 0x0026408C File Offset: 0x0026308C
		internal static void OnDrop(object sender, DragEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.TextView.Validate(e.GetPosition(textEditor.TextView.RenderScope)))
			{
				return;
			}
			textEditor._dragDropProcess.TargetOnDrop(e);
		}

		// Token: 0x060055DE RID: 21982 RVA: 0x002640E0 File Offset: 0x002630E0
		internal static void OnClearState(object sender, DragEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			textEditor._dragDropProcess.DeleteCaret();
		}

		// Token: 0x02000B65 RID: 2917
		internal class _DragDropProcess
		{
			// Token: 0x06008DD6 RID: 36310 RVA: 0x0033F3D8 File Offset: 0x0033E3D8
			internal _DragDropProcess(TextEditor textEditor)
			{
				Invariant.Assert(textEditor != null);
				this._textEditor = textEditor;
			}

			// Token: 0x06008DD7 RID: 36311 RVA: 0x0033F3F0 File Offset: 0x0033E3F0
			internal bool SourceOnMouseLeftButtonDown(Point mouseDownPoint)
			{
				ITextSelection selection = this._textEditor.Selection;
				if (this._textEditor.UiScope is PasswordBox)
				{
					this._dragStarted = false;
				}
				else
				{
					int num = (int)SystemParameters.MinimumHorizontalDragDistance;
					int num2 = (int)SystemParameters.MinimumVerticalDragDistance;
					this._dragRect = new Rect(mouseDownPoint.X - (double)num, mouseDownPoint.Y - (double)num2, (double)(num * 2), (double)(num2 * 2));
					this._dragStarted = selection.Contains(mouseDownPoint);
				}
				return this._dragStarted;
			}

			// Token: 0x06008DD8 RID: 36312 RVA: 0x0033F46C File Offset: 0x0033E46C
			internal void DoMouseLeftButtonUp(MouseButtonEventArgs e)
			{
				if (this._dragStarted)
				{
					if (this.TextView.IsValid)
					{
						Point position = e.GetPosition(this._textEditor.TextView.RenderScope);
						ITextPointer textPositionFromPoint = this.TextView.GetTextPositionFromPoint(position, true);
						if (textPositionFromPoint != null)
						{
							this._textEditor.Selection.SetSelectionByMouse(textPositionFromPoint, position);
						}
					}
					this._dragStarted = false;
				}
			}

			// Token: 0x06008DD9 RID: 36313 RVA: 0x0033F4D0 File Offset: 0x0033E4D0
			internal bool SourceOnMouseMove(Point mouseMovePoint)
			{
				if (!this._dragStarted)
				{
					return false;
				}
				if (!this.InitialThresholdCrossed(mouseMovePoint))
				{
					return true;
				}
				ITextSelection selection = this._textEditor.Selection;
				this._dragStarted = false;
				this._dragSourceTextRange = new TextRange(selection.Start, selection.End);
				IDataObject dataObject = TextEditorCopyPaste._CreateDataObject(this._textEditor, true);
				if (dataObject != null)
				{
					this.SourceDoDragDrop(selection, dataObject);
					this._textEditor.UiScope.ReleaseMouseCapture();
					return true;
				}
				return false;
			}

			// Token: 0x06008DDA RID: 36314 RVA: 0x0033F547 File Offset: 0x0033E547
			private bool InitialThresholdCrossed(Point dragPoint)
			{
				return !this._dragRect.Contains(dragPoint.X, dragPoint.Y);
			}

			// Token: 0x06008DDB RID: 36315 RVA: 0x0033F568 File Offset: 0x0033E568
			private void SourceDoDragDrop(ITextSelection selection, IDataObject dataObject)
			{
				DragDropEffects dragDropEffects = DragDropEffects.Copy;
				if (!this._textEditor.IsReadOnly)
				{
					dragDropEffects |= DragDropEffects.Move;
				}
				DragDropEffects dragDropEffects2 = DragDropEffects.None;
				try
				{
					dragDropEffects2 = DragDrop.DoDragDrop(this._textEditor.UiScope, dataObject, dragDropEffects);
				}
				catch (COMException ex) when (ex.HResult == -2147418113)
				{
				}
				if (!this._textEditor.IsReadOnly && dragDropEffects2 == DragDropEffects.Move && this._dragSourceTextRange != null && !this._dragSourceTextRange.IsEmpty)
				{
					using (selection.DeclareChangeBlock())
					{
						this._dragSourceTextRange.Text = string.Empty;
					}
				}
				this._dragSourceTextRange = null;
				if (!this._textEditor.IsReadOnly)
				{
					BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(this._textEditor.UiScope, TextBox.TextProperty);
					if (bindingExpressionBase != null)
					{
						bindingExpressionBase.UpdateSource();
						bindingExpressionBase.UpdateTarget();
					}
				}
			}

			// Token: 0x06008DDC RID: 36316 RVA: 0x0033F660 File Offset: 0x0033E660
			internal void TargetEnsureDropCaret()
			{
				if (this._caretDragDrop == null)
				{
					this._caretDragDrop = new CaretElement(this._textEditor, false);
					this._caretDragDrop.Hide();
				}
			}

			// Token: 0x06008DDD RID: 36317 RVA: 0x0033F687 File Offset: 0x0033E687
			internal void TargetOnDragEnter(DragEventArgs e)
			{
				if (!this.AllowDragDrop(e))
				{
					return;
				}
				if ((e.AllowedEffects & DragDropEffects.Move) != DragDropEffects.None)
				{
					e.Effects = DragDropEffects.Move;
				}
				if ((e.KeyStates & DragDropKeyStates.ControlKey) > DragDropKeyStates.None)
				{
					e.Effects |= DragDropEffects.Copy;
				}
				this.TargetEnsureDropCaret();
			}

			// Token: 0x06008DDE RID: 36318 RVA: 0x0033F6C8 File Offset: 0x0033E6C8
			internal void TargetOnDragOver(DragEventArgs e)
			{
				if (!this.AllowDragDrop(e))
				{
					return;
				}
				if ((e.AllowedEffects & DragDropEffects.Move) != DragDropEffects.None)
				{
					e.Effects = DragDropEffects.Move;
				}
				if ((e.KeyStates & DragDropKeyStates.ControlKey) > DragDropKeyStates.None)
				{
					e.Effects |= DragDropEffects.Copy;
				}
				if (this._caretDragDrop != null)
				{
					if (!this._textEditor.TextView.Validate(e.GetPosition(this._textEditor.TextView.RenderScope)))
					{
						return;
					}
					FrameworkElement scroller = this._textEditor._Scroller;
					if (scroller != null)
					{
						IScrollInfo scrollInfo = scroller as IScrollInfo;
						if (scrollInfo == null && scroller is ScrollViewer)
						{
							scrollInfo = ((ScrollViewer)scroller).ScrollInfo;
						}
						Invariant.Assert(scrollInfo != null);
						Point position = e.GetPosition(scroller);
						double num = (double)this._textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
						double num2 = 16.0;
						if (position.Y < num2)
						{
							if (position.Y > num2 / 2.0)
							{
								scrollInfo.LineUp();
							}
							else
							{
								scrollInfo.PageUp();
							}
						}
						else if (position.Y > num - num2)
						{
							if (position.Y < num - num2 / 2.0)
							{
								scrollInfo.LineDown();
							}
							else
							{
								scrollInfo.PageDown();
							}
						}
					}
					this._textEditor.TextView.RenderScope.UpdateLayout();
					if (this._textEditor.TextView.IsValid)
					{
						ITextPointer dropPosition = this.GetDropPosition(this._textEditor.TextView.RenderScope, e.GetPosition(this._textEditor.TextView.RenderScope));
						if (dropPosition != null)
						{
							Rect rectangleFromTextPosition = this.TextView.GetRectangleFromTextPosition(dropPosition);
							object value = dropPosition.GetValue(TextElement.FontStyleProperty);
							bool italic = this._textEditor.AcceptsRichContent && value != DependencyProperty.UnsetValue && (FontStyle)value == FontStyles.Italic;
							Brush caretBrush = TextSelection.GetCaretBrush(this._textEditor);
							this._caretDragDrop.Update(true, rectangleFromTextPosition, caretBrush, 0.5, italic, CaretScrollMethod.None, double.NaN);
						}
					}
				}
			}

			// Token: 0x06008DDF RID: 36319 RVA: 0x0033F8E0 File Offset: 0x0033E8E0
			private ITextPointer GetDropPosition(Visual target, Point point)
			{
				Invariant.Assert(target != null);
				Invariant.Assert(this._textEditor.TextView.IsValid);
				if (target != this._textEditor.TextView.RenderScope && target != null && this._textEditor.TextView.RenderScope.IsAncestorOf(target))
				{
					target.TransformToAncestor(this._textEditor.TextView.RenderScope).TryTransform(point, out point);
				}
				ITextPointer textPointer = this.TextView.GetTextPositionFromPoint(point, true);
				if (textPointer != null)
				{
					textPointer = textPointer.GetInsertionPosition(textPointer.LogicalDirection);
					if (this._textEditor.AcceptsRichContent)
					{
						TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(this.TextView, textPointer);
						if (!normalizedLineRange.IsNull && textPointer.CompareTo(normalizedLineRange.End) < 0 && !TextPointerBase.IsAtWordBoundary(textPointer, LogicalDirection.Forward) && this._dragSourceTextRange != null && TextPointerBase.IsAtWordBoundary(this._dragSourceTextRange.Start, LogicalDirection.Forward) && TextPointerBase.IsAtWordBoundary(this._dragSourceTextRange.End, LogicalDirection.Forward))
						{
							TextSegment wordRange = TextPointerBase.GetWordRange(textPointer);
							string textInternal = TextRangeBase.GetTextInternal(wordRange.Start, wordRange.End);
							textPointer = ((wordRange.Start.GetOffsetToPosition(textPointer) < textInternal.Length / 2) ? wordRange.Start : wordRange.End);
						}
					}
				}
				return textPointer;
			}

			// Token: 0x06008DE0 RID: 36320 RVA: 0x0033FA2D File Offset: 0x0033EA2D
			internal void DeleteCaret()
			{
				if (this._caretDragDrop != null)
				{
					AdornerLayer.GetAdornerLayer(this.TextView.RenderScope).Remove(this._caretDragDrop);
					this._caretDragDrop = null;
				}
			}

			// Token: 0x06008DE1 RID: 36321 RVA: 0x0033FA5C File Offset: 0x0033EA5C
			internal void TargetOnDrop(DragEventArgs e)
			{
				if (!this.AllowDragDrop(e))
				{
					return;
				}
				ITextSelection selection = this._textEditor.Selection;
				Invariant.Assert(selection != null);
				if (e.Data == null || e.AllowedEffects == DragDropEffects.None)
				{
					e.Effects = DragDropEffects.None;
					return;
				}
				if ((e.KeyStates & DragDropKeyStates.ControlKey) != DragDropKeyStates.None)
				{
					e.Effects = DragDropEffects.Copy;
				}
				else if (e.Effects != DragDropEffects.Copy)
				{
					e.Effects = DragDropEffects.Move;
				}
				if (!this._textEditor.TextView.Validate(e.GetPosition(this._textEditor.TextView.RenderScope)))
				{
					e.Effects = DragDropEffects.None;
					return;
				}
				ITextPointer dropPosition = this.GetDropPosition(this._textEditor.TextView.RenderScope, e.GetPosition(this._textEditor.TextView.RenderScope));
				if (dropPosition != null)
				{
					if (this._dragSourceTextRange != null && this._dragSourceTextRange.Start.TextContainer == selection.Start.TextContainer && !selection.IsEmpty && this.IsSelectionContainsDropPosition(selection, dropPosition))
					{
						selection.SetCaretToPosition(dropPosition, LogicalDirection.Backward, false, true);
						e.Effects = DragDropEffects.None;
						e.Handled = true;
					}
					else
					{
						using (selection.DeclareChangeBlock())
						{
							if ((e.Effects & DragDropEffects.Move) != DragDropEffects.None && this._dragSourceTextRange != null && this._dragSourceTextRange.Start.TextContainer == selection.Start.TextContainer)
							{
								this._dragSourceTextRange.Text = string.Empty;
							}
							selection.SetCaretToPosition(dropPosition, LogicalDirection.Backward, false, true);
							e.Handled = TextEditorCopyPaste._DoPaste(this._textEditor, e.Data, true);
						}
					}
					if (e.Handled)
					{
						this.Win32SetForegroundWindow();
						this._textEditor.UiScope.Focus();
						return;
					}
					e.Effects = DragDropEffects.None;
				}
			}

			// Token: 0x06008DE2 RID: 36322 RVA: 0x0033FC24 File Offset: 0x0033EC24
			private bool IsSelectionContainsDropPosition(ITextSelection selection, ITextPointer dropPosition)
			{
				bool flag = selection.Contains(dropPosition);
				if (flag && selection.IsTableCellRange)
				{
					for (int i = 0; i < selection.TextSegments.Count; i++)
					{
						if (dropPosition.CompareTo(selection._TextSegments[i].End) == 0)
						{
							flag = false;
							break;
						}
					}
				}
				return flag;
			}

			// Token: 0x06008DE3 RID: 36323 RVA: 0x0033FC7C File Offset: 0x0033EC7C
			private bool AllowDragDrop(DragEventArgs e)
			{
				if (!this._textEditor.IsReadOnly && this._textEditor.TextView != null && this._textEditor.TextView.RenderScope != null)
				{
					Window window = Window.GetWindow(this._textEditor.TextView.RenderScope);
					if (window == null)
					{
						return true;
					}
					WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
					if (SafeNativeMethods.IsWindowEnabled(new HandleRef(null, windowInteropHelper.Handle)))
					{
						return true;
					}
				}
				e.Effects = DragDropEffects.None;
				return false;
			}

			// Token: 0x06008DE4 RID: 36324 RVA: 0x0033FCF4 File Offset: 0x0033ECF4
			private void Win32SetForegroundWindow()
			{
				IntPtr intPtr = IntPtr.Zero;
				PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this._textEditor.UiScope);
				if (presentationSource != null)
				{
					intPtr = (presentationSource as IWin32Window).Handle;
				}
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.SetForegroundWindow(new HandleRef(null, intPtr));
				}
			}

			// Token: 0x17001EFD RID: 7933
			// (get) Token: 0x06008DE5 RID: 36325 RVA: 0x0033FD43 File Offset: 0x0033ED43
			private ITextView TextView
			{
				get
				{
					return this._textEditor.TextView;
				}
			}

			// Token: 0x040048C2 RID: 18626
			private TextEditor _textEditor;

			// Token: 0x040048C3 RID: 18627
			private ITextRange _dragSourceTextRange;

			// Token: 0x040048C4 RID: 18628
			private bool _dragStarted;

			// Token: 0x040048C5 RID: 18629
			private CaretElement _caretDragDrop;

			// Token: 0x040048C6 RID: 18630
			private Rect _dragRect;
		}
	}
}
