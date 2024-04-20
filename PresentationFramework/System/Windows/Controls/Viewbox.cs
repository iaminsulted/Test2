using System;
using System.Collections;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000801 RID: 2049
	public class Viewbox : Decorator
	{
		// Token: 0x06007706 RID: 30470 RVA: 0x002F0F08 File Offset: 0x002EFF08
		static Viewbox()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.ViewBox);
		}

		// Token: 0x06007708 RID: 30472 RVA: 0x002F0F9C File Offset: 0x002EFF9C
		private static bool ValidateStretchValue(object value)
		{
			Stretch stretch = (Stretch)value;
			return stretch == Stretch.Uniform || stretch == Stretch.None || stretch == Stretch.Fill || stretch == Stretch.UniformToFill;
		}

		// Token: 0x06007709 RID: 30473 RVA: 0x002F0FC4 File Offset: 0x002EFFC4
		private static bool ValidateStretchDirectionValue(object value)
		{
			StretchDirection stretchDirection = (StretchDirection)value;
			return stretchDirection == StretchDirection.Both || stretchDirection == StretchDirection.DownOnly || stretchDirection == StretchDirection.UpOnly;
		}

		// Token: 0x17001BA3 RID: 7075
		// (get) Token: 0x0600770A RID: 30474 RVA: 0x002F0FE6 File Offset: 0x002EFFE6
		private ContainerVisual InternalVisual
		{
			get
			{
				if (this._internalVisual == null)
				{
					this._internalVisual = new ContainerVisual();
					base.AddVisualChild(this._internalVisual);
				}
				return this._internalVisual;
			}
		}

		// Token: 0x17001BA4 RID: 7076
		// (get) Token: 0x0600770B RID: 30475 RVA: 0x002F1010 File Offset: 0x002F0010
		// (set) Token: 0x0600770C RID: 30476 RVA: 0x002F1040 File Offset: 0x002F0040
		private UIElement InternalChild
		{
			get
			{
				VisualCollection children = this.InternalVisual.Children;
				if (children.Count != 0)
				{
					return children[0] as UIElement;
				}
				return null;
			}
			set
			{
				VisualCollection children = this.InternalVisual.Children;
				if (children.Count != 0)
				{
					children.Clear();
				}
				children.Add(value);
			}
		}

		// Token: 0x17001BA5 RID: 7077
		// (get) Token: 0x0600770D RID: 30477 RVA: 0x002F106F File Offset: 0x002F006F
		// (set) Token: 0x0600770E RID: 30478 RVA: 0x002F107C File Offset: 0x002F007C
		private Transform InternalTransform
		{
			get
			{
				return this.InternalVisual.Transform;
			}
			set
			{
				this.InternalVisual.Transform = value;
			}
		}

		// Token: 0x17001BA6 RID: 7078
		// (get) Token: 0x0600770F RID: 30479 RVA: 0x002F108A File Offset: 0x002F008A
		// (set) Token: 0x06007710 RID: 30480 RVA: 0x002F1094 File Offset: 0x002F0094
		public override UIElement Child
		{
			get
			{
				return this.InternalChild;
			}
			set
			{
				UIElement internalChild = this.InternalChild;
				if (internalChild != value)
				{
					base.RemoveLogicalChild(internalChild);
					if (value != null)
					{
						base.AddLogicalChild(value);
					}
					this.InternalChild = value;
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001BA7 RID: 7079
		// (get) Token: 0x06007711 RID: 30481 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007712 RID: 30482 RVA: 0x002F10CA File Offset: 0x002F00CA
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this.InternalVisual;
		}

		// Token: 0x17001BA8 RID: 7080
		// (get) Token: 0x06007713 RID: 30483 RVA: 0x002F10F0 File Offset: 0x002F00F0
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.InternalChild == null)
				{
					return EmptyEnumerator.Instance;
				}
				return new SingleChildEnumerator(this.InternalChild);
			}
		}

		// Token: 0x17001BA9 RID: 7081
		// (get) Token: 0x06007714 RID: 30484 RVA: 0x002F110B File Offset: 0x002F010B
		// (set) Token: 0x06007715 RID: 30485 RVA: 0x002F111D File Offset: 0x002F011D
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(Viewbox.StretchProperty);
			}
			set
			{
				base.SetValue(Viewbox.StretchProperty, value);
			}
		}

		// Token: 0x17001BAA RID: 7082
		// (get) Token: 0x06007716 RID: 30486 RVA: 0x002F1130 File Offset: 0x002F0130
		// (set) Token: 0x06007717 RID: 30487 RVA: 0x002F1142 File Offset: 0x002F0142
		public StretchDirection StretchDirection
		{
			get
			{
				return (StretchDirection)base.GetValue(Viewbox.StretchDirectionProperty);
			}
			set
			{
				base.SetValue(Viewbox.StretchDirectionProperty, value);
			}
		}

		// Token: 0x06007718 RID: 30488 RVA: 0x002F1158 File Offset: 0x002F0158
		protected override Size MeasureOverride(Size constraint)
		{
			UIElement internalChild = this.InternalChild;
			Size result = default(Size);
			if (internalChild != null)
			{
				Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
				internalChild.Measure(availableSize);
				Size desiredSize = internalChild.DesiredSize;
				Size size = Viewbox.ComputeScaleFactor(constraint, desiredSize, this.Stretch, this.StretchDirection);
				result.Width = size.Width * desiredSize.Width;
				result.Height = size.Height * desiredSize.Height;
			}
			return result;
		}

		// Token: 0x06007719 RID: 30489 RVA: 0x002F11E0 File Offset: 0x002F01E0
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElement internalChild = this.InternalChild;
			if (internalChild != null)
			{
				Size desiredSize = internalChild.DesiredSize;
				Size size = Viewbox.ComputeScaleFactor(arrangeSize, desiredSize, this.Stretch, this.StretchDirection);
				this.InternalTransform = new ScaleTransform(size.Width, size.Height);
				internalChild.Arrange(new Rect(default(Point), internalChild.DesiredSize));
				arrangeSize.Width = size.Width * desiredSize.Width;
				arrangeSize.Height = size.Height * desiredSize.Height;
			}
			return arrangeSize;
		}

		// Token: 0x0600771A RID: 30490 RVA: 0x002F1274 File Offset: 0x002F0274
		internal static Size ComputeScaleFactor(Size availableSize, Size contentSize, Stretch stretch, StretchDirection stretchDirection)
		{
			double num = 1.0;
			double num2 = 1.0;
			bool flag = !double.IsPositiveInfinity(availableSize.Width);
			bool flag2 = !double.IsPositiveInfinity(availableSize.Height);
			if ((stretch == Stretch.Uniform || stretch == Stretch.UniformToFill || stretch == Stretch.Fill) && (flag || flag2))
			{
				num = (DoubleUtil.IsZero(contentSize.Width) ? 0.0 : (availableSize.Width / contentSize.Width));
				num2 = (DoubleUtil.IsZero(contentSize.Height) ? 0.0 : (availableSize.Height / contentSize.Height));
				if (!flag)
				{
					num = num2;
				}
				else if (!flag2)
				{
					num2 = num;
				}
				else
				{
					switch (stretch)
					{
					case Stretch.Uniform:
						num2 = (num = ((num < num2) ? num : num2));
						break;
					case Stretch.UniformToFill:
						num2 = (num = ((num > num2) ? num : num2));
						break;
					}
				}
				switch (stretchDirection)
				{
				case StretchDirection.UpOnly:
					if (num < 1.0)
					{
						num = 1.0;
					}
					if (num2 < 1.0)
					{
						num2 = 1.0;
					}
					break;
				case StretchDirection.DownOnly:
					if (num > 1.0)
					{
						num = 1.0;
					}
					if (num2 > 1.0)
					{
						num2 = 1.0;
					}
					break;
				}
			}
			return new Size(num, num2);
		}

		// Token: 0x040038C4 RID: 14532
		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(Viewbox), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Viewbox.ValidateStretchValue));

		// Token: 0x040038C5 RID: 14533
		public static readonly DependencyProperty StretchDirectionProperty = DependencyProperty.Register("StretchDirection", typeof(StretchDirection), typeof(Viewbox), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Viewbox.ValidateStretchDirectionValue));

		// Token: 0x040038C6 RID: 14534
		private ContainerVisual _internalVisual;
	}
}
