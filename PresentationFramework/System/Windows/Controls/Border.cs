using System;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x0200071E RID: 1822
	public class Border : Decorator
	{
		// Token: 0x06005FBB RID: 24507 RVA: 0x00296098 File Offset: 0x00295098
		static Border()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.Border);
		}

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x06005FBD RID: 24509 RVA: 0x00296226 File Offset: 0x00295226
		// (set) Token: 0x06005FBE RID: 24510 RVA: 0x00296238 File Offset: 0x00295238
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(Border.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(Border.BorderThicknessProperty, value);
			}
		}

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x06005FBF RID: 24511 RVA: 0x0029624B File Offset: 0x0029524B
		// (set) Token: 0x06005FC0 RID: 24512 RVA: 0x0029625D File Offset: 0x0029525D
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(Border.PaddingProperty);
			}
			set
			{
				base.SetValue(Border.PaddingProperty, value);
			}
		}

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x06005FC1 RID: 24513 RVA: 0x00296270 File Offset: 0x00295270
		// (set) Token: 0x06005FC2 RID: 24514 RVA: 0x00296282 File Offset: 0x00295282
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(Border.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(Border.CornerRadiusProperty, value);
			}
		}

		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x06005FC3 RID: 24515 RVA: 0x00296295 File Offset: 0x00295295
		// (set) Token: 0x06005FC4 RID: 24516 RVA: 0x002962A7 File Offset: 0x002952A7
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(Border.BorderBrushProperty);
			}
			set
			{
				base.SetValue(Border.BorderBrushProperty, value);
			}
		}

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x06005FC5 RID: 24517 RVA: 0x002962B5 File Offset: 0x002952B5
		// (set) Token: 0x06005FC6 RID: 24518 RVA: 0x002962C7 File Offset: 0x002952C7
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(Border.BackgroundProperty);
			}
			set
			{
				base.SetValue(Border.BackgroundProperty, value);
			}
		}

		// Token: 0x06005FC7 RID: 24519 RVA: 0x002962D5 File Offset: 0x002952D5
		private static void OnClearPenCache(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Border border = (Border)d;
			border.LeftPenCache = null;
			border.RightPenCache = null;
			border.TopPenCache = null;
			border.BottomPenCache = null;
		}

		// Token: 0x06005FC8 RID: 24520 RVA: 0x002962F8 File Offset: 0x002952F8
		private static bool IsThicknessValid(object value)
		{
			return ((Thickness)value).IsValid(false, false, false, false);
		}

		// Token: 0x06005FC9 RID: 24521 RVA: 0x00296318 File Offset: 0x00295318
		private static bool IsCornerRadiusValid(object value)
		{
			return ((CornerRadius)value).IsValid(false, false, false, false);
		}

		// Token: 0x06005FCA RID: 24522 RVA: 0x00296338 File Offset: 0x00295338
		protected override Size MeasureOverride(Size constraint)
		{
			UIElement child = this.Child;
			Size result = default(Size);
			Thickness borderThickness = this.BorderThickness;
			if (base.UseLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				DpiScale dpi = base.GetDpi();
				borderThickness = new Thickness(UIElement.RoundLayoutValue(borderThickness.Left, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Top, dpi.DpiScaleY), UIElement.RoundLayoutValue(borderThickness.Right, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Bottom, dpi.DpiScaleY));
			}
			Size size = Border.HelperCollapseThickness(borderThickness);
			Size size2 = Border.HelperCollapseThickness(this.Padding);
			if (child != null)
			{
				Size size3 = new Size(size.Width + size2.Width, size.Height + size2.Height);
				Size availableSize = new Size(Math.Max(0.0, constraint.Width - size3.Width), Math.Max(0.0, constraint.Height - size3.Height));
				child.Measure(availableSize);
				Size desiredSize = child.DesiredSize;
				result.Width = desiredSize.Width + size3.Width;
				result.Height = desiredSize.Height + size3.Height;
			}
			else
			{
				result = new Size(size.Width + size2.Width, size.Height + size2.Height);
			}
			return result;
		}

		// Token: 0x06005FCB RID: 24523 RVA: 0x002964AC File Offset: 0x002954AC
		protected override Size ArrangeOverride(Size finalSize)
		{
			Thickness borderThickness = this.BorderThickness;
			if (base.UseLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				DpiScale dpi = base.GetDpi();
				borderThickness = new Thickness(UIElement.RoundLayoutValue(borderThickness.Left, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Top, dpi.DpiScaleY), UIElement.RoundLayoutValue(borderThickness.Right, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Bottom, dpi.DpiScaleY));
			}
			Rect rect = new Rect(finalSize);
			Rect rect2 = Border.HelperDeflateRect(rect, borderThickness);
			UIElement child = this.Child;
			if (child != null)
			{
				Rect finalRect = Border.HelperDeflateRect(rect2, this.Padding);
				child.Arrange(finalRect);
			}
			CornerRadius cornerRadius = this.CornerRadius;
			Brush borderBrush = this.BorderBrush;
			bool flag = Border.AreUniformCorners(cornerRadius);
			this._useComplexRenderCodePath = !flag;
			if (!this._useComplexRenderCodePath && borderBrush != null)
			{
				SolidColorBrush solidColorBrush = borderBrush as SolidColorBrush;
				bool isUniform = borderThickness.IsUniform;
				this._useComplexRenderCodePath = (solidColorBrush == null || (solidColorBrush.Color.A < byte.MaxValue && !isUniform) || (!DoubleUtil.IsZero(cornerRadius.TopLeft) && !isUniform));
			}
			if (this._useComplexRenderCodePath)
			{
				Border.Radii radii = new Border.Radii(cornerRadius, borderThickness, false);
				StreamGeometry streamGeometry = null;
				if (!DoubleUtil.IsZero(rect2.Width) && !DoubleUtil.IsZero(rect2.Height))
				{
					streamGeometry = new StreamGeometry();
					using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
					{
						Border.GenerateGeometry(streamGeometryContext, rect2, radii);
					}
					streamGeometry.Freeze();
					this.BackgroundGeometryCache = streamGeometry;
				}
				else
				{
					this.BackgroundGeometryCache = null;
				}
				if (!DoubleUtil.IsZero(rect.Width) && !DoubleUtil.IsZero(rect.Height))
				{
					Border.Radii radii2 = new Border.Radii(cornerRadius, borderThickness, true);
					StreamGeometry streamGeometry2 = new StreamGeometry();
					using (StreamGeometryContext streamGeometryContext2 = streamGeometry2.Open())
					{
						Border.GenerateGeometry(streamGeometryContext2, rect, radii2);
						if (streamGeometry != null)
						{
							Border.GenerateGeometry(streamGeometryContext2, rect2, radii);
						}
					}
					streamGeometry2.Freeze();
					this.BorderGeometryCache = streamGeometry2;
				}
				else
				{
					this.BorderGeometryCache = null;
				}
			}
			else
			{
				this.BackgroundGeometryCache = null;
				this.BorderGeometryCache = null;
			}
			return finalSize;
		}

		// Token: 0x06005FCC RID: 24524 RVA: 0x002966F4 File Offset: 0x002956F4
		protected override void OnRender(DrawingContext dc)
		{
			bool useLayoutRounding = base.UseLayoutRounding;
			DpiScale dpi = base.GetDpi();
			if (this._useComplexRenderCodePath)
			{
				StreamGeometry borderGeometryCache = this.BorderGeometryCache;
				Brush brush;
				if (borderGeometryCache != null && (brush = this.BorderBrush) != null)
				{
					dc.DrawGeometry(brush, null, borderGeometryCache);
				}
				StreamGeometry backgroundGeometryCache = this.BackgroundGeometryCache;
				if (backgroundGeometryCache != null && (brush = this.Background) != null)
				{
					dc.DrawGeometry(brush, null, backgroundGeometryCache);
					return;
				}
			}
			else
			{
				Thickness borderThickness = this.BorderThickness;
				CornerRadius cornerRadius = this.CornerRadius;
				double topLeft = cornerRadius.TopLeft;
				bool flag = !DoubleUtil.IsZero(topLeft);
				Brush borderBrush;
				if (!borderThickness.IsZero && (borderBrush = this.BorderBrush) != null)
				{
					Pen pen = this.LeftPenCache;
					if (pen == null)
					{
						pen = new Pen();
						pen.Brush = borderBrush;
						if (useLayoutRounding)
						{
							pen.Thickness = UIElement.RoundLayoutValue(borderThickness.Left, dpi.DpiScaleX);
						}
						else
						{
							pen.Thickness = borderThickness.Left;
						}
						if (borderBrush.IsFrozen)
						{
							pen.Freeze();
						}
						this.LeftPenCache = pen;
					}
					if (borderThickness.IsUniform)
					{
						double num = pen.Thickness * 0.5;
						Rect rectangle = new Rect(new Point(num, num), new Point(base.RenderSize.Width - num, base.RenderSize.Height - num));
						if (flag)
						{
							dc.DrawRoundedRectangle(null, pen, rectangle, topLeft, topLeft);
						}
						else
						{
							dc.DrawRectangle(null, pen, rectangle);
						}
					}
					else
					{
						if (DoubleUtil.GreaterThan(borderThickness.Left, 0.0))
						{
							double num = pen.Thickness * 0.5;
							dc.DrawLine(pen, new Point(num, 0.0), new Point(num, base.RenderSize.Height));
						}
						if (DoubleUtil.GreaterThan(borderThickness.Right, 0.0))
						{
							pen = this.RightPenCache;
							if (pen == null)
							{
								pen = new Pen();
								pen.Brush = borderBrush;
								if (useLayoutRounding)
								{
									pen.Thickness = UIElement.RoundLayoutValue(borderThickness.Right, dpi.DpiScaleX);
								}
								else
								{
									pen.Thickness = borderThickness.Right;
								}
								if (borderBrush.IsFrozen)
								{
									pen.Freeze();
								}
								this.RightPenCache = pen;
							}
							double num = pen.Thickness * 0.5;
							dc.DrawLine(pen, new Point(base.RenderSize.Width - num, 0.0), new Point(base.RenderSize.Width - num, base.RenderSize.Height));
						}
						if (DoubleUtil.GreaterThan(borderThickness.Top, 0.0))
						{
							pen = this.TopPenCache;
							if (pen == null)
							{
								pen = new Pen();
								pen.Brush = borderBrush;
								if (useLayoutRounding)
								{
									pen.Thickness = UIElement.RoundLayoutValue(borderThickness.Top, dpi.DpiScaleY);
								}
								else
								{
									pen.Thickness = borderThickness.Top;
								}
								if (borderBrush.IsFrozen)
								{
									pen.Freeze();
								}
								this.TopPenCache = pen;
							}
							double num = pen.Thickness * 0.5;
							dc.DrawLine(pen, new Point(0.0, num), new Point(base.RenderSize.Width, num));
						}
						if (DoubleUtil.GreaterThan(borderThickness.Bottom, 0.0))
						{
							pen = this.BottomPenCache;
							if (pen == null)
							{
								pen = new Pen();
								pen.Brush = borderBrush;
								if (useLayoutRounding)
								{
									pen.Thickness = UIElement.RoundLayoutValue(borderThickness.Bottom, dpi.DpiScaleY);
								}
								else
								{
									pen.Thickness = borderThickness.Bottom;
								}
								if (borderBrush.IsFrozen)
								{
									pen.Freeze();
								}
								this.BottomPenCache = pen;
							}
							double num = pen.Thickness * 0.5;
							dc.DrawLine(pen, new Point(0.0, base.RenderSize.Height - num), new Point(base.RenderSize.Width, base.RenderSize.Height - num));
						}
					}
				}
				Brush background = this.Background;
				if (background != null)
				{
					Point point;
					Point point2;
					if (useLayoutRounding)
					{
						point = new Point(UIElement.RoundLayoutValue(borderThickness.Left, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Top, dpi.DpiScaleY));
						if (FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
						{
							point2 = new Point(UIElement.RoundLayoutValue(base.RenderSize.Width - borderThickness.Right, dpi.DpiScaleX), UIElement.RoundLayoutValue(base.RenderSize.Height - borderThickness.Bottom, dpi.DpiScaleY));
						}
						else
						{
							point2 = new Point(base.RenderSize.Width - UIElement.RoundLayoutValue(borderThickness.Right, dpi.DpiScaleX), base.RenderSize.Height - UIElement.RoundLayoutValue(borderThickness.Bottom, dpi.DpiScaleY));
						}
					}
					else
					{
						point = new Point(borderThickness.Left, borderThickness.Top);
						point2 = new Point(base.RenderSize.Width - borderThickness.Right, base.RenderSize.Height - borderThickness.Bottom);
					}
					if (point2.X > point.X && point2.Y > point.Y)
					{
						if (flag)
						{
							double topLeft2 = new Border.Radii(cornerRadius, borderThickness, false).TopLeft;
							dc.DrawRoundedRectangle(background, null, new Rect(point, point2), topLeft2, topLeft2);
							return;
						}
						dc.DrawRectangle(background, null, new Rect(point, point2));
					}
				}
			}
		}

		// Token: 0x06005FCD RID: 24525 RVA: 0x00296CD6 File Offset: 0x00295CD6
		private static Size HelperCollapseThickness(Thickness th)
		{
			return new Size(th.Left + th.Right, th.Top + th.Bottom);
		}

		// Token: 0x06005FCE RID: 24526 RVA: 0x00296CFC File Offset: 0x00295CFC
		private static bool AreUniformCorners(CornerRadius borderRadii)
		{
			double topLeft = borderRadii.TopLeft;
			return DoubleUtil.AreClose(topLeft, borderRadii.TopRight) && DoubleUtil.AreClose(topLeft, borderRadii.BottomLeft) && DoubleUtil.AreClose(topLeft, borderRadii.BottomRight);
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x00296D40 File Offset: 0x00295D40
		private static Rect HelperDeflateRect(Rect rt, Thickness thick)
		{
			return new Rect(rt.Left + thick.Left, rt.Top + thick.Top, Math.Max(0.0, rt.Width - thick.Left - thick.Right), Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));
		}

		// Token: 0x06005FD0 RID: 24528 RVA: 0x00296DBC File Offset: 0x00295DBC
		private static void GenerateGeometry(StreamGeometryContext ctx, Rect rect, Border.Radii radii)
		{
			Point point = new Point(radii.LeftTop, 0.0);
			Point point2 = new Point(rect.Width - radii.RightTop, 0.0);
			Point point3 = new Point(rect.Width, radii.TopRight);
			Point point4 = new Point(rect.Width, rect.Height - radii.BottomRight);
			Point point5 = new Point(rect.Width - radii.RightBottom, rect.Height);
			Point point6 = new Point(radii.LeftBottom, rect.Height);
			Point point7 = new Point(0.0, rect.Height - radii.BottomLeft);
			Point point8 = new Point(0.0, radii.TopLeft);
			if (point.X > point2.X)
			{
				double x = radii.LeftTop / (radii.LeftTop + radii.RightTop) * rect.Width;
				point.X = x;
				point2.X = x;
			}
			if (point3.Y > point4.Y)
			{
				double y = radii.TopRight / (radii.TopRight + radii.BottomRight) * rect.Height;
				point3.Y = y;
				point4.Y = y;
			}
			if (point5.X < point6.X)
			{
				double x2 = radii.LeftBottom / (radii.LeftBottom + radii.RightBottom) * rect.Width;
				point5.X = x2;
				point6.X = x2;
			}
			if (point7.Y < point8.Y)
			{
				double y2 = radii.TopLeft / (radii.TopLeft + radii.BottomLeft) * rect.Height;
				point7.Y = y2;
				point8.Y = y2;
			}
			Vector vector = new Vector(rect.TopLeft.X, rect.TopLeft.Y);
			point += vector;
			point2 += vector;
			point3 += vector;
			point4 += vector;
			point5 += vector;
			point6 += vector;
			point7 += vector;
			point8 += vector;
			ctx.BeginFigure(point, true, true);
			ctx.LineTo(point2, true, false);
			double num = rect.TopRight.X - point2.X;
			double num2 = point3.Y - rect.TopRight.Y;
			if (!DoubleUtil.IsZero(num) || !DoubleUtil.IsZero(num2))
			{
				ctx.ArcTo(point3, new Size(num, num2), 0.0, false, SweepDirection.Clockwise, true, false);
			}
			ctx.LineTo(point4, true, false);
			num = rect.BottomRight.X - point5.X;
			num2 = rect.BottomRight.Y - point4.Y;
			if (!DoubleUtil.IsZero(num) || !DoubleUtil.IsZero(num2))
			{
				ctx.ArcTo(point5, new Size(num, num2), 0.0, false, SweepDirection.Clockwise, true, false);
			}
			ctx.LineTo(point6, true, false);
			num = point6.X - rect.BottomLeft.X;
			num2 = rect.BottomLeft.Y - point7.Y;
			if (!DoubleUtil.IsZero(num) || !DoubleUtil.IsZero(num2))
			{
				ctx.ArcTo(point7, new Size(num, num2), 0.0, false, SweepDirection.Clockwise, true, false);
			}
			ctx.LineTo(point8, true, false);
			num = point.X - rect.TopLeft.X;
			num2 = point8.Y - rect.TopLeft.Y;
			if (!DoubleUtil.IsZero(num) || !DoubleUtil.IsZero(num2))
			{
				ctx.ArcTo(point, new Size(num, num2), 0.0, false, SweepDirection.Clockwise, true, false);
			}
		}

		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x06005FD1 RID: 24529 RVA: 0x001FCA9D File Offset: 0x001FBA9D
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x06005FD2 RID: 24530 RVA: 0x002971C5 File Offset: 0x002961C5
		// (set) Token: 0x06005FD3 RID: 24531 RVA: 0x002971D2 File Offset: 0x002961D2
		private StreamGeometry BorderGeometryCache
		{
			get
			{
				return Border.BorderGeometryField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.BorderGeometryField.ClearValue(this);
					return;
				}
				Border.BorderGeometryField.SetValue(this, value);
			}
		}

		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x06005FD4 RID: 24532 RVA: 0x002971EF File Offset: 0x002961EF
		// (set) Token: 0x06005FD5 RID: 24533 RVA: 0x002971FC File Offset: 0x002961FC
		private StreamGeometry BackgroundGeometryCache
		{
			get
			{
				return Border.BackgroundGeometryField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.BackgroundGeometryField.ClearValue(this);
					return;
				}
				Border.BackgroundGeometryField.SetValue(this, value);
			}
		}

		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x06005FD6 RID: 24534 RVA: 0x00297219 File Offset: 0x00296219
		// (set) Token: 0x06005FD7 RID: 24535 RVA: 0x00297226 File Offset: 0x00296226
		private Pen LeftPenCache
		{
			get
			{
				return Border.LeftPenField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.LeftPenField.ClearValue(this);
					return;
				}
				Border.LeftPenField.SetValue(this, value);
			}
		}

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x06005FD8 RID: 24536 RVA: 0x00297243 File Offset: 0x00296243
		// (set) Token: 0x06005FD9 RID: 24537 RVA: 0x00297250 File Offset: 0x00296250
		private Pen RightPenCache
		{
			get
			{
				return Border.RightPenField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.RightPenField.ClearValue(this);
					return;
				}
				Border.RightPenField.SetValue(this, value);
			}
		}

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x06005FDA RID: 24538 RVA: 0x0029726D File Offset: 0x0029626D
		// (set) Token: 0x06005FDB RID: 24539 RVA: 0x0029727A File Offset: 0x0029627A
		private Pen TopPenCache
		{
			get
			{
				return Border.TopPenField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.TopPenField.ClearValue(this);
					return;
				}
				Border.TopPenField.SetValue(this, value);
			}
		}

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x06005FDC RID: 24540 RVA: 0x00297297 File Offset: 0x00296297
		// (set) Token: 0x06005FDD RID: 24541 RVA: 0x002972A4 File Offset: 0x002962A4
		private Pen BottomPenCache
		{
			get
			{
				return Border.BottomPenField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.BottomPenField.ClearValue(this);
					return;
				}
				Border.BottomPenField.SetValue(this, value);
			}
		}

		// Token: 0x040031EA RID: 12778
		[CommonDependencyProperty]
		public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Border), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Border.OnClearPenCache)), new ValidateValueCallback(Border.IsThicknessValid));

		// Token: 0x040031EB RID: 12779
		public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Border), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Border.IsThicknessValid));

		// Token: 0x040031EC RID: 12780
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Border), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Border.IsCornerRadiusValid));

		// Token: 0x040031ED RID: 12781
		[CommonDependencyProperty]
		public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Border), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, new PropertyChangedCallback(Border.OnClearPenCache)));

		// Token: 0x040031EE RID: 12782
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(Border), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		// Token: 0x040031EF RID: 12783
		private bool _useComplexRenderCodePath;

		// Token: 0x040031F0 RID: 12784
		private static readonly UncommonField<StreamGeometry> BorderGeometryField = new UncommonField<StreamGeometry>();

		// Token: 0x040031F1 RID: 12785
		private static readonly UncommonField<StreamGeometry> BackgroundGeometryField = new UncommonField<StreamGeometry>();

		// Token: 0x040031F2 RID: 12786
		private static readonly UncommonField<Pen> LeftPenField = new UncommonField<Pen>();

		// Token: 0x040031F3 RID: 12787
		private static readonly UncommonField<Pen> RightPenField = new UncommonField<Pen>();

		// Token: 0x040031F4 RID: 12788
		private static readonly UncommonField<Pen> TopPenField = new UncommonField<Pen>();

		// Token: 0x040031F5 RID: 12789
		private static readonly UncommonField<Pen> BottomPenField = new UncommonField<Pen>();

		// Token: 0x02000BC0 RID: 3008
		private struct Radii
		{
			// Token: 0x06008F46 RID: 36678 RVA: 0x00343A64 File Offset: 0x00342A64
			internal Radii(CornerRadius radii, Thickness borders, bool outer)
			{
				double num = 0.5 * borders.Left;
				double num2 = 0.5 * borders.Top;
				double num3 = 0.5 * borders.Right;
				double num4 = 0.5 * borders.Bottom;
				if (!outer)
				{
					this.LeftTop = Math.Max(0.0, radii.TopLeft - num);
					this.TopLeft = Math.Max(0.0, radii.TopLeft - num2);
					this.TopRight = Math.Max(0.0, radii.TopRight - num2);
					this.RightTop = Math.Max(0.0, radii.TopRight - num3);
					this.RightBottom = Math.Max(0.0, radii.BottomRight - num3);
					this.BottomRight = Math.Max(0.0, radii.BottomRight - num4);
					this.BottomLeft = Math.Max(0.0, radii.BottomLeft - num4);
					this.LeftBottom = Math.Max(0.0, radii.BottomLeft - num);
					return;
				}
				if (DoubleUtil.IsZero(radii.TopLeft))
				{
					this.LeftTop = (this.TopLeft = 0.0);
				}
				else
				{
					this.LeftTop = radii.TopLeft + num;
					this.TopLeft = radii.TopLeft + num2;
				}
				if (DoubleUtil.IsZero(radii.TopRight))
				{
					this.TopRight = (this.RightTop = 0.0);
				}
				else
				{
					this.TopRight = radii.TopRight + num2;
					this.RightTop = radii.TopRight + num3;
				}
				if (DoubleUtil.IsZero(radii.BottomRight))
				{
					this.RightBottom = (this.BottomRight = 0.0);
				}
				else
				{
					this.RightBottom = radii.BottomRight + num3;
					this.BottomRight = radii.BottomRight + num4;
				}
				if (DoubleUtil.IsZero(radii.BottomLeft))
				{
					this.BottomLeft = (this.LeftBottom = 0.0);
					return;
				}
				this.BottomLeft = radii.BottomLeft + num4;
				this.LeftBottom = radii.BottomLeft + num;
			}

			// Token: 0x040049C5 RID: 18885
			internal double LeftTop;

			// Token: 0x040049C6 RID: 18886
			internal double TopLeft;

			// Token: 0x040049C7 RID: 18887
			internal double TopRight;

			// Token: 0x040049C8 RID: 18888
			internal double RightTop;

			// Token: 0x040049C9 RID: 18889
			internal double RightBottom;

			// Token: 0x040049CA RID: 18890
			internal double BottomRight;

			// Token: 0x040049CB RID: 18891
			internal double BottomLeft;

			// Token: 0x040049CC RID: 18892
			internal double LeftBottom;
		}
	}
}
