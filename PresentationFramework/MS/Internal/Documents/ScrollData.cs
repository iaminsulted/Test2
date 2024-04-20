using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020001EE RID: 494
	internal class ScrollData
	{
		// Token: 0x06001170 RID: 4464 RVA: 0x001440BD File Offset: 0x001430BD
		internal void LineUp(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y - 16.0);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x001440DB File Offset: 0x001430DB
		internal void LineDown(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y + 16.0);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x001440F9 File Offset: 0x001430F9
		internal void LineLeft(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X - 16.0);
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00144117 File Offset: 0x00143117
		internal void LineRight(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X + 16.0);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00144135 File Offset: 0x00143135
		internal void PageUp(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y - this._viewport.Height);
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00144155 File Offset: 0x00143155
		internal void PageDown(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y + this._viewport.Height);
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00144175 File Offset: 0x00143175
		internal void PageLeft(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X - this._viewport.Width);
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00144195 File Offset: 0x00143195
		internal void PageRight(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X + this._viewport.Width);
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x001441B5 File Offset: 0x001431B5
		internal void MouseWheelUp(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y - 48.0);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x001441D3 File Offset: 0x001431D3
		internal void MouseWheelDown(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y + 48.0);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x001441F1 File Offset: 0x001431F1
		internal void MouseWheelLeft(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X - 48.0);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0014420F File Offset: 0x0014320F
		internal void MouseWheelRight(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X + 48.0);
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00144230 File Offset: 0x00143230
		internal void SetHorizontalOffset(UIElement owner, double offset)
		{
			if (!this.CanHorizontallyScroll)
			{
				return;
			}
			offset = Math.Max(0.0, Math.Min(this._extent.Width - this._viewport.Width, offset));
			if (!DoubleUtil.AreClose(offset, this._offset.X))
			{
				this._offset.X = offset;
				owner.InvalidateArrange();
				if (this._scrollOwner != null)
				{
					this._scrollOwner.InvalidateScrollInfo();
				}
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x001442AC File Offset: 0x001432AC
		internal void SetVerticalOffset(UIElement owner, double offset)
		{
			if (!this.CanVerticallyScroll)
			{
				return;
			}
			offset = Math.Max(0.0, Math.Min(this._extent.Height - this._viewport.Height, offset));
			if (!DoubleUtil.AreClose(offset, this._offset.Y))
			{
				this._offset.Y = offset;
				owner.InvalidateArrange();
				if (this._scrollOwner != null)
				{
					this._scrollOwner.InvalidateScrollInfo();
				}
			}
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00144328 File Offset: 0x00143328
		internal Rect MakeVisible(UIElement owner, Visual visual, Rect rectangle)
		{
			if (rectangle.IsEmpty || visual == null || (visual != owner && !owner.IsAncestorOf(visual)))
			{
				return Rect.Empty;
			}
			rectangle = visual.TransformToAncestor(owner).TransformBounds(rectangle);
			Rect rect = new Rect(this._offset.X, this._offset.Y, this._viewport.Width, this._viewport.Height);
			rectangle.X += rect.X;
			rectangle.Y += rect.Y;
			double num = this.ComputeScrollOffset(rect.Left, rect.Right, rectangle.Left, rectangle.Right);
			double num2 = this.ComputeScrollOffset(rect.Top, rect.Bottom, rectangle.Top, rectangle.Bottom);
			this.SetHorizontalOffset(owner, num);
			this.SetVerticalOffset(owner, num2);
			if (this.CanHorizontallyScroll)
			{
				rect.X = num;
			}
			else
			{
				rectangle.X = rect.X;
			}
			if (this.CanVerticallyScroll)
			{
				rect.Y = num2;
			}
			else
			{
				rectangle.Y = rect.Y;
			}
			rectangle.Intersect(rect);
			if (!rectangle.IsEmpty)
			{
				rectangle.X -= rect.X;
				rectangle.Y -= rect.Y;
			}
			return rectangle;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00144494 File Offset: 0x00143494
		internal void SetScrollOwner(UIElement owner, ScrollViewer value)
		{
			if (value != this._scrollOwner)
			{
				this._disableHorizonalScroll = false;
				this._disableVerticalScroll = false;
				this._offset = default(Vector);
				this._viewport = default(Size);
				this._extent = default(Size);
				this._scrollOwner = value;
				owner.InvalidateArrange();
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x001444E9 File Offset: 0x001434E9
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x001444F4 File Offset: 0x001434F4
		internal bool CanVerticallyScroll
		{
			get
			{
				return !this._disableVerticalScroll;
			}
			set
			{
				this._disableVerticalScroll = !value;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x00144500 File Offset: 0x00143500
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x0014450B File Offset: 0x0014350B
		internal bool CanHorizontallyScroll
		{
			get
			{
				return !this._disableHorizonalScroll;
			}
			set
			{
				this._disableHorizonalScroll = !value;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x00144517 File Offset: 0x00143517
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x00144524 File Offset: 0x00143524
		internal double ExtentWidth
		{
			get
			{
				return this._extent.Width;
			}
			set
			{
				this._extent.Width = value;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x00144532 File Offset: 0x00143532
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x0014453F File Offset: 0x0014353F
		internal double ExtentHeight
		{
			get
			{
				return this._extent.Height;
			}
			set
			{
				this._extent.Height = value;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x0014454D File Offset: 0x0014354D
		internal double ViewportWidth
		{
			get
			{
				return this._viewport.Width;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x0014455A File Offset: 0x0014355A
		internal double ViewportHeight
		{
			get
			{
				return this._viewport.Height;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x00144567 File Offset: 0x00143567
		internal double HorizontalOffset
		{
			get
			{
				return this._offset.X;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00144574 File Offset: 0x00143574
		internal double VerticalOffset
		{
			get
			{
				return this._offset.Y;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x00144581 File Offset: 0x00143581
		internal ScrollViewer ScrollOwner
		{
			get
			{
				return this._scrollOwner;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x00144589 File Offset: 0x00143589
		// (set) Token: 0x0600118E RID: 4494 RVA: 0x00144591 File Offset: 0x00143591
		internal Vector Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x0014459A File Offset: 0x0014359A
		// (set) Token: 0x06001190 RID: 4496 RVA: 0x001445A2 File Offset: 0x001435A2
		internal Size Extent
		{
			get
			{
				return this._extent;
			}
			set
			{
				this._extent = value;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x001445AB File Offset: 0x001435AB
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x001445B3 File Offset: 0x001435B3
		internal Size Viewport
		{
			get
			{
				return this._viewport;
			}
			set
			{
				this._viewport = value;
			}
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x001445BC File Offset: 0x001435BC
		private double ComputeScrollOffset(double topView, double bottomView, double topChild, double bottomChild)
		{
			bool flag = DoubleUtil.GreaterThanOrClose(topChild, topView) && DoubleUtil.LessThan(topChild, bottomView);
			bool flag2 = DoubleUtil.LessThanOrClose(bottomChild, bottomView) && DoubleUtil.GreaterThan(bottomChild, topView);
			double result;
			if (flag && flag2)
			{
				result = topView;
			}
			else
			{
				result = topChild;
			}
			return result;
		}

		// Token: 0x04000B12 RID: 2834
		private bool _disableHorizonalScroll;

		// Token: 0x04000B13 RID: 2835
		private bool _disableVerticalScroll;

		// Token: 0x04000B14 RID: 2836
		private Vector _offset;

		// Token: 0x04000B15 RID: 2837
		private Size _viewport;

		// Token: 0x04000B16 RID: 2838
		private Size _extent;

		// Token: 0x04000B17 RID: 2839
		private ScrollViewer _scrollOwner;
	}
}
