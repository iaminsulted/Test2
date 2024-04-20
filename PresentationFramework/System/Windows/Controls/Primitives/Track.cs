using System;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000861 RID: 2145
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class Track : FrameworkElement
	{
		// Token: 0x06007E85 RID: 32389 RVA: 0x0031905C File Offset: 0x0031805C
		static Track()
		{
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Track), new UIPropertyMetadata(new PropertyChangedCallback(Track.OnIsEnabledChanged)));
		}

		// Token: 0x06007E87 RID: 32391 RVA: 0x003191EC File Offset: 0x003181EC
		public virtual double ValueFromPoint(Point pt)
		{
			double val;
			if (this.Orientation == Orientation.Horizontal)
			{
				val = this.Value + this.ValueFromDistance(pt.X - this.ThumbCenterOffset, pt.Y - base.RenderSize.Height * 0.5);
			}
			else
			{
				val = this.Value + this.ValueFromDistance(pt.X - base.RenderSize.Width * 0.5, pt.Y - this.ThumbCenterOffset);
			}
			return Math.Max(this.Minimum, Math.Min(this.Maximum, val));
		}

		// Token: 0x06007E88 RID: 32392 RVA: 0x00319294 File Offset: 0x00318294
		public virtual double ValueFromDistance(double horizontal, double vertical)
		{
			double num = (double)(this.IsDirectionReversed ? -1 : 1);
			if (this.Orientation == Orientation.Horizontal)
			{
				return num * horizontal * this.Density;
			}
			return -1.0 * num * vertical * this.Density;
		}

		// Token: 0x06007E89 RID: 32393 RVA: 0x003192D8 File Offset: 0x003182D8
		private void UpdateComponent(Control oldValue, Control newValue)
		{
			if (oldValue != newValue)
			{
				if (this._visualChildren == null)
				{
					this._visualChildren = new Visual[3];
				}
				if (oldValue != null)
				{
					base.RemoveVisualChild(oldValue);
				}
				int i = 0;
				while (i < 3 && this._visualChildren[i] != null)
				{
					if (this._visualChildren[i] == oldValue)
					{
						while (i < 2)
						{
							if (this._visualChildren[i + 1] == null)
							{
								break;
							}
							this._visualChildren[i] = this._visualChildren[i + 1];
							i++;
						}
					}
					else
					{
						i++;
					}
				}
				this._visualChildren[i] = newValue;
				base.AddVisualChild(newValue);
				base.InvalidateMeasure();
				base.InvalidateArrange();
			}
		}

		// Token: 0x17001D31 RID: 7473
		// (get) Token: 0x06007E8A RID: 32394 RVA: 0x00319371 File Offset: 0x00318371
		// (set) Token: 0x06007E8B RID: 32395 RVA: 0x00319379 File Offset: 0x00318379
		public RepeatButton DecreaseRepeatButton
		{
			get
			{
				return this._decreaseButton;
			}
			set
			{
				if (this._increaseButton == value)
				{
					throw new NotSupportedException(SR.Get("Track_SameButtons"));
				}
				this.UpdateComponent(this._decreaseButton, value);
				this._decreaseButton = value;
				if (this._decreaseButton != null)
				{
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		// Token: 0x17001D32 RID: 7474
		// (get) Token: 0x06007E8C RID: 32396 RVA: 0x003193B5 File Offset: 0x003183B5
		// (set) Token: 0x06007E8D RID: 32397 RVA: 0x003193BD File Offset: 0x003183BD
		public Thumb Thumb
		{
			get
			{
				return this._thumb;
			}
			set
			{
				this.UpdateComponent(this._thumb, value);
				this._thumb = value;
			}
		}

		// Token: 0x17001D33 RID: 7475
		// (get) Token: 0x06007E8E RID: 32398 RVA: 0x003193D3 File Offset: 0x003183D3
		// (set) Token: 0x06007E8F RID: 32399 RVA: 0x003193DB File Offset: 0x003183DB
		public RepeatButton IncreaseRepeatButton
		{
			get
			{
				return this._increaseButton;
			}
			set
			{
				if (this._decreaseButton == value)
				{
					throw new NotSupportedException(SR.Get("Track_SameButtons"));
				}
				this.UpdateComponent(this._increaseButton, value);
				this._increaseButton = value;
				if (this._increaseButton != null)
				{
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		// Token: 0x17001D34 RID: 7476
		// (get) Token: 0x06007E90 RID: 32400 RVA: 0x00319417 File Offset: 0x00318417
		// (set) Token: 0x06007E91 RID: 32401 RVA: 0x00319429 File Offset: 0x00318429
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(Track.OrientationProperty);
			}
			set
			{
				base.SetValue(Track.OrientationProperty, value);
			}
		}

		// Token: 0x17001D35 RID: 7477
		// (get) Token: 0x06007E92 RID: 32402 RVA: 0x0031943C File Offset: 0x0031843C
		// (set) Token: 0x06007E93 RID: 32403 RVA: 0x0031944E File Offset: 0x0031844E
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(Track.MinimumProperty);
			}
			set
			{
				base.SetValue(Track.MinimumProperty, value);
			}
		}

		// Token: 0x17001D36 RID: 7478
		// (get) Token: 0x06007E94 RID: 32404 RVA: 0x00319461 File Offset: 0x00318461
		// (set) Token: 0x06007E95 RID: 32405 RVA: 0x00319473 File Offset: 0x00318473
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(Track.MaximumProperty);
			}
			set
			{
				base.SetValue(Track.MaximumProperty, value);
			}
		}

		// Token: 0x17001D37 RID: 7479
		// (get) Token: 0x06007E96 RID: 32406 RVA: 0x00319486 File Offset: 0x00318486
		// (set) Token: 0x06007E97 RID: 32407 RVA: 0x00319498 File Offset: 0x00318498
		public double Value
		{
			get
			{
				return (double)base.GetValue(Track.ValueProperty);
			}
			set
			{
				base.SetValue(Track.ValueProperty, value);
			}
		}

		// Token: 0x17001D38 RID: 7480
		// (get) Token: 0x06007E98 RID: 32408 RVA: 0x003194AB File Offset: 0x003184AB
		// (set) Token: 0x06007E99 RID: 32409 RVA: 0x003194BD File Offset: 0x003184BD
		public double ViewportSize
		{
			get
			{
				return (double)base.GetValue(Track.ViewportSizeProperty);
			}
			set
			{
				base.SetValue(Track.ViewportSizeProperty, value);
			}
		}

		// Token: 0x06007E9A RID: 32410 RVA: 0x003194D0 File Offset: 0x003184D0
		private static bool IsValidViewport(object o)
		{
			double num = (double)o;
			return num >= 0.0 || double.IsNaN(num);
		}

		// Token: 0x17001D39 RID: 7481
		// (get) Token: 0x06007E9B RID: 32411 RVA: 0x003194F8 File Offset: 0x003184F8
		// (set) Token: 0x06007E9C RID: 32412 RVA: 0x0031950A File Offset: 0x0031850A
		public bool IsDirectionReversed
		{
			get
			{
				return (bool)base.GetValue(Track.IsDirectionReversedProperty);
			}
			set
			{
				base.SetValue(Track.IsDirectionReversedProperty, value);
			}
		}

		// Token: 0x06007E9D RID: 32413 RVA: 0x00319518 File Offset: 0x00318518
		protected override Visual GetVisualChild(int index)
		{
			if (this._visualChildren == null || this._visualChildren[index] == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._visualChildren[index];
		}

		// Token: 0x17001D3A RID: 7482
		// (get) Token: 0x06007E9E RID: 32414 RVA: 0x0031954F File Offset: 0x0031854F
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._visualChildren == null || this._visualChildren[0] == null)
				{
					return 0;
				}
				if (this._visualChildren[1] == null)
				{
					return 1;
				}
				if (this._visualChildren[2] != null)
				{
					return 3;
				}
				return 2;
			}
		}

		// Token: 0x06007E9F RID: 32415 RVA: 0x00319580 File Offset: 0x00318580
		protected override Size MeasureOverride(Size availableSize)
		{
			Size desiredSize = new Size(0.0, 0.0);
			if (this.Thumb != null)
			{
				this.Thumb.Measure(availableSize);
				desiredSize = this.Thumb.DesiredSize;
			}
			if (!double.IsNaN(this.ViewportSize))
			{
				if (this.Orientation == Orientation.Vertical)
				{
					desiredSize.Height = 0.0;
				}
				else
				{
					desiredSize.Width = 0.0;
				}
			}
			return desiredSize;
		}

		// Token: 0x06007EA0 RID: 32416 RVA: 0x003195FF File Offset: 0x003185FF
		private static void CoerceLength(ref double componentLength, double trackLength)
		{
			if (componentLength < 0.0)
			{
				componentLength = 0.0;
				return;
			}
			if (componentLength > trackLength || double.IsNaN(componentLength))
			{
				componentLength = trackLength;
			}
		}

		// Token: 0x06007EA1 RID: 32417 RVA: 0x0031962C File Offset: 0x0031862C
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.Orientation == Orientation.Vertical;
			double viewportSize = Math.Max(0.0, this.ViewportSize);
			double num;
			double num2;
			double num3;
			if (double.IsNaN(this.ViewportSize))
			{
				this.ComputeSliderLengths(arrangeSize, flag, out num, out num2, out num3);
			}
			else if (!this.ComputeScrollBarLengths(arrangeSize, viewportSize, flag, out num, out num2, out num3))
			{
				return arrangeSize;
			}
			Point location = default(Point);
			Size size = arrangeSize;
			bool isDirectionReversed = this.IsDirectionReversed;
			if (flag)
			{
				Track.CoerceLength(ref num, arrangeSize.Height);
				Track.CoerceLength(ref num3, arrangeSize.Height);
				Track.CoerceLength(ref num2, arrangeSize.Height);
				location.Y = (isDirectionReversed ? (num + num2) : 0.0);
				size.Height = num3;
				if (this.IncreaseRepeatButton != null)
				{
					this.IncreaseRepeatButton.Arrange(new Rect(location, size));
				}
				location.Y = (isDirectionReversed ? 0.0 : (num3 + num2));
				size.Height = num;
				if (this.DecreaseRepeatButton != null)
				{
					this.DecreaseRepeatButton.Arrange(new Rect(location, size));
				}
				location.Y = (isDirectionReversed ? num : num3);
				size.Height = num2;
				if (this.Thumb != null)
				{
					this.Thumb.Arrange(new Rect(location, size));
				}
				this.ThumbCenterOffset = location.Y + num2 * 0.5;
			}
			else
			{
				Track.CoerceLength(ref num, arrangeSize.Width);
				Track.CoerceLength(ref num3, arrangeSize.Width);
				Track.CoerceLength(ref num2, arrangeSize.Width);
				location.X = (isDirectionReversed ? (num3 + num2) : 0.0);
				size.Width = num;
				if (this.DecreaseRepeatButton != null)
				{
					this.DecreaseRepeatButton.Arrange(new Rect(location, size));
				}
				location.X = (isDirectionReversed ? 0.0 : (num + num2));
				size.Width = num3;
				if (this.IncreaseRepeatButton != null)
				{
					this.IncreaseRepeatButton.Arrange(new Rect(location, size));
				}
				location.X = (isDirectionReversed ? num3 : num);
				size.Width = num2;
				if (this.Thumb != null)
				{
					this.Thumb.Arrange(new Rect(location, size));
				}
				this.ThumbCenterOffset = location.X + num2 * 0.5;
			}
			return arrangeSize;
		}

		// Token: 0x06007EA2 RID: 32418 RVA: 0x0031988C File Offset: 0x0031888C
		private void ComputeSliderLengths(Size arrangeSize, bool isVertical, out double decreaseButtonLength, out double thumbLength, out double increaseButtonLength)
		{
			double minimum = this.Minimum;
			double num = Math.Max(0.0, this.Maximum - minimum);
			double num2 = Math.Min(num, this.Value - minimum);
			double num3;
			if (isVertical)
			{
				num3 = arrangeSize.Height;
				thumbLength = ((this.Thumb == null) ? 0.0 : this.Thumb.DesiredSize.Height);
			}
			else
			{
				num3 = arrangeSize.Width;
				thumbLength = ((this.Thumb == null) ? 0.0 : this.Thumb.DesiredSize.Width);
			}
			Track.CoerceLength(ref thumbLength, num3);
			double num4 = num3 - thumbLength;
			decreaseButtonLength = num4 * num2 / num;
			Track.CoerceLength(ref decreaseButtonLength, num4);
			increaseButtonLength = num4 - decreaseButtonLength;
			Track.CoerceLength(ref increaseButtonLength, num4);
			this.Density = num / num4;
		}

		// Token: 0x06007EA3 RID: 32419 RVA: 0x0031996C File Offset: 0x0031896C
		private bool ComputeScrollBarLengths(Size arrangeSize, double viewportSize, bool isVertical, out double decreaseButtonLength, out double thumbLength, out double increaseButtonLength)
		{
			double minimum = this.Minimum;
			double num = Math.Max(0.0, this.Maximum - minimum);
			double num2 = Math.Min(num, this.Value - minimum);
			double num3 = Math.Max(0.0, num) + viewportSize;
			double num4;
			double val;
			if (isVertical)
			{
				num4 = arrangeSize.Height;
				object obj = base.TryFindResource(SystemParameters.VerticalScrollBarButtonHeightKey);
				val = Math.Floor(((obj is double) ? ((double)obj) : SystemParameters.VerticalScrollBarButtonHeight) * 0.5);
			}
			else
			{
				num4 = arrangeSize.Width;
				object obj2 = base.TryFindResource(SystemParameters.HorizontalScrollBarButtonWidthKey);
				val = Math.Floor(((obj2 is double) ? ((double)obj2) : SystemParameters.HorizontalScrollBarButtonWidth) * 0.5);
			}
			thumbLength = num4 * viewportSize / num3;
			Track.CoerceLength(ref thumbLength, num4);
			thumbLength = Math.Max(val, thumbLength);
			bool flag = DoubleUtil.LessThanOrClose(num, 0.0);
			bool flag2 = thumbLength > num4;
			if (flag || flag2)
			{
				if (base.Visibility != Visibility.Hidden)
				{
					base.Visibility = Visibility.Hidden;
				}
				this.ThumbCenterOffset = double.NaN;
				this.Density = double.NaN;
				decreaseButtonLength = 0.0;
				increaseButtonLength = 0.0;
				return false;
			}
			if (base.Visibility != Visibility.Visible)
			{
				base.Visibility = Visibility.Visible;
			}
			double num5 = num4 - thumbLength;
			decreaseButtonLength = num5 * num2 / num;
			Track.CoerceLength(ref decreaseButtonLength, num5);
			increaseButtonLength = num5 - decreaseButtonLength;
			Track.CoerceLength(ref increaseButtonLength, num5);
			this.Density = num / num5;
			return true;
		}

		// Token: 0x06007EA4 RID: 32420 RVA: 0x00317D08 File Offset: 0x00316D08
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

		// Token: 0x06007EA5 RID: 32421 RVA: 0x00319B04 File Offset: 0x00318B04
		private void BindChildToTemplatedParent(FrameworkElement element, DependencyProperty target, DependencyProperty source)
		{
			if (element != null && !element.HasNonDefaultValue(target))
			{
				element.SetBinding(target, new Binding
				{
					Source = base.TemplatedParent,
					Path = new PropertyPath(source)
				});
			}
		}

		// Token: 0x06007EA6 RID: 32422 RVA: 0x00319B44 File Offset: 0x00318B44
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			RangeBase rangeBase = base.TemplatedParent as RangeBase;
			if (rangeBase != null)
			{
				this.BindToTemplatedParent(Track.MinimumProperty, RangeBase.MinimumProperty);
				this.BindToTemplatedParent(Track.MaximumProperty, RangeBase.MaximumProperty);
				this.BindToTemplatedParent(Track.ValueProperty, RangeBase.ValueProperty);
				if (rangeBase is ScrollBar)
				{
					this.BindToTemplatedParent(Track.ViewportSizeProperty, ScrollBar.ViewportSizeProperty);
					this.BindToTemplatedParent(Track.OrientationProperty, ScrollBar.OrientationProperty);
					return;
				}
				if (rangeBase is Slider)
				{
					this.BindToTemplatedParent(Track.OrientationProperty, Slider.OrientationProperty);
					this.BindToTemplatedParent(Track.IsDirectionReversedProperty, Slider.IsDirectionReversedProperty);
					this.BindChildToTemplatedParent(this.DecreaseRepeatButton, RepeatButton.DelayProperty, Slider.DelayProperty);
					this.BindChildToTemplatedParent(this.DecreaseRepeatButton, RepeatButton.IntervalProperty, Slider.IntervalProperty);
					this.BindChildToTemplatedParent(this.IncreaseRepeatButton, RepeatButton.DelayProperty, Slider.DelayProperty);
					this.BindChildToTemplatedParent(this.IncreaseRepeatButton, RepeatButton.IntervalProperty, Slider.IntervalProperty);
				}
			}
		}

		// Token: 0x06007EA7 RID: 32423 RVA: 0x00319C42 File Offset: 0x00318C42
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				Mouse.Synchronize();
			}
		}

		// Token: 0x17001D3B RID: 7483
		// (get) Token: 0x06007EA8 RID: 32424 RVA: 0x00319C57 File Offset: 0x00318C57
		// (set) Token: 0x06007EA9 RID: 32425 RVA: 0x00319C5F File Offset: 0x00318C5F
		private double ThumbCenterOffset
		{
			get
			{
				return this._thumbCenterOffset;
			}
			set
			{
				this._thumbCenterOffset = value;
			}
		}

		// Token: 0x17001D3C RID: 7484
		// (get) Token: 0x06007EAA RID: 32426 RVA: 0x00319C68 File Offset: 0x00318C68
		// (set) Token: 0x06007EAB RID: 32427 RVA: 0x00319C70 File Offset: 0x00318C70
		private double Density
		{
			get
			{
				return this._density;
			}
			set
			{
				this._density = value;
			}
		}

		// Token: 0x17001D3D RID: 7485
		// (get) Token: 0x06007EAC RID: 32428 RVA: 0x001FD464 File Offset: 0x001FC464
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x04003B40 RID: 15168
		[CommonDependencyProperty]
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Track), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		// Token: 0x04003B41 RID: 15169
		[CommonDependencyProperty]
		public static readonly DependencyProperty MinimumProperty = RangeBase.MinimumProperty.AddOwner(typeof(Track), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

		// Token: 0x04003B42 RID: 15170
		[CommonDependencyProperty]
		public static readonly DependencyProperty MaximumProperty = RangeBase.MaximumProperty.AddOwner(typeof(Track), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsArrange));

		// Token: 0x04003B43 RID: 15171
		[CommonDependencyProperty]
		public static readonly DependencyProperty ValueProperty = RangeBase.ValueProperty.AddOwner(typeof(Track), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		// Token: 0x04003B44 RID: 15172
		[CommonDependencyProperty]
		public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(double), typeof(Track), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsArrange), new ValidateValueCallback(Track.IsValidViewport));

		// Token: 0x04003B45 RID: 15173
		[CommonDependencyProperty]
		public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register("IsDirectionReversed", typeof(bool), typeof(Track), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003B46 RID: 15174
		private RepeatButton _increaseButton;

		// Token: 0x04003B47 RID: 15175
		private RepeatButton _decreaseButton;

		// Token: 0x04003B48 RID: 15176
		private Thumb _thumb;

		// Token: 0x04003B49 RID: 15177
		private Visual[] _visualChildren;

		// Token: 0x04003B4A RID: 15178
		private double _density = double.NaN;

		// Token: 0x04003B4B RID: 15179
		private double _thumbCenterOffset = double.NaN;
	}
}
