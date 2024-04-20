using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.Controls;

namespace System.Windows.Controls
{
	// Token: 0x02000718 RID: 1816
	[ContentProperty("Child")]
	public class AdornedElementPlaceholder : FrameworkElement, IAddChild
	{
		// Token: 0x06005F85 RID: 24453 RVA: 0x002953B8 File Offset: 0x002943B8
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				return;
			}
			if (!(value is UIElement))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(UIElement)
				}), "value");
			}
			if (this.Child != null)
			{
				throw new ArgumentException(SR.Get("CanOnlyHaveOneChild", new object[]
				{
					base.GetType(),
					value.GetType()
				}));
			}
			this.Child = (UIElement)value;
		}

		// Token: 0x06005F86 RID: 24454 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x06005F87 RID: 24455 RVA: 0x0029543E File Offset: 0x0029443E
		public UIElement AdornedElement
		{
			get
			{
				if (this.TemplatedAdorner != null)
				{
					return this.TemplatedAdorner.AdornedElement;
				}
				return null;
			}
		}

		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x06005F88 RID: 24456 RVA: 0x00295455 File Offset: 0x00294455
		// (set) Token: 0x06005F89 RID: 24457 RVA: 0x00295460 File Offset: 0x00294460
		[DefaultValue(null)]
		public virtual UIElement Child
		{
			get
			{
				return this._child;
			}
			set
			{
				UIElement child = this._child;
				if (child != value)
				{
					base.RemoveVisualChild(child);
					base.RemoveLogicalChild(child);
					this._child = value;
					base.AddVisualChild(this._child);
					base.AddLogicalChild(value);
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x06005F8A RID: 24458 RVA: 0x002954A6 File Offset: 0x002944A6
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._child != null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06005F8B RID: 24459 RVA: 0x002954B3 File Offset: 0x002944B3
		protected override Visual GetVisualChild(int index)
		{
			if (this._child == null || index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._child;
		}

		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x06005F8C RID: 24460 RVA: 0x002954E1 File Offset: 0x002944E1
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new SingleChildEnumerator(this._child);
			}
		}

		// Token: 0x06005F8D RID: 24461 RVA: 0x002954EE File Offset: 0x002944EE
		protected override void OnInitialized(EventArgs e)
		{
			if (base.TemplatedParent == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementPlaceholderMustBeInTemplate"));
			}
			base.OnInitialized(e);
		}

		// Token: 0x06005F8E RID: 24462 RVA: 0x00295510 File Offset: 0x00294510
		protected override Size MeasureOverride(Size constraint)
		{
			if (base.TemplatedParent == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementPlaceholderMustBeInTemplate"));
			}
			if (this.AdornedElement == null)
			{
				return new Size(0.0, 0.0);
			}
			Size renderSize = this.AdornedElement.RenderSize;
			UIElement child = this.Child;
			if (child != null)
			{
				child.Measure(renderSize);
			}
			return renderSize;
		}

		// Token: 0x06005F8F RID: 24463 RVA: 0x00295574 File Offset: 0x00294574
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			UIElement child = this.Child;
			if (child != null)
			{
				child.Arrange(new Rect(arrangeBounds));
			}
			return arrangeBounds;
		}

		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x06005F90 RID: 24464 RVA: 0x00295598 File Offset: 0x00294598
		private TemplatedAdorner TemplatedAdorner
		{
			get
			{
				if (this._templatedAdorner == null)
				{
					FrameworkElement frameworkElement = base.TemplatedParent as FrameworkElement;
					if (frameworkElement != null)
					{
						this._templatedAdorner = (VisualTreeHelper.GetParent(frameworkElement) as TemplatedAdorner);
						if (this._templatedAdorner != null && this._templatedAdorner.ReferenceElement == null)
						{
							this._templatedAdorner.ReferenceElement = this;
						}
					}
				}
				return this._templatedAdorner;
			}
		}

		// Token: 0x040031D4 RID: 12756
		private UIElement _child;

		// Token: 0x040031D5 RID: 12757
		private TemplatedAdorner _templatedAdorner;
	}
}
