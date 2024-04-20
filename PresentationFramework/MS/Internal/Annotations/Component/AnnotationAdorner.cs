using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020002C7 RID: 711
	internal sealed class AnnotationAdorner : Adorner
	{
		// Token: 0x06001A86 RID: 6790 RVA: 0x00164534 File Offset: 0x00163534
		public AnnotationAdorner(IAnnotationComponent component, UIElement annotatedElement) : base(annotatedElement)
		{
			if (component is UIElement)
			{
				this._annotationComponent = component;
				base.AddVisualChild((UIElement)this._annotationComponent);
				return;
			}
			throw new ArgumentException(SR.Get("AnnotationAdorner_NotUIElement"), "component");
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x00164574 File Offset: 0x00163574
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (!(this._annotationComponent is UIElement))
			{
				return null;
			}
			transform = base.GetDesiredTransform(transform);
			GeneralTransform desiredTransform = this._annotationComponent.GetDesiredTransform(transform);
			if (this._annotationComponent.AnnotatedElement == null)
			{
				return null;
			}
			if (desiredTransform == null)
			{
				this._annotatedElement = this._annotationComponent.AnnotatedElement;
				this._annotatedElement.LayoutUpdated += this.OnLayoutUpdated;
				return transform;
			}
			return desiredTransform;
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x001645E3 File Offset: 0x001635E3
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0 || this._annotationComponent == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return (UIElement)this._annotationComponent;
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x00164616 File Offset: 0x00163616
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._annotationComponent == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x00164624 File Offset: 0x00163624
		protected override Size MeasureOverride(Size availableSize)
		{
			Size availableSize2 = new Size(double.PositiveInfinity, double.PositiveInfinity);
			Invariant.Assert(this._annotationComponent != null, "AnnotationAdorner should only have one child - the annotation component.");
			((UIElement)this._annotationComponent).Measure(availableSize2);
			return new Size(0.0, 0.0);
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x00164685 File Offset: 0x00163685
		protected override Size ArrangeOverride(Size finalSize)
		{
			Invariant.Assert(this._annotationComponent != null, "AnnotationAdorner should only have one child - the annotation component.");
			((UIElement)this._annotationComponent).Arrange(new Rect(((UIElement)this._annotationComponent).DesiredSize));
			return finalSize;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x001646C0 File Offset: 0x001636C0
		internal void RemoveChildren()
		{
			base.RemoveVisualChild((UIElement)this._annotationComponent);
			this._annotationComponent = null;
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x001646DA File Offset: 0x001636DA
		internal void InvalidateTransform()
		{
			UIElement uielement = (AdornerLayer)VisualTreeHelper.GetParent(this);
			base.InvalidateMeasure();
			uielement.InvalidateVisual();
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x001646F2 File Offset: 0x001636F2
		internal IAnnotationComponent AnnotationComponent
		{
			get
			{
				return this._annotationComponent;
			}
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x001646FC File Offset: 0x001636FC
		private void OnLayoutUpdated(object sender, EventArgs args)
		{
			this._annotatedElement.LayoutUpdated -= this.OnLayoutUpdated;
			this._annotatedElement = null;
			if (this._annotationComponent.AttachedAnnotations.Count > 0)
			{
				this._annotationComponent.PresentationContext.Host.InvalidateMeasure();
				base.InvalidateMeasure();
			}
		}

		// Token: 0x04000DBC RID: 3516
		private IAnnotationComponent _annotationComponent;

		// Token: 0x04000DBD RID: 3517
		private UIElement _annotatedElement;
	}
}
