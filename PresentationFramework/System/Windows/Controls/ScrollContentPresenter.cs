using System;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal;
using MS.Utility;

namespace System.Windows.Controls
{
	// Token: 0x020007C2 RID: 1986
	public sealed class ScrollContentPresenter : ContentPresenter, IScrollInfo
	{
		// Token: 0x06007187 RID: 29063 RVA: 0x002DAB5C File Offset: 0x002D9B5C
		public ScrollContentPresenter()
		{
			this._adornerLayer = new AdornerLayer();
		}

		// Token: 0x06007188 RID: 29064 RVA: 0x002DAB6F File Offset: 0x002D9B6F
		public void LineUp()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset - 16.0);
			}
		}

		// Token: 0x06007189 RID: 29065 RVA: 0x002DAB8F File Offset: 0x002D9B8F
		public void LineDown()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset + 16.0);
			}
		}

		// Token: 0x0600718A RID: 29066 RVA: 0x002DABAF File Offset: 0x002D9BAF
		public void LineLeft()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset - 16.0);
			}
		}

		// Token: 0x0600718B RID: 29067 RVA: 0x002DABCF File Offset: 0x002D9BCF
		public void LineRight()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset + 16.0);
			}
		}

		// Token: 0x0600718C RID: 29068 RVA: 0x002DABEF File Offset: 0x002D9BEF
		public void PageUp()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset - this.ViewportHeight);
			}
		}

		// Token: 0x0600718D RID: 29069 RVA: 0x002DAC0C File Offset: 0x002D9C0C
		public void PageDown()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset + this.ViewportHeight);
			}
		}

		// Token: 0x0600718E RID: 29070 RVA: 0x002DAC29 File Offset: 0x002D9C29
		public void PageLeft()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset - this.ViewportWidth);
			}
		}

		// Token: 0x0600718F RID: 29071 RVA: 0x002DAC46 File Offset: 0x002D9C46
		public void PageRight()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset + this.ViewportWidth);
			}
		}

		// Token: 0x06007190 RID: 29072 RVA: 0x002DAC63 File Offset: 0x002D9C63
		public void MouseWheelUp()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset - 48.0);
			}
		}

		// Token: 0x06007191 RID: 29073 RVA: 0x002DAC83 File Offset: 0x002D9C83
		public void MouseWheelDown()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset + 48.0);
			}
		}

		// Token: 0x06007192 RID: 29074 RVA: 0x002DACA3 File Offset: 0x002D9CA3
		public void MouseWheelLeft()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset - 48.0);
			}
		}

		// Token: 0x06007193 RID: 29075 RVA: 0x002DACC3 File Offset: 0x002D9CC3
		public void MouseWheelRight()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset + 48.0);
			}
		}

		// Token: 0x06007194 RID: 29076 RVA: 0x002DACE4 File Offset: 0x002D9CE4
		public void SetHorizontalOffset(double offset)
		{
			if (this.IsScrollClient)
			{
				double num = ScrollContentPresenter.ValidateInputOffset(offset, "HorizontalOffset");
				if (!DoubleUtil.AreClose(this.EnsureScrollData()._offset.X, num))
				{
					this._scrollData._offset.X = num;
					base.InvalidateArrange();
				}
			}
		}

		// Token: 0x06007195 RID: 29077 RVA: 0x002DAD34 File Offset: 0x002D9D34
		public void SetVerticalOffset(double offset)
		{
			if (this.IsScrollClient)
			{
				double num = ScrollContentPresenter.ValidateInputOffset(offset, "VerticalOffset");
				if (!DoubleUtil.AreClose(this.EnsureScrollData()._offset.Y, num))
				{
					this._scrollData._offset.Y = num;
					base.InvalidateArrange();
				}
			}
		}

		// Token: 0x06007196 RID: 29078 RVA: 0x002DAD84 File Offset: 0x002D9D84
		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			return this.MakeVisible(visual, rectangle, true);
		}

		// Token: 0x17001A47 RID: 6727
		// (get) Token: 0x06007197 RID: 29079 RVA: 0x002DAD8F File Offset: 0x002D9D8F
		public AdornerLayer AdornerLayer
		{
			get
			{
				return this._adornerLayer;
			}
		}

		// Token: 0x17001A48 RID: 6728
		// (get) Token: 0x06007198 RID: 29080 RVA: 0x002DAD97 File Offset: 0x002D9D97
		// (set) Token: 0x06007199 RID: 29081 RVA: 0x002DADA9 File Offset: 0x002D9DA9
		public bool CanContentScroll
		{
			get
			{
				return (bool)base.GetValue(ScrollContentPresenter.CanContentScrollProperty);
			}
			set
			{
				base.SetValue(ScrollContentPresenter.CanContentScrollProperty, value);
			}
		}

		// Token: 0x17001A49 RID: 6729
		// (get) Token: 0x0600719A RID: 29082 RVA: 0x002DADB7 File Offset: 0x002D9DB7
		// (set) Token: 0x0600719B RID: 29083 RVA: 0x002DADCE File Offset: 0x002D9DCE
		public bool CanHorizontallyScroll
		{
			get
			{
				return this.IsScrollClient && this.EnsureScrollData()._canHorizontallyScroll;
			}
			set
			{
				if (this.IsScrollClient && this.EnsureScrollData()._canHorizontallyScroll != value)
				{
					this._scrollData._canHorizontallyScroll = value;
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001A4A RID: 6730
		// (get) Token: 0x0600719C RID: 29084 RVA: 0x002DADF8 File Offset: 0x002D9DF8
		// (set) Token: 0x0600719D RID: 29085 RVA: 0x002DAE0F File Offset: 0x002D9E0F
		public bool CanVerticallyScroll
		{
			get
			{
				return this.IsScrollClient && this.EnsureScrollData()._canVerticallyScroll;
			}
			set
			{
				if (this.IsScrollClient && this.EnsureScrollData()._canVerticallyScroll != value)
				{
					this._scrollData._canVerticallyScroll = value;
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001A4B RID: 6731
		// (get) Token: 0x0600719E RID: 29086 RVA: 0x002DAE39 File Offset: 0x002D9E39
		public double ExtentWidth
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._extent.Width;
			}
		}

		// Token: 0x17001A4C RID: 6732
		// (get) Token: 0x0600719F RID: 29087 RVA: 0x002DAE5D File Offset: 0x002D9E5D
		public double ExtentHeight
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._extent.Height;
			}
		}

		// Token: 0x17001A4D RID: 6733
		// (get) Token: 0x060071A0 RID: 29088 RVA: 0x002DAE81 File Offset: 0x002D9E81
		public double ViewportWidth
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._viewport.Width;
			}
		}

		// Token: 0x17001A4E RID: 6734
		// (get) Token: 0x060071A1 RID: 29089 RVA: 0x002DAEA5 File Offset: 0x002D9EA5
		public double ViewportHeight
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._viewport.Height;
			}
		}

		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x060071A2 RID: 29090 RVA: 0x002DAEC9 File Offset: 0x002D9EC9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double HorizontalOffset
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._computedOffset.X;
			}
		}

		// Token: 0x17001A50 RID: 6736
		// (get) Token: 0x060071A3 RID: 29091 RVA: 0x002DAEED File Offset: 0x002D9EED
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double VerticalOffset
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._computedOffset.Y;
			}
		}

		// Token: 0x17001A51 RID: 6737
		// (get) Token: 0x060071A4 RID: 29092 RVA: 0x002DAF11 File Offset: 0x002D9F11
		// (set) Token: 0x060071A5 RID: 29093 RVA: 0x002DAF28 File Offset: 0x002D9F28
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ScrollViewer ScrollOwner
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return null;
				}
				return this._scrollData._scrollOwner;
			}
			set
			{
				if (this.IsScrollClient)
				{
					this._scrollData._scrollOwner = value;
				}
			}
		}

		// Token: 0x17001A52 RID: 6738
		// (get) Token: 0x060071A6 RID: 29094 RVA: 0x002DAF3E File Offset: 0x002D9F3E
		protected override int VisualChildrenCount
		{
			get
			{
				if (base.TemplateChild != null)
				{
					return 2;
				}
				return 0;
			}
		}

		// Token: 0x060071A7 RID: 29095 RVA: 0x002DAF4C File Offset: 0x002D9F4C
		protected override Visual GetVisualChild(int index)
		{
			if (base.TemplateChild == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index == 0)
			{
				return base.TemplateChild;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._adornerLayer;
		}

		// Token: 0x17001A53 RID: 6739
		// (get) Token: 0x060071A8 RID: 29096 RVA: 0x002DAFAD File Offset: 0x002D9FAD
		// (set) Token: 0x060071A9 RID: 29097 RVA: 0x002DAFB8 File Offset: 0x002D9FB8
		internal override UIElement TemplateChild
		{
			get
			{
				return base.TemplateChild;
			}
			set
			{
				UIElement templateChild = base.TemplateChild;
				if (value != templateChild)
				{
					if (templateChild != null && value == null)
					{
						base.RemoveVisualChild(this._adornerLayer);
					}
					base.TemplateChild = value;
					if (templateChild == null && value != null)
					{
						base.AddVisualChild(this._adornerLayer);
					}
				}
			}
		}

		// Token: 0x060071AA RID: 29098 RVA: 0x002DAFFC File Offset: 0x002D9FFC
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = default(Size);
			bool flag = this.IsScrollClient && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:MeasureOverride");
			}
			try
			{
				if (this.VisualChildrenCount > 0)
				{
					this._adornerLayer.Measure(constraint);
					if (!this.IsScrollClient)
					{
						size = base.MeasureOverride(constraint);
					}
					else
					{
						Size constraint2 = constraint;
						if (this._scrollData._canHorizontallyScroll)
						{
							constraint2.Width = double.PositiveInfinity;
						}
						if (this._scrollData._canVerticallyScroll)
						{
							constraint2.Height = double.PositiveInfinity;
						}
						size = base.MeasureOverride(constraint2);
					}
				}
				if (this.IsScrollClient)
				{
					this.VerifyScrollData(constraint, size);
				}
				size.Width = Math.Min(constraint.Width, size.Width);
				size.Height = Math.Min(constraint.Height, size.Height);
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:MeasureOverride");
				}
			}
			return size;
		}

		// Token: 0x060071AB RID: 29099 RVA: 0x002DB114 File Offset: 0x002DA114
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.IsScrollClient && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:ArrangeOverride");
			}
			try
			{
				int visualChildrenCount = this.VisualChildrenCount;
				if (this.IsScrollClient)
				{
					this.VerifyScrollData(arrangeSize, this._scrollData._extent);
				}
				if (visualChildrenCount > 0)
				{
					this._adornerLayer.Arrange(new Rect(arrangeSize));
					UIElement uielement = this.GetVisualChild(0) as UIElement;
					if (uielement != null)
					{
						Rect finalRect = new Rect(uielement.DesiredSize);
						if (this.IsScrollClient)
						{
							finalRect.X = -this.HorizontalOffset;
							finalRect.Y = -this.VerticalOffset;
						}
						finalRect.Width = Math.Max(finalRect.Width, arrangeSize.Width);
						finalRect.Height = Math.Max(finalRect.Height, arrangeSize.Height);
						uielement.Arrange(finalRect);
					}
				}
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:ArrangeOverride");
				}
			}
			return arrangeSize;
		}

		// Token: 0x060071AC RID: 29100 RVA: 0x002DB228 File Offset: 0x002DA228
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			return new RectangleGeometry(new Rect(base.RenderSize));
		}

		// Token: 0x060071AD RID: 29101 RVA: 0x002DB23A File Offset: 0x002DA23A
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.HookupScrollingComponents();
		}

		// Token: 0x060071AE RID: 29102 RVA: 0x002DB248 File Offset: 0x002DA248
		internal Rect MakeVisible(Visual visual, Rect rectangle, bool throwOnError)
		{
			if (rectangle.IsEmpty || visual == null || visual == this || !base.IsAncestorOf(visual))
			{
				return Rect.Empty;
			}
			rectangle = visual.TransformToAncestor(this).TransformBounds(rectangle);
			if (!this.IsScrollClient || (!throwOnError && rectangle.IsEmpty))
			{
				return rectangle;
			}
			Rect rect = new Rect(this.HorizontalOffset, this.VerticalOffset, this.ViewportWidth, this.ViewportHeight);
			rectangle.X += rect.X;
			rectangle.Y += rect.Y;
			double num = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect.Left, rect.Right, rectangle.Left, rectangle.Right);
			double num2 = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect.Top, rect.Bottom, rectangle.Top, rectangle.Bottom);
			this.SetHorizontalOffset(num);
			this.SetVerticalOffset(num2);
			rect.X = num;
			rect.Y = num2;
			rectangle.Intersect(rect);
			if (throwOnError)
			{
				rectangle.X -= rect.X;
				rectangle.Y -= rect.Y;
			}
			else if (!rectangle.IsEmpty)
			{
				rectangle.X -= rect.X;
				rectangle.Y -= rect.Y;
			}
			return rectangle;
		}

		// Token: 0x060071AF RID: 29103 RVA: 0x002DB3B0 File Offset: 0x002DA3B0
		internal static double ComputeScrollOffsetWithMinimalScroll(double topView, double bottomView, double topChild, double bottomChild)
		{
			bool flag = false;
			bool flag2 = false;
			return ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(topView, bottomView, topChild, bottomChild, ref flag, ref flag2);
		}

		// Token: 0x060071B0 RID: 29104 RVA: 0x002DB3D0 File Offset: 0x002DA3D0
		internal static double ComputeScrollOffsetWithMinimalScroll(double topView, double bottomView, double topChild, double bottomChild, ref bool alignTop, ref bool alignBottom)
		{
			bool flag = DoubleUtil.LessThan(topChild, topView) && DoubleUtil.LessThan(bottomChild, bottomView);
			bool flag2 = DoubleUtil.GreaterThan(bottomChild, bottomView) && DoubleUtil.GreaterThan(topChild, topView);
			bool flag3 = bottomChild - topChild > bottomView - topView;
			if (((flag && !flag3) || (flag2 && flag3)) | alignTop)
			{
				alignTop = true;
				return topChild;
			}
			if ((flag || flag2) | alignBottom)
			{
				alignBottom = true;
				return bottomChild - (bottomView - topView);
			}
			return topView;
		}

		// Token: 0x060071B1 RID: 29105 RVA: 0x002DB43A File Offset: 0x002DA43A
		internal static double ValidateInputOffset(double offset, string parameterName)
		{
			if (DoubleUtil.IsNaN(offset))
			{
				throw new ArgumentOutOfRangeException(parameterName, SR.Get("ScrollViewer_CannotBeNaN", new object[]
				{
					parameterName
				}));
			}
			return Math.Max(0.0, offset);
		}

		// Token: 0x060071B2 RID: 29106 RVA: 0x002DB46E File Offset: 0x002DA46E
		private ScrollContentPresenter.ScrollData EnsureScrollData()
		{
			if (this._scrollData == null)
			{
				this._scrollData = new ScrollContentPresenter.ScrollData();
			}
			return this._scrollData;
		}

		// Token: 0x060071B3 RID: 29107 RVA: 0x002DB48C File Offset: 0x002DA48C
		internal void HookupScrollingComponents()
		{
			ScrollViewer scrollViewer = base.TemplatedParent as ScrollViewer;
			if (scrollViewer != null)
			{
				IScrollInfo scrollInfo = null;
				if (this.CanContentScroll)
				{
					scrollInfo = (base.Content as IScrollInfo);
					if (scrollInfo == null)
					{
						Visual visual = base.Content as Visual;
						if (visual != null)
						{
							ItemsPresenter itemsPresenter = visual as ItemsPresenter;
							if (itemsPresenter == null)
							{
								FrameworkElement frameworkElement = scrollViewer.TemplatedParent as FrameworkElement;
								if (frameworkElement != null)
								{
									itemsPresenter = (frameworkElement.GetTemplateChild("ItemsPresenter") as ItemsPresenter);
								}
							}
							if (itemsPresenter != null)
							{
								itemsPresenter.ApplyTemplate();
								if (VisualTreeHelper.GetChildrenCount(itemsPresenter) > 0)
								{
									scrollInfo = (VisualTreeHelper.GetChild(itemsPresenter, 0) as IScrollInfo);
								}
							}
						}
					}
				}
				if (scrollInfo == null)
				{
					scrollInfo = this;
					this.EnsureScrollData();
				}
				if (scrollInfo != this._scrollInfo && this._scrollInfo != null)
				{
					if (this.IsScrollClient)
					{
						this._scrollData = null;
					}
					else
					{
						this._scrollInfo.ScrollOwner = null;
					}
				}
				if (scrollInfo != null)
				{
					this._scrollInfo = scrollInfo;
					scrollInfo.ScrollOwner = scrollViewer;
					scrollViewer.ScrollInfo = scrollInfo;
					return;
				}
			}
			else if (this._scrollInfo != null)
			{
				if (this._scrollInfo.ScrollOwner != null)
				{
					this._scrollInfo.ScrollOwner.ScrollInfo = null;
				}
				this._scrollInfo.ScrollOwner = null;
				this._scrollInfo = null;
				this._scrollData = null;
			}
		}

		// Token: 0x060071B4 RID: 29108 RVA: 0x002DB5B4 File Offset: 0x002DA5B4
		private void VerifyScrollData(Size viewport, Size extent)
		{
			bool flag = true;
			if (double.IsInfinity(viewport.Width))
			{
				viewport.Width = extent.Width;
			}
			if (double.IsInfinity(viewport.Height))
			{
				viewport.Height = extent.Height;
			}
			bool flag2 = flag & DoubleUtil.AreClose(viewport, this._scrollData._viewport) & DoubleUtil.AreClose(extent, this._scrollData._extent);
			this._scrollData._viewport = viewport;
			this._scrollData._extent = extent;
			if (!(flag2 & this.CoerceOffsets()))
			{
				this.ScrollOwner.InvalidateScrollInfo();
			}
		}

		// Token: 0x060071B5 RID: 29109 RVA: 0x002DB64A File Offset: 0x002DA64A
		internal static double CoerceOffset(double offset, double extent, double viewport)
		{
			if (offset > extent - viewport)
			{
				offset = extent - viewport;
			}
			if (offset < 0.0)
			{
				offset = 0.0;
			}
			return offset;
		}

		// Token: 0x060071B6 RID: 29110 RVA: 0x002DB670 File Offset: 0x002DA670
		private bool CoerceOffsets()
		{
			Vector vector = new Vector(ScrollContentPresenter.CoerceOffset(this._scrollData._offset.X, this._scrollData._extent.Width, this._scrollData._viewport.Width), ScrollContentPresenter.CoerceOffset(this._scrollData._offset.Y, this._scrollData._extent.Height, this._scrollData._viewport.Height));
			bool result = DoubleUtil.AreClose(this._scrollData._computedOffset, vector);
			this._scrollData._computedOffset = vector;
			return result;
		}

		// Token: 0x060071B7 RID: 29111 RVA: 0x002DB70C File Offset: 0x002DA70C
		private static void OnCanContentScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollContentPresenter scrollContentPresenter = (ScrollContentPresenter)d;
			if (scrollContentPresenter._scrollInfo == null)
			{
				return;
			}
			scrollContentPresenter.HookupScrollingComponents();
			scrollContentPresenter.InvalidateMeasure();
		}

		// Token: 0x17001A54 RID: 6740
		// (get) Token: 0x060071B8 RID: 29112 RVA: 0x002DB735 File Offset: 0x002DA735
		private bool IsScrollClient
		{
			get
			{
				return this._scrollInfo == this;
			}
		}

		// Token: 0x17001A55 RID: 6741
		// (get) Token: 0x060071B9 RID: 29113 RVA: 0x001FCA42 File Offset: 0x001FBA42
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x04003730 RID: 14128
		public static readonly DependencyProperty CanContentScrollProperty = ScrollViewer.CanContentScrollProperty.AddOwner(typeof(ScrollContentPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(ScrollContentPresenter.OnCanContentScrollChanged)));

		// Token: 0x04003731 RID: 14129
		private IScrollInfo _scrollInfo;

		// Token: 0x04003732 RID: 14130
		private ScrollContentPresenter.ScrollData _scrollData;

		// Token: 0x04003733 RID: 14131
		private readonly AdornerLayer _adornerLayer;

		// Token: 0x02000C13 RID: 3091
		private class ScrollData
		{
			// Token: 0x04004AE3 RID: 19171
			internal ScrollViewer _scrollOwner;

			// Token: 0x04004AE4 RID: 19172
			internal bool _canHorizontallyScroll;

			// Token: 0x04004AE5 RID: 19173
			internal bool _canVerticallyScroll;

			// Token: 0x04004AE6 RID: 19174
			internal Vector _offset;

			// Token: 0x04004AE7 RID: 19175
			internal Vector _computedOffset;

			// Token: 0x04004AE8 RID: 19176
			internal Size _viewport;

			// Token: 0x04004AE9 RID: 19177
			internal Size _extent;
		}
	}
}
