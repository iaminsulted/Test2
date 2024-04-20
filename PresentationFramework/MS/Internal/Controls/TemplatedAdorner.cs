using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Controls
{
	// Token: 0x0200025F RID: 607
	internal sealed class TemplatedAdorner : Adorner
	{
		// Token: 0x06001781 RID: 6017 RVA: 0x0015E790 File Offset: 0x0015D790
		public void ClearChild()
		{
			base.RemoveVisualChild(this._child);
			this._child = null;
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0015E7A8 File Offset: 0x0015D7A8
		public TemplatedAdorner(UIElement adornedElement, ControlTemplate adornerTemplate) : base(adornedElement)
		{
			this._child = new Control
			{
				DataContext = Validation.GetErrors(adornedElement),
				IsTabStop = false,
				Template = adornerTemplate
			};
			base.AddVisualChild(this._child);
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x0015E7F0 File Offset: 0x0015D7F0
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (this.ReferenceElement == null)
			{
				return transform;
			}
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			generalTransformGroup.Children.Add(transform);
			GeneralTransform generalTransform = base.TransformToDescendant(this.ReferenceElement);
			if (generalTransform != null)
			{
				generalTransformGroup.Children.Add(generalTransform);
			}
			return generalTransformGroup;
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x0015E836 File Offset: 0x0015D836
		// (set) Token: 0x06001785 RID: 6021 RVA: 0x0015E83E File Offset: 0x0015D83E
		public FrameworkElement ReferenceElement
		{
			get
			{
				return this._referenceElement;
			}
			set
			{
				this._referenceElement = value;
			}
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x0015E847 File Offset: 0x0015D847
		protected override Visual GetVisualChild(int index)
		{
			if (this._child == null || index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._child;
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x0015E875 File Offset: 0x0015D875
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._child == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0015E884 File Offset: 0x0015D884
		protected override Size MeasureOverride(Size constraint)
		{
			if (this.ReferenceElement != null && base.AdornedElement != null && base.AdornedElement.IsMeasureValid && !DoubleUtil.AreClose(this.ReferenceElement.DesiredSize, base.AdornedElement.DesiredSize))
			{
				this.ReferenceElement.InvalidateMeasure();
			}
			this._child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			return this._child.DesiredSize;
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x0015E904 File Offset: 0x0015D904
		protected override Size ArrangeOverride(Size size)
		{
			Size size2 = base.ArrangeOverride(size);
			if (this._child != null)
			{
				this._child.Arrange(new Rect(default(Point), size2));
			}
			return size2;
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x0015E93C File Offset: 0x0015D93C
		internal override bool NeedsUpdate(Size oldSize)
		{
			bool result = base.NeedsUpdate(oldSize);
			Visibility visibility = base.AdornedElement.IsVisible ? Visibility.Visible : Visibility.Collapsed;
			if (visibility != base.Visibility)
			{
				base.Visibility = visibility;
				result = true;
			}
			return result;
		}

		// Token: 0x04000CAE RID: 3246
		private Control _child;

		// Token: 0x04000CAF RID: 3247
		private FrameworkElement _referenceElement;
	}
}
