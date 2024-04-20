using System;
using System.ComponentModel;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.PresentationFramework;

namespace System.Windows.Shapes
{
	// Token: 0x020003FE RID: 1022
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public abstract class Shape : FrameworkElement
	{
		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06002C0D RID: 11277 RVA: 0x001A5A18 File Offset: 0x001A4A18
		// (set) Token: 0x06002C0E RID: 11278 RVA: 0x001A5A2A File Offset: 0x001A4A2A
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(Shape.StretchProperty);
			}
			set
			{
				base.SetValue(Shape.StretchProperty, value);
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06002C0F RID: 11279 RVA: 0x001A5A40 File Offset: 0x001A4A40
		public virtual Geometry RenderedGeometry
		{
			get
			{
				this.EnsureRenderedGeometry();
				Geometry geometry = this._renderedGeometry.CloneCurrentValue();
				if (geometry == null || geometry == Geometry.Empty)
				{
					return Geometry.Empty;
				}
				if (geometry == this._renderedGeometry)
				{
					geometry = geometry.Clone();
					geometry.Freeze();
				}
				return geometry;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06002C10 RID: 11280 RVA: 0x001A5A88 File Offset: 0x001A4A88
		public virtual Transform GeometryTransform
		{
			get
			{
				BoxedMatrix value = Shape.StretchMatrixField.GetValue(this);
				if (value == null)
				{
					return Transform.Identity;
				}
				return new MatrixTransform(value.Value);
			}
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x001A5AB5 File Offset: 0x001A4AB5
		private static void OnPenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Shape)d)._pen = null;
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06002C12 RID: 11282 RVA: 0x001A5AC3 File Offset: 0x001A4AC3
		// (set) Token: 0x06002C13 RID: 11283 RVA: 0x001A5AD5 File Offset: 0x001A4AD5
		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(Shape.FillProperty);
			}
			set
			{
				base.SetValue(Shape.FillProperty, value);
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06002C14 RID: 11284 RVA: 0x001A5AE3 File Offset: 0x001A4AE3
		// (set) Token: 0x06002C15 RID: 11285 RVA: 0x001A5AF5 File Offset: 0x001A4AF5
		public Brush Stroke
		{
			get
			{
				return (Brush)base.GetValue(Shape.StrokeProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeProperty, value);
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06002C16 RID: 11286 RVA: 0x001A5B03 File Offset: 0x001A4B03
		// (set) Token: 0x06002C17 RID: 11287 RVA: 0x001A5B15 File Offset: 0x001A4B15
		[TypeConverter(typeof(LengthConverter))]
		public double StrokeThickness
		{
			get
			{
				return (double)base.GetValue(Shape.StrokeThicknessProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeThicknessProperty, value);
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06002C18 RID: 11288 RVA: 0x001A5B28 File Offset: 0x001A4B28
		// (set) Token: 0x06002C19 RID: 11289 RVA: 0x001A5B3A File Offset: 0x001A4B3A
		public PenLineCap StrokeStartLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(Shape.StrokeStartLineCapProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeStartLineCapProperty, value);
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06002C1A RID: 11290 RVA: 0x001A5B4D File Offset: 0x001A4B4D
		// (set) Token: 0x06002C1B RID: 11291 RVA: 0x001A5B5F File Offset: 0x001A4B5F
		public PenLineCap StrokeEndLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(Shape.StrokeEndLineCapProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeEndLineCapProperty, value);
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06002C1C RID: 11292 RVA: 0x001A5B72 File Offset: 0x001A4B72
		// (set) Token: 0x06002C1D RID: 11293 RVA: 0x001A5B84 File Offset: 0x001A4B84
		public PenLineCap StrokeDashCap
		{
			get
			{
				return (PenLineCap)base.GetValue(Shape.StrokeDashCapProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeDashCapProperty, value);
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06002C1E RID: 11294 RVA: 0x001A5B97 File Offset: 0x001A4B97
		// (set) Token: 0x06002C1F RID: 11295 RVA: 0x001A5BA9 File Offset: 0x001A4BA9
		public PenLineJoin StrokeLineJoin
		{
			get
			{
				return (PenLineJoin)base.GetValue(Shape.StrokeLineJoinProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeLineJoinProperty, value);
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06002C20 RID: 11296 RVA: 0x001A5BBC File Offset: 0x001A4BBC
		// (set) Token: 0x06002C21 RID: 11297 RVA: 0x001A5BCE File Offset: 0x001A4BCE
		public double StrokeMiterLimit
		{
			get
			{
				return (double)base.GetValue(Shape.StrokeMiterLimitProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeMiterLimitProperty, value);
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06002C22 RID: 11298 RVA: 0x001A5BE1 File Offset: 0x001A4BE1
		// (set) Token: 0x06002C23 RID: 11299 RVA: 0x001A5BF3 File Offset: 0x001A4BF3
		public double StrokeDashOffset
		{
			get
			{
				return (double)base.GetValue(Shape.StrokeDashOffsetProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeDashOffsetProperty, value);
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06002C24 RID: 11300 RVA: 0x001A5C06 File Offset: 0x001A4C06
		// (set) Token: 0x06002C25 RID: 11301 RVA: 0x001A5C18 File Offset: 0x001A4C18
		public DoubleCollection StrokeDashArray
		{
			get
			{
				return (DoubleCollection)base.GetValue(Shape.StrokeDashArrayProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeDashArrayProperty, value);
			}
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x001A5C28 File Offset: 0x001A4C28
		protected override Size MeasureOverride(Size constraint)
		{
			this.CacheDefiningGeometry();
			Stretch stretch = this.Stretch;
			Size size;
			if (stretch == Stretch.None)
			{
				size = this.GetNaturalSize();
			}
			else
			{
				size = this.GetStretchedRenderSize(stretch, this.GetStrokeThickness(), constraint, this.GetDefiningGeometryBounds());
			}
			if (this.SizeIsInvalidOrEmpty(size))
			{
				size = new Size(0.0, 0.0);
				this._renderedGeometry = Geometry.Empty;
			}
			return size;
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x001A5C94 File Offset: 0x001A4C94
		protected override Size ArrangeOverride(Size finalSize)
		{
			Stretch stretch = this.Stretch;
			Size size;
			if (stretch == Stretch.None)
			{
				Shape.StretchMatrixField.ClearValue(this);
				this.ResetRenderedGeometry();
				size = finalSize;
			}
			else
			{
				size = this.GetStretchedRenderSizeAndSetStretchMatrix(stretch, this.GetStrokeThickness(), finalSize, this.GetDefiningGeometryBounds());
			}
			if (this.SizeIsInvalidOrEmpty(size))
			{
				size = new Size(0.0, 0.0);
				this._renderedGeometry = Geometry.Empty;
			}
			return size;
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x001A5D03 File Offset: 0x001A4D03
		protected override void OnRender(DrawingContext drawingContext)
		{
			this.EnsureRenderedGeometry();
			if (this._renderedGeometry != Geometry.Empty)
			{
				drawingContext.DrawGeometry(this.Fill, this.GetPen(), this._renderedGeometry);
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06002C29 RID: 11305
		protected abstract Geometry DefiningGeometry { get; }

		// Token: 0x06002C2A RID: 11306 RVA: 0x001A5D30 File Offset: 0x001A4D30
		internal bool SizeIsInvalidOrEmpty(Size size)
		{
			return DoubleUtil.IsNaN(size.Width) || DoubleUtil.IsNaN(size.Height) || size.IsEmpty;
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06002C2B RID: 11307 RVA: 0x001A5D58 File Offset: 0x001A4D58
		internal bool IsPenNoOp
		{
			get
			{
				double strokeThickness = this.StrokeThickness;
				return this.Stroke == null || DoubleUtil.IsNaN(strokeThickness) || DoubleUtil.IsZero(strokeThickness);
			}
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x001A5D84 File Offset: 0x001A4D84
		internal double GetStrokeThickness()
		{
			if (this.IsPenNoOp)
			{
				return 0.0;
			}
			return Math.Abs(this.StrokeThickness);
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x001A5DA4 File Offset: 0x001A4DA4
		internal Pen GetPen()
		{
			if (this.IsPenNoOp)
			{
				return null;
			}
			if (this._pen == null)
			{
				double thickness = Math.Abs(this.StrokeThickness);
				this._pen = new Pen();
				this._pen.CanBeInheritanceContext = false;
				this._pen.Thickness = thickness;
				this._pen.Brush = this.Stroke;
				this._pen.StartLineCap = this.StrokeStartLineCap;
				this._pen.EndLineCap = this.StrokeEndLineCap;
				this._pen.DashCap = this.StrokeDashCap;
				this._pen.LineJoin = this.StrokeLineJoin;
				this._pen.MiterLimit = this.StrokeMiterLimit;
				DoubleCollection doubleCollection = null;
				bool flag;
				if (base.GetValueSource(Shape.StrokeDashArrayProperty, null, out flag) != BaseValueSourceInternal.Default || flag)
				{
					doubleCollection = this.StrokeDashArray;
				}
				double strokeDashOffset = this.StrokeDashOffset;
				if (doubleCollection != null || strokeDashOffset != 0.0)
				{
					this._pen.DashStyle = new DashStyle(doubleCollection, strokeDashOffset);
				}
			}
			return this._pen;
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x001A5EB4 File Offset: 0x001A4EB4
		internal static bool IsDoubleFiniteNonNegative(object o)
		{
			double num = (double)o;
			return !double.IsInfinity(num) && !DoubleUtil.IsNaN(num) && num >= 0.0;
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x001A5EEC File Offset: 0x001A4EEC
		internal static bool IsDoubleFinite(object o)
		{
			double num = (double)o;
			return !double.IsInfinity(num) && !DoubleUtil.IsNaN(num);
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x001A5F13 File Offset: 0x001A4F13
		internal static bool IsDoubleFiniteOrNaN(object o)
		{
			return !double.IsInfinity((double)o);
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void CacheDefiningGeometry()
		{
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x001A5F24 File Offset: 0x001A4F24
		internal Size GetStretchedRenderSize(Stretch mode, double strokeThickness, Size availableSize, Rect geometryBounds)
		{
			double num;
			double num2;
			double num3;
			double num4;
			Size result;
			this.GetStretchMetrics(mode, strokeThickness, availableSize, geometryBounds, out num, out num2, out num3, out num4, out result);
			return result;
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x001A5F48 File Offset: 0x001A4F48
		internal Size GetStretchedRenderSizeAndSetStretchMatrix(Stretch mode, double strokeThickness, Size availableSize, Rect geometryBounds)
		{
			double scaleX;
			double scaleY;
			double offsetX;
			double offsetY;
			Size result;
			this.GetStretchMetrics(mode, strokeThickness, availableSize, geometryBounds, out scaleX, out scaleY, out offsetX, out offsetY, out result);
			Matrix identity = Matrix.Identity;
			identity.ScaleAt(scaleX, scaleY, geometryBounds.Location.X, geometryBounds.Location.Y);
			identity.Translate(offsetX, offsetY);
			Shape.StretchMatrixField.SetValue(this, new BoxedMatrix(identity));
			this.ResetRenderedGeometry();
			return result;
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x001A5FBD File Offset: 0x001A4FBD
		internal void ResetRenderedGeometry()
		{
			this._renderedGeometry = null;
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x001A5FC8 File Offset: 0x001A4FC8
		internal void GetStretchMetrics(Stretch mode, double strokeThickness, Size availableSize, Rect geometryBounds, out double xScale, out double yScale, out double dX, out double dY, out Size stretchedSize)
		{
			if (!geometryBounds.IsEmpty)
			{
				double num = strokeThickness / 2.0;
				bool flag = false;
				xScale = Math.Max(availableSize.Width - strokeThickness, 0.0);
				yScale = Math.Max(availableSize.Height - strokeThickness, 0.0);
				dX = num - geometryBounds.Left;
				dY = num - geometryBounds.Top;
				if (geometryBounds.Width > xScale * 5E-324)
				{
					xScale /= geometryBounds.Width;
				}
				else
				{
					xScale = 1.0;
					if (geometryBounds.Width == 0.0)
					{
						flag = true;
					}
				}
				if (geometryBounds.Height > yScale * 5E-324)
				{
					yScale /= geometryBounds.Height;
				}
				else
				{
					yScale = 1.0;
					if (geometryBounds.Height == 0.0)
					{
						flag = true;
					}
				}
				if (mode != Stretch.Fill && !flag)
				{
					if (mode == Stretch.Uniform)
					{
						if (yScale > xScale)
						{
							yScale = xScale;
						}
						else
						{
							xScale = yScale;
						}
					}
					else if (xScale > yScale)
					{
						yScale = xScale;
					}
					else
					{
						xScale = yScale;
					}
				}
				stretchedSize = new Size(geometryBounds.Width * xScale + strokeThickness, geometryBounds.Height * yScale + strokeThickness);
				return;
			}
			xScale = (yScale = 1.0);
			dX = (dY = 0.0);
			stretchedSize = new Size(0.0, 0.0);
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x001A616C File Offset: 0x001A516C
		internal virtual Size GetNaturalSize()
		{
			Geometry definingGeometry = this.DefiningGeometry;
			Pen pen = this.GetPen();
			DashStyle dashStyle = null;
			if (pen != null)
			{
				dashStyle = pen.DashStyle;
				if (dashStyle != null)
				{
					pen.DashStyle = null;
				}
			}
			Rect renderBounds = definingGeometry.GetRenderBounds(pen);
			if (dashStyle != null)
			{
				pen.DashStyle = dashStyle;
			}
			return new Size(Math.Max(renderBounds.Right, 0.0), Math.Max(renderBounds.Bottom, 0.0));
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x001A61DC File Offset: 0x001A51DC
		internal virtual Rect GetDefiningGeometryBounds()
		{
			return this.DefiningGeometry.Bounds;
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x001A61EC File Offset: 0x001A51EC
		internal void EnsureRenderedGeometry()
		{
			if (this._renderedGeometry == null)
			{
				this._renderedGeometry = this.DefiningGeometry;
				if (this.Stretch != Stretch.None)
				{
					Geometry geometry = this._renderedGeometry.CloneCurrentValue();
					if (this._renderedGeometry == geometry)
					{
						this._renderedGeometry = geometry.Clone();
					}
					else
					{
						this._renderedGeometry = geometry;
					}
					Transform transform = this._renderedGeometry.Transform;
					BoxedMatrix value = Shape.StretchMatrixField.GetValue(this);
					Matrix matrix = (value == null) ? Matrix.Identity : value.Value;
					if (transform == null || transform.IsIdentity)
					{
						this._renderedGeometry.Transform = new MatrixTransform(matrix);
						return;
					}
					this._renderedGeometry.Transform = new MatrixTransform(transform.Value * matrix);
				}
			}
		}

		// Token: 0x04001B07 RID: 6919
		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(Shape), new FrameworkPropertyMetadata(Stretch.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

		// Token: 0x04001B08 RID: 6920
		[CommonDependencyProperty]
		public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(Shape), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		// Token: 0x04001B09 RID: 6921
		[CommonDependencyProperty]
		public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(Shape), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		// Token: 0x04001B0A RID: 6922
		[CommonDependencyProperty]
		public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Shape), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		// Token: 0x04001B0B RID: 6923
		public static readonly DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(Shape), new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)), new ValidateValueCallback(ValidateEnums.IsPenLineCapValid));

		// Token: 0x04001B0C RID: 6924
		public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(Shape), new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)), new ValidateValueCallback(ValidateEnums.IsPenLineCapValid));

		// Token: 0x04001B0D RID: 6925
		public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(Shape), new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)), new ValidateValueCallback(ValidateEnums.IsPenLineCapValid));

		// Token: 0x04001B0E RID: 6926
		public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(Shape), new FrameworkPropertyMetadata(PenLineJoin.Miter, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)), new ValidateValueCallback(ValidateEnums.IsPenLineJoinValid));

		// Token: 0x04001B0F RID: 6927
		public static readonly DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(Shape), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		// Token: 0x04001B10 RID: 6928
		public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(Shape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		// Token: 0x04001B11 RID: 6929
		public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(Shape), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(DoubleCollection.Empty), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		// Token: 0x04001B12 RID: 6930
		private Pen _pen;

		// Token: 0x04001B13 RID: 6931
		private Geometry _renderedGeometry = Geometry.Empty;

		// Token: 0x04001B14 RID: 6932
		private static UncommonField<BoxedMatrix> StretchMatrixField = new UncommonField<BoxedMatrix>(null);
	}
}
