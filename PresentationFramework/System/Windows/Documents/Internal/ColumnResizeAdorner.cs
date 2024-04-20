using System;
using System.Windows.Media;

namespace System.Windows.Documents.Internal
{
	// Token: 0x0200070F RID: 1807
	internal class ColumnResizeAdorner : Adorner
	{
		// Token: 0x06005E24 RID: 24100 RVA: 0x0028E540 File Offset: 0x0028D540
		internal ColumnResizeAdorner(UIElement scope) : base(scope)
		{
			this._pen = new Pen(new SolidColorBrush(Colors.LightSlateGray), 2.0);
			this._x = double.NaN;
			this._top = double.NaN;
			this._height = double.NaN;
		}

		// Token: 0x06005E25 RID: 24101 RVA: 0x0028E5A0 File Offset: 0x0028D5A0
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			TranslateTransform value = new TranslateTransform(this._x, this._top);
			generalTransformGroup.Children.Add(value);
			if (transform != null)
			{
				generalTransformGroup.Children.Add(transform);
			}
			return generalTransformGroup;
		}

		// Token: 0x06005E26 RID: 24102 RVA: 0x0028E5E1 File Offset: 0x0028D5E1
		protected override void OnRender(DrawingContext drawingContext)
		{
			drawingContext.DrawLine(this._pen, new Point(0.0, 0.0), new Point(0.0, this._height));
		}

		// Token: 0x06005E27 RID: 24103 RVA: 0x0028E61C File Offset: 0x0028D61C
		internal void Update(double newX)
		{
			if (this._x != newX)
			{
				this._x = newX;
				AdornerLayer adornerLayer = VisualTreeHelper.GetParent(this) as AdornerLayer;
				if (adornerLayer != null)
				{
					adornerLayer.Update(base.AdornedElement);
					adornerLayer.InvalidateVisual();
				}
			}
		}

		// Token: 0x06005E28 RID: 24104 RVA: 0x0028E65A File Offset: 0x0028D65A
		internal void Initialize(UIElement renderScope, double xPos, double yPos, double height)
		{
			this._adornerLayer = AdornerLayer.GetAdornerLayer(renderScope);
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Add(this);
			}
			this._x = xPos;
			this._top = yPos;
			this._height = height;
		}

		// Token: 0x06005E29 RID: 24105 RVA: 0x0028E692 File Offset: 0x0028D692
		internal void Uninitialize()
		{
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Remove(this);
				this._adornerLayer = null;
			}
		}

		// Token: 0x04003174 RID: 12660
		private double _x;

		// Token: 0x04003175 RID: 12661
		private double _top;

		// Token: 0x04003176 RID: 12662
		private double _height;

		// Token: 0x04003177 RID: 12663
		private Pen _pen;

		// Token: 0x04003178 RID: 12664
		private AdornerLayer _adornerLayer;
	}
}
