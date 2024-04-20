using System;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Shapes
{
	// Token: 0x020003FC RID: 1020
	public sealed class Polyline : Shape
	{
		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x001A5599 File Offset: 0x001A4599
		// (set) Token: 0x06002BF6 RID: 11254 RVA: 0x001A55AB File Offset: 0x001A45AB
		public PointCollection Points
		{
			get
			{
				return (PointCollection)base.GetValue(Polyline.PointsProperty);
			}
			set
			{
				base.SetValue(Polyline.PointsProperty, value);
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x001A55B9 File Offset: 0x001A45B9
		// (set) Token: 0x06002BF8 RID: 11256 RVA: 0x001A55CB File Offset: 0x001A45CB
		public FillRule FillRule
		{
			get
			{
				return (FillRule)base.GetValue(Polyline.FillRuleProperty);
			}
			set
			{
				base.SetValue(Polyline.FillRuleProperty, value);
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x001A55DE File Offset: 0x001A45DE
		protected override Geometry DefiningGeometry
		{
			get
			{
				return this._polylineGeometry;
			}
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x001A55E8 File Offset: 0x001A45E8
		internal override void CacheDefiningGeometry()
		{
			PointCollection points = this.Points;
			PathFigure pathFigure = new PathFigure();
			if (points == null)
			{
				this._polylineGeometry = Geometry.Empty;
				return;
			}
			if (points.Count > 0)
			{
				pathFigure.StartPoint = points[0];
				if (points.Count > 1)
				{
					Point[] array = new Point[points.Count - 1];
					for (int i = 1; i < points.Count; i++)
					{
						array[i - 1] = points[i];
					}
					pathFigure.Segments.Add(new PolyLineSegment(array, true));
				}
			}
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures.Add(pathFigure);
			pathGeometry.FillRule = this.FillRule;
			if (pathGeometry.Bounds == Rect.Empty)
			{
				this._polylineGeometry = Geometry.Empty;
				return;
			}
			this._polylineGeometry = pathGeometry;
		}

		// Token: 0x04001B01 RID: 6913
		public static readonly DependencyProperty PointsProperty = DependencyProperty.Register("Points", typeof(PointCollection), typeof(Polyline), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(PointCollection.Empty), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04001B02 RID: 6914
		public static readonly DependencyProperty FillRuleProperty = DependencyProperty.Register("FillRule", typeof(FillRule), typeof(Polyline), new FrameworkPropertyMetadata(FillRule.EvenOdd, FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(ValidateEnums.IsFillRuleValid));

		// Token: 0x04001B03 RID: 6915
		private Geometry _polylineGeometry;
	}
}
