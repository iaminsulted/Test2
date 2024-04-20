using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Controls
{
	// Token: 0x02000257 RID: 599
	internal class InkCanvasFeedbackAdorner : Adorner
	{
		// Token: 0x06001721 RID: 5921 RVA: 0x0015CD53 File Offset: 0x0015BD53
		private InkCanvasFeedbackAdorner() : base(null)
		{
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x0015CD84 File Offset: 0x0015BD84
		internal InkCanvasFeedbackAdorner(InkCanvas inkCanvas) : base((inkCanvas != null) ? inkCanvas.InnerCanvas : null)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			this._inkCanvas = inkCanvas;
			this._adornerBorderPen = new Pen(Brushes.Black, 1.0);
			DoubleCollection doubleCollection = new DoubleCollection();
			doubleCollection.Add(4.5);
			doubleCollection.Add(4.5);
			this._adornerBorderPen.DashStyle = new DashStyle(doubleCollection, 2.25);
			this._adornerBorderPen.DashCap = PenLineCap.Flat;
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x0015CE44 File Offset: 0x0015BE44
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			base.VerifyAccess();
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			generalTransformGroup.Children.Add(transform);
			if (!DoubleUtil.AreClose(this._offsetX, 0.0) || !DoubleUtil.AreClose(this._offsetY, 0.0))
			{
				generalTransformGroup.Children.Add(new TranslateTransform(this._offsetX, this._offsetY));
			}
			return generalTransformGroup;
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x0015CEC0 File Offset: 0x0015BEC0
		private void OnBoundsUpdated(Rect rect)
		{
			base.VerifyAccess();
			if (rect != this._previousRect)
			{
				bool flag = false;
				Size size;
				double num2;
				double num3;
				if (!rect.IsEmpty)
				{
					double num = 12.0;
					Rect rect2 = Rect.Inflate(rect, num, num);
					size = new Size(rect2.Width, rect2.Height);
					num2 = rect2.Left;
					num3 = rect2.Top;
				}
				else
				{
					size = new Size(0.0, 0.0);
					num2 = 0.0;
					num3 = 0.0;
				}
				if (this._frameSize != size)
				{
					this._frameSize = size;
					flag = true;
				}
				if (!DoubleUtil.AreClose(this._offsetX, num2) || !DoubleUtil.AreClose(this._offsetY, num3))
				{
					this._offsetX = num2;
					this._offsetY = num3;
					flag = true;
				}
				if (flag)
				{
					base.InvalidateMeasure();
					base.InvalidateVisual();
					if ((UIElement)VisualTreeHelper.GetParent(this) != null)
					{
						((UIElement)VisualTreeHelper.GetParent(this)).InvalidateArrange();
					}
				}
				this._previousRect = rect;
			}
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x0015CFD0 File Offset: 0x0015BFD0
		protected override Size MeasureOverride(Size constraint)
		{
			base.VerifyAccess();
			return this._frameSize;
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0015CFE0 File Offset: 0x0015BFE0
		protected override void OnRender(DrawingContext drawingContext)
		{
			drawingContext.DrawRectangle(null, this._adornerBorderPen, new Rect(4.0, 4.0, this._frameSize.Width - 8.0, this._frameSize.Height - 8.0));
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0015D03B File Offset: 0x0015C03B
		internal void UpdateBounds(Rect rect)
		{
			this.OnBoundsUpdated(rect);
		}

		// Token: 0x04000C8C RID: 3212
		private InkCanvas _inkCanvas;

		// Token: 0x04000C8D RID: 3213
		private Size _frameSize = new Size(0.0, 0.0);

		// Token: 0x04000C8E RID: 3214
		private Rect _previousRect = Rect.Empty;

		// Token: 0x04000C8F RID: 3215
		private double _offsetX;

		// Token: 0x04000C90 RID: 3216
		private double _offsetY;

		// Token: 0x04000C91 RID: 3217
		private Pen _adornerBorderPen;

		// Token: 0x04000C92 RID: 3218
		private const int CornerResizeHandleSize = 8;

		// Token: 0x04000C93 RID: 3219
		private const double BorderMargin = 8.0;
	}
}
