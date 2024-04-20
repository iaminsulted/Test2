using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MS.Internal.Controls
{
	// Token: 0x02000258 RID: 600
	internal class InkCanvasInnerCanvas : Panel
	{
		// Token: 0x06001728 RID: 5928 RVA: 0x0015D044 File Offset: 0x0015C044
		internal InkCanvasInnerCanvas(InkCanvas inkCanvas)
		{
			this._inkCanvas = inkCanvas;
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x0015D053 File Offset: 0x0015C053
		private InkCanvasInnerCanvas()
		{
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x0015D05C File Offset: 0x0015C05C
		protected internal override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			UIElement uielement = visualRemoved as UIElement;
			if (uielement != null)
			{
				this.InkCanvas.InkCanvasSelection.RemoveElement(uielement);
			}
			this.InkCanvas.RaiseOnVisualChildrenChanged(visualAdded, visualRemoved);
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x0015D09C File Offset: 0x0015C09C
		protected override Size MeasureOverride(Size constraint)
		{
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			Size result = default(Size);
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					uielement.Measure(availableSize);
					double num = InkCanvas.GetLeft(uielement);
					if (!DoubleUtil.IsNaN(num))
					{
						result.Width = Math.Max(result.Width, num + uielement.DesiredSize.Width);
					}
					else
					{
						result.Width = Math.Max(result.Width, uielement.DesiredSize.Width);
					}
					double num2 = InkCanvas.GetTop(uielement);
					if (!DoubleUtil.IsNaN(num2))
					{
						result.Height = Math.Max(result.Height, num2 + uielement.DesiredSize.Height);
					}
					else
					{
						result.Height = Math.Max(result.Height, uielement.DesiredSize.Height);
					}
				}
			}
			return result;
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x0015D1DC File Offset: 0x0015C1DC
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					double x = 0.0;
					double y = 0.0;
					double num = InkCanvas.GetLeft(uielement);
					if (!DoubleUtil.IsNaN(num))
					{
						x = num;
					}
					else
					{
						double num2 = InkCanvas.GetRight(uielement);
						if (!DoubleUtil.IsNaN(num2))
						{
							x = arrangeSize.Width - uielement.DesiredSize.Width - num2;
						}
					}
					double num3 = InkCanvas.GetTop(uielement);
					if (!DoubleUtil.IsNaN(num3))
					{
						y = num3;
					}
					else
					{
						double num4 = InkCanvas.GetBottom(uielement);
						if (!DoubleUtil.IsNaN(num4))
						{
							y = arrangeSize.Height - uielement.DesiredSize.Height - num4;
						}
					}
					uielement.Arrange(new Rect(new Point(x, y), uielement.DesiredSize));
				}
			}
			return arrangeSize;
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x0015D2F4 File Offset: 0x0015C2F4
		protected override void OnChildDesiredSizeChanged(UIElement child)
		{
			base.OnChildDesiredSizeChanged(child);
			base.InvalidateMeasure();
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x0015D303 File Offset: 0x0015C303
		protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return base.CreateUIElementCollection(this._inkCanvas);
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x0015CBEC File Offset: 0x0015BBEC
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return EmptyEnumerator.Instance;
			}
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x0015D311 File Offset: 0x0015C311
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			if (base.ClipToBounds)
			{
				return base.GetLayoutClip(layoutSlotSize);
			}
			return null;
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x0015D324 File Offset: 0x0015C324
		internal UIElement HitTestOnElements(Point point)
		{
			UIElement result = null;
			HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, point);
			if (hitTestResult != null)
			{
				Visual visual = hitTestResult.VisualHit as Visual;
				Visual3D visual3D = hitTestResult.VisualHit as Visual3D;
				DependencyObject dependencyObject = null;
				if (visual != null)
				{
					dependencyObject = visual;
				}
				else if (visual3D != null)
				{
					dependencyObject = visual3D;
				}
				while (dependencyObject != null)
				{
					DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
					if (parent == this.InkCanvas.InnerCanvas)
					{
						result = (dependencyObject as UIElement);
						break;
					}
					dependencyObject = parent;
				}
			}
			return result;
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001732 RID: 5938 RVA: 0x0015D394 File Offset: 0x0015C394
		internal IEnumerator PrivateLogicalChildren
		{
			get
			{
				return base.LogicalChildren;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x0015D39C File Offset: 0x0015C39C
		internal InkCanvas InkCanvas
		{
			get
			{
				return this._inkCanvas;
			}
		}

		// Token: 0x04000C94 RID: 3220
		private InkCanvas _inkCanvas;
	}
}
