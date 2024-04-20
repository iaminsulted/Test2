using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200085C RID: 2140
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class TickBar : FrameworkElement
	{
		// Token: 0x06007E33 RID: 32307 RVA: 0x00316EEC File Offset: 0x00315EEC
		static TickBar()
		{
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(TickBar), new FrameworkPropertyMetadata(true));
		}

		// Token: 0x17001D1B RID: 7451
		// (get) Token: 0x06007E35 RID: 32309 RVA: 0x00317113 File Offset: 0x00316113
		// (set) Token: 0x06007E36 RID: 32310 RVA: 0x00317125 File Offset: 0x00316125
		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(TickBar.FillProperty);
			}
			set
			{
				base.SetValue(TickBar.FillProperty, value);
			}
		}

		// Token: 0x17001D1C RID: 7452
		// (get) Token: 0x06007E37 RID: 32311 RVA: 0x00317133 File Offset: 0x00316133
		// (set) Token: 0x06007E38 RID: 32312 RVA: 0x00317145 File Offset: 0x00316145
		[Bindable(true)]
		[Category("Appearance")]
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(TickBar.MinimumProperty);
			}
			set
			{
				base.SetValue(TickBar.MinimumProperty, value);
			}
		}

		// Token: 0x17001D1D RID: 7453
		// (get) Token: 0x06007E39 RID: 32313 RVA: 0x00317158 File Offset: 0x00316158
		// (set) Token: 0x06007E3A RID: 32314 RVA: 0x0031716A File Offset: 0x0031616A
		[Bindable(true)]
		[Category("Appearance")]
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(TickBar.MaximumProperty);
			}
			set
			{
				base.SetValue(TickBar.MaximumProperty, value);
			}
		}

		// Token: 0x17001D1E RID: 7454
		// (get) Token: 0x06007E3B RID: 32315 RVA: 0x0031717D File Offset: 0x0031617D
		// (set) Token: 0x06007E3C RID: 32316 RVA: 0x0031718F File Offset: 0x0031618F
		[Bindable(true)]
		[Category("Appearance")]
		public double SelectionStart
		{
			get
			{
				return (double)base.GetValue(TickBar.SelectionStartProperty);
			}
			set
			{
				base.SetValue(TickBar.SelectionStartProperty, value);
			}
		}

		// Token: 0x17001D1F RID: 7455
		// (get) Token: 0x06007E3D RID: 32317 RVA: 0x003171A2 File Offset: 0x003161A2
		// (set) Token: 0x06007E3E RID: 32318 RVA: 0x003171B4 File Offset: 0x003161B4
		[Category("Appearance")]
		[Bindable(true)]
		public double SelectionEnd
		{
			get
			{
				return (double)base.GetValue(TickBar.SelectionEndProperty);
			}
			set
			{
				base.SetValue(TickBar.SelectionEndProperty, value);
			}
		}

		// Token: 0x17001D20 RID: 7456
		// (get) Token: 0x06007E3F RID: 32319 RVA: 0x003171C7 File Offset: 0x003161C7
		// (set) Token: 0x06007E40 RID: 32320 RVA: 0x003171D9 File Offset: 0x003161D9
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelectionRangeEnabled
		{
			get
			{
				return (bool)base.GetValue(TickBar.IsSelectionRangeEnabledProperty);
			}
			set
			{
				base.SetValue(TickBar.IsSelectionRangeEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001D21 RID: 7457
		// (get) Token: 0x06007E41 RID: 32321 RVA: 0x003171EC File Offset: 0x003161EC
		// (set) Token: 0x06007E42 RID: 32322 RVA: 0x003171FE File Offset: 0x003161FE
		[Bindable(true)]
		[Category("Appearance")]
		public double TickFrequency
		{
			get
			{
				return (double)base.GetValue(TickBar.TickFrequencyProperty);
			}
			set
			{
				base.SetValue(TickBar.TickFrequencyProperty, value);
			}
		}

		// Token: 0x17001D22 RID: 7458
		// (get) Token: 0x06007E43 RID: 32323 RVA: 0x00317211 File Offset: 0x00316211
		// (set) Token: 0x06007E44 RID: 32324 RVA: 0x00317223 File Offset: 0x00316223
		[Bindable(true)]
		[Category("Appearance")]
		public DoubleCollection Ticks
		{
			get
			{
				return (DoubleCollection)base.GetValue(TickBar.TicksProperty);
			}
			set
			{
				base.SetValue(TickBar.TicksProperty, value);
			}
		}

		// Token: 0x17001D23 RID: 7459
		// (get) Token: 0x06007E45 RID: 32325 RVA: 0x00317231 File Offset: 0x00316231
		// (set) Token: 0x06007E46 RID: 32326 RVA: 0x00317243 File Offset: 0x00316243
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsDirectionReversed
		{
			get
			{
				return (bool)base.GetValue(TickBar.IsDirectionReversedProperty);
			}
			set
			{
				base.SetValue(TickBar.IsDirectionReversedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001D24 RID: 7460
		// (get) Token: 0x06007E47 RID: 32327 RVA: 0x00317256 File Offset: 0x00316256
		// (set) Token: 0x06007E48 RID: 32328 RVA: 0x00317268 File Offset: 0x00316268
		[Bindable(true)]
		[Category("Appearance")]
		public TickBarPlacement Placement
		{
			get
			{
				return (TickBarPlacement)base.GetValue(TickBar.PlacementProperty);
			}
			set
			{
				base.SetValue(TickBar.PlacementProperty, value);
			}
		}

		// Token: 0x06007E49 RID: 32329 RVA: 0x0031727C File Offset: 0x0031627C
		private static bool IsValidTickBarPlacement(object o)
		{
			TickBarPlacement tickBarPlacement = (TickBarPlacement)o;
			return tickBarPlacement == TickBarPlacement.Left || tickBarPlacement == TickBarPlacement.Top || tickBarPlacement == TickBarPlacement.Right || tickBarPlacement == TickBarPlacement.Bottom;
		}

		// Token: 0x17001D25 RID: 7461
		// (get) Token: 0x06007E4A RID: 32330 RVA: 0x003172A1 File Offset: 0x003162A1
		// (set) Token: 0x06007E4B RID: 32331 RVA: 0x003172B3 File Offset: 0x003162B3
		[Category("Appearance")]
		[Bindable(true)]
		public double ReservedSpace
		{
			get
			{
				return (double)base.GetValue(TickBar.ReservedSpaceProperty);
			}
			set
			{
				base.SetValue(TickBar.ReservedSpaceProperty, value);
			}
		}

		// Token: 0x06007E4C RID: 32332 RVA: 0x003172C8 File Offset: 0x003162C8
		protected override void OnRender(DrawingContext dc)
		{
			Size size = new Size(base.ActualWidth, base.ActualHeight);
			double num = this.Maximum - this.Minimum;
			double num2 = 0.0;
			double num3 = 1.0;
			double num4 = 1.0;
			Point point = new Point(0.0, 0.0);
			Point point2 = new Point(0.0, 0.0);
			double num5 = this.ReservedSpace * 0.5;
			switch (this.Placement)
			{
			case TickBarPlacement.Left:
				if (DoubleUtil.GreaterThanOrClose(this.ReservedSpace, size.Height))
				{
					return;
				}
				size.Height -= this.ReservedSpace;
				num2 = -size.Width;
				point = new Point(size.Width, size.Height + num5);
				point2 = new Point(size.Width, num5);
				num3 = size.Height / num * -1.0;
				num4 = -1.0;
				break;
			case TickBarPlacement.Top:
				if (DoubleUtil.GreaterThanOrClose(this.ReservedSpace, size.Width))
				{
					return;
				}
				size.Width -= this.ReservedSpace;
				num2 = -size.Height;
				point = new Point(num5, size.Height);
				point2 = new Point(num5 + size.Width, size.Height);
				num3 = size.Width / num;
				num4 = 1.0;
				break;
			case TickBarPlacement.Right:
				if (DoubleUtil.GreaterThanOrClose(this.ReservedSpace, size.Height))
				{
					return;
				}
				size.Height -= this.ReservedSpace;
				num2 = size.Width;
				point = new Point(0.0, size.Height + num5);
				point2 = new Point(0.0, num5);
				num3 = size.Height / num * -1.0;
				num4 = -1.0;
				break;
			case TickBarPlacement.Bottom:
				if (DoubleUtil.GreaterThanOrClose(this.ReservedSpace, size.Width))
				{
					return;
				}
				size.Width -= this.ReservedSpace;
				num2 = size.Height;
				point = new Point(num5, 0.0);
				point2 = new Point(num5 + size.Width, 0.0);
				num3 = size.Width / num;
				num4 = 1.0;
				break;
			}
			double num6 = num2 * 0.75;
			if (this.IsDirectionReversed)
			{
				num4 = -num4;
				num3 *= -1.0;
				Point point3 = point;
				point = point2;
				point2 = point3;
			}
			Pen pen = new Pen(this.Fill, 1.0);
			bool snapsToDevicePixels = base.SnapsToDevicePixels;
			DoubleCollection doubleCollection = snapsToDevicePixels ? new DoubleCollection() : null;
			DoubleCollection doubleCollection2 = snapsToDevicePixels ? new DoubleCollection() : null;
			if (this.Placement == TickBarPlacement.Left || this.Placement == TickBarPlacement.Right)
			{
				double num7 = this.TickFrequency;
				if (num7 > 0.0)
				{
					double num8 = (this.Maximum - this.Minimum) / size.Height;
					if (num7 < num8)
					{
						num7 = num8;
					}
				}
				dc.DrawLine(pen, point, new Point(point.X + num2, point.Y));
				dc.DrawLine(pen, new Point(point.X, point2.Y), new Point(point.X + num2, point2.Y));
				if (snapsToDevicePixels)
				{
					doubleCollection.Add(point.X);
					doubleCollection2.Add(point.Y - 0.5);
					doubleCollection.Add(point.X + num2);
					doubleCollection2.Add(point2.Y - 0.5);
					doubleCollection.Add(point.X + num6);
				}
				DoubleCollection doubleCollection3 = null;
				bool flag;
				if (base.GetValueSource(TickBar.TicksProperty, null, out flag) != BaseValueSourceInternal.Default || flag)
				{
					doubleCollection3 = this.Ticks;
				}
				if (doubleCollection3 != null && doubleCollection3.Count > 0)
				{
					for (int i = 0; i < doubleCollection3.Count; i++)
					{
						if (!DoubleUtil.LessThanOrClose(doubleCollection3[i], this.Minimum) && !DoubleUtil.GreaterThanOrClose(doubleCollection3[i], this.Maximum))
						{
							double num9 = (doubleCollection3[i] - this.Minimum) * num3 + point.Y;
							dc.DrawLine(pen, new Point(point.X, num9), new Point(point.X + num6, num9));
							if (snapsToDevicePixels)
							{
								doubleCollection2.Add(num9 - 0.5);
							}
						}
					}
				}
				else if (num7 > 0.0)
				{
					for (double num10 = num7; num10 < num; num10 += num7)
					{
						double num11 = num10 * num3 + point.Y;
						dc.DrawLine(pen, new Point(point.X, num11), new Point(point.X + num6, num11));
						if (snapsToDevicePixels)
						{
							doubleCollection2.Add(num11 - 0.5);
						}
					}
				}
				if (this.IsSelectionRangeEnabled)
				{
					double num12 = (this.SelectionStart - this.Minimum) * num3 + point.Y;
					Point point4 = new Point(point.X, num12);
					Point start = new Point(point.X + num6, num12);
					Point point5 = new Point(point.X + num6, num12 + Math.Abs(num6) * num4);
					PathSegment[] segments = new PathSegment[]
					{
						new LineSegment(point5, true),
						new LineSegment(point4, true)
					};
					PathGeometry geometry = new PathGeometry(new PathFigure[]
					{
						new PathFigure(start, segments, true)
					});
					dc.DrawGeometry(this.Fill, pen, geometry);
					num12 = (this.SelectionEnd - this.Minimum) * num3 + point.Y;
					point4 = new Point(point.X, num12);
					start = new Point(point.X + num6, num12);
					point5 = new Point(point.X + num6, num12 - Math.Abs(num6) * num4);
					segments = new PathSegment[]
					{
						new LineSegment(point5, true),
						new LineSegment(point4, true)
					};
					geometry = new PathGeometry(new PathFigure[]
					{
						new PathFigure(start, segments, true)
					});
					dc.DrawGeometry(this.Fill, pen, geometry);
				}
			}
			else
			{
				double num13 = this.TickFrequency;
				if (num13 > 0.0)
				{
					double num14 = (this.Maximum - this.Minimum) / size.Width;
					if (num13 < num14)
					{
						num13 = num14;
					}
				}
				dc.DrawLine(pen, point, new Point(point.X, point.Y + num2));
				dc.DrawLine(pen, new Point(point2.X, point.Y), new Point(point2.X, point.Y + num2));
				if (snapsToDevicePixels)
				{
					doubleCollection.Add(point.X - 0.5);
					doubleCollection2.Add(point.Y);
					doubleCollection.Add(point.X - 0.5);
					doubleCollection2.Add(point2.Y + num2);
					doubleCollection2.Add(point2.Y + num6);
				}
				DoubleCollection doubleCollection4 = null;
				bool flag2;
				if (base.GetValueSource(TickBar.TicksProperty, null, out flag2) != BaseValueSourceInternal.Default || flag2)
				{
					doubleCollection4 = this.Ticks;
				}
				if (doubleCollection4 != null && doubleCollection4.Count > 0)
				{
					for (int j = 0; j < doubleCollection4.Count; j++)
					{
						if (!DoubleUtil.LessThanOrClose(doubleCollection4[j], this.Minimum) && !DoubleUtil.GreaterThanOrClose(doubleCollection4[j], this.Maximum))
						{
							double num15 = (doubleCollection4[j] - this.Minimum) * num3 + point.X;
							dc.DrawLine(pen, new Point(num15, point.Y), new Point(num15, point.Y + num6));
							if (snapsToDevicePixels)
							{
								doubleCollection.Add(num15 - 0.5);
							}
						}
					}
				}
				else if (num13 > 0.0)
				{
					for (double num16 = num13; num16 < num; num16 += num13)
					{
						double num17 = num16 * num3 + point.X;
						dc.DrawLine(pen, new Point(num17, point.Y), new Point(num17, point.Y + num6));
						if (snapsToDevicePixels)
						{
							doubleCollection.Add(num17 - 0.5);
						}
					}
				}
				if (this.IsSelectionRangeEnabled)
				{
					double num18 = (this.SelectionStart - this.Minimum) * num3 + point.X;
					Point point6 = new Point(num18, point.Y);
					Point start2 = new Point(num18, point.Y + num6);
					Point point7 = new Point(num18 + Math.Abs(num6) * num4, point.Y + num6);
					PathSegment[] segments2 = new PathSegment[]
					{
						new LineSegment(point7, true),
						new LineSegment(point6, true)
					};
					PathGeometry geometry2 = new PathGeometry(new PathFigure[]
					{
						new PathFigure(start2, segments2, true)
					});
					dc.DrawGeometry(this.Fill, pen, geometry2);
					num18 = (this.SelectionEnd - this.Minimum) * num3 + point.X;
					point6 = new Point(num18, point.Y);
					start2 = new Point(num18, point.Y + num6);
					point7 = new Point(num18 - Math.Abs(num6) * num4, point.Y + num6);
					segments2 = new PathSegment[]
					{
						new LineSegment(point7, true),
						new LineSegment(point6, true)
					};
					geometry2 = new PathGeometry(new PathFigure[]
					{
						new PathFigure(start2, segments2, true)
					});
					dc.DrawGeometry(this.Fill, pen, geometry2);
				}
			}
			if (snapsToDevicePixels)
			{
				doubleCollection.Add(base.ActualWidth);
				doubleCollection2.Add(base.ActualHeight);
				base.VisualXSnappingGuidelines = doubleCollection;
				base.VisualYSnappingGuidelines = doubleCollection2;
			}
		}

		// Token: 0x06007E4D RID: 32333 RVA: 0x00317D08 File Offset: 0x00316D08
		private void BindToTemplatedParent(DependencyProperty target, DependencyProperty source)
		{
			if (!base.HasNonDefaultValue(target))
			{
				base.SetBinding(target, new Binding
				{
					RelativeSource = RelativeSource.TemplatedParent,
					Path = new PropertyPath(source)
				});
			}
		}

		// Token: 0x06007E4E RID: 32334 RVA: 0x00317D44 File Offset: 0x00316D44
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			Slider slider = base.TemplatedParent as Slider;
			if (slider != null)
			{
				this.BindToTemplatedParent(TickBar.TicksProperty, Slider.TicksProperty);
				this.BindToTemplatedParent(TickBar.TickFrequencyProperty, Slider.TickFrequencyProperty);
				this.BindToTemplatedParent(TickBar.IsSelectionRangeEnabledProperty, Slider.IsSelectionRangeEnabledProperty);
				this.BindToTemplatedParent(TickBar.SelectionStartProperty, Slider.SelectionStartProperty);
				this.BindToTemplatedParent(TickBar.SelectionEndProperty, Slider.SelectionEndProperty);
				this.BindToTemplatedParent(TickBar.MinimumProperty, RangeBase.MinimumProperty);
				this.BindToTemplatedParent(TickBar.MaximumProperty, RangeBase.MaximumProperty);
				this.BindToTemplatedParent(TickBar.IsDirectionReversedProperty, Slider.IsDirectionReversedProperty);
				if (!base.HasNonDefaultValue(TickBar.ReservedSpaceProperty) && slider.Track != null)
				{
					Binding binding = new Binding();
					binding.Source = slider.Track.Thumb;
					if (slider.Orientation == Orientation.Horizontal)
					{
						binding.Path = new PropertyPath(FrameworkElement.ActualWidthProperty);
					}
					else
					{
						binding.Path = new PropertyPath(FrameworkElement.ActualHeightProperty);
					}
					base.SetBinding(TickBar.ReservedSpaceProperty, binding);
				}
			}
		}

		// Token: 0x04003B24 RID: 15140
		public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(TickBar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, null, null));

		// Token: 0x04003B25 RID: 15141
		public static readonly DependencyProperty MinimumProperty = RangeBase.MinimumProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003B26 RID: 15142
		public static readonly DependencyProperty MaximumProperty = RangeBase.MaximumProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003B27 RID: 15143
		public static readonly DependencyProperty SelectionStartProperty = Slider.SelectionStartProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(-1.0, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003B28 RID: 15144
		public static readonly DependencyProperty SelectionEndProperty = Slider.SelectionEndProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(-1.0, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003B29 RID: 15145
		public static readonly DependencyProperty IsSelectionRangeEnabledProperty = Slider.IsSelectionRangeEnabledProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003B2A RID: 15146
		public static readonly DependencyProperty TickFrequencyProperty = Slider.TickFrequencyProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003B2B RID: 15147
		public static readonly DependencyProperty TicksProperty = Slider.TicksProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(DoubleCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003B2C RID: 15148
		public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003B2D RID: 15149
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(TickBarPlacement), typeof(TickBar), new FrameworkPropertyMetadata(TickBarPlacement.Top, FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(TickBar.IsValidTickBarPlacement));

		// Token: 0x04003B2E RID: 15150
		public static readonly DependencyProperty ReservedSpaceProperty = DependencyProperty.Register("ReservedSpace", typeof(double), typeof(TickBar), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
	}
}
