using System;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200084B RID: 2123
	internal sealed class PopupRoot : FrameworkElement
	{
		// Token: 0x06007C96 RID: 31894 RVA: 0x003101C0 File Offset: 0x0030F1C0
		static PopupRoot()
		{
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(PopupRoot), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
		}

		// Token: 0x06007C97 RID: 31895 RVA: 0x003101E0 File Offset: 0x0030F1E0
		internal PopupRoot()
		{
			this.Initialize();
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x003101F0 File Offset: 0x0030F1F0
		private void Initialize()
		{
			this._transformDecorator = new Decorator();
			base.AddVisualChild(this._transformDecorator);
			this._transformDecorator.ClipToBounds = true;
			this._adornerDecorator = new NonLogicalAdornerDecorator();
			this._transformDecorator.Child = this._adornerDecorator;
		}

		// Token: 0x17001CC8 RID: 7368
		// (get) Token: 0x06007C99 RID: 31897 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x0031023C File Offset: 0x0030F23C
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._transformDecorator;
		}

		// Token: 0x06007C9B RID: 31899 RVA: 0x00310262 File Offset: 0x0030F262
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new PopupRootAutomationPeer(this);
		}

		// Token: 0x17001CC9 RID: 7369
		// (get) Token: 0x06007C9C RID: 31900 RVA: 0x0031026A File Offset: 0x0030F26A
		// (set) Token: 0x06007C9D RID: 31901 RVA: 0x00310277 File Offset: 0x0030F277
		internal UIElement Child
		{
			get
			{
				return this._adornerDecorator.Child;
			}
			set
			{
				this._adornerDecorator.Child = value;
			}
		}

		// Token: 0x17001CCA RID: 7370
		// (get) Token: 0x06007C9E RID: 31902 RVA: 0x00310288 File Offset: 0x0030F288
		internal Vector AnimationOffset
		{
			get
			{
				TranslateTransform translateTransform = this._adornerDecorator.RenderTransform as TranslateTransform;
				if (translateTransform != null)
				{
					return new Vector(translateTransform.X, translateTransform.Y);
				}
				return default(Vector);
			}
		}

		// Token: 0x17001CCB RID: 7371
		// (set) Token: 0x06007C9F RID: 31903 RVA: 0x003102C4 File Offset: 0x0030F2C4
		internal Transform Transform
		{
			set
			{
				this._transformDecorator.LayoutTransform = value;
			}
		}

		// Token: 0x06007CA0 RID: 31904 RVA: 0x003102D4 File Offset: 0x0030F2D4
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = new Size(double.PositiveInfinity, double.PositiveInfinity);
			Popup popup = base.Parent as Popup;
			try
			{
				this._transformDecorator.Measure(size);
			}
			catch (Exception savedException)
			{
				if (popup != null)
				{
					popup.SavedException = savedException;
				}
				throw;
			}
			size = this._transformDecorator.DesiredSize;
			if (popup != null)
			{
				bool flag;
				bool flag2;
				Size popupSizeRestrictions = this.GetPopupSizeRestrictions(popup, size, out flag, out flag2);
				if (flag || flag2)
				{
					if (flag == flag2)
					{
						size = this.Get2DRestrictedDesiredSize(popupSizeRestrictions);
					}
					else
					{
						Size availableSize = new Size(flag ? popupSizeRestrictions.Width : double.PositiveInfinity, flag2 ? popupSizeRestrictions.Height : double.PositiveInfinity);
						this._transformDecorator.Measure(availableSize);
						size = this._transformDecorator.DesiredSize;
						popupSizeRestrictions = this.GetPopupSizeRestrictions(popup, size, out flag, out flag2);
						if (flag || flag2)
						{
							size = this.Get2DRestrictedDesiredSize(popupSizeRestrictions);
						}
					}
				}
			}
			return size;
		}

		// Token: 0x06007CA1 RID: 31905 RVA: 0x003103D0 File Offset: 0x0030F3D0
		private Size GetPopupSizeRestrictions(Popup popup, Size desiredSize, out bool restrictWidth, out bool restrictHeight)
		{
			Size result = popup.RestrictSize(desiredSize);
			restrictWidth = (Math.Abs(result.Width - desiredSize.Width) > 0.01);
			restrictHeight = (Math.Abs(result.Height - desiredSize.Height) > 0.01);
			return result;
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x0031042C File Offset: 0x0030F42C
		private Size Get2DRestrictedDesiredSize(Size restrictedSize)
		{
			this._transformDecorator.Measure(restrictedSize);
			Size desiredSize = this._transformDecorator.DesiredSize;
			return new Size(Math.Min(restrictedSize.Width, desiredSize.Width), Math.Min(restrictedSize.Height, desiredSize.Height));
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x0031047C File Offset: 0x0030F47C
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			this._transformDecorator.Arrange(new Rect(arrangeSize));
			return arrangeSize;
		}

		// Token: 0x06007CA4 RID: 31908 RVA: 0x00310490 File Offset: 0x0030F490
		internal void SetupLayoutBindings(Popup popup)
		{
			Binding binding = new Binding("Width");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.WidthProperty, binding);
			binding = new Binding("Height");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.HeightProperty, binding);
			binding = new Binding("MinWidth");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.MinWidthProperty, binding);
			binding = new Binding("MinHeight");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.MinHeightProperty, binding);
			binding = new Binding("MaxWidth");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.MaxWidthProperty, binding);
			binding = new Binding("MaxHeight");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.MaxHeightProperty, binding);
		}

		// Token: 0x06007CA5 RID: 31909 RVA: 0x003105A0 File Offset: 0x0030F5A0
		internal void SetupFadeAnimation(Duration duration, bool visible)
		{
			DoubleAnimation animation = new DoubleAnimation(visible ? 0.0 : 1.0, visible ? 1.0 : 0.0, duration, FillBehavior.HoldEnd);
			base.BeginAnimation(UIElement.OpacityProperty, animation);
		}

		// Token: 0x06007CA6 RID: 31910 RVA: 0x003105F0 File Offset: 0x0030F5F0
		internal void SetupTranslateAnimations(PopupAnimation animationType, Duration duration, bool animateFromRight, bool animateFromBottom)
		{
			UIElement child = this.Child;
			if (child == null)
			{
				return;
			}
			TranslateTransform translateTransform = this._adornerDecorator.RenderTransform as TranslateTransform;
			if (translateTransform == null)
			{
				translateTransform = new TranslateTransform();
				this._adornerDecorator.RenderTransform = translateTransform;
			}
			if (animationType == PopupAnimation.Scroll)
			{
				FlowDirection flowDirection = (FlowDirection)child.GetValue(FrameworkElement.FlowDirectionProperty);
				FlowDirection flowDirection2 = base.FlowDirection;
				if (flowDirection != flowDirection2)
				{
					animateFromRight = !animateFromRight;
				}
				double width = this._adornerDecorator.RenderSize.Width;
				DoubleAnimation animation = new DoubleAnimation(animateFromRight ? width : (-width), 0.0, duration, FillBehavior.Stop);
				translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
			}
			double height = this._adornerDecorator.RenderSize.Height;
			DoubleAnimation animation2 = new DoubleAnimation(animateFromBottom ? height : (-height), 0.0, duration, FillBehavior.Stop);
			translateTransform.BeginAnimation(TranslateTransform.YProperty, animation2);
		}

		// Token: 0x06007CA7 RID: 31911 RVA: 0x003106D0 File Offset: 0x0030F6D0
		internal void StopAnimations()
		{
			base.BeginAnimation(UIElement.OpacityProperty, null);
			TranslateTransform translateTransform = this._adornerDecorator.RenderTransform as TranslateTransform;
			if (translateTransform != null)
			{
				translateTransform.BeginAnimation(TranslateTransform.XProperty, null);
				translateTransform.BeginAnimation(TranslateTransform.YProperty, null);
			}
		}

		// Token: 0x06007CA8 RID: 31912 RVA: 0x00310718 File Offset: 0x0030F718
		internal override bool IgnoreModelParentBuildRoute(RoutedEventArgs e)
		{
			if (e is QueryCursorEventArgs)
			{
				return true;
			}
			FrameworkElement frameworkElement = this.Child as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.IgnoreModelParentBuildRoute(e);
			}
			return base.IgnoreModelParentBuildRoute(e);
		}

		// Token: 0x04003A99 RID: 15001
		private Decorator _transformDecorator;

		// Token: 0x04003A9A RID: 15002
		private AdornerDecorator _adornerDecorator;
	}
}
