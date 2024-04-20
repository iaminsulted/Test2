using System;
using System.ComponentModel;
using System.Windows.Media;

namespace System.Windows.Shapes
{
	// Token: 0x020003F9 RID: 1017
	public sealed class Line : Shape
	{
		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06002BDB RID: 11227 RVA: 0x001A51A7 File Offset: 0x001A41A7
		// (set) Token: 0x06002BDC RID: 11228 RVA: 0x001A51B9 File Offset: 0x001A41B9
		[TypeConverter(typeof(LengthConverter))]
		public double X1
		{
			get
			{
				return (double)base.GetValue(Line.X1Property);
			}
			set
			{
				base.SetValue(Line.X1Property, value);
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06002BDD RID: 11229 RVA: 0x001A51CC File Offset: 0x001A41CC
		// (set) Token: 0x06002BDE RID: 11230 RVA: 0x001A51DE File Offset: 0x001A41DE
		[TypeConverter(typeof(LengthConverter))]
		public double Y1
		{
			get
			{
				return (double)base.GetValue(Line.Y1Property);
			}
			set
			{
				base.SetValue(Line.Y1Property, value);
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06002BDF RID: 11231 RVA: 0x001A51F1 File Offset: 0x001A41F1
		// (set) Token: 0x06002BE0 RID: 11232 RVA: 0x001A5203 File Offset: 0x001A4203
		[TypeConverter(typeof(LengthConverter))]
		public double X2
		{
			get
			{
				return (double)base.GetValue(Line.X2Property);
			}
			set
			{
				base.SetValue(Line.X2Property, value);
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06002BE1 RID: 11233 RVA: 0x001A5216 File Offset: 0x001A4216
		// (set) Token: 0x06002BE2 RID: 11234 RVA: 0x001A5228 File Offset: 0x001A4228
		[TypeConverter(typeof(LengthConverter))]
		public double Y2
		{
			get
			{
				return (double)base.GetValue(Line.Y2Property);
			}
			set
			{
				base.SetValue(Line.Y2Property, value);
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06002BE3 RID: 11235 RVA: 0x001A523B File Offset: 0x001A423B
		protected override Geometry DefiningGeometry
		{
			get
			{
				return this._lineGeometry;
			}
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x001A5244 File Offset: 0x001A4244
		internal override void CacheDefiningGeometry()
		{
			Point startPoint = new Point(this.X1, this.Y1);
			Point endPoint = new Point(this.X2, this.Y2);
			this._lineGeometry = new LineGeometry(startPoint, endPoint);
		}

		// Token: 0x04001AF8 RID: 6904
		public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Shape.IsDoubleFinite));

		// Token: 0x04001AF9 RID: 6905
		public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Shape.IsDoubleFinite));

		// Token: 0x04001AFA RID: 6906
		public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Shape.IsDoubleFinite));

		// Token: 0x04001AFB RID: 6907
		public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Shape.IsDoubleFinite));

		// Token: 0x04001AFC RID: 6908
		private LineGeometry _lineGeometry;
	}
}
