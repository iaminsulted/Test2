using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.Controls;

namespace MS.Internal.Ink
{
	// Token: 0x0200018C RID: 396
	internal sealed class LassoSelectionBehavior : StylusEditingBehavior
	{
		// Token: 0x06000D29 RID: 3369 RVA: 0x0013170C File Offset: 0x0013070C
		internal LassoSelectionBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x001334D4 File Offset: 0x001324D4
		protected override void OnSwitchToMode(InkCanvasEditingMode mode)
		{
			switch (mode)
			{
			case InkCanvasEditingMode.None:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				break;
			case InkCanvasEditingMode.Ink:
			case InkCanvasEditingMode.GestureOnly:
			case InkCanvasEditingMode.InkAndGesture:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			case InkCanvasEditingMode.Select:
				break;
			case InkCanvasEditingMode.EraseByPoint:
			case InkCanvasEditingMode.EraseByStroke:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00133548 File Offset: 0x00132548
		protected override void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
			this._disableLasso = false;
			bool flag = false;
			List<Point> list = new List<Point>();
			for (int i = 0; i < stylusPoints.Count; i++)
			{
				Point point = (Point)stylusPoints[i];
				if (i == 0)
				{
					this._startPoint = point;
					list.Add(point);
				}
				else if (!flag)
				{
					if (DoubleUtil.GreaterThan((point - this._startPoint).LengthSquared, 49.0))
					{
						list.Add(point);
						flag = true;
					}
				}
				else
				{
					list.Add(point);
				}
			}
			if (flag)
			{
				this.StartLasso(list);
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x001335DC File Offset: 0x001325DC
		protected override void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
			if (this._lassoHelper != null)
			{
				List<Point> list = new List<Point>();
				for (int i = 0; i < stylusPoints.Count; i++)
				{
					list.Add((Point)stylusPoints[i]);
				}
				Point[] array = this._lassoHelper.AddPoints(list);
				if (array.Length != 0)
				{
					this._incrementalLassoHitTester.AddPoints(array);
					return;
				}
			}
			else if (!this._disableLasso)
			{
				bool flag = false;
				List<Point> list2 = new List<Point>();
				for (int j = 0; j < stylusPoints.Count; j++)
				{
					Point point = (Point)stylusPoints[j];
					if (!flag)
					{
						if (DoubleUtil.GreaterThan((point - this._startPoint).LengthSquared, 49.0))
						{
							list2.Add(point);
							flag = true;
						}
					}
					else
					{
						list2.Add(point);
					}
				}
				if (flag)
				{
					this.StartLasso(list2);
				}
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x001336BC File Offset: 0x001326BC
		protected override void StylusInputEnd(bool commit)
		{
			StrokeCollection strokeCollection = new StrokeCollection();
			List<UIElement> list = new List<UIElement>();
			if (this._lassoHelper != null)
			{
				strokeCollection = base.InkCanvas.EndDynamicSelection(this._lassoHelper.Visual);
				list = this.HitTestForElements();
				this._incrementalLassoHitTester.SelectionChanged -= this.OnSelectionChanged;
				this._incrementalLassoHitTester.EndHitTesting();
				this._incrementalLassoHitTester = null;
				this._lassoHelper = null;
			}
			else
			{
				Stroke stroke;
				UIElement uielement;
				this.TapSelectObject(this._startPoint, out stroke, out uielement);
				if (stroke != null)
				{
					strokeCollection = new StrokeCollection();
					strokeCollection.Add(stroke);
				}
				else if (uielement != null)
				{
					list.Add(uielement);
				}
			}
			base.SelfDeactivate();
			if (commit)
			{
				base.InkCanvas.ChangeInkCanvasSelection(strokeCollection, list.ToArray());
			}
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00133774 File Offset: 0x00132774
		protected override void OnCommitWithoutStylusInput(bool commit)
		{
			base.SelfDeactivate();
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0013377C File Offset: 0x0013277C
		protected override Cursor GetCurrentCursor()
		{
			return Cursors.Cross;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00133783 File Offset: 0x00132783
		private void OnSelectionChanged(object sender, LassoSelectionChangedEventArgs e)
		{
			base.InkCanvas.UpdateDynamicSelection(e.SelectedStrokes, e.DeselectedStrokes);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0013379C File Offset: 0x0013279C
		private List<UIElement> HitTestForElements()
		{
			List<UIElement> list = new List<UIElement>();
			if (base.InkCanvas.Children.Count == 0)
			{
				return list;
			}
			for (int i = 0; i < base.InkCanvas.Children.Count; i++)
			{
				UIElement uiElement = base.InkCanvas.Children[i];
				this.HitTestElement(base.InkCanvas.InnerCanvas, uiElement, list);
			}
			return list;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00133804 File Offset: 0x00132804
		private void HitTestElement(InkCanvasInnerCanvas parent, UIElement uiElement, List<UIElement> elementsToSelect)
		{
			LassoSelectionBehavior.ElementCornerPoints transformedElementCornerPoints = LassoSelectionBehavior.GetTransformedElementCornerPoints(parent, uiElement);
			if (transformedElementCornerPoints.Set)
			{
				Point[] points = this.GeneratePointGrid(transformedElementCornerPoints);
				if (this._lassoHelper.ArePointsInLasso(points, 60))
				{
					elementsToSelect.Add(uiElement);
				}
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00133840 File Offset: 0x00132840
		private static LassoSelectionBehavior.ElementCornerPoints GetTransformedElementCornerPoints(InkCanvasInnerCanvas canvas, UIElement childElement)
		{
			LassoSelectionBehavior.ElementCornerPoints result = default(LassoSelectionBehavior.ElementCornerPoints);
			result.Set = false;
			if (childElement.Visibility != Visibility.Visible)
			{
				return result;
			}
			GeneralTransform generalTransform = childElement.TransformToAncestor(canvas);
			generalTransform.TryTransform(new Point(0.0, 0.0), out result.UpperLeft);
			generalTransform.TryTransform(new Point(childElement.RenderSize.Width, 0.0), out result.UpperRight);
			generalTransform.TryTransform(new Point(0.0, childElement.RenderSize.Height), out result.LowerLeft);
			generalTransform.TryTransform(new Point(childElement.RenderSize.Width, childElement.RenderSize.Height), out result.LowerRight);
			result.Set = true;
			return result;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00133920 File Offset: 0x00132920
		private Point[] GeneratePointGrid(LassoSelectionBehavior.ElementCornerPoints elementPoints)
		{
			if (!elementPoints.Set)
			{
				return new Point[0];
			}
			ArrayList arrayList = new ArrayList();
			this.UpdatePointDistances(elementPoints);
			arrayList.Add(elementPoints.UpperLeft);
			arrayList.Add(elementPoints.UpperRight);
			this.FillInPoints(arrayList, elementPoints.UpperLeft, elementPoints.UpperRight);
			arrayList.Add(elementPoints.LowerLeft);
			arrayList.Add(elementPoints.LowerRight);
			this.FillInPoints(arrayList, elementPoints.LowerLeft, elementPoints.LowerRight);
			this.FillInGrid(arrayList, elementPoints.UpperLeft, elementPoints.UpperRight, elementPoints.LowerRight, elementPoints.LowerLeft);
			Point[] array = new Point[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x001339EC File Offset: 0x001329EC
		private void FillInPoints(ArrayList pointArray, Point point1, Point point2)
		{
			if (!this.PointsAreCloseEnough(point1, point2))
			{
				Point point3 = LassoSelectionBehavior.GeneratePointBetweenPoints(point1, point2);
				pointArray.Add(point3);
				if (!this.PointsAreCloseEnough(point1, point3))
				{
					this.FillInPoints(pointArray, point1, point3);
				}
				if (!this.PointsAreCloseEnough(point3, point2))
				{
					this.FillInPoints(pointArray, point3, point2);
				}
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00133A40 File Offset: 0x00132A40
		private void FillInGrid(ArrayList pointArray, Point upperLeft, Point upperRight, Point lowerRight, Point lowerLeft)
		{
			if (!this.PointsAreCloseEnough(upperLeft, lowerLeft))
			{
				Point point = LassoSelectionBehavior.GeneratePointBetweenPoints(upperLeft, lowerLeft);
				Point point2 = LassoSelectionBehavior.GeneratePointBetweenPoints(upperRight, lowerRight);
				pointArray.Add(point);
				pointArray.Add(point2);
				this.FillInPoints(pointArray, point, point2);
				if (!this.PointsAreCloseEnough(upperLeft, point))
				{
					this.FillInGrid(pointArray, upperLeft, upperRight, point2, point);
				}
				if (!this.PointsAreCloseEnough(point, lowerLeft))
				{
					this.FillInGrid(pointArray, point, point2, lowerRight, lowerLeft);
				}
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00133ABC File Offset: 0x00132ABC
		private static Point GeneratePointBetweenPoints(Point point1, Point point2)
		{
			double num = (point1.X > point2.X) ? point1.X : point2.X;
			double num2 = (point1.X < point2.X) ? point1.X : point2.X;
			double num3 = (point1.Y > point2.Y) ? point1.Y : point2.Y;
			double num4 = (point1.Y < point2.Y) ? point1.Y : point2.Y;
			return new Point(num2 + (num - num2) * 0.5, num4 + (num3 - num4) * 0.5);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00133B70 File Offset: 0x00132B70
		private bool PointsAreCloseEnough(Point point1, Point point2)
		{
			double num = point1.X - point2.X;
			double num2 = point1.Y - point2.Y;
			return num < this._xDiff && num > -this._xDiff && num2 < this._yDiff && num2 > -this._yDiff;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00133BC8 File Offset: 0x00132BC8
		private void UpdatePointDistances(LassoSelectionBehavior.ElementCornerPoints elementPoints)
		{
			double num = elementPoints.UpperLeft.X - elementPoints.UpperRight.X;
			if (num < 0.0)
			{
				num = -num;
			}
			double num2 = elementPoints.UpperLeft.Y - elementPoints.LowerLeft.Y;
			if (num2 < 0.0)
			{
				num2 = -num2;
			}
			this._xDiff = num * 0.25;
			if (this._xDiff > 50.0)
			{
				this._xDiff = 50.0;
			}
			else if (this._xDiff < 15.0)
			{
				this._xDiff = 15.0;
			}
			this._yDiff = num2 * 0.25;
			if (this._yDiff > 50.0)
			{
				this._yDiff = 50.0;
				return;
			}
			if (this._yDiff < 15.0)
			{
				this._yDiff = 15.0;
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00133CCC File Offset: 0x00132CCC
		private void StartLasso(List<Point> points)
		{
			if (base.InkCanvas.ClearSelectionRaiseSelectionChanging() && base.EditingCoordinator.ActiveEditingMode == InkCanvasEditingMode.Select)
			{
				this._incrementalLassoHitTester = base.InkCanvas.Strokes.GetIncrementalLassoHitTester(80);
				this._incrementalLassoHitTester.SelectionChanged += this.OnSelectionChanged;
				this._lassoHelper = new LassoHelper();
				base.InkCanvas.BeginDynamicSelection(this._lassoHelper.Visual);
				Point[] array = this._lassoHelper.AddPoints(points);
				if (array.Length != 0)
				{
					this._incrementalLassoHitTester.AddPoints(array);
					return;
				}
			}
			else
			{
				this._disableLasso = true;
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00133D6C File Offset: 0x00132D6C
		private void TapSelectObject(Point point, out Stroke tappedStroke, out UIElement tappedElement)
		{
			tappedStroke = null;
			tappedElement = null;
			StrokeCollection strokeCollection = base.InkCanvas.Strokes.HitTest(point, 5.0);
			if (strokeCollection.Count > 0)
			{
				tappedStroke = strokeCollection[strokeCollection.Count - 1];
				return;
			}
			Point point2 = base.InkCanvas.TransformToVisual(base.InkCanvas.InnerCanvas).Transform(point);
			tappedElement = base.InkCanvas.InnerCanvas.HitTestOnElements(point2);
		}

		// Token: 0x040009CC RID: 2508
		private Point _startPoint;

		// Token: 0x040009CD RID: 2509
		private bool _disableLasso;

		// Token: 0x040009CE RID: 2510
		private LassoHelper _lassoHelper;

		// Token: 0x040009CF RID: 2511
		private IncrementalLassoHitTester _incrementalLassoHitTester;

		// Token: 0x040009D0 RID: 2512
		private double _xDiff;

		// Token: 0x040009D1 RID: 2513
		private double _yDiff;

		// Token: 0x040009D2 RID: 2514
		private const double _maxThreshold = 50.0;

		// Token: 0x040009D3 RID: 2515
		private const double _minThreshold = 15.0;

		// Token: 0x040009D4 RID: 2516
		private const int _percentIntersectForInk = 80;

		// Token: 0x040009D5 RID: 2517
		private const int _percentIntersectForElements = 60;

		// Token: 0x020009C8 RID: 2504
		private struct ElementCornerPoints
		{
			// Token: 0x04003F9F RID: 16287
			internal Point UpperLeft;

			// Token: 0x04003FA0 RID: 16288
			internal Point UpperRight;

			// Token: 0x04003FA1 RID: 16289
			internal Point LowerRight;

			// Token: 0x04003FA2 RID: 16290
			internal Point LowerLeft;

			// Token: 0x04003FA3 RID: 16291
			internal bool Set;
		}
	}
}
