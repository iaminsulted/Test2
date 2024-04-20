using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x0200018B RID: 395
	internal class LassoHelper
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x00132F30 File Offset: 0x00131F30
		public Visual Visual
		{
			get
			{
				this.EnsureVisual();
				return this._containerVisual;
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00132F40 File Offset: 0x00131F40
		public Point[] AddPoints(List<Point> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			this.EnsureReady();
			List<Point> list = new List<Point>();
			int count = points.Count;
			for (int i = 0; i < count; i++)
			{
				Point point = points[i];
				if (this._count == 0)
				{
					this.AddLassoPoint(point);
					list.Add(point);
					this._lasso.Add(point);
					this._boundingBox.Union(point);
					this._firstLassoPoint = point;
					this._lastLassoPoint = point;
					this._count++;
				}
				else
				{
					Vector vector = point - this._lastLassoPoint;
					double lengthSquared = vector.LengthSquared;
					if (DoubleUtil.AreClose(49.0, lengthSquared))
					{
						this.AddLassoPoint(point);
						list.Add(point);
						this._lasso.Add(point);
						this._boundingBox.Union(point);
						this._lastLassoPoint = point;
						this._count++;
					}
					else if (49.0 < lengthSquared)
					{
						double num = Math.Sqrt(49.0 / lengthSquared);
						Point lastLassoPoint = this._lastLassoPoint;
						for (double num2 = num; num2 < 1.0; num2 += num)
						{
							Point point2 = lastLassoPoint + vector * num2;
							this.AddLassoPoint(point2);
							list.Add(point2);
							this._lasso.Add(point2);
							this._boundingBox.Union(point2);
							this._lastLassoPoint = point2;
							this._count++;
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x001330E0 File Offset: 0x001320E0
		private void AddLassoPoint(Point lassoPoint)
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = null;
			try
			{
				drawingContext = drawingVisual.RenderOpen();
				drawingContext.DrawEllipse(this._brush, this._pen, lassoPoint, 2.5, 2.5);
			}
			finally
			{
				if (drawingContext != null)
				{
					drawingContext.Close();
				}
			}
			this._containerVisual.Children.Add(drawingVisual);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00133150 File Offset: 0x00132150
		public bool ArePointsInLasso(Point[] points, int percentIntersect)
		{
			int num = points.Length * percentIntersect / 100;
			if (num == 0 || 50 <= points.Length * percentIntersect % 100)
			{
				num++;
			}
			int num2 = 0;
			foreach (Point point in points)
			{
				if (this.Contains(point))
				{
					num2++;
					if (num2 == num)
					{
						break;
					}
				}
			}
			return num2 == num;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x001331AC File Offset: 0x001321AC
		private bool Contains(Point point)
		{
			if (!this._boundingBox.Contains(point))
			{
				return false;
			}
			bool flag = false;
			int num = this._lasso.Count;
			while (--num >= 0)
			{
				if (!DoubleUtil.AreClose(this._lasso[num].Y, point.Y))
				{
					flag = (point.Y < this._lasso[num].Y);
					break;
				}
			}
			bool flag2 = false;
			bool flag3 = false;
			Point point2 = this._lasso[this._lasso.Count - 1];
			for (int i = 0; i < this._lasso.Count; i++)
			{
				Point point3 = this._lasso[i];
				if (DoubleUtil.AreClose(point3.Y, point.Y))
				{
					if (DoubleUtil.AreClose(point3.X, point.X))
					{
						flag2 = true;
						break;
					}
					if (i != 0 && DoubleUtil.AreClose(point2.Y, point.Y) && DoubleUtil.GreaterThanOrClose(point.X, Math.Min(point2.X, point3.X)) && DoubleUtil.LessThanOrClose(point.X, Math.Max(point2.X, point3.X)))
					{
						flag2 = true;
						break;
					}
				}
				else if (flag != point.Y < point3.Y)
				{
					flag = !flag;
					if (DoubleUtil.GreaterThanOrClose(point.X, Math.Max(point2.X, point3.X)))
					{
						flag2 = !flag2;
						if (i == 0 && DoubleUtil.AreClose(point.X, Math.Max(point2.X, point3.X)))
						{
							flag3 = true;
						}
					}
					else if (DoubleUtil.GreaterThanOrClose(point.X, Math.Min(point2.X, point3.X)))
					{
						Vector vector = point3 - point2;
						double value = point2.X + vector.X / vector.Y * (point.Y - point2.Y);
						if (DoubleUtil.GreaterThanOrClose(point.X, value))
						{
							flag2 = !flag2;
							if (i == 0 && DoubleUtil.AreClose(point.X, value))
							{
								flag3 = true;
							}
						}
					}
				}
				point2 = point3;
			}
			return flag2 && !flag3;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00133414 File Offset: 0x00132414
		private void EnsureVisual()
		{
			if (this._containerVisual == null)
			{
				this._containerVisual = new DrawingVisual();
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0013342C File Offset: 0x0013242C
		private void EnsureReady()
		{
			if (!this._isActivated)
			{
				this._isActivated = true;
				this.EnsureVisual();
				this._brush = new SolidColorBrush(LassoHelper.DotColor);
				this._brush.Freeze();
				this._pen = new Pen(new SolidColorBrush(LassoHelper.DotCircumferenceColor), 0.5);
				this._pen.LineJoin = PenLineJoin.Round;
				this._pen.Freeze();
				this._lasso = new List<Point>(100);
				this._boundingBox = Rect.Empty;
				this._count = 0;
			}
		}

		// Token: 0x040009BC RID: 2492
		private DrawingVisual _containerVisual;

		// Token: 0x040009BD RID: 2493
		private Brush _brush;

		// Token: 0x040009BE RID: 2494
		private Pen _pen;

		// Token: 0x040009BF RID: 2495
		private bool _isActivated;

		// Token: 0x040009C0 RID: 2496
		private Point _firstLassoPoint;

		// Token: 0x040009C1 RID: 2497
		private Point _lastLassoPoint;

		// Token: 0x040009C2 RID: 2498
		private int _count;

		// Token: 0x040009C3 RID: 2499
		private List<Point> _lasso;

		// Token: 0x040009C4 RID: 2500
		private Rect _boundingBox;

		// Token: 0x040009C5 RID: 2501
		public const double MinDistanceSquared = 49.0;

		// Token: 0x040009C6 RID: 2502
		private const double DotRadius = 2.5;

		// Token: 0x040009C7 RID: 2503
		private const double DotCircumferenceThickness = 0.5;

		// Token: 0x040009C8 RID: 2504
		private const double ConnectLineThickness = 0.75;

		// Token: 0x040009C9 RID: 2505
		private const double ConnectLineOpacity = 0.75;

		// Token: 0x040009CA RID: 2506
		private static readonly Color DotColor = Colors.Orange;

		// Token: 0x040009CB RID: 2507
		private static readonly Color DotCircumferenceColor = Colors.White;
	}
}
