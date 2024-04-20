using System;
using System.Windows.Media;

namespace System.Windows.Shapes
{
	// Token: 0x020003F8 RID: 1016
	public sealed class Ellipse : Shape
	{
		// Token: 0x06002BCF RID: 11215 RVA: 0x001A4F0D File Offset: 0x001A3F0D
		static Ellipse()
		{
			Shape.StretchProperty.OverrideMetadata(typeof(Ellipse), new FrameworkPropertyMetadata(Stretch.Fill));
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x001A4F2E File Offset: 0x001A3F2E
		public override Geometry RenderedGeometry
		{
			get
			{
				return this.DefiningGeometry;
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x001A4F36 File Offset: 0x001A3F36
		public override Transform GeometryTransform
		{
			get
			{
				return Transform.Identity;
			}
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x001A4F40 File Offset: 0x001A3F40
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

		// Token: 0x06002BD3 RID: 11219 RVA: 0x001A4FB0 File Offset: 0x001A3FB0
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

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x001A50E9 File Offset: 0x001A40E9
		protected override Geometry DefiningGeometry
		{
			get
			{
				if (this._rect.IsEmpty)
				{
					return Geometry.Empty;
				}
				return new EllipseGeometry(this._rect);
			}
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x001A510C File Offset: 0x001A410C
		protected override void OnRender(DrawingContext drawingContext)
		{
			if (!this._rect.IsEmpty)
			{
				Pen pen = base.GetPen();
				drawingContext.DrawGeometry(base.Fill, pen, new EllipseGeometry(this._rect));
			}
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x001A5148 File Offset: 0x001A4148
		internal override void CacheDefiningGeometry()
		{
			double num = base.GetStrokeThickness() / 2.0;
			this._rect = new Rect(num, num, 0.0, 0.0);
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x001A5185 File Offset: 0x001A4185
		internal override Size GetNaturalSize()
		{
			double strokeThickness = base.GetStrokeThickness();
			return new Size(strokeThickness, strokeThickness);
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x001A5193 File Offset: 0x001A4193
		internal override Rect GetDefiningGeometryBounds()
		{
			return this._rect;
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06002BD9 RID: 11225 RVA: 0x001A519B File Offset: 0x001A419B
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x04001AF7 RID: 6903
		private Rect _rect = Rect.Empty;
	}
}
