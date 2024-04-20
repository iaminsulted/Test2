using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x0200060E RID: 1550
	internal sealed class GeometryWalker : CapacityStreamGeometryContext
	{
		// Token: 0x06004B6C RID: 19308 RVA: 0x00236F55 File Offset: 0x00235F55
		public GeometryWalker(FixedSOMPageConstructor pageConstructor)
		{
			this._pageConstructor = pageConstructor;
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x00236F64 File Offset: 0x00235F64
		public void FindLines(StreamGeometry geometry, bool stroke, bool fill, Matrix trans)
		{
			this._transform = trans;
			this._fill = fill;
			this._stroke = stroke;
			PathGeometry.ParsePathGeometryData(geometry.GetPathGeometryData(), this);
			this.CheckCloseFigure();
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x00236F90 File Offset: 0x00235F90
		private void CheckCloseFigure()
		{
			if (this._needClose)
			{
				if (this._stroke && this._isClosed)
				{
					this._pageConstructor._AddLine(this._startPoint, this._lastPoint, this._transform);
				}
				if (this._fill && this._isFilled)
				{
					this._pageConstructor._ProcessFilledRect(this._transform, new Rect(this._xMin, this._yMin, this._xMax - this._xMin, this._yMax - this._yMin));
				}
				this._needClose = false;
			}
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x0023702C File Offset: 0x0023602C
		private void GatherBounds(Point p)
		{
			if (p.X < this._xMin)
			{
				this._xMin = p.X;
			}
			else if (p.X > this._xMax)
			{
				this._xMax = p.X;
			}
			if (p.Y < this._yMin)
			{
				this._yMin = p.Y;
				return;
			}
			if (p.Y > this._yMax)
			{
				this._yMax = p.Y;
			}
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x002370AC File Offset: 0x002360AC
		public override void BeginFigure(Point startPoint, bool isFilled, bool isClosed)
		{
			this.CheckCloseFigure();
			this._startPoint = startPoint;
			this._lastPoint = startPoint;
			this._isClosed = isClosed;
			this._isFilled = isFilled;
			if (this._isFilled && this._fill)
			{
				this._xMin = (this._xMax = startPoint.X);
				this._yMin = (this._yMax = startPoint.Y);
			}
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x00237118 File Offset: 0x00236118
		public override void LineTo(Point point, bool isStroked, bool isSmoothJoin)
		{
			if (isStroked && this._stroke)
			{
				this._pageConstructor._AddLine(this._lastPoint, point, this._transform);
			}
			if (this._isFilled && this._fill)
			{
				this.GatherBounds(point);
			}
			this._lastPoint = point;
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x00237166 File Offset: 0x00236166
		public override void QuadraticBezierTo(Point point1, Point point2, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = point2;
			this._fill = false;
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x00237176 File Offset: 0x00236176
		public override void BezierTo(Point point1, Point point2, Point point3, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = point3;
			this._fill = false;
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x00237188 File Offset: 0x00236188
		public override void PolyLineTo(IList<Point> points, bool isStroked, bool isSmoothJoin)
		{
			if (isStroked && this._stroke)
			{
				for (int i = 0; i < points.Count; i++)
				{
					this._pageConstructor._AddLine(this._lastPoint, points[i], this._transform);
					this._lastPoint = points[i];
				}
			}
			else
			{
				this._lastPoint = points[points.Count - 1];
			}
			if (this._isFilled && this._fill)
			{
				for (int j = 0; j < points.Count; j++)
				{
					this.GatherBounds(points[j]);
				}
			}
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x00237220 File Offset: 0x00236220
		public override void PolyQuadraticBezierTo(IList<Point> points, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = points[points.Count - 1];
			this._fill = false;
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x00237220 File Offset: 0x00236220
		public override void PolyBezierTo(IList<Point> points, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = points[points.Count - 1];
			this._fill = false;
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x0023723D File Offset: 0x0023623D
		public override void ArcTo(Point point, Size size, double rotationAngle, bool isLargeArc, SweepDirection sweepDirection, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = point;
			this._fill = false;
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override void SetClosedState(bool closed)
		{
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override void SetFigureCount(int figureCount)
		{
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x0023724D File Offset: 0x0023624D
		internal override void SetSegmentCount(int segmentCount)
		{
			if (segmentCount != 0)
			{
				this._needClose = true;
			}
		}

		// Token: 0x04002783 RID: 10115
		private FixedSOMPageConstructor _pageConstructor;

		// Token: 0x04002784 RID: 10116
		private Matrix _transform;

		// Token: 0x04002785 RID: 10117
		private bool _stroke;

		// Token: 0x04002786 RID: 10118
		private bool _fill;

		// Token: 0x04002787 RID: 10119
		private Point _startPoint;

		// Token: 0x04002788 RID: 10120
		private Point _lastPoint;

		// Token: 0x04002789 RID: 10121
		private bool _isClosed;

		// Token: 0x0400278A RID: 10122
		private bool _isFilled;

		// Token: 0x0400278B RID: 10123
		private double _xMin;

		// Token: 0x0400278C RID: 10124
		private double _xMax;

		// Token: 0x0400278D RID: 10125
		private double _yMin;

		// Token: 0x0400278E RID: 10126
		private double _yMax;

		// Token: 0x0400278F RID: 10127
		private bool _needClose;
	}
}
