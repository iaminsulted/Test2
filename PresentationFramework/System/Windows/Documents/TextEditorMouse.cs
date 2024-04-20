using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006A4 RID: 1700
	internal static class TextEditorMouse
	{
		// Token: 0x060055EC RID: 21996 RVA: 0x00264730 File Offset: 0x00263730
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			if (registerEventListeners)
			{
				EventManager.RegisterClassHandler(controlType, Mouse.QueryCursorEvent, new QueryCursorEventHandler(TextEditorMouse.OnQueryCursor));
				EventManager.RegisterClassHandler(controlType, Mouse.MouseDownEvent, new MouseButtonEventHandler(TextEditorMouse.OnMouseDown));
				EventManager.RegisterClassHandler(controlType, Mouse.MouseMoveEvent, new MouseEventHandler(TextEditorMouse.OnMouseMove));
				EventManager.RegisterClassHandler(controlType, Mouse.MouseUpEvent, new MouseButtonEventHandler(TextEditorMouse.OnMouseUp));
			}
		}

		// Token: 0x060055ED RID: 21997 RVA: 0x0026479C File Offset: 0x0026379C
		internal static void SetCaretPositionOnMouseEvent(TextEditor This, Point mouseDownPoint, MouseButton changedButton, int clickCount)
		{
			ITextPointer textPositionFromPoint = This.TextView.GetTextPositionFromPoint(mouseDownPoint, true);
			if (textPositionFromPoint == null)
			{
				TextEditorMouse.MoveFocusToUiScope(This);
				return;
			}
			TextEditorSelection._ClearSuggestedX(This);
			TextEditorTyping._BreakTypingSequence(This);
			if (This.Selection is TextSelection)
			{
				((TextSelection)This.Selection).ClearSpringloadFormatting();
			}
			This._forceWordSelection = false;
			This._forceParagraphSelection = false;
			if (changedButton == MouseButton.Right || clickCount == 1)
			{
				if (changedButton != MouseButton.Left || !This._dragDropProcess.SourceOnMouseLeftButtonDown(mouseDownPoint))
				{
					This.Selection.SetSelectionByMouse(textPositionFromPoint, mouseDownPoint);
					return;
				}
			}
			else
			{
				if (clickCount == 2 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None && This.Selection.IsEmpty)
				{
					This._forceWordSelection = true;
					This._forceParagraphSelection = false;
					This.Selection.SelectWord(textPositionFromPoint);
					return;
				}
				if (clickCount == 3 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None && This.AcceptsRichContent)
				{
					This._forceParagraphSelection = true;
					This._forceWordSelection = false;
					This.Selection.SelectParagraph(textPositionFromPoint);
				}
			}
		}

		// Token: 0x060055EE RID: 21998 RVA: 0x00264884 File Offset: 0x00263884
		internal static bool IsPointWithinInteractiveArea(TextEditor textEditor, Point point)
		{
			bool flag = TextEditorMouse.IsPointWithinRenderScope(textEditor, point);
			if (flag)
			{
				flag = textEditor.TextView.IsValid;
				if (flag)
				{
					GeneralTransform generalTransform = textEditor.UiScope.TransformToDescendant(textEditor.TextView.RenderScope);
					if (generalTransform != null)
					{
						generalTransform.TryTransform(point, out point);
					}
					flag = (textEditor.TextView.GetTextPositionFromPoint(point, true) != null);
				}
			}
			return flag;
		}

		// Token: 0x060055EF RID: 21999 RVA: 0x002648E4 File Offset: 0x002638E4
		internal static void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			textEditor.CloseToolTip();
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (!textEditor.UiScope.Focusable)
			{
				return;
			}
			if (e.ButtonState == MouseButtonState.Released)
			{
				return;
			}
			e.Handled = true;
			TextEditorMouse.MoveFocusToUiScope(textEditor);
			if (textEditor.UiScope != Keyboard.FocusedElement)
			{
				return;
			}
			if (e.ChangedButton != MouseButton.Left)
			{
				return;
			}
			if (textEditor.TextView == null)
			{
				return;
			}
			textEditor.CompleteComposition();
			if (!textEditor.TextView.IsValid)
			{
				textEditor.TextView.RenderScope.UpdateLayout();
				if (textEditor.TextView == null || !textEditor.TextView.IsValid)
				{
					return;
				}
			}
			if (!TextEditorMouse.IsPointWithinInteractiveArea(textEditor, e.GetPosition(textEditor.UiScope)))
			{
				return;
			}
			textEditor.TextView.ThrottleBackgroundTasksForUserInput();
			Point position = e.GetPosition(textEditor.TextView.RenderScope);
			if (TextEditor.IsTableEditingEnabled && TextRangeEditTables.TableBorderHitTest(textEditor.TextView, position))
			{
				textEditor._tableColResizeInfo = TextRangeEditTables.StartColumnResize(textEditor.TextView, position);
				Invariant.Assert(textEditor._tableColResizeInfo != null);
				textEditor._mouseCapturingInProgress = true;
				try
				{
					textEditor.UiScope.CaptureMouse();
					return;
				}
				finally
				{
					textEditor._mouseCapturingInProgress = false;
				}
			}
			textEditor.Selection.BeginChange();
			try
			{
				TextEditorMouse.SetCaretPositionOnMouseEvent(textEditor, position, e.ChangedButton, e.ClickCount);
				textEditor._mouseCapturingInProgress = true;
				textEditor.UiScope.CaptureMouse();
			}
			finally
			{
				textEditor._mouseCapturingInProgress = false;
				textEditor.Selection.EndChange();
			}
		}

		// Token: 0x060055F0 RID: 22000 RVA: 0x00264A70 File Offset: 0x00263A70
		internal static void OnMouseMove(object sender, MouseEventArgs e)
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
			if (textEditor.TextView == null || !textEditor.TextView.IsValid)
			{
				return;
			}
			if (textEditor.UiScope.IsKeyboardFocused)
			{
				TextEditorMouse.OnMouseMoveWithFocus(textEditor, e);
				return;
			}
			TextEditorMouse.OnMouseMoveWithoutFocus(textEditor, e);
		}

		// Token: 0x060055F1 RID: 22001 RVA: 0x00264AC4 File Offset: 0x00263AC4
		internal static void OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (e.ChangedButton != MouseButton.Left)
			{
				return;
			}
			if (e.RightButton != MouseButtonState.Released)
			{
				return;
			}
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.TextView == null || !textEditor.TextView.IsValid)
			{
				return;
			}
			if (!textEditor.UiScope.IsMouseCaptured)
			{
				return;
			}
			e.Handled = true;
			textEditor.CancelExtendSelection();
			Point position = e.GetPosition(textEditor.TextView.RenderScope);
			TextEditorMouse.UpdateCursor(textEditor, position);
			if (textEditor._tableColResizeInfo != null)
			{
				using (textEditor.Selection.DeclareChangeBlock())
				{
					textEditor._tableColResizeInfo.ResizeColumn(position);
					textEditor._tableColResizeInfo = null;
					goto IL_D5;
				}
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				textEditor._dragDropProcess.DoMouseLeftButtonUp(e);
				textEditor._forceWordSelection = false;
				textEditor._forceParagraphSelection = false;
			}
			IL_D5:
			textEditor._mouseCapturingInProgress = true;
			try
			{
				textEditor.UiScope.ReleaseMouseCapture();
			}
			finally
			{
				textEditor._mouseCapturingInProgress = false;
			}
		}

		// Token: 0x060055F2 RID: 22002 RVA: 0x00264BEC File Offset: 0x00263BEC
		internal static void OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			if (textEditor.TextView == null)
			{
				return;
			}
			if (TextEditorMouse.IsPointWithinInteractiveArea(textEditor, Mouse.GetPosition(textEditor.UiScope)))
			{
				e.Cursor = textEditor._cursor;
				e.Handled = true;
			}
		}

		// Token: 0x060055F3 RID: 22003 RVA: 0x00264C33 File Offset: 0x00263C33
		private static void OnMouseMoveWithoutFocus(TextEditor This, MouseEventArgs e)
		{
			TextEditorMouse.UpdateCursor(This, e.GetPosition(This.TextView.RenderScope));
		}

		// Token: 0x060055F4 RID: 22004 RVA: 0x00264C4C File Offset: 0x00263C4C
		private static void OnMouseMoveWithFocus(TextEditor This, MouseEventArgs e)
		{
			if (This._mouseCapturingInProgress)
			{
				return;
			}
			TextEditor._ThreadLocalStore.PureControlShift = false;
			Point position = e.GetPosition(This.TextView.RenderScope);
			TextEditorMouse.UpdateCursor(This, position);
			Invariant.Assert(This.Selection != null);
			if (e.LeftButton != MouseButtonState.Pressed)
			{
				return;
			}
			if (!This.UiScope.IsMouseCaptured)
			{
				return;
			}
			This.TextView.ThrottleBackgroundTasksForUserInput();
			if (This._tableColResizeInfo != null)
			{
				This._tableColResizeInfo.UpdateAdorner(position);
				return;
			}
			e.Handled = true;
			Invariant.Assert(This.Selection != null);
			ITextPointer textPointer = This.TextView.GetTextPositionFromPoint(position, true);
			Invariant.Assert(This.Selection != null);
			if (textPointer == null)
			{
				This.RequestExtendSelection(position);
				return;
			}
			This.CancelExtendSelection();
			Invariant.Assert(This.Selection != null);
			if (!This._dragDropProcess.SourceOnMouseMove(position))
			{
				FrameworkElement scroller = This._Scroller;
				if (scroller != null && This.UiScope is TextBoxBase)
				{
					ITextPointer textPointer2 = null;
					Point point = new Point(position.X, position.Y);
					Point position2 = e.GetPosition(scroller);
					double num = ((TextBoxBase)This.UiScope).ViewportHeight;
					double num2 = 16.0;
					if (position2.Y < 0.0 - num2)
					{
						Rect rectangleFromTextPosition = This.TextView.GetRectangleFromTextPosition(textPointer);
						point = new Point(point.X, rectangleFromTextPosition.Bottom - num);
						textPointer2 = This.TextView.GetTextPositionFromPoint(point, true);
					}
					else if (position2.Y > num + num2)
					{
						Rect rectangleFromTextPosition2 = This.TextView.GetRectangleFromTextPosition(textPointer);
						point = new Point(point.X, rectangleFromTextPosition2.Top + num);
						textPointer2 = This.TextView.GetTextPositionFromPoint(point, true);
					}
					double num3 = ((TextBoxBase)This.UiScope).ViewportWidth;
					if (position2.X < 0.0)
					{
						point = new Point(point.X - num2, point.Y);
						textPointer2 = This.TextView.GetTextPositionFromPoint(point, true);
					}
					else if (position2.X > num3)
					{
						point = new Point(point.X + num2, point.Y);
						textPointer2 = This.TextView.GetTextPositionFromPoint(point, true);
					}
					if (textPointer2 != null)
					{
						textPointer = textPointer2;
					}
				}
				using (This.Selection.DeclareChangeBlock())
				{
					if (textPointer.GetNextInsertionPosition(LogicalDirection.Forward) == null && textPointer.ParentType != null)
					{
						Rect characterRect = textPointer.GetCharacterRect(LogicalDirection.Backward);
						if (position.X > characterRect.X + characterRect.Width)
						{
							textPointer = This.TextContainer.End;
						}
					}
					This.Selection.ExtendSelectionByMouse(textPointer, This._forceWordSelection, This._forceParagraphSelection);
				}
			}
		}

		// Token: 0x060055F5 RID: 22005 RVA: 0x00264F24 File Offset: 0x00263F24
		private static bool MoveFocusToUiScope(TextEditor This)
		{
			long contentChangeCounter = This._ContentChangeCounter;
			Visual visual = VisualTreeHelper.GetParent(This.UiScope) as Visual;
			while (visual != null && !(visual is ScrollViewer))
			{
				visual = (VisualTreeHelper.GetParent(visual) as Visual);
			}
			if (visual != null)
			{
				((ScrollViewer)visual).AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(TextEditorMouse.OnScrollChangedDuringGotFocus));
			}
			ITextSelection selection = This.Selection;
			try
			{
				selection.Changed += TextEditorMouse.OnSelectionChangedDuringGotFocus;
				TextEditorMouse._selectionChanged = false;
				This.UiScope.Focus();
			}
			finally
			{
				selection.Changed -= TextEditorMouse.OnSelectionChangedDuringGotFocus;
				if (visual != null)
				{
					((ScrollViewer)visual).RemoveHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(TextEditorMouse.OnScrollChangedDuringGotFocus));
				}
			}
			return This.UiScope == Keyboard.FocusedElement && contentChangeCounter == This._ContentChangeCounter && !TextEditorMouse._selectionChanged;
		}

		// Token: 0x060055F6 RID: 22006 RVA: 0x00265010 File Offset: 0x00264010
		private static void OnSelectionChangedDuringGotFocus(object sender, EventArgs e)
		{
			TextEditorMouse._selectionChanged = true;
		}

		// Token: 0x060055F7 RID: 22007 RVA: 0x00265018 File Offset: 0x00264018
		private static void OnScrollChangedDuringGotFocus(object sender, ScrollChangedEventArgs e)
		{
			ScrollViewer scrollViewer = e.OriginalSource as ScrollViewer;
			if (scrollViewer != null)
			{
				scrollViewer.RemoveHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(TextEditorMouse.OnScrollChangedDuringGotFocus));
				scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.HorizontalChange);
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.VerticalChange);
			}
		}

		// Token: 0x060055F8 RID: 22008 RVA: 0x00265074 File Offset: 0x00264074
		private static void UpdateCursor(TextEditor This, Point mouseMovePoint)
		{
			Invariant.Assert(This.TextView != null && This.TextView.IsValid);
			Cursor cursor = Cursors.IBeam;
			if (TextEditor.IsTableEditingEnabled && TextRangeEditTables.TableBorderHitTest(This.TextView, mouseMovePoint))
			{
				cursor = Cursors.SizeWE;
			}
			else if (This.Selection != null && !This.UiScope.IsMouseCaptured)
			{
				if (This.Selection.IsEmpty)
				{
					UIElement uielementWhenMouseOver = TextEditorMouse.GetUIElementWhenMouseOver(This, mouseMovePoint);
					if (uielementWhenMouseOver != null && uielementWhenMouseOver.IsEnabled)
					{
						cursor = Cursors.Arrow;
					}
				}
				else if (This.UiScope.IsFocused && This.Selection.Contains(mouseMovePoint))
				{
					cursor = Cursors.Arrow;
				}
			}
			if (cursor != This._cursor)
			{
				This._cursor = cursor;
				Mouse.UpdateCursor();
			}
		}

		// Token: 0x060055F9 RID: 22009 RVA: 0x00265134 File Offset: 0x00264134
		private static UIElement GetUIElementWhenMouseOver(TextEditor This, Point mouseMovePoint)
		{
			ITextPointer textPositionFromPoint = This.TextView.GetTextPositionFromPoint(mouseMovePoint, false);
			if (textPositionFromPoint == null)
			{
				return null;
			}
			if (textPositionFromPoint.GetPointerContext(textPositionFromPoint.LogicalDirection) != TextPointerContext.EmbeddedElement)
			{
				return null;
			}
			ITextPointer textPointer = textPositionFromPoint.GetNextContextPosition(textPositionFromPoint.LogicalDirection);
			LogicalDirection gravity = (textPositionFromPoint.LogicalDirection == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward;
			textPointer = textPointer.CreatePointer(0, gravity);
			Rect rectangleFromTextPosition = This.TextView.GetRectangleFromTextPosition(textPositionFromPoint);
			Rect rectangleFromTextPosition2 = This.TextView.GetRectangleFromTextPosition(textPointer);
			Rect rect = rectangleFromTextPosition;
			rect.Union(rectangleFromTextPosition2);
			if (!rect.Contains(mouseMovePoint))
			{
				return null;
			}
			return textPositionFromPoint.GetAdjacentElement(textPositionFromPoint.LogicalDirection) as UIElement;
		}

		// Token: 0x060055FA RID: 22010 RVA: 0x002651CC File Offset: 0x002641CC
		private static bool IsPointWithinRenderScope(TextEditor textEditor, Point point)
		{
			DependencyObject parent = textEditor.TextContainer.Parent;
			UIElement renderScope = textEditor.TextView.RenderScope;
			CaretElement caretElement = textEditor.Selection.CaretElement;
			HitTestResult hitTestResult = VisualTreeHelper.HitTest(textEditor.UiScope, point);
			if (hitTestResult != null)
			{
				bool flag = false;
				if (hitTestResult.VisualHit is Visual)
				{
					flag = ((Visual)hitTestResult.VisualHit).IsDescendantOf(renderScope);
				}
				if (hitTestResult.VisualHit is Visual3D)
				{
					flag = ((Visual3D)hitTestResult.VisualHit).IsDescendantOf(renderScope);
				}
				if (hitTestResult.VisualHit == renderScope || flag || hitTestResult.VisualHit == caretElement)
				{
					return true;
				}
			}
			DependencyObject dependencyObject = textEditor.UiScope.InputHitTest(point) as DependencyObject;
			while (dependencyObject != null)
			{
				if (dependencyObject == parent || dependencyObject == renderScope || dependencyObject == caretElement)
				{
					return true;
				}
				if (dependencyObject is FrameworkElement && ((FrameworkElement)dependencyObject).TemplatedParent == textEditor.UiScope)
				{
					dependencyObject = null;
				}
				else if (dependencyObject is Visual)
				{
					dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
				}
				else if (dependencyObject is FrameworkContentElement)
				{
					dependencyObject = ((FrameworkContentElement)dependencyObject).Parent;
				}
				else
				{
					dependencyObject = null;
				}
			}
			return false;
		}

		// Token: 0x04002F5B RID: 12123
		private static bool _selectionChanged;
	}
}
