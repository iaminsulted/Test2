using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;
using MS.Internal.Controls;

namespace MS.Internal.Ink
{
	// Token: 0x02000188 RID: 392
	internal sealed class InkCanvasSelection
	{
		// Token: 0x06000CEB RID: 3307 RVA: 0x00131D78 File Offset: 0x00130D78
		internal InkCanvasSelection(InkCanvas inkCanvas)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			this._inkCanvas = inkCanvas;
			this._inkCanvas.FeedbackAdorner.UpdateBounds(Rect.Empty);
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00131DAA File Offset: 0x00130DAA
		internal StrokeCollection SelectedStrokes
		{
			get
			{
				if (this._selectedStrokes == null)
				{
					this._selectedStrokes = new StrokeCollection();
					this._areStrokesChanged = true;
				}
				return this._selectedStrokes;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x00131DCC File Offset: 0x00130DCC
		internal ReadOnlyCollection<UIElement> SelectedElements
		{
			get
			{
				if (this._selectedElements == null)
				{
					this._selectedElements = new List<UIElement>();
				}
				return new ReadOnlyCollection<UIElement>(this._selectedElements);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00131DEC File Offset: 0x00130DEC
		internal bool HasSelection
		{
			get
			{
				return this.SelectedStrokes.Count != 0 || this.SelectedElements.Count != 0;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00131E0B File Offset: 0x00130E0B
		internal Rect SelectionBounds
		{
			get
			{
				return Rect.Union(this.GetStrokesBounds(), this.GetElementsUnionBounds());
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00131E20 File Offset: 0x00130E20
		internal void StartFeedbackAdorner(Rect feedbackRect, InkCanvasSelectionHitResult activeSelectionHitResult)
		{
			this._activeSelectionHitResult = new InkCanvasSelectionHitResult?(activeSelectionHitResult);
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._inkCanvas.InnerCanvas);
			InkCanvasFeedbackAdorner feedbackAdorner = this._inkCanvas.FeedbackAdorner;
			adornerLayer.Add(feedbackAdorner);
			feedbackAdorner.UpdateBounds(feedbackRect);
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00131E62 File Offset: 0x00130E62
		internal void UpdateFeedbackAdorner(Rect feedbackRect)
		{
			this._inkCanvas.FeedbackAdorner.UpdateBounds(feedbackRect);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00131E78 File Offset: 0x00130E78
		internal void EndFeedbackAdorner(Rect finalRectangle)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._inkCanvas.InnerCanvas);
			InkCanvasFeedbackAdorner feedbackAdorner = this._inkCanvas.FeedbackAdorner;
			feedbackAdorner.UpdateBounds(Rect.Empty);
			adornerLayer.Remove(feedbackAdorner);
			this.CommitChanges(finalRectangle, true);
			this._activeSelectionHitResult = null;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00131EC8 File Offset: 0x00130EC8
		internal void Select(StrokeCollection strokes, IList<UIElement> elements, bool raiseSelectionChanged)
		{
			bool flag;
			bool flag2;
			this.SelectionIsDifferentThanCurrent(strokes, out flag, elements, out flag2);
			if (flag || flag2)
			{
				if (flag && this.SelectedStrokes.Count != 0)
				{
					this.QuitListeningToStrokeChanges();
					int count = this.SelectedStrokes.Count;
					for (int i = 0; i < count; i++)
					{
						this.SelectedStrokes[i].IsSelected = false;
					}
				}
				this._selectedStrokes = strokes;
				this._areStrokesChanged = true;
				this._selectedElements = new List<UIElement>(elements);
				if (this._inkCanvas.ActiveEditingMode == InkCanvasEditingMode.Select)
				{
					int count = strokes.Count;
					for (int j = 0; j < count; j++)
					{
						strokes[j].IsSelected = true;
					}
				}
				this.UpdateCanvasLayoutUpdatedHandler();
				this.UpdateSelectionAdorner();
				this.ListenToStrokeChanges();
				if (raiseSelectionChanged)
				{
					this._inkCanvas.RaiseSelectionChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00131FA0 File Offset: 0x00130FA0
		internal void CommitChanges(Rect finalRectangle, bool raiseEvent)
		{
			Rect selectionBounds = this.SelectionBounds;
			if (selectionBounds.IsEmpty)
			{
				return;
			}
			try
			{
				this.QuitListeningToStrokeChanges();
				if (raiseEvent)
				{
					if (!DoubleUtil.AreClose(finalRectangle.Height, selectionBounds.Height) || !DoubleUtil.AreClose(finalRectangle.Width, selectionBounds.Width))
					{
						this.CommitResizeChange(finalRectangle);
					}
					else if (!DoubleUtil.AreClose(finalRectangle.Top, selectionBounds.Top) || !DoubleUtil.AreClose(finalRectangle.Left, selectionBounds.Left))
					{
						this.CommitMoveChange(finalRectangle);
					}
				}
				else
				{
					this.MoveSelection(selectionBounds, finalRectangle);
				}
			}
			finally
			{
				this.ListenToStrokeChanges();
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00132050 File Offset: 0x00131050
		internal void RemoveElement(UIElement removedElement)
		{
			if (this._selectedElements == null || this._selectedElements.Count == 0)
			{
				return;
			}
			if (this._selectedElements.Remove(removedElement) && this._selectedElements.Count == 0)
			{
				this.UpdateCanvasLayoutUpdatedHandler();
				this.UpdateSelectionAdorner();
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0013208F File Offset: 0x0013108F
		internal void UpdateElementBounds(UIElement element, Matrix transform)
		{
			this.UpdateElementBounds(element, element, transform);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0013209C File Offset: 0x0013109C
		internal void UpdateElementBounds(UIElement originalElement, UIElement updatedElement, Matrix transform)
		{
			if (originalElement.DependencyObjectType.Id == updatedElement.DependencyObjectType.Id)
			{
				GeneralTransform generalTransform = originalElement.TransformToAncestor(this._inkCanvas.InnerCanvas);
				FrameworkElement frameworkElement = originalElement as FrameworkElement;
				Thickness thickness = default(Thickness);
				Size renderSize;
				if (frameworkElement == null)
				{
					renderSize = originalElement.RenderSize;
				}
				else
				{
					renderSize = new Size(frameworkElement.ActualWidth, frameworkElement.ActualHeight);
					thickness = frameworkElement.Margin;
				}
				Rect rect = new Rect(0.0, 0.0, renderSize.Width, renderSize.Height);
				rect = generalTransform.TransformBounds(rect);
				Rect rect2 = Rect.Transform(rect, transform);
				if (!DoubleUtil.AreClose(rect.Width, rect2.Width))
				{
					if (frameworkElement == null)
					{
						Size renderSize2 = originalElement.RenderSize;
						renderSize2.Width = rect2.Width;
						updatedElement.RenderSize = renderSize2;
					}
					else
					{
						((FrameworkElement)updatedElement).Width = rect2.Width;
					}
				}
				if (!DoubleUtil.AreClose(rect.Height, rect2.Height))
				{
					if (frameworkElement == null)
					{
						Size renderSize3 = originalElement.RenderSize;
						renderSize3.Height = rect2.Height;
						updatedElement.RenderSize = renderSize3;
					}
					else
					{
						((FrameworkElement)updatedElement).Height = rect2.Height;
					}
				}
				double left = InkCanvas.GetLeft(originalElement);
				double top = InkCanvas.GetTop(originalElement);
				double right = InkCanvas.GetRight(originalElement);
				double bottom = InkCanvas.GetBottom(originalElement);
				Point point = default(Point);
				if (!double.IsNaN(left))
				{
					point.X = left;
				}
				else if (!double.IsNaN(right))
				{
					point.X = right;
				}
				if (!double.IsNaN(top))
				{
					point.Y = top;
				}
				else if (!double.IsNaN(bottom))
				{
					point.Y = bottom;
				}
				Point point2 = point * transform;
				if (!double.IsNaN(left))
				{
					InkCanvas.SetLeft(updatedElement, point2.X - thickness.Left);
				}
				else if (!double.IsNaN(right))
				{
					InkCanvas.SetRight(updatedElement, right - (point2.X - point.X));
				}
				else
				{
					InkCanvas.SetLeft(updatedElement, point2.X - thickness.Left);
				}
				if (!double.IsNaN(top))
				{
					InkCanvas.SetTop(updatedElement, point2.Y - thickness.Top);
					return;
				}
				if (!double.IsNaN(bottom))
				{
					InkCanvas.SetBottom(updatedElement, bottom - (point2.Y - point.Y));
					return;
				}
				InkCanvas.SetTop(updatedElement, point2.Y - thickness.Top);
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00132301 File Offset: 0x00131301
		internal void TransformStrokes(StrokeCollection strokes, Matrix matrix)
		{
			strokes.Transform(matrix, false);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0013230C File Offset: 0x0013130C
		internal InkCanvasSelectionHitResult HitTestSelection(Point pointOnInkCanvas)
		{
			if (this._activeSelectionHitResult != null)
			{
				return this._activeSelectionHitResult.Value;
			}
			if (!this.HasSelection)
			{
				return InkCanvasSelectionHitResult.None;
			}
			Point point = this._inkCanvas.TransformToDescendant(this._inkCanvas.SelectionAdorner).Transform(pointOnInkCanvas);
			InkCanvasSelectionHitResult inkCanvasSelectionHitResult = this._inkCanvas.SelectionAdorner.SelectionHandleHitTest(point);
			if (inkCanvasSelectionHitResult == InkCanvasSelectionHitResult.Selection && this.SelectedElements.Count == 1 && this.SelectedStrokes.Count == 0)
			{
				Point pointOnInnerCanvas = this._inkCanvas.TransformToDescendant(this._inkCanvas.InnerCanvas).Transform(pointOnInkCanvas);
				if (this.HasHitSingleSelectedElement(pointOnInnerCanvas))
				{
					inkCanvasSelectionHitResult = InkCanvasSelectionHitResult.None;
				}
			}
			return inkCanvasSelectionHitResult;
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x001323B4 File Offset: 0x001313B4
		internal void SelectionIsDifferentThanCurrent(StrokeCollection strokes, out bool strokesAreDifferent, IList<UIElement> elements, out bool elementsAreDifferent)
		{
			strokesAreDifferent = false;
			elementsAreDifferent = false;
			if (this.SelectedStrokes.Count == 0)
			{
				if (strokes.Count > 0)
				{
					strokesAreDifferent = true;
				}
			}
			else if (!InkCanvasSelection.StrokesAreEqual(this.SelectedStrokes, strokes))
			{
				strokesAreDifferent = true;
			}
			if (this.SelectedElements.Count == 0)
			{
				if (elements.Count > 0)
				{
					elementsAreDifferent = true;
					return;
				}
			}
			else if (!InkCanvasSelection.FrameworkElementArraysAreEqual(elements, this.SelectedElements))
			{
				elementsAreDifferent = true;
			}
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00132424 File Offset: 0x00131424
		private bool HasHitSingleSelectedElement(Point pointOnInnerCanvas)
		{
			bool result = false;
			if (this.SelectedElements.Count == 1)
			{
				IEnumerator<Rect> enumerator = this.SelectedElementsBoundsEnumerator.GetEnumerator();
				if (enumerator.MoveNext())
				{
					Rect rect = enumerator.Current;
					result = rect.Contains(pointOnInnerCanvas);
				}
			}
			return result;
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00132468 File Offset: 0x00131468
		private void QuitListeningToStrokeChanges()
		{
			if (this._inkCanvas.Strokes != null)
			{
				this._inkCanvas.Strokes.StrokesChanged -= this.OnStrokeCollectionChanged;
			}
			foreach (Stroke stroke in this.SelectedStrokes)
			{
				stroke.Invalidated -= this.OnStrokeInvalidated;
			}
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x001324E8 File Offset: 0x001314E8
		private void ListenToStrokeChanges()
		{
			if (this._inkCanvas.Strokes != null)
			{
				this._inkCanvas.Strokes.StrokesChanged += this.OnStrokeCollectionChanged;
			}
			foreach (Stroke stroke in this.SelectedStrokes)
			{
				stroke.Invalidated += this.OnStrokeInvalidated;
			}
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00132568 File Offset: 0x00131568
		private void CommitMoveChange(Rect finalRectangle)
		{
			Rect selectionBounds = this.SelectionBounds;
			InkCanvasSelectionEditingEventArgs inkCanvasSelectionEditingEventArgs = new InkCanvasSelectionEditingEventArgs(selectionBounds, finalRectangle);
			this._inkCanvas.RaiseSelectionMoving(inkCanvasSelectionEditingEventArgs);
			if (!inkCanvasSelectionEditingEventArgs.Cancel)
			{
				if (finalRectangle != inkCanvasSelectionEditingEventArgs.NewRectangle)
				{
					finalRectangle = inkCanvasSelectionEditingEventArgs.NewRectangle;
				}
				this.MoveSelection(selectionBounds, finalRectangle);
				this._inkCanvas.RaiseSelectionMoved(EventArgs.Empty);
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x001325C8 File Offset: 0x001315C8
		private void CommitResizeChange(Rect finalRectangle)
		{
			Rect selectionBounds = this.SelectionBounds;
			InkCanvasSelectionEditingEventArgs inkCanvasSelectionEditingEventArgs = new InkCanvasSelectionEditingEventArgs(selectionBounds, finalRectangle);
			this._inkCanvas.RaiseSelectionResizing(inkCanvasSelectionEditingEventArgs);
			if (!inkCanvasSelectionEditingEventArgs.Cancel)
			{
				if (finalRectangle != inkCanvasSelectionEditingEventArgs.NewRectangle)
				{
					finalRectangle = inkCanvasSelectionEditingEventArgs.NewRectangle;
				}
				this.MoveSelection(selectionBounds, finalRectangle);
				this._inkCanvas.RaiseSelectionResized(EventArgs.Empty);
			}
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00132628 File Offset: 0x00131628
		private void MoveSelection(Rect previousRect, Rect newRect)
		{
			Matrix matrix = InkCanvasSelection.MapRectToRect(newRect, previousRect);
			int count = this.SelectedElements.Count;
			IList<UIElement> selectedElements = this.SelectedElements;
			for (int i = 0; i < count; i++)
			{
				this.UpdateElementBounds(selectedElements[i], matrix);
			}
			if (this.SelectedStrokes.Count > 0)
			{
				this.TransformStrokes(this.SelectedStrokes, matrix);
				this._areStrokesChanged = true;
			}
			if (this.SelectedElements.Count == 0)
			{
				this.UpdateSelectionAdorner();
			}
			this._inkCanvas.BringIntoView(newRect);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x001326AB File Offset: 0x001316AB
		private void OnCanvasLayoutUpdated(object sender, EventArgs e)
		{
			this.UpdateSelectionAdorner();
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x001326B3 File Offset: 0x001316B3
		private void OnStrokeInvalidated(object sender, EventArgs e)
		{
			this.OnStrokeCollectionChanged(sender, new StrokeCollectionChangedEventArgs(new StrokeCollection(), new StrokeCollection()));
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x001326CC File Offset: 0x001316CC
		private void OnStrokeCollectionChanged(object target, StrokeCollectionChangedEventArgs e)
		{
			if (e.Added.Count != 0 && e.Removed.Count == 0)
			{
				return;
			}
			foreach (Stroke stroke in e.Removed)
			{
				if (this.SelectedStrokes.Contains(stroke))
				{
					stroke.Invalidated -= this.OnStrokeInvalidated;
					stroke.IsSelected = false;
					this.SelectedStrokes.Remove(stroke);
				}
			}
			this._areStrokesChanged = true;
			this.UpdateSelectionAdorner();
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00132770 File Offset: 0x00131770
		private Rect GetStrokesBounds()
		{
			if (this._areStrokesChanged)
			{
				this._cachedStrokesBounds = ((this.SelectedStrokes.Count != 0) ? this.SelectedStrokes.GetBounds() : Rect.Empty);
				this._areStrokesChanged = false;
			}
			return this._cachedStrokesBounds;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x001327AC File Offset: 0x001317AC
		private List<Rect> GetElementsBounds()
		{
			List<Rect> list = new List<Rect>();
			if (this.SelectedElements.Count != 0)
			{
				foreach (Rect item in this.SelectedElementsBoundsEnumerator)
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00132810 File Offset: 0x00131810
		private Rect GetElementsUnionBounds()
		{
			if (this.SelectedElements.Count == 0)
			{
				return Rect.Empty;
			}
			Rect empty = Rect.Empty;
			foreach (Rect rect in this.SelectedElementsBoundsEnumerator)
			{
				empty.Union(rect);
			}
			return empty;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00132878 File Offset: 0x00131878
		private void UpdateSelectionAdorner()
		{
			if (this._inkCanvas.ActiveEditingMode != InkCanvasEditingMode.None)
			{
				this._inkCanvas.SelectionAdorner.UpdateSelectionWireFrame(this.GetStrokesBounds(), this.GetElementsBounds());
			}
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x001328A4 File Offset: 0x001318A4
		private void EnusreElementsBounds()
		{
			InkCanvasInnerCanvas innerCanvas = this._inkCanvas.InnerCanvas;
			if (!innerCanvas.IsMeasureValid || !innerCanvas.IsArrangeValid)
			{
				innerCanvas.UpdateLayout();
			}
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x001328D4 File Offset: 0x001318D4
		private static Matrix MapRectToRect(Rect target, Rect source)
		{
			if (source.IsEmpty)
			{
				throw new ArgumentOutOfRangeException("source", SR.Get("InvalidDiameter"));
			}
			double num = target.Width / source.Width;
			double offsetX = target.Left - num * source.Left;
			double num2 = target.Height / source.Height;
			double offsetY = target.Top - num2 * source.Top;
			return new Matrix(num, 0.0, 0.0, num2, offsetX, offsetY);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00132960 File Offset: 0x00131960
		private void UpdateCanvasLayoutUpdatedHandler()
		{
			if (this.SelectedElements.Count != 0)
			{
				if (this._layoutUpdatedHandler == null)
				{
					this._layoutUpdatedHandler = new EventHandler(this.OnCanvasLayoutUpdated);
					this._inkCanvas.InnerCanvas.LayoutUpdated += this._layoutUpdatedHandler;
					return;
				}
			}
			else if (this._layoutUpdatedHandler != null)
			{
				this._inkCanvas.InnerCanvas.LayoutUpdated -= this._layoutUpdatedHandler;
				this._layoutUpdatedHandler = null;
			}
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x001329D0 File Offset: 0x001319D0
		private static bool StrokesAreEqual(StrokeCollection strokes1, StrokeCollection strokes2)
		{
			if (strokes1 == null && strokes2 == null)
			{
				return true;
			}
			if (strokes1 == null || strokes2 == null)
			{
				return false;
			}
			if (strokes1.Count != strokes2.Count)
			{
				return false;
			}
			foreach (Stroke item in strokes1)
			{
				if (!strokes2.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00132A44 File Offset: 0x00131A44
		private static bool FrameworkElementArraysAreEqual(IList<UIElement> elements1, IList<UIElement> elements2)
		{
			if (elements1 == null && elements2 == null)
			{
				return true;
			}
			if (elements1 == null || elements2 == null)
			{
				return false;
			}
			if (elements1.Count != elements2.Count)
			{
				return false;
			}
			foreach (UIElement item in elements1)
			{
				if (!elements2.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00132AB8 File Offset: 0x00131AB8
		private IEnumerable<Rect> SelectedElementsBoundsEnumerator
		{
			get
			{
				this.EnusreElementsBounds();
				InkCanvasInnerCanvas innerCanvas = this._inkCanvas.InnerCanvas;
				foreach (UIElement uielement in this.SelectedElements)
				{
					GeneralTransform generalTransform = uielement.TransformToAncestor(innerCanvas);
					Size renderSize = uielement.RenderSize;
					Rect rect = new Rect(0.0, 0.0, renderSize.Width, renderSize.Height);
					rect = generalTransform.TransformBounds(rect);
					yield return rect;
				}
				IEnumerator<UIElement> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x040009AE RID: 2478
		private InkCanvas _inkCanvas;

		// Token: 0x040009AF RID: 2479
		private StrokeCollection _selectedStrokes;

		// Token: 0x040009B0 RID: 2480
		private Rect _cachedStrokesBounds;

		// Token: 0x040009B1 RID: 2481
		private bool _areStrokesChanged;

		// Token: 0x040009B2 RID: 2482
		private List<UIElement> _selectedElements;

		// Token: 0x040009B3 RID: 2483
		private EventHandler _layoutUpdatedHandler;

		// Token: 0x040009B4 RID: 2484
		private InkCanvasSelectionHitResult? _activeSelectionHitResult;
	}
}
