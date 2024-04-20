using System;
using System.ComponentModel;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x0200080C RID: 2060
	public class WrapPanel : Panel
	{
		// Token: 0x060078AC RID: 30892 RVA: 0x00300FA8 File Offset: 0x002FFFA8
		static WrapPanel()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.WrapPanel);
		}

		// Token: 0x060078AD RID: 30893 RVA: 0x0030107A File Offset: 0x0030007A
		public WrapPanel()
		{
			this._orientation = (Orientation)WrapPanel.OrientationProperty.GetDefaultValue(base.DependencyObjectType);
		}

		// Token: 0x060078AE RID: 30894 RVA: 0x0017B5D8 File Offset: 0x0017A5D8
		private static bool IsWidthHeightValid(object value)
		{
			double num = (double)value;
			return DoubleUtil.IsNaN(num) || (num >= 0.0 && !double.IsPositiveInfinity(num));
		}

		// Token: 0x17001BEE RID: 7150
		// (get) Token: 0x060078AF RID: 30895 RVA: 0x0030109D File Offset: 0x0030009D
		// (set) Token: 0x060078B0 RID: 30896 RVA: 0x003010AF File Offset: 0x003000AF
		[TypeConverter(typeof(LengthConverter))]
		public double ItemWidth
		{
			get
			{
				return (double)base.GetValue(WrapPanel.ItemWidthProperty);
			}
			set
			{
				base.SetValue(WrapPanel.ItemWidthProperty, value);
			}
		}

		// Token: 0x17001BEF RID: 7151
		// (get) Token: 0x060078B1 RID: 30897 RVA: 0x003010C2 File Offset: 0x003000C2
		// (set) Token: 0x060078B2 RID: 30898 RVA: 0x003010D4 File Offset: 0x003000D4
		[TypeConverter(typeof(LengthConverter))]
		public double ItemHeight
		{
			get
			{
				return (double)base.GetValue(WrapPanel.ItemHeightProperty);
			}
			set
			{
				base.SetValue(WrapPanel.ItemHeightProperty, value);
			}
		}

		// Token: 0x17001BF0 RID: 7152
		// (get) Token: 0x060078B3 RID: 30899 RVA: 0x003010E7 File Offset: 0x003000E7
		// (set) Token: 0x060078B4 RID: 30900 RVA: 0x003010EF File Offset: 0x003000EF
		public Orientation Orientation
		{
			get
			{
				return this._orientation;
			}
			set
			{
				base.SetValue(WrapPanel.OrientationProperty, value);
			}
		}

		// Token: 0x060078B5 RID: 30901 RVA: 0x00301102 File Offset: 0x00300102
		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WrapPanel)d)._orientation = (Orientation)e.NewValue;
		}

		// Token: 0x060078B6 RID: 30902 RVA: 0x0030111C File Offset: 0x0030011C
		protected override Size MeasureOverride(Size constraint)
		{
			WrapPanel.UVSize uvsize = new WrapPanel.UVSize(this.Orientation);
			WrapPanel.UVSize uvsize2 = new WrapPanel.UVSize(this.Orientation);
			WrapPanel.UVSize uvsize3 = new WrapPanel.UVSize(this.Orientation, constraint.Width, constraint.Height);
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			bool flag = !DoubleUtil.IsNaN(itemWidth);
			bool flag2 = !DoubleUtil.IsNaN(itemHeight);
			Size availableSize = new Size(flag ? itemWidth : constraint.Width, flag2 ? itemHeight : constraint.Height);
			UIElementCollection internalChildren = base.InternalChildren;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					uielement.Measure(availableSize);
					WrapPanel.UVSize uvsize4 = new WrapPanel.UVSize(this.Orientation, flag ? itemWidth : uielement.DesiredSize.Width, flag2 ? itemHeight : uielement.DesiredSize.Height);
					if (DoubleUtil.GreaterThan(uvsize.U + uvsize4.U, uvsize3.U))
					{
						uvsize2.U = Math.Max(uvsize.U, uvsize2.U);
						uvsize2.V += uvsize.V;
						uvsize = uvsize4;
						if (DoubleUtil.GreaterThan(uvsize4.U, uvsize3.U))
						{
							uvsize2.U = Math.Max(uvsize4.U, uvsize2.U);
							uvsize2.V += uvsize4.V;
							uvsize = new WrapPanel.UVSize(this.Orientation);
						}
					}
					else
					{
						uvsize.U += uvsize4.U;
						uvsize.V = Math.Max(uvsize4.V, uvsize.V);
					}
				}
				i++;
			}
			uvsize2.U = Math.Max(uvsize.U, uvsize2.U);
			uvsize2.V += uvsize.V;
			return new Size(uvsize2.Width, uvsize2.Height);
		}

		// Token: 0x060078B7 RID: 30903 RVA: 0x00301324 File Offset: 0x00300324
		protected override Size ArrangeOverride(Size finalSize)
		{
			int num = 0;
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			double num2 = 0.0;
			double itemU = (this.Orientation == Orientation.Horizontal) ? itemWidth : itemHeight;
			WrapPanel.UVSize uvsize = new WrapPanel.UVSize(this.Orientation);
			WrapPanel.UVSize uvsize2 = new WrapPanel.UVSize(this.Orientation, finalSize.Width, finalSize.Height);
			bool flag = !DoubleUtil.IsNaN(itemWidth);
			bool flag2 = !DoubleUtil.IsNaN(itemHeight);
			bool useItemU = (this.Orientation == Orientation.Horizontal) ? flag : flag2;
			UIElementCollection internalChildren = base.InternalChildren;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					WrapPanel.UVSize uvsize3 = new WrapPanel.UVSize(this.Orientation, flag ? itemWidth : uielement.DesiredSize.Width, flag2 ? itemHeight : uielement.DesiredSize.Height);
					if (DoubleUtil.GreaterThan(uvsize.U + uvsize3.U, uvsize2.U))
					{
						this.arrangeLine(num2, uvsize.V, num, i, useItemU, itemU);
						num2 += uvsize.V;
						uvsize = uvsize3;
						if (DoubleUtil.GreaterThan(uvsize3.U, uvsize2.U))
						{
							this.arrangeLine(num2, uvsize3.V, i, ++i, useItemU, itemU);
							num2 += uvsize3.V;
							uvsize = new WrapPanel.UVSize(this.Orientation);
						}
						num = i;
					}
					else
					{
						uvsize.U += uvsize3.U;
						uvsize.V = Math.Max(uvsize3.V, uvsize.V);
					}
				}
				i++;
			}
			if (num < internalChildren.Count)
			{
				this.arrangeLine(num2, uvsize.V, num, internalChildren.Count, useItemU, itemU);
			}
			return finalSize;
		}

		// Token: 0x060078B8 RID: 30904 RVA: 0x003014FC File Offset: 0x003004FC
		private void arrangeLine(double v, double lineV, int start, int end, bool useItemU, double itemU)
		{
			double num = 0.0;
			bool flag = this.Orientation == Orientation.Horizontal;
			UIElementCollection internalChildren = base.InternalChildren;
			for (int i = start; i < end; i++)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					WrapPanel.UVSize uvsize = new WrapPanel.UVSize(this.Orientation, uielement.DesiredSize.Width, uielement.DesiredSize.Height);
					double num2 = useItemU ? itemU : uvsize.U;
					uielement.Arrange(new Rect(flag ? num : v, flag ? v : num, flag ? num2 : lineV, flag ? lineV : num2));
					num += num2;
				}
			}
		}

		// Token: 0x04003973 RID: 14707
		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(WrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(WrapPanel.IsWidthHeightValid));

		// Token: 0x04003974 RID: 14708
		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(WrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(WrapPanel.IsWidthHeightValid));

		// Token: 0x04003975 RID: 14709
		public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof(WrapPanel), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(WrapPanel.OnOrientationChanged)));

		// Token: 0x04003976 RID: 14710
		private Orientation _orientation;

		// Token: 0x02000C40 RID: 3136
		private struct UVSize
		{
			// Token: 0x06009150 RID: 37200 RVA: 0x00348EF8 File Offset: 0x00347EF8
			internal UVSize(Orientation orientation, double width, double height)
			{
				this.U = (this.V = 0.0);
				this._orientation = orientation;
				this.Width = width;
				this.Height = height;
			}

			// Token: 0x06009151 RID: 37201 RVA: 0x00348F34 File Offset: 0x00347F34
			internal UVSize(Orientation orientation)
			{
				this.U = (this.V = 0.0);
				this._orientation = orientation;
			}

			// Token: 0x17001FD0 RID: 8144
			// (get) Token: 0x06009152 RID: 37202 RVA: 0x00348F60 File Offset: 0x00347F60
			// (set) Token: 0x06009153 RID: 37203 RVA: 0x00348F77 File Offset: 0x00347F77
			internal double Width
			{
				get
				{
					if (this._orientation != Orientation.Horizontal)
					{
						return this.V;
					}
					return this.U;
				}
				set
				{
					if (this._orientation == Orientation.Horizontal)
					{
						this.U = value;
						return;
					}
					this.V = value;
				}
			}

			// Token: 0x17001FD1 RID: 8145
			// (get) Token: 0x06009154 RID: 37204 RVA: 0x00348F90 File Offset: 0x00347F90
			// (set) Token: 0x06009155 RID: 37205 RVA: 0x00348FA7 File Offset: 0x00347FA7
			internal double Height
			{
				get
				{
					if (this._orientation != Orientation.Horizontal)
					{
						return this.U;
					}
					return this.V;
				}
				set
				{
					if (this._orientation == Orientation.Horizontal)
					{
						this.V = value;
						return;
					}
					this.U = value;
				}
			}

			// Token: 0x04004C04 RID: 19460
			internal double U;

			// Token: 0x04004C05 RID: 19461
			internal double V;

			// Token: 0x04004C06 RID: 19462
			private Orientation _orientation;
		}
	}
}
