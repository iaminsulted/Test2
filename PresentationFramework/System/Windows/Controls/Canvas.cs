using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x0200072B RID: 1835
	public class Canvas : Panel
	{
		// Token: 0x0600608F RID: 24719 RVA: 0x00299E1C File Offset: 0x00298E1C
		static Canvas()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.Canvas);
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x00299F68 File Offset: 0x00298F68
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetLeft(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Canvas.LeftProperty);
		}

		// Token: 0x06006092 RID: 24722 RVA: 0x00299F88 File Offset: 0x00298F88
		public static void SetLeft(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Canvas.LeftProperty, length);
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x00299FA9 File Offset: 0x00298FA9
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetTop(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Canvas.TopProperty);
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x00299FC9 File Offset: 0x00298FC9
		public static void SetTop(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Canvas.TopProperty, length);
		}

		// Token: 0x06006095 RID: 24725 RVA: 0x00299FEA File Offset: 0x00298FEA
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetRight(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Canvas.RightProperty);
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x0029A00A File Offset: 0x0029900A
		public static void SetRight(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Canvas.RightProperty, length);
		}

		// Token: 0x06006097 RID: 24727 RVA: 0x0029A02B File Offset: 0x0029902B
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetBottom(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Canvas.BottomProperty);
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x0029A04B File Offset: 0x0029904B
		public static void SetBottom(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Canvas.BottomProperty, length);
		}

		// Token: 0x06006099 RID: 24729 RVA: 0x0029A06C File Offset: 0x0029906C
		private static void OnPositioningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = d as UIElement;
			if (uielement != null)
			{
				Canvas canvas = VisualTreeHelper.GetParent(uielement) as Canvas;
				if (canvas != null)
				{
					canvas.InvalidateArrange();
				}
			}
		}

		// Token: 0x0600609A RID: 24730 RVA: 0x0029A098 File Offset: 0x00299098
		protected override Size MeasureOverride(Size constraint)
		{
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					uielement.Measure(availableSize);
				}
			}
			return default(Size);
		}

		// Token: 0x0600609B RID: 24731 RVA: 0x0029A118 File Offset: 0x00299118
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					double x = 0.0;
					double y = 0.0;
					double left = Canvas.GetLeft(uielement);
					if (!DoubleUtil.IsNaN(left))
					{
						x = left;
					}
					else
					{
						double right = Canvas.GetRight(uielement);
						if (!DoubleUtil.IsNaN(right))
						{
							x = arrangeSize.Width - uielement.DesiredSize.Width - right;
						}
					}
					double top = Canvas.GetTop(uielement);
					if (!DoubleUtil.IsNaN(top))
					{
						y = top;
					}
					else
					{
						double bottom = Canvas.GetBottom(uielement);
						if (!DoubleUtil.IsNaN(bottom))
						{
							y = arrangeSize.Height - uielement.DesiredSize.Height - bottom;
						}
					}
					uielement.Arrange(new Rect(new Point(x, y), uielement.DesiredSize));
				}
			}
			return arrangeSize;
		}

		// Token: 0x0600609C RID: 24732 RVA: 0x0029A22C File Offset: 0x0029922C
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			if (base.ClipToBounds)
			{
				return new RectangleGeometry(new Rect(base.RenderSize));
			}
			return null;
		}

		// Token: 0x17001651 RID: 5713
		// (get) Token: 0x0600609D RID: 24733 RVA: 0x001FCA9D File Offset: 0x001FBA9D
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x04003232 RID: 12850
		public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(double), typeof(Canvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Canvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		// Token: 0x04003233 RID: 12851
		public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(double), typeof(Canvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Canvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		// Token: 0x04003234 RID: 12852
		public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right", typeof(double), typeof(Canvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Canvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		// Token: 0x04003235 RID: 12853
		public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(double), typeof(Canvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Canvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));
	}
}
