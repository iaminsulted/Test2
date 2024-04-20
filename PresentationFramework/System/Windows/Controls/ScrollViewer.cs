using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	// Token: 0x020007CD RID: 1997
	[TemplatePart(Name = "PART_HorizontalScrollBar", Type = typeof(ScrollBar))]
	[Localizability(LocalizationCategory.Ignore)]
	[DefaultEvent("ScrollChangedEvent")]
	[TemplatePart(Name = "PART_VerticalScrollBar", Type = typeof(ScrollBar))]
	[TemplatePart(Name = "PART_ScrollContentPresenter", Type = typeof(ScrollContentPresenter))]
	public class ScrollViewer : ContentControl
	{
		// Token: 0x06007239 RID: 29241 RVA: 0x002DD370 File Offset: 0x002DC370
		public void LineUp()
		{
			this.EnqueueCommand(ScrollViewer.Commands.LineUp, 0.0, null);
		}

		// Token: 0x0600723A RID: 29242 RVA: 0x002DD383 File Offset: 0x002DC383
		public void LineDown()
		{
			this.EnqueueCommand(ScrollViewer.Commands.LineDown, 0.0, null);
		}

		// Token: 0x0600723B RID: 29243 RVA: 0x002DD396 File Offset: 0x002DC396
		public void LineLeft()
		{
			this.EnqueueCommand(ScrollViewer.Commands.LineLeft, 0.0, null);
		}

		// Token: 0x0600723C RID: 29244 RVA: 0x002DD3A9 File Offset: 0x002DC3A9
		public void LineRight()
		{
			this.EnqueueCommand(ScrollViewer.Commands.LineRight, 0.0, null);
		}

		// Token: 0x0600723D RID: 29245 RVA: 0x002DD3BC File Offset: 0x002DC3BC
		public void PageUp()
		{
			this.EnqueueCommand(ScrollViewer.Commands.PageUp, 0.0, null);
		}

		// Token: 0x0600723E RID: 29246 RVA: 0x002DD3CF File Offset: 0x002DC3CF
		public void PageDown()
		{
			this.EnqueueCommand(ScrollViewer.Commands.PageDown, 0.0, null);
		}

		// Token: 0x0600723F RID: 29247 RVA: 0x002DD3E2 File Offset: 0x002DC3E2
		public void PageLeft()
		{
			this.EnqueueCommand(ScrollViewer.Commands.PageLeft, 0.0, null);
		}

		// Token: 0x06007240 RID: 29248 RVA: 0x002DD3F5 File Offset: 0x002DC3F5
		public void PageRight()
		{
			this.EnqueueCommand(ScrollViewer.Commands.PageRight, 0.0, null);
		}

		// Token: 0x06007241 RID: 29249 RVA: 0x002DD408 File Offset: 0x002DC408
		public void ScrollToLeftEnd()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, double.NegativeInfinity, null);
		}

		// Token: 0x06007242 RID: 29250 RVA: 0x002DD41C File Offset: 0x002DC41C
		public void ScrollToRightEnd()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, double.PositiveInfinity, null);
		}

		// Token: 0x06007243 RID: 29251 RVA: 0x002DD430 File Offset: 0x002DC430
		public void ScrollToHome()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, double.NegativeInfinity, null);
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, double.NegativeInfinity, null);
		}

		// Token: 0x06007244 RID: 29252 RVA: 0x002DD456 File Offset: 0x002DC456
		public void ScrollToEnd()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, double.NegativeInfinity, null);
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, double.PositiveInfinity, null);
		}

		// Token: 0x06007245 RID: 29253 RVA: 0x002DD47C File Offset: 0x002DC47C
		public void ScrollToTop()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, double.NegativeInfinity, null);
		}

		// Token: 0x06007246 RID: 29254 RVA: 0x002DD490 File Offset: 0x002DC490
		public void ScrollToBottom()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, double.PositiveInfinity, null);
		}

		// Token: 0x06007247 RID: 29255 RVA: 0x002DD4A4 File Offset: 0x002DC4A4
		public void ScrollToHorizontalOffset(double offset)
		{
			double param = ScrollContentPresenter.ValidateInputOffset(offset, "offset");
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, param, null);
		}

		// Token: 0x06007248 RID: 29256 RVA: 0x002DD4C8 File Offset: 0x002DC4C8
		public void ScrollToVerticalOffset(double offset)
		{
			double param = ScrollContentPresenter.ValidateInputOffset(offset, "offset");
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, param, null);
		}

		// Token: 0x06007249 RID: 29257 RVA: 0x002DD4EC File Offset: 0x002DC4EC
		private void DeferScrollToHorizontalOffset(double offset)
		{
			double horizontalOffset = ScrollContentPresenter.ValidateInputOffset(offset, "offset");
			this.HorizontalOffset = horizontalOffset;
		}

		// Token: 0x0600724A RID: 29258 RVA: 0x002DD50C File Offset: 0x002DC50C
		private void DeferScrollToVerticalOffset(double offset)
		{
			double verticalOffset = ScrollContentPresenter.ValidateInputOffset(offset, "offset");
			this.VerticalOffset = verticalOffset;
		}

		// Token: 0x0600724B RID: 29259 RVA: 0x002DD52C File Offset: 0x002DC52C
		internal void MakeVisible(Visual child, Rect rect)
		{
			ScrollViewer.MakeVisibleParams mvp = new ScrollViewer.MakeVisibleParams(child, rect);
			this.EnqueueCommand(ScrollViewer.Commands.MakeVisible, 0.0, mvp);
		}

		// Token: 0x0600724C RID: 29260 RVA: 0x002DD553 File Offset: 0x002DC553
		private void EnsureLayoutUpdatedHandler()
		{
			if (this._layoutUpdatedHandler == null)
			{
				this._layoutUpdatedHandler = new EventHandler(this.OnLayoutUpdated);
				base.LayoutUpdated += this._layoutUpdatedHandler;
			}
			base.InvalidateArrange();
		}

		// Token: 0x0600724D RID: 29261 RVA: 0x002DD581 File Offset: 0x002DC581
		private void ClearLayoutUpdatedHandler()
		{
			if (this._layoutUpdatedHandler != null && this._queue.IsEmpty())
			{
				base.LayoutUpdated -= this._layoutUpdatedHandler;
				this._layoutUpdatedHandler = null;
			}
		}

		// Token: 0x0600724E RID: 29262 RVA: 0x002DD5AC File Offset: 0x002DC5AC
		public void InvalidateScrollInfo()
		{
			if (this.ScrollInfo == null)
			{
				return;
			}
			if (!base.MeasureInProgress && (!base.ArrangeInProgress || !this.InvalidatedMeasureFromArrange))
			{
				double value = this.ScrollInfo.ExtentWidth;
				double value2 = this.ScrollInfo.ViewportWidth;
				if (this.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto && ((this._scrollVisibilityX == Visibility.Collapsed && DoubleUtil.GreaterThan(value, value2)) || (this._scrollVisibilityX == Visibility.Visible && DoubleUtil.LessThanOrClose(value, value2))))
				{
					base.InvalidateMeasure();
				}
				else
				{
					value = this.ScrollInfo.ExtentHeight;
					value2 = this.ScrollInfo.ViewportHeight;
					if (this.VerticalScrollBarVisibility == ScrollBarVisibility.Auto && ((this._scrollVisibilityY == Visibility.Collapsed && DoubleUtil.GreaterThan(value, value2)) || (this._scrollVisibilityY == Visibility.Visible && DoubleUtil.LessThanOrClose(value, value2))))
					{
						base.InvalidateMeasure();
					}
				}
			}
			if (!DoubleUtil.AreClose(this.HorizontalOffset, this.ScrollInfo.HorizontalOffset) || !DoubleUtil.AreClose(this.VerticalOffset, this.ScrollInfo.VerticalOffset) || !DoubleUtil.AreClose(this.ViewportWidth, this.ScrollInfo.ViewportWidth) || !DoubleUtil.AreClose(this.ViewportHeight, this.ScrollInfo.ViewportHeight) || !DoubleUtil.AreClose(this.ExtentWidth, this.ScrollInfo.ExtentWidth) || !DoubleUtil.AreClose(this.ExtentHeight, this.ScrollInfo.ExtentHeight))
			{
				this.EnsureLayoutUpdatedHandler();
			}
		}

		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x0600724F RID: 29263 RVA: 0x002DD70C File Offset: 0x002DC70C
		// (set) Token: 0x06007250 RID: 29264 RVA: 0x002DD71E File Offset: 0x002DC71E
		public bool CanContentScroll
		{
			get
			{
				return (bool)base.GetValue(ScrollViewer.CanContentScrollProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.CanContentScrollProperty, value);
			}
		}

		// Token: 0x17001A7C RID: 6780
		// (get) Token: 0x06007251 RID: 29265 RVA: 0x002DD72C File Offset: 0x002DC72C
		// (set) Token: 0x06007252 RID: 29266 RVA: 0x002DD73E File Offset: 0x002DC73E
		[Bindable(true)]
		[Category("Appearance")]
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, value);
			}
		}

		// Token: 0x17001A7D RID: 6781
		// (get) Token: 0x06007253 RID: 29267 RVA: 0x002DD751 File Offset: 0x002DC751
		// (set) Token: 0x06007254 RID: 29268 RVA: 0x002DD763 File Offset: 0x002DC763
		[Bindable(true)]
		[Category("Appearance")]
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(ScrollViewer.VerticalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, value);
			}
		}

		// Token: 0x17001A7E RID: 6782
		// (get) Token: 0x06007255 RID: 29269 RVA: 0x002DD776 File Offset: 0x002DC776
		public Visibility ComputedHorizontalScrollBarVisibility
		{
			get
			{
				return this._scrollVisibilityX;
			}
		}

		// Token: 0x17001A7F RID: 6783
		// (get) Token: 0x06007256 RID: 29270 RVA: 0x002DD77E File Offset: 0x002DC77E
		public Visibility ComputedVerticalScrollBarVisibility
		{
			get
			{
				return this._scrollVisibilityY;
			}
		}

		// Token: 0x17001A80 RID: 6784
		// (get) Token: 0x06007257 RID: 29271 RVA: 0x002DD786 File Offset: 0x002DC786
		// (set) Token: 0x06007258 RID: 29272 RVA: 0x002DD78E File Offset: 0x002DC78E
		public double HorizontalOffset
		{
			get
			{
				return this._xPositionISI;
			}
			private set
			{
				base.SetValue(ScrollViewer.HorizontalOffsetPropertyKey, value);
			}
		}

		// Token: 0x17001A81 RID: 6785
		// (get) Token: 0x06007259 RID: 29273 RVA: 0x002DD7A1 File Offset: 0x002DC7A1
		// (set) Token: 0x0600725A RID: 29274 RVA: 0x002DD7A9 File Offset: 0x002DC7A9
		public double VerticalOffset
		{
			get
			{
				return this._yPositionISI;
			}
			private set
			{
				base.SetValue(ScrollViewer.VerticalOffsetPropertyKey, value);
			}
		}

		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x0600725B RID: 29275 RVA: 0x002DD7BC File Offset: 0x002DC7BC
		[Category("Layout")]
		public double ExtentWidth
		{
			get
			{
				return this._xExtent;
			}
		}

		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x0600725C RID: 29276 RVA: 0x002DD7C4 File Offset: 0x002DC7C4
		[Category("Layout")]
		public double ExtentHeight
		{
			get
			{
				return this._yExtent;
			}
		}

		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x0600725D RID: 29277 RVA: 0x002DD7CC File Offset: 0x002DC7CC
		public double ScrollableWidth
		{
			get
			{
				return Math.Max(0.0, this.ExtentWidth - this.ViewportWidth);
			}
		}

		// Token: 0x17001A85 RID: 6789
		// (get) Token: 0x0600725E RID: 29278 RVA: 0x002DD7E9 File Offset: 0x002DC7E9
		public double ScrollableHeight
		{
			get
			{
				return Math.Max(0.0, this.ExtentHeight - this.ViewportHeight);
			}
		}

		// Token: 0x17001A86 RID: 6790
		// (get) Token: 0x0600725F RID: 29279 RVA: 0x002DD806 File Offset: 0x002DC806
		[Category("Layout")]
		public double ViewportWidth
		{
			get
			{
				return this._xSize;
			}
		}

		// Token: 0x17001A87 RID: 6791
		// (get) Token: 0x06007260 RID: 29280 RVA: 0x002DD80E File Offset: 0x002DC80E
		[Category("Layout")]
		public double ViewportHeight
		{
			get
			{
				return this._ySize;
			}
		}

		// Token: 0x06007261 RID: 29281 RVA: 0x002DD816 File Offset: 0x002DC816
		public static void SetCanContentScroll(DependencyObject element, bool canContentScroll)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.CanContentScrollProperty, canContentScroll);
		}

		// Token: 0x06007262 RID: 29282 RVA: 0x002DD832 File Offset: 0x002DC832
		public static bool GetCanContentScroll(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ScrollViewer.CanContentScrollProperty);
		}

		// Token: 0x06007263 RID: 29283 RVA: 0x002DD852 File Offset: 0x002DC852
		public static void SetHorizontalScrollBarVisibility(DependencyObject element, ScrollBarVisibility horizontalScrollBarVisibility)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, horizontalScrollBarVisibility);
		}

		// Token: 0x06007264 RID: 29284 RVA: 0x002DD873 File Offset: 0x002DC873
		public static ScrollBarVisibility GetHorizontalScrollBarVisibility(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (ScrollBarVisibility)element.GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty);
		}

		// Token: 0x06007265 RID: 29285 RVA: 0x002DD893 File Offset: 0x002DC893
		public static void SetVerticalScrollBarVisibility(DependencyObject element, ScrollBarVisibility verticalScrollBarVisibility)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, verticalScrollBarVisibility);
		}

		// Token: 0x06007266 RID: 29286 RVA: 0x002DD8B4 File Offset: 0x002DC8B4
		public static ScrollBarVisibility GetVerticalScrollBarVisibility(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (ScrollBarVisibility)element.GetValue(ScrollViewer.VerticalScrollBarVisibilityProperty);
		}

		// Token: 0x17001A88 RID: 6792
		// (get) Token: 0x06007267 RID: 29287 RVA: 0x002DD8D4 File Offset: 0x002DC8D4
		// (set) Token: 0x06007268 RID: 29288 RVA: 0x002DD8E6 File Offset: 0x002DC8E6
		public double ContentVerticalOffset
		{
			get
			{
				return (double)base.GetValue(ScrollViewer.ContentVerticalOffsetProperty);
			}
			private set
			{
				base.SetValue(ScrollViewer.ContentVerticalOffsetPropertyKey, value);
			}
		}

		// Token: 0x17001A89 RID: 6793
		// (get) Token: 0x06007269 RID: 29289 RVA: 0x002DD8F9 File Offset: 0x002DC8F9
		// (set) Token: 0x0600726A RID: 29290 RVA: 0x002DD90B File Offset: 0x002DC90B
		public double ContentHorizontalOffset
		{
			get
			{
				return (double)base.GetValue(ScrollViewer.ContentHorizontalOffsetProperty);
			}
			private set
			{
				base.SetValue(ScrollViewer.ContentHorizontalOffsetPropertyKey, value);
			}
		}

		// Token: 0x0600726B RID: 29291 RVA: 0x002DD91E File Offset: 0x002DC91E
		public static bool GetIsDeferredScrollingEnabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ScrollViewer.IsDeferredScrollingEnabledProperty);
		}

		// Token: 0x0600726C RID: 29292 RVA: 0x002DD93E File Offset: 0x002DC93E
		public static void SetIsDeferredScrollingEnabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.IsDeferredScrollingEnabledProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x0600726D RID: 29293 RVA: 0x002DD95F File Offset: 0x002DC95F
		// (set) Token: 0x0600726E RID: 29294 RVA: 0x002DD971 File Offset: 0x002DC971
		public bool IsDeferredScrollingEnabled
		{
			get
			{
				return (bool)base.GetValue(ScrollViewer.IsDeferredScrollingEnabledProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.IsDeferredScrollingEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x14000146 RID: 326
		// (add) Token: 0x0600726F RID: 29295 RVA: 0x002DD984 File Offset: 0x002DC984
		// (remove) Token: 0x06007270 RID: 29296 RVA: 0x002DD992 File Offset: 0x002DC992
		[Category("Action")]
		public event ScrollChangedEventHandler ScrollChanged
		{
			add
			{
				base.AddHandler(ScrollViewer.ScrollChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ScrollViewer.ScrollChangedEvent, value);
			}
		}

		// Token: 0x06007271 RID: 29297 RVA: 0x002DD9A0 File Offset: 0x002DC9A0
		protected override void OnStylusSystemGesture(StylusSystemGestureEventArgs e)
		{
			this._seenTapGesture = (e.SystemGesture == SystemGesture.Tap);
		}

		// Token: 0x06007272 RID: 29298 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnScrollChanged(ScrollChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06007273 RID: 29299 RVA: 0x002DD9B4 File Offset: 0x002DC9B4
		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			Rect rect = new Rect(0.0, 0.0, base.ActualWidth, base.ActualHeight);
			if (rect.Contains(hitTestParameters.HitPoint))
			{
				return new PointHitTestResult(this, hitTestParameters.HitPoint);
			}
			return null;
		}

		// Token: 0x17001A8B RID: 6795
		// (get) Token: 0x06007274 RID: 29300 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007275 RID: 29301 RVA: 0x002DDA04 File Offset: 0x002DCA04
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			Control control = base.TemplatedParent as Control;
			if (control != null && control.HandlesScrolling)
			{
				return;
			}
			if (e.OriginalSource == this)
			{
				this.ScrollInDirection(e);
				return;
			}
			if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
			{
				ScrollContentPresenter scrollContentPresenter = base.GetTemplateChild("PART_ScrollContentPresenter") as ScrollContentPresenter;
				if (scrollContentPresenter == null)
				{
					this.ScrollInDirection(e);
					return;
				}
				FocusNavigationDirection direction = KeyboardNavigation.KeyToTraversalDirection(e.Key);
				DependencyObject dependencyObject = null;
				DependencyObject dependencyObject2 = Keyboard.FocusedElement as DependencyObject;
				if (this.IsInViewport(scrollContentPresenter, dependencyObject2))
				{
					UIElement uielement = dependencyObject2 as UIElement;
					if (uielement != null)
					{
						dependencyObject = uielement.PredictFocus(direction);
					}
					else
					{
						ContentElement contentElement = dependencyObject2 as ContentElement;
						if (contentElement != null)
						{
							dependencyObject = contentElement.PredictFocus(direction);
						}
						else
						{
							UIElement3D uielement3D = dependencyObject2 as UIElement3D;
							if (uielement3D != null)
							{
								dependencyObject = uielement3D.PredictFocus(direction);
							}
						}
					}
				}
				else
				{
					dependencyObject = scrollContentPresenter.PredictFocus(direction);
				}
				if (dependencyObject == null)
				{
					this.ScrollInDirection(e);
					return;
				}
				if (this.IsInViewport(scrollContentPresenter, dependencyObject))
				{
					((IInputElement)dependencyObject).Focus();
					e.Handled = true;
					return;
				}
				this.ScrollInDirection(e);
				base.UpdateLayout();
				if (this.IsInViewport(scrollContentPresenter, dependencyObject))
				{
					((IInputElement)dependencyObject).Focus();
					return;
				}
			}
			else
			{
				this.ScrollInDirection(e);
			}
		}

		// Token: 0x06007276 RID: 29302 RVA: 0x002DDB54 File Offset: 0x002DCB54
		private bool IsInViewport(ScrollContentPresenter scp, DependencyObject element)
		{
			Visual visualRoot = KeyboardNavigation.GetVisualRoot(scp);
			Visual visualRoot2 = KeyboardNavigation.GetVisualRoot(element);
			while (visualRoot != visualRoot2)
			{
				if (visualRoot2 == null)
				{
					return false;
				}
				FrameworkElement frameworkElement = visualRoot2 as FrameworkElement;
				if (frameworkElement == null)
				{
					return false;
				}
				element = frameworkElement.Parent;
				if (element == null)
				{
					return false;
				}
				visualRoot2 = KeyboardNavigation.GetVisualRoot(element);
			}
			Rect rectangle = KeyboardNavigation.GetRectangle(scp);
			Rect rectangle2 = KeyboardNavigation.GetRectangle(element);
			return rectangle.IntersectsWith(rectangle2);
		}

		// Token: 0x06007277 RID: 29303 RVA: 0x002DDBB4 File Offset: 0x002DCBB4
		internal void ScrollInDirection(KeyEventArgs e)
		{
			bool flag = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) > ModifierKeys.None;
			if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) <= ModifierKeys.None)
			{
				bool flag2 = base.FlowDirection == FlowDirection.RightToLeft;
				switch (e.Key)
				{
				case Key.Prior:
					this.PageUp();
					e.Handled = true;
					return;
				case Key.Next:
					this.PageDown();
					e.Handled = true;
					return;
				case Key.End:
					if (flag)
					{
						this.ScrollToBottom();
					}
					else
					{
						this.ScrollToRightEnd();
					}
					e.Handled = true;
					break;
				case Key.Home:
					if (flag)
					{
						this.ScrollToTop();
					}
					else
					{
						this.ScrollToLeftEnd();
					}
					e.Handled = true;
					return;
				case Key.Left:
					if (flag2)
					{
						this.LineRight();
					}
					else
					{
						this.LineLeft();
					}
					e.Handled = true;
					return;
				case Key.Up:
					this.LineUp();
					e.Handled = true;
					return;
				case Key.Right:
					if (flag2)
					{
						this.LineLeft();
					}
					else
					{
						this.LineRight();
					}
					e.Handled = true;
					return;
				case Key.Down:
					this.LineDown();
					e.Handled = true;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06007278 RID: 29304 RVA: 0x002DDCC0 File Offset: 0x002DCCC0
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			if (!this.HandlesMouseWheelScrolling)
			{
				return;
			}
			if (this.ScrollInfo != null)
			{
				if (e.Delta < 0)
				{
					this.ScrollInfo.MouseWheelDown();
				}
				else
				{
					this.ScrollInfo.MouseWheelUp();
				}
			}
			e.Handled = true;
		}

		// Token: 0x06007279 RID: 29305 RVA: 0x002DDD0F File Offset: 0x002DCD0F
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (base.Focus())
			{
				e.Handled = true;
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x0600727A RID: 29306 RVA: 0x002DDD28 File Offset: 0x002DCD28
		protected override Size MeasureOverride(Size constraint)
		{
			this.InChildInvalidateMeasure = false;
			IScrollInfo scrollInfo = this.ScrollInfo;
			UIElement uielement = (this.VisualChildrenCount > 0) ? (this.GetVisualChild(0) as UIElement) : null;
			ScrollBarVisibility verticalScrollBarVisibility = this.VerticalScrollBarVisibility;
			ScrollBarVisibility horizontalScrollBarVisibility = this.HorizontalScrollBarVisibility;
			Size result = default(Size);
			if (uielement != null)
			{
				bool flag = EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLVIEWER:MeasureOverride");
				}
				try
				{
					bool flag2 = verticalScrollBarVisibility == ScrollBarVisibility.Auto;
					bool flag3 = horizontalScrollBarVisibility == ScrollBarVisibility.Auto;
					bool flag4 = verticalScrollBarVisibility == ScrollBarVisibility.Disabled;
					bool flag5 = horizontalScrollBarVisibility == ScrollBarVisibility.Disabled;
					Visibility visibility = (verticalScrollBarVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
					Visibility visibility2 = (horizontalScrollBarVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
					if (this._scrollVisibilityY != visibility)
					{
						this._scrollVisibilityY = visibility;
						base.SetValue(ScrollViewer.ComputedVerticalScrollBarVisibilityPropertyKey, this._scrollVisibilityY);
					}
					if (this._scrollVisibilityX != visibility2)
					{
						this._scrollVisibilityX = visibility2;
						base.SetValue(ScrollViewer.ComputedHorizontalScrollBarVisibilityPropertyKey, this._scrollVisibilityX);
					}
					if (scrollInfo != null)
					{
						scrollInfo.CanHorizontallyScroll = !flag5;
						scrollInfo.CanVerticallyScroll = !flag4;
					}
					try
					{
						this.InChildMeasurePass1 = true;
						uielement.Measure(constraint);
					}
					finally
					{
						this.InChildMeasurePass1 = false;
					}
					scrollInfo = this.ScrollInfo;
					if (scrollInfo != null && (flag3 || flag2))
					{
						bool flag6 = flag3 && DoubleUtil.GreaterThan(scrollInfo.ExtentWidth, scrollInfo.ViewportWidth);
						bool flag7 = flag2 && DoubleUtil.GreaterThan(scrollInfo.ExtentHeight, scrollInfo.ViewportHeight);
						if (flag6 && this._scrollVisibilityX != Visibility.Visible)
						{
							this._scrollVisibilityX = Visibility.Visible;
							base.SetValue(ScrollViewer.ComputedHorizontalScrollBarVisibilityPropertyKey, this._scrollVisibilityX);
						}
						if (flag7 && this._scrollVisibilityY != Visibility.Visible)
						{
							this._scrollVisibilityY = Visibility.Visible;
							base.SetValue(ScrollViewer.ComputedVerticalScrollBarVisibilityPropertyKey, this._scrollVisibilityY);
						}
						if (flag6 || flag7)
						{
							this.InChildInvalidateMeasure = true;
							uielement.InvalidateMeasure();
							try
							{
								this.InChildMeasurePass2 = true;
								uielement.Measure(constraint);
							}
							finally
							{
								this.InChildMeasurePass2 = false;
							}
						}
						if (flag3 && flag2 && flag6 != flag7)
						{
							object obj = !flag6 && DoubleUtil.GreaterThan(scrollInfo.ExtentWidth, scrollInfo.ViewportWidth);
							bool flag8 = !flag7 && DoubleUtil.GreaterThan(scrollInfo.ExtentHeight, scrollInfo.ViewportHeight);
							object obj2 = obj;
							if (obj2 != null)
							{
								if (this._scrollVisibilityX != Visibility.Visible)
								{
									this._scrollVisibilityX = Visibility.Visible;
									base.SetValue(ScrollViewer.ComputedHorizontalScrollBarVisibilityPropertyKey, this._scrollVisibilityX);
								}
							}
							else if (flag8 && this._scrollVisibilityY != Visibility.Visible)
							{
								this._scrollVisibilityY = Visibility.Visible;
								base.SetValue(ScrollViewer.ComputedVerticalScrollBarVisibilityPropertyKey, this._scrollVisibilityY);
							}
							if ((obj2 | flag8) != null)
							{
								this.InChildInvalidateMeasure = true;
								uielement.InvalidateMeasure();
								try
								{
									this.InChildMeasurePass3 = true;
									uielement.Measure(constraint);
								}
								finally
								{
									this.InChildMeasurePass3 = false;
								}
							}
						}
					}
				}
				finally
				{
					if (flag)
					{
						EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLVIEWER:MeasureOverride");
					}
				}
				result = uielement.DesiredSize;
			}
			if (!base.ArrangeDirty && this.InvalidatedMeasureFromArrange)
			{
				this.InvalidatedMeasureFromArrange = false;
			}
			return result;
		}

		// Token: 0x0600727B RID: 29307 RVA: 0x002DE074 File Offset: 0x002DD074
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool invalidatedMeasureFromArrange = this.InvalidatedMeasureFromArrange;
			Size result = base.ArrangeOverride(arrangeSize);
			if (invalidatedMeasureFromArrange)
			{
				this.InvalidatedMeasureFromArrange = false;
			}
			else
			{
				this.InvalidatedMeasureFromArrange = base.MeasureDirty;
			}
			return result;
		}

		// Token: 0x0600727C RID: 29308 RVA: 0x002DE0A8 File Offset: 0x002DD0A8
		private void BindToTemplatedParent(DependencyProperty property)
		{
			if (!base.HasNonDefaultValue(property))
			{
				base.SetBinding(property, new Binding
				{
					RelativeSource = RelativeSource.TemplatedParent,
					Path = new PropertyPath(property)
				});
			}
		}

		// Token: 0x0600727D RID: 29309 RVA: 0x002DE0E4 File Offset: 0x002DD0E4
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.TemplatedParent != null)
			{
				this.BindToTemplatedParent(ScrollViewer.HorizontalScrollBarVisibilityProperty);
				this.BindToTemplatedParent(ScrollViewer.VerticalScrollBarVisibilityProperty);
				this.BindToTemplatedParent(ScrollViewer.CanContentScrollProperty);
				this.BindToTemplatedParent(ScrollViewer.IsDeferredScrollingEnabledProperty);
				this.BindToTemplatedParent(ScrollViewer.PanningModeProperty);
			}
		}

		// Token: 0x0600727E RID: 29310 RVA: 0x002DE138 File Offset: 0x002DD138
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ScrollBar scrollBar = base.GetTemplateChild("PART_HorizontalScrollBar") as ScrollBar;
			if (scrollBar != null)
			{
				scrollBar.IsStandalone = false;
			}
			scrollBar = (base.GetTemplateChild("PART_VerticalScrollBar") as ScrollBar);
			if (scrollBar != null)
			{
				scrollBar.IsStandalone = false;
			}
			this.OnPanningModeChanged();
		}

		// Token: 0x17001A8C RID: 6796
		// (get) Token: 0x0600727F RID: 29311 RVA: 0x002DE187 File Offset: 0x002DD187
		// (set) Token: 0x06007280 RID: 29312 RVA: 0x002DE18F File Offset: 0x002DD18F
		protected internal IScrollInfo ScrollInfo
		{
			get
			{
				return this._scrollInfo;
			}
			set
			{
				this._scrollInfo = value;
				if (this._scrollInfo != null)
				{
					this._scrollInfo.CanHorizontallyScroll = (this.HorizontalScrollBarVisibility > ScrollBarVisibility.Disabled);
					this._scrollInfo.CanVerticallyScroll = (this.VerticalScrollBarVisibility > ScrollBarVisibility.Disabled);
					this.EnsureQueueProcessing();
				}
			}
		}

		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x06007281 RID: 29313 RVA: 0x002DE1CE File Offset: 0x002DD1CE
		// (set) Token: 0x06007282 RID: 29314 RVA: 0x002DE1E0 File Offset: 0x002DD1E0
		public PanningMode PanningMode
		{
			get
			{
				return (PanningMode)base.GetValue(ScrollViewer.PanningModeProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.PanningModeProperty, value);
			}
		}

		// Token: 0x06007283 RID: 29315 RVA: 0x002DE1F3 File Offset: 0x002DD1F3
		public static void SetPanningMode(DependencyObject element, PanningMode panningMode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.PanningModeProperty, panningMode);
		}

		// Token: 0x06007284 RID: 29316 RVA: 0x002DE214 File Offset: 0x002DD214
		public static PanningMode GetPanningMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (PanningMode)element.GetValue(ScrollViewer.PanningModeProperty);
		}

		// Token: 0x06007285 RID: 29317 RVA: 0x002DE234 File Offset: 0x002DD234
		private static void OnPanningModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewer scrollViewer = d as ScrollViewer;
			if (scrollViewer != null)
			{
				scrollViewer.OnPanningModeChanged();
			}
		}

		// Token: 0x06007286 RID: 29318 RVA: 0x002DE251 File Offset: 0x002DD251
		private void OnPanningModeChanged()
		{
			if (!base.HasTemplateGeneratedSubTree)
			{
				return;
			}
			bool panningMode = this.PanningMode != PanningMode.None;
			base.InvalidateProperty(UIElement.IsManipulationEnabledProperty);
			if (panningMode)
			{
				base.SetCurrentValueInternal(UIElement.IsManipulationEnabledProperty, BooleanBoxes.TrueBox);
			}
		}

		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x06007287 RID: 29319 RVA: 0x002DE27F File Offset: 0x002DD27F
		// (set) Token: 0x06007288 RID: 29320 RVA: 0x002DE291 File Offset: 0x002DD291
		public double PanningDeceleration
		{
			get
			{
				return (double)base.GetValue(ScrollViewer.PanningDecelerationProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.PanningDecelerationProperty, value);
			}
		}

		// Token: 0x06007289 RID: 29321 RVA: 0x002DE2A4 File Offset: 0x002DD2A4
		public static void SetPanningDeceleration(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.PanningDecelerationProperty, value);
		}

		// Token: 0x0600728A RID: 29322 RVA: 0x002DE2C5 File Offset: 0x002DD2C5
		public static double GetPanningDeceleration(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ScrollViewer.PanningDecelerationProperty);
		}

		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x0600728B RID: 29323 RVA: 0x002DE2E5 File Offset: 0x002DD2E5
		// (set) Token: 0x0600728C RID: 29324 RVA: 0x002DE2F7 File Offset: 0x002DD2F7
		public double PanningRatio
		{
			get
			{
				return (double)base.GetValue(ScrollViewer.PanningRatioProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.PanningRatioProperty, value);
			}
		}

		// Token: 0x0600728D RID: 29325 RVA: 0x002DE30A File Offset: 0x002DD30A
		public static void SetPanningRatio(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.PanningRatioProperty, value);
		}

		// Token: 0x0600728E RID: 29326 RVA: 0x002DE32B File Offset: 0x002DD32B
		public static double GetPanningRatio(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ScrollViewer.PanningRatioProperty);
		}

		// Token: 0x0600728F RID: 29327 RVA: 0x002DE34C File Offset: 0x002DD34C
		private static bool CheckFiniteNonNegative(object value)
		{
			double num = (double)value;
			return DoubleUtil.GreaterThanOrClose(num, 0.0) && !double.IsInfinity(num);
		}

		// Token: 0x06007290 RID: 29328 RVA: 0x002DE37C File Offset: 0x002DD37C
		protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
		{
			this._panningInfo = null;
			this._seenTapGesture = false;
			PanningMode panningMode = this.PanningMode;
			if (panningMode != PanningMode.None)
			{
				this.CompleteScrollManipulation = false;
				ScrollContentPresenter scrollContentPresenter = base.GetTemplateChild("PART_ScrollContentPresenter") as ScrollContentPresenter;
				if (this.ShouldManipulateScroll(e, scrollContentPresenter))
				{
					if (panningMode == PanningMode.HorizontalOnly)
					{
						e.Mode = ManipulationModes.TranslateX;
					}
					else if (panningMode == PanningMode.VerticalOnly)
					{
						e.Mode = ManipulationModes.TranslateY;
					}
					else
					{
						e.Mode = ManipulationModes.Translate;
					}
					e.ManipulationContainer = this;
					this._panningInfo = new ScrollViewer.PanningInfo
					{
						OriginalHorizontalOffset = this.HorizontalOffset,
						OriginalVerticalOffset = this.VerticalOffset,
						PanningMode = panningMode
					};
					double num = this.ViewportWidth + 1.0;
					double num2 = this.ViewportHeight + 1.0;
					if (scrollContentPresenter != null)
					{
						this._panningInfo.DeltaPerHorizontalOffet = (DoubleUtil.AreClose(num, 0.0) ? 0.0 : (scrollContentPresenter.ActualWidth / num));
						this._panningInfo.DeltaPerVerticalOffset = (DoubleUtil.AreClose(num2, 0.0) ? 0.0 : (scrollContentPresenter.ActualHeight / num2));
					}
					else
					{
						this._panningInfo.DeltaPerHorizontalOffet = (DoubleUtil.AreClose(num, 0.0) ? 0.0 : (base.ActualWidth / num));
						this._panningInfo.DeltaPerVerticalOffset = (DoubleUtil.AreClose(num2, 0.0) ? 0.0 : (base.ActualHeight / num2));
					}
					if (!this.ManipulationBindingsInitialized)
					{
						this.BindToTemplatedParent(ScrollViewer.PanningDecelerationProperty);
						this.BindToTemplatedParent(ScrollViewer.PanningRatioProperty);
						this.ManipulationBindingsInitialized = true;
					}
				}
				else
				{
					e.Cancel();
					this.ForceNextManipulationComplete = false;
				}
				e.Handled = true;
			}
		}

		// Token: 0x06007291 RID: 29329 RVA: 0x002DE53C File Offset: 0x002DD53C
		private bool ShouldManipulateScroll(ManipulationStartingEventArgs e, ScrollContentPresenter viewport)
		{
			if (!PresentationSource.UnderSamePresentationSource(new DependencyObject[]
			{
				e.OriginalSource as DependencyObject,
				this
			}))
			{
				return false;
			}
			if (viewport == null)
			{
				return true;
			}
			GeneralTransform generalTransform = base.TransformToDescendant(viewport);
			double actualWidth = viewport.ActualWidth;
			double actualHeight = viewport.ActualHeight;
			foreach (IManipulator manipulator in e.Manipulators)
			{
				Point point = generalTransform.Transform(manipulator.GetPosition(this));
				if (DoubleUtil.LessThan(point.X, 0.0) || DoubleUtil.LessThan(point.Y, 0.0) || DoubleUtil.GreaterThan(point.X, actualWidth) || DoubleUtil.GreaterThan(point.Y, actualHeight))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007292 RID: 29330 RVA: 0x002DE624 File Offset: 0x002DD624
		protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
		{
			if (this._panningInfo != null)
			{
				if (e.IsInertial && this.CompleteScrollManipulation)
				{
					e.Complete();
				}
				else
				{
					bool flag = false;
					if (this._seenTapGesture)
					{
						e.Cancel();
						this._panningInfo = null;
					}
					else if (this._panningInfo.IsPanning)
					{
						this.ManipulateScroll(e);
					}
					else if (this.CanStartScrollManipulation(e.CumulativeManipulation.Translation, out flag))
					{
						this._panningInfo.IsPanning = true;
						this.ManipulateScroll(e);
					}
					else if (flag)
					{
						e.Cancel();
						this._panningInfo = null;
					}
				}
				e.Handled = true;
			}
		}

		// Token: 0x06007293 RID: 29331 RVA: 0x002DE6C8 File Offset: 0x002DD6C8
		private void ManipulateScroll(ManipulationDeltaEventArgs e)
		{
			PanningMode panningMode = this._panningInfo.PanningMode;
			if (panningMode != PanningMode.VerticalOnly)
			{
				this.ManipulateScroll(e.DeltaManipulation.Translation.X, e.CumulativeManipulation.Translation.X, true);
			}
			if (panningMode != PanningMode.HorizontalOnly)
			{
				this.ManipulateScroll(e.DeltaManipulation.Translation.Y, e.CumulativeManipulation.Translation.Y, false);
			}
			if (e.IsInertial && this.IsPastInertialLimit())
			{
				e.Complete();
				return;
			}
			double num = this._panningInfo.UnusedTranslation.X;
			if (!this._panningInfo.InHorizontalFeedback && DoubleUtil.LessThan(Math.Abs(num), 8.0))
			{
				num = 0.0;
			}
			this._panningInfo.InHorizontalFeedback = !DoubleUtil.AreClose(num, 0.0);
			double num2 = this._panningInfo.UnusedTranslation.Y;
			if (!this._panningInfo.InVerticalFeedback && DoubleUtil.LessThan(Math.Abs(num2), 5.0))
			{
				num2 = 0.0;
			}
			this._panningInfo.InVerticalFeedback = !DoubleUtil.AreClose(num2, 0.0);
			if (this._panningInfo.InHorizontalFeedback || this._panningInfo.InVerticalFeedback)
			{
				e.ReportBoundaryFeedback(new ManipulationDelta(new Vector(num, num2), 0.0, new Vector(1.0, 1.0), default(Vector)));
				if (e.IsInertial && this._panningInfo.InertiaBoundaryBeginTimestamp == 0)
				{
					this._panningInfo.InertiaBoundaryBeginTimestamp = Environment.TickCount;
				}
			}
		}

		// Token: 0x06007294 RID: 29332 RVA: 0x002DE890 File Offset: 0x002DD890
		private void ManipulateScroll(double delta, double cumulativeTranslation, bool isHorizontal)
		{
			double num = isHorizontal ? this._panningInfo.UnusedTranslation.X : this._panningInfo.UnusedTranslation.Y;
			double value = isHorizontal ? this.HorizontalOffset : this.VerticalOffset;
			double num2 = isHorizontal ? this.ScrollableWidth : this.ScrollableHeight;
			if (DoubleUtil.AreClose(num2, 0.0))
			{
				num = 0.0;
				delta = 0.0;
			}
			else if ((DoubleUtil.GreaterThan(delta, 0.0) && DoubleUtil.AreClose(value, 0.0)) || (DoubleUtil.LessThan(delta, 0.0) && DoubleUtil.AreClose(value, num2)))
			{
				num += delta;
				delta = 0.0;
			}
			else if (DoubleUtil.LessThan(delta, 0.0) && DoubleUtil.GreaterThan(num, 0.0))
			{
				double num3 = Math.Max(num + delta, 0.0);
				delta += num - num3;
				num = num3;
			}
			else if (DoubleUtil.GreaterThan(delta, 0.0) && DoubleUtil.LessThan(num, 0.0))
			{
				double num4 = Math.Min(num + delta, 0.0);
				delta += num - num4;
				num = num4;
			}
			if (isHorizontal)
			{
				if (!DoubleUtil.AreClose(delta, 0.0))
				{
					this.ScrollToHorizontalOffset(this._panningInfo.OriginalHorizontalOffset - Math.Round(this.PanningRatio * cumulativeTranslation / this._panningInfo.DeltaPerHorizontalOffet));
				}
				this._panningInfo.UnusedTranslation = new Vector(num, this._panningInfo.UnusedTranslation.Y);
				return;
			}
			if (!DoubleUtil.AreClose(delta, 0.0))
			{
				this.ScrollToVerticalOffset(this._panningInfo.OriginalVerticalOffset - Math.Round(this.PanningRatio * cumulativeTranslation / this._panningInfo.DeltaPerVerticalOffset));
			}
			this._panningInfo.UnusedTranslation = new Vector(this._panningInfo.UnusedTranslation.X, num);
		}

		// Token: 0x06007295 RID: 29333 RVA: 0x002DEAB0 File Offset: 0x002DDAB0
		private bool IsPastInertialLimit()
		{
			return Math.Abs(Environment.TickCount - this._panningInfo.InertiaBoundaryBeginTimestamp) >= 100 && (DoubleUtil.GreaterThanOrClose(Math.Abs(this._panningInfo.UnusedTranslation.X), 50.0) || DoubleUtil.GreaterThanOrClose(Math.Abs(this._panningInfo.UnusedTranslation.Y), 50.0));
		}

		// Token: 0x06007296 RID: 29334 RVA: 0x002DEB2C File Offset: 0x002DDB2C
		private bool CanStartScrollManipulation(Vector translation, out bool cancelManipulation)
		{
			cancelManipulation = false;
			PanningMode panningMode = this._panningInfo.PanningMode;
			if (panningMode == PanningMode.None)
			{
				cancelManipulation = true;
				return false;
			}
			bool flag = DoubleUtil.GreaterThan(Math.Abs(translation.X), 3.0);
			bool flag2 = DoubleUtil.GreaterThan(Math.Abs(translation.Y), 3.0);
			if ((panningMode == PanningMode.Both && (flag || flag2)) || (panningMode == PanningMode.HorizontalOnly && flag) || (panningMode == PanningMode.VerticalOnly && flag2))
			{
				return true;
			}
			if (panningMode == PanningMode.HorizontalFirst)
			{
				bool flag3 = DoubleUtil.GreaterThanOrClose(Math.Abs(translation.X), Math.Abs(translation.Y));
				if (flag && flag3)
				{
					return true;
				}
				if (flag2)
				{
					cancelManipulation = true;
					return false;
				}
			}
			else if (panningMode == PanningMode.VerticalFirst)
			{
				bool flag4 = DoubleUtil.GreaterThanOrClose(Math.Abs(translation.Y), Math.Abs(translation.X));
				if (flag2 && flag4)
				{
					return true;
				}
				if (flag)
				{
					cancelManipulation = true;
					return false;
				}
			}
			return false;
		}

		// Token: 0x06007297 RID: 29335 RVA: 0x002DEC08 File Offset: 0x002DDC08
		protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e)
		{
			if (this._panningInfo != null)
			{
				if (!this._panningInfo.IsPanning && !this.ForceNextManipulationComplete)
				{
					e.Cancel();
					this._panningInfo = null;
				}
				else
				{
					e.TranslationBehavior.DesiredDeceleration = this.PanningDeceleration;
				}
				e.Handled = true;
			}
		}

		// Token: 0x06007298 RID: 29336 RVA: 0x002DEC5C File Offset: 0x002DDC5C
		protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
		{
			if (this._panningInfo != null)
			{
				if (!e.IsInertial || !this.CompleteScrollManipulation)
				{
					if (e.IsInertial && !DoubleUtil.AreClose(e.FinalVelocities.LinearVelocity, default(Vector)) && !this.IsPastInertialLimit())
					{
						this.ForceNextManipulationComplete = true;
					}
					else
					{
						if (!e.IsInertial && !this._panningInfo.IsPanning && !this.ForceNextManipulationComplete)
						{
							e.Cancel();
						}
						this.ForceNextManipulationComplete = false;
					}
				}
				this._panningInfo = null;
				this.CompleteScrollManipulation = false;
				e.Handled = true;
			}
		}

		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x06007299 RID: 29337 RVA: 0x002DECF8 File Offset: 0x002DDCF8
		// (set) Token: 0x0600729A RID: 29338 RVA: 0x002DED05 File Offset: 0x002DDD05
		internal bool HandlesMouseWheelScrolling
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.HandlesMouseWheelScrolling) == ScrollViewer.Flags.HandlesMouseWheelScrolling;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.HandlesMouseWheelScrolling, value);
			}
		}

		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x0600729B RID: 29339 RVA: 0x002DED0F File Offset: 0x002DDD0F
		// (set) Token: 0x0600729C RID: 29340 RVA: 0x002DED1C File Offset: 0x002DDD1C
		internal bool InChildInvalidateMeasure
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InChildInvalidateMeasure) == ScrollViewer.Flags.InChildInvalidateMeasure;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InChildInvalidateMeasure, value);
			}
		}

		// Token: 0x0600729D RID: 29341 RVA: 0x002DED28 File Offset: 0x002DDD28
		private bool ExecuteNextCommand()
		{
			IScrollInfo scrollInfo = this.ScrollInfo;
			if (scrollInfo == null)
			{
				return false;
			}
			ScrollViewer.Command command = this._queue.Fetch();
			switch (command.Code)
			{
			case ScrollViewer.Commands.Invalid:
				return false;
			case ScrollViewer.Commands.LineUp:
				scrollInfo.LineUp();
				break;
			case ScrollViewer.Commands.LineDown:
				scrollInfo.LineDown();
				break;
			case ScrollViewer.Commands.LineLeft:
				scrollInfo.LineLeft();
				break;
			case ScrollViewer.Commands.LineRight:
				scrollInfo.LineRight();
				break;
			case ScrollViewer.Commands.PageUp:
				scrollInfo.PageUp();
				break;
			case ScrollViewer.Commands.PageDown:
				scrollInfo.PageDown();
				break;
			case ScrollViewer.Commands.PageLeft:
				scrollInfo.PageLeft();
				break;
			case ScrollViewer.Commands.PageRight:
				scrollInfo.PageRight();
				break;
			case ScrollViewer.Commands.SetHorizontalOffset:
				scrollInfo.SetHorizontalOffset(command.Param);
				break;
			case ScrollViewer.Commands.SetVerticalOffset:
				scrollInfo.SetVerticalOffset(command.Param);
				break;
			case ScrollViewer.Commands.MakeVisible:
			{
				Visual child = command.MakeVisibleParam.Child;
				Visual visual = scrollInfo as Visual;
				if (child != null && visual != null && (visual == child || visual.IsAncestorOf(child)) && base.IsAncestorOf(visual))
				{
					Rect rectangle = command.MakeVisibleParam.TargetRect;
					if (rectangle.IsEmpty)
					{
						UIElement uielement = child as UIElement;
						if (uielement != null)
						{
							rectangle = new Rect(uielement.RenderSize);
						}
						else
						{
							rectangle = default(Rect);
						}
					}
					Rect rect;
					if (scrollInfo.GetType() == typeof(ScrollContentPresenter))
					{
						rect = ((ScrollContentPresenter)scrollInfo).MakeVisible(child, rectangle, false);
					}
					else
					{
						rect = scrollInfo.MakeVisible(child, rectangle);
					}
					if (!rect.IsEmpty)
					{
						rect = visual.TransformToAncestor(this).TransformBounds(rect);
					}
					base.BringIntoView(rect);
				}
				break;
			}
			}
			return true;
		}

		// Token: 0x0600729E RID: 29342 RVA: 0x002DEEDF File Offset: 0x002DDEDF
		private void EnqueueCommand(ScrollViewer.Commands code, double param, ScrollViewer.MakeVisibleParams mvp)
		{
			this._queue.Enqueue(new ScrollViewer.Command(code, param, mvp));
			this.EnsureQueueProcessing();
		}

		// Token: 0x0600729F RID: 29343 RVA: 0x002DEEFA File Offset: 0x002DDEFA
		private void EnsureQueueProcessing()
		{
			if (!this._queue.IsEmpty())
			{
				this.EnsureLayoutUpdatedHandler();
			}
		}

		// Token: 0x060072A0 RID: 29344 RVA: 0x002DEF10 File Offset: 0x002DDF10
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			if (this.ExecuteNextCommand())
			{
				base.InvalidateArrange();
				return;
			}
			double horizontalOffset = this.HorizontalOffset;
			double verticalOffset = this.VerticalOffset;
			double viewportWidth = this.ViewportWidth;
			double viewportHeight = this.ViewportHeight;
			double extentWidth = this.ExtentWidth;
			double extentHeight = this.ExtentHeight;
			double scrollableWidth = this.ScrollableWidth;
			double scrollableHeight = this.ScrollableHeight;
			bool flag = false;
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(horizontalOffset, this.ScrollInfo.HorizontalOffset))
			{
				this._xPositionISI = this.ScrollInfo.HorizontalOffset;
				this.HorizontalOffset = this._xPositionISI;
				this.ContentHorizontalOffset = this._xPositionISI;
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(verticalOffset, this.ScrollInfo.VerticalOffset))
			{
				this._yPositionISI = this.ScrollInfo.VerticalOffset;
				this.VerticalOffset = this._yPositionISI;
				this.ContentVerticalOffset = this._yPositionISI;
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(viewportWidth, this.ScrollInfo.ViewportWidth))
			{
				this._xSize = this.ScrollInfo.ViewportWidth;
				base.SetValue(ScrollViewer.ViewportWidthPropertyKey, this._xSize);
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(viewportHeight, this.ScrollInfo.ViewportHeight))
			{
				this._ySize = this.ScrollInfo.ViewportHeight;
				base.SetValue(ScrollViewer.ViewportHeightPropertyKey, this._ySize);
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(extentWidth, this.ScrollInfo.ExtentWidth))
			{
				this._xExtent = this.ScrollInfo.ExtentWidth;
				base.SetValue(ScrollViewer.ExtentWidthPropertyKey, this._xExtent);
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(extentHeight, this.ScrollInfo.ExtentHeight))
			{
				this._yExtent = this.ScrollInfo.ExtentHeight;
				base.SetValue(ScrollViewer.ExtentHeightPropertyKey, this._yExtent);
				flag = true;
			}
			double scrollableWidth2 = this.ScrollableWidth;
			if (!DoubleUtil.AreClose(scrollableWidth, this.ScrollableWidth))
			{
				base.SetValue(ScrollViewer.ScrollableWidthPropertyKey, scrollableWidth2);
				flag = true;
			}
			double scrollableHeight2 = this.ScrollableHeight;
			if (!DoubleUtil.AreClose(scrollableHeight, this.ScrollableHeight))
			{
				base.SetValue(ScrollViewer.ScrollableHeightPropertyKey, scrollableHeight2);
				flag = true;
			}
			if (flag)
			{
				ScrollChangedEventArgs scrollChangedEventArgs = new ScrollChangedEventArgs(new Vector(this.HorizontalOffset, this.VerticalOffset), new Vector(this.HorizontalOffset - horizontalOffset, this.VerticalOffset - verticalOffset), new Size(this.ExtentWidth, this.ExtentHeight), new Vector(this.ExtentWidth - extentWidth, this.ExtentHeight - extentHeight), new Size(this.ViewportWidth, this.ViewportHeight), new Vector(this.ViewportWidth - viewportWidth, this.ViewportHeight - viewportHeight));
				scrollChangedEventArgs.RoutedEvent = ScrollViewer.ScrollChangedEvent;
				scrollChangedEventArgs.Source = this;
				try
				{
					this.OnScrollChanged(scrollChangedEventArgs);
					ScrollViewerAutomationPeer scrollViewerAutomationPeer = UIElementAutomationPeer.FromElement(this) as ScrollViewerAutomationPeer;
					if (scrollViewerAutomationPeer != null)
					{
						scrollViewerAutomationPeer.RaiseAutomationEvents(extentWidth, extentHeight, viewportWidth, viewportHeight, horizontalOffset, verticalOffset);
					}
				}
				finally
				{
					this.ClearLayoutUpdatedHandler();
				}
			}
			this.ClearLayoutUpdatedHandler();
		}

		// Token: 0x060072A1 RID: 29345 RVA: 0x002DF240 File Offset: 0x002DE240
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ScrollViewerAutomationPeer(this);
		}

		// Token: 0x060072A2 RID: 29346 RVA: 0x002DF248 File Offset: 0x002DE248
		private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			ScrollViewer scrollViewer = sender as ScrollViewer;
			Visual visual = e.TargetObject as Visual;
			if (visual != null)
			{
				if (visual != scrollViewer && visual.IsDescendantOf(scrollViewer))
				{
					e.Handled = true;
					scrollViewer.MakeVisible(visual, e.TargetRect);
					return;
				}
			}
			else
			{
				ContentElement contentElement = e.TargetObject as ContentElement;
				if (contentElement != null)
				{
					IContentHost contentHost = ContentHostHelper.FindContentHost(contentElement);
					visual = (contentHost as Visual);
					if (visual != null && visual.IsDescendantOf(scrollViewer))
					{
						ReadOnlyCollection<Rect> rectangles = contentHost.GetRectangles(contentElement);
						if (rectangles.Count > 0)
						{
							e.Handled = true;
							scrollViewer.MakeVisible(visual, rectangles[0]);
						}
					}
				}
			}
		}

		// Token: 0x060072A3 RID: 29347 RVA: 0x002DF2E0 File Offset: 0x002DE2E0
		private static void OnScrollCommand(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Command == ScrollBar.DeferScrollToHorizontalOffsetCommand)
			{
				if (args.Parameter is double)
				{
					((ScrollViewer)target).DeferScrollToHorizontalOffset((double)args.Parameter);
				}
			}
			else if (args.Command == ScrollBar.DeferScrollToVerticalOffsetCommand)
			{
				if (args.Parameter is double)
				{
					((ScrollViewer)target).DeferScrollToVerticalOffset((double)args.Parameter);
				}
			}
			else if (args.Command == ScrollBar.LineLeftCommand)
			{
				((ScrollViewer)target).LineLeft();
			}
			else if (args.Command == ScrollBar.LineRightCommand)
			{
				((ScrollViewer)target).LineRight();
			}
			else if (args.Command == ScrollBar.PageLeftCommand)
			{
				((ScrollViewer)target).PageLeft();
			}
			else if (args.Command == ScrollBar.PageRightCommand)
			{
				((ScrollViewer)target).PageRight();
			}
			else if (args.Command == ScrollBar.LineUpCommand)
			{
				((ScrollViewer)target).LineUp();
			}
			else if (args.Command == ScrollBar.LineDownCommand)
			{
				((ScrollViewer)target).LineDown();
			}
			else if (args.Command == ScrollBar.PageUpCommand || args.Command == ComponentCommands.ScrollPageUp)
			{
				((ScrollViewer)target).PageUp();
			}
			else if (args.Command == ScrollBar.PageDownCommand || args.Command == ComponentCommands.ScrollPageDown)
			{
				((ScrollViewer)target).PageDown();
			}
			else if (args.Command == ScrollBar.ScrollToEndCommand)
			{
				((ScrollViewer)target).ScrollToEnd();
			}
			else if (args.Command == ScrollBar.ScrollToHomeCommand)
			{
				((ScrollViewer)target).ScrollToHome();
			}
			else if (args.Command == ScrollBar.ScrollToLeftEndCommand)
			{
				((ScrollViewer)target).ScrollToLeftEnd();
			}
			else if (args.Command == ScrollBar.ScrollToRightEndCommand)
			{
				((ScrollViewer)target).ScrollToRightEnd();
			}
			else if (args.Command == ScrollBar.ScrollToTopCommand)
			{
				((ScrollViewer)target).ScrollToTop();
			}
			else if (args.Command == ScrollBar.ScrollToBottomCommand)
			{
				((ScrollViewer)target).ScrollToBottom();
			}
			else if (args.Command == ScrollBar.ScrollToHorizontalOffsetCommand)
			{
				if (args.Parameter is double)
				{
					((ScrollViewer)target).ScrollToHorizontalOffset((double)args.Parameter);
				}
			}
			else if (args.Command == ScrollBar.ScrollToVerticalOffsetCommand && args.Parameter is double)
			{
				((ScrollViewer)target).ScrollToVerticalOffset((double)args.Parameter);
			}
			ScrollViewer scrollViewer = target as ScrollViewer;
			if (scrollViewer != null)
			{
				scrollViewer.CompleteScrollManipulation = true;
			}
		}

		// Token: 0x060072A4 RID: 29348 RVA: 0x002DF57C File Offset: 0x002DE57C
		private static void OnQueryScrollCommand(object target, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = true;
			if (args.Command == ComponentCommands.ScrollPageUp || args.Command == ComponentCommands.ScrollPageDown)
			{
				ScrollViewer scrollViewer = target as ScrollViewer;
				Control control = (scrollViewer != null) ? (scrollViewer.TemplatedParent as Control) : null;
				if (control != null && control.HandlesScrolling)
				{
					args.CanExecute = false;
					args.ContinueRouting = true;
					args.Handled = true;
					return;
				}
			}
			else if (args.Command == ScrollBar.DeferScrollToHorizontalOffsetCommand || args.Command == ScrollBar.DeferScrollToVerticalOffsetCommand)
			{
				ScrollViewer scrollViewer2 = target as ScrollViewer;
				if (scrollViewer2 != null && !scrollViewer2.IsDeferredScrollingEnabled)
				{
					args.CanExecute = false;
					args.Handled = true;
				}
			}
		}

		// Token: 0x060072A5 RID: 29349 RVA: 0x002DF620 File Offset: 0x002DE620
		private static void InitializeCommands()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(ScrollViewer.OnScrollCommand);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(ScrollViewer.OnQueryScrollCommand);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.LineLeftCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.LineRightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.PageLeftCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.PageRightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.LineUpCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.LineDownCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.PageUpCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.PageDownCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToLeftEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToRightEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToHomeCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToTopCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToBottomCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToHorizontalOffsetCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToVerticalOffsetCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.DeferScrollToHorizontalOffsetCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.DeferScrollToVerticalOffsetCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ComponentCommands.ScrollPageUp, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ComponentCommands.ScrollPageDown, executedRoutedEventHandler, canExecuteRoutedEventHandler);
		}

		// Token: 0x060072A6 RID: 29350 RVA: 0x002DF800 File Offset: 0x002DE800
		private static ControlTemplate CreateDefaultControlTemplate()
		{
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Grid), "Grid");
			FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof(ColumnDefinition), "ColumnDefinitionOne");
			FrameworkElementFactory frameworkElementFactory3 = new FrameworkElementFactory(typeof(ColumnDefinition), "ColumnDefinitionTwo");
			FrameworkElementFactory frameworkElementFactory4 = new FrameworkElementFactory(typeof(RowDefinition), "RowDefinitionOne");
			FrameworkElementFactory frameworkElementFactory5 = new FrameworkElementFactory(typeof(RowDefinition), "RowDefinitionTwo");
			FrameworkElementFactory frameworkElementFactory6 = new FrameworkElementFactory(typeof(ScrollBar), "PART_VerticalScrollBar");
			FrameworkElementFactory frameworkElementFactory7 = new FrameworkElementFactory(typeof(ScrollBar), "PART_HorizontalScrollBar");
			FrameworkElementFactory frameworkElementFactory8 = new FrameworkElementFactory(typeof(ScrollContentPresenter), "PART_ScrollContentPresenter");
			FrameworkElementFactory frameworkElementFactory9 = new FrameworkElementFactory(typeof(Rectangle), "Corner");
			Binding binding = new Binding("HorizontalOffset");
			binding.Mode = BindingMode.OneWay;
			binding.RelativeSource = RelativeSource.TemplatedParent;
			Binding binding2 = new Binding("VerticalOffset");
			binding2.Mode = BindingMode.OneWay;
			binding2.RelativeSource = RelativeSource.TemplatedParent;
			frameworkElementFactory.SetValue(Panel.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
			frameworkElementFactory.AppendChild(frameworkElementFactory2);
			frameworkElementFactory.AppendChild(frameworkElementFactory3);
			frameworkElementFactory.AppendChild(frameworkElementFactory4);
			frameworkElementFactory.AppendChild(frameworkElementFactory5);
			frameworkElementFactory.AppendChild(frameworkElementFactory9);
			frameworkElementFactory.AppendChild(frameworkElementFactory8);
			frameworkElementFactory.AppendChild(frameworkElementFactory6);
			frameworkElementFactory.AppendChild(frameworkElementFactory7);
			frameworkElementFactory2.SetValue(ColumnDefinition.WidthProperty, new GridLength(1.0, GridUnitType.Star));
			frameworkElementFactory3.SetValue(ColumnDefinition.WidthProperty, new GridLength(1.0, GridUnitType.Auto));
			frameworkElementFactory4.SetValue(RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Star));
			frameworkElementFactory5.SetValue(RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Auto));
			frameworkElementFactory8.SetValue(Grid.ColumnProperty, 0);
			frameworkElementFactory8.SetValue(Grid.RowProperty, 0);
			frameworkElementFactory8.SetValue(FrameworkElement.MarginProperty, new TemplateBindingExtension(Control.PaddingProperty));
			frameworkElementFactory8.SetValue(ContentControl.ContentProperty, new TemplateBindingExtension(ContentControl.ContentProperty));
			frameworkElementFactory8.SetValue(ContentControl.ContentTemplateProperty, new TemplateBindingExtension(ContentControl.ContentTemplateProperty));
			frameworkElementFactory8.SetValue(ScrollViewer.CanContentScrollProperty, new TemplateBindingExtension(ScrollViewer.CanContentScrollProperty));
			frameworkElementFactory7.SetValue(ScrollBar.OrientationProperty, Orientation.Horizontal);
			frameworkElementFactory7.SetValue(Grid.ColumnProperty, 0);
			frameworkElementFactory7.SetValue(Grid.RowProperty, 1);
			frameworkElementFactory7.SetValue(RangeBase.MinimumProperty, 0.0);
			frameworkElementFactory7.SetValue(RangeBase.MaximumProperty, new TemplateBindingExtension(ScrollViewer.ScrollableWidthProperty));
			frameworkElementFactory7.SetValue(ScrollBar.ViewportSizeProperty, new TemplateBindingExtension(ScrollViewer.ViewportWidthProperty));
			frameworkElementFactory7.SetBinding(RangeBase.ValueProperty, binding);
			frameworkElementFactory7.SetValue(UIElement.VisibilityProperty, new TemplateBindingExtension(ScrollViewer.ComputedHorizontalScrollBarVisibilityProperty));
			frameworkElementFactory7.SetValue(FrameworkElement.CursorProperty, Cursors.Arrow);
			frameworkElementFactory7.SetValue(AutomationProperties.AutomationIdProperty, "HorizontalScrollBar");
			frameworkElementFactory6.SetValue(Grid.ColumnProperty, 1);
			frameworkElementFactory6.SetValue(Grid.RowProperty, 0);
			frameworkElementFactory6.SetValue(RangeBase.MinimumProperty, 0.0);
			frameworkElementFactory6.SetValue(RangeBase.MaximumProperty, new TemplateBindingExtension(ScrollViewer.ScrollableHeightProperty));
			frameworkElementFactory6.SetValue(ScrollBar.ViewportSizeProperty, new TemplateBindingExtension(ScrollViewer.ViewportHeightProperty));
			frameworkElementFactory6.SetBinding(RangeBase.ValueProperty, binding2);
			frameworkElementFactory6.SetValue(UIElement.VisibilityProperty, new TemplateBindingExtension(ScrollViewer.ComputedVerticalScrollBarVisibilityProperty));
			frameworkElementFactory6.SetValue(FrameworkElement.CursorProperty, Cursors.Arrow);
			frameworkElementFactory6.SetValue(AutomationProperties.AutomationIdProperty, "VerticalScrollBar");
			frameworkElementFactory9.SetValue(Grid.ColumnProperty, 1);
			frameworkElementFactory9.SetValue(Grid.RowProperty, 1);
			frameworkElementFactory9.SetResourceReference(Shape.FillProperty, SystemColors.ControlBrushKey);
			ControlTemplate controlTemplate = new ControlTemplate(typeof(ScrollViewer));
			controlTemplate.VisualTree = frameworkElementFactory;
			controlTemplate.Seal();
			return controlTemplate;
		}

		// Token: 0x060072A7 RID: 29351 RVA: 0x002DFC1D File Offset: 0x002DEC1D
		private void SetFlagValue(ScrollViewer.Flags flag, bool value)
		{
			if (value)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x17001A92 RID: 6802
		// (get) Token: 0x060072A8 RID: 29352 RVA: 0x002DFC40 File Offset: 0x002DEC40
		// (set) Token: 0x060072A9 RID: 29353 RVA: 0x002DFC4D File Offset: 0x002DEC4D
		private bool InvalidatedMeasureFromArrange
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InvalidatedMeasureFromArrange) == ScrollViewer.Flags.InvalidatedMeasureFromArrange;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InvalidatedMeasureFromArrange, value);
			}
		}

		// Token: 0x17001A93 RID: 6803
		// (get) Token: 0x060072AA RID: 29354 RVA: 0x002DFC57 File Offset: 0x002DEC57
		// (set) Token: 0x060072AB RID: 29355 RVA: 0x002DFC64 File Offset: 0x002DEC64
		private bool ForceNextManipulationComplete
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.ForceNextManipulationComplete) == ScrollViewer.Flags.ForceNextManipulationComplete;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.ForceNextManipulationComplete, value);
			}
		}

		// Token: 0x17001A94 RID: 6804
		// (get) Token: 0x060072AC RID: 29356 RVA: 0x002DFC6E File Offset: 0x002DEC6E
		// (set) Token: 0x060072AD RID: 29357 RVA: 0x002DFC7D File Offset: 0x002DEC7D
		private bool ManipulationBindingsInitialized
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.ManipulationBindingsInitialized) == ScrollViewer.Flags.ManipulationBindingsInitialized;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.ManipulationBindingsInitialized, value);
			}
		}

		// Token: 0x17001A95 RID: 6805
		// (get) Token: 0x060072AE RID: 29358 RVA: 0x002DFC88 File Offset: 0x002DEC88
		// (set) Token: 0x060072AF RID: 29359 RVA: 0x002DFC97 File Offset: 0x002DEC97
		private bool CompleteScrollManipulation
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.CompleteScrollManipulation) == ScrollViewer.Flags.CompleteScrollManipulation;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.CompleteScrollManipulation, value);
			}
		}

		// Token: 0x17001A96 RID: 6806
		// (get) Token: 0x060072B0 RID: 29360 RVA: 0x002DFCA2 File Offset: 0x002DECA2
		// (set) Token: 0x060072B1 RID: 29361 RVA: 0x002DFCB1 File Offset: 0x002DECB1
		internal bool InChildMeasurePass1
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InChildMeasurePass1) == ScrollViewer.Flags.InChildMeasurePass1;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InChildMeasurePass1, value);
			}
		}

		// Token: 0x17001A97 RID: 6807
		// (get) Token: 0x060072B2 RID: 29362 RVA: 0x002DFCBC File Offset: 0x002DECBC
		// (set) Token: 0x060072B3 RID: 29363 RVA: 0x002DFCD1 File Offset: 0x002DECD1
		internal bool InChildMeasurePass2
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InChildMeasurePass2) == ScrollViewer.Flags.InChildMeasurePass2;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InChildMeasurePass2, value);
			}
		}

		// Token: 0x17001A98 RID: 6808
		// (get) Token: 0x060072B4 RID: 29364 RVA: 0x002DFCDF File Offset: 0x002DECDF
		// (set) Token: 0x060072B5 RID: 29365 RVA: 0x002DFCF4 File Offset: 0x002DECF4
		internal bool InChildMeasurePass3
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InChildMeasurePass3) == ScrollViewer.Flags.InChildMeasurePass3;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InChildMeasurePass3, value);
			}
		}

		// Token: 0x060072B6 RID: 29366 RVA: 0x002DFD04 File Offset: 0x002DED04
		static ScrollViewer()
		{
			ScrollViewer.ScrollChangedEvent = EventManager.RegisterRoutedEvent("ScrollChanged", RoutingStrategy.Bubble, typeof(ScrollChangedEventHandler), typeof(ScrollViewer));
			ScrollViewer.PanningModeProperty = DependencyProperty.RegisterAttached("PanningMode", typeof(PanningMode), typeof(ScrollViewer), new FrameworkPropertyMetadata(PanningMode.None, new PropertyChangedCallback(ScrollViewer.OnPanningModeChanged)));
			ScrollViewer.PanningDecelerationProperty = DependencyProperty.RegisterAttached("PanningDeceleration", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.001), new ValidateValueCallback(ScrollViewer.CheckFiniteNonNegative));
			ScrollViewer.PanningRatioProperty = DependencyProperty.RegisterAttached("PanningRatio", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(1.0), new ValidateValueCallback(ScrollViewer.CheckFiniteNonNegative));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(typeof(ScrollViewer)));
			ScrollViewer._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ScrollViewer));
			ScrollViewer.InitializeCommands();
			ControlTemplate defaultValue = ScrollViewer.CreateDefaultControlTemplate();
			Control.TemplateProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(defaultValue));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			EventManager.RegisterClassHandler(typeof(ScrollViewer), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(ScrollViewer.OnRequestBringIntoView));
			ControlsTraceLogger.AddControl(TelemetryControls.ScrollViewer);
		}

		// Token: 0x060072B7 RID: 29367 RVA: 0x002E02B4 File Offset: 0x002DF2B4
		private static bool IsValidScrollBarVisibility(object o)
		{
			ScrollBarVisibility scrollBarVisibility = (ScrollBarVisibility)o;
			return scrollBarVisibility == ScrollBarVisibility.Disabled || scrollBarVisibility == ScrollBarVisibility.Auto || scrollBarVisibility == ScrollBarVisibility.Hidden || scrollBarVisibility == ScrollBarVisibility.Visible;
		}

		// Token: 0x17001A99 RID: 6809
		// (get) Token: 0x060072B8 RID: 29368 RVA: 0x001FD464 File Offset: 0x001FC464
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17001A9A RID: 6810
		// (get) Token: 0x060072B9 RID: 29369 RVA: 0x002E02D9 File Offset: 0x002DF2D9
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ScrollViewer._dType;
			}
		}

		// Token: 0x04003763 RID: 14179
		[CommonDependencyProperty]
		public static readonly DependencyProperty CanContentScrollProperty = DependencyProperty.RegisterAttached("CanContentScroll", typeof(bool), typeof(ScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003764 RID: 14180
		[CommonDependencyProperty]
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.RegisterAttached("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ScrollViewer.IsValidScrollBarVisibility));

		// Token: 0x04003765 RID: 14181
		[CommonDependencyProperty]
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.RegisterAttached("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Visible, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ScrollViewer.IsValidScrollBarVisibility));

		// Token: 0x04003766 RID: 14182
		private static readonly DependencyPropertyKey ComputedHorizontalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("ComputedHorizontalScrollBarVisibility", typeof(Visibility), typeof(ScrollViewer), new FrameworkPropertyMetadata(Visibility.Visible));

		// Token: 0x04003767 RID: 14183
		public static readonly DependencyProperty ComputedHorizontalScrollBarVisibilityProperty = ScrollViewer.ComputedHorizontalScrollBarVisibilityPropertyKey.DependencyProperty;

		// Token: 0x04003768 RID: 14184
		private static readonly DependencyPropertyKey ComputedVerticalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("ComputedVerticalScrollBarVisibility", typeof(Visibility), typeof(ScrollViewer), new FrameworkPropertyMetadata(Visibility.Visible));

		// Token: 0x04003769 RID: 14185
		public static readonly DependencyProperty ComputedVerticalScrollBarVisibilityProperty = ScrollViewer.ComputedVerticalScrollBarVisibilityPropertyKey.DependencyProperty;

		// Token: 0x0400376A RID: 14186
		private static readonly DependencyPropertyKey VerticalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("VerticalOffset", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x0400376B RID: 14187
		public static readonly DependencyProperty VerticalOffsetProperty = ScrollViewer.VerticalOffsetPropertyKey.DependencyProperty;

		// Token: 0x0400376C RID: 14188
		private static readonly DependencyPropertyKey HorizontalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("HorizontalOffset", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x0400376D RID: 14189
		public static readonly DependencyProperty HorizontalOffsetProperty = ScrollViewer.HorizontalOffsetPropertyKey.DependencyProperty;

		// Token: 0x0400376E RID: 14190
		private static readonly DependencyPropertyKey ContentVerticalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("ContentVerticalOffset", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x0400376F RID: 14191
		public static readonly DependencyProperty ContentVerticalOffsetProperty = ScrollViewer.ContentVerticalOffsetPropertyKey.DependencyProperty;

		// Token: 0x04003770 RID: 14192
		private static readonly DependencyPropertyKey ContentHorizontalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("ContentHorizontalOffset", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04003771 RID: 14193
		public static readonly DependencyProperty ContentHorizontalOffsetProperty = ScrollViewer.ContentHorizontalOffsetPropertyKey.DependencyProperty;

		// Token: 0x04003772 RID: 14194
		private static readonly DependencyPropertyKey ExtentWidthPropertyKey = DependencyProperty.RegisterReadOnly("ExtentWidth", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04003773 RID: 14195
		public static readonly DependencyProperty ExtentWidthProperty = ScrollViewer.ExtentWidthPropertyKey.DependencyProperty;

		// Token: 0x04003774 RID: 14196
		private static readonly DependencyPropertyKey ExtentHeightPropertyKey = DependencyProperty.RegisterReadOnly("ExtentHeight", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04003775 RID: 14197
		public static readonly DependencyProperty ExtentHeightProperty = ScrollViewer.ExtentHeightPropertyKey.DependencyProperty;

		// Token: 0x04003776 RID: 14198
		private static readonly DependencyPropertyKey ScrollableWidthPropertyKey = DependencyProperty.RegisterReadOnly("ScrollableWidth", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04003777 RID: 14199
		public static readonly DependencyProperty ScrollableWidthProperty = ScrollViewer.ScrollableWidthPropertyKey.DependencyProperty;

		// Token: 0x04003778 RID: 14200
		private static readonly DependencyPropertyKey ScrollableHeightPropertyKey = DependencyProperty.RegisterReadOnly("ScrollableHeight", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04003779 RID: 14201
		public static readonly DependencyProperty ScrollableHeightProperty = ScrollViewer.ScrollableHeightPropertyKey.DependencyProperty;

		// Token: 0x0400377A RID: 14202
		private static readonly DependencyPropertyKey ViewportWidthPropertyKey = DependencyProperty.RegisterReadOnly("ViewportWidth", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x0400377B RID: 14203
		public static readonly DependencyProperty ViewportWidthProperty = ScrollViewer.ViewportWidthPropertyKey.DependencyProperty;

		// Token: 0x0400377C RID: 14204
		internal static readonly DependencyPropertyKey ViewportHeightPropertyKey = DependencyProperty.RegisterReadOnly("ViewportHeight", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		// Token: 0x0400377D RID: 14205
		public static readonly DependencyProperty ViewportHeightProperty = ScrollViewer.ViewportHeightPropertyKey.DependencyProperty;

		// Token: 0x0400377E RID: 14206
		public static readonly DependencyProperty IsDeferredScrollingEnabledProperty = DependencyProperty.RegisterAttached("IsDeferredScrollingEnabled", typeof(bool), typeof(ScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003780 RID: 14208
		public static readonly DependencyProperty PanningModeProperty;

		// Token: 0x04003781 RID: 14209
		public static readonly DependencyProperty PanningDecelerationProperty;

		// Token: 0x04003782 RID: 14210
		public static readonly DependencyProperty PanningRatioProperty;

		// Token: 0x04003783 RID: 14211
		private bool _seenTapGesture;

		// Token: 0x04003784 RID: 14212
		internal const double _scrollLineDelta = 16.0;

		// Token: 0x04003785 RID: 14213
		internal const double _mouseWheelDelta = 48.0;

		// Token: 0x04003786 RID: 14214
		private const string HorizontalScrollBarTemplateName = "PART_HorizontalScrollBar";

		// Token: 0x04003787 RID: 14215
		private const string VerticalScrollBarTemplateName = "PART_VerticalScrollBar";

		// Token: 0x04003788 RID: 14216
		internal const string ScrollContentPresenterTemplateName = "PART_ScrollContentPresenter";

		// Token: 0x04003789 RID: 14217
		private Visibility _scrollVisibilityX;

		// Token: 0x0400378A RID: 14218
		private Visibility _scrollVisibilityY;

		// Token: 0x0400378B RID: 14219
		private double _xPositionISI;

		// Token: 0x0400378C RID: 14220
		private double _yPositionISI;

		// Token: 0x0400378D RID: 14221
		private double _xExtent;

		// Token: 0x0400378E RID: 14222
		private double _yExtent;

		// Token: 0x0400378F RID: 14223
		private double _xSize;

		// Token: 0x04003790 RID: 14224
		private double _ySize;

		// Token: 0x04003791 RID: 14225
		private EventHandler _layoutUpdatedHandler;

		// Token: 0x04003792 RID: 14226
		private IScrollInfo _scrollInfo;

		// Token: 0x04003793 RID: 14227
		private ScrollViewer.CommandQueue _queue;

		// Token: 0x04003794 RID: 14228
		private ScrollViewer.PanningInfo _panningInfo;

		// Token: 0x04003795 RID: 14229
		private ScrollViewer.Flags _flags = ScrollViewer.Flags.HandlesMouseWheelScrolling;

		// Token: 0x04003796 RID: 14230
		private static DependencyObjectType _dType;

		// Token: 0x02000C15 RID: 3093
		private class PanningInfo
		{
			// Token: 0x17001F8B RID: 8075
			// (get) Token: 0x06009064 RID: 36964 RVA: 0x00346829 File Offset: 0x00345829
			// (set) Token: 0x06009065 RID: 36965 RVA: 0x00346831 File Offset: 0x00345831
			public PanningMode PanningMode { get; set; }

			// Token: 0x17001F8C RID: 8076
			// (get) Token: 0x06009066 RID: 36966 RVA: 0x0034683A File Offset: 0x0034583A
			// (set) Token: 0x06009067 RID: 36967 RVA: 0x00346842 File Offset: 0x00345842
			public double OriginalHorizontalOffset { get; set; }

			// Token: 0x17001F8D RID: 8077
			// (get) Token: 0x06009068 RID: 36968 RVA: 0x0034684B File Offset: 0x0034584B
			// (set) Token: 0x06009069 RID: 36969 RVA: 0x00346853 File Offset: 0x00345853
			public double OriginalVerticalOffset { get; set; }

			// Token: 0x17001F8E RID: 8078
			// (get) Token: 0x0600906A RID: 36970 RVA: 0x0034685C File Offset: 0x0034585C
			// (set) Token: 0x0600906B RID: 36971 RVA: 0x00346864 File Offset: 0x00345864
			public double DeltaPerHorizontalOffet { get; set; }

			// Token: 0x17001F8F RID: 8079
			// (get) Token: 0x0600906C RID: 36972 RVA: 0x0034686D File Offset: 0x0034586D
			// (set) Token: 0x0600906D RID: 36973 RVA: 0x00346875 File Offset: 0x00345875
			public double DeltaPerVerticalOffset { get; set; }

			// Token: 0x17001F90 RID: 8080
			// (get) Token: 0x0600906E RID: 36974 RVA: 0x0034687E File Offset: 0x0034587E
			// (set) Token: 0x0600906F RID: 36975 RVA: 0x00346886 File Offset: 0x00345886
			public bool IsPanning { get; set; }

			// Token: 0x17001F91 RID: 8081
			// (get) Token: 0x06009070 RID: 36976 RVA: 0x0034688F File Offset: 0x0034588F
			// (set) Token: 0x06009071 RID: 36977 RVA: 0x00346897 File Offset: 0x00345897
			public Vector UnusedTranslation { get; set; }

			// Token: 0x17001F92 RID: 8082
			// (get) Token: 0x06009072 RID: 36978 RVA: 0x003468A0 File Offset: 0x003458A0
			// (set) Token: 0x06009073 RID: 36979 RVA: 0x003468A8 File Offset: 0x003458A8
			public bool InHorizontalFeedback { get; set; }

			// Token: 0x17001F93 RID: 8083
			// (get) Token: 0x06009074 RID: 36980 RVA: 0x003468B1 File Offset: 0x003458B1
			// (set) Token: 0x06009075 RID: 36981 RVA: 0x003468B9 File Offset: 0x003458B9
			public bool InVerticalFeedback { get; set; }

			// Token: 0x17001F94 RID: 8084
			// (get) Token: 0x06009076 RID: 36982 RVA: 0x003468C2 File Offset: 0x003458C2
			// (set) Token: 0x06009077 RID: 36983 RVA: 0x003468CA File Offset: 0x003458CA
			public int InertiaBoundaryBeginTimestamp { get; set; }

			// Token: 0x04004AF5 RID: 19189
			public const double PrePanTranslation = 3.0;

			// Token: 0x04004AF6 RID: 19190
			public const double MaxInertiaBoundaryTranslation = 50.0;

			// Token: 0x04004AF7 RID: 19191
			public const double PreFeedbackTranslationX = 8.0;

			// Token: 0x04004AF8 RID: 19192
			public const double PreFeedbackTranslationY = 5.0;

			// Token: 0x04004AF9 RID: 19193
			public const int InertiaBoundryMinimumTicks = 100;
		}

		// Token: 0x02000C16 RID: 3094
		private enum Commands
		{
			// Token: 0x04004AFB RID: 19195
			Invalid,
			// Token: 0x04004AFC RID: 19196
			LineUp,
			// Token: 0x04004AFD RID: 19197
			LineDown,
			// Token: 0x04004AFE RID: 19198
			LineLeft,
			// Token: 0x04004AFF RID: 19199
			LineRight,
			// Token: 0x04004B00 RID: 19200
			PageUp,
			// Token: 0x04004B01 RID: 19201
			PageDown,
			// Token: 0x04004B02 RID: 19202
			PageLeft,
			// Token: 0x04004B03 RID: 19203
			PageRight,
			// Token: 0x04004B04 RID: 19204
			SetHorizontalOffset,
			// Token: 0x04004B05 RID: 19205
			SetVerticalOffset,
			// Token: 0x04004B06 RID: 19206
			MakeVisible
		}

		// Token: 0x02000C17 RID: 3095
		private struct Command
		{
			// Token: 0x06009079 RID: 36985 RVA: 0x003468D3 File Offset: 0x003458D3
			internal Command(ScrollViewer.Commands code, double param, ScrollViewer.MakeVisibleParams mvp)
			{
				this.Code = code;
				this.Param = param;
				this.MakeVisibleParam = mvp;
			}

			// Token: 0x04004B07 RID: 19207
			internal ScrollViewer.Commands Code;

			// Token: 0x04004B08 RID: 19208
			internal double Param;

			// Token: 0x04004B09 RID: 19209
			internal ScrollViewer.MakeVisibleParams MakeVisibleParam;
		}

		// Token: 0x02000C18 RID: 3096
		private class MakeVisibleParams
		{
			// Token: 0x0600907A RID: 36986 RVA: 0x003468EA File Offset: 0x003458EA
			internal MakeVisibleParams(Visual child, Rect targetRect)
			{
				this.Child = child;
				this.TargetRect = targetRect;
			}

			// Token: 0x04004B0A RID: 19210
			internal Visual Child;

			// Token: 0x04004B0B RID: 19211
			internal Rect TargetRect;
		}

		// Token: 0x02000C19 RID: 3097
		private struct CommandQueue
		{
			// Token: 0x0600907B RID: 36987 RVA: 0x00346900 File Offset: 0x00345900
			internal void Enqueue(ScrollViewer.Command command)
			{
				if (this._lastWritePosition == this._lastReadPosition)
				{
					this._array = new ScrollViewer.Command[32];
					this._lastWritePosition = (this._lastReadPosition = 0);
				}
				if (!this.OptimizeCommand(command))
				{
					this._lastWritePosition = (this._lastWritePosition + 1) % 32;
					if (this._lastWritePosition == this._lastReadPosition)
					{
						this._lastReadPosition = (this._lastReadPosition + 1) % 32;
					}
					this._array[this._lastWritePosition] = command;
				}
			}

			// Token: 0x0600907C RID: 36988 RVA: 0x00346984 File Offset: 0x00345984
			private bool OptimizeCommand(ScrollViewer.Command command)
			{
				if (this._lastWritePosition != this._lastReadPosition && ((command.Code == ScrollViewer.Commands.SetHorizontalOffset && this._array[this._lastWritePosition].Code == ScrollViewer.Commands.SetHorizontalOffset) || (command.Code == ScrollViewer.Commands.SetVerticalOffset && this._array[this._lastWritePosition].Code == ScrollViewer.Commands.SetVerticalOffset) || (command.Code == ScrollViewer.Commands.MakeVisible && this._array[this._lastWritePosition].Code == ScrollViewer.Commands.MakeVisible)))
				{
					this._array[this._lastWritePosition].Param = command.Param;
					this._array[this._lastWritePosition].MakeVisibleParam = command.MakeVisibleParam;
					return true;
				}
				return false;
			}

			// Token: 0x0600907D RID: 36989 RVA: 0x00346A4C File Offset: 0x00345A4C
			internal ScrollViewer.Command Fetch()
			{
				if (this._lastWritePosition == this._lastReadPosition)
				{
					return new ScrollViewer.Command(ScrollViewer.Commands.Invalid, 0.0, null);
				}
				this._lastReadPosition = (this._lastReadPosition + 1) % 32;
				ScrollViewer.Command result = this._array[this._lastReadPosition];
				this._array[this._lastReadPosition].MakeVisibleParam = null;
				if (this._lastWritePosition == this._lastReadPosition)
				{
					this._array = null;
				}
				return result;
			}

			// Token: 0x0600907E RID: 36990 RVA: 0x00346AC6 File Offset: 0x00345AC6
			internal bool IsEmpty()
			{
				return this._lastWritePosition == this._lastReadPosition;
			}

			// Token: 0x04004B0C RID: 19212
			private const int _capacity = 32;

			// Token: 0x04004B0D RID: 19213
			private int _lastWritePosition;

			// Token: 0x04004B0E RID: 19214
			private int _lastReadPosition;

			// Token: 0x04004B0F RID: 19215
			private ScrollViewer.Command[] _array;
		}

		// Token: 0x02000C1A RID: 3098
		[Flags]
		private enum Flags
		{
			// Token: 0x04004B11 RID: 19217
			None = 0,
			// Token: 0x04004B12 RID: 19218
			InvalidatedMeasureFromArrange = 1,
			// Token: 0x04004B13 RID: 19219
			InChildInvalidateMeasure = 2,
			// Token: 0x04004B14 RID: 19220
			HandlesMouseWheelScrolling = 4,
			// Token: 0x04004B15 RID: 19221
			ForceNextManipulationComplete = 8,
			// Token: 0x04004B16 RID: 19222
			ManipulationBindingsInitialized = 16,
			// Token: 0x04004B17 RID: 19223
			CompleteScrollManipulation = 32,
			// Token: 0x04004B18 RID: 19224
			InChildMeasurePass1 = 64,
			// Token: 0x04004B19 RID: 19225
			InChildMeasurePass2 = 128,
			// Token: 0x04004B1A RID: 19226
			InChildMeasurePass3 = 192
		}
	}
}
