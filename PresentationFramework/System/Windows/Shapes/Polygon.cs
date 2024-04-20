using System;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Shapes
{
	// Token: 0x020003FB RID: 1019
	public sealed class Polygon : Shape
	{
		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002BED RID: 11245 RVA: 0x001A5410 File Offset: 0x001A4410
		// (set) Token: 0x06002BEE RID: 11246 RVA: 0x001A5422 File Offset: 0x001A4422
		public PointCollection Points
		{
			get
			{
				return (PointCollection)base.GetValue(Polygon.PointsProperty);
			}
			set
			{
				base.SetValue(Polygon.PointsProperty, value);
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002BEF RID: 11247 RVA: 0x001A5430 File Offset: 0x001A4430
		// (set) Token: 0x06002BF0 RID: 11248 RVA: 0x001A5442 File Offset: 0x001A4442
		public FillRule FillRule
		{
			get
			{
				return (FillRule)base.GetValue(Polygon.FillRuleProperty);
			}
			set
			{
				base.SetValue(Polygon.FillRuleProperty, value);
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06002BF1 RID: 11249 RVA: 0x001A5455 File Offset: 0x001A4455
		protected override Geometry DefiningGeometry
		{
			get
			{
				return this._polygonGeometry;
			}
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x001A5460 File Offset: 0x001A4460
		internal override void CacheDefiningGeometry()
		{
			PointCollection points = this.Points;
			PathFigure pathFigure = new PathFigure();
			if (points == null)
			{
				this._polygonGeometry = Geometry.Empty;
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
				pathFigure.IsClosed = true;
			}
			this._polygonGeometry = new PathGeometry
			{
				Figures = 
				{
					pathFigure
				},
				FillRule = this.FillRule
			};
		}

		// Token: 0x04001AFE RID: 6910
		public static readonly DependencyProperty PointsProperty = DependencyProperty.Register("Points", typeof(PointCollection), typeof(Polygon), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(PointCollection.Empty), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04001AFF RID: 6911
		public static readonly DependencyProperty FillRuleProperty = DependencyProperty.Register("FillRule", typeof(FillRule), typeof(Polygon), new FrameworkPropertyMetadata(FillRule.EvenOdd, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(ValidateEnums.IsFillRuleValid));

		// Token: 0x04001B00 RID: 6912
		private Geometry _polygonGeometry;
	}
}
