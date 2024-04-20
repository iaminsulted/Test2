using System;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	// Token: 0x020007DE RID: 2014
	public class StackPanel : Panel, IScrollInfo, IStackMeasure
	{
		// Token: 0x060073C9 RID: 29641 RVA: 0x002E417C File Offset: 0x002E317C
		static StackPanel()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.StackPanel);
		}

		// Token: 0x060073CB RID: 29643 RVA: 0x002E41DB File Offset: 0x002E31DB
		public void LineUp()
		{
			this.SetVerticalOffset(this.VerticalOffset - ((this.Orientation == Orientation.Vertical) ? 1.0 : 16.0));
		}

		// Token: 0x060073CC RID: 29644 RVA: 0x002E4207 File Offset: 0x002E3207
		public void LineDown()
		{
			this.SetVerticalOffset(this.VerticalOffset + ((this.Orientation == Orientation.Vertical) ? 1.0 : 16.0));
		}

		// Token: 0x060073CD RID: 29645 RVA: 0x002E4233 File Offset: 0x002E3233
		public void LineLeft()
		{
			this.SetHorizontalOffset(this.HorizontalOffset - ((this.Orientation == Orientation.Horizontal) ? 1.0 : 16.0));
		}

		// Token: 0x060073CE RID: 29646 RVA: 0x002E425E File Offset: 0x002E325E
		public void LineRight()
		{
			this.SetHorizontalOffset(this.HorizontalOffset + ((this.Orientation == Orientation.Horizontal) ? 1.0 : 16.0));
		}

		// Token: 0x060073CF RID: 29647 RVA: 0x002E4289 File Offset: 0x002E3289
		public void PageUp()
		{
			this.SetVerticalOffset(this.VerticalOffset - this.ViewportHeight);
		}

		// Token: 0x060073D0 RID: 29648 RVA: 0x002E429E File Offset: 0x002E329E
		public void PageDown()
		{
			this.SetVerticalOffset(this.VerticalOffset + this.ViewportHeight);
		}

		// Token: 0x060073D1 RID: 29649 RVA: 0x002E42B3 File Offset: 0x002E32B3
		public void PageLeft()
		{
			this.SetHorizontalOffset(this.HorizontalOffset - this.ViewportWidth);
		}

		// Token: 0x060073D2 RID: 29650 RVA: 0x002E42C8 File Offset: 0x002E32C8
		public void PageRight()
		{
			this.SetHorizontalOffset(this.HorizontalOffset + this.ViewportWidth);
		}

		// Token: 0x060073D3 RID: 29651 RVA: 0x002E42E0 File Offset: 0x002E32E0
		public void MouseWheelUp()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				this.SetVerticalOffset(this.VerticalOffset - (double)SystemParameters.WheelScrollLines * ((this.Orientation == Orientation.Vertical) ? 1.0 : 16.0));
				return;
			}
			this.PageUp();
		}

		// Token: 0x060073D4 RID: 29652 RVA: 0x002E4330 File Offset: 0x002E3330
		public void MouseWheelDown()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				this.SetVerticalOffset(this.VerticalOffset + (double)SystemParameters.WheelScrollLines * ((this.Orientation == Orientation.Vertical) ? 1.0 : 16.0));
				return;
			}
			this.PageDown();
		}

		// Token: 0x060073D5 RID: 29653 RVA: 0x002E437D File Offset: 0x002E337D
		public void MouseWheelLeft()
		{
			this.SetHorizontalOffset(this.HorizontalOffset - 3.0 * ((this.Orientation == Orientation.Horizontal) ? 1.0 : 16.0));
		}

		// Token: 0x060073D6 RID: 29654 RVA: 0x002E43B2 File Offset: 0x002E33B2
		public void MouseWheelRight()
		{
			this.SetHorizontalOffset(this.HorizontalOffset + 3.0 * ((this.Orientation == Orientation.Horizontal) ? 1.0 : 16.0));
		}

		// Token: 0x060073D7 RID: 29655 RVA: 0x002E43E8 File Offset: 0x002E33E8
		public void SetHorizontalOffset(double offset)
		{
			this.EnsureScrollData();
			double num = ScrollContentPresenter.ValidateInputOffset(offset, "HorizontalOffset");
			if (!DoubleUtil.AreClose(num, this._scrollData._offset.X))
			{
				this._scrollData._offset.X = num;
				base.InvalidateMeasure();
			}
		}

		// Token: 0x060073D8 RID: 29656 RVA: 0x002E4438 File Offset: 0x002E3438
		public void SetVerticalOffset(double offset)
		{
			this.EnsureScrollData();
			double num = ScrollContentPresenter.ValidateInputOffset(offset, "VerticalOffset");
			if (!DoubleUtil.AreClose(num, this._scrollData._offset.Y))
			{
				this._scrollData._offset.Y = num;
				base.InvalidateMeasure();
			}
		}

		// Token: 0x060073D9 RID: 29657 RVA: 0x002E4488 File Offset: 0x002E3488
		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			Vector vector = default(Vector);
			Rect result = default(Rect);
			if (rectangle.IsEmpty || visual == null || visual == this || !base.IsAncestorOf(visual))
			{
				return Rect.Empty;
			}
			rectangle = visual.TransformToAncestor(this).TransformBounds(rectangle);
			if (!this.IsScrolling)
			{
				return rectangle;
			}
			this.MakeVisiblePhysicalHelper(rectangle, ref vector, ref result);
			int childIndex = this.FindChildIndexThatParentsVisual(visual);
			this.MakeVisibleLogicalHelper(childIndex, ref vector, ref result);
			vector.X = ScrollContentPresenter.CoerceOffset(vector.X, this._scrollData._extent.Width, this._scrollData._viewport.Width);
			vector.Y = ScrollContentPresenter.CoerceOffset(vector.Y, this._scrollData._extent.Height, this._scrollData._viewport.Height);
			if (!DoubleUtil.AreClose(vector, this._scrollData._offset))
			{
				this._scrollData._offset = vector;
				base.InvalidateMeasure();
				this.OnScrollChange();
			}
			return result;
		}

		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x060073DA RID: 29658 RVA: 0x002E458D File Offset: 0x002E358D
		// (set) Token: 0x060073DB RID: 29659 RVA: 0x002E459F File Offset: 0x002E359F
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(StackPanel.OrientationProperty);
			}
			set
			{
				base.SetValue(StackPanel.OrientationProperty, value);
			}
		}

		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x060073DC RID: 29660 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal override bool HasLogicalOrientation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001ADC RID: 6876
		// (get) Token: 0x060073DD RID: 29661 RVA: 0x002E45B2 File Offset: 0x002E35B2
		protected internal override Orientation LogicalOrientation
		{
			get
			{
				return this.Orientation;
			}
		}

		// Token: 0x17001ADD RID: 6877
		// (get) Token: 0x060073DE RID: 29662 RVA: 0x002E45BA File Offset: 0x002E35BA
		// (set) Token: 0x060073DF RID: 29663 RVA: 0x002E45D1 File Offset: 0x002E35D1
		[DefaultValue(false)]
		public bool CanHorizontallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData._allowHorizontal;
			}
			set
			{
				this.EnsureScrollData();
				if (this._scrollData._allowHorizontal != value)
				{
					this._scrollData._allowHorizontal = value;
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001ADE RID: 6878
		// (get) Token: 0x060073E0 RID: 29664 RVA: 0x002E45F9 File Offset: 0x002E35F9
		// (set) Token: 0x060073E1 RID: 29665 RVA: 0x002E4610 File Offset: 0x002E3610
		[DefaultValue(false)]
		public bool CanVerticallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData._allowVertical;
			}
			set
			{
				this.EnsureScrollData();
				if (this._scrollData._allowVertical != value)
				{
					this._scrollData._allowVertical = value;
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x060073E2 RID: 29666 RVA: 0x002E4638 File Offset: 0x002E3638
		public double ExtentWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._extent.Width;
			}
		}

		// Token: 0x17001AE0 RID: 6880
		// (get) Token: 0x060073E3 RID: 29667 RVA: 0x002E465C File Offset: 0x002E365C
		public double ExtentHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._extent.Height;
			}
		}

		// Token: 0x17001AE1 RID: 6881
		// (get) Token: 0x060073E4 RID: 29668 RVA: 0x002E4680 File Offset: 0x002E3680
		public double ViewportWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._viewport.Width;
			}
		}

		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x060073E5 RID: 29669 RVA: 0x002E46A4 File Offset: 0x002E36A4
		public double ViewportHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._viewport.Height;
			}
		}

		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x060073E6 RID: 29670 RVA: 0x002E46C8 File Offset: 0x002E36C8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double HorizontalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._computedOffset.X;
			}
		}

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x060073E7 RID: 29671 RVA: 0x002E46EC File Offset: 0x002E36EC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double VerticalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._computedOffset.Y;
			}
		}

		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x060073E8 RID: 29672 RVA: 0x002E4710 File Offset: 0x002E3710
		// (set) Token: 0x060073E9 RID: 29673 RVA: 0x002E4723 File Offset: 0x002E3723
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ScrollViewer ScrollOwner
		{
			get
			{
				this.EnsureScrollData();
				return this._scrollData._scrollOwner;
			}
			set
			{
				this.EnsureScrollData();
				if (value != this._scrollData._scrollOwner)
				{
					StackPanel.ResetScrolling(this);
					this._scrollData._scrollOwner = value;
				}
			}
		}

		// Token: 0x060073EA RID: 29674 RVA: 0x002E474C File Offset: 0x002E374C
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = default(Size);
			bool flag = this.IsScrolling && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "STACK:MeasureOverride");
			}
			try
			{
				result = StackPanel.StackMeasureHelper(this, this._scrollData, constraint);
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "STACK:MeasureOverride");
				}
			}
			return result;
		}

		// Token: 0x060073EB RID: 29675 RVA: 0x002E47C4 File Offset: 0x002E37C4
		internal static Size StackMeasureHelper(IStackMeasure measureElement, IStackMeasureScrollData scrollData, Size constraint)
		{
			Size size = default(Size);
			UIElementCollection internalChildren = measureElement.InternalChildren;
			Size availableSize = constraint;
			bool flag = measureElement.Orientation == Orientation.Horizontal;
			int num = -1;
			int i;
			double num2;
			if (flag)
			{
				availableSize.Width = double.PositiveInfinity;
				if (measureElement.IsScrolling && measureElement.CanVerticallyScroll)
				{
					availableSize.Height = double.PositiveInfinity;
				}
				i = (measureElement.IsScrolling ? StackPanel.CoerceOffsetToInteger(scrollData.Offset.X, internalChildren.Count) : 0);
				num2 = constraint.Width;
			}
			else
			{
				availableSize.Height = double.PositiveInfinity;
				if (measureElement.IsScrolling && measureElement.CanHorizontallyScroll)
				{
					availableSize.Width = double.PositiveInfinity;
				}
				i = (measureElement.IsScrolling ? StackPanel.CoerceOffsetToInteger(scrollData.Offset.Y, internalChildren.Count) : 0);
				num2 = constraint.Height;
			}
			int j = 0;
			int count = internalChildren.Count;
			while (j < count)
			{
				UIElement uielement = internalChildren[j];
				if (uielement != null)
				{
					uielement.Measure(availableSize);
					Size desiredSize = uielement.DesiredSize;
					double num3;
					if (flag)
					{
						size.Width += desiredSize.Width;
						size.Height = Math.Max(size.Height, desiredSize.Height);
						num3 = desiredSize.Width;
					}
					else
					{
						size.Width = Math.Max(size.Width, desiredSize.Width);
						size.Height += desiredSize.Height;
						num3 = desiredSize.Height;
					}
					if (measureElement.IsScrolling && num == -1 && j >= i)
					{
						num2 -= num3;
						if (DoubleUtil.LessThanOrClose(num2, 0.0))
						{
							num = j;
						}
					}
				}
				j++;
			}
			if (measureElement.IsScrolling)
			{
				Size viewport = constraint;
				Size extent = size;
				Vector offset = scrollData.Offset;
				if (num == -1)
				{
					num = internalChildren.Count - 1;
				}
				while (i > 0)
				{
					double num4 = num2;
					if (flag)
					{
						num4 -= internalChildren[i - 1].DesiredSize.Width;
					}
					else
					{
						num4 -= internalChildren[i - 1].DesiredSize.Height;
					}
					if (DoubleUtil.LessThan(num4, 0.0))
					{
						break;
					}
					i--;
					num2 = num4;
				}
				int count2 = internalChildren.Count;
				int num5 = num - i;
				if (num5 == 0 || DoubleUtil.GreaterThanOrClose(num2, 0.0))
				{
					num5++;
				}
				if (flag)
				{
					scrollData.SetPhysicalViewport(viewport.Width);
					viewport.Width = (double)num5;
					extent.Width = (double)count2;
					offset.X = (double)i;
					offset.Y = Math.Max(0.0, Math.Min(offset.Y, extent.Height - viewport.Height));
				}
				else
				{
					scrollData.SetPhysicalViewport(viewport.Height);
					viewport.Height = (double)num5;
					extent.Height = (double)count2;
					offset.Y = (double)i;
					offset.X = Math.Max(0.0, Math.Min(offset.X, extent.Width - viewport.Width));
				}
				size.Width = Math.Min(size.Width, constraint.Width);
				size.Height = Math.Min(size.Height, constraint.Height);
				StackPanel.VerifyScrollingData(measureElement, scrollData, viewport, extent, offset);
			}
			return size;
		}

		// Token: 0x060073EC RID: 29676 RVA: 0x002E4B50 File Offset: 0x002E3B50
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.IsScrolling && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "STACK:ArrangeOverride");
			}
			try
			{
				StackPanel.StackArrangeHelper(this, this._scrollData, arrangeSize);
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "STACK:ArrangeOverride");
				}
			}
			return arrangeSize;
		}

		// Token: 0x060073ED RID: 29677 RVA: 0x002E4BC0 File Offset: 0x002E3BC0
		internal static Size StackArrangeHelper(IStackMeasure arrangeElement, IStackMeasureScrollData scrollData, Size arrangeSize)
		{
			UIElementCollection internalChildren = arrangeElement.InternalChildren;
			bool flag = arrangeElement.Orientation == Orientation.Horizontal;
			Rect finalRect = new Rect(arrangeSize);
			double num = 0.0;
			if (arrangeElement.IsScrolling)
			{
				if (flag)
				{
					finalRect.X = StackPanel.ComputePhysicalFromLogicalOffset(arrangeElement, scrollData.ComputedOffset.X, true);
					finalRect.Y = -1.0 * scrollData.ComputedOffset.Y;
				}
				else
				{
					finalRect.X = -1.0 * scrollData.ComputedOffset.X;
					finalRect.Y = StackPanel.ComputePhysicalFromLogicalOffset(arrangeElement, scrollData.ComputedOffset.Y, false);
				}
			}
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					if (flag)
					{
						finalRect.X += num;
						num = uielement.DesiredSize.Width;
						finalRect.Width = num;
						finalRect.Height = Math.Max(arrangeSize.Height, uielement.DesiredSize.Height);
					}
					else
					{
						finalRect.Y += num;
						num = uielement.DesiredSize.Height;
						finalRect.Height = num;
						finalRect.Width = Math.Max(arrangeSize.Width, uielement.DesiredSize.Width);
					}
					uielement.Arrange(finalRect);
				}
				i++;
			}
			return arrangeSize;
		}

		// Token: 0x060073EE RID: 29678 RVA: 0x002E4D4B File Offset: 0x002E3D4B
		private void EnsureScrollData()
		{
			if (this._scrollData == null)
			{
				this._scrollData = new StackPanel.ScrollData();
			}
		}

		// Token: 0x060073EF RID: 29679 RVA: 0x002E4D60 File Offset: 0x002E3D60
		private static void ResetScrolling(StackPanel element)
		{
			element.InvalidateMeasure();
			if (element.IsScrolling)
			{
				element._scrollData.ClearLayout();
			}
		}

		// Token: 0x060073F0 RID: 29680 RVA: 0x002E4D7B File Offset: 0x002E3D7B
		private void OnScrollChange()
		{
			if (this.ScrollOwner != null)
			{
				this.ScrollOwner.InvalidateScrollInfo();
			}
		}

		// Token: 0x060073F1 RID: 29681 RVA: 0x002E4D90 File Offset: 0x002E3D90
		private static void VerifyScrollingData(IStackMeasure measureElement, IStackMeasureScrollData scrollData, Size viewport, Size extent, Vector offset)
		{
			bool flag = true & DoubleUtil.AreClose(viewport, scrollData.Viewport) & DoubleUtil.AreClose(extent, scrollData.Extent) & DoubleUtil.AreClose(offset, scrollData.ComputedOffset);
			scrollData.Offset = offset;
			if (!flag)
			{
				scrollData.Viewport = viewport;
				scrollData.Extent = extent;
				scrollData.ComputedOffset = offset;
				measureElement.OnScrollChange();
			}
		}

		// Token: 0x060073F2 RID: 29682 RVA: 0x002E4DEC File Offset: 0x002E3DEC
		private static double ComputePhysicalFromLogicalOffset(IStackMeasure arrangeElement, double logicalOffset, bool fHorizontal)
		{
			double num = 0.0;
			UIElementCollection internalChildren = arrangeElement.InternalChildren;
			int num2 = 0;
			while ((double)num2 < logicalOffset)
			{
				num -= (fHorizontal ? internalChildren[num2].DesiredSize.Width : internalChildren[num2].DesiredSize.Height);
				num2++;
			}
			return num;
		}

		// Token: 0x060073F3 RID: 29683 RVA: 0x002E4E48 File Offset: 0x002E3E48
		private int FindChildIndexThatParentsVisual(Visual child)
		{
			DependencyObject dependencyObject = child;
			DependencyObject parent = VisualTreeHelper.GetParent(child);
			while (parent != this)
			{
				dependencyObject = parent;
				parent = VisualTreeHelper.GetParent(dependencyObject);
				if (parent == null)
				{
					throw new ArgumentException(SR.Get("Stack_VisualInDifferentSubTree"), "child");
				}
			}
			return base.Children.IndexOf((UIElement)dependencyObject);
		}

		// Token: 0x060073F4 RID: 29684 RVA: 0x002E4E98 File Offset: 0x002E3E98
		private void MakeVisiblePhysicalHelper(Rect r, ref Vector newOffset, ref Rect newRect)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			double num;
			double num2;
			double num3;
			double num4;
			if (flag)
			{
				num = this._scrollData._computedOffset.Y;
				num2 = this.ViewportHeight;
				num3 = r.Y;
				num4 = r.Height;
			}
			else
			{
				num = this._scrollData._computedOffset.X;
				num2 = this.ViewportWidth;
				num3 = r.X;
				num4 = r.Width;
			}
			num3 += num;
			double num5 = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(num, num + num2, num3, num3 + num4);
			double num6 = Math.Max(num3, num5);
			num4 = Math.Max(Math.Min(num4 + num3, num5 + num2) - num6, 0.0);
			num3 = num6;
			num3 -= num;
			if (flag)
			{
				newOffset.Y = num5;
				newRect.Y = num3;
				newRect.Height = num4;
				return;
			}
			newOffset.X = num5;
			newRect.X = num3;
			newRect.Width = num4;
		}

		// Token: 0x060073F5 RID: 29685 RVA: 0x002E4F74 File Offset: 0x002E3F74
		private void MakeVisibleLogicalHelper(int childIndex, ref Vector newOffset, ref Rect newRect)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			double num = 0.0;
			int num2;
			int num3;
			if (flag)
			{
				num2 = (int)this._scrollData._computedOffset.X;
				num3 = (int)this._scrollData._viewport.Width;
			}
			else
			{
				num2 = (int)this._scrollData._computedOffset.Y;
				num3 = (int)this._scrollData._viewport.Height;
			}
			int num4 = num2;
			if (childIndex < num2)
			{
				num4 = childIndex;
			}
			else if (childIndex > num2 + num3 - 1)
			{
				Size desiredSize = base.InternalChildren[childIndex].DesiredSize;
				double num5 = flag ? desiredSize.Width : desiredSize.Height;
				double num6 = this._scrollData._physicalViewport - num5;
				int num7 = childIndex;
				while (num7 > 0 && DoubleUtil.GreaterThanOrClose(num6, 0.0))
				{
					num7--;
					desiredSize = base.InternalChildren[num7].DesiredSize;
					num5 = (flag ? desiredSize.Width : desiredSize.Height);
					num += num5;
					num6 -= num5;
				}
				if (num7 != childIndex && DoubleUtil.LessThan(num6, 0.0))
				{
					num -= num5;
					num7++;
				}
				num4 = num7;
			}
			if (flag)
			{
				newOffset.X = (double)num4;
				newRect.X = num;
				newRect.Width = base.InternalChildren[childIndex].DesiredSize.Width;
				return;
			}
			newOffset.Y = (double)num4;
			newRect.Y = num;
			newRect.Height = base.InternalChildren[childIndex].DesiredSize.Height;
		}

		// Token: 0x060073F6 RID: 29686 RVA: 0x002E5118 File Offset: 0x002E4118
		private static int CoerceOffsetToInteger(double offset, int numberOfItems)
		{
			int num;
			if (double.IsNegativeInfinity(offset))
			{
				num = 0;
			}
			else if (double.IsPositiveInfinity(offset))
			{
				num = numberOfItems - 1;
			}
			else
			{
				num = (int)offset;
				num = Math.Max(Math.Min(numberOfItems - 1, num), 0);
			}
			return num;
		}

		// Token: 0x060073F7 RID: 29687 RVA: 0x002E5153 File Offset: 0x002E4153
		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StackPanel.ResetScrolling(d as StackPanel);
		}

		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x060073F8 RID: 29688 RVA: 0x002E5160 File Offset: 0x002E4160
		private bool IsScrolling
		{
			get
			{
				return this._scrollData != null && this._scrollData._scrollOwner != null;
			}
		}

		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x060073F9 RID: 29689 RVA: 0x001FCA9D File Offset: 0x001FBA9D
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x17001AE8 RID: 6888
		// (get) Token: 0x060073FA RID: 29690 RVA: 0x002E517A File Offset: 0x002E417A
		bool IStackMeasure.IsScrolling
		{
			get
			{
				return this.IsScrolling;
			}
		}

		// Token: 0x17001AE9 RID: 6889
		// (get) Token: 0x060073FB RID: 29691 RVA: 0x002D7272 File Offset: 0x002D6272
		UIElementCollection IStackMeasure.InternalChildren
		{
			get
			{
				return base.InternalChildren;
			}
		}

		// Token: 0x060073FC RID: 29692 RVA: 0x002E5182 File Offset: 0x002E4182
		void IStackMeasure.OnScrollChange()
		{
			this.OnScrollChange();
		}

		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x060073FD RID: 29693 RVA: 0x002E518A File Offset: 0x002E418A
		private bool CanMouseWheelVerticallyScroll
		{
			get
			{
				return SystemParameters.WheelScrollLines > 0;
			}
		}

		// Token: 0x040037EB RID: 14315
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(StackPanel), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(StackPanel.OnOrientationChanged)), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		// Token: 0x040037EC RID: 14316
		private StackPanel.ScrollData _scrollData;

		// Token: 0x02000C21 RID: 3105
		private class ScrollData : IStackMeasureScrollData
		{
			// Token: 0x06009094 RID: 37012 RVA: 0x00346CF8 File Offset: 0x00345CF8
			internal void ClearLayout()
			{
				this._offset = default(Vector);
				this._viewport = (this._extent = default(Size));
				this._physicalViewport = 0.0;
			}

			// Token: 0x17001F98 RID: 8088
			// (get) Token: 0x06009095 RID: 37013 RVA: 0x00346D38 File Offset: 0x00345D38
			// (set) Token: 0x06009096 RID: 37014 RVA: 0x00346D40 File Offset: 0x00345D40
			public Vector Offset
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

			// Token: 0x17001F99 RID: 8089
			// (get) Token: 0x06009097 RID: 37015 RVA: 0x00346D49 File Offset: 0x00345D49
			// (set) Token: 0x06009098 RID: 37016 RVA: 0x00346D51 File Offset: 0x00345D51
			public Size Viewport
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

			// Token: 0x17001F9A RID: 8090
			// (get) Token: 0x06009099 RID: 37017 RVA: 0x00346D5A File Offset: 0x00345D5A
			// (set) Token: 0x0600909A RID: 37018 RVA: 0x00346D62 File Offset: 0x00345D62
			public Size Extent
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

			// Token: 0x17001F9B RID: 8091
			// (get) Token: 0x0600909B RID: 37019 RVA: 0x00346D6B File Offset: 0x00345D6B
			// (set) Token: 0x0600909C RID: 37020 RVA: 0x00346D73 File Offset: 0x00345D73
			public Vector ComputedOffset
			{
				get
				{
					return this._computedOffset;
				}
				set
				{
					this._computedOffset = value;
				}
			}

			// Token: 0x0600909D RID: 37021 RVA: 0x00346D7C File Offset: 0x00345D7C
			public void SetPhysicalViewport(double value)
			{
				this._physicalViewport = value;
			}

			// Token: 0x04004B2B RID: 19243
			internal bool _allowHorizontal;

			// Token: 0x04004B2C RID: 19244
			internal bool _allowVertical;

			// Token: 0x04004B2D RID: 19245
			internal Vector _offset;

			// Token: 0x04004B2E RID: 19246
			internal Vector _computedOffset = new Vector(0.0, 0.0);

			// Token: 0x04004B2F RID: 19247
			internal Size _viewport;

			// Token: 0x04004B30 RID: 19248
			internal Size _extent;

			// Token: 0x04004B31 RID: 19249
			internal double _physicalViewport;

			// Token: 0x04004B32 RID: 19250
			internal ScrollViewer _scrollOwner;
		}
	}
}
