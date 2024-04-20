using System;
using System.ComponentModel;
using System.Windows.Media;

namespace System.Windows.Shapes
{
	// Token: 0x020003FD RID: 1021
	public sealed class Rectangle : Shape
	{
		// Token: 0x06002BFD RID: 11261 RVA: 0x001A574C File Offset: 0x001A474C
		static Rectangle()
		{
			Shape.StretchProperty.OverrideMetadata(typeof(Rectangle), new FrameworkPropertyMetadata(Stretch.Fill));
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06002BFE RID: 11262 RVA: 0x001A57E8 File Offset: 0x001A47E8
		// (set) Token: 0x06002BFF RID: 11263 RVA: 0x001A57FA File Offset: 0x001A47FA
		[TypeConverter(typeof(LengthConverter))]
		public double RadiusX
		{
			get
			{
				return (double)base.GetValue(Rectangle.RadiusXProperty);
			}
			set
			{
				base.SetValue(Rectangle.RadiusXProperty, value);
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06002C00 RID: 11264 RVA: 0x001A580D File Offset: 0x001A480D
		// (set) Token: 0x06002C01 RID: 11265 RVA: 0x001A581F File Offset: 0x001A481F
		[TypeConverter(typeof(LengthConverter))]
		public double RadiusY
		{
			get
			{
				return (double)base.GetValue(Rectangle.RadiusYProperty);
			}
			set
			{
				base.SetValue(Rectangle.RadiusYProperty, value);
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06002C02 RID: 11266 RVA: 0x001A5832 File Offset: 0x001A4832
		public override Geometry RenderedGeometry
		{
			get
			{
				return new RectangleGeometry(this._rect, this.RadiusX, this.RadiusY);
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06002C03 RID: 11267 RVA: 0x001A4F36 File Offset: 0x001A3F36
		public override Transform GeometryTransform
		{
			get
			{
				return Transform.Identity;
			}
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x001A4F40 File Offset: 0x001A3F40
		protected override Size MeasureOverride(Size constraint)
		{
			if (base.Stretch != Stretch.UniformToFill)
			{
				return this.GetNaturalSize();
			}
			double num = constraint.Width;
			double height = constraint.Height;
			if (double.IsInfinity(num) && double.IsInfinity(height))
			{
				return this.GetNaturalSize();
			}
			if (double.IsInfinity(num) || double.IsInfinity(height))
			{
				num = Math.Min(num, height);
			}
			else
			{
				num = Math.Max(num, height);
			}
			return new Size(num, num);
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x001A584C File Offset: 0x001A484C
		protected override Size ArrangeOverride(Size finalSize)
		{
			double strokeThickness = base.GetStrokeThickness();
			double num = strokeThickness / 2.0;
			this._rect = new Rect(num, num, Math.Max(0.0, finalSize.Width - strokeThickness), Math.Max(0.0, finalSize.Height - strokeThickness));
			switch (base.Stretch)
			{
			case Stretch.None:
				this._rect.Width = (this._rect.Height = 0.0);
				break;
			case Stretch.Uniform:
				if (this._rect.Width > this._rect.Height)
				{
					this._rect.Width = this._rect.Height;
				}
				else
				{
					this._rect.Height = this._rect.Width;
				}
				break;
			case Stretch.UniformToFill:
				if (this._rect.Width < this._rect.Height)
				{
					this._rect.Width = this._rect.Height;
				}
				else
				{
					this._rect.Height = this._rect.Width;
				}
				break;
			}
			base.ResetRenderedGeometry();
			return finalSize;
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06002C06 RID: 11270 RVA: 0x001A5832 File Offset: 0x001A4832
		protected override Geometry DefiningGeometry
		{
			get
			{
				return new RectangleGeometry(this._rect, this.RadiusX, this.RadiusY);
			}
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x001A5988 File Offset: 0x001A4988
		protected override void OnRender(DrawingContext drawingContext)
		{
			Pen pen = base.GetPen();
			drawingContext.DrawRoundedRectangle(base.Fill, pen, this._rect, this.RadiusX, this.RadiusY);
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x001A59BC File Offset: 0x001A49BC
		internal override void CacheDefiningGeometry()
		{
			double num = base.GetStrokeThickness() / 2.0;
			this._rect = new Rect(num, num, 0.0, 0.0);
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x001A5185 File Offset: 0x001A4185
		internal override Size GetNaturalSize()
		{
			double strokeThickness = base.GetStrokeThickness();
			return new Size(strokeThickness, strokeThickness);
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x001A59F9 File Offset: 0x001A49F9
		internal override Rect GetDefiningGeometryBounds()
		{
			return this._rect;
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06002C0B RID: 11275 RVA: 0x001A5A01 File Offset: 0x001A4A01
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x04001B04 RID: 6916
		public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register("RadiusX", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04001B05 RID: 6917
		public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register("RadiusY", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04001B06 RID: 6918
		private Rect _rect = Rect.Empty;
	}
}
