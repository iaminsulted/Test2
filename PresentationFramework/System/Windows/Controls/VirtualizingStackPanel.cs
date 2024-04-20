using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Utility;

namespace System.Windows.Controls
{
	// Token: 0x02000809 RID: 2057
	public class VirtualizingStackPanel : VirtualizingPanel, IScrollInfo, IStackMeasure
	{
		// Token: 0x0600779D RID: 30621 RVA: 0x002F329C File Offset: 0x002F229C
		public VirtualizingStackPanel()
		{
			base.IsVisibleChanged += this.OnIsVisibleChanged;
		}

		// Token: 0x0600779E RID: 30622 RVA: 0x002F32B8 File Offset: 0x002F22B8
		static VirtualizingStackPanel()
		{
			object synchronized = DependencyProperty.Synchronized;
			lock (synchronized)
			{
				VirtualizingStackPanel._indicesStoredInItemValueStorage = new int[]
				{
					VirtualizingStackPanel.ContainerSizeProperty.GlobalIndex,
					VirtualizingStackPanel.ContainerSizeDualProperty.GlobalIndex,
					VirtualizingStackPanel.AreContainersUniformlySizedProperty.GlobalIndex,
					VirtualizingStackPanel.UniformOrAverageContainerSizeProperty.GlobalIndex,
					VirtualizingStackPanel.UniformOrAverageContainerSizeDualProperty.GlobalIndex,
					VirtualizingStackPanel.ItemsHostInsetProperty.GlobalIndex
				};
			}
		}

		// Token: 0x0600779F RID: 30623 RVA: 0x002F34F8 File Offset: 0x002F24F8
		public virtual void LineUp()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LineUp, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset - 16.0) : this.NewItemOffset(flag, -1.0, true);
			this.SetVerticalOffsetImpl(offset, true);
		}

		// Token: 0x060077A0 RID: 30624 RVA: 0x002F3560 File Offset: 0x002F2560
		public virtual void LineDown()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LineDown, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset + 16.0) : this.NewItemOffset(flag, 1.0, false);
			this.SetVerticalOffsetImpl(offset, true);
		}

		// Token: 0x060077A1 RID: 30625 RVA: 0x002F35C8 File Offset: 0x002F25C8
		public virtual void LineLeft()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LineLeft, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset - 16.0) : this.NewItemOffset(flag, -1.0, true);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		// Token: 0x060077A2 RID: 30626 RVA: 0x002F3634 File Offset: 0x002F2634
		public virtual void LineRight()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LineRight, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset + 16.0) : this.NewItemOffset(flag, 1.0, false);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		// Token: 0x060077A3 RID: 30627 RVA: 0x002F36A0 File Offset: 0x002F26A0
		public virtual void PageUp()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.PageUp, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset - this.ViewportHeight) : this.NewItemOffset(flag, -this.ViewportHeight, true);
			this.SetVerticalOffsetImpl(offset, true);
		}

		// Token: 0x060077A4 RID: 30628 RVA: 0x002F3704 File Offset: 0x002F2704
		public virtual void PageDown()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.PageDown, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset + this.ViewportHeight) : this.NewItemOffset(flag, this.ViewportHeight, true);
			this.SetVerticalOffsetImpl(offset, true);
		}

		// Token: 0x060077A5 RID: 30629 RVA: 0x002F3768 File Offset: 0x002F2768
		public virtual void PageLeft()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.PageLeft, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset - this.ViewportWidth) : this.NewItemOffset(flag, -this.ViewportWidth, true);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		// Token: 0x060077A6 RID: 30630 RVA: 0x002F37D0 File Offset: 0x002F27D0
		public virtual void PageRight()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.PageRight, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset + this.ViewportWidth) : this.NewItemOffset(flag, this.ViewportWidth, true);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		// Token: 0x060077A7 RID: 30631 RVA: 0x002F3834 File Offset: 0x002F2834
		public virtual void MouseWheelUp()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MouseWheelUp, Array.Empty<object>());
				}
				int wheelScrollLines = SystemParameters.WheelScrollLines;
				bool flag = this.Orientation == Orientation.Horizontal;
				double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset - (double)wheelScrollLines * 16.0) : this.NewItemOffset(flag, (double)(-(double)wheelScrollLines), true);
				this.SetVerticalOffsetImpl(offset, true);
				return;
			}
			this.PageUp();
		}

		// Token: 0x060077A8 RID: 30632 RVA: 0x002F38B0 File Offset: 0x002F28B0
		public virtual void MouseWheelDown()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MouseWheelDown, Array.Empty<object>());
				}
				int wheelScrollLines = SystemParameters.WheelScrollLines;
				bool flag = this.Orientation == Orientation.Horizontal;
				double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset + (double)wheelScrollLines * 16.0) : this.NewItemOffset(flag, (double)wheelScrollLines, false);
				this.SetVerticalOffsetImpl(offset, true);
				return;
			}
			this.PageDown();
		}

		// Token: 0x060077A9 RID: 30633 RVA: 0x002F392C File Offset: 0x002F292C
		public virtual void MouseWheelLeft()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MouseWheelLeft, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset - 48.0) : this.NewItemOffset(flag, -3.0, true);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		// Token: 0x060077AA RID: 30634 RVA: 0x002F3998 File Offset: 0x002F2998
		public virtual void MouseWheelRight()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MouseWheelRight, Array.Empty<object>());
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset + 48.0) : this.NewItemOffset(flag, 3.0, false);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		// Token: 0x060077AB RID: 30635 RVA: 0x002F3A04 File Offset: 0x002F2A04
		private double NewItemOffset(bool isHorizontal, double delta, bool fromFirst)
		{
			if (DoubleUtil.IsZero(delta))
			{
				delta = 1.0;
			}
			if (VirtualizingStackPanel.IsVSP45Compat)
			{
				return (isHorizontal ? this.HorizontalOffset : this.VerticalOffset) + delta;
			}
			double num;
			FrameworkElement frameworkElement = this.ComputeFirstContainerInViewport(this, isHorizontal ? FocusNavigationDirection.Right : FocusNavigationDirection.Down, this, null, true, out num);
			if (frameworkElement == null || DoubleUtil.IsZero(num))
			{
				return (isHorizontal ? this.HorizontalOffset : this.VerticalOffset) + delta;
			}
			double num2 = this.FindScrollOffset(frameworkElement);
			if (fromFirst)
			{
				num2 -= num;
			}
			if (isHorizontal)
			{
				this._scrollData._computedOffset.X = num2;
			}
			else
			{
				this._scrollData._computedOffset.Y = num2;
			}
			return num2 + delta;
		}

		// Token: 0x060077AC RID: 30636 RVA: 0x002F3AAC File Offset: 0x002F2AAC
		public void SetHorizontalOffset(double offset)
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetHorizontalOffset, new object[]
				{
					offset,
					"delta:",
					offset - this.HorizontalOffset
				});
			}
			this.ClearAnchorInformation(true);
			this.SetHorizontalOffsetImpl(offset, false);
		}

		// Token: 0x060077AD RID: 30637 RVA: 0x002F3B08 File Offset: 0x002F2B08
		private void SetHorizontalOffsetImpl(double offset, bool setAnchorInformation)
		{
			if (!this.IsScrolling)
			{
				return;
			}
			double num = ScrollContentPresenter.ValidateInputOffset(offset, "HorizontalOffset");
			if (!DoubleUtil.AreClose(num, this._scrollData._offset.X))
			{
				Vector offset2 = this._scrollData._offset;
				this._scrollData._offset.X = num;
				this.OnViewportOffsetChanged(offset2, this._scrollData._offset);
				if (this.IsVirtualizing)
				{
					this.IsScrollActive = true;
					this._scrollData.SetHorizontalScrollType(offset2.X, num);
					base.InvalidateMeasure();
					if (!VirtualizingStackPanel.IsVSP45Compat && this.Orientation == Orientation.Horizontal)
					{
						this.IncrementScrollGeneration();
						if (DoubleUtil.LessThanOrClose(Math.Abs(num - offset2.X), this.ViewportWidth))
						{
							if (!this.IsPixelBased)
							{
								this._scrollData._offset.X = Math.Floor(this._scrollData._offset.X);
								this._scrollData._computedOffset.X = Math.Floor(this._scrollData._computedOffset.X);
							}
							else if (base.UseLayoutRounding)
							{
								DpiScale dpi = base.GetDpi();
								this._scrollData._offset.X = UIElement.RoundLayoutValue(this._scrollData._offset.X, dpi.DpiScaleX);
								this._scrollData._computedOffset.X = UIElement.RoundLayoutValue(this._scrollData._computedOffset.X, dpi.DpiScaleX);
							}
							if (!setAnchorInformation && !this.IsPixelBased)
							{
								double num2;
								FrameworkElement v = this.ComputeFirstContainerInViewport(this, FocusNavigationDirection.Right, this, null, true, out num2);
								if (num2 > 0.0)
								{
									double x = this.FindScrollOffset(v);
									this._scrollData._computedOffset.X = x;
								}
							}
							setAnchorInformation = true;
						}
					}
				}
				else if (!this.IsPixelBased)
				{
					base.InvalidateMeasure();
				}
				else
				{
					this._scrollData._offset.X = ScrollContentPresenter.CoerceOffset(num, this._scrollData._extent.Width, this._scrollData._viewport.Width);
					this._scrollData._computedOffset.X = this._scrollData._offset.X;
					base.InvalidateArrange();
					this.OnScrollChange();
				}
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetHOff, new object[]
					{
						this._scrollData._offset,
						this._scrollData._extent,
						this._scrollData._computedOffset
					});
				}
			}
			if (setAnchorInformation)
			{
				this.SetAnchorInformation(true);
			}
		}

		// Token: 0x060077AE RID: 30638 RVA: 0x002F3DAC File Offset: 0x002F2DAC
		public void SetVerticalOffset(double offset)
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetVerticalOffset, new object[]
				{
					offset,
					"delta:",
					offset - this.VerticalOffset
				});
			}
			this.ClearAnchorInformation(true);
			this.SetVerticalOffsetImpl(offset, false);
		}

		// Token: 0x060077AF RID: 30639 RVA: 0x002F3E08 File Offset: 0x002F2E08
		private void SetVerticalOffsetImpl(double offset, bool setAnchorInformation)
		{
			if (!this.IsScrolling)
			{
				return;
			}
			double num = ScrollContentPresenter.ValidateInputOffset(offset, "VerticalOffset");
			if (!DoubleUtil.AreClose(num, this._scrollData._offset.Y))
			{
				Vector offset2 = this._scrollData._offset;
				this._scrollData._offset.Y = num;
				this.OnViewportOffsetChanged(offset2, this._scrollData._offset);
				if (this.IsVirtualizing)
				{
					base.InvalidateMeasure();
					this.IsScrollActive = true;
					this._scrollData.SetVerticalScrollType(offset2.Y, num);
					if (!VirtualizingStackPanel.IsVSP45Compat && this.Orientation == Orientation.Vertical)
					{
						this.IncrementScrollGeneration();
						if (DoubleUtil.LessThanOrClose(Math.Abs(num - offset2.Y), this.ViewportHeight))
						{
							if (!this.IsPixelBased)
							{
								this._scrollData._offset.Y = Math.Floor(this._scrollData._offset.Y);
								this._scrollData._computedOffset.Y = Math.Floor(this._scrollData._computedOffset.Y);
							}
							else if (base.UseLayoutRounding)
							{
								DpiScale dpi = base.GetDpi();
								this._scrollData._offset.Y = UIElement.RoundLayoutValue(this._scrollData._offset.Y, dpi.DpiScaleY);
								this._scrollData._computedOffset.Y = UIElement.RoundLayoutValue(this._scrollData._computedOffset.Y, dpi.DpiScaleY);
							}
							if (!setAnchorInformation && !this.IsPixelBased)
							{
								double num2;
								FrameworkElement v = this.ComputeFirstContainerInViewport(this, FocusNavigationDirection.Down, this, null, true, out num2);
								if (num2 > 0.0)
								{
									double y = this.FindScrollOffset(v);
									this._scrollData._computedOffset.Y = y;
								}
							}
							setAnchorInformation = true;
						}
					}
				}
				else if (!this.IsPixelBased)
				{
					base.InvalidateMeasure();
				}
				else
				{
					this._scrollData._offset.Y = ScrollContentPresenter.CoerceOffset(num, this._scrollData._extent.Height, this._scrollData._viewport.Height);
					this._scrollData._computedOffset.Y = this._scrollData._offset.Y;
					base.InvalidateArrange();
					this.OnScrollChange();
				}
			}
			if (setAnchorInformation)
			{
				this.SetAnchorInformation(false);
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetVOff, new object[]
				{
					this._scrollData._offset,
					this._scrollData._extent,
					this._scrollData._computedOffset
				});
			}
		}

		// Token: 0x060077B0 RID: 30640 RVA: 0x002F40B0 File Offset: 0x002F30B0
		private void SetAnchorInformation(bool isHorizontalOffset)
		{
			if (this.IsScrolling && this.IsVirtualizing)
			{
				bool flag = this.Orientation == Orientation.Horizontal;
				if (flag == isHorizontalOffset && (!this.GetAreContainersUniformlySized(null, this) || this.HasVirtualizingChildren))
				{
					ItemsControl itemsControl;
					ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
					if (itemsControl != null)
					{
						bool flag2 = VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this);
						double num = flag ? (this._scrollData._offset.X - this._scrollData._computedOffset.X) : (this._scrollData._offset.Y - this._scrollData._computedOffset.Y);
						if (flag2)
						{
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.BSetAnchor, new object[]
							{
								num
							});
						}
						if (this._scrollData._firstContainerInViewport != null)
						{
							this.OnAnchorOperation(true);
							if (flag)
							{
								VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
								scrollData._offset.X = scrollData._offset.X + num;
							}
							else
							{
								VirtualizingStackPanel.ScrollData scrollData2 = this._scrollData;
								scrollData2._offset.Y = scrollData2._offset.Y + num;
							}
						}
						if (this._scrollData._firstContainerInViewport == null)
						{
							this._scrollData._firstContainerInViewport = this.ComputeFirstContainerInViewport(itemsControl.GetViewportElement(), flag ? FocusNavigationDirection.Right : FocusNavigationDirection.Down, this, delegate(DependencyObject d)
							{
								d.SetCurrentValue(VirtualizingPanel.IsContainerVirtualizableProperty, false);
							}, false, out this._scrollData._firstContainerOffsetFromViewport);
							if (this._scrollData._firstContainerInViewport != null)
							{
								this._scrollData._expectedDistanceBetweenViewports = num;
								DispatcherOperation value = base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(this.OnAnchorOperation));
								VirtualizingStackPanel.AnchorOperationField.SetValue(this, value);
							}
						}
						else
						{
							this._scrollData._expectedDistanceBetweenViewports += num;
						}
						if (flag2)
						{
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ESetAnchor, new object[]
							{
								this._scrollData._expectedDistanceBetweenViewports,
								this._scrollData._firstContainerInViewport,
								this._scrollData._firstContainerOffsetFromViewport
							});
						}
					}
				}
			}
		}

		// Token: 0x060077B1 RID: 30641 RVA: 0x002F42B8 File Offset: 0x002F32B8
		private void OnAnchorOperation()
		{
			bool isAnchorOperationPending = false;
			this.OnAnchorOperation(isAnchorOperationPending);
		}

		// Token: 0x060077B2 RID: 30642 RVA: 0x002F42D0 File Offset: 0x002F32D0
		private void OnAnchorOperation(bool isAnchorOperationPending)
		{
			ItemsControl itemsControl;
			ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
			if (itemsControl == null || !VisualTreeHelper.IsAncestorOf(this, this._scrollData._firstContainerInViewport))
			{
				this.ClearAnchorInformation(isAnchorOperationPending);
				return;
			}
			bool flag = VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this);
			if (flag)
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.BOnAnchor, new object[]
				{
					isAnchorOperationPending,
					this._scrollData._expectedDistanceBetweenViewports,
					this._scrollData._firstContainerInViewport
				});
			}
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			if (!isVSP45Compat && !isAnchorOperationPending && (base.MeasureDirty || base.ArrangeDirty))
			{
				if (flag)
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ROnAnchor, Array.Empty<object>());
				}
				this.CancelPendingAnchoredInvalidateMeasure();
				DispatcherOperation value = base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.OnAnchorOperation));
				VirtualizingStackPanel.AnchorOperationField.SetValue(this, value);
				return;
			}
			bool flag2 = this.Orientation == Orientation.Horizontal;
			FrameworkElement firstContainerInViewport = this._scrollData._firstContainerInViewport;
			double firstContainerOffsetFromViewport = this._scrollData._firstContainerOffsetFromViewport;
			double num = this.FindScrollOffset(this._scrollData._firstContainerInViewport);
			double num2;
			FrameworkElement v = this.ComputeFirstContainerInViewport(itemsControl.GetViewportElement(), flag2 ? FocusNavigationDirection.Right : FocusNavigationDirection.Down, this, null, false, out num2);
			double num3 = this.FindScrollOffset(v);
			double num4 = num3 - num2 - (num - firstContainerOffsetFromViewport);
			bool flag3 = LayoutDoubleUtil.AreClose(this._scrollData._expectedDistanceBetweenViewports, num4);
			if (!flag3 && !isVSP45Compat && !this.IsPixelBased)
			{
				double num5;
				this.ComputeFirstContainerInViewport(this, flag2 ? FocusNavigationDirection.Right : FocusNavigationDirection.Down, this, null, true, out num5);
				double num6 = num4 - this._scrollData._expectedDistanceBetweenViewports;
				flag3 = (!LayoutDoubleUtil.LessThan(num6, 0.0) && !LayoutDoubleUtil.LessThan(num5, num6));
				if (flag3)
				{
					num2 += num5;
				}
				if (!flag3)
				{
					if (flag2)
					{
						flag3 = DoubleUtil.GreaterThanOrClose(this._scrollData._computedOffset.X, this._scrollData._extent.Width - this._scrollData._viewport.Width);
					}
					else
					{
						flag3 = DoubleUtil.GreaterThanOrClose(this._scrollData._computedOffset.Y, this._scrollData._extent.Height - this._scrollData._viewport.Height);
					}
				}
			}
			if (flag3)
			{
				if (flag2)
				{
					this._scrollData._computedOffset.X = num3 - num2;
					this._scrollData._offset.X = this._scrollData._computedOffset.X;
				}
				else
				{
					this._scrollData._computedOffset.Y = num3 - num2;
					this._scrollData._offset.Y = this._scrollData._computedOffset.Y;
				}
				this.ClearAnchorInformation(isAnchorOperationPending);
				if (flag)
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SOnAnchor, new object[]
					{
						this._scrollData._offset
					});
					return;
				}
			}
			else
			{
				bool flag4 = false;
				double num7;
				double num8;
				if (flag2)
				{
					this._scrollData._computedOffset.X = num - firstContainerOffsetFromViewport;
					num7 = this._scrollData._computedOffset.X + num4;
					num8 = this._scrollData._computedOffset.X + this._scrollData._expectedDistanceBetweenViewports;
					if (DoubleUtil.LessThan(num8, 0.0) || DoubleUtil.GreaterThan(num8, this._scrollData._extent.Width - this._scrollData._viewport.Width))
					{
						if (DoubleUtil.AreClose(num7, 0.0) || DoubleUtil.AreClose(num7, this._scrollData._extent.Width - this._scrollData._viewport.Width))
						{
							this._scrollData._computedOffset.X = num7;
							this._scrollData._offset.X = num7;
						}
						else
						{
							flag4 = true;
							this._scrollData._offset.X = num8;
						}
					}
					else
					{
						flag4 = true;
						this._scrollData._offset.X = num8;
					}
				}
				else
				{
					this._scrollData._computedOffset.Y = num - firstContainerOffsetFromViewport;
					num7 = this._scrollData._computedOffset.Y + num4;
					num8 = this._scrollData._computedOffset.Y + this._scrollData._expectedDistanceBetweenViewports;
					if (DoubleUtil.LessThan(num8, 0.0) || DoubleUtil.GreaterThan(num8, this._scrollData._extent.Height - this._scrollData._viewport.Height))
					{
						if (DoubleUtil.AreClose(num7, 0.0) || DoubleUtil.AreClose(num7, this._scrollData._extent.Height - this._scrollData._viewport.Height))
						{
							this._scrollData._computedOffset.Y = num7;
							this._scrollData._offset.Y = num7;
						}
						else
						{
							flag4 = true;
							this._scrollData._offset.Y = num8;
						}
					}
					else
					{
						flag4 = true;
						this._scrollData._offset.Y = num8;
					}
				}
				if (flag)
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.EOnAnchor, new object[]
					{
						flag4,
						num8,
						num7,
						this._scrollData._offset,
						this._scrollData._computedOffset
					});
				}
				if (flag4)
				{
					this.OnScrollChange();
					base.InvalidateMeasure();
					if (!isVSP45Compat)
					{
						this.CancelPendingAnchoredInvalidateMeasure();
						this.IncrementScrollGeneration();
					}
					if (!isAnchorOperationPending)
					{
						DispatcherOperation value2 = base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(this.OnAnchorOperation));
						VirtualizingStackPanel.AnchorOperationField.SetValue(this, value2);
					}
					if (!isVSP45Compat && this.IsScrollActive)
					{
						DispatcherOperation value3 = VirtualizingStackPanel.ClearIsScrollActiveOperationField.GetValue(this);
						if (value3 != null)
						{
							value3.Abort();
							VirtualizingStackPanel.ClearIsScrollActiveOperationField.SetValue(this, null);
							return;
						}
					}
				}
				else
				{
					this.ClearAnchorInformation(isAnchorOperationPending);
				}
			}
		}

		// Token: 0x060077B3 RID: 30643 RVA: 0x002F48BC File Offset: 0x002F38BC
		private void ClearAnchorInformation(bool shouldAbort)
		{
			if (this._scrollData == null)
			{
				return;
			}
			if (this._scrollData._firstContainerInViewport != null)
			{
				DependencyObject dependencyObject = this._scrollData._firstContainerInViewport;
				do
				{
					DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
					Panel panel = parent as Panel;
					if (panel != null && panel.IsItemsHost)
					{
						dependencyObject.InvalidateProperty(VirtualizingPanel.IsContainerVirtualizableProperty);
					}
					dependencyObject = parent;
				}
				while (dependencyObject != null && dependencyObject != this);
				this._scrollData._firstContainerInViewport = null;
				this._scrollData._firstContainerOffsetFromViewport = 0.0;
				this._scrollData._expectedDistanceBetweenViewports = 0.0;
				if (shouldAbort)
				{
					VirtualizingStackPanel.AnchorOperationField.GetValue(this).Abort();
				}
				VirtualizingStackPanel.AnchorOperationField.ClearValue(this);
			}
		}

		// Token: 0x060077B4 RID: 30644 RVA: 0x002F496C File Offset: 0x002F396C
		private FrameworkElement ComputeFirstContainerInViewport(FrameworkElement viewportElement, FocusNavigationDirection direction, Panel itemsHost, Action<DependencyObject> action, bool findTopContainer, out double firstContainerOffsetFromViewport)
		{
			bool flag;
			return this.ComputeFirstContainerInViewport(viewportElement, direction, itemsHost, action, findTopContainer, out firstContainerOffsetFromViewport, out flag);
		}

		// Token: 0x060077B5 RID: 30645 RVA: 0x002F498C File Offset: 0x002F398C
		private FrameworkElement ComputeFirstContainerInViewport(FrameworkElement viewportElement, FocusNavigationDirection direction, Panel itemsHost, Action<DependencyObject> action, bool findTopContainer, out double firstContainerOffsetFromViewport, out bool foundTopContainer)
		{
			firstContainerOffsetFromViewport = 0.0;
			foundTopContainer = false;
			if (itemsHost == null)
			{
				return null;
			}
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			if (!isVSP45Compat)
			{
				viewportElement = this;
			}
			FrameworkElement frameworkElement = null;
			UIElementCollection children = itemsHost.Children;
			if (children != null)
			{
				int count = children.Count;
				int num = 0;
				for (int i = (itemsHost is VirtualizingStackPanel) ? ((VirtualizingStackPanel)itemsHost)._firstItemInExtendedViewportChildIndex : 0; i < count; i++)
				{
					FrameworkElement frameworkElement2 = children[i] as FrameworkElement;
					if (frameworkElement2 != null)
					{
						if (frameworkElement2.IsVisible)
						{
							Rect rect;
							ElementViewportPosition elementViewportPosition = ItemsControl.GetElementViewportPosition(viewportElement, frameworkElement2, direction, false, !isVSP45Compat, out rect);
							if (elementViewportPosition == ElementViewportPosition.PartiallyInViewport || elementViewportPosition == ElementViewportPosition.CompletelyInViewport)
							{
								bool flag = false;
								if (!this.IsPixelBased)
								{
									double num2 = (direction == FocusNavigationDirection.Down) ? rect.Y : rect.X;
									if (findTopContainer && DoubleUtil.GreaterThan(num2, 0.0))
									{
										break;
									}
									flag = DoubleUtil.IsZero(num2);
								}
								if (action != null)
								{
									action(frameworkElement2);
								}
								if (isVSP45Compat)
								{
									ItemsControl itemsControl = frameworkElement2 as ItemsControl;
									if (itemsControl != null)
									{
										if (itemsControl.ItemsHost != null && itemsControl.ItemsHost.IsVisible)
										{
											frameworkElement = this.ComputeFirstContainerInViewport(viewportElement, direction, itemsControl.ItemsHost, action, findTopContainer, out firstContainerOffsetFromViewport);
										}
									}
									else
									{
										GroupItem groupItem = frameworkElement2 as GroupItem;
										if (groupItem != null && groupItem.ItemsHost != null && groupItem.ItemsHost.IsVisible)
										{
											frameworkElement = this.ComputeFirstContainerInViewport(viewportElement, direction, groupItem.ItemsHost, action, findTopContainer, out firstContainerOffsetFromViewport);
										}
									}
								}
								else
								{
									Panel panel = null;
									ItemsControl itemsControl2;
									GroupItem groupItem2;
									if ((itemsControl2 = (frameworkElement2 as ItemsControl)) != null)
									{
										panel = itemsControl2.ItemsHost;
									}
									else if ((groupItem2 = (frameworkElement2 as GroupItem)) != null)
									{
										panel = groupItem2.ItemsHost;
									}
									panel = (panel as VirtualizingStackPanel);
									if (panel != null && panel.IsVisible)
									{
										frameworkElement = this.ComputeFirstContainerInViewport(viewportElement, direction, panel, action, findTopContainer, out firstContainerOffsetFromViewport, out foundTopContainer);
									}
								}
								if (frameworkElement == null)
								{
									frameworkElement = frameworkElement2;
									foundTopContainer = flag;
									if (this.IsPixelBased)
									{
										if (direction == FocusNavigationDirection.Down)
										{
											firstContainerOffsetFromViewport = rect.Y;
											if (!isVSP45Compat)
											{
												firstContainerOffsetFromViewport -= frameworkElement2.Margin.Top;
												break;
											}
											break;
										}
										else
										{
											firstContainerOffsetFromViewport = rect.X;
											if (!isVSP45Compat)
											{
												firstContainerOffsetFromViewport -= frameworkElement2.Margin.Left;
												break;
											}
											break;
										}
									}
									else
									{
										if (findTopContainer && flag)
										{
											firstContainerOffsetFromViewport += (double)num;
											break;
										}
										break;
									}
								}
								else
								{
									if (this.IsPixelBased)
									{
										break;
									}
									IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = frameworkElement2 as IHierarchicalVirtualizationAndScrollInfo;
									if (hierarchicalVirtualizationAndScrollInfo == null)
									{
										break;
									}
									if (isVSP45Compat)
									{
										if (direction == FocusNavigationDirection.Down)
										{
											if (DoubleUtil.GreaterThanOrClose(rect.Y, 0.0))
											{
												firstContainerOffsetFromViewport += hierarchicalVirtualizationAndScrollInfo.HeaderDesiredSizes.LogicalSize.Height;
												break;
											}
											break;
										}
										else
										{
											if (DoubleUtil.GreaterThanOrClose(rect.X, 0.0))
											{
												firstContainerOffsetFromViewport += hierarchicalVirtualizationAndScrollInfo.HeaderDesiredSizes.LogicalSize.Width;
												break;
											}
											break;
										}
									}
									else
									{
										Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(hierarchicalVirtualizationAndScrollInfo, null, null);
										if (direction == FocusNavigationDirection.Down)
										{
											if (this.IsHeaderBeforeItems(false, frameworkElement2, ref itemsHostInsetForChild) && DoubleUtil.GreaterThanOrClose(rect.Y, 0.0) && (findTopContainer || !foundTopContainer || DoubleUtil.GreaterThan(rect.Y, 0.0)))
											{
												firstContainerOffsetFromViewport += 1.0;
												break;
											}
											break;
										}
										else
										{
											if (this.IsHeaderBeforeItems(true, frameworkElement2, ref itemsHostInsetForChild) && DoubleUtil.GreaterThanOrClose(rect.X, 0.0) && (findTopContainer || !foundTopContainer || DoubleUtil.GreaterThan(rect.X, 0.0)))
											{
												firstContainerOffsetFromViewport += 1.0;
												break;
											}
											break;
										}
									}
								}
							}
							else
							{
								if (elementViewportPosition == ElementViewportPosition.AfterViewport)
								{
									break;
								}
								num = 0;
							}
						}
						else
						{
							num++;
						}
					}
				}
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.CFCIV, new object[]
				{
					this.ContainerPath(frameworkElement),
					firstContainerOffsetFromViewport
				});
			}
			return frameworkElement;
		}

		// Token: 0x060077B6 RID: 30646 RVA: 0x002F4DA8 File Offset: 0x002F3DA8
		internal void AnchoredInvalidateMeasure()
		{
			this.WasLastMeasurePassAnchored = (this.FirstContainerInViewport != null || this.BringIntoViewLeafContainer != null);
			if (VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.GetValue(this) == null)
			{
				DispatcherOperation value = base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(delegate()
				{
					if (VirtualizingStackPanel.IsVSP45Compat)
					{
						VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.ClearValue(this);
						if (this.WasLastMeasurePassAnchored)
						{
							this.SetAnchorInformation(this.Orientation == Orientation.Horizontal);
						}
						base.InvalidateMeasure();
						return;
					}
					base.InvalidateMeasure();
					VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.ClearValue(this);
					if (this.WasLastMeasurePassAnchored)
					{
						this.SetAnchorInformation(this.Orientation == Orientation.Horizontal);
					}
				}));
				VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.SetValue(this, value);
			}
		}

		// Token: 0x060077B7 RID: 30647 RVA: 0x002F4E04 File Offset: 0x002F3E04
		private void CancelPendingAnchoredInvalidateMeasure()
		{
			DispatcherOperation value = VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.GetValue(this);
			if (value != null)
			{
				value.Abort();
				VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.ClearValue(this);
			}
		}

		// Token: 0x060077B8 RID: 30648 RVA: 0x002F4E34 File Offset: 0x002F3E34
		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			this.ClearAnchorInformation(true);
			Vector vector = default(Vector);
			Rect result = default(Rect);
			Rect rect = rectangle;
			bool flag = this.Orientation == Orientation.Horizontal;
			if (rectangle.IsEmpty || visual == null || visual == this || !base.IsAncestorOf(visual))
			{
				return Rect.Empty;
			}
			rectangle = visual.TransformToAncestor(this).TransformBounds(rectangle);
			if (!this.IsScrolling)
			{
				return rectangle;
			}
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			bool alignTopOfBringIntoViewContainer = false;
			bool alignBottomOfBringIntoViewContainer = false;
			this.MakeVisiblePhysicalHelper(rectangle, ref vector, ref result, !flag, ref alignTopOfBringIntoViewContainer, ref alignBottomOfBringIntoViewContainer);
			alignTopOfBringIntoViewContainer = (this._scrollData._bringIntoViewLeafContainer == visual && this.AlignTopOfBringIntoViewContainer);
			alignBottomOfBringIntoViewContainer = (this._scrollData._bringIntoViewLeafContainer == visual && (isVSP45Compat ? (!this.AlignTopOfBringIntoViewContainer) : this.AlignBottomOfBringIntoViewContainer));
			if (this.IsPixelBased)
			{
				this.MakeVisiblePhysicalHelper(rectangle, ref vector, ref result, flag, ref alignTopOfBringIntoViewContainer, ref alignBottomOfBringIntoViewContainer);
			}
			else
			{
				int childIndex = (int)this.FindScrollOffset(visual);
				this.MakeVisibleLogicalHelper(childIndex, rectangle, ref vector, ref result, ref alignTopOfBringIntoViewContainer, ref alignBottomOfBringIntoViewContainer);
			}
			vector.X = ScrollContentPresenter.CoerceOffset(vector.X, this._scrollData._extent.Width, this._scrollData._viewport.Width);
			vector.Y = ScrollContentPresenter.CoerceOffset(vector.Y, this._scrollData._extent.Height, this._scrollData._viewport.Height);
			if (!LayoutDoubleUtil.AreClose(vector.X, this._scrollData._offset.X) || !LayoutDoubleUtil.AreClose(vector.Y, this._scrollData._offset.Y))
			{
				if (visual != this._scrollData._bringIntoViewLeafContainer)
				{
					this._scrollData._bringIntoViewLeafContainer = visual;
					this.AlignTopOfBringIntoViewContainer = alignTopOfBringIntoViewContainer;
					this.AlignBottomOfBringIntoViewContainer = alignBottomOfBringIntoViewContainer;
				}
				Vector offset = this._scrollData._offset;
				this._scrollData._offset = vector;
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MakeVisible, new object[]
					{
						this._scrollData._offset,
						rectangle,
						this._scrollData._bringIntoViewLeafContainer
					});
				}
				this.OnViewportOffsetChanged(offset, vector);
				if (this.IsVirtualizing)
				{
					this.IsScrollActive = true;
					this._scrollData.SetHorizontalScrollType(offset.X, vector.X);
					this._scrollData.SetVerticalScrollType(offset.Y, vector.Y);
					base.InvalidateMeasure();
				}
				else if (!this.IsPixelBased)
				{
					base.InvalidateMeasure();
				}
				else
				{
					this._scrollData._computedOffset = vector;
					base.InvalidateArrange();
				}
				this.OnScrollChange();
				if (this.ScrollOwner != null)
				{
					this.ScrollOwner.MakeVisible(visual, rect);
				}
			}
			else
			{
				if (isVSP45Compat)
				{
					this._scrollData._bringIntoViewLeafContainer = null;
				}
				this.AlignTopOfBringIntoViewContainer = false;
				this.AlignBottomOfBringIntoViewContainer = false;
			}
			return result;
		}

		// Token: 0x060077B9 RID: 30649 RVA: 0x002F5114 File Offset: 0x002F4114
		protected internal override void BringIndexIntoView(int index)
		{
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			if (!itemsOwner.IsGrouping)
			{
				this.BringContainerIntoView(itemsOwner, index);
				return;
			}
			base.EnsureGenerator();
			ItemContainerGenerator itemContainerGenerator = (ItemContainerGenerator)base.Generator;
			IList itemsInternal = itemContainerGenerator.ItemsInternal;
			for (int i = 0; i < itemsInternal.Count; i++)
			{
				CollectionViewGroup collectionViewGroup = itemsInternal[i] as CollectionViewGroup;
				if (collectionViewGroup != null)
				{
					if (index >= collectionViewGroup.ItemCount)
					{
						index -= collectionViewGroup.ItemCount;
					}
					else
					{
						GroupItem groupItem = itemContainerGenerator.ContainerFromItem(collectionViewGroup) as GroupItem;
						if (groupItem == null)
						{
							this.BringContainerIntoView(itemsOwner, i);
							groupItem = (itemContainerGenerator.ContainerFromItem(collectionViewGroup) as GroupItem);
						}
						if (groupItem == null)
						{
							break;
						}
						groupItem.UpdateLayout();
						VirtualizingPanel virtualizingPanel = groupItem.ItemsHost as VirtualizingPanel;
						if (virtualizingPanel != null)
						{
							virtualizingPanel.BringIndexIntoViewPublic(index);
							return;
						}
						break;
					}
				}
				else if (i == index)
				{
					this.BringContainerIntoView(itemsOwner, i);
				}
			}
		}

		// Token: 0x060077BA RID: 30650 RVA: 0x002F51F0 File Offset: 0x002F41F0
		private void BringContainerIntoView(ItemsControl itemsControl, int itemIndex)
		{
			if (itemIndex < 0 || itemIndex >= this.ItemCount)
			{
				throw new ArgumentOutOfRangeException("itemIndex");
			}
			IItemContainerGenerator generator = base.Generator;
			int childIndex;
			GeneratorPosition position = this.IndexToGeneratorPositionForStart(itemIndex, out childIndex);
			UIElement uielement;
			using (generator.StartAt(position, GeneratorDirection.Forward, true))
			{
				bool newlyRealized;
				uielement = (generator.GenerateNext(out newlyRealized) as UIElement);
				if (uielement != null && this.AddContainerFromGenerator(childIndex, uielement, newlyRealized, false))
				{
					base.InvalidateZState();
				}
			}
			if (uielement != null)
			{
				FrameworkElement frameworkElement = uielement as FrameworkElement;
				if (frameworkElement != null)
				{
					this._bringIntoViewContainer = frameworkElement;
					base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(delegate()
					{
						this._bringIntoViewContainer = null;
					}));
					if (!itemsControl.IsGrouping && VirtualizingPanel.GetScrollUnit(itemsControl) == ScrollUnit.Item)
					{
						frameworkElement.BringIntoView();
						return;
					}
					if (!(frameworkElement is GroupItem))
					{
						base.UpdateLayout();
						frameworkElement.BringIntoView();
					}
				}
			}
		}

		// Token: 0x17001BB8 RID: 7096
		// (get) Token: 0x060077BB RID: 30651 RVA: 0x002F52D4 File Offset: 0x002F42D4
		// (set) Token: 0x060077BC RID: 30652 RVA: 0x002F52E6 File Offset: 0x002F42E6
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(VirtualizingStackPanel.OrientationProperty);
			}
			set
			{
				base.SetValue(VirtualizingStackPanel.OrientationProperty, value);
			}
		}

		// Token: 0x17001BB9 RID: 7097
		// (get) Token: 0x060077BD RID: 30653 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal override bool HasLogicalOrientation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001BBA RID: 7098
		// (get) Token: 0x060077BE RID: 30654 RVA: 0x002F52F9 File Offset: 0x002F42F9
		protected internal override Orientation LogicalOrientation
		{
			get
			{
				return this.Orientation;
			}
		}

		// Token: 0x17001BBB RID: 7099
		// (get) Token: 0x060077BF RID: 30655 RVA: 0x002F5301 File Offset: 0x002F4301
		// (set) Token: 0x060077C0 RID: 30656 RVA: 0x002F5318 File Offset: 0x002F4318
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

		// Token: 0x17001BBC RID: 7100
		// (get) Token: 0x060077C1 RID: 30657 RVA: 0x002F5340 File Offset: 0x002F4340
		// (set) Token: 0x060077C2 RID: 30658 RVA: 0x002F5357 File Offset: 0x002F4357
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

		// Token: 0x17001BBD RID: 7101
		// (get) Token: 0x060077C3 RID: 30659 RVA: 0x002F537F File Offset: 0x002F437F
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

		// Token: 0x17001BBE RID: 7102
		// (get) Token: 0x060077C4 RID: 30660 RVA: 0x002F53A3 File Offset: 0x002F43A3
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

		// Token: 0x17001BBF RID: 7103
		// (get) Token: 0x060077C5 RID: 30661 RVA: 0x002F53C7 File Offset: 0x002F43C7
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

		// Token: 0x17001BC0 RID: 7104
		// (get) Token: 0x060077C6 RID: 30662 RVA: 0x002F53EB File Offset: 0x002F43EB
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

		// Token: 0x17001BC1 RID: 7105
		// (get) Token: 0x060077C7 RID: 30663 RVA: 0x002F540F File Offset: 0x002F440F
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

		// Token: 0x17001BC2 RID: 7106
		// (get) Token: 0x060077C8 RID: 30664 RVA: 0x002F5433 File Offset: 0x002F4433
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

		// Token: 0x17001BC3 RID: 7107
		// (get) Token: 0x060077C9 RID: 30665 RVA: 0x002F5457 File Offset: 0x002F4457
		// (set) Token: 0x060077CA RID: 30666 RVA: 0x002F546E File Offset: 0x002F446E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ScrollViewer ScrollOwner
		{
			get
			{
				if (this._scrollData == null)
				{
					return null;
				}
				return this._scrollData._scrollOwner;
			}
			set
			{
				if (this._scrollData == null)
				{
					this.EnsureScrollData();
				}
				if (value != this._scrollData._scrollOwner)
				{
					VirtualizingStackPanel.ResetScrolling(this);
					this._scrollData._scrollOwner = value;
				}
			}
		}

		// Token: 0x060077CB RID: 30667 RVA: 0x002F549E File Offset: 0x002F449E
		public static void AddCleanUpVirtualizedItemHandler(DependencyObject element, CleanUpVirtualizedItemEventHandler handler)
		{
			UIElement.AddHandler(element, VirtualizingStackPanel.CleanUpVirtualizedItemEvent, handler);
		}

		// Token: 0x060077CC RID: 30668 RVA: 0x002F54AC File Offset: 0x002F44AC
		public static void RemoveCleanUpVirtualizedItemHandler(DependencyObject element, CleanUpVirtualizedItemEventHandler handler)
		{
			UIElement.RemoveHandler(element, VirtualizingStackPanel.CleanUpVirtualizedItemEvent, handler);
		}

		// Token: 0x060077CD RID: 30669 RVA: 0x002F54BC File Offset: 0x002F44BC
		protected virtual void OnCleanUpVirtualizedItem(CleanUpVirtualizedItemEventArgs e)
		{
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			if (itemsOwner != null)
			{
				itemsOwner.RaiseEvent(e);
			}
		}

		// Token: 0x17001BC4 RID: 7108
		// (get) Token: 0x060077CE RID: 30670 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected override bool CanHierarchicallyScrollAndVirtualizeCore
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060077CF RID: 30671 RVA: 0x002F54DC File Offset: 0x002F44DC
		protected override Size MeasureOverride(Size constraint)
		{
			List<double> previouslyMeasuredOffsets = null;
			double? lastPageSafeOffset = null;
			double? lastPagePixelSize = null;
			if (VirtualizingStackPanel.IsVSP45Compat)
			{
				return this.MeasureOverrideImpl(constraint, ref lastPageSafeOffset, ref previouslyMeasuredOffsets, ref lastPagePixelSize, false);
			}
			VirtualizingStackPanel.OffsetInformation offsetInformation = VirtualizingStackPanel.OffsetInformationField.GetValue(this);
			if (offsetInformation != null)
			{
				previouslyMeasuredOffsets = offsetInformation.previouslyMeasuredOffsets;
				lastPageSafeOffset = offsetInformation.lastPageSafeOffset;
				lastPagePixelSize = offsetInformation.lastPagePixelSize;
			}
			Size result = this.MeasureOverrideImpl(constraint, ref lastPageSafeOffset, ref previouslyMeasuredOffsets, ref lastPagePixelSize, false);
			if (this.IsScrollActive)
			{
				offsetInformation = new VirtualizingStackPanel.OffsetInformation();
				offsetInformation.previouslyMeasuredOffsets = previouslyMeasuredOffsets;
				offsetInformation.lastPageSafeOffset = lastPageSafeOffset;
				offsetInformation.lastPagePixelSize = lastPagePixelSize;
				VirtualizingStackPanel.OffsetInformationField.SetValue(this, offsetInformation);
			}
			return result;
		}

		// Token: 0x060077D0 RID: 30672 RVA: 0x002F5574 File Offset: 0x002F4574
		private Size MeasureOverrideImpl(Size constraint, ref double? lastPageSafeOffset, ref List<double> previouslyMeasuredOffsets, ref double? lastPagePixelSize, bool remeasure)
		{
			bool flag = this.IsScrolling && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "VirtualizingStackPanel :MeasureOverride");
			}
			Size size = default(Size);
			Size size2 = default(Size);
			Size size3 = default(Size);
			Size size4 = default(Size);
			Size size5 = default(Size);
			Size size6 = default(Size);
			Size size7 = default(Size);
			Size size8 = default(Size);
			bool flag2 = false;
			this.ItemsChangedDuringMeasure = false;
			try
			{
				if (!base.IsItemsHost)
				{
					size = this.MeasureNonItemsHost(constraint);
				}
				else
				{
					bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
					ItemsControl itemsControl = null;
					GroupItem groupItem = null;
					IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = null;
					IContainItemStorage containItemStorage = null;
					object obj = null;
					bool flag3 = this.Orientation == Orientation.Horizontal;
					bool flag4 = false;
					IContainItemStorage containItemStorage2;
					this.GetOwners(true, flag3, out itemsControl, out groupItem, out containItemStorage, out hierarchicalVirtualizationAndScrollInfo, out obj, out containItemStorage2, out flag4);
					Rect empty = Rect.Empty;
					Rect empty2 = Rect.Empty;
					VirtualizationCacheLength virtualizationCacheLength = new VirtualizationCacheLength(0.0);
					VirtualizationCacheLengthUnit cacheUnit = VirtualizationCacheLengthUnit.Pixel;
					long scrollGeneration;
					this.InitializeViewport(obj, containItemStorage2, hierarchicalVirtualizationAndScrollInfo, flag3, constraint, ref empty, ref virtualizationCacheLength, ref cacheUnit, out empty2, out scrollGeneration);
					int minValue = int.MinValue;
					int num = int.MaxValue;
					int minValue2 = int.MinValue;
					UIElement uielement = null;
					double num2 = 0.0;
					double num3 = 0.0;
					bool flag5 = false;
					bool flag6 = false;
					bool flag7 = false;
					base.EnsureGenerator();
					IList realizedChildren = this.RealizedChildren;
					IItemContainerGenerator generator = base.Generator;
					IList itemsInternal = ((ItemContainerGenerator)generator).ItemsInternal;
					int count = itemsInternal.Count;
					IContainItemStorage itemStorageProvider = isVSP45Compat ? containItemStorage : containItemStorage2;
					bool areContainersUniformlySized = this.GetAreContainersUniformlySized(itemStorageProvider, obj);
					bool computedAreContainersUniformlySized = areContainersUniformlySized;
					double num4;
					double num5;
					bool flag8;
					this.GetUniformOrAverageContainerSize(itemStorageProvider, obj, this.IsPixelBased || isVSP45Compat, out num4, out num5, out flag8);
					double computedUniformOrAverageContainerSize = num4;
					double computedUniformOrAverageContainerPixelSize = num5;
					if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
					{
						VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.BeginMeasure, new object[]
						{
							constraint,
							"MC:",
							this.MeasureCaches,
							"reM:",
							remeasure,
							"acs:",
							num4,
							areContainersUniformlySized,
							flag8
						});
					}
					double num6;
					this.ComputeFirstItemInViewportIndexAndOffset(itemsInternal, count, containItemStorage, empty, virtualizationCacheLength, flag3, areContainersUniformlySized, num4, out num2, out num6, out minValue, out flag5);
					double num7;
					this.ComputeFirstItemInViewportIndexAndOffset(itemsInternal, count, containItemStorage, empty2, new VirtualizationCacheLength(0.0), flag3, areContainersUniformlySized, num4, out num3, out num7, out minValue2, out flag7);
					if (this.IsVirtualizing && !remeasure && this.InRecyclingMode)
					{
						int num8 = this._itemsInExtendedViewportCount;
						if (!isVSP45Compat)
						{
							int num9 = (int)Math.Ceiling(Math.Min(1.0, flag3 ? (empty.Width / empty2.Width) : (empty.Height / empty2.Height)) * (double)this._itemsInExtendedViewportCount);
							num8 = Math.Max(num8, minValue + num9 - minValue2);
						}
						this.CleanupContainers(minValue2, num8, itemsControl);
					}
					Size size9 = constraint;
					if (flag3)
					{
						size9.Width = double.PositiveInfinity;
						if (this.IsScrolling && this.CanVerticallyScroll)
						{
							size9.Height = double.PositiveInfinity;
						}
					}
					else
					{
						size9.Height = double.PositiveInfinity;
						if (this.IsScrolling && this.CanHorizontallyScroll)
						{
							size9.Width = double.PositiveInfinity;
						}
					}
					remeasure = false;
					this._actualItemsInExtendedViewportCount = 0;
					this._firstItemInExtendedViewportIndex = 0;
					this._firstItemInExtendedViewportOffset = 0.0;
					this._firstItemInExtendedViewportChildIndex = 0;
					bool flag9 = false;
					int num10 = 0;
					bool flag10 = false;
					bool flag11 = false;
					bool flag12 = false;
					if (count > 0)
					{
						using (((ItemContainerGenerator)generator).GenerateBatches())
						{
							if (!flag5 || !this.IsEndOfCache(flag3, virtualizationCacheLength.CacheBeforeViewport, cacheUnit, size5, size6) || !this.IsEndOfViewport(flag3, empty, size3))
							{
								bool flag13 = false;
								do
								{
									flag13 = false;
									bool flag14 = false;
									bool isAfterFirstItem = false;
									bool isAfterLastItem = false;
									if (this.IsViewportEmpty(flag3, empty) && DoubleUtil.GreaterThan(virtualizationCacheLength.CacheBeforeViewport, 0.0))
									{
										flag14 = true;
									}
									int num11 = minValue;
									GeneratorPosition position = this.IndexToGeneratorPositionForStart(minValue, out num10);
									int num12 = num10;
									this._firstItemInExtendedViewportIndex = minValue;
									this._firstItemInExtendedViewportOffset = num2;
									this._firstItemInExtendedViewportChildIndex = num10;
									using (generator.StartAt(position, GeneratorDirection.Backward, true))
									{
										for (int i = num11; i >= 0; i--)
										{
											object item = itemsInternal[i];
											this.MeasureChild(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref item, ref realizedChildren, ref this._firstItemInExtendedViewportChildIndex, ref flag9, ref flag3, ref size9, ref empty, ref virtualizationCacheLength, ref cacheUnit, ref scrollGeneration, ref flag5, ref num2, ref size, ref size3, ref size5, ref size7, ref size2, ref size4, ref size6, ref size8, ref flag4, i < minValue || flag14, isAfterFirstItem, isAfterLastItem, false, false, ref flag10, ref flag2);
											if (this.ItemsChangedDuringMeasure)
											{
												remeasure = true;
												goto IL_DDB;
											}
											this._actualItemsInExtendedViewportCount++;
											if (!flag5)
											{
												if (isVSP45Compat)
												{
													this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, ref areContainersUniformlySized, ref num4, ref flag11, flag3, false);
												}
												else
												{
													this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, computedUniformOrAverageContainerPixelSize, ref areContainersUniformlySized, ref num4, ref num5, ref flag11, flag3, false);
												}
												this.ComputeFirstItemInViewportIndexAndOffset(itemsInternal, count, containItemStorage, empty, virtualizationCacheLength, flag3, areContainersUniformlySized, num4, out num2, out num6, out minValue, out flag5);
												if (!flag5)
												{
													break;
												}
												if (i != minValue)
												{
													size = default(Size);
													size2 = default(Size);
													this._actualItemsInExtendedViewportCount--;
													flag13 = true;
													break;
												}
												this.MeasureChild(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref item, ref realizedChildren, ref this._firstItemInExtendedViewportChildIndex, ref flag9, ref flag3, ref size9, ref empty, ref virtualizationCacheLength, ref cacheUnit, ref scrollGeneration, ref flag5, ref num2, ref size, ref size3, ref size5, ref size7, ref size2, ref size4, ref size6, ref size8, ref flag4, false, false, false, true, true, ref flag10, ref flag2);
												if (this.ItemsChangedDuringMeasure)
												{
													remeasure = true;
													goto IL_DDB;
												}
											}
											if (!isVSP45Compat && uielement == null && flag5 && i == num11 && 0 <= num12 && num12 < realizedChildren.Count)
											{
												uielement = (realizedChildren[num12] as UIElement);
												if (this.IsScrolling && this._scrollData._firstContainerInViewport != null && !areContainersUniformlySized)
												{
													Size size10;
													this.GetContainerSizeForItem(containItemStorage, item, flag3, areContainersUniformlySized, num4, out size10);
													double num13 = Math.Max(flag3 ? empty.X : empty.Y, 0.0);
													double num14 = flag3 ? size10.Width : size10.Height;
													if (!DoubleUtil.AreClose(num13, 0.0) && !LayoutDoubleUtil.LessThan(num13, num2 + num14))
													{
														double num15 = num14 - num6;
														if (!LayoutDoubleUtil.AreClose(num15, 0.0))
														{
															if (flag3)
															{
																VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
																scrollData._offset.X = scrollData._offset.X + num15;
															}
															else
															{
																VirtualizingStackPanel.ScrollData scrollData2 = this._scrollData;
																scrollData2._offset.Y = scrollData2._offset.Y + num15;
															}
															if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
															{
																VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SizeChangeDuringAnchorScroll, new object[]
																{
																	"fivOffset:",
																	num2,
																	"vpSpan:",
																	num13,
																	"oldCSpan:",
																	num6,
																	"newCSpan:",
																	num14,
																	"delta:",
																	num15,
																	"newVpOff:",
																	this._scrollData._offset
																});
															}
															remeasure = true;
															goto IL_DDB;
														}
													}
												}
											}
											if (this.IsEndOfCache(flag3, virtualizationCacheLength.CacheBeforeViewport, cacheUnit, size5, size6))
											{
												break;
											}
											this._firstItemInExtendedViewportIndex = Math.Max(this._firstItemInExtendedViewportIndex - 1, 0);
											this.IndexToGeneratorPositionForStart(this._firstItemInExtendedViewportIndex, out this._firstItemInExtendedViewportChildIndex);
											this._firstItemInExtendedViewportChildIndex = Math.Max(this._firstItemInExtendedViewportChildIndex, 0);
										}
									}
								}
								while (flag13);
								this.ComputeDistance(itemsInternal, containItemStorage, flag3, areContainersUniformlySized, num4, 0, this._firstItemInExtendedViewportIndex, out this._firstItemInExtendedViewportOffset);
							}
							if (flag5 && (!this.IsEndOfCache(flag3, virtualizationCacheLength.CacheAfterViewport, cacheUnit, size7, size8) || !this.IsEndOfViewport(flag3, empty, size3)))
							{
								bool isBeforeFirstItem = false;
								bool flag15 = false;
								int num16;
								bool flag16;
								if (this.IsViewportEmpty(flag3, empty))
								{
									num16 = 0;
									flag16 = true;
									flag15 = true;
								}
								else
								{
									num16 = minValue + 1;
									flag16 = true;
								}
								GeneratorPosition position = this.IndexToGeneratorPositionForStart(num16, out num10);
								using (generator.StartAt(position, GeneratorDirection.Forward, true))
								{
									int j = num16;
									while (j < count)
									{
										object obj2 = itemsInternal[j];
										this.MeasureChild(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref obj2, ref realizedChildren, ref num10, ref flag9, ref flag3, ref size9, ref empty, ref virtualizationCacheLength, ref cacheUnit, ref scrollGeneration, ref flag5, ref num2, ref size, ref size3, ref size5, ref size7, ref size2, ref size4, ref size6, ref size8, ref flag4, isBeforeFirstItem, j > minValue || flag16, j > num || flag15, false, false, ref flag10, ref flag2);
										if (this.ItemsChangedDuringMeasure)
										{
											remeasure = true;
											goto IL_DDB;
										}
										this._actualItemsInExtendedViewportCount++;
										if (this.IsEndOfViewport(flag3, empty, size3))
										{
											if (!flag6)
											{
												flag6 = true;
												num = j;
											}
											if (this.IsEndOfCache(flag3, virtualizationCacheLength.CacheAfterViewport, cacheUnit, size7, size8))
											{
												break;
											}
										}
										j++;
										num10++;
									}
								}
							}
						}
					}
					if (this.IsVirtualizing && !this.IsPixelBased && (flag2 || hierarchicalVirtualizationAndScrollInfo != null) && (this.MeasureCaches || (DoubleUtil.AreClose(virtualizationCacheLength.CacheBeforeViewport, 0.0) && DoubleUtil.AreClose(virtualizationCacheLength.CacheAfterViewport, 0.0))))
					{
						int num17 = this._firstItemInExtendedViewportChildIndex + this._actualItemsInExtendedViewportCount;
						int count2 = realizedChildren.Count;
						for (int k = num17; k < count2; k++)
						{
							this.MeasureExistingChildBeyondExtendedViewport(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref realizedChildren, ref k, ref flag9, ref flag3, ref size9, ref flag5, ref num2, ref flag4, ref flag2, ref flag10, ref scrollGeneration);
							if (this.ItemsChangedDuringMeasure)
							{
								remeasure = true;
								goto IL_DDB;
							}
						}
					}
					if (this._bringIntoViewContainer != null && !flag10)
					{
						num10 = realizedChildren.IndexOf(this._bringIntoViewContainer);
						if (num10 < 0)
						{
							this._bringIntoViewContainer = null;
						}
						else
						{
							this.MeasureExistingChildBeyondExtendedViewport(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref realizedChildren, ref num10, ref flag9, ref flag3, ref size9, ref flag5, ref num2, ref flag4, ref flag2, ref flag10, ref scrollGeneration);
							if (this.ItemsChangedDuringMeasure)
							{
								remeasure = true;
								goto IL_DDB;
							}
						}
					}
					if (isVSP45Compat)
					{
						this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, ref areContainersUniformlySized, ref num4, ref flag11, flag3, false);
					}
					else
					{
						this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, computedUniformOrAverageContainerPixelSize, ref areContainersUniformlySized, ref num4, ref num5, ref flag11, flag3, false);
					}
					if (this.IsVirtualizing)
					{
						this.ExtendPixelAndLogicalSizes(realizedChildren, itemsInternal, count, containItemStorage, areContainersUniformlySized, num4, num5, ref size, ref size2, flag3, this._firstItemInExtendedViewportIndex, this._firstItemInExtendedViewportChildIndex, minValue, true);
						this.ExtendPixelAndLogicalSizes(realizedChildren, itemsInternal, count, containItemStorage, areContainersUniformlySized, num4, num5, ref size, ref size2, flag3, this._firstItemInExtendedViewportIndex + this._actualItemsInExtendedViewportCount, this._firstItemInExtendedViewportChildIndex + this._actualItemsInExtendedViewportCount, -1, false);
					}
					this._previousStackPixelSizeInViewport = size3;
					this._previousStackLogicalSizeInViewport = size4;
					this._previousStackPixelSizeInCacheBeforeViewport = size5;
					if (!this.IsPixelBased && DoubleUtil.GreaterThan(flag3 ? empty.Left : empty.Top, num2))
					{
						IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement);
						if (virtualizingChild != null)
						{
							Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(virtualizingChild, null, null);
							this._pixelDistanceToViewport += (flag3 ? itemsHostInsetForChild.Left : itemsHostInsetForChild.Top);
							VirtualizingStackPanel virtualizingStackPanel = virtualizingChild.ItemsHost as VirtualizingStackPanel;
							if (virtualizingStackPanel != null)
							{
								this._pixelDistanceToViewport += virtualizingStackPanel._pixelDistanceToViewport;
							}
						}
					}
					if (double.IsInfinity(empty.Width))
					{
						empty.Width = size.Width;
					}
					if (double.IsInfinity(empty.Height))
					{
						empty.Height = size.Height;
					}
					this._extendedViewport = this.ExtendViewport(hierarchicalVirtualizationAndScrollInfo, flag3, empty, virtualizationCacheLength, cacheUnit, size5, size6, size7, size8, size, size2, ref this._itemsInExtendedViewportCount);
					this._viewport = empty;
					if (hierarchicalVirtualizationAndScrollInfo != null && base.IsVisible)
					{
						hierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes = new HierarchicalVirtualizationItemDesiredSizes(size2, size4, size6, size8, size, size3, size5, size7);
						hierarchicalVirtualizationAndScrollInfo.MustDisableVirtualization = flag4;
					}
					if (this.MustDisableVirtualization != flag4)
					{
						this.MustDisableVirtualization = flag4;
						remeasure |= this.IsScrolling;
					}
					double newOffset = 0.0;
					if (!isVSP45Compat)
					{
						if (flag11 || flag12)
						{
							newOffset = this.ComputeEffectiveOffset(ref empty, uielement, minValue, num2, itemsInternal, containItemStorage, hierarchicalVirtualizationAndScrollInfo, flag3, areContainersUniformlySized, num4, scrollGeneration);
							if (uielement != null)
							{
								double num18;
								this.ComputeDistance(itemsInternal, containItemStorage, flag3, areContainersUniformlySized, num4, 0, this._firstItemInExtendedViewportIndex, out num18);
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ReviseArrangeOffset, new object[]
									{
										this._firstItemInExtendedViewportOffset,
										num18
									});
								}
								this._firstItemInExtendedViewportOffset = num18;
							}
							if (!this.IsScrolling)
							{
								DependencyObject dependencyObject = containItemStorage as DependencyObject;
								Panel panel = (dependencyObject != null) ? (VisualTreeHelper.GetParent(dependencyObject) as Panel) : null;
								if (panel != null)
								{
									panel.InvalidateMeasure();
								}
							}
						}
						if (this.HasVirtualizingChildren)
						{
							VirtualizingStackPanel.FirstContainerInformation value = new VirtualizingStackPanel.FirstContainerInformation(ref empty, uielement, minValue, num2, scrollGeneration);
							VirtualizingStackPanel.FirstContainerInformationField.SetValue(this, value);
						}
					}
					if (this.IsScrolling)
					{
						if (isVSP45Compat)
						{
							this.SetAndVerifyScrollingData(flag3, empty, constraint, ref size, ref size2, ref size3, ref size4, ref size5, ref size6, ref remeasure, ref lastPageSafeOffset, ref previouslyMeasuredOffsets);
						}
						else
						{
							this.SetAndVerifyScrollingData(flag3, empty, constraint, uielement, num2, flag11, newOffset, ref size, ref size2, ref size3, ref size4, ref size5, ref size6, ref remeasure, ref lastPageSafeOffset, ref lastPagePixelSize, ref previouslyMeasuredOffsets);
						}
					}
					IL_DDB:
					if (!remeasure)
					{
						if (this.IsVirtualizing)
						{
							if (this.InRecyclingMode)
							{
								this.DisconnectRecycledContainers();
								if (flag9)
								{
									base.InvalidateZState();
								}
							}
							else
							{
								this.EnsureCleanupOperation(false);
							}
						}
						this.HasVirtualizingChildren = flag2;
					}
					if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
					{
						DependencyObject dependencyObject2 = hierarchicalVirtualizationAndScrollInfo as DependencyObject;
						VirtualizingStackPanel.EffectiveOffsetInformation effectiveOffsetInformation = (dependencyObject2 != null) ? VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(dependencyObject2) : null;
						VirtualizingStackPanel.SnapshotData value2 = new VirtualizingStackPanel.SnapshotData
						{
							UniformOrAverageContainerSize = num5,
							UniformOrAverageContainerPixelSize = num5,
							EffectiveOffsets = ((effectiveOffsetInformation != null) ? effectiveOffsetInformation.OffsetList : null)
						};
						VirtualizingStackPanel.SnapshotDataField.SetValue(this, value2);
					}
				}
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "VirtualizingStackPanel :MeasureOverride");
				}
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.EndMeasure, new object[]
				{
					size,
					remeasure
				});
			}
			if (remeasure)
			{
				if (!VirtualizingStackPanel.IsVSP45Compat && this.IsScrolling)
				{
					this.IncrementScrollGeneration();
				}
				return this.MeasureOverrideImpl(constraint, ref lastPageSafeOffset, ref previouslyMeasuredOffsets, ref lastPagePixelSize, remeasure);
			}
			return size;
		}

		// Token: 0x060077D1 RID: 30673 RVA: 0x002F64CC File Offset: 0x002F54CC
		private Size MeasureNonItemsHost(Size constraint)
		{
			return StackPanel.StackMeasureHelper(this, this._scrollData, constraint);
		}

		// Token: 0x060077D2 RID: 30674 RVA: 0x002F64DB File Offset: 0x002F54DB
		private Size ArrangeNonItemsHost(Size arrangeSize)
		{
			return StackPanel.StackArrangeHelper(this, this._scrollData, arrangeSize);
		}

		// Token: 0x060077D3 RID: 30675 RVA: 0x002F64EC File Offset: 0x002F54EC
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.IsScrolling && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "VirtualizingStackPanel :ArrangeOverride");
			}
			try
			{
				if (!base.IsItemsHost)
				{
					this.ArrangeNonItemsHost(arrangeSize);
				}
				else
				{
					ItemsControl itemsControl = null;
					GroupItem groupItem = null;
					IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = null;
					IContainItemStorage containItemStorage = null;
					object item = null;
					bool flag2 = this.Orientation == Orientation.Horizontal;
					bool flag3 = false;
					IContainItemStorage containItemStorage2;
					this.GetOwners(false, flag2, out itemsControl, out groupItem, out containItemStorage, out hierarchicalVirtualizationAndScrollInfo, out item, out containItemStorage2, out flag3);
					if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
					{
						VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.BeginArrange, new object[]
						{
							arrangeSize,
							"ptv:",
							this._pixelDistanceToViewport,
							"ptfc:",
							this._pixelDistanceToFirstContainerInExtendedViewport
						});
					}
					base.EnsureGenerator();
					IList realizedChildren = this.RealizedChildren;
					IItemContainerGenerator generator = base.Generator;
					IList itemsInternal = ((ItemContainerGenerator)generator).ItemsInternal;
					int count = itemsInternal.Count;
					IContainItemStorage itemStorageProvider = VirtualizingStackPanel.IsVSP45Compat ? containItemStorage : containItemStorage2;
					bool areContainersUniformlySized = this.GetAreContainersUniformlySized(itemStorageProvider, item);
					double uniformOrAverageContainerSize;
					double num;
					this.GetUniformOrAverageContainerSize(itemStorageProvider, item, this.IsPixelBased || VirtualizingStackPanel.IsVSP45Compat, out uniformOrAverageContainerSize, out num);
					ScrollViewer scrollOwner = this.ScrollOwner;
					double num2 = 0.0;
					if (scrollOwner != null && scrollOwner.CanContentScroll)
					{
						num2 = this.GetMaxChildArrangeLength(realizedChildren, flag2);
					}
					num2 = Math.Max(flag2 ? arrangeSize.Height : arrangeSize.Width, num2);
					Size childDesiredSize = Size.Empty;
					Rect rect = new Rect(arrangeSize);
					Size size = default(Size);
					int num3 = -1;
					Point point = default(Point);
					bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
					for (int i = this._firstItemInExtendedViewportChildIndex; i < realizedChildren.Count; i++)
					{
						UIElement uielement = (UIElement)realizedChildren[i];
						childDesiredSize = uielement.DesiredSize;
						if (i >= this._firstItemInExtendedViewportChildIndex && i < this._firstItemInExtendedViewportChildIndex + this._actualItemsInExtendedViewportCount)
						{
							if (i == this._firstItemInExtendedViewportChildIndex)
							{
								this.ArrangeFirstItemInExtendedViewport(flag2, uielement, childDesiredSize, num2, ref rect, ref size, ref point, ref num3);
								Size childDesiredSize2 = Size.Empty;
								Rect rect2 = rect;
								Size desiredSize = uielement.DesiredSize;
								int num4 = num3;
								Point point2 = point;
								for (int j = this._firstItemInExtendedViewportChildIndex - 1; j >= 0; j--)
								{
									UIElement uielement2 = (UIElement)realizedChildren[j];
									childDesiredSize2 = uielement2.DesiredSize;
									this.ArrangeItemsBeyondTheExtendedViewport(flag2, uielement2, childDesiredSize2, num2, itemsInternal, generator, containItemStorage, areContainersUniformlySized, uniformOrAverageContainerSize, true, ref rect2, ref desiredSize, ref point2, ref num4);
									if (!isVSP45Compat)
									{
										this.SetItemsHostInsetForChild(j, uielement2, containItemStorage, flag2);
									}
								}
							}
							else
							{
								this.ArrangeOtherItemsInExtendedViewport(flag2, uielement, childDesiredSize, num2, i, ref rect, ref size, ref point, ref num3);
							}
						}
						else
						{
							this.ArrangeItemsBeyondTheExtendedViewport(flag2, uielement, childDesiredSize, num2, itemsInternal, generator, containItemStorage, areContainersUniformlySized, uniformOrAverageContainerSize, false, ref rect, ref size, ref point, ref num3);
						}
						if (!isVSP45Compat)
						{
							this.SetItemsHostInsetForChild(i, uielement, containItemStorage, flag2);
						}
					}
					if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
					{
						DependencyObject dependencyObject = hierarchicalVirtualizationAndScrollInfo as DependencyObject;
						VirtualizingStackPanel.EffectiveOffsetInformation effectiveOffsetInformation = (dependencyObject != null) ? VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(dependencyObject) : null;
						VirtualizingStackPanel.SnapshotData value = new VirtualizingStackPanel.SnapshotData
						{
							UniformOrAverageContainerSize = num,
							UniformOrAverageContainerPixelSize = num,
							EffectiveOffsets = ((effectiveOffsetInformation != null) ? effectiveOffsetInformation.OffsetList : null)
						};
						VirtualizingStackPanel.SnapshotDataField.SetValue(this, value);
						VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.EndArrange, new object[]
						{
							arrangeSize,
							this._firstItemInExtendedViewportIndex,
							this._firstItemInExtendedViewportOffset
						});
					}
				}
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "VirtualizingStackPanel :ArrangeOverride");
				}
			}
			return arrangeSize;
		}

		// Token: 0x060077D4 RID: 30676 RVA: 0x002F68B8 File Offset: 0x002F58B8
		protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ItemsChanged, new object[]
				{
					args.Action,
					"pos:",
					args.OldPosition,
					args.Position,
					"count:",
					args.ItemCount,
					args.ItemUICount,
					base.MeasureInProgress ? "MeasureInProgress" : string.Empty
				});
			}
			if (base.MeasureInProgress)
			{
				this.ItemsChangedDuringMeasure = true;
			}
			base.OnItemsChanged(sender, args);
			bool flag = false;
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Remove:
				this.OnItemsRemove(args);
				flag = true;
				break;
			case NotifyCollectionChangedAction.Replace:
				this.OnItemsReplace(args);
				flag = true;
				break;
			case NotifyCollectionChangedAction.Move:
				this.OnItemsMove(args);
				break;
			case NotifyCollectionChangedAction.Reset:
				flag = true;
				VirtualizingStackPanel.GetItemStorageProvider(this).Clear();
				this.ClearAsyncOperations();
				break;
			}
			if (flag && this.IsScrolling)
			{
				this.ResetMaximumDesiredSize();
			}
		}

		// Token: 0x060077D5 RID: 30677 RVA: 0x002F69CF File Offset: 0x002F59CF
		internal void ResetMaximumDesiredSize()
		{
			if (this.IsScrolling)
			{
				this._scrollData._maxDesiredSize = default(Size);
			}
		}

		// Token: 0x060077D6 RID: 30678 RVA: 0x002F69EC File Offset: 0x002F59EC
		protected override bool ShouldItemsChangeAffectLayoutCore(bool areItemChangesLocal, ItemsChangedEventArgs args)
		{
			bool flag = true;
			if (this.IsVirtualizing)
			{
				if (areItemChangesLocal)
				{
					switch (args.Action)
					{
					case NotifyCollectionChangedAction.Add:
						flag = (base.Generator.IndexFromGeneratorPosition(args.Position) < this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount);
						break;
					case NotifyCollectionChangedAction.Remove:
					{
						int num = base.Generator.IndexFromGeneratorPosition(args.OldPosition);
						flag = (args.ItemUICount > 0 || num < this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount);
						break;
					}
					case NotifyCollectionChangedAction.Replace:
						flag = (args.ItemUICount > 0);
						break;
					case NotifyCollectionChangedAction.Move:
					{
						int num2 = base.Generator.IndexFromGeneratorPosition(args.Position);
						int num3 = base.Generator.IndexFromGeneratorPosition(args.OldPosition);
						flag = (num2 < this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount || num3 < this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount);
						break;
					}
					}
				}
				else
				{
					flag = (base.Generator.IndexFromGeneratorPosition(args.Position) != this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount - 1);
				}
				if (!flag)
				{
					if (this.IsScrolling)
					{
						flag = !this.IsExtendedViewportFull();
						if (!flag)
						{
							this.UpdateExtent(areItemChangesLocal);
						}
					}
					else
					{
						DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(this);
						VirtualizingPanel virtualizingPanel = VisualTreeHelper.GetParent(itemsOwnerInternal) as VirtualizingPanel;
						if (virtualizingPanel != null)
						{
							this.UpdateExtent(areItemChangesLocal);
							IItemContainerGenerator itemContainerGenerator = virtualizingPanel.ItemContainerGenerator;
							int itemIndex = ((ItemContainerGenerator)itemContainerGenerator).IndexFromContainer(itemsOwnerInternal, true);
							ItemsChangedEventArgs args2 = new ItemsChangedEventArgs(NotifyCollectionChangedAction.Reset, itemContainerGenerator.GeneratorPositionFromIndex(itemIndex), 1, 1);
							flag = virtualizingPanel.ShouldItemsChangeAffectLayout(false, args2);
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060077D7 RID: 30679 RVA: 0x002F6B84 File Offset: 0x002F5B84
		private void UpdateExtent(bool areItemChangesLocal)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			ItemsControl itemsControl;
			GroupItem groupItem;
			IContainItemStorage containItemStorage;
			IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo;
			object obj;
			IContainItemStorage containItemStorage2;
			bool flag2;
			this.GetOwners(false, flag, out itemsControl, out groupItem, out containItemStorage, out hierarchicalVirtualizationAndScrollInfo, out obj, out containItemStorage2, out flag2);
			IContainItemStorage itemStorageProvider = isVSP45Compat ? containItemStorage : containItemStorage2;
			bool areContainersUniformlySized = this.GetAreContainersUniformlySized(itemStorageProvider, obj);
			double num;
			double num2;
			this.GetUniformOrAverageContainerSize(itemStorageProvider, obj, isVSP45Compat || this.IsPixelBased, out num, out num2);
			IList realizedChildren = this.RealizedChildren;
			IList itemsInternal = ((ItemContainerGenerator)base.Generator).ItemsInternal;
			int count = itemsInternal.Count;
			if (!areItemChangesLocal)
			{
				double computedUniformOrAverageContainerSize = num;
				double computedUniformOrAverageContainerPixelSize = num2;
				bool computedAreContainersUniformlySized = areContainersUniformlySized;
				bool flag3 = false;
				if (isVSP45Compat)
				{
					this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, ref areContainersUniformlySized, ref num, ref flag3, flag, true);
				}
				else
				{
					this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, computedUniformOrAverageContainerPixelSize, ref areContainersUniformlySized, ref num, ref num2, ref flag3, flag, true);
				}
				if (flag3 && !VirtualizingStackPanel.IsVSP45Compat)
				{
					VirtualizingStackPanel.FirstContainerInformation value = VirtualizingStackPanel.FirstContainerInformationField.GetValue(this);
					if (value != null)
					{
						this.ComputeEffectiveOffset(ref value.Viewport, value.FirstContainer, value.FirstItemIndex, value.FirstItemOffset, itemsInternal, containItemStorage, hierarchicalVirtualizationAndScrollInfo, flag, areContainersUniformlySized, num, value.ScrollGeneration);
					}
				}
			}
			double num3 = 0.0;
			this.ComputeDistance(itemsInternal, containItemStorage, flag, areContainersUniformlySized, num, 0, itemsInternal.Count, out num3);
			if (this.IsScrolling)
			{
				if (flag)
				{
					this._scrollData._extent.Width = num3;
				}
				else
				{
					this._scrollData._extent.Height = num3;
				}
				this.ScrollOwner.InvalidateScrollInfo();
				return;
			}
			if (hierarchicalVirtualizationAndScrollInfo != null)
			{
				HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = hierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes;
				if (this.IsPixelBased)
				{
					Size pixelSize = itemDesiredSizes.PixelSize;
					if (flag)
					{
						pixelSize.Width = num3;
					}
					else
					{
						pixelSize.Height = num3;
					}
					itemDesiredSizes = new HierarchicalVirtualizationItemDesiredSizes(itemDesiredSizes.LogicalSize, itemDesiredSizes.LogicalSizeInViewport, itemDesiredSizes.LogicalSizeBeforeViewport, itemDesiredSizes.LogicalSizeAfterViewport, pixelSize, itemDesiredSizes.PixelSizeInViewport, itemDesiredSizes.PixelSizeBeforeViewport, itemDesiredSizes.PixelSizeAfterViewport);
				}
				else
				{
					Size logicalSize = itemDesiredSizes.LogicalSize;
					if (flag)
					{
						logicalSize.Width = num3;
					}
					else
					{
						logicalSize.Height = num3;
					}
					itemDesiredSizes = new HierarchicalVirtualizationItemDesiredSizes(logicalSize, itemDesiredSizes.LogicalSizeInViewport, itemDesiredSizes.LogicalSizeBeforeViewport, itemDesiredSizes.LogicalSizeAfterViewport, itemDesiredSizes.PixelSize, itemDesiredSizes.PixelSizeInViewport, itemDesiredSizes.PixelSizeBeforeViewport, itemDesiredSizes.PixelSizeAfterViewport);
				}
				hierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes = itemDesiredSizes;
			}
		}

		// Token: 0x060077D8 RID: 30680 RVA: 0x002F6DF0 File Offset: 0x002F5DF0
		private bool IsExtendedViewportFull()
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			if ((flag && DoubleUtil.GreaterThanOrClose(base.DesiredSize.Width, base.PreviousConstraint.Width)) || (!flag && DoubleUtil.GreaterThanOrClose(base.DesiredSize.Height, base.PreviousConstraint.Height)))
			{
				IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider = null;
				Rect viewport = this._viewport;
				Rect extendedViewport = this._extendedViewport;
				Rect rect = Rect.Empty;
				VirtualizationCacheLength cacheLength = VirtualizingPanel.GetCacheLength(this);
				VirtualizationCacheLengthUnit cacheLengthUnit = VirtualizingPanel.GetCacheLengthUnit(this);
				int itemsInExtendedViewportCount = this._itemsInExtendedViewportCount;
				this.NormalizeCacheLength(flag, viewport, ref cacheLength, ref cacheLengthUnit);
				rect = this.ExtendViewport(virtualizationInfoProvider, flag, viewport, cacheLength, cacheLengthUnit, Size.Empty, Size.Empty, Size.Empty, Size.Empty, Size.Empty, Size.Empty, ref itemsInExtendedViewportCount);
				return (flag && DoubleUtil.GreaterThanOrClose(extendedViewport.Width, rect.Width)) || (!flag && DoubleUtil.GreaterThanOrClose(extendedViewport.Height, rect.Height));
			}
			return false;
		}

		// Token: 0x060077D9 RID: 30681 RVA: 0x002F6EFC File Offset: 0x002F5EFC
		protected override void OnClearChildren()
		{
			base.OnClearChildren();
			if (this.IsVirtualizing && base.IsItemsHost)
			{
				ItemsControl itemsControl;
				ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
				this.CleanupContainers(int.MaxValue, int.MaxValue, itemsControl);
			}
			if (this._realizedChildren != null)
			{
				this._realizedChildren.Clear();
			}
			base.InternalChildren.ClearInternal();
		}

		// Token: 0x060077DA RID: 30682 RVA: 0x002F6F58 File Offset: 0x002F5F58
		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(bool)e.NewValue)
			{
				IHierarchicalVirtualizationAndScrollInfo virtualizingProvider = this.GetVirtualizingProvider();
				if (virtualizingProvider != null)
				{
					Helper.ClearVirtualizingElement(virtualizingProvider);
				}
				this.ClearAsyncOperations();
				return;
			}
			base.InvalidateMeasure();
		}

		// Token: 0x060077DB RID: 30683 RVA: 0x002F6F90 File Offset: 0x002F5F90
		internal void ClearAllContainers()
		{
			IItemContainerGenerator generator = base.Generator;
			if (generator != null)
			{
				generator.RemoveAll();
			}
		}

		// Token: 0x060077DC RID: 30684 RVA: 0x002F6FB0 File Offset: 0x002F5FB0
		private IHierarchicalVirtualizationAndScrollInfo GetVirtualizingProvider()
		{
			ItemsControl element = null;
			DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(this, out element);
			if (itemsOwnerInternal is GroupItem)
			{
				return VirtualizingStackPanel.GetVirtualizingProvider(itemsOwnerInternal);
			}
			return VirtualizingStackPanel.GetVirtualizingProvider(element);
		}

		// Token: 0x060077DD RID: 30685 RVA: 0x002F6FE0 File Offset: 0x002F5FE0
		private static IHierarchicalVirtualizationAndScrollInfo GetVirtualizingProvider(DependencyObject element)
		{
			IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = element as IHierarchicalVirtualizationAndScrollInfo;
			if (hierarchicalVirtualizationAndScrollInfo != null)
			{
				VirtualizingPanel virtualizingPanel = VisualTreeHelper.GetParent(element) as VirtualizingPanel;
				if (virtualizingPanel == null || !virtualizingPanel.CanHierarchicallyScrollAndVirtualize)
				{
					hierarchicalVirtualizationAndScrollInfo = null;
				}
			}
			return hierarchicalVirtualizationAndScrollInfo;
		}

		// Token: 0x060077DE RID: 30686 RVA: 0x002F7014 File Offset: 0x002F6014
		private static IHierarchicalVirtualizationAndScrollInfo GetVirtualizingChild(DependencyObject element)
		{
			bool flag = false;
			return VirtualizingStackPanel.GetVirtualizingChild(element, ref flag);
		}

		// Token: 0x060077DF RID: 30687 RVA: 0x002F702C File Offset: 0x002F602C
		private static IHierarchicalVirtualizationAndScrollInfo GetVirtualizingChild(DependencyObject element, ref bool isChildHorizontal)
		{
			IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = element as IHierarchicalVirtualizationAndScrollInfo;
			if (hierarchicalVirtualizationAndScrollInfo != null && hierarchicalVirtualizationAndScrollInfo.ItemsHost != null)
			{
				isChildHorizontal = (hierarchicalVirtualizationAndScrollInfo.ItemsHost.LogicalOrientationPublic == Orientation.Horizontal);
				VirtualizingPanel virtualizingPanel = hierarchicalVirtualizationAndScrollInfo.ItemsHost as VirtualizingPanel;
				if (virtualizingPanel == null || !virtualizingPanel.CanHierarchicallyScrollAndVirtualize)
				{
					hierarchicalVirtualizationAndScrollInfo = null;
				}
			}
			return hierarchicalVirtualizationAndScrollInfo;
		}

		// Token: 0x060077E0 RID: 30688 RVA: 0x002F7078 File Offset: 0x002F6078
		private static IContainItemStorage GetItemStorageProvider(Panel itemsHost)
		{
			ItemsControl itemsControl = null;
			DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(itemsHost, out itemsControl);
			return itemsOwnerInternal as IContainItemStorage;
		}

		// Token: 0x060077E1 RID: 30689 RVA: 0x002F709C File Offset: 0x002F609C
		private void GetOwners(bool shouldSetVirtualizationState, bool isHorizontal, out ItemsControl itemsControl, out GroupItem groupItem, out IContainItemStorage itemStorageProvider, out IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, out object parentItem, out IContainItemStorage parentItemStorageProvider, out bool mustDisableVirtualization)
		{
			groupItem = null;
			parentItem = null;
			parentItemStorageProvider = null;
			bool isScrolling = this.IsScrolling;
			mustDisableVirtualization = (isScrolling && this.MustDisableVirtualization);
			DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
			if (itemsOwnerInternal != itemsControl)
			{
				groupItem = (itemsOwnerInternal as GroupItem);
				parentItem = itemsControl.ItemContainerGenerator.ItemFromContainer(groupItem);
			}
			else if (!isScrolling)
			{
				ItemsControl itemsControl2 = ItemsControl.GetItemsOwnerInternal(VisualTreeHelper.GetParent(itemsControl)) as ItemsControl;
				if (itemsControl2 != null)
				{
					parentItem = itemsControl2.ItemContainerGenerator.ItemFromContainer(itemsControl);
				}
				else
				{
					parentItem = this;
				}
			}
			else
			{
				parentItem = this;
			}
			itemStorageProvider = (itemsOwnerInternal as IContainItemStorage);
			virtualizationInfoProvider = null;
			parentItemStorageProvider = ((VirtualizingStackPanel.IsVSP45Compat || isScrolling || itemsOwnerInternal == null) ? null : (ItemsControl.GetItemsOwnerInternal(VisualTreeHelper.GetParent(itemsOwnerInternal)) as IContainItemStorage));
			if (groupItem != null)
			{
				virtualizationInfoProvider = VirtualizingStackPanel.GetVirtualizingProvider(groupItem);
				mustDisableVirtualization = (virtualizationInfoProvider != null && virtualizationInfoProvider.MustDisableVirtualization);
			}
			else if (!isScrolling)
			{
				virtualizationInfoProvider = VirtualizingStackPanel.GetVirtualizingProvider(itemsControl);
				mustDisableVirtualization = (virtualizationInfoProvider != null && virtualizationInfoProvider.MustDisableVirtualization);
			}
			if (shouldSetVirtualizationState)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled)
				{
					VirtualizingStackPanel.ScrollTracer.ConfigureTracing(this, itemsOwnerInternal, parentItem, itemsControl);
				}
				this.SetVirtualizationState(itemStorageProvider, itemsControl, mustDisableVirtualization);
			}
		}

		// Token: 0x060077E2 RID: 30690 RVA: 0x002F71C4 File Offset: 0x002F61C4
		private void SetVirtualizationState(IContainItemStorage itemStorageProvider, ItemsControl itemsControl, bool mustDisableVirtualization)
		{
			if (itemsControl != null)
			{
				bool isVirtualizing = VirtualizingPanel.GetIsVirtualizing(itemsControl);
				bool isVirtualizingWhenGrouping = VirtualizingPanel.GetIsVirtualizingWhenGrouping(itemsControl);
				VirtualizationMode virtualizationMode = VirtualizingPanel.GetVirtualizationMode(itemsControl);
				bool isGrouping = itemsControl.IsGrouping;
				this.IsVirtualizing = (!mustDisableVirtualization && ((!isGrouping && isVirtualizing) || (isGrouping && isVirtualizing && isVirtualizingWhenGrouping)));
				ScrollUnit scrollUnit = VirtualizingPanel.GetScrollUnit(itemsControl);
				bool isPixelBased = this.IsPixelBased;
				this.IsPixelBased = (mustDisableVirtualization || scrollUnit == ScrollUnit.Pixel);
				if (this.IsScrolling)
				{
					if (!this.HasMeasured || isPixelBased != this.IsPixelBased)
					{
						VirtualizingStackPanel.ClearItemValueStorageRecursive(itemStorageProvider, this);
					}
					VirtualizingPanel.SetCacheLength(this, VirtualizingPanel.GetCacheLength(itemsControl));
					VirtualizingPanel.SetCacheLengthUnit(this, VirtualizingPanel.GetCacheLengthUnit(itemsControl));
				}
				if (this.HasMeasured)
				{
					if ((this.InRecyclingMode ? VirtualizationMode.Recycling : VirtualizationMode.Standard) != virtualizationMode)
					{
						throw new InvalidOperationException(SR.Get("CantSwitchVirtualizationModePostMeasure"));
					}
				}
				else
				{
					this.HasMeasured = true;
				}
				this.InRecyclingMode = (virtualizationMode == VirtualizationMode.Recycling);
			}
		}

		// Token: 0x060077E3 RID: 30691 RVA: 0x002F72A4 File Offset: 0x002F62A4
		private static void ClearItemValueStorageRecursive(IContainItemStorage itemStorageProvider, Panel itemsHost)
		{
			Helper.ClearItemValueStorage((DependencyObject)itemStorageProvider, VirtualizingStackPanel._indicesStoredInItemValueStorage);
			UIElementCollection internalChildren = itemsHost.InternalChildren;
			int count = internalChildren.Count;
			for (int i = 0; i < count; i++)
			{
				IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = internalChildren[i] as IHierarchicalVirtualizationAndScrollInfo;
				if (hierarchicalVirtualizationAndScrollInfo != null)
				{
					Panel itemsHost2 = hierarchicalVirtualizationAndScrollInfo.ItemsHost;
					if (itemsHost2 != null)
					{
						IContainItemStorage itemStorageProvider2 = VirtualizingStackPanel.GetItemStorageProvider(itemsHost2);
						if (itemStorageProvider2 != null)
						{
							VirtualizingStackPanel.ClearItemValueStorageRecursive(itemStorageProvider2, itemsHost2);
						}
					}
				}
			}
		}

		// Token: 0x060077E4 RID: 30692 RVA: 0x002F7310 File Offset: 0x002F6310
		private void InitializeViewport(object parentItem, IContainItemStorage parentItemStorageProvider, IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, bool isHorizontal, Size constraint, ref Rect viewport, ref VirtualizationCacheLength cacheSize, ref VirtualizationCacheLengthUnit cacheUnit, out Rect extendedViewport, out long scrollGeneration)
		{
			Size extent = default(Size);
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			if (this.IsScrolling)
			{
				Size size = constraint;
				double x = this._scrollData._offset.X;
				double y = this._scrollData._offset.Y;
				extent = this._scrollData._extent;
				Size viewport2 = this._scrollData._viewport;
				scrollGeneration = this._scrollData._scrollGeneration;
				if (!this.IsScrollActive || this.IgnoreMaxDesiredSize)
				{
					this._scrollData._maxDesiredSize = default(Size);
				}
				if (this.IsPixelBased)
				{
					viewport = new Rect(x, y, size.Width, size.Height);
					this.CoerceScrollingViewportOffset(ref viewport, extent, isHorizontal);
				}
				else
				{
					viewport = new Rect(x, y, viewport2.Width, viewport2.Height);
					this.CoerceScrollingViewportOffset(ref viewport, extent, isHorizontal);
					viewport.Size = size;
				}
				if (this.IsVirtualizing)
				{
					cacheSize = VirtualizingPanel.GetCacheLength(this);
					cacheUnit = VirtualizingPanel.GetCacheLengthUnit(this);
					if (DoubleUtil.GreaterThan(cacheSize.CacheBeforeViewport, 0.0) || DoubleUtil.GreaterThan(cacheSize.CacheAfterViewport, 0.0))
					{
						if (!this.MeasureCaches)
						{
							this.WasLastMeasurePassAnchored = (this._scrollData._firstContainerInViewport != null || this._scrollData._bringIntoViewLeafContainer != null);
							if (VirtualizingStackPanel.MeasureCachesOperationField.GetValue(this) == null)
							{
								Action measureCachesAction = null;
								int retryCount = 3;
								measureCachesAction = delegate()
								{
									int num2 = 0;
									int retryCount = retryCount;
									retryCount--;
									bool flag = num2 < retryCount && (this.MeasureDirty || this.ArrangeDirty);
									try
									{
										if (isVSP45Compat || !flag)
										{
											VirtualizingStackPanel.MeasureCachesOperationField.ClearValue(this);
											this.MeasureCaches = true;
											if (this.WasLastMeasurePassAnchored)
											{
												this.SetAnchorInformation(isHorizontal);
											}
											this.InvalidateMeasure();
											this.UpdateLayout();
										}
									}
									finally
									{
										flag = (flag || (0 < retryCount && (this.MeasureDirty || this.ArrangeDirty)));
										if (!isVSP45Compat && flag)
										{
											VirtualizingStackPanel.MeasureCachesOperationField.SetValue(this, this.Dispatcher.BeginInvoke(DispatcherPriority.Background, measureCachesAction));
										}
										this.MeasureCaches = false;
										if (VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.GetValue(this) == null && (isVSP45Compat || !flag))
										{
											if (isVSP45Compat)
											{
												this.IsScrollActive = false;
											}
											else if (this.IsScrollActive)
											{
												DispatcherOperation dispatcherOperation = VirtualizingStackPanel.ClearIsScrollActiveOperationField.GetValue(this);
												if (dispatcherOperation != null)
												{
													dispatcherOperation.Abort();
												}
												dispatcherOperation = this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.ClearIsScrollActive));
												VirtualizingStackPanel.ClearIsScrollActiveOperationField.SetValue(this, dispatcherOperation);
											}
										}
									}
								};
								DispatcherOperation value = base.Dispatcher.BeginInvoke(DispatcherPriority.Background, measureCachesAction);
								VirtualizingStackPanel.MeasureCachesOperationField.SetValue(this, value);
							}
						}
					}
					else if (this.IsScrollActive && VirtualizingStackPanel.ClearIsScrollActiveOperationField.GetValue(this) == null)
					{
						DispatcherOperation value2 = base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.ClearIsScrollActive));
						VirtualizingStackPanel.ClearIsScrollActiveOperationField.SetValue(this, value2);
					}
					this.NormalizeCacheLength(isHorizontal, viewport, ref cacheSize, ref cacheUnit);
				}
				else
				{
					cacheSize = new VirtualizationCacheLength(double.PositiveInfinity, this.IsViewportEmpty(isHorizontal, viewport) ? 0.0 : double.PositiveInfinity);
					cacheUnit = VirtualizationCacheLengthUnit.Pixel;
					this.ClearAsyncOperations();
				}
			}
			else if (virtualizationInfoProvider != null)
			{
				HierarchicalVirtualizationConstraints constraints = virtualizationInfoProvider.Constraints;
				viewport = constraints.Viewport;
				cacheSize = constraints.CacheLength;
				cacheUnit = constraints.CacheLengthUnit;
				scrollGeneration = constraints.ScrollGeneration;
				this.MeasureCaches = virtualizationInfoProvider.InBackgroundLayout;
				if (isVSP45Compat)
				{
					this.AdjustNonScrollingViewportForHeader(virtualizationInfoProvider, ref viewport, ref cacheSize, ref cacheUnit);
				}
				else
				{
					this.AdjustNonScrollingViewportForInset(isHorizontal, parentItem, parentItemStorageProvider, virtualizationInfoProvider, ref viewport, ref cacheSize, ref cacheUnit);
					DependencyObject instance = virtualizationInfoProvider as DependencyObject;
					VirtualizingStackPanel.EffectiveOffsetInformation value3 = VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(instance);
					if (value3 != null)
					{
						List<double> offsetList = value3.OffsetList;
						int num = -1;
						if (value3.ScrollGeneration >= scrollGeneration)
						{
							double value4 = isHorizontal ? viewport.X : viewport.Y;
							int i = 0;
							int count = offsetList.Count;
							while (i < count)
							{
								if (LayoutDoubleUtil.AreClose(value4, offsetList[i]))
								{
									num = i;
									break;
								}
								i++;
							}
						}
						if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
						{
							object[] array = new object[offsetList.Count + 7];
							array[0] = "gen";
							array[1] = value3.ScrollGeneration;
							array[2] = constraints.ScrollGeneration;
							array[3] = viewport.Location;
							array[4] = "at";
							array[5] = num;
							array[6] = "in";
							for (int j = 0; j < offsetList.Count; j++)
							{
								array[j + 7] = offsetList[j];
							}
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.UseSubstOffset, array);
						}
						if (num >= 0)
						{
							if (isHorizontal)
							{
								viewport.X = offsetList[offsetList.Count - 1];
							}
							else
							{
								viewport.Y = offsetList[offsetList.Count - 1];
							}
							offsetList.RemoveRange(0, num);
						}
						if (num < 0 || offsetList.Count <= 1)
						{
							VirtualizingStackPanel.EffectiveOffsetInformationField.ClearValue(instance);
						}
					}
				}
			}
			else
			{
				scrollGeneration = 0L;
				viewport = new Rect(0.0, 0.0, constraint.Width, constraint.Height);
				if (isHorizontal)
				{
					viewport.Width = double.PositiveInfinity;
				}
				else
				{
					viewport.Height = double.PositiveInfinity;
				}
			}
			extendedViewport = this._extendedViewport;
			if (isHorizontal)
			{
				extendedViewport.X += viewport.X - this._viewport.X;
			}
			else
			{
				extendedViewport.Y += viewport.Y - this._viewport.Y;
			}
			if (this.IsVirtualizing)
			{
				if (this.MeasureCaches)
				{
					this.IsMeasureCachesPending = false;
					return;
				}
				if (DoubleUtil.GreaterThan(cacheSize.CacheBeforeViewport, 0.0) || DoubleUtil.GreaterThan(cacheSize.CacheAfterViewport, 0.0))
				{
					this.IsMeasureCachesPending = true;
				}
			}
		}

		// Token: 0x060077E5 RID: 30693 RVA: 0x002F78B8 File Offset: 0x002F68B8
		private void ClearMeasureCachesState()
		{
			DispatcherOperation value = VirtualizingStackPanel.MeasureCachesOperationField.GetValue(this);
			if (value != null)
			{
				value.Abort();
				VirtualizingStackPanel.MeasureCachesOperationField.ClearValue(this);
			}
			this.IsMeasureCachesPending = false;
			if (this._cleanupOperation != null && this._cleanupOperation.Abort())
			{
				this._cleanupOperation = null;
			}
			if (this._cleanupDelay != null)
			{
				this._cleanupDelay.Stop();
				this._cleanupDelay = null;
			}
		}

		// Token: 0x060077E6 RID: 30694 RVA: 0x002F7924 File Offset: 0x002F6924
		private void ClearIsScrollActive()
		{
			VirtualizingStackPanel.ClearIsScrollActiveOperationField.ClearValue(this);
			VirtualizingStackPanel.OffsetInformationField.ClearValue(this);
			this._scrollData._bringIntoViewLeafContainer = null;
			this.IsScrollActive = false;
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				this._scrollData._offset = this._scrollData._computedOffset;
			}
		}

		// Token: 0x060077E7 RID: 30695 RVA: 0x002F7978 File Offset: 0x002F6978
		private void NormalizeCacheLength(bool isHorizontal, Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheUnit)
		{
			if (cacheUnit == VirtualizationCacheLengthUnit.Page)
			{
				double num = isHorizontal ? viewport.Width : viewport.Height;
				if (double.IsPositiveInfinity(num))
				{
					cacheLength = new VirtualizationCacheLength(0.0, 0.0);
				}
				else
				{
					cacheLength = new VirtualizationCacheLength(cacheLength.CacheBeforeViewport * num, cacheLength.CacheAfterViewport * num);
				}
				cacheUnit = VirtualizationCacheLengthUnit.Pixel;
			}
			if (this.IsViewportEmpty(isHorizontal, viewport))
			{
				cacheLength = new VirtualizationCacheLength(0.0, 0.0);
			}
		}

		// Token: 0x060077E8 RID: 30696 RVA: 0x002F7A0C File Offset: 0x002F6A0C
		private Rect ExtendViewport(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, bool isHorizontal, Rect viewport, VirtualizationCacheLength cacheLength, VirtualizationCacheLengthUnit cacheUnit, Size stackPixelSizeInCacheBeforeViewport, Size stackLogicalSizeInCacheBeforeViewport, Size stackPixelSizeInCacheAfterViewport, Size stackLogicalSizeInCacheAfterViewport, Size stackPixelSize, Size stackLogicalSize, ref int itemsInExtendedViewportCount)
		{
			Rect rect = viewport;
			if (isHorizontal)
			{
				double num = (DoubleUtil.GreaterThan(this._previousStackPixelSizeInViewport.Width, 0.0) && DoubleUtil.GreaterThan(this._previousStackLogicalSizeInViewport.Width, 0.0)) ? (this._previousStackPixelSizeInViewport.Width / this._previousStackLogicalSizeInViewport.Width) : 16.0;
				double num2 = stackPixelSize.Width;
				double num3 = stackLogicalSize.Width;
				double num4;
				double num5;
				double num6;
				if (this.MeasureCaches)
				{
					num4 = stackPixelSizeInCacheBeforeViewport.Width;
					num5 = stackPixelSizeInCacheAfterViewport.Width;
					num6 = stackLogicalSizeInCacheBeforeViewport.Width;
					double width = stackLogicalSizeInCacheAfterViewport.Width;
				}
				else
				{
					num4 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? (cacheLength.CacheBeforeViewport * num) : cacheLength.CacheBeforeViewport);
					num5 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? (cacheLength.CacheAfterViewport * num) : cacheLength.CacheAfterViewport);
					num6 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? cacheLength.CacheBeforeViewport : (cacheLength.CacheBeforeViewport / num));
					if (cacheUnit != VirtualizationCacheLengthUnit.Item)
					{
						double num7 = cacheLength.CacheAfterViewport / num;
					}
					else
					{
						double cacheAfterViewport = cacheLength.CacheAfterViewport;
					}
					if (this.IsPixelBased)
					{
						num4 = Math.Max(num4, Math.Abs(this._viewport.X - this._extendedViewport.X));
					}
					else
					{
						num6 = Math.Max(num6, Math.Abs(this._viewport.X - this._extendedViewport.X));
					}
				}
				if (this.IsPixelBased)
				{
					if (!this.IsScrolling && virtualizationInfoProvider != null && this.IsViewportEmpty(isHorizontal, rect) && DoubleUtil.GreaterThan(num4, 0.0))
					{
						rect.X = num2 - num4;
					}
					else
					{
						rect.X -= num4;
					}
					rect.Width += num4 + num5;
					if (this.IsScrolling)
					{
						if (DoubleUtil.LessThan(rect.X, 0.0))
						{
							rect.Width = Math.Max(rect.Width + rect.X, 0.0);
							rect.X = 0.0;
						}
						if (DoubleUtil.GreaterThan(rect.X + rect.Width, this._scrollData._extent.Width))
						{
							rect.Width = this._scrollData._extent.Width - rect.X;
						}
					}
				}
				else
				{
					if (!this.IsScrolling && virtualizationInfoProvider != null && this.IsViewportEmpty(isHorizontal, rect) && DoubleUtil.GreaterThan(num4, 0.0))
					{
						rect.X = num3 - num6;
					}
					else
					{
						rect.X -= num6;
					}
					rect.Width += num4 + num5;
					if (this.IsScrolling)
					{
						if (DoubleUtil.LessThan(rect.X, 0.0))
						{
							rect.Width = Math.Max(rect.Width / num + rect.X, 0.0) * num;
							rect.X = 0.0;
						}
						if (DoubleUtil.GreaterThan(rect.X + rect.Width / num, this._scrollData._extent.Width))
						{
							rect.Width = (this._scrollData._extent.Width - rect.X) * num;
						}
					}
				}
			}
			else
			{
				double num8 = (DoubleUtil.GreaterThan(this._previousStackPixelSizeInViewport.Height, 0.0) && DoubleUtil.GreaterThan(this._previousStackLogicalSizeInViewport.Height, 0.0)) ? (this._previousStackPixelSizeInViewport.Height / this._previousStackLogicalSizeInViewport.Height) : 16.0;
				double num2 = stackPixelSize.Height;
				double num3 = stackLogicalSize.Height;
				double num4;
				double num5;
				double num6;
				if (this.MeasureCaches)
				{
					num4 = stackPixelSizeInCacheBeforeViewport.Height;
					num5 = stackPixelSizeInCacheAfterViewport.Height;
					num6 = stackLogicalSizeInCacheBeforeViewport.Height;
					double height = stackLogicalSizeInCacheAfterViewport.Height;
				}
				else
				{
					num4 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? (cacheLength.CacheBeforeViewport * num8) : cacheLength.CacheBeforeViewport);
					num5 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? (cacheLength.CacheAfterViewport * num8) : cacheLength.CacheAfterViewport);
					num6 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? cacheLength.CacheBeforeViewport : (cacheLength.CacheBeforeViewport / num8));
					if (cacheUnit != VirtualizationCacheLengthUnit.Item)
					{
						double num9 = cacheLength.CacheAfterViewport / num8;
					}
					else
					{
						double cacheAfterViewport2 = cacheLength.CacheAfterViewport;
					}
					if (this.IsPixelBased)
					{
						num4 = Math.Max(num4, Math.Abs(this._viewport.Y - this._extendedViewport.Y));
					}
					else
					{
						num6 = Math.Max(num6, Math.Abs(this._viewport.Y - this._extendedViewport.Y));
					}
				}
				if (this.IsPixelBased)
				{
					if (!this.IsScrolling && virtualizationInfoProvider != null && this.IsViewportEmpty(isHorizontal, rect) && DoubleUtil.GreaterThan(num4, 0.0))
					{
						rect.Y = num2 - num4;
					}
					else
					{
						rect.Y -= num4;
					}
					rect.Height += num4 + num5;
					if (this.IsScrolling)
					{
						if (DoubleUtil.LessThan(rect.Y, 0.0))
						{
							rect.Height = Math.Max(rect.Height + rect.Y, 0.0);
							rect.Y = 0.0;
						}
						if (DoubleUtil.GreaterThan(rect.Y + rect.Height, this._scrollData._extent.Height))
						{
							rect.Height = this._scrollData._extent.Height - rect.Y;
						}
					}
				}
				else
				{
					if (!this.IsScrolling && virtualizationInfoProvider != null && this.IsViewportEmpty(isHorizontal, rect) && DoubleUtil.GreaterThan(num4, 0.0))
					{
						rect.Y = num3 - num6;
					}
					else
					{
						rect.Y -= num6;
					}
					rect.Height += num4 + num5;
					if (this.IsScrolling)
					{
						if (DoubleUtil.LessThan(rect.Y, 0.0))
						{
							rect.Height = Math.Max(rect.Height / num8 + rect.Y, 0.0) * num8;
							rect.Y = 0.0;
						}
						if (DoubleUtil.GreaterThan(rect.Y + rect.Height / num8, this._scrollData._extent.Height))
						{
							rect.Height = (this._scrollData._extent.Height - rect.Y) * num8;
						}
					}
				}
			}
			if (this.MeasureCaches)
			{
				itemsInExtendedViewportCount = this._actualItemsInExtendedViewportCount;
			}
			else
			{
				int val = (int)Math.Ceiling(Math.Max(1.0, isHorizontal ? (rect.Width / viewport.Width) : (rect.Height / viewport.Height)) * (double)this._actualItemsInExtendedViewportCount);
				itemsInExtendedViewportCount = Math.Max(val, itemsInExtendedViewportCount);
			}
			return rect;
		}

		// Token: 0x060077E9 RID: 30697 RVA: 0x002F8128 File Offset: 0x002F7128
		private void CoerceScrollingViewportOffset(ref Rect viewport, Size extent, bool isHorizontal)
		{
			if (!this._scrollData.IsEmpty)
			{
				viewport.X = ScrollContentPresenter.CoerceOffset(viewport.X, extent.Width, viewport.Width);
				if (!this.IsPixelBased && isHorizontal && DoubleUtil.IsZero(viewport.Width) && DoubleUtil.AreClose(viewport.X, extent.Width))
				{
					viewport.X = ScrollContentPresenter.CoerceOffset(viewport.X - 1.0, extent.Width, viewport.Width);
				}
			}
			if (!this._scrollData.IsEmpty)
			{
				viewport.Y = ScrollContentPresenter.CoerceOffset(viewport.Y, extent.Height, viewport.Height);
				if (!this.IsPixelBased && !isHorizontal && DoubleUtil.IsZero(viewport.Height) && DoubleUtil.AreClose(viewport.Y, extent.Height))
				{
					viewport.Y = ScrollContentPresenter.CoerceOffset(viewport.Y - 1.0, extent.Height, viewport.Height);
				}
			}
		}

		// Token: 0x060077EA RID: 30698 RVA: 0x002F8238 File Offset: 0x002F7238
		private void AdjustNonScrollingViewportForHeader(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, ref Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheLengthUnit)
		{
			bool forHeader = true;
			this.AdjustNonScrollingViewport(virtualizationInfoProvider, ref viewport, ref cacheLength, ref cacheLengthUnit, forHeader);
		}

		// Token: 0x060077EB RID: 30699 RVA: 0x002F8254 File Offset: 0x002F7254
		private void AdjustNonScrollingViewportForItems(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, ref Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheLengthUnit)
		{
			bool forHeader = false;
			this.AdjustNonScrollingViewport(virtualizationInfoProvider, ref viewport, ref cacheLength, ref cacheLengthUnit, forHeader);
		}

		// Token: 0x060077EC RID: 30700 RVA: 0x002F8270 File Offset: 0x002F7270
		private void AdjustNonScrollingViewport(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, ref Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheUnit, bool forHeader)
		{
			Rect rect = viewport;
			double num = cacheLength.CacheBeforeViewport;
			double num2 = cacheLength.CacheAfterViewport;
			HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = virtualizationInfoProvider.HeaderDesiredSizes;
			HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizationInfoProvider.ItemDesiredSizes;
			Size size = forHeader ? headerDesiredSizes.PixelSize : itemDesiredSizes.PixelSize;
			Size size2 = forHeader ? headerDesiredSizes.LogicalSize : itemDesiredSizes.LogicalSize;
			RelativeHeaderPosition relativeHeaderPosition = RelativeHeaderPosition.Top;
			if ((forHeader && relativeHeaderPosition == RelativeHeaderPosition.Left) || (!forHeader && relativeHeaderPosition == RelativeHeaderPosition.Right))
			{
				viewport.X -= (this.IsPixelBased ? size.Width : size2.Width);
				if (DoubleUtil.GreaterThan(rect.X, 0.0))
				{
					if (this.IsPixelBased && DoubleUtil.GreaterThan(size.Width, rect.X))
					{
						double num3 = size.Width - rect.X;
						double num4 = size.Width - num3;
						viewport.Width = Math.Max(viewport.Width - num3, 0.0);
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num = Math.Max(num - num4, 0.0);
						}
						else
						{
							num = Math.Max(num - Math.Floor(size2.Width * num4 / size.Width), 0.0);
						}
					}
				}
				else if (DoubleUtil.GreaterThan(rect.Width, 0.0))
				{
					if (DoubleUtil.GreaterThanOrClose(rect.Width, size.Width))
					{
						viewport.Width = Math.Max(0.0, rect.Width - size.Width);
					}
					else
					{
						double num5 = rect.Width;
						double num6 = size.Width - num5;
						viewport.Width = 0.0;
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num2 = Math.Max(num2 - num6, 0.0);
						}
						else
						{
							num2 = Math.Max(num2 - Math.Floor(size2.Width * num6 / size.Width), 0.0);
						}
					}
				}
				else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					num2 = Math.Max(num2 - size.Width, 0.0);
				}
				else
				{
					num2 = Math.Max(num2 - size2.Width, 0.0);
				}
			}
			else if ((forHeader && relativeHeaderPosition == RelativeHeaderPosition.Top) || (!forHeader && relativeHeaderPosition == RelativeHeaderPosition.Bottom))
			{
				viewport.Y -= (this.IsPixelBased ? size.Height : size2.Height);
				if (DoubleUtil.GreaterThan(rect.Y, 0.0))
				{
					if (this.IsPixelBased && DoubleUtil.GreaterThan(size.Height, rect.Y))
					{
						double num3 = size.Height - rect.Y;
						double num4 = size.Height - num3;
						viewport.Height = Math.Max(viewport.Height - num3, 0.0);
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num = Math.Max(num - num4, 0.0);
						}
						else
						{
							num = Math.Max(num - Math.Floor(size2.Height * num4 / size.Height), 0.0);
						}
					}
				}
				else if (DoubleUtil.GreaterThan(rect.Height, 0.0))
				{
					if (DoubleUtil.GreaterThanOrClose(rect.Height, size.Height))
					{
						viewport.Height = Math.Max(0.0, rect.Height - size.Height);
					}
					else
					{
						double num5 = rect.Height;
						double num6 = size.Height - num5;
						viewport.Height = 0.0;
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num2 = Math.Max(num2 - num6, 0.0);
						}
						else
						{
							num2 = Math.Max(num2 - Math.Floor(size2.Height * num6 / size.Height), 0.0);
						}
					}
				}
				else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					num2 = Math.Max(num2 - size.Height, 0.0);
				}
				else
				{
					num2 = Math.Max(num2 - size2.Height, 0.0);
				}
			}
			cacheLength = new VirtualizationCacheLength(num, num2);
		}

		// Token: 0x060077ED RID: 30701 RVA: 0x002F8708 File Offset: 0x002F7708
		private void AdjustNonScrollingViewportForInset(bool isHorizontal, object parentItem, IContainItemStorage parentItemStorageProvider, IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, ref Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheUnit)
		{
			Rect rect = viewport;
			FrameworkElement container = virtualizationInfoProvider as FrameworkElement;
			Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(virtualizationInfoProvider, parentItemStorageProvider, parentItem);
			bool flag = this.IsHeaderBeforeItems(isHorizontal, container, ref itemsHostInsetForChild);
			double num = cacheLength.CacheBeforeViewport;
			double num2 = cacheLength.CacheAfterViewport;
			if (isHorizontal)
			{
				viewport.X -= (this.IsPixelBased ? itemsHostInsetForChild.Left : ((double)(flag ? 1 : 0)));
			}
			else
			{
				viewport.Y -= (this.IsPixelBased ? itemsHostInsetForChild.Top : ((double)(flag ? 1 : 0)));
			}
			if (isHorizontal)
			{
				if (DoubleUtil.GreaterThan(rect.X, 0.0))
				{
					if (DoubleUtil.GreaterThan(viewport.Width, 0.0))
					{
						if (this.IsPixelBased && DoubleUtil.GreaterThan(0.0, viewport.X))
						{
							if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
							{
								num = Math.Max(0.0, num - rect.X);
							}
							viewport.Width = Math.Max(0.0, viewport.Width + viewport.X);
						}
					}
					else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
					{
						num = Math.Max(0.0, num - itemsHostInsetForChild.Right);
					}
					else if (!flag)
					{
						num = Math.Max(0.0, num - 1.0);
					}
				}
				else if (DoubleUtil.GreaterThan(viewport.Width, 0.0))
				{
					if (DoubleUtil.GreaterThanOrClose(viewport.Width, itemsHostInsetForChild.Left))
					{
						viewport.Width = Math.Max(0.0, viewport.Width - itemsHostInsetForChild.Left);
					}
					else
					{
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num2 = Math.Max(0.0, num2 - (itemsHostInsetForChild.Left - viewport.Width));
						}
						viewport.Width = 0.0;
					}
				}
				else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					num2 = Math.Max(0.0, num2 - itemsHostInsetForChild.Left);
				}
				else if (flag)
				{
					num2 = Math.Max(0.0, num2 - 1.0);
				}
			}
			else if (DoubleUtil.GreaterThan(rect.Y, 0.0))
			{
				if (DoubleUtil.GreaterThan(viewport.Height, 0.0))
				{
					if (this.IsPixelBased && DoubleUtil.GreaterThan(0.0, viewport.Y))
					{
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num = Math.Max(0.0, num - rect.Y);
						}
						viewport.Height = Math.Max(0.0, viewport.Height + viewport.Y);
					}
				}
				else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					num = Math.Max(0.0, num - itemsHostInsetForChild.Bottom);
				}
				else if (!flag)
				{
					num = Math.Max(0.0, num - 1.0);
				}
			}
			else if (DoubleUtil.GreaterThan(viewport.Height, 0.0))
			{
				if (DoubleUtil.GreaterThanOrClose(viewport.Height, itemsHostInsetForChild.Top))
				{
					viewport.Height = Math.Max(0.0, viewport.Height - itemsHostInsetForChild.Top);
				}
				else
				{
					if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
					{
						num2 = Math.Max(0.0, num2 - (itemsHostInsetForChild.Top - viewport.Height));
					}
					viewport.Height = 0.0;
				}
			}
			else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
			{
				num2 = Math.Max(0.0, num2 - itemsHostInsetForChild.Top);
			}
			else if (flag)
			{
				num2 = Math.Max(0.0, num2 - 1.0);
			}
			cacheLength = new VirtualizationCacheLength(num, num2);
		}

		// Token: 0x060077EE RID: 30702 RVA: 0x002F8B30 File Offset: 0x002F7B30
		private void ComputeFirstItemInViewportIndexAndOffset(IList items, int itemCount, IContainItemStorage itemStorageProvider, Rect viewport, VirtualizationCacheLength cacheSize, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, out double firstItemInViewportOffset, out double firstItemInViewportContainerSpan, out int firstItemInViewportIndex, out bool foundFirstItemInViewport)
		{
			firstItemInViewportOffset = 0.0;
			firstItemInViewportContainerSpan = 0.0;
			firstItemInViewportIndex = 0;
			foundFirstItemInViewport = false;
			if (this.IsViewportEmpty(isHorizontal, viewport))
			{
				if (DoubleUtil.GreaterThan(cacheSize.CacheBeforeViewport, 0.0))
				{
					firstItemInViewportIndex = itemCount - 1;
					this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, itemCount - 1, out firstItemInViewportOffset);
					foundFirstItemInViewport = true;
				}
				else
				{
					firstItemInViewportIndex = 0;
					firstItemInViewportOffset = 0.0;
					foundFirstItemInViewport = DoubleUtil.GreaterThan(cacheSize.CacheAfterViewport, 0.0);
				}
			}
			else
			{
				double num = Math.Max(isHorizontal ? viewport.X : viewport.Y, 0.0);
				if (areContainersUniformlySized)
				{
					if (DoubleUtil.GreaterThan(uniformOrAverageContainerSize, 0.0))
					{
						firstItemInViewportIndex = (int)Math.Floor(num / uniformOrAverageContainerSize);
						firstItemInViewportOffset = (double)firstItemInViewportIndex * uniformOrAverageContainerSize;
					}
					firstItemInViewportContainerSpan = uniformOrAverageContainerSize;
					foundFirstItemInViewport = (firstItemInViewportIndex < itemCount);
					if (!foundFirstItemInViewport)
					{
						firstItemInViewportOffset = 0.0;
						firstItemInViewportIndex = 0;
					}
				}
				else if (DoubleUtil.AreClose(num, 0.0))
				{
					foundFirstItemInViewport = true;
					firstItemInViewportOffset = 0.0;
					firstItemInViewportIndex = 0;
				}
				else
				{
					double num2 = 0.0;
					bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
					for (int i = 0; i < itemCount; i++)
					{
						object item = items[i];
						Size size;
						this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, out size);
						double num3 = isHorizontal ? size.Width : size.Height;
						num2 += num3;
						if (isVSP45Compat ? DoubleUtil.GreaterThan(num2, num) : LayoutDoubleUtil.LessThan(num, num2))
						{
							firstItemInViewportIndex = i;
							firstItemInViewportOffset = num2 - num3;
							firstItemInViewportContainerSpan = num3;
							break;
						}
					}
					foundFirstItemInViewport = (isVSP45Compat ? DoubleUtil.GreaterThan(num2, num) : LayoutDoubleUtil.LessThan(num, num2));
					if (!foundFirstItemInViewport)
					{
						firstItemInViewportOffset = 0.0;
						firstItemInViewportIndex = 0;
					}
				}
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.CFIVIO, new object[]
				{
					viewport,
					foundFirstItemInViewport,
					firstItemInViewportIndex,
					firstItemInViewportOffset
				});
			}
		}

		// Token: 0x060077EF RID: 30703 RVA: 0x002F8D74 File Offset: 0x002F7D74
		private double ComputeEffectiveOffset(ref Rect viewport, DependencyObject firstContainer, int itemIndex, double firstItemOffset, IList items, IContainItemStorage itemStorageProvider, IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, long scrollGeneration)
		{
			if (firstContainer == null || this.IsViewportEmpty(isHorizontal, viewport))
			{
				return -1.0;
			}
			double num = isHorizontal ? viewport.X : viewport.Y;
			double num2;
			this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, itemIndex, out num2);
			num2 += num - firstItemOffset;
			VirtualizingStackPanel.EffectiveOffsetInformation effectiveOffsetInformation = VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(firstContainer);
			List<double> list = (effectiveOffsetInformation != null) ? effectiveOffsetInformation.OffsetList : null;
			if (list != null)
			{
				int count = list.Count;
				num2 += list[count - 1] - list[0];
			}
			DependencyObject dependencyObject = virtualizationInfoProvider as DependencyObject;
			if (dependencyObject != null && !LayoutDoubleUtil.AreClose(num, num2))
			{
				effectiveOffsetInformation = VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(dependencyObject);
				if (effectiveOffsetInformation == null || effectiveOffsetInformation.ScrollGeneration != scrollGeneration)
				{
					effectiveOffsetInformation = new VirtualizingStackPanel.EffectiveOffsetInformation(scrollGeneration);
					effectiveOffsetInformation.OffsetList.Add(num);
				}
				effectiveOffsetInformation.OffsetList.Add(num2);
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					List<double> offsetList = effectiveOffsetInformation.OffsetList;
					object[] array = new object[offsetList.Count + 2];
					array[0] = scrollGeneration;
					array[1] = ":";
					for (int i = 0; i < offsetList.Count; i++)
					{
						array[i + 2] = offsetList[i];
					}
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.StoreSubstOffset, array);
				}
				VirtualizingStackPanel.EffectiveOffsetInformationField.SetValue(dependencyObject, effectiveOffsetInformation);
			}
			return num2;
		}

		// Token: 0x060077F0 RID: 30704 RVA: 0x002F8ED9 File Offset: 0x002F7ED9
		private void IncrementScrollGeneration()
		{
			this._scrollData._scrollGeneration += 1L;
		}

		// Token: 0x060077F1 RID: 30705 RVA: 0x002F8EF0 File Offset: 0x002F7EF0
		private void ExtendPixelAndLogicalSizes(IList children, IList items, int itemCount, IContainItemStorage itemStorageProvider, bool areContainersUniformlySized, double uniformOrAverageContainerSize, double uniformOrAverageContainerPixelSize, ref Size stackPixelSize, ref Size stackLogicalSize, bool isHorizontal, int pivotIndex, int pivotChildIndex, int firstContainerInViewportIndex, bool before)
		{
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			double num = 0.0;
			double num2;
			if (before)
			{
				if (isVSP45Compat)
				{
					this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, pivotIndex, out num2);
				}
				else
				{
					this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize, 0, pivotIndex, out num2, out num);
					if (!this.IsPixelBased)
					{
						double num3;
						double num4;
						this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize, pivotIndex, firstContainerInViewportIndex - pivotIndex, out num3, out num4);
						this._pixelDistanceToViewport = num + num4;
						this._pixelDistanceToFirstContainerInExtendedViewport = num;
					}
				}
			}
			else if (isVSP45Compat)
			{
				this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, pivotIndex, itemCount - pivotIndex, out num2);
			}
			else
			{
				this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize, pivotIndex, itemCount - pivotIndex, out num2, out num);
			}
			if (!this.IsPixelBased)
			{
				if (isHorizontal)
				{
					stackLogicalSize.Width += num2;
				}
				else
				{
					stackLogicalSize.Height += num2;
				}
				if (isVSP45Compat)
				{
					if (!this.IsScrolling)
					{
						int num5;
						int num6;
						if (before)
						{
							num5 = 0;
							num6 = pivotChildIndex;
						}
						else
						{
							num5 = pivotChildIndex;
							num6 = children.Count;
						}
						for (int i = num5; i < num6; i++)
						{
							Size desiredSize = ((UIElement)children[i]).DesiredSize;
							if (isHorizontal)
							{
								stackPixelSize.Width += desiredSize.Width;
							}
							else
							{
								stackPixelSize.Height += desiredSize.Height;
							}
						}
						return;
					}
				}
				else if (!this.IsScrolling)
				{
					if (isHorizontal)
					{
						stackPixelSize.Width += num;
						return;
					}
					stackPixelSize.Height += num;
				}
				return;
			}
			if (isHorizontal)
			{
				stackPixelSize.Width += num2;
				return;
			}
			stackPixelSize.Height += num2;
		}

		// Token: 0x060077F2 RID: 30706 RVA: 0x002F90AC File Offset: 0x002F80AC
		private void ComputeDistance(IList items, IContainItemStorage itemStorageProvider, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, int startIndex, int itemCount, out double distance)
		{
			if (!this.IsPixelBased && !VirtualizingStackPanel.IsVSP45Compat)
			{
				double num;
				this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 1.0, startIndex, itemCount, out distance, out num);
				return;
			}
			distance = 0.0;
			if (!areContainersUniformlySized)
			{
				for (int i = startIndex; i < startIndex + itemCount; i++)
				{
					object item = items[i];
					Size size;
					this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, out size);
					if (isHorizontal)
					{
						distance += size.Width;
					}
					else
					{
						distance += size.Height;
					}
				}
				return;
			}
			if (isHorizontal)
			{
				distance += uniformOrAverageContainerSize * (double)itemCount;
				return;
			}
			distance += uniformOrAverageContainerSize * (double)itemCount;
		}

		// Token: 0x060077F3 RID: 30707 RVA: 0x002F9160 File Offset: 0x002F8160
		private void ComputeDistance(IList items, IContainItemStorage itemStorageProvider, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, double uniformOrAverageContainerPixelSize, int startIndex, int itemCount, out double distance, out double pixelDistance)
		{
			distance = 0.0;
			pixelDistance = 0.0;
			if (areContainersUniformlySized)
			{
				distance += uniformOrAverageContainerSize * (double)itemCount;
				pixelDistance += uniformOrAverageContainerPixelSize * (double)itemCount;
				return;
			}
			for (int i = startIndex; i < startIndex + itemCount; i++)
			{
				object item = items[i];
				Size size;
				Size size2;
				this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize, out size, out size2);
				if (isHorizontal)
				{
					distance += size.Width;
					pixelDistance += size2.Width;
				}
				else
				{
					distance += size.Height;
					pixelDistance += size2.Height;
				}
			}
		}

		// Token: 0x060077F4 RID: 30708 RVA: 0x002F9210 File Offset: 0x002F8210
		private void GetContainerSizeForItem(IContainItemStorage itemStorageProvider, object item, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, out Size containerSize)
		{
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				Size size;
				this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 1.0, out containerSize, out size);
				return;
			}
			containerSize = Size.Empty;
			if (areContainersUniformlySized)
			{
				containerSize = default(Size);
				if (isHorizontal)
				{
					containerSize.Width = uniformOrAverageContainerSize;
					containerSize.Height = (this.IsPixelBased ? base.DesiredSize.Height : 1.0);
					return;
				}
				containerSize.Height = uniformOrAverageContainerSize;
				containerSize.Width = (this.IsPixelBased ? base.DesiredSize.Width : 1.0);
				return;
			}
			else
			{
				object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeProperty);
				if (obj != null)
				{
					containerSize = (Size)obj;
					return;
				}
				containerSize = default(Size);
				if (isHorizontal)
				{
					containerSize.Width = uniformOrAverageContainerSize;
					containerSize.Height = (this.IsPixelBased ? base.DesiredSize.Height : 1.0);
					return;
				}
				containerSize.Height = uniformOrAverageContainerSize;
				containerSize.Width = (this.IsPixelBased ? base.DesiredSize.Width : 1.0);
				return;
			}
		}

		// Token: 0x060077F5 RID: 30709 RVA: 0x002F9350 File Offset: 0x002F8350
		private void GetContainerSizeForItem(IContainItemStorage itemStorageProvider, object item, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, double uniformOrAverageContainerPixelSize, out Size containerSize, out Size containerPixelSize)
		{
			containerSize = default(Size);
			containerPixelSize = default(Size);
			bool flag = areContainersUniformlySized;
			if (!areContainersUniformlySized)
			{
				if (this.IsPixelBased)
				{
					object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeProperty);
					if (obj != null)
					{
						containerSize = (Size)obj;
						containerPixelSize = containerSize;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					object obj2 = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeDualProperty);
					if (obj2 != null)
					{
						VirtualizingStackPanel.ContainerSizeDual containerSizeDual = (VirtualizingStackPanel.ContainerSizeDual)obj2;
						containerSize = containerSizeDual.ItemSize;
						containerPixelSize = containerSizeDual.PixelSize;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				if (isHorizontal)
				{
					double height = base.DesiredSize.Height;
					containerSize.Width = uniformOrAverageContainerSize;
					containerSize.Height = (this.IsPixelBased ? height : 1.0);
					containerPixelSize.Width = uniformOrAverageContainerPixelSize;
					containerPixelSize.Height = height;
					return;
				}
				double width = base.DesiredSize.Width;
				containerSize.Height = uniformOrAverageContainerSize;
				containerSize.Width = (this.IsPixelBased ? width : 1.0);
				containerPixelSize.Height = uniformOrAverageContainerPixelSize;
				containerPixelSize.Width = width;
			}
		}

		// Token: 0x060077F6 RID: 30710 RVA: 0x002F947C File Offset: 0x002F847C
		private void SetContainerSizeForItem(IContainItemStorage itemStorageProvider, IContainItemStorage parentItemStorageProvider, object parentItem, object item, Size containerSize, bool isHorizontal, ref bool hasUniformOrAverageContainerSizeBeenSet, ref double uniformOrAverageContainerSize, ref bool areContainersUniformlySized)
		{
			if (!hasUniformOrAverageContainerSizeBeenSet)
			{
				if (VirtualizingStackPanel.IsVSP45Compat)
				{
					parentItemStorageProvider = itemStorageProvider;
				}
				hasUniformOrAverageContainerSizeBeenSet = true;
				uniformOrAverageContainerSize = (isHorizontal ? containerSize.Width : containerSize.Height);
				this.SetUniformOrAverageContainerSize(parentItemStorageProvider, parentItem, uniformOrAverageContainerSize, 1.0);
			}
			else if (areContainersUniformlySized)
			{
				if (isHorizontal)
				{
					areContainersUniformlySized = DoubleUtil.AreClose(containerSize.Width, uniformOrAverageContainerSize);
				}
				else
				{
					areContainersUniformlySized = DoubleUtil.AreClose(containerSize.Height, uniformOrAverageContainerSize);
				}
			}
			if (!areContainersUniformlySized)
			{
				itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.ContainerSizeProperty, containerSize);
			}
		}

		// Token: 0x060077F7 RID: 30711 RVA: 0x002F9514 File Offset: 0x002F8514
		private void SetContainerSizeForItem(IContainItemStorage itemStorageProvider, IContainItemStorage parentItemStorageProvider, object parentItem, object item, Size containerSize, Size containerPixelSize, bool isHorizontal, bool hasVirtualizingChildren, ref bool hasUniformOrAverageContainerSizeBeenSet, ref double uniformOrAverageContainerSize, ref double uniformOrAverageContainerPixelSize, ref bool areContainersUniformlySized, ref bool hasAnyContainerSpanChanged)
		{
			if (!hasUniformOrAverageContainerSizeBeenSet)
			{
				hasUniformOrAverageContainerSizeBeenSet = true;
				uniformOrAverageContainerSize = (isHorizontal ? containerSize.Width : containerSize.Height);
				uniformOrAverageContainerPixelSize = (isHorizontal ? containerPixelSize.Width : containerPixelSize.Height);
				this.SetUniformOrAverageContainerSize(parentItemStorageProvider, parentItem, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize);
			}
			else if (areContainersUniformlySized)
			{
				bool flag = this.IsPixelBased || (this.IsScrolling && !hasVirtualizingChildren);
				if (isHorizontal)
				{
					areContainersUniformlySized = (DoubleUtil.AreClose(containerSize.Width, uniformOrAverageContainerSize) && (flag || DoubleUtil.AreClose(containerPixelSize.Width, uniformOrAverageContainerPixelSize)));
				}
				else
				{
					areContainersUniformlySized = (DoubleUtil.AreClose(containerSize.Height, uniformOrAverageContainerSize) && (flag || DoubleUtil.AreClose(containerPixelSize.Height, uniformOrAverageContainerPixelSize)));
				}
			}
			if (!areContainersUniformlySized)
			{
				double value = 0.0;
				double value2 = 0.0;
				bool flag2 = VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this);
				if (this.IsPixelBased)
				{
					object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeProperty);
					Size size = (obj != null) ? ((Size)obj) : Size.Empty;
					if (obj == null || containerSize != size)
					{
						if (flag2)
						{
							ItemContainerGenerator itemContainerGenerator = (ItemContainerGenerator)base.Generator;
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetContainerSize, new object[]
							{
								itemContainerGenerator.IndexFromContainer(itemContainerGenerator.ContainerFromItem(item)),
								size,
								containerSize
							});
						}
						if (isHorizontal)
						{
							value = ((obj != null) ? size.Width : uniformOrAverageContainerSize);
							value2 = containerSize.Width;
						}
						else
						{
							value = ((obj != null) ? size.Height : uniformOrAverageContainerSize);
							value2 = containerSize.Height;
						}
					}
					itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.ContainerSizeProperty, containerSize);
				}
				else
				{
					object obj2 = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeDualProperty);
					VirtualizingStackPanel.ContainerSizeDual containerSizeDual = (obj2 != null) ? ((VirtualizingStackPanel.ContainerSizeDual)obj2) : new VirtualizingStackPanel.ContainerSizeDual(Size.Empty, Size.Empty);
					if (obj2 == null || containerSize != containerSizeDual.ItemSize || containerPixelSize != containerSizeDual.PixelSize)
					{
						if (flag2)
						{
							ItemContainerGenerator itemContainerGenerator2 = (ItemContainerGenerator)base.Generator;
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetContainerSize, new object[]
							{
								itemContainerGenerator2.IndexFromContainer(itemContainerGenerator2.ContainerFromItem(item)),
								containerSizeDual.ItemSize,
								containerSize,
								containerSizeDual.PixelSize,
								containerPixelSize
							});
						}
						if (isHorizontal)
						{
							value = ((obj2 != null) ? containerSizeDual.ItemSize.Width : uniformOrAverageContainerSize);
							value2 = containerSize.Width;
						}
						else
						{
							value = ((obj2 != null) ? containerSizeDual.ItemSize.Height : uniformOrAverageContainerSize);
							value2 = containerSize.Height;
						}
					}
					VirtualizingStackPanel.ContainerSizeDual value3 = new VirtualizingStackPanel.ContainerSizeDual(containerPixelSize, containerSize);
					itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.ContainerSizeDualProperty, value3);
				}
				if (!LayoutDoubleUtil.AreClose(value, value2))
				{
					hasAnyContainerSpanChanged = true;
				}
			}
		}

		// Token: 0x060077F8 RID: 30712 RVA: 0x002F9820 File Offset: 0x002F8820
		private Thickness GetItemsHostInsetForChild(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, IContainItemStorage parentItemStorageProvider = null, object parentItem = null)
		{
			FrameworkElement frameworkElement = virtualizationInfoProvider as FrameworkElement;
			if (parentItemStorageProvider == null)
			{
				return (Thickness)frameworkElement.GetValue(VirtualizingStackPanel.ItemsHostInsetProperty);
			}
			Thickness thickness = default(Thickness);
			object obj = parentItemStorageProvider.ReadItemValue(parentItem, VirtualizingStackPanel.ItemsHostInsetProperty);
			if (obj != null)
			{
				thickness = (Thickness)obj;
			}
			else if ((obj = frameworkElement.ReadLocalValue(VirtualizingStackPanel.ItemsHostInsetProperty)) != DependencyProperty.UnsetValue)
			{
				thickness = (Thickness)obj;
			}
			else
			{
				HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = virtualizationInfoProvider.HeaderDesiredSizes;
				Thickness margin = frameworkElement.Margin;
				thickness.Top = headerDesiredSizes.PixelSize.Height + margin.Top;
				thickness.Left = headerDesiredSizes.PixelSize.Width + margin.Left;
				parentItemStorageProvider.StoreItemValue(parentItem, VirtualizingStackPanel.ItemsHostInsetProperty, thickness);
			}
			frameworkElement.SetValue(VirtualizingStackPanel.ItemsHostInsetProperty, thickness);
			return thickness;
		}

		// Token: 0x060077F9 RID: 30713 RVA: 0x002F98F8 File Offset: 0x002F88F8
		private void SetItemsHostInsetForChild(int index, UIElement child, IContainItemStorage itemStorageProvider, bool isHorizontal)
		{
			bool flag = isHorizontal;
			IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(child, ref flag);
			Panel panel = (virtualizingChild == null) ? null : virtualizingChild.ItemsHost;
			if (panel == null || !panel.IsVisible)
			{
				return;
			}
			GeneralTransform generalTransform = child.TransformToDescendant(panel);
			if (generalTransform == null)
			{
				return;
			}
			FrameworkElement frameworkElement = virtualizingChild as FrameworkElement;
			Thickness thickness = (frameworkElement == null) ? default(Thickness) : frameworkElement.Margin;
			Rect rect = new Rect(default(Point), child.DesiredSize);
			rect.Offset(-thickness.Left, -thickness.Top);
			Rect rect2 = generalTransform.TransformBounds(rect);
			Size desiredSize = panel.DesiredSize;
			double left = DoubleUtil.AreClose(0.0, rect2.Left) ? 0.0 : (-rect2.Left);
			double top = DoubleUtil.AreClose(0.0, rect2.Top) ? 0.0 : (-rect2.Top);
			double right = DoubleUtil.AreClose(desiredSize.Width, rect2.Right) ? 0.0 : (rect2.Right - desiredSize.Width);
			double bottom = DoubleUtil.AreClose(desiredSize.Height, rect2.Bottom) ? 0.0 : (rect2.Bottom - desiredSize.Height);
			Thickness thickness2 = new Thickness(left, top, right, bottom);
			object itemFromContainer = this.GetItemFromContainer(child);
			if (itemFromContainer == DependencyProperty.UnsetValue)
			{
				return;
			}
			object obj = itemStorageProvider.ReadItemValue(itemFromContainer, VirtualizingStackPanel.ItemsHostInsetProperty);
			bool flag2 = obj == null;
			bool flag3 = flag2;
			if (!flag2)
			{
				Thickness thickness3 = (Thickness)obj;
				flag2 = (!DoubleUtil.AreClose(thickness3.Left, thickness2.Left) || !DoubleUtil.AreClose(thickness3.Top, thickness2.Top) || !DoubleUtil.AreClose(thickness3.Right, thickness2.Right) || !DoubleUtil.AreClose(thickness3.Bottom, thickness2.Bottom));
				flag3 = (flag2 && ((isHorizontal && (!VirtualizingStackPanel.AreInsetsClose(thickness3.Left, thickness2.Left) || !VirtualizingStackPanel.AreInsetsClose(thickness3.Right, thickness2.Right))) || (!isHorizontal && (!VirtualizingStackPanel.AreInsetsClose(thickness3.Top, thickness2.Top) || !VirtualizingStackPanel.AreInsetsClose(thickness3.Bottom, thickness2.Bottom)))));
			}
			if (flag2)
			{
				itemStorageProvider.StoreItemValue(itemFromContainer, VirtualizingStackPanel.ItemsHostInsetProperty, thickness2);
				child.SetValue(VirtualizingStackPanel.ItemsHostInsetProperty, thickness2);
			}
			if (flag3)
			{
				ItemsControl scrollingItemsControl = this.GetScrollingItemsControl(child);
				Panel panel2 = (scrollingItemsControl == null) ? null : scrollingItemsControl.ItemsHost;
				if (panel2 != null)
				{
					VirtualizingStackPanel virtualizingStackPanel = panel2 as VirtualizingStackPanel;
					if (virtualizingStackPanel != null)
					{
						virtualizingStackPanel.AnchoredInvalidateMeasure();
						return;
					}
					panel2.InvalidateMeasure();
				}
			}
		}

		// Token: 0x060077FA RID: 30714 RVA: 0x002F9BD0 File Offset: 0x002F8BD0
		private static bool AreInsetsClose(double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			double num = (Math.Abs(value1) + Math.Abs(value2)) * 0.001;
			double num2 = value1 - value2;
			return -num <= num2 && num >= num2;
		}

		// Token: 0x060077FB RID: 30715 RVA: 0x002F9C10 File Offset: 0x002F8C10
		private ItemsControl GetScrollingItemsControl(UIElement container)
		{
			if (container is TreeViewItem)
			{
				for (ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container); itemsControl != null; itemsControl = ItemsControl.ItemsControlFromItemContainer(itemsControl))
				{
					TreeView treeView = itemsControl as TreeView;
					if (treeView != null)
					{
						return treeView;
					}
				}
			}
			else if (container is GroupItem)
			{
				DependencyObject dependencyObject = container;
				ItemsControl itemsControl2;
				for (;;)
				{
					dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
					itemsControl2 = (dependencyObject as ItemsControl);
					if (itemsControl2 != null)
					{
						break;
					}
					if (dependencyObject == null)
					{
						goto Block_6;
					}
				}
				return itemsControl2;
				Block_6:;
			}
			else if (container != null)
			{
				string name = container.GetType().Name;
			}
			return null;
		}

		// Token: 0x060077FC RID: 30716 RVA: 0x002F9C78 File Offset: 0x002F8C78
		private object GetItemFromContainer(DependencyObject container)
		{
			return container.ReadLocalValue(System.Windows.Controls.ItemContainerGenerator.ItemForItemContainerProperty);
		}

		// Token: 0x060077FD RID: 30717 RVA: 0x002F9C88 File Offset: 0x002F8C88
		private bool IsHeaderBeforeItems(bool isHorizontal, FrameworkElement container, ref Thickness inset)
		{
			Thickness thickness = (container == null) ? default(Thickness) : container.Margin;
			if (isHorizontal)
			{
				return DoubleUtil.GreaterThanOrClose(inset.Left - thickness.Left, inset.Right - thickness.Right);
			}
			return DoubleUtil.GreaterThanOrClose(inset.Top - thickness.Top, inset.Bottom - thickness.Bottom);
		}

		// Token: 0x060077FE RID: 30718 RVA: 0x002F9CF0 File Offset: 0x002F8CF0
		private bool IsEndOfCache(bool isHorizontal, double cacheSize, VirtualizationCacheLengthUnit cacheUnit, Size stackPixelSizeInCache, Size stackLogicalSizeInCache)
		{
			if (!this.MeasureCaches)
			{
				return true;
			}
			if (cacheUnit == VirtualizationCacheLengthUnit.Item)
			{
				if (isHorizontal)
				{
					return DoubleUtil.GreaterThanOrClose(stackLogicalSizeInCache.Width, cacheSize);
				}
				return DoubleUtil.GreaterThanOrClose(stackLogicalSizeInCache.Height, cacheSize);
			}
			else
			{
				if (cacheUnit != VirtualizationCacheLengthUnit.Pixel)
				{
					return false;
				}
				if (isHorizontal)
				{
					return DoubleUtil.GreaterThanOrClose(stackPixelSizeInCache.Width, cacheSize);
				}
				return DoubleUtil.GreaterThanOrClose(stackPixelSizeInCache.Height, cacheSize);
			}
		}

		// Token: 0x060077FF RID: 30719 RVA: 0x002F9D4D File Offset: 0x002F8D4D
		private bool IsEndOfViewport(bool isHorizontal, Rect viewport, Size stackPixelSizeInViewport)
		{
			if (isHorizontal)
			{
				return DoubleUtil.GreaterThanOrClose(stackPixelSizeInViewport.Width, viewport.Width);
			}
			return DoubleUtil.GreaterThanOrClose(stackPixelSizeInViewport.Height, viewport.Height);
		}

		// Token: 0x06007800 RID: 30720 RVA: 0x002F9D79 File Offset: 0x002F8D79
		private bool IsViewportEmpty(bool isHorizontal, Rect viewport)
		{
			if (isHorizontal)
			{
				return DoubleUtil.AreClose(viewport.Width, 0.0);
			}
			return DoubleUtil.AreClose(viewport.Height, 0.0);
		}

		// Token: 0x06007801 RID: 30721 RVA: 0x002F9DAC File Offset: 0x002F8DAC
		private void SetViewportForChild(bool isHorizontal, IContainItemStorage itemStorageProvider, bool areContainersUniformlySized, double uniformOrAverageContainerSize, bool mustDisableVirtualization, UIElement child, IHierarchicalVirtualizationAndScrollInfo virtualizingChild, object item, bool isBeforeFirstItem, bool isAfterFirstItem, double firstItemInViewportOffset, Rect parentViewport, VirtualizationCacheLength parentCacheSize, VirtualizationCacheLengthUnit parentCacheUnit, long scrollGeneration, Size stackPixelSize, Size stackPixelSizeInViewport, Size stackPixelSizeInCacheBeforeViewport, Size stackPixelSizeInCacheAfterViewport, Size stackLogicalSize, Size stackLogicalSizeInViewport, Size stackLogicalSizeInCacheBeforeViewport, Size stackLogicalSizeInCacheAfterViewport, out Rect childViewport, ref VirtualizationCacheLength childCacheSize, ref VirtualizationCacheLengthUnit childCacheUnit)
		{
			childViewport = parentViewport;
			if (isHorizontal)
			{
				if (isBeforeFirstItem)
				{
					Size size;
					this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, out size);
					childViewport.X = (this.IsPixelBased ? stackPixelSizeInCacheBeforeViewport.Width : stackLogicalSizeInCacheBeforeViewport.Width) + size.Width;
					childViewport.Width = 0.0;
				}
				else if (isAfterFirstItem)
				{
					childViewport.X = Math.Min(childViewport.X, 0.0) - (this.IsPixelBased ? (stackPixelSizeInViewport.Width + stackPixelSizeInCacheAfterViewport.Width) : (stackLogicalSizeInViewport.Width + stackLogicalSizeInCacheAfterViewport.Width));
					childViewport.Width = Math.Max(childViewport.Width - stackPixelSizeInViewport.Width, 0.0);
				}
				else
				{
					childViewport.X -= firstItemInViewportOffset;
					childViewport.Width = Math.Max(childViewport.Width - stackPixelSizeInViewport.Width, 0.0);
				}
				if (parentCacheUnit == VirtualizationCacheLengthUnit.Item)
				{
					childCacheSize = new VirtualizationCacheLength((isAfterFirstItem || DoubleUtil.LessThanOrClose(childViewport.X, 0.0)) ? 0.0 : Math.Max(parentCacheSize.CacheBeforeViewport - stackLogicalSizeInCacheBeforeViewport.Width, 0.0), isBeforeFirstItem ? 0.0 : Math.Max(parentCacheSize.CacheAfterViewport - stackLogicalSizeInCacheAfterViewport.Width, 0.0));
					childCacheUnit = VirtualizationCacheLengthUnit.Item;
				}
				else if (parentCacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					childCacheSize = new VirtualizationCacheLength((isAfterFirstItem || DoubleUtil.LessThanOrClose(childViewport.X, 0.0)) ? 0.0 : Math.Max(parentCacheSize.CacheBeforeViewport - stackPixelSizeInCacheBeforeViewport.Width, 0.0), isBeforeFirstItem ? 0.0 : Math.Max(parentCacheSize.CacheAfterViewport - stackPixelSizeInCacheAfterViewport.Width, 0.0));
					childCacheUnit = VirtualizationCacheLengthUnit.Pixel;
				}
			}
			else
			{
				if (isBeforeFirstItem)
				{
					Size size2;
					this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, out size2);
					childViewport.Y = (this.IsPixelBased ? stackPixelSizeInCacheBeforeViewport.Height : stackLogicalSizeInCacheBeforeViewport.Height) + size2.Height;
					childViewport.Height = 0.0;
				}
				else if (isAfterFirstItem)
				{
					childViewport.Y = Math.Min(childViewport.Y, 0.0) - (this.IsPixelBased ? (stackPixelSizeInViewport.Height + stackPixelSizeInCacheAfterViewport.Height) : (stackLogicalSizeInViewport.Height + stackLogicalSizeInCacheAfterViewport.Height));
					childViewport.Height = Math.Max(childViewport.Height - stackPixelSizeInViewport.Height, 0.0);
				}
				else
				{
					childViewport.Y -= firstItemInViewportOffset;
					childViewport.Height = Math.Max(childViewport.Height - stackPixelSizeInViewport.Height, 0.0);
				}
				if (parentCacheUnit == VirtualizationCacheLengthUnit.Item)
				{
					childCacheSize = new VirtualizationCacheLength((isAfterFirstItem || DoubleUtil.LessThanOrClose(childViewport.Y, 0.0)) ? 0.0 : Math.Max(parentCacheSize.CacheBeforeViewport - stackLogicalSizeInCacheBeforeViewport.Height, 0.0), isBeforeFirstItem ? 0.0 : Math.Max(parentCacheSize.CacheAfterViewport - stackLogicalSizeInCacheAfterViewport.Height, 0.0));
					childCacheUnit = VirtualizationCacheLengthUnit.Item;
				}
				else if (parentCacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					childCacheSize = new VirtualizationCacheLength((isAfterFirstItem || DoubleUtil.LessThanOrClose(childViewport.Y, 0.0)) ? 0.0 : Math.Max(parentCacheSize.CacheBeforeViewport - stackPixelSizeInCacheBeforeViewport.Height, 0.0), isBeforeFirstItem ? 0.0 : Math.Max(parentCacheSize.CacheAfterViewport - stackPixelSizeInCacheAfterViewport.Height, 0.0));
					childCacheUnit = VirtualizationCacheLengthUnit.Pixel;
				}
			}
			if (virtualizingChild != null)
			{
				virtualizingChild.Constraints = new HierarchicalVirtualizationConstraints(childCacheSize, childCacheUnit, childViewport)
				{
					ScrollGeneration = scrollGeneration
				};
				virtualizingChild.InBackgroundLayout = this.MeasureCaches;
				virtualizingChild.MustDisableVirtualization = mustDisableVirtualization;
			}
			if (child is IHierarchicalVirtualizationAndScrollInfo)
			{
				this.InvalidateMeasureOnItemsHost((IHierarchicalVirtualizationAndScrollInfo)child);
			}
		}

		// Token: 0x06007802 RID: 30722 RVA: 0x002FA22C File Offset: 0x002F922C
		private void InvalidateMeasureOnItemsHost(IHierarchicalVirtualizationAndScrollInfo virtualizingChild)
		{
			Panel itemsHost = virtualizingChild.ItemsHost;
			if (itemsHost != null)
			{
				Helper.InvalidateMeasureOnPath(itemsHost, this, true);
				if (!(itemsHost is VirtualizingStackPanel))
				{
					IList internalChildren = itemsHost.InternalChildren;
					for (int i = 0; i < internalChildren.Count; i++)
					{
						IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = internalChildren[i] as IHierarchicalVirtualizationAndScrollInfo;
						if (hierarchicalVirtualizationAndScrollInfo != null)
						{
							this.InvalidateMeasureOnItemsHost(hierarchicalVirtualizationAndScrollInfo);
						}
					}
				}
			}
		}

		// Token: 0x06007803 RID: 30723 RVA: 0x002FA284 File Offset: 0x002F9284
		private void GetSizesForChild(bool isHorizontal, bool isChildHorizontal, bool isBeforeFirstItem, bool isAfterLastItem, IHierarchicalVirtualizationAndScrollInfo virtualizingChild, Size childDesiredSize, Rect childViewport, VirtualizationCacheLength childCacheSize, VirtualizationCacheLengthUnit childCacheUnit, out Size childPixelSize, out Size childPixelSizeInViewport, out Size childPixelSizeInCacheBeforeViewport, out Size childPixelSizeInCacheAfterViewport, out Size childLogicalSize, out Size childLogicalSizeInViewport, out Size childLogicalSizeInCacheBeforeViewport, out Size childLogicalSizeInCacheAfterViewport)
		{
			childPixelSize = default(Size);
			childPixelSizeInViewport = default(Size);
			childPixelSizeInCacheBeforeViewport = default(Size);
			childPixelSizeInCacheAfterViewport = default(Size);
			childLogicalSize = default(Size);
			childLogicalSizeInViewport = default(Size);
			childLogicalSizeInCacheBeforeViewport = default(Size);
			childLogicalSizeInCacheAfterViewport = default(Size);
			if (virtualizingChild != null)
			{
				RelativeHeaderPosition relativeHeaderPosition = RelativeHeaderPosition.Top;
				HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = virtualizingChild.HeaderDesiredSizes;
				HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizingChild.ItemDesiredSizes;
				Size pixelSize = headerDesiredSizes.PixelSize;
				Size logicalSize = headerDesiredSizes.LogicalSize;
				childPixelSize = childDesiredSize;
				if (relativeHeaderPosition == RelativeHeaderPosition.Top || relativeHeaderPosition == RelativeHeaderPosition.Bottom)
				{
					childLogicalSize.Height = itemDesiredSizes.LogicalSize.Height + logicalSize.Height;
					childLogicalSize.Width = Math.Max(itemDesiredSizes.LogicalSize.Width, logicalSize.Width);
				}
				else
				{
					childLogicalSize.Width = itemDesiredSizes.LogicalSize.Width + logicalSize.Width;
					childLogicalSize.Height = Math.Max(itemDesiredSizes.LogicalSize.Height, logicalSize.Height);
				}
				if (this.IsPixelBased && ((isHorizontal && DoubleUtil.AreClose(itemDesiredSizes.PixelSize.Width, itemDesiredSizes.PixelSizeInViewport.Width)) || (!isHorizontal && DoubleUtil.AreClose(itemDesiredSizes.PixelSize.Height, itemDesiredSizes.PixelSizeInViewport.Height))))
				{
					Rect childViewport2 = childViewport;
					if (relativeHeaderPosition == RelativeHeaderPosition.Top || relativeHeaderPosition == RelativeHeaderPosition.Left)
					{
						VirtualizationCacheLength virtualizationCacheLength = childCacheSize;
						VirtualizationCacheLengthUnit virtualizationCacheLengthUnit = childCacheUnit;
						this.AdjustNonScrollingViewportForHeader(virtualizingChild, ref childViewport2, ref virtualizationCacheLength, ref virtualizationCacheLengthUnit);
					}
					this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, itemDesiredSizes.PixelSizeInViewport, itemDesiredSizes.LogicalSizeInViewport, childViewport2, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
				}
				else
				{
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInViewport, itemDesiredSizes.PixelSizeInViewport);
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInViewport, itemDesiredSizes.LogicalSizeInViewport);
				}
				if (isChildHorizontal == isHorizontal)
				{
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInCacheBeforeViewport, itemDesiredSizes.PixelSizeBeforeViewport);
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInCacheBeforeViewport, itemDesiredSizes.LogicalSizeBeforeViewport);
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInCacheAfterViewport, itemDesiredSizes.PixelSizeAfterViewport);
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInCacheAfterViewport, itemDesiredSizes.LogicalSizeAfterViewport);
				}
				Rect childViewport3 = childViewport;
				Size sz = default(Size);
				Size sz2 = default(Size);
				Size sz3 = default(Size);
				Size sz4 = default(Size);
				Size sz5 = default(Size);
				Size sz6 = default(Size);
				object obj = relativeHeaderPosition == RelativeHeaderPosition.Left || relativeHeaderPosition == RelativeHeaderPosition.Right;
				if (relativeHeaderPosition == RelativeHeaderPosition.Bottom || relativeHeaderPosition == RelativeHeaderPosition.Right)
				{
					VirtualizationCacheLength virtualizationCacheLength2 = childCacheSize;
					VirtualizationCacheLengthUnit virtualizationCacheLengthUnit2 = childCacheUnit;
					this.AdjustNonScrollingViewportForItems(virtualizingChild, ref childViewport3, ref virtualizationCacheLength2, ref virtualizationCacheLengthUnit2);
				}
				if (isBeforeFirstItem)
				{
					sz3 = pixelSize;
					sz4 = logicalSize;
				}
				else if (isAfterLastItem)
				{
					sz5 = pixelSize;
					sz6 = logicalSize;
				}
				else
				{
					this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, pixelSize, logicalSize, childViewport3, ref sz, ref sz2, ref sz3, ref sz4, ref sz5, ref sz6);
				}
				object isHorizontal2 = obj;
				VirtualizingStackPanel.StackSizes(isHorizontal2 != null, ref childPixelSizeInViewport, sz);
				VirtualizingStackPanel.StackSizes(isHorizontal2 != null, ref childLogicalSizeInViewport, sz2);
				VirtualizingStackPanel.StackSizes(isHorizontal2 != null, ref childPixelSizeInCacheBeforeViewport, sz3);
				VirtualizingStackPanel.StackSizes(isHorizontal2 != null, ref childLogicalSizeInCacheBeforeViewport, sz4);
				VirtualizingStackPanel.StackSizes(isHorizontal2 != null, ref childPixelSizeInCacheAfterViewport, sz5);
				VirtualizingStackPanel.StackSizes(isHorizontal2 != null, ref childLogicalSizeInCacheAfterViewport, sz6);
				return;
			}
			childPixelSize = childDesiredSize;
			childLogicalSize = new Size((double)(DoubleUtil.GreaterThan(childPixelSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSize.Height, 0.0) ? 1 : 0));
			if (isBeforeFirstItem)
			{
				childPixelSizeInCacheBeforeViewport = childDesiredSize;
				childLogicalSizeInCacheBeforeViewport = new Size((double)(DoubleUtil.GreaterThan(childPixelSizeInCacheBeforeViewport.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSizeInCacheBeforeViewport.Height, 0.0) ? 1 : 0));
				return;
			}
			if (isAfterLastItem)
			{
				childPixelSizeInCacheAfterViewport = childDesiredSize;
				childLogicalSizeInCacheAfterViewport = new Size((double)(DoubleUtil.GreaterThan(childPixelSizeInCacheAfterViewport.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSizeInCacheAfterViewport.Height, 0.0) ? 1 : 0));
				return;
			}
			this.GetSizesForChildIntersectingTheViewport(isHorizontal, isHorizontal, childPixelSize, childLogicalSize, childViewport, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
		}

		// Token: 0x06007804 RID: 30724 RVA: 0x002FA674 File Offset: 0x002F9674
		private void GetSizesForChildWithInset(bool isHorizontal, bool isChildHorizontal, bool isBeforeFirstItem, bool isAfterLastItem, IHierarchicalVirtualizationAndScrollInfo virtualizingChild, Size childDesiredSize, Rect childViewport, VirtualizationCacheLength childCacheSize, VirtualizationCacheLengthUnit childCacheUnit, out Size childPixelSize, out Size childPixelSizeInViewport, out Size childPixelSizeInCacheBeforeViewport, out Size childPixelSizeInCacheAfterViewport, out Size childLogicalSize, out Size childLogicalSizeInViewport, out Size childLogicalSizeInCacheBeforeViewport, out Size childLogicalSizeInCacheAfterViewport)
		{
			childPixelSize = childDesiredSize;
			childPixelSizeInViewport = default(Size);
			childPixelSizeInCacheBeforeViewport = default(Size);
			childPixelSizeInCacheAfterViewport = default(Size);
			childLogicalSize = default(Size);
			childLogicalSizeInViewport = default(Size);
			childLogicalSizeInCacheBeforeViewport = default(Size);
			childLogicalSizeInCacheAfterViewport = default(Size);
			HierarchicalVirtualizationItemDesiredSizes hierarchicalVirtualizationItemDesiredSizes = (virtualizingChild != null) ? virtualizingChild.ItemDesiredSizes : default(HierarchicalVirtualizationItemDesiredSizes);
			if ((!isHorizontal && (hierarchicalVirtualizationItemDesiredSizes.PixelSize.Height > 0.0 || hierarchicalVirtualizationItemDesiredSizes.LogicalSize.Height > 0.0)) || (isHorizontal && (hierarchicalVirtualizationItemDesiredSizes.PixelSize.Width > 0.0 || hierarchicalVirtualizationItemDesiredSizes.LogicalSize.Width > 0.0)))
			{
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInCacheBeforeViewport, hierarchicalVirtualizationItemDesiredSizes.PixelSizeBeforeViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInViewport, hierarchicalVirtualizationItemDesiredSizes.PixelSizeInViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInCacheAfterViewport, hierarchicalVirtualizationItemDesiredSizes.PixelSizeAfterViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSize, hierarchicalVirtualizationItemDesiredSizes.LogicalSize);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInCacheBeforeViewport, hierarchicalVirtualizationItemDesiredSizes.LogicalSizeBeforeViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInViewport, hierarchicalVirtualizationItemDesiredSizes.LogicalSizeInViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInCacheAfterViewport, hierarchicalVirtualizationItemDesiredSizes.LogicalSizeAfterViewport);
				Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(virtualizingChild, null, null);
				bool flag = this.IsHeaderBeforeItems(isHorizontal, virtualizingChild as FrameworkElement, ref itemsHostInsetForChild);
				Size childPixelSize2 = isHorizontal ? new Size(Math.Max(itemsHostInsetForChild.Left, 0.0), childDesiredSize.Height) : new Size(childDesiredSize.Width, Math.Max(itemsHostInsetForChild.Top, 0.0));
				Size size = flag ? new Size(1.0, 1.0) : new Size(0.0, 0.0);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSize, size);
				this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, childPixelSize2, size, childViewport, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
				Size childPixelSize3 = isHorizontal ? new Size(Math.Max(itemsHostInsetForChild.Right, 0.0), childDesiredSize.Height) : new Size(childDesiredSize.Width, Math.Max(itemsHostInsetForChild.Bottom, 0.0));
				Size size2 = flag ? new Size(0.0, 0.0) : new Size(1.0, 1.0);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSize, size2);
				Rect childViewport2 = childViewport;
				if (isHorizontal)
				{
					childViewport2.X -= (this.IsPixelBased ? (childPixelSize2.Width + hierarchicalVirtualizationItemDesiredSizes.PixelSize.Width) : (size.Width + hierarchicalVirtualizationItemDesiredSizes.LogicalSize.Width));
					childViewport2.Width = Math.Max(0.0, childViewport2.Width - childPixelSizeInViewport.Width);
				}
				else
				{
					childViewport2.Y -= (this.IsPixelBased ? (childPixelSize2.Height + hierarchicalVirtualizationItemDesiredSizes.PixelSize.Height) : (size.Height + hierarchicalVirtualizationItemDesiredSizes.LogicalSize.Height));
					childViewport2.Height = Math.Max(0.0, childViewport2.Height - childPixelSizeInViewport.Height);
				}
				this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, childPixelSize3, size2, childViewport2, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
				return;
			}
			childLogicalSize = new Size(1.0, 1.0);
			if (isBeforeFirstItem)
			{
				childPixelSizeInCacheBeforeViewport = childDesiredSize;
				childLogicalSizeInCacheBeforeViewport = new Size((double)(DoubleUtil.GreaterThan(childPixelSizeInCacheBeforeViewport.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSizeInCacheBeforeViewport.Height, 0.0) ? 1 : 0));
				return;
			}
			if (isAfterLastItem)
			{
				childPixelSizeInCacheAfterViewport = childDesiredSize;
				childLogicalSizeInCacheAfterViewport = new Size((double)(DoubleUtil.GreaterThan(childPixelSizeInCacheAfterViewport.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSizeInCacheAfterViewport.Height, 0.0) ? 1 : 0));
				return;
			}
			this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, childPixelSize, childLogicalSize, childViewport, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
		}

		// Token: 0x06007805 RID: 30725 RVA: 0x002FAAD8 File Offset: 0x002F9AD8
		private void GetSizesForChildIntersectingTheViewport(bool isHorizontal, bool childIsHorizontal, Size childPixelSize, Size childLogicalSize, Rect childViewport, ref Size childPixelSizeInViewport, ref Size childLogicalSizeInViewport, ref Size childPixelSizeInCacheBeforeViewport, ref Size childLogicalSizeInCacheBeforeViewport, ref Size childPixelSizeInCacheAfterViewport, ref Size childLogicalSizeInCacheAfterViewport)
		{
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			double num6 = 0.0;
			if (isHorizontal)
			{
				if (this.IsPixelBased)
				{
					if (childIsHorizontal != isHorizontal && (DoubleUtil.GreaterThanOrClose(childViewport.Y, childPixelSize.Height) || DoubleUtil.AreClose(childViewport.Height, 0.0)))
					{
						return;
					}
					num3 = (DoubleUtil.LessThan(childViewport.X, childPixelSize.Width) ? Math.Max(childViewport.X, 0.0) : childPixelSize.Width);
					num = Math.Min(childViewport.Width, childPixelSize.Width - num3);
					num5 = Math.Max(childPixelSize.Width - num - num3, 0.0);
				}
				else
				{
					if (childIsHorizontal != isHorizontal && (DoubleUtil.GreaterThanOrClose(childViewport.Y, childLogicalSize.Height) || DoubleUtil.AreClose(childViewport.Height, 0.0)))
					{
						return;
					}
					if (DoubleUtil.GreaterThanOrClose(childViewport.X, childLogicalSize.Width))
					{
						num3 = childPixelSize.Width;
						if (!isVSP45Compat)
						{
							num4 = childLogicalSize.Width;
						}
					}
					else if (DoubleUtil.GreaterThan(childViewport.Width, 0.0))
					{
						num = childPixelSize.Width;
					}
					else
					{
						num5 = childPixelSize.Width;
						if (!isVSP45Compat)
						{
							num6 = childLogicalSize.Width;
						}
					}
				}
				if (DoubleUtil.GreaterThan(childPixelSize.Width, 0.0))
				{
					num4 = Math.Floor(childLogicalSize.Width * num3 / childPixelSize.Width);
					num6 = Math.Floor(childLogicalSize.Width * num5 / childPixelSize.Width);
					num2 = childLogicalSize.Width - num4 - num6;
				}
				else if (!isVSP45Compat)
				{
					num2 = childLogicalSize.Width - num4 - num6;
				}
				double val = Math.Min(childViewport.Height, childPixelSize.Height - Math.Max(childViewport.Y, 0.0));
				childPixelSizeInViewport.Width += num;
				childPixelSizeInViewport.Height = Math.Max(childPixelSizeInViewport.Height, val);
				childPixelSizeInCacheBeforeViewport.Width += num3;
				childPixelSizeInCacheBeforeViewport.Height = Math.Max(childPixelSizeInCacheBeforeViewport.Height, val);
				childPixelSizeInCacheAfterViewport.Width += num5;
				childPixelSizeInCacheAfterViewport.Height = Math.Max(childPixelSizeInCacheAfterViewport.Height, val);
				childLogicalSizeInViewport.Width += num2;
				childLogicalSizeInViewport.Height = Math.Max(childLogicalSizeInViewport.Height, childLogicalSize.Height);
				childLogicalSizeInCacheBeforeViewport.Width += num4;
				childLogicalSizeInCacheBeforeViewport.Height = Math.Max(childLogicalSizeInCacheBeforeViewport.Height, childLogicalSize.Height);
				childLogicalSizeInCacheAfterViewport.Width += num6;
				childLogicalSizeInCacheAfterViewport.Height = Math.Max(childLogicalSizeInCacheAfterViewport.Height, childLogicalSize.Height);
				return;
			}
			if (this.IsPixelBased)
			{
				if (childIsHorizontal != isHorizontal && (DoubleUtil.GreaterThanOrClose(childViewport.X, childPixelSize.Width) || DoubleUtil.AreClose(childViewport.Width, 0.0)))
				{
					return;
				}
				num3 = (DoubleUtil.LessThan(childViewport.Y, childPixelSize.Height) ? Math.Max(childViewport.Y, 0.0) : childPixelSize.Height);
				num = Math.Min(childViewport.Height, childPixelSize.Height - num3);
				num5 = Math.Max(childPixelSize.Height - num - num3, 0.0);
			}
			else
			{
				if (childIsHorizontal != isHorizontal && (DoubleUtil.GreaterThanOrClose(childViewport.X, childLogicalSize.Width) || DoubleUtil.AreClose(childViewport.Width, 0.0)))
				{
					return;
				}
				if (DoubleUtil.GreaterThanOrClose(childViewport.Y, childLogicalSize.Height))
				{
					num3 = childPixelSize.Height;
					if (!isVSP45Compat)
					{
						num4 = childLogicalSize.Height;
					}
				}
				else if (DoubleUtil.GreaterThan(childViewport.Height, 0.0))
				{
					num = childPixelSize.Height;
				}
				else
				{
					num5 = childPixelSize.Height;
					if (!isVSP45Compat)
					{
						num6 = childLogicalSize.Height;
					}
				}
			}
			if (DoubleUtil.GreaterThan(childPixelSize.Height, 0.0))
			{
				num4 = Math.Floor(childLogicalSize.Height * num3 / childPixelSize.Height);
				num6 = Math.Floor(childLogicalSize.Height * num5 / childPixelSize.Height);
				num2 = childLogicalSize.Height - num4 - num6;
			}
			else if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				num2 = childLogicalSize.Height - num4 - num6;
			}
			double val2 = Math.Min(childViewport.Width, childPixelSize.Width - Math.Max(childViewport.X, 0.0));
			childPixelSizeInViewport.Height += num;
			childPixelSizeInViewport.Width = Math.Max(childPixelSizeInViewport.Width, val2);
			childPixelSizeInCacheBeforeViewport.Height += num3;
			childPixelSizeInCacheBeforeViewport.Width = Math.Max(childPixelSizeInCacheBeforeViewport.Width, val2);
			childPixelSizeInCacheAfterViewport.Height += num5;
			childPixelSizeInCacheAfterViewport.Width = Math.Max(childPixelSizeInCacheAfterViewport.Width, val2);
			childLogicalSizeInViewport.Height += num2;
			childLogicalSizeInViewport.Width = Math.Max(childLogicalSizeInViewport.Width, childLogicalSize.Width);
			childLogicalSizeInCacheBeforeViewport.Height += num4;
			childLogicalSizeInCacheBeforeViewport.Width = Math.Max(childLogicalSizeInCacheBeforeViewport.Width, childLogicalSize.Width);
			childLogicalSizeInCacheAfterViewport.Height += num6;
			childLogicalSizeInCacheAfterViewport.Width = Math.Max(childLogicalSizeInCacheAfterViewport.Width, childLogicalSize.Width);
		}

		// Token: 0x06007806 RID: 30726 RVA: 0x002FB0A4 File Offset: 0x002FA0A4
		private void UpdateStackSizes(bool isHorizontal, bool foundFirstItemInViewport, Size childPixelSize, Size childPixelSizeInViewport, Size childPixelSizeInCacheBeforeViewport, Size childPixelSizeInCacheAfterViewport, Size childLogicalSize, Size childLogicalSizeInViewport, Size childLogicalSizeInCacheBeforeViewport, Size childLogicalSizeInCacheAfterViewport, ref Size stackPixelSize, ref Size stackPixelSizeInViewport, ref Size stackPixelSizeInCacheBeforeViewport, ref Size stackPixelSizeInCacheAfterViewport, ref Size stackLogicalSize, ref Size stackLogicalSizeInViewport, ref Size stackLogicalSizeInCacheBeforeViewport, ref Size stackLogicalSizeInCacheAfterViewport)
		{
			VirtualizingStackPanel.StackSizes(isHorizontal, ref stackPixelSize, childPixelSize);
			VirtualizingStackPanel.StackSizes(isHorizontal, ref stackLogicalSize, childLogicalSize);
			if (foundFirstItemInViewport)
			{
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackPixelSizeInViewport, childPixelSizeInViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackLogicalSizeInViewport, childLogicalSizeInViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackPixelSizeInCacheBeforeViewport, childPixelSizeInCacheBeforeViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackLogicalSizeInCacheBeforeViewport, childLogicalSizeInCacheBeforeViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackPixelSizeInCacheAfterViewport, childPixelSizeInCacheAfterViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackLogicalSizeInCacheAfterViewport, childLogicalSizeInCacheAfterViewport);
			}
		}

		// Token: 0x06007807 RID: 30727 RVA: 0x002FB104 File Offset: 0x002FA104
		private static void StackSizes(bool isHorizontal, ref Size sz1, Size sz2)
		{
			if (isHorizontal)
			{
				sz1.Width += sz2.Width;
				sz1.Height = Math.Max(sz1.Height, sz2.Height);
				return;
			}
			sz1.Height += sz2.Height;
			sz1.Width = Math.Max(sz1.Width, sz2.Width);
		}

		// Token: 0x06007808 RID: 30728 RVA: 0x002FB170 File Offset: 0x002FA170
		private void SyncUniformSizeFlags(object parentItem, IContainItemStorage parentItemStorageProvider, IList children, IList items, IContainItemStorage itemStorageProvider, int itemCount, bool computedAreContainersUniformlySized, double computedUniformOrAverageContainerSize, ref bool areContainersUniformlySized, ref double uniformOrAverageContainerSize, ref bool hasAverageContainerSizeChanged, bool isHorizontal, bool evaluateAreContainersUniformlySized)
		{
			parentItemStorageProvider = itemStorageProvider;
			if (evaluateAreContainersUniformlySized || areContainersUniformlySized != computedAreContainersUniformlySized)
			{
				if (!evaluateAreContainersUniformlySized)
				{
					areContainersUniformlySized = computedAreContainersUniformlySized;
					this.SetAreContainersUniformlySized(parentItemStorageProvider, parentItem, areContainersUniformlySized);
				}
				for (int i = 0; i < children.Count; i++)
				{
					UIElement uielement = children[i] as UIElement;
					if (uielement != null && VirtualizingPanel.GetShouldCacheContainerSize(uielement))
					{
						IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement);
						Size desiredSize;
						if (virtualizingChild != null)
						{
							HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = virtualizingChild.HeaderDesiredSizes;
							HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizingChild.ItemDesiredSizes;
							if (this.IsPixelBased)
							{
								desiredSize = new Size(Math.Max(headerDesiredSizes.PixelSize.Width, itemDesiredSizes.PixelSize.Width), headerDesiredSizes.PixelSize.Height + itemDesiredSizes.PixelSize.Height);
							}
							else
							{
								desiredSize = new Size(Math.Max(headerDesiredSizes.LogicalSize.Width, itemDesiredSizes.LogicalSize.Width), headerDesiredSizes.LogicalSize.Height + itemDesiredSizes.LogicalSize.Height);
							}
						}
						else if (this.IsPixelBased)
						{
							desiredSize = uielement.DesiredSize;
						}
						else
						{
							desiredSize = new Size((double)(DoubleUtil.GreaterThan(uielement.DesiredSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(uielement.DesiredSize.Height, 0.0) ? 1 : 0));
						}
						if (evaluateAreContainersUniformlySized && computedAreContainersUniformlySized)
						{
							if (isHorizontal)
							{
								computedAreContainersUniformlySized = DoubleUtil.AreClose(desiredSize.Width, uniformOrAverageContainerSize);
							}
							else
							{
								computedAreContainersUniformlySized = DoubleUtil.AreClose(desiredSize.Height, uniformOrAverageContainerSize);
							}
							if (!computedAreContainersUniformlySized)
							{
								i = -1;
							}
						}
						else
						{
							itemStorageProvider.StoreItemValue(((ItemContainerGenerator)base.Generator).ItemFromContainer(uielement), VirtualizingStackPanel.ContainerSizeProperty, desiredSize);
						}
					}
				}
				if (evaluateAreContainersUniformlySized)
				{
					areContainersUniformlySized = computedAreContainersUniformlySized;
					this.SetAreContainersUniformlySized(parentItemStorageProvider, parentItem, areContainersUniformlySized);
				}
			}
			if (!computedAreContainersUniformlySized)
			{
				double num = 0.0;
				int num2 = 0;
				for (int j = 0; j < itemCount; j++)
				{
					object obj = itemStorageProvider.ReadItemValue(items[j], VirtualizingStackPanel.ContainerSizeProperty);
					if (obj != null)
					{
						Size size = (Size)obj;
						if (isHorizontal)
						{
							num += size.Width;
							num2++;
						}
						else
						{
							num += size.Height;
							num2++;
						}
					}
				}
				if (num2 > 0)
				{
					if (this.IsPixelBased)
					{
						uniformOrAverageContainerSize = num / (double)num2;
					}
					else
					{
						uniformOrAverageContainerSize = Math.Round(num / (double)num2);
					}
				}
			}
			else
			{
				uniformOrAverageContainerSize = computedUniformOrAverageContainerSize;
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SyncAveSize, new object[]
				{
					uniformOrAverageContainerSize,
					areContainersUniformlySized,
					hasAverageContainerSizeChanged
				});
			}
		}

		// Token: 0x06007809 RID: 30729 RVA: 0x002FB44C File Offset: 0x002FA44C
		private void SyncUniformSizeFlags(object parentItem, IContainItemStorage parentItemStorageProvider, IList children, IList items, IContainItemStorage itemStorageProvider, int itemCount, bool computedAreContainersUniformlySized, double computedUniformOrAverageContainerSize, double computedUniformOrAverageContainerPixelSize, ref bool areContainersUniformlySized, ref double uniformOrAverageContainerSize, ref double uniformOrAverageContainerPixelSize, ref bool hasAverageContainerSizeChanged, bool isHorizontal, bool evaluateAreContainersUniformlySized)
		{
			bool flag = VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this);
			ItemContainerGenerator itemContainerGenerator = (ItemContainerGenerator)base.Generator;
			if (evaluateAreContainersUniformlySized || areContainersUniformlySized != computedAreContainersUniformlySized)
			{
				if (!evaluateAreContainersUniformlySized)
				{
					areContainersUniformlySized = computedAreContainersUniformlySized;
					this.SetAreContainersUniformlySized(parentItemStorageProvider, parentItem, areContainersUniformlySized);
				}
				for (int i = 0; i < children.Count; i++)
				{
					UIElement uielement = children[i] as UIElement;
					if (uielement != null && VirtualizingPanel.GetShouldCacheContainerSize(uielement))
					{
						IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement);
						Size desiredSize;
						Size size;
						if (virtualizingChild != null)
						{
							HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizingChild.ItemDesiredSizes;
							object obj = uielement.ReadLocalValue(VirtualizingStackPanel.ItemsHostInsetProperty);
							if (obj != DependencyProperty.UnsetValue)
							{
								Thickness thickness = (Thickness)obj;
								desiredSize = new Size(thickness.Left + itemDesiredSizes.PixelSize.Width + thickness.Right, thickness.Top + itemDesiredSizes.PixelSize.Height + thickness.Bottom);
							}
							else
							{
								desiredSize = uielement.DesiredSize;
							}
							if (this.IsPixelBased)
							{
								size = desiredSize;
							}
							else
							{
								size = (isHorizontal ? new Size(1.0 + itemDesiredSizes.LogicalSize.Width, Math.Max(1.0, itemDesiredSizes.LogicalSize.Height)) : new Size(Math.Max(1.0, itemDesiredSizes.LogicalSize.Width), 1.0 + itemDesiredSizes.LogicalSize.Height));
							}
						}
						else
						{
							desiredSize = uielement.DesiredSize;
							if (this.IsPixelBased)
							{
								size = desiredSize;
							}
							else
							{
								size = new Size((double)(DoubleUtil.GreaterThan(uielement.DesiredSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(uielement.DesiredSize.Height, 0.0) ? 1 : 0));
							}
						}
						if (evaluateAreContainersUniformlySized && computedAreContainersUniformlySized)
						{
							if (isHorizontal)
							{
								computedAreContainersUniformlySized = (DoubleUtil.AreClose(size.Width, uniformOrAverageContainerSize) && (this.IsPixelBased || DoubleUtil.AreClose(desiredSize.Width, uniformOrAverageContainerPixelSize)));
							}
							else
							{
								computedAreContainersUniformlySized = (DoubleUtil.AreClose(size.Height, uniformOrAverageContainerSize) && (this.IsPixelBased || DoubleUtil.AreClose(desiredSize.Height, uniformOrAverageContainerPixelSize)));
							}
							if (!computedAreContainersUniformlySized)
							{
								i = -1;
							}
						}
						else if (this.IsPixelBased)
						{
							if (flag)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetContainerSize, new object[]
								{
									itemContainerGenerator.IndexFromContainer(uielement),
									size
								});
							}
							itemStorageProvider.StoreItemValue(itemContainerGenerator.ItemFromContainer(uielement), VirtualizingStackPanel.ContainerSizeProperty, size);
						}
						else
						{
							if (flag)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetContainerSize, new object[]
								{
									itemContainerGenerator.IndexFromContainer(uielement),
									size,
									desiredSize
								});
							}
							VirtualizingStackPanel.ContainerSizeDual value = new VirtualizingStackPanel.ContainerSizeDual(desiredSize, size);
							itemStorageProvider.StoreItemValue(itemContainerGenerator.ItemFromContainer(uielement), VirtualizingStackPanel.ContainerSizeDualProperty, value);
						}
					}
				}
				if (evaluateAreContainersUniformlySized)
				{
					areContainersUniformlySized = computedAreContainersUniformlySized;
					this.SetAreContainersUniformlySized(parentItemStorageProvider, parentItem, areContainersUniformlySized);
				}
			}
			if (!computedAreContainersUniformlySized)
			{
				Size size2 = default(Size);
				Size size3 = default(Size);
				double num = 0.0;
				double num2 = 0.0;
				int num3 = 0;
				for (int j = 0; j < itemCount; j++)
				{
					object obj2;
					if (this.IsPixelBased)
					{
						obj2 = itemStorageProvider.ReadItemValue(items[j], VirtualizingStackPanel.ContainerSizeProperty);
						if (obj2 != null)
						{
							size2 = (Size)obj2;
							size3 = size2;
						}
					}
					else
					{
						obj2 = itemStorageProvider.ReadItemValue(items[j], VirtualizingStackPanel.ContainerSizeDualProperty);
						if (obj2 != null)
						{
							VirtualizingStackPanel.ContainerSizeDual containerSizeDual = (VirtualizingStackPanel.ContainerSizeDual)obj2;
							size2 = containerSizeDual.ItemSize;
							size3 = containerSizeDual.PixelSize;
						}
					}
					if (obj2 != null)
					{
						if (isHorizontal)
						{
							num += size2.Width;
							num2 += size3.Width;
							num3++;
						}
						else
						{
							num += size2.Height;
							num2 += size3.Height;
							num3++;
						}
					}
				}
				if (num3 > 0)
				{
					uniformOrAverageContainerPixelSize = num2 / (double)num3;
					if (base.UseLayoutRounding)
					{
						DpiScale dpi = base.GetDpi();
						double num4 = isHorizontal ? dpi.DpiScaleX : dpi.DpiScaleY;
						uniformOrAverageContainerPixelSize = UIElement.RoundLayoutValue(Math.Max(uniformOrAverageContainerPixelSize, num4), num4);
					}
					if (this.IsPixelBased)
					{
						uniformOrAverageContainerSize = uniformOrAverageContainerPixelSize;
					}
					else
					{
						uniformOrAverageContainerSize = Math.Round(num / (double)num3);
					}
					if (this.SetUniformOrAverageContainerSize(parentItemStorageProvider, parentItem, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize))
					{
						hasAverageContainerSizeChanged = true;
					}
				}
			}
			else
			{
				uniformOrAverageContainerSize = computedUniformOrAverageContainerSize;
				uniformOrAverageContainerPixelSize = computedUniformOrAverageContainerPixelSize;
			}
			if (flag)
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SyncAveSize, new object[]
				{
					uniformOrAverageContainerSize,
					uniformOrAverageContainerPixelSize,
					areContainersUniformlySized,
					hasAverageContainerSizeChanged
				});
			}
		}

		// Token: 0x0600780A RID: 30730 RVA: 0x002FB93C File Offset: 0x002FA93C
		private void ClearAsyncOperations()
		{
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			if (isVSP45Compat)
			{
				DispatcherOperation value = VirtualizingStackPanel.MeasureCachesOperationField.GetValue(this);
				if (value != null)
				{
					value.Abort();
					VirtualizingStackPanel.MeasureCachesOperationField.ClearValue(this);
				}
			}
			else
			{
				this.ClearMeasureCachesState();
			}
			DispatcherOperation value2 = VirtualizingStackPanel.AnchorOperationField.GetValue(this);
			if (value2 != null)
			{
				if (isVSP45Compat)
				{
					value2.Abort();
					VirtualizingStackPanel.AnchorOperationField.ClearValue(this);
				}
				else
				{
					this.ClearAnchorInformation(true);
				}
			}
			DispatcherOperation value3 = VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.GetValue(this);
			if (value3 != null)
			{
				value3.Abort();
				VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.ClearValue(this);
			}
			DispatcherOperation value4 = VirtualizingStackPanel.ClearIsScrollActiveOperationField.GetValue(this);
			if (value4 != null)
			{
				if (isVSP45Compat)
				{
					value4.Abort();
					VirtualizingStackPanel.ClearIsScrollActiveOperationField.ClearValue(this);
					return;
				}
				value4.Abort();
				this.ClearIsScrollActive();
			}
		}

		// Token: 0x0600780B RID: 30731 RVA: 0x002FBA00 File Offset: 0x002FAA00
		private bool GetAreContainersUniformlySized(IContainItemStorage itemStorageProvider, object item)
		{
			if (item == this)
			{
				if (this.AreContainersUniformlySized != null)
				{
					return this.AreContainersUniformlySized.Value;
				}
			}
			else
			{
				object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.AreContainersUniformlySizedProperty);
				if (obj != null)
				{
					return (bool)obj;
				}
			}
			return true;
		}

		// Token: 0x0600780C RID: 30732 RVA: 0x002FBA48 File Offset: 0x002FAA48
		private void SetAreContainersUniformlySized(IContainItemStorage itemStorageProvider, object item, bool value)
		{
			if (item == this)
			{
				this.AreContainersUniformlySized = new bool?(value);
				return;
			}
			itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.AreContainersUniformlySizedProperty, value);
		}

		// Token: 0x0600780D RID: 30733 RVA: 0x002FBA70 File Offset: 0x002FAA70
		private double GetUniformOrAverageContainerSize(IContainItemStorage itemStorageProvider, object item)
		{
			double result;
			double num;
			this.GetUniformOrAverageContainerSize(itemStorageProvider, item, this.IsPixelBased || VirtualizingStackPanel.IsVSP45Compat, out result, out num);
			return result;
		}

		// Token: 0x0600780E RID: 30734 RVA: 0x002FBA9C File Offset: 0x002FAA9C
		private void GetUniformOrAverageContainerSize(IContainItemStorage itemStorageProvider, object item, bool isSingleValue, out double uniformOrAverageContainerSize, out double uniformOrAverageContainerPixelSize)
		{
			bool flag;
			this.GetUniformOrAverageContainerSize(itemStorageProvider, item, isSingleValue, out uniformOrAverageContainerSize, out uniformOrAverageContainerPixelSize, out flag);
		}

		// Token: 0x0600780F RID: 30735 RVA: 0x002FBAB8 File Offset: 0x002FAAB8
		private void GetUniformOrAverageContainerSize(IContainItemStorage itemStorageProvider, object item, bool isSingleValue, out double uniformOrAverageContainerSize, out double uniformOrAverageContainerPixelSize, out bool hasUniformOrAverageContainerSizeBeenSet)
		{
			if (item == this)
			{
				if (this.UniformOrAverageContainerSize != null)
				{
					hasUniformOrAverageContainerSizeBeenSet = true;
					uniformOrAverageContainerSize = this.UniformOrAverageContainerSize.Value;
					if (isSingleValue)
					{
						uniformOrAverageContainerPixelSize = uniformOrAverageContainerSize;
						return;
					}
					uniformOrAverageContainerPixelSize = this.UniformOrAverageContainerPixelSize.Value;
					return;
				}
			}
			else if (isSingleValue)
			{
				object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeProperty);
				if (obj != null)
				{
					hasUniformOrAverageContainerSizeBeenSet = true;
					uniformOrAverageContainerSize = (double)obj;
					uniformOrAverageContainerPixelSize = uniformOrAverageContainerSize;
					return;
				}
			}
			else
			{
				object obj2 = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeDualProperty);
				if (obj2 != null)
				{
					VirtualizingStackPanel.UniformOrAverageContainerSizeDual uniformOrAverageContainerSizeDual = (VirtualizingStackPanel.UniformOrAverageContainerSizeDual)obj2;
					hasUniformOrAverageContainerSizeBeenSet = true;
					uniformOrAverageContainerSize = uniformOrAverageContainerSizeDual.ItemSize;
					uniformOrAverageContainerPixelSize = uniformOrAverageContainerSizeDual.PixelSize;
					return;
				}
			}
			hasUniformOrAverageContainerSizeBeenSet = false;
			uniformOrAverageContainerPixelSize = 16.0;
			uniformOrAverageContainerSize = (this.IsPixelBased ? uniformOrAverageContainerPixelSize : 1.0);
		}

		// Token: 0x06007810 RID: 30736 RVA: 0x002FBB90 File Offset: 0x002FAB90
		private bool SetUniformOrAverageContainerSize(IContainItemStorage itemStorageProvider, object item, double value, double pixelValue)
		{
			bool result = false;
			if (DoubleUtil.GreaterThan(value, 0.0))
			{
				if (item == this)
				{
					double? uniformOrAverageContainerSize = this.UniformOrAverageContainerSize;
					if (!(uniformOrAverageContainerSize.GetValueOrDefault() == value & uniformOrAverageContainerSize != null))
					{
						this.UniformOrAverageContainerSize = new double?(value);
						this.UniformOrAverageContainerPixelSize = new double?(pixelValue);
						result = true;
					}
				}
				else if (this.IsPixelBased || VirtualizingStackPanel.IsVSP45Compat)
				{
					object objA = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeProperty);
					itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeProperty, value);
					result = !object.Equals(objA, value);
				}
				else
				{
					VirtualizingStackPanel.UniformOrAverageContainerSizeDual uniformOrAverageContainerSizeDual = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeDualProperty) as VirtualizingStackPanel.UniformOrAverageContainerSizeDual;
					VirtualizingStackPanel.UniformOrAverageContainerSizeDual value2 = new VirtualizingStackPanel.UniformOrAverageContainerSizeDual(pixelValue, value);
					itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeDualProperty, value2);
					result = (uniformOrAverageContainerSizeDual == null || uniformOrAverageContainerSizeDual.ItemSize != value);
				}
			}
			return result;
		}

		// Token: 0x06007811 RID: 30737 RVA: 0x002FBC70 File Offset: 0x002FAC70
		private void MeasureExistingChildBeyondExtendedViewport(ref IItemContainerGenerator generator, ref IContainItemStorage itemStorageProvider, ref IContainItemStorage parentItemStorageProvider, ref object parentItem, ref bool hasUniformOrAverageContainerSizeBeenSet, ref double computedUniformOrAverageContainerSize, ref double computedUniformOrAverageContainerPixelSize, ref bool computedAreContainersUniformlySized, ref bool hasAnyContainerSpanChanged, ref IList items, ref IList children, ref int childIndex, ref bool visualOrderChanged, ref bool isHorizontal, ref Size childConstraint, ref bool foundFirstItemInViewport, ref double firstItemInViewportOffset, ref bool mustDisableVirtualization, ref bool hasVirtualizingChildren, ref bool hasBringIntoViewContainerBeenMeasured, ref long scrollGeneration)
		{
			object obj = ((ItemContainerGenerator)generator).ItemFromContainer((UIElement)children[childIndex]);
			Rect rect = default(Rect);
			VirtualizationCacheLength virtualizationCacheLength = default(VirtualizationCacheLength);
			VirtualizationCacheLengthUnit virtualizationCacheLengthUnit = VirtualizationCacheLengthUnit.Pixel;
			Size size = default(Size);
			Size size2 = default(Size);
			Size size3 = default(Size);
			Size size4 = default(Size);
			Size size5 = default(Size);
			Size size6 = default(Size);
			Size size7 = default(Size);
			Size size8 = default(Size);
			bool isBeforeFirstItem = childIndex < this._firstItemInExtendedViewportChildIndex;
			bool isAfterFirstItem = childIndex > this._firstItemInExtendedViewportChildIndex;
			bool isAfterLastItem = childIndex > this._firstItemInExtendedViewportChildIndex + this._actualItemsInExtendedViewportCount;
			bool skipActualMeasure = false;
			bool skipGeneration = true;
			this.MeasureChild(ref generator, ref itemStorageProvider, ref parentItemStorageProvider, ref parentItem, ref hasUniformOrAverageContainerSizeBeenSet, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref hasAnyContainerSpanChanged, ref items, ref obj, ref children, ref childIndex, ref visualOrderChanged, ref isHorizontal, ref childConstraint, ref rect, ref virtualizationCacheLength, ref virtualizationCacheLengthUnit, ref scrollGeneration, ref foundFirstItemInViewport, ref firstItemInViewportOffset, ref size, ref size2, ref size3, ref size4, ref size5, ref size6, ref size7, ref size8, ref mustDisableVirtualization, isBeforeFirstItem, isAfterFirstItem, isAfterLastItem, skipActualMeasure, skipGeneration, ref hasBringIntoViewContainerBeenMeasured, ref hasVirtualizingChildren);
		}

		// Token: 0x06007812 RID: 30738 RVA: 0x002FBD70 File Offset: 0x002FAD70
		private void MeasureChild(ref IItemContainerGenerator generator, ref IContainItemStorage itemStorageProvider, ref IContainItemStorage parentItemStorageProvider, ref object parentItem, ref bool hasUniformOrAverageContainerSizeBeenSet, ref double computedUniformOrAverageContainerSize, ref double computedUniformOrAverageContainerPixelSize, ref bool computedAreContainersUniformlySized, ref bool hasAnyContainerSpanChanged, ref IList items, ref object item, ref IList children, ref int childIndex, ref bool visualOrderChanged, ref bool isHorizontal, ref Size childConstraint, ref Rect viewport, ref VirtualizationCacheLength cacheSize, ref VirtualizationCacheLengthUnit cacheUnit, ref long scrollGeneration, ref bool foundFirstItemInViewport, ref double firstItemInViewportOffset, ref Size stackPixelSize, ref Size stackPixelSizeInViewport, ref Size stackPixelSizeInCacheBeforeViewport, ref Size stackPixelSizeInCacheAfterViewport, ref Size stackLogicalSize, ref Size stackLogicalSizeInViewport, ref Size stackLogicalSizeInCacheBeforeViewport, ref Size stackLogicalSizeInCacheAfterViewport, ref bool mustDisableVirtualization, bool isBeforeFirstItem, bool isAfterFirstItem, bool isAfterLastItem, bool skipActualMeasure, bool skipGeneration, ref bool hasBringIntoViewContainerBeenMeasured, ref bool hasVirtualizingChildren)
		{
			Rect empty = Rect.Empty;
			VirtualizationCacheLength childCacheSize = new VirtualizationCacheLength(0.0);
			VirtualizationCacheLengthUnit childCacheUnit = VirtualizationCacheLengthUnit.Pixel;
			Size childDesiredSize = default(Size);
			UIElement uielement;
			if (!skipActualMeasure && !skipGeneration)
			{
				bool newlyRealized;
				uielement = (generator.GenerateNext(out newlyRealized) as UIElement);
				ItemContainerGenerator itemContainerGenerator;
				if (uielement == null && (itemContainerGenerator = (generator as ItemContainerGenerator)) != null)
				{
					itemContainerGenerator.Verify();
				}
				visualOrderChanged |= this.AddContainerFromGenerator(childIndex, uielement, newlyRealized, isBeforeFirstItem);
			}
			else
			{
				uielement = (UIElement)children[childIndex];
			}
			hasBringIntoViewContainerBeenMeasured |= (uielement == this._bringIntoViewContainer);
			bool flag = isHorizontal;
			IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement, ref flag);
			this.SetViewportForChild(isHorizontal, itemStorageProvider, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, mustDisableVirtualization, uielement, virtualizingChild, item, isBeforeFirstItem, isAfterFirstItem, firstItemInViewportOffset, viewport, cacheSize, cacheUnit, scrollGeneration, stackPixelSize, stackPixelSizeInViewport, stackPixelSizeInCacheBeforeViewport, stackPixelSizeInCacheAfterViewport, stackLogicalSize, stackLogicalSizeInViewport, stackLogicalSizeInCacheBeforeViewport, stackLogicalSizeInCacheAfterViewport, out empty, ref childCacheSize, ref childCacheUnit);
			if (!skipActualMeasure)
			{
				uielement.Measure(childConstraint);
			}
			childDesiredSize = uielement.DesiredSize;
			if (virtualizingChild != null)
			{
				virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement, ref flag);
				mustDisableVirtualization |= ((virtualizingChild != null && virtualizingChild.MustDisableVirtualization) || flag != isHorizontal);
			}
			Size size;
			Size childPixelSizeInViewport;
			Size childPixelSizeInCacheBeforeViewport;
			Size childPixelSizeInCacheAfterViewport;
			Size size2;
			Size childLogicalSizeInViewport;
			Size childLogicalSizeInCacheBeforeViewport;
			Size childLogicalSizeInCacheAfterViewport;
			if (VirtualizingStackPanel.IsVSP45Compat)
			{
				this.GetSizesForChild(isHorizontal, flag, isBeforeFirstItem, isAfterLastItem, virtualizingChild, childDesiredSize, empty, childCacheSize, childCacheUnit, out size, out childPixelSizeInViewport, out childPixelSizeInCacheBeforeViewport, out childPixelSizeInCacheAfterViewport, out size2, out childLogicalSizeInViewport, out childLogicalSizeInCacheBeforeViewport, out childLogicalSizeInCacheAfterViewport);
			}
			else
			{
				this.GetSizesForChildWithInset(isHorizontal, flag, isBeforeFirstItem, isAfterLastItem, virtualizingChild, childDesiredSize, empty, childCacheSize, childCacheUnit, out size, out childPixelSizeInViewport, out childPixelSizeInCacheBeforeViewport, out childPixelSizeInCacheAfterViewport, out size2, out childLogicalSizeInViewport, out childLogicalSizeInCacheBeforeViewport, out childLogicalSizeInCacheAfterViewport);
			}
			this.UpdateStackSizes(isHorizontal, foundFirstItemInViewport, size, childPixelSizeInViewport, childPixelSizeInCacheBeforeViewport, childPixelSizeInCacheAfterViewport, size2, childLogicalSizeInViewport, childLogicalSizeInCacheBeforeViewport, childLogicalSizeInCacheAfterViewport, ref stackPixelSize, ref stackPixelSizeInViewport, ref stackPixelSizeInCacheBeforeViewport, ref stackPixelSizeInCacheAfterViewport, ref stackLogicalSize, ref stackLogicalSizeInViewport, ref stackLogicalSizeInCacheBeforeViewport, ref stackLogicalSizeInCacheAfterViewport);
			if (virtualizingChild != null)
			{
				hasVirtualizingChildren = true;
			}
			if (VirtualizingPanel.GetShouldCacheContainerSize(uielement))
			{
				if (VirtualizingStackPanel.IsVSP45Compat)
				{
					this.SetContainerSizeForItem(itemStorageProvider, parentItemStorageProvider, parentItem, item, this.IsPixelBased ? size : size2, isHorizontal, ref hasUniformOrAverageContainerSizeBeenSet, ref computedUniformOrAverageContainerSize, ref computedAreContainersUniformlySized);
					return;
				}
				this.SetContainerSizeForItem(itemStorageProvider, parentItemStorageProvider, parentItem, item, this.IsPixelBased ? size : size2, size, isHorizontal, hasVirtualizingChildren, ref hasUniformOrAverageContainerSizeBeenSet, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref hasAnyContainerSpanChanged);
			}
		}

		// Token: 0x06007813 RID: 30739 RVA: 0x002FBFBC File Offset: 0x002FAFBC
		private void ArrangeFirstItemInExtendedViewport(bool isHorizontal, UIElement child, Size childDesiredSize, double arrangeLength, ref Rect rcChild, ref Size previousChildSize, ref Point previousChildOffset, ref int previousChildItemIndex)
		{
			rcChild.X = 0.0;
			rcChild.Y = 0.0;
			if (this.IsScrolling)
			{
				if (!this.IsPixelBased)
				{
					if (isHorizontal)
					{
						rcChild.X = -1.0 * ((VirtualizingStackPanel.IsVSP45Compat || !this.IsVirtualizing || !this.HasVirtualizingChildren) ? this._previousStackPixelSizeInCacheBeforeViewport.Width : this._pixelDistanceToViewport);
						rcChild.Y = -1.0 * this._scrollData._computedOffset.Y;
					}
					else
					{
						rcChild.Y = -1.0 * ((VirtualizingStackPanel.IsVSP45Compat || !this.IsVirtualizing || !this.HasVirtualizingChildren) ? this._previousStackPixelSizeInCacheBeforeViewport.Height : this._pixelDistanceToViewport);
						rcChild.X = -1.0 * this._scrollData._computedOffset.X;
					}
				}
				else
				{
					rcChild.X = -1.0 * this._scrollData._computedOffset.X;
					rcChild.Y = -1.0 * this._scrollData._computedOffset.Y;
				}
			}
			if (this.IsVirtualizing)
			{
				if (this.IsPixelBased)
				{
					if (isHorizontal)
					{
						rcChild.X += this._firstItemInExtendedViewportOffset;
					}
					else
					{
						rcChild.Y += this._firstItemInExtendedViewportOffset;
					}
				}
				else if (!VirtualizingStackPanel.IsVSP45Compat && (!this.IsScrolling || this.HasVirtualizingChildren))
				{
					if (isHorizontal)
					{
						rcChild.X += this._pixelDistanceToFirstContainerInExtendedViewport;
					}
					else
					{
						rcChild.Y += this._pixelDistanceToFirstContainerInExtendedViewport;
					}
				}
			}
			bool flag = isHorizontal;
			IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(child, ref flag);
			if (isHorizontal)
			{
				rcChild.Width = childDesiredSize.Width;
				rcChild.Height = Math.Max(arrangeLength, childDesiredSize.Height);
				previousChildSize = childDesiredSize;
				if (!this.IsPixelBased && virtualizingChild != null && VirtualizingStackPanel.IsVSP45Compat)
				{
					HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizingChild.ItemDesiredSizes;
					previousChildSize.Width = itemDesiredSizes.PixelSizeInViewport.Width;
					if (flag == isHorizontal)
					{
						previousChildSize.Width += itemDesiredSizes.PixelSizeBeforeViewport.Width + itemDesiredSizes.PixelSizeAfterViewport.Width;
					}
					RelativeHeaderPosition relativeHeaderPosition = RelativeHeaderPosition.Top;
					Size pixelSize = virtualizingChild.HeaderDesiredSizes.PixelSize;
					if (relativeHeaderPosition == RelativeHeaderPosition.Left || relativeHeaderPosition == RelativeHeaderPosition.Right)
					{
						previousChildSize.Width += pixelSize.Width;
					}
					else
					{
						previousChildSize.Width = Math.Max(previousChildSize.Width, pixelSize.Width);
					}
				}
			}
			else
			{
				rcChild.Height = childDesiredSize.Height;
				rcChild.Width = Math.Max(arrangeLength, childDesiredSize.Width);
				previousChildSize = childDesiredSize;
				if (!this.IsPixelBased && virtualizingChild != null && VirtualizingStackPanel.IsVSP45Compat)
				{
					HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes2 = virtualizingChild.ItemDesiredSizes;
					previousChildSize.Height = itemDesiredSizes2.PixelSizeInViewport.Height;
					if (flag == isHorizontal)
					{
						previousChildSize.Height += itemDesiredSizes2.PixelSizeBeforeViewport.Height + itemDesiredSizes2.PixelSizeAfterViewport.Height;
					}
					RelativeHeaderPosition relativeHeaderPosition2 = RelativeHeaderPosition.Top;
					Size pixelSize2 = virtualizingChild.HeaderDesiredSizes.PixelSize;
					if (relativeHeaderPosition2 == RelativeHeaderPosition.Top || relativeHeaderPosition2 == RelativeHeaderPosition.Bottom)
					{
						previousChildSize.Height += pixelSize2.Height;
					}
					else
					{
						previousChildSize.Height = Math.Max(previousChildSize.Height, pixelSize2.Height);
					}
				}
			}
			previousChildItemIndex = this._firstItemInExtendedViewportIndex;
			previousChildOffset = rcChild.Location;
			child.Arrange(rcChild);
		}

		// Token: 0x06007814 RID: 30740 RVA: 0x002FC38C File Offset: 0x002FB38C
		private void ArrangeOtherItemsInExtendedViewport(bool isHorizontal, UIElement child, Size childDesiredSize, double arrangeLength, int index, ref Rect rcChild, ref Size previousChildSize, ref Point previousChildOffset, ref int previousChildItemIndex)
		{
			if (isHorizontal)
			{
				rcChild.X += previousChildSize.Width;
				rcChild.Width = childDesiredSize.Width;
				rcChild.Height = Math.Max(arrangeLength, childDesiredSize.Height);
			}
			else
			{
				rcChild.Y += previousChildSize.Height;
				rcChild.Height = childDesiredSize.Height;
				rcChild.Width = Math.Max(arrangeLength, childDesiredSize.Width);
			}
			previousChildSize = childDesiredSize;
			previousChildItemIndex = this._firstItemInExtendedViewportIndex + (index - this._firstItemInExtendedViewportChildIndex);
			previousChildOffset = rcChild.Location;
			child.Arrange(rcChild);
		}

		// Token: 0x06007815 RID: 30741 RVA: 0x002FC444 File Offset: 0x002FB444
		private void ArrangeItemsBeyondTheExtendedViewport(bool isHorizontal, UIElement child, Size childDesiredSize, double arrangeLength, IList items, IItemContainerGenerator generator, IContainItemStorage itemStorageProvider, bool areContainersUniformlySized, double uniformOrAverageContainerSize, bool beforeExtendedViewport, ref Rect rcChild, ref Size previousChildSize, ref Point previousChildOffset, ref int previousChildItemIndex)
		{
			if (isHorizontal)
			{
				rcChild.Width = childDesiredSize.Width;
				rcChild.Height = Math.Max(arrangeLength, childDesiredSize.Height);
				if (this.IsPixelBased)
				{
					int num = ((ItemContainerGenerator)generator).IndexFromContainer(child, true);
					if (beforeExtendedViewport)
					{
						double num2;
						if (previousChildItemIndex == -1)
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, num, out num2);
						}
						else
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, num, previousChildItemIndex - num, out num2);
						}
						rcChild.X = previousChildOffset.X - num2;
						rcChild.Y = previousChildOffset.Y;
					}
					else
					{
						double num2;
						if (previousChildItemIndex == -1)
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, num, out num2);
						}
						else
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, previousChildItemIndex, num - previousChildItemIndex, out num2);
						}
						rcChild.X = previousChildOffset.X + num2;
						rcChild.Y = previousChildOffset.Y;
					}
					previousChildItemIndex = num;
				}
				else if (beforeExtendedViewport)
				{
					rcChild.X -= childDesiredSize.Width;
				}
				else
				{
					rcChild.X += previousChildSize.Width;
				}
			}
			else
			{
				rcChild.Height = childDesiredSize.Height;
				rcChild.Width = Math.Max(arrangeLength, childDesiredSize.Width);
				if (this.IsPixelBased)
				{
					int num3 = ((ItemContainerGenerator)generator).IndexFromContainer(child, true);
					if (beforeExtendedViewport)
					{
						double num4;
						if (previousChildItemIndex == -1)
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, num3, out num4);
						}
						else
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, num3, previousChildItemIndex - num3, out num4);
						}
						rcChild.Y = previousChildOffset.Y - num4;
						rcChild.X = previousChildOffset.X;
					}
					else
					{
						double num4;
						if (previousChildItemIndex == -1)
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, num3, out num4);
						}
						else
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, previousChildItemIndex, num3 - previousChildItemIndex, out num4);
						}
						rcChild.Y = previousChildOffset.Y + num4;
						rcChild.X = previousChildOffset.X;
					}
					previousChildItemIndex = num3;
				}
				else if (beforeExtendedViewport)
				{
					rcChild.Y -= childDesiredSize.Height;
				}
				else
				{
					rcChild.Y += previousChildSize.Height;
				}
			}
			previousChildSize = childDesiredSize;
			previousChildOffset = rcChild.Location;
			child.Arrange(rcChild);
		}

		// Token: 0x06007816 RID: 30742 RVA: 0x002FC6BB File Offset: 0x002FB6BB
		private void InsertNewContainer(int childIndex, UIElement container)
		{
			this.InsertContainer(childIndex, container, false);
		}

		// Token: 0x06007817 RID: 30743 RVA: 0x002FC6C7 File Offset: 0x002FB6C7
		private bool InsertRecycledContainer(int childIndex, UIElement container)
		{
			return this.InsertContainer(childIndex, container, true);
		}

		// Token: 0x06007818 RID: 30744 RVA: 0x002FC6D4 File Offset: 0x002FB6D4
		private bool InsertContainer(int childIndex, UIElement container, bool isRecycled)
		{
			bool result = false;
			UIElementCollection internalChildren = base.InternalChildren;
			int num;
			if (childIndex > 0)
			{
				num = this.ChildIndexFromRealizedIndex(childIndex - 1);
				num++;
			}
			else
			{
				num = this.ChildIndexFromRealizedIndex(childIndex);
			}
			if (!isRecycled || num >= internalChildren.Count || internalChildren[num] != container)
			{
				if (num < internalChildren.Count)
				{
					int index = num;
					if (isRecycled && container.InternalVisualParent != null)
					{
						internalChildren.MoveVisualChild(container, internalChildren[num]);
						result = true;
					}
					else
					{
						VirtualizingPanel.InsertInternalChild(internalChildren, index, container);
					}
				}
				else if (isRecycled && container.InternalVisualParent != null)
				{
					internalChildren.MoveVisualChild(container, null);
					result = true;
				}
				else
				{
					VirtualizingPanel.AddInternalChild(internalChildren, container);
				}
			}
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				if (this.ItemsChangedDuringMeasure)
				{
					this._realizedChildren = null;
				}
				if (this._realizedChildren != null)
				{
					this._realizedChildren.Insert(childIndex, container);
				}
				else
				{
					this.EnsureRealizedChildren();
				}
			}
			base.Generator.PrepareItemContainer(container);
			return result;
		}

		// Token: 0x06007819 RID: 30745 RVA: 0x002FC7B8 File Offset: 0x002FB7B8
		private void EnsureCleanupOperation(bool delay)
		{
			if (delay)
			{
				bool flag = true;
				if (this._cleanupOperation != null)
				{
					flag = this._cleanupOperation.Abort();
					if (flag)
					{
						this._cleanupOperation = null;
					}
				}
				if (flag && this._cleanupDelay == null)
				{
					this._cleanupDelay = new DispatcherTimer();
					this._cleanupDelay.Tick += this.OnDelayCleanup;
					this._cleanupDelay.Interval = TimeSpan.FromMilliseconds(500.0);
					this._cleanupDelay.Start();
					return;
				}
			}
			else if (this._cleanupOperation == null && this._cleanupDelay == null)
			{
				this._cleanupOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnCleanUp), null);
			}
		}

		// Token: 0x0600781A RID: 30746 RVA: 0x002FC86C File Offset: 0x002FB86C
		private bool PreviousChildIsGenerated(int childIndex)
		{
			GeneratorPosition position = new GeneratorPosition(childIndex, 0);
			position = base.Generator.GeneratorPositionFromIndex(base.Generator.IndexFromGeneratorPosition(position) - 1);
			return position.Offset == 0 && position.Index >= 0;
		}

		// Token: 0x0600781B RID: 30747 RVA: 0x002FC8B4 File Offset: 0x002FB8B4
		private bool AddContainerFromGenerator(int childIndex, UIElement child, bool newlyRealized, bool isBeforeViewport)
		{
			bool result = false;
			if (!newlyRealized)
			{
				if (this.InRecyclingMode)
				{
					IList realizedChildren = this.RealizedChildren;
					if (childIndex < 0 || childIndex >= realizedChildren.Count || realizedChildren[childIndex] != child)
					{
						result = this.InsertRecycledContainer(childIndex, child);
					}
				}
			}
			else
			{
				this.InsertNewContainer(childIndex, child);
			}
			return result;
		}

		// Token: 0x0600781C RID: 30748 RVA: 0x002FC900 File Offset: 0x002FB900
		private void OnItemsRemove(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.Position, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x0600781D RID: 30749 RVA: 0x002FC91C File Offset: 0x002FB91C
		private void OnItemsReplace(ItemsChangedEventArgs args)
		{
			if (args.ItemUICount > 0)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				using (base.Generator.StartAt(args.Position, GeneratorDirection.Forward, true))
				{
					for (int i = 0; i < args.ItemUICount; i++)
					{
						int index = args.Position.Index + i;
						bool flag;
						UIElement uielement = base.Generator.GenerateNext(out flag) as UIElement;
						internalChildren.SetInternal(index, uielement);
						base.Generator.PrepareItemContainer(uielement);
					}
				}
			}
		}

		// Token: 0x0600781E RID: 30750 RVA: 0x002FC9B8 File Offset: 0x002FB9B8
		private void OnItemsMove(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.OldPosition, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x0600781F RID: 30751 RVA: 0x002FC9D4 File Offset: 0x002FB9D4
		private void RemoveChildRange(GeneratorPosition position, int itemCount, int itemUICount)
		{
			if (base.IsItemsHost)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				int num = position.Index;
				if (position.Offset > 0)
				{
					num++;
				}
				if (num < internalChildren.Count && itemUICount > 0)
				{
					VirtualizingPanel.RemoveInternalChildRange(internalChildren, num, itemUICount);
					if (this.IsVirtualizing && this.InRecyclingMode)
					{
						this._realizedChildren.RemoveRange(num, itemUICount);
					}
				}
			}
		}

		// Token: 0x06007820 RID: 30752 RVA: 0x002FCA3A File Offset: 0x002FBA3A
		private void CleanupContainers(int firstItemInExtendedViewportIndex, int itemsInExtendedViewportCount, ItemsControl itemsControl)
		{
			this.CleanupContainers(firstItemInExtendedViewportIndex, itemsInExtendedViewportCount, itemsControl, false, 0);
		}

		// Token: 0x06007821 RID: 30753 RVA: 0x002FCA48 File Offset: 0x002FBA48
		private bool CleanupContainers(int firstItemInExtendedViewportIndex, int itemsInExtendedViewportCount, ItemsControl itemsControl, bool timeBound, int startTickCount)
		{
			IList realizedChildren = this.RealizedChildren;
			if (realizedChildren.Count == 0)
			{
				return false;
			}
			int num = -1;
			int num2 = 0;
			int num3 = -1;
			bool flag = false;
			bool isVirtualizing = this.IsVirtualizing;
			bool result = false;
			for (int i = 0; i < realizedChildren.Count; i++)
			{
				if (timeBound && Environment.TickCount - startTickCount > 50 && num2 > 0)
				{
					result = true;
					break;
				}
				UIElement uielement = (UIElement)realizedChildren[i];
				int num4 = num3;
				num3 = this.GetGeneratedIndex(i);
				object item = itemsControl.ItemContainerGenerator.ItemFromContainer(uielement);
				if (num3 - num4 != 1)
				{
					flag = true;
				}
				if (flag)
				{
					if (num >= 0 && num2 > 0)
					{
						this.CleanupRange(realizedChildren, base.Generator, num, num2);
						i -= num2;
						num2 = 0;
						num = -1;
					}
					flag = false;
				}
				if ((num3 < firstItemInExtendedViewportIndex || num3 >= firstItemInExtendedViewportIndex + itemsInExtendedViewportCount) && num3 >= 0 && !((IGeneratorHost)itemsControl).IsItemItsOwnContainer(item) && !uielement.IsKeyboardFocusWithin && uielement != this._bringIntoViewContainer && this.NotifyCleanupItem(uielement, itemsControl) && VirtualizingPanel.GetIsContainerVirtualizable(uielement))
				{
					if (num == -1)
					{
						num = i;
					}
					num2++;
				}
				else
				{
					flag = true;
				}
			}
			if (num >= 0 && num2 > 0)
			{
				this.CleanupRange(realizedChildren, base.Generator, num, num2);
			}
			return result;
		}

		// Token: 0x06007822 RID: 30754 RVA: 0x002FCB78 File Offset: 0x002FBB78
		private void EnsureRealizedChildren()
		{
			if (this._realizedChildren == null)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				this._realizedChildren = new List<UIElement>(internalChildren.Count);
				for (int i = 0; i < internalChildren.Count; i++)
				{
					this._realizedChildren.Add(internalChildren[i]);
				}
			}
		}

		// Token: 0x06007823 RID: 30755 RVA: 0x002FCBC8 File Offset: 0x002FBBC8
		[Conditional("DEBUG")]
		private void debug_VerifyRealizedChildren()
		{
			ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			if (itemContainerGenerator != null && itemsOwner != null && !itemsOwner.IsGrouping)
			{
				foreach (object obj in base.InternalChildren)
				{
					UIElement container = (UIElement)obj;
					int num = itemContainerGenerator.IndexFromContainer(container);
					if (num != -1)
					{
						base.Generator.GeneratorPositionFromIndex(num);
					}
				}
			}
		}

		// Token: 0x06007824 RID: 30756 RVA: 0x002FCC5C File Offset: 0x002FBC5C
		[Conditional("DEBUG")]
		private void debug_AssertRealizedChildrenEqualVisualChildren()
		{
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				for (int i = 0; i < internalChildren.Count; i++)
				{
				}
			}
		}

		// Token: 0x06007825 RID: 30757 RVA: 0x002FCC94 File Offset: 0x002FBC94
		private int ChildIndexFromRealizedIndex(int realizedChildIndex)
		{
			if (this.IsVirtualizing && this.InRecyclingMode && realizedChildIndex < this._realizedChildren.Count)
			{
				UIElement uielement = this._realizedChildren[realizedChildIndex];
				UIElementCollection internalChildren = base.InternalChildren;
				for (int i = realizedChildIndex; i < internalChildren.Count; i++)
				{
					if (internalChildren[i] == uielement)
					{
						return i;
					}
				}
			}
			return realizedChildIndex;
		}

		// Token: 0x06007826 RID: 30758 RVA: 0x002FCCF4 File Offset: 0x002FBCF4
		private void DisconnectRecycledContainers()
		{
			int num = 0;
			UIElement uielement = (this._realizedChildren.Count > 0) ? this._realizedChildren[0] : null;
			UIElementCollection internalChildren = base.InternalChildren;
			for (int i = 0; i < internalChildren.Count; i++)
			{
				UIElement uielement2 = internalChildren[i];
				if (uielement2 == uielement)
				{
					num++;
					if (num < this._realizedChildren.Count)
					{
						uielement = this._realizedChildren[num];
					}
					else
					{
						uielement = null;
					}
				}
				else
				{
					internalChildren.RemoveNoVerify(uielement2);
					i--;
				}
			}
		}

		// Token: 0x06007827 RID: 30759 RVA: 0x002FCD7C File Offset: 0x002FBD7C
		private GeneratorPosition IndexToGeneratorPositionForStart(int index, out int childIndex)
		{
			IItemContainerGenerator generator = base.Generator;
			GeneratorPosition result = (generator != null) ? generator.GeneratorPositionFromIndex(index) : new GeneratorPosition(-1, index + 1);
			childIndex = ((result.Offset == 0) ? result.Index : (result.Index + 1));
			return result;
		}

		// Token: 0x06007828 RID: 30760 RVA: 0x002FCDC4 File Offset: 0x002FBDC4
		private void OnDelayCleanup(object sender, EventArgs e)
		{
			bool flag = false;
			try
			{
				flag = this.CleanUp();
			}
			finally
			{
				if (!flag)
				{
					this._cleanupDelay.Stop();
					this._cleanupDelay = null;
				}
			}
		}

		// Token: 0x06007829 RID: 30761 RVA: 0x002FCE04 File Offset: 0x002FBE04
		private object OnCleanUp(object args)
		{
			bool flag = false;
			try
			{
				flag = this.CleanUp();
			}
			finally
			{
				this._cleanupOperation = null;
			}
			if (flag)
			{
				this.EnsureCleanupOperation(true);
			}
			return null;
		}

		// Token: 0x0600782A RID: 30762 RVA: 0x002FCE40 File Offset: 0x002FBE40
		private bool CleanUp()
		{
			ItemsControl itemsControl = null;
			ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
			if (itemsControl == null || !this.IsVirtualizing || !base.IsItemsHost)
			{
				return false;
			}
			if (!VirtualizingStackPanel.IsVSP45Compat && this.IsMeasureCachesPending)
			{
				return true;
			}
			int tickCount = Environment.TickCount;
			bool result = false;
			UIElementCollection internalChildren = base.InternalChildren;
			int minDesiredGenerated = this.MinDesiredGenerated;
			int num = this.MaxDesiredGenerated - minDesiredGenerated;
			int num2 = internalChildren.Count - num;
			if (this.HasVirtualizingChildren || num2 > num * 2)
			{
				result = ((Mouse.LeftButton == MouseButtonState.Pressed && num2 < 1000) || this.CleanupContainers(this._firstItemInExtendedViewportIndex, this._actualItemsInExtendedViewportCount, itemsControl, true, tickCount));
			}
			return result;
		}

		// Token: 0x0600782B RID: 30763 RVA: 0x002FCEE3 File Offset: 0x002FBEE3
		private bool NotifyCleanupItem(int childIndex, UIElementCollection children, ItemsControl itemsControl)
		{
			return this.NotifyCleanupItem(children[childIndex], itemsControl);
		}

		// Token: 0x0600782C RID: 30764 RVA: 0x002FCEF4 File Offset: 0x002FBEF4
		private bool NotifyCleanupItem(UIElement child, ItemsControl itemsControl)
		{
			CleanUpVirtualizedItemEventArgs cleanUpVirtualizedItemEventArgs = new CleanUpVirtualizedItemEventArgs(itemsControl.ItemContainerGenerator.ItemFromContainer(child), child);
			cleanUpVirtualizedItemEventArgs.Source = this;
			this.OnCleanUpVirtualizedItem(cleanUpVirtualizedItemEventArgs);
			return !cleanUpVirtualizedItemEventArgs.Cancel;
		}

		// Token: 0x0600782D RID: 30765 RVA: 0x002FCF2C File Offset: 0x002FBF2C
		private void CleanupRange(IList children, IItemContainerGenerator generator, int startIndex, int count)
		{
			if (this.InRecyclingMode)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					List<string> list = new List<string>(count);
					for (int i = 0; i < count; i++)
					{
						list.Add(this.ContainerPath((DependencyObject)children[startIndex + i]));
					}
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RecycleChildren, new object[]
					{
						startIndex,
						count,
						list
					});
				}
				((IRecyclingItemContainerGenerator)generator).Recycle(new GeneratorPosition(startIndex, 0), count);
				this._realizedChildren.RemoveRange(startIndex, count);
				return;
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				List<string> list2 = new List<string>(count);
				for (int j = 0; j < count; j++)
				{
					list2.Add(this.ContainerPath((DependencyObject)children[startIndex + j]));
				}
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemoveChildren, new object[]
				{
					startIndex,
					count,
					list2
				});
			}
			VirtualizingPanel.RemoveInternalChildRange((UIElementCollection)children, startIndex, count);
			generator.Remove(new GeneratorPosition(startIndex, 0), count);
			this.AdjustFirstVisibleChildIndex(startIndex, count);
		}

		// Token: 0x0600782E RID: 30766 RVA: 0x002FD053 File Offset: 0x002FC053
		private void AdjustFirstVisibleChildIndex(int startIndex, int count)
		{
			if (startIndex < this._firstItemInExtendedViewportChildIndex)
			{
				if (startIndex + count - 1 < this._firstItemInExtendedViewportChildIndex)
				{
					this._firstItemInExtendedViewportChildIndex -= count;
					return;
				}
				this._firstItemInExtendedViewportChildIndex = startIndex;
			}
		}

		// Token: 0x17001BC5 RID: 7109
		// (get) Token: 0x0600782F RID: 30767 RVA: 0x002FD081 File Offset: 0x002FC081
		private int MinDesiredGenerated
		{
			get
			{
				return Math.Max(0, this._firstItemInExtendedViewportIndex);
			}
		}

		// Token: 0x17001BC6 RID: 7110
		// (get) Token: 0x06007830 RID: 30768 RVA: 0x002FD08F File Offset: 0x002FC08F
		private int MaxDesiredGenerated
		{
			get
			{
				return Math.Min(this.ItemCount, this._firstItemInExtendedViewportIndex + this._actualItemsInExtendedViewportCount);
			}
		}

		// Token: 0x17001BC7 RID: 7111
		// (get) Token: 0x06007831 RID: 30769 RVA: 0x002FD0A9 File Offset: 0x002FC0A9
		private int ItemCount
		{
			get
			{
				base.EnsureGenerator();
				return ((ItemContainerGenerator)base.Generator).ItemsInternal.Count;
			}
		}

		// Token: 0x06007832 RID: 30770 RVA: 0x002FD0C6 File Offset: 0x002FC0C6
		private void EnsureScrollData()
		{
			if (this._scrollData == null)
			{
				this._scrollData = new VirtualizingStackPanel.ScrollData();
			}
		}

		// Token: 0x06007833 RID: 30771 RVA: 0x002FD0DB File Offset: 0x002FC0DB
		private static void ResetScrolling(VirtualizingStackPanel element)
		{
			element.InvalidateMeasure();
			if (element.IsScrolling)
			{
				element._scrollData.ClearLayout();
			}
		}

		// Token: 0x06007834 RID: 30772 RVA: 0x002FD0F6 File Offset: 0x002FC0F6
		private void OnScrollChange()
		{
			if (this.ScrollOwner != null)
			{
				this.ScrollOwner.InvalidateScrollInfo();
			}
		}

		// Token: 0x06007835 RID: 30773 RVA: 0x002FD10C File Offset: 0x002FC10C
		private void SetAndVerifyScrollingData(bool isHorizontal, Rect viewport, Size constraint, UIElement firstContainerInViewport, double firstContainerOffsetFromViewport, bool hasAverageContainerSizeChanged, double newOffset, ref Size stackPixelSize, ref Size stackLogicalSize, ref Size stackPixelSizeInViewport, ref Size stackLogicalSizeInViewport, ref Size stackPixelSizeInCacheBeforeViewport, ref Size stackLogicalSizeInCacheBeforeViewport, ref bool remeasure, ref double? lastPageSafeOffset, ref double? lastPagePixelSize, ref List<double> previouslyMeasuredOffsets)
		{
			Vector vector = new Vector(viewport.Location.X, viewport.Location.Y);
			Vector offset = this._scrollData._offset;
			Size size;
			Size size2;
			if (this.IsPixelBased)
			{
				size = stackPixelSize;
				size2 = viewport.Size;
			}
			else
			{
				size = stackLogicalSize;
				size2 = stackLogicalSizeInViewport;
				if (isHorizontal)
				{
					if (DoubleUtil.GreaterThan(stackPixelSizeInViewport.Width, constraint.Width) && size2.Width > 1.0)
					{
						double num = size2.Width;
						size2.Width = num - 1.0;
					}
					size2.Height = viewport.Height;
				}
				else
				{
					if (DoubleUtil.GreaterThan(stackPixelSizeInViewport.Height, constraint.Height) && size2.Height > 1.0)
					{
						double num = size2.Height;
						size2.Height = num - 1.0;
					}
					size2.Width = viewport.Width;
				}
			}
			if (isHorizontal)
			{
				if (this.MeasureCaches && this.IsVirtualizing)
				{
					stackPixelSize.Height = this._scrollData._extent.Height;
				}
				this._scrollData._maxDesiredSize.Height = Math.Max(this._scrollData._maxDesiredSize.Height, stackPixelSize.Height);
				stackPixelSize.Height = this._scrollData._maxDesiredSize.Height;
				size.Height = stackPixelSize.Height;
				if (double.IsPositiveInfinity(constraint.Height))
				{
					size2.Height = stackPixelSize.Height;
				}
			}
			else
			{
				if (this.MeasureCaches && this.IsVirtualizing)
				{
					stackPixelSize.Width = this._scrollData._extent.Width;
				}
				this._scrollData._maxDesiredSize.Width = Math.Max(this._scrollData._maxDesiredSize.Width, stackPixelSize.Width);
				stackPixelSize.Width = this._scrollData._maxDesiredSize.Width;
				size.Width = stackPixelSize.Width;
				if (double.IsPositiveInfinity(constraint.Width))
				{
					size2.Width = stackPixelSize.Width;
				}
			}
			if (!double.IsPositiveInfinity(constraint.Width))
			{
				stackPixelSize.Width = ((this.IsPixelBased || DoubleUtil.AreClose(vector.X, 0.0)) ? Math.Min(stackPixelSize.Width, constraint.Width) : constraint.Width);
			}
			if (!double.IsPositiveInfinity(constraint.Height))
			{
				stackPixelSize.Height = ((this.IsPixelBased || DoubleUtil.AreClose(vector.Y, 0.0)) ? Math.Min(stackPixelSize.Height, constraint.Height) : constraint.Height);
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SVSDBegin, new object[]
				{
					"isa:",
					this.IsScrollActive,
					"mc:",
					this.MeasureCaches,
					"o:",
					this._scrollData._offset,
					"co:",
					vector,
					"ex:",
					size,
					"vs:",
					size2,
					"pxInV:",
					stackPixelSizeInViewport
				});
				if (hasAverageContainerSizeChanged)
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SVSDBegin, new object[]
					{
						"acs:",
						this.UniformOrAverageContainerSize,
						this.UniformOrAverageContainerPixelSize
					});
				}
			}
			bool flag = isHorizontal ? (!DoubleUtil.AreClose(vector.X, this._scrollData._offset.X) || (this.IsScrollActive && vector.X > 0.0 && DoubleUtil.GreaterThanOrClose(vector.X, this._scrollData.Extent.Width - this._scrollData.Viewport.Width))) : (!DoubleUtil.AreClose(vector.Y, this._scrollData._offset.Y) || (this.IsScrollActive && vector.Y > 0.0 && DoubleUtil.GreaterThanOrClose(vector.Y, this._scrollData.Extent.Height - this._scrollData.Viewport.Height)));
			if (!isHorizontal)
			{
				if (DoubleUtil.AreClose(vector.X, this._scrollData._offset.X))
				{
					if (this.IsScrollActive && vector.X > 0.0)
					{
						DoubleUtil.GreaterThanOrClose(vector.X, this._scrollData.Extent.Width - this._scrollData.Viewport.Width);
					}
				}
			}
			else if (DoubleUtil.AreClose(vector.Y, this._scrollData._offset.Y))
			{
				if (this.IsScrollActive && vector.Y > 0.0)
				{
					DoubleUtil.GreaterThanOrClose(vector.Y, this._scrollData.Extent.Height - this._scrollData.Viewport.Height);
				}
			}
			bool flag2 = false;
			if (hasAverageContainerSizeChanged && newOffset >= 0.0)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.AdjustOffset, new object[]
					{
						newOffset,
						vector
					});
				}
				if (isHorizontal)
				{
					if (!LayoutDoubleUtil.AreClose(vector.X, newOffset))
					{
						double num2 = newOffset - vector.X;
						vector.X = newOffset;
						offset.X = newOffset;
						this._viewport.X = newOffset;
						this._extendedViewport.X = this._extendedViewport.X + num2;
						flag2 = true;
						if (DoubleUtil.GreaterThan(newOffset + size2.Width, size.Width))
						{
							flag = true;
							this.IsScrollActive = true;
							this._scrollData.HorizontalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
						}
					}
				}
				else if (!LayoutDoubleUtil.AreClose(vector.Y, newOffset))
				{
					double num3 = newOffset - vector.Y;
					vector.Y = newOffset;
					offset.Y = newOffset;
					this._viewport.Y = newOffset;
					this._extendedViewport.Y = this._extendedViewport.Y + num3;
					if (DoubleUtil.GreaterThan(newOffset + size2.Height, size.Height))
					{
						flag = true;
						this.IsScrollActive = true;
						this._scrollData.VerticalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
					}
				}
			}
			if (lastPagePixelSize != null && lastPageSafeOffset == null && !DoubleUtil.AreClose(isHorizontal ? stackPixelSizeInViewport.Width : stackPixelSizeInViewport.Height, lastPagePixelSize.Value))
			{
				flag2 = true;
				flag = true;
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LastPageSizeChange, new object[]
					{
						vector,
						stackPixelSizeInViewport,
						lastPagePixelSize
					});
				}
			}
			if (flag2)
			{
				if (previouslyMeasuredOffsets != null)
				{
					previouslyMeasuredOffsets.Clear();
				}
				lastPageSafeOffset = null;
				lastPagePixelSize = null;
			}
			bool flag3 = !DoubleUtil.AreClose(size2, this._scrollData._viewport);
			bool flag4 = !DoubleUtil.AreClose(size, this._scrollData._extent);
			bool flag5 = !DoubleUtil.AreClose(vector, this._scrollData._computedOffset);
			bool flag6;
			bool flag7;
			if (flag4)
			{
				flag6 = !DoubleUtil.AreClose(size.Width, this._scrollData._extent.Width);
				flag7 = !DoubleUtil.AreClose(size.Height, this._scrollData._extent.Height);
			}
			else
			{
				flag7 = (flag6 = false);
			}
			Vector vector2 = vector;
			bool flag8 = false;
			ScrollViewer scrollOwner = this.ScrollOwner;
			if (scrollOwner.InChildMeasurePass1 || scrollOwner.InChildMeasurePass2)
			{
				if (scrollOwner.VerticalScrollBarVisibility == ScrollBarVisibility.Auto)
				{
					Visibility computedVerticalScrollBarVisibility = scrollOwner.ComputedVerticalScrollBarVisibility;
					Visibility visibility = DoubleUtil.LessThanOrClose(size.Height, size2.Height) ? Visibility.Collapsed : Visibility.Visible;
					if (computedVerticalScrollBarVisibility != visibility)
					{
						vector2 = offset;
						flag8 = true;
					}
				}
				if (!flag8 && scrollOwner.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto)
				{
					Visibility computedHorizontalScrollBarVisibility = scrollOwner.ComputedHorizontalScrollBarVisibility;
					Visibility visibility2 = DoubleUtil.LessThanOrClose(size.Width, size2.Width) ? Visibility.Collapsed : Visibility.Visible;
					if (computedHorizontalScrollBarVisibility != visibility2)
					{
						vector2 = offset;
						flag8 = true;
					}
				}
				if (flag8 && VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ScrollBarChangeVisibility, new object[]
					{
						vector2
					});
				}
			}
			if (isHorizontal)
			{
				if (!flag8)
				{
					if (this.WasOffsetPreviouslyMeasured(previouslyMeasuredOffsets, vector.X))
					{
						if (!this.IsPixelBased && lastPageSafeOffset != null && !DoubleUtil.AreClose(lastPageSafeOffset.Value, vector.X))
						{
							if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureCycle, new object[]
								{
									vector2.X,
									lastPageSafeOffset
								});
							}
							vector2.X = lastPageSafeOffset.Value;
							lastPageSafeOffset = null;
							remeasure = true;
						}
					}
					else if (!remeasure)
					{
						if (!this.IsPixelBased)
						{
							if (!remeasure && !this.IsEndOfViewport(isHorizontal, viewport, stackPixelSizeInViewport) && DoubleUtil.GreaterThan(stackLogicalSize.Width, stackLogicalSizeInViewport.Width))
							{
								if (lastPageSafeOffset == null || vector.X < lastPageSafeOffset.Value)
								{
									lastPageSafeOffset = new double?(vector.X);
									lastPagePixelSize = new double?(stackPixelSizeInViewport.Width);
								}
								double num4 = stackPixelSizeInViewport.Width / stackLogicalSizeInViewport.Width;
								double num5 = Math.Floor(viewport.Width / num4);
								if (DoubleUtil.GreaterThan(num5, size2.Width))
								{
									if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
									{
										VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExpandViewport, new object[]
										{
											"off:",
											vector.X,
											lastPageSafeOffset,
											"pxSz:",
											stackPixelSizeInViewport.Width,
											viewport.Width,
											"itSz:",
											stackLogicalSizeInViewport.Width,
											size2.Width,
											"newVpSz:",
											num5
										});
									}
									vector2.X = double.PositiveInfinity;
									size2.Width = num5;
									remeasure = true;
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
								}
							}
							if (!remeasure && flag && flag3 && !DoubleUtil.AreClose(this._scrollData._viewport.Width, size2.Width))
							{
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndChangeOffset, new object[]
									{
										"off:",
										vector.X,
										"vpSz:",
										this._scrollData._viewport.Width,
										size2.Width,
										"newOff:",
										this._scrollData._offset
									});
								}
								remeasure = true;
								vector2.X = double.PositiveInfinity;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
								if (DoubleUtil.AreClose(size2.Width, 0.0))
								{
									size2.Width = this._scrollData._viewport.Width;
								}
							}
						}
						if (!remeasure && flag6)
						{
							if (this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.ToEnd || (DoubleUtil.GreaterThan(vector.X, 0.0) && DoubleUtil.GreaterThan(vector.X, size.Width - size2.Width)))
							{
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExtentChanged, new object[]
									{
										"off:",
										vector.X,
										"ext:",
										this._scrollData._extent.Width,
										size.Width,
										"vpSz:",
										size2.Width
									});
								}
								remeasure = true;
								vector2.X = double.PositiveInfinity;
								this._scrollData.HorizontalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
							}
							else if (this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.Absolute && !DoubleUtil.AreClose(this._scrollData._extent.Width, 0.0) && !DoubleUtil.AreClose(size.Width, 0.0))
							{
								if (this.IsPixelBased)
								{
									if (!LayoutDoubleUtil.AreClose(vector.X / size.Width, this._scrollData._offset.X / this._scrollData._extent.Width))
									{
										remeasure = true;
										vector2.X = size.Width * this._scrollData._offset.X / this._scrollData._extent.Width;
										this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
									}
								}
								else if (!LayoutDoubleUtil.AreClose(Math.Floor(vector.X) / size.Width, Math.Floor(this._scrollData._offset.X) / this._scrollData._extent.Width))
								{
									remeasure = true;
									vector2.X = Math.Floor(size.Width * Math.Floor(this._scrollData._offset.X) / this._scrollData._extent.Width);
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
								}
								if ((VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this)) & remeasure)
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureRatio, new object[]
									{
										"expRat:",
										this._scrollData._offset.X,
										this._scrollData._extent.Width,
										this._scrollData._offset.X / this._scrollData._extent.Width,
										"actRat:",
										vector.X,
										size.Width,
										vector.X / size.Width,
										"newOff:",
										vector2.X
									});
								}
							}
						}
						if (!remeasure && flag7)
						{
							if (this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.ToEnd || (DoubleUtil.GreaterThan(vector.Y, 0.0) && DoubleUtil.GreaterThan(vector.Y, size.Height - size2.Height)))
							{
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExtentChanged, new object[]
									{
										"perp",
										"off:",
										vector.Y,
										"ext:",
										this._scrollData._extent.Height,
										size.Height,
										"vpSz:",
										size2.Height
									});
								}
								remeasure = true;
								vector2.Y = double.PositiveInfinity;
								this._scrollData.VerticalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
							}
							else if (this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.Absolute && !DoubleUtil.AreClose(this._scrollData._extent.Height, 0.0) && !DoubleUtil.AreClose(size.Height, 0.0))
							{
								if (!LayoutDoubleUtil.AreClose(vector.Y / size.Height, this._scrollData._offset.Y / this._scrollData._extent.Height))
								{
									remeasure = true;
									vector2.Y = size.Height * this._scrollData._offset.Y / this._scrollData._extent.Height;
								}
								if ((VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this)) & remeasure)
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureRatio, new object[]
									{
										"perp",
										"expRat:",
										this._scrollData._offset.Y,
										this._scrollData._extent.Height,
										this._scrollData._offset.Y / this._scrollData._extent.Height,
										"actRat:",
										vector.Y,
										size.Height,
										vector.Y / size.Height,
										"newOff:",
										vector2.Y
									});
								}
							}
						}
					}
				}
			}
			else if (!flag8)
			{
				if (this.WasOffsetPreviouslyMeasured(previouslyMeasuredOffsets, vector.Y))
				{
					if (!this.IsPixelBased && lastPageSafeOffset != null && !DoubleUtil.AreClose(lastPageSafeOffset.Value, vector.Y))
					{
						if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
						{
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureCycle, new object[]
							{
								vector2.Y,
								lastPageSafeOffset
							});
						}
						vector2.Y = lastPageSafeOffset.Value;
						lastPageSafeOffset = null;
						remeasure = true;
					}
				}
				else if (!remeasure)
				{
					if (!this.IsPixelBased)
					{
						if (!remeasure && !this.IsEndOfViewport(isHorizontal, viewport, stackPixelSizeInViewport) && DoubleUtil.GreaterThan(stackLogicalSize.Height, stackLogicalSizeInViewport.Height))
						{
							if (lastPageSafeOffset == null || vector.Y < lastPageSafeOffset.Value)
							{
								lastPageSafeOffset = new double?(vector.Y);
								lastPagePixelSize = new double?(stackPixelSizeInViewport.Height);
							}
							double num6 = stackPixelSizeInViewport.Height / stackLogicalSizeInViewport.Height;
							double num7 = Math.Floor(viewport.Height / num6);
							if (DoubleUtil.GreaterThan(num7, size2.Height))
							{
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExpandViewport, new object[]
									{
										"off:",
										vector.Y,
										lastPageSafeOffset,
										"pxSz:",
										stackPixelSizeInViewport.Height,
										viewport.Height,
										"itSz:",
										stackLogicalSizeInViewport.Height,
										size2.Height,
										"newVpSz:",
										num7
									});
								}
								vector2.Y = double.PositiveInfinity;
								size2.Height = num7;
								remeasure = true;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
							}
						}
						if (!remeasure && flag && flag3 && !DoubleUtil.AreClose(this._scrollData._viewport.Height, size2.Height))
						{
							if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndChangeOffset, new object[]
								{
									"off:",
									vector.Y,
									"vpSz:",
									this._scrollData._viewport.Height,
									size2.Height,
									"newOff:",
									this._scrollData._offset
								});
							}
							remeasure = true;
							vector2.Y = double.PositiveInfinity;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
							if (DoubleUtil.AreClose(size2.Height, 0.0))
							{
								size2.Height = this._scrollData._viewport.Height;
							}
						}
					}
					if (!remeasure && flag7)
					{
						if (this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.ToEnd || (DoubleUtil.GreaterThan(vector.Y, 0.0) && DoubleUtil.GreaterThan(vector.Y, size.Height - size2.Height)))
						{
							if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExtentChanged, new object[]
								{
									"off:",
									vector.Y,
									"ext:",
									this._scrollData._extent.Height,
									size.Height,
									"vpSz:",
									size2.Height
								});
							}
							remeasure = true;
							vector2.Y = double.PositiveInfinity;
							this._scrollData.VerticalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
						}
						else if (this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.Absolute && !DoubleUtil.AreClose(this._scrollData._extent.Height, 0.0) && !DoubleUtil.AreClose(size.Height, 0.0))
						{
							if (this.IsPixelBased)
							{
								if (!LayoutDoubleUtil.AreClose(vector.Y / size.Height, this._scrollData._offset.Y / this._scrollData._extent.Height))
								{
									remeasure = true;
									vector2.Y = size.Height * this._scrollData._offset.Y / this._scrollData._extent.Height;
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
								}
							}
							else if (!LayoutDoubleUtil.AreClose(Math.Floor(vector.Y) / size.Height, Math.Floor(this._scrollData._offset.Y) / this._scrollData._extent.Height))
							{
								remeasure = true;
								vector2.Y = Math.Floor(size.Height * Math.Floor(this._scrollData._offset.Y) / this._scrollData._extent.Height);
							}
							if ((VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this)) & remeasure)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureRatio, new object[]
								{
									"expRat:",
									this._scrollData._offset.Y,
									this._scrollData._extent.Height,
									this._scrollData._offset.Y / this._scrollData._extent.Height,
									"actRat:",
									vector.Y,
									size.Height,
									vector.Y / size.Height,
									"newOff:",
									vector2.Y
								});
							}
						}
					}
					if (!remeasure && flag6)
					{
						if (this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.ToEnd || (DoubleUtil.GreaterThan(vector.X, 0.0) && DoubleUtil.GreaterThan(vector.X, size.Width - size2.Width)))
						{
							if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExtentChanged, new object[]
								{
									"perp",
									"off:",
									vector.X,
									"ext:",
									this._scrollData._extent.Width,
									size.Width,
									"vpSz:",
									size2.Width
								});
							}
							remeasure = true;
							vector2.X = double.PositiveInfinity;
							this._scrollData.HorizontalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
						}
						else if (this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.Absolute && !DoubleUtil.AreClose(this._scrollData._extent.Width, 0.0) && !DoubleUtil.AreClose(size.Width, 0.0))
						{
							if (!LayoutDoubleUtil.AreClose(vector.X / size.Width, this._scrollData._offset.X / this._scrollData._extent.Width))
							{
								remeasure = true;
								vector2.X = size.Width * this._scrollData._offset.X / this._scrollData._extent.Width;
							}
							if ((VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this)) & remeasure)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureRatio, new object[]
								{
									"perp",
									"expRat:",
									this._scrollData._offset.X,
									this._scrollData._extent.Width,
									this._scrollData._offset.X / this._scrollData._extent.Width,
									"actRat:",
									vector.X,
									size.Width,
									vector.X / size.Width,
									"newOff:",
									vector2.X
								});
							}
						}
					}
				}
			}
			if (remeasure && this.IsVirtualizing && !this.IsScrollActive)
			{
				if (isHorizontal && this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.ToEnd)
				{
					this.IsScrollActive = true;
				}
				if (!isHorizontal && this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.ToEnd)
				{
					this.IsScrollActive = true;
				}
			}
			if (!this.IsVirtualizing && !remeasure)
			{
				this.ClearIsScrollActive();
			}
			flag3 = !DoubleUtil.AreClose(size2, this._scrollData._viewport);
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SVSDEnd, new object[]
				{
					"off:",
					this._scrollData._offset,
					vector2,
					"ext:",
					this._scrollData._extent,
					size,
					"co:",
					this._scrollData._computedOffset,
					vector,
					"vp:",
					this._scrollData._viewport,
					size2
				});
			}
			if (flag3 || flag4 || flag5)
			{
				Vector computedOffset = this._scrollData._computedOffset;
				Size viewport2 = this._scrollData._viewport;
				this._scrollData._viewport = size2;
				this._scrollData._extent = size;
				this._scrollData._computedOffset = vector;
				if (flag3)
				{
					this.OnViewportSizeChanged(viewport2, size2);
				}
				if (flag5)
				{
					this.OnViewportOffsetChanged(computedOffset, vector);
				}
				this.OnScrollChange();
			}
			this._scrollData._offset = vector2;
		}

		// Token: 0x06007836 RID: 30774 RVA: 0x002FEDD8 File Offset: 0x002FDDD8
		private void SetAndVerifyScrollingData(bool isHorizontal, Rect viewport, Size constraint, ref Size stackPixelSize, ref Size stackLogicalSize, ref Size stackPixelSizeInViewport, ref Size stackLogicalSizeInViewport, ref Size stackPixelSizeInCacheBeforeViewport, ref Size stackLogicalSizeInCacheBeforeViewport, ref bool remeasure, ref double? lastPageSafeOffset, ref List<double> previouslyMeasuredOffsets)
		{
			Vector vector = new Vector(viewport.Location.X, viewport.Location.Y);
			Size size;
			Size size2;
			if (this.IsPixelBased)
			{
				size = stackPixelSize;
				size2 = viewport.Size;
			}
			else
			{
				size = stackLogicalSize;
				size2 = stackLogicalSizeInViewport;
				if (isHorizontal)
				{
					if (DoubleUtil.GreaterThan(stackPixelSizeInViewport.Width, constraint.Width) && size2.Width > 1.0)
					{
						double num = size2.Width;
						size2.Width = num - 1.0;
					}
					size2.Height = viewport.Height;
				}
				else
				{
					if (DoubleUtil.GreaterThan(stackPixelSizeInViewport.Height, constraint.Height) && size2.Height > 1.0)
					{
						double num = size2.Height;
						size2.Height = num - 1.0;
					}
					size2.Width = viewport.Width;
				}
			}
			if (isHorizontal)
			{
				if (this.MeasureCaches && this.IsVirtualizing)
				{
					stackPixelSize.Height = this._scrollData._extent.Height;
				}
				this._scrollData._maxDesiredSize.Height = Math.Max(this._scrollData._maxDesiredSize.Height, stackPixelSize.Height);
				stackPixelSize.Height = this._scrollData._maxDesiredSize.Height;
				size.Height = stackPixelSize.Height;
				if (double.IsPositiveInfinity(constraint.Height))
				{
					size2.Height = stackPixelSize.Height;
				}
			}
			else
			{
				if (this.MeasureCaches && this.IsVirtualizing)
				{
					stackPixelSize.Width = this._scrollData._extent.Width;
				}
				this._scrollData._maxDesiredSize.Width = Math.Max(this._scrollData._maxDesiredSize.Width, stackPixelSize.Width);
				stackPixelSize.Width = this._scrollData._maxDesiredSize.Width;
				size.Width = stackPixelSize.Width;
				if (double.IsPositiveInfinity(constraint.Width))
				{
					size2.Width = stackPixelSize.Width;
				}
			}
			if (!double.IsPositiveInfinity(constraint.Width))
			{
				stackPixelSize.Width = ((this.IsPixelBased || DoubleUtil.AreClose(vector.X, 0.0)) ? Math.Min(stackPixelSize.Width, constraint.Width) : constraint.Width);
			}
			if (!double.IsPositiveInfinity(constraint.Height))
			{
				stackPixelSize.Height = ((this.IsPixelBased || DoubleUtil.AreClose(vector.Y, 0.0)) ? Math.Min(stackPixelSize.Height, constraint.Height) : constraint.Height);
			}
			bool flag = !DoubleUtil.AreClose(size2, this._scrollData._viewport);
			bool flag2 = !DoubleUtil.AreClose(size, this._scrollData._extent);
			bool flag3 = !DoubleUtil.AreClose(vector, this._scrollData._computedOffset);
			Vector offset = vector;
			bool flag4 = true;
			ScrollViewer scrollOwner = this.ScrollOwner;
			if (scrollOwner.InChildMeasurePass1 || scrollOwner.InChildMeasurePass2)
			{
				if (scrollOwner.VerticalScrollBarVisibility == ScrollBarVisibility.Auto)
				{
					Visibility computedVerticalScrollBarVisibility = scrollOwner.ComputedVerticalScrollBarVisibility;
					Visibility visibility = DoubleUtil.LessThanOrClose(size.Height, size2.Height) ? Visibility.Collapsed : Visibility.Visible;
					if (computedVerticalScrollBarVisibility != visibility)
					{
						offset = this._scrollData._offset;
						flag4 = false;
					}
				}
				if (flag4 && scrollOwner.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto)
				{
					Visibility computedHorizontalScrollBarVisibility = scrollOwner.ComputedHorizontalScrollBarVisibility;
					Visibility visibility2 = DoubleUtil.LessThanOrClose(size.Width, size2.Width) ? Visibility.Collapsed : Visibility.Visible;
					if (computedHorizontalScrollBarVisibility != visibility2)
					{
						offset = this._scrollData._offset;
					}
				}
			}
			if (isHorizontal)
			{
				flag4 = !this.WasOffsetPreviouslyMeasured(previouslyMeasuredOffsets, vector.X);
				if (flag4)
				{
					bool flag5 = !DoubleUtil.AreClose(vector.X, this._scrollData._offset.X);
					if (!this.IsPixelBased)
					{
						if (!this.IsEndOfViewport(isHorizontal, viewport, stackPixelSizeInViewport) && DoubleUtil.GreaterThan(stackLogicalSize.Width, stackLogicalSizeInViewport.Width))
						{
							lastPageSafeOffset = new double?((lastPageSafeOffset != null) ? Math.Min(vector.X, lastPageSafeOffset.Value) : vector.X);
							double num2 = stackPixelSizeInViewport.Width / stackLogicalSizeInViewport.Width;
							double num3 = Math.Floor(viewport.Width / num2);
							if (DoubleUtil.GreaterThan(num3, size2.Width))
							{
								offset.X = double.PositiveInfinity;
								size2.Width = num3;
								remeasure = true;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
							}
						}
						if (!remeasure && flag5 && flag && !DoubleUtil.AreClose(this._scrollData._viewport.Width, size2.Width))
						{
							remeasure = true;
							offset.X = this._scrollData._offset.X;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
							if (DoubleUtil.AreClose(size2.Width, 0.0))
							{
								size2.Width = this._scrollData._viewport.Width;
							}
						}
					}
					if (!remeasure && flag2 && !DoubleUtil.AreClose(this._scrollData._extent.Width, size.Width))
					{
						if (DoubleUtil.GreaterThan(vector.X, 0.0) && DoubleUtil.GreaterThan(vector.X, size.Width - size2.Width))
						{
							remeasure = true;
							offset.X = double.PositiveInfinity;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
						}
						if (!remeasure && flag5)
						{
							remeasure = true;
							offset.X = this._scrollData._offset.X;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
						}
						if (!remeasure && ((this.MeasureCaches && !this.WasLastMeasurePassAnchored) || (this._scrollData._firstContainerInViewport == null && flag3 && !LayoutDoubleUtil.AreClose(vector.X, this._scrollData._computedOffset.X))) && !DoubleUtil.AreClose(this._scrollData._extent.Width, 0.0) && !DoubleUtil.AreClose(size.Width, 0.0))
						{
							if (this.IsPixelBased)
							{
								if (!LayoutDoubleUtil.AreClose(vector.X / size.Width, this._scrollData._offset.X / this._scrollData._extent.Width))
								{
									remeasure = true;
									offset.X = size.Width * this._scrollData._offset.X / this._scrollData._extent.Width;
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
								}
							}
							else if (!LayoutDoubleUtil.AreClose(Math.Floor(vector.X) / size.Width, Math.Floor(this._scrollData._offset.X) / this._scrollData._extent.Width))
							{
								remeasure = true;
								offset.X = Math.Floor(size.Width * Math.Floor(this._scrollData._offset.X) / this._scrollData._extent.Width);
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
							}
						}
					}
				}
				else if (!this.IsPixelBased && lastPageSafeOffset != null && !DoubleUtil.AreClose(lastPageSafeOffset.Value, vector.X))
				{
					offset.X = lastPageSafeOffset.Value;
					lastPageSafeOffset = null;
					remeasure = true;
				}
			}
			else
			{
				flag4 = !this.WasOffsetPreviouslyMeasured(previouslyMeasuredOffsets, vector.Y);
				if (flag4)
				{
					bool flag6 = !DoubleUtil.AreClose(vector.Y, this._scrollData._offset.Y);
					if (!this.IsPixelBased)
					{
						if (!this.IsEndOfViewport(isHorizontal, viewport, stackPixelSizeInViewport) && DoubleUtil.GreaterThan(stackLogicalSize.Height, stackLogicalSizeInViewport.Height))
						{
							lastPageSafeOffset = new double?((lastPageSafeOffset != null) ? Math.Min(vector.Y, lastPageSafeOffset.Value) : vector.Y);
							double num4 = stackPixelSizeInViewport.Height / stackLogicalSizeInViewport.Height;
							double num5 = Math.Floor(viewport.Height / num4);
							if (DoubleUtil.GreaterThan(num5, size2.Height))
							{
								offset.Y = double.PositiveInfinity;
								size2.Height = num5;
								remeasure = true;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
							}
						}
						if (!remeasure && flag6 && flag && !DoubleUtil.AreClose(this._scrollData._viewport.Height, size2.Height))
						{
							remeasure = true;
							offset.Y = this._scrollData._offset.Y;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
							if (DoubleUtil.AreClose(size2.Height, 0.0))
							{
								size2.Height = this._scrollData._viewport.Height;
							}
						}
					}
					if (!remeasure && flag2 && !DoubleUtil.AreClose(this._scrollData._extent.Height, size.Height))
					{
						if (DoubleUtil.GreaterThan(vector.Y, 0.0) && DoubleUtil.GreaterThan(vector.Y, size.Height - size2.Height))
						{
							remeasure = true;
							offset.Y = double.PositiveInfinity;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
						}
						if (!remeasure && flag6)
						{
							remeasure = true;
							offset.Y = this._scrollData._offset.Y;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
						}
						if (!remeasure && ((this.MeasureCaches && !this.WasLastMeasurePassAnchored) || (this._scrollData._firstContainerInViewport == null && flag3 && !LayoutDoubleUtil.AreClose(vector.Y, this._scrollData._computedOffset.Y))) && !DoubleUtil.AreClose(this._scrollData._extent.Height, 0.0) && !DoubleUtil.AreClose(size.Height, 0.0))
						{
							if (this.IsPixelBased)
							{
								if (!LayoutDoubleUtil.AreClose(vector.Y / size.Height, this._scrollData._offset.Y / this._scrollData._extent.Height))
								{
									remeasure = true;
									offset.Y = size.Height * this._scrollData._offset.Y / this._scrollData._extent.Height;
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
								}
							}
							else if (!LayoutDoubleUtil.AreClose(Math.Floor(vector.Y) / size.Height, Math.Floor(this._scrollData._offset.Y) / this._scrollData._extent.Height))
							{
								remeasure = true;
								offset.Y = Math.Floor(size.Height * Math.Floor(this._scrollData._offset.Y) / this._scrollData._extent.Height);
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
							}
						}
					}
				}
				else if (!this.IsPixelBased && lastPageSafeOffset != null && !DoubleUtil.AreClose(lastPageSafeOffset.Value, vector.Y))
				{
					offset.Y = lastPageSafeOffset.Value;
					lastPageSafeOffset = null;
					remeasure = true;
				}
			}
			flag = !DoubleUtil.AreClose(size2, this._scrollData._viewport);
			if (flag || flag2 || flag3)
			{
				Vector computedOffset = this._scrollData._computedOffset;
				Size viewport2 = this._scrollData._viewport;
				this._scrollData._viewport = size2;
				this._scrollData._extent = size;
				this._scrollData._computedOffset = vector;
				if (flag)
				{
					this.OnViewportSizeChanged(viewport2, size2);
				}
				if (flag3)
				{
					this.OnViewportOffsetChanged(computedOffset, vector);
				}
				this.OnScrollChange();
			}
			this._scrollData._offset = offset;
		}

		// Token: 0x06007837 RID: 30775 RVA: 0x002FFAA9 File Offset: 0x002FEAA9
		private void StorePreviouslyMeasuredOffset(ref List<double> previouslyMeasuredOffsets, double offset)
		{
			if (previouslyMeasuredOffsets == null)
			{
				previouslyMeasuredOffsets = new List<double>();
			}
			previouslyMeasuredOffsets.Add(offset);
		}

		// Token: 0x06007838 RID: 30776 RVA: 0x002FFAC0 File Offset: 0x002FEAC0
		private bool WasOffsetPreviouslyMeasured(List<double> previouslyMeasuredOffsets, double offset)
		{
			if (previouslyMeasuredOffsets != null)
			{
				for (int i = 0; i < previouslyMeasuredOffsets.Count; i++)
				{
					if (DoubleUtil.AreClose(previouslyMeasuredOffsets[i], offset))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007839 RID: 30777 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnViewportSizeChanged(Size oldViewportSize, Size newViewportSize)
		{
		}

		// Token: 0x0600783A RID: 30778 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnViewportOffsetChanged(Vector oldViewportOffset, Vector newViewportOffset)
		{
		}

		// Token: 0x0600783B RID: 30779 RVA: 0x002FFAF4 File Offset: 0x002FEAF4
		protected override double GetItemOffsetCore(UIElement child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			bool isHorizontal = this.Orientation == Orientation.Horizontal;
			ItemsControl itemsControl;
			GroupItem groupItem;
			IContainItemStorage containItemStorage;
			IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo;
			object item;
			IContainItemStorage containItemStorage2;
			bool flag;
			this.GetOwners(false, isHorizontal, out itemsControl, out groupItem, out containItemStorage, out hierarchicalVirtualizationAndScrollInfo, out item, out containItemStorage2, out flag);
			ItemContainerGenerator itemContainerGenerator = (ItemContainerGenerator)base.Generator;
			IList itemsInternal = itemContainerGenerator.ItemsInternal;
			int num = itemContainerGenerator.IndexFromContainer(child, true);
			double result = 0.0;
			if (num >= 0)
			{
				IContainItemStorage itemStorageProvider = VirtualizingStackPanel.IsVSP45Compat ? containItemStorage : containItemStorage2;
				this.ComputeDistance(itemsInternal, containItemStorage, isHorizontal, this.GetAreContainersUniformlySized(itemStorageProvider, item), this.GetUniformOrAverageContainerSize(itemStorageProvider, item), 0, num, out result);
			}
			return result;
		}

		// Token: 0x0600783C RID: 30780 RVA: 0x002FFB90 File Offset: 0x002FEB90
		private double FindScrollOffset(Visual v)
		{
			ItemsControl.GetItemsOwner(this);
			DependencyObject dependencyObject = v;
			DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
			double num = 0.0;
			bool flag = this.Orientation == Orientation.Horizontal;
			bool returnLocalIndex = true;
			for (;;)
			{
				IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(parent);
				if (virtualizingChild != null)
				{
					Panel itemsHost = virtualizingChild.ItemsHost;
					dependencyObject = this.FindDirectDescendentOfItemsHost(itemsHost, dependencyObject);
					if (dependencyObject != null)
					{
						VirtualizingPanel virtualizingPanel = itemsHost as VirtualizingPanel;
						if (virtualizingPanel != null && virtualizingPanel.CanHierarchicallyScrollAndVirtualize)
						{
							double itemOffset = virtualizingPanel.GetItemOffset((UIElement)dependencyObject);
							num += itemOffset;
							if (this.IsPixelBased)
							{
								if (VirtualizingStackPanel.IsVSP45Compat)
								{
									Size pixelSize = virtualizingChild.HeaderDesiredSizes.PixelSize;
									num += (flag ? pixelSize.Width : pixelSize.Height);
								}
								else
								{
									Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(virtualizingChild, null, null);
									num += (flag ? itemsHostInsetForChild.Left : itemsHostInsetForChild.Top);
								}
							}
							else if (VirtualizingStackPanel.IsVSP45Compat)
							{
								Size logicalSize = virtualizingChild.HeaderDesiredSizes.LogicalSize;
								num += (flag ? logicalSize.Width : logicalSize.Height);
							}
							else
							{
								Thickness itemsHostInsetForChild2 = this.GetItemsHostInsetForChild(virtualizingChild, null, null);
								bool flag2 = this.IsHeaderBeforeItems(flag, virtualizingChild as FrameworkElement, ref itemsHostInsetForChild2);
								num += (double)(flag2 ? 1 : 0);
							}
						}
					}
					dependencyObject = (DependencyObject)virtualizingChild;
				}
				else if (parent == this)
				{
					break;
				}
				parent = VisualTreeHelper.GetParent(parent);
			}
			dependencyObject = this.FindDirectDescendentOfItemsHost(this, dependencyObject);
			if (dependencyObject != null)
			{
				IContainItemStorage itemStorageProvider = VirtualizingStackPanel.GetItemStorageProvider(this);
				IContainItemStorage itemStorageProvider2 = VirtualizingStackPanel.IsVSP45Compat ? itemStorageProvider : (ItemsControl.GetItemsOwnerInternal(VisualTreeHelper.GetParent((Visual)itemStorageProvider)) as IContainItemStorage);
				IList itemsInternal = ((ItemContainerGenerator)this.Generator).ItemsInternal;
				int itemCount = ((ItemContainerGenerator)this.Generator).IndexFromContainer(dependencyObject, returnLocalIndex);
				double num2;
				this.ComputeDistance(itemsInternal, itemStorageProvider, flag, this.GetAreContainersUniformlySized(itemStorageProvider2, this), this.GetUniformOrAverageContainerSize(itemStorageProvider2, this), 0, itemCount, out num2);
				num += num2;
			}
			return num;
		}

		// Token: 0x0600783D RID: 30781 RVA: 0x002FFD98 File Offset: 0x002FED98
		private DependencyObject FindDirectDescendentOfItemsHost(Panel itemsHost, DependencyObject child)
		{
			if (itemsHost == null || !itemsHost.IsVisible)
			{
				return null;
			}
			for (DependencyObject parent = VisualTreeHelper.GetParent(child); parent != itemsHost; parent = VisualTreeHelper.GetParent(child))
			{
				child = parent;
				if (child == null)
				{
					break;
				}
			}
			return child;
		}

		// Token: 0x0600783E RID: 30782 RVA: 0x002FFDD0 File Offset: 0x002FEDD0
		private void MakeVisiblePhysicalHelper(Rect r, ref Vector newOffset, ref Rect newRect, bool isHorizontal, ref bool alignTop, ref bool alignBottom)
		{
			double num;
			double num2;
			double num3;
			double num4;
			if (isHorizontal)
			{
				num = this._scrollData._computedOffset.X;
				num2 = this.ViewportWidth;
				num3 = r.X;
				num4 = r.Width;
			}
			else
			{
				num = this._scrollData._computedOffset.Y;
				num2 = this.ViewportHeight;
				num3 = r.Y;
				num4 = r.Height;
			}
			num3 += num;
			double num5 = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(num, num + num2, num3, num3 + num4, ref alignTop, ref alignBottom);
			if (alignTop)
			{
				num3 = num;
			}
			else if (alignBottom)
			{
				num3 = num + num2 - num4;
			}
			double num6 = Math.Max(num3, num5);
			num4 = Math.Max(Math.Min(num4 + num3, num5 + num2) - num6, 0.0);
			num3 = num6;
			num3 -= num;
			if (isHorizontal)
			{
				newOffset.X = num5;
				newRect.X = num3;
				newRect.Width = num4;
				return;
			}
			newOffset.Y = num5;
			newRect.Y = num3;
			newRect.Height = num4;
		}

		// Token: 0x0600783F RID: 30783 RVA: 0x002FFEC0 File Offset: 0x002FEEC0
		private void MakeVisibleLogicalHelper(int childIndex, Rect r, ref Vector newOffset, ref Rect newRect, ref bool alignTop, ref bool alignBottom)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			double num = r.Y;
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
				alignTop = true;
				num = 0.0;
				num4 = childIndex;
			}
			else if (childIndex > num2 + Math.Max(num3 - 1, 0))
			{
				alignBottom = true;
				num4 = childIndex - num3 + 1;
				num = (flag ? base.ActualWidth : base.ActualHeight) * (1.0 - 1.0 / (double)num3);
			}
			if (flag)
			{
				newOffset.X = (double)num4;
				newRect.X = num;
				newRect.Width = r.Width;
				return;
			}
			newOffset.Y = (double)num4;
			newRect.Y = num;
			newRect.Height = r.Height;
		}

		// Token: 0x06007840 RID: 30784 RVA: 0x002FFFCA File Offset: 0x002FEFCA
		private int GetGeneratedIndex(int childIndex)
		{
			return base.Generator.IndexFromGeneratorPosition(new GeneratorPosition(childIndex, 0));
		}

		// Token: 0x06007841 RID: 30785 RVA: 0x002FFFE0 File Offset: 0x002FEFE0
		private double GetMaxChildArrangeLength(IList children, bool isHorizontal)
		{
			double num = 0.0;
			int i = 0;
			int count = children.Count;
			while (i < count)
			{
				Size desiredSize = ((UIElement)children[i]).DesiredSize;
				if (isHorizontal)
				{
					num = Math.Max(num, desiredSize.Height);
				}
				else
				{
					num = Math.Max(num, desiredSize.Width);
				}
				i++;
			}
			return num;
		}

		// Token: 0x06007842 RID: 30786 RVA: 0x0030003E File Offset: 0x002FF03E
		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			VirtualizingStackPanel.ResetScrolling(d as VirtualizingStackPanel);
		}

		// Token: 0x17001BC8 RID: 7112
		// (get) Token: 0x06007843 RID: 30787 RVA: 0x0030004B File Offset: 0x002FF04B
		// (set) Token: 0x06007844 RID: 30788 RVA: 0x00300053 File Offset: 0x002FF053
		private bool HasMeasured
		{
			get
			{
				return base.VSP_HasMeasured;
			}
			set
			{
				base.VSP_HasMeasured = value;
			}
		}

		// Token: 0x17001BC9 RID: 7113
		// (get) Token: 0x06007845 RID: 30789 RVA: 0x0030005C File Offset: 0x002FF05C
		// (set) Token: 0x06007846 RID: 30790 RVA: 0x00300064 File Offset: 0x002FF064
		private bool InRecyclingMode
		{
			get
			{
				return base.VSP_InRecyclingMode;
			}
			set
			{
				base.VSP_InRecyclingMode = value;
			}
		}

		// Token: 0x17001BCA RID: 7114
		// (get) Token: 0x06007847 RID: 30791 RVA: 0x0030006D File Offset: 0x002FF06D
		internal bool IsScrolling
		{
			get
			{
				return this._scrollData != null && this._scrollData._scrollOwner != null;
			}
		}

		// Token: 0x17001BCB RID: 7115
		// (get) Token: 0x06007848 RID: 30792 RVA: 0x00300087 File Offset: 0x002FF087
		// (set) Token: 0x06007849 RID: 30793 RVA: 0x0030008F File Offset: 0x002FF08F
		internal bool IsPixelBased
		{
			get
			{
				return base.VSP_IsPixelBased;
			}
			set
			{
				base.VSP_IsPixelBased = value;
			}
		}

		// Token: 0x17001BCC RID: 7116
		// (get) Token: 0x0600784A RID: 30794 RVA: 0x00300098 File Offset: 0x002FF098
		// (set) Token: 0x0600784B RID: 30795 RVA: 0x003000A0 File Offset: 0x002FF0A0
		internal bool MustDisableVirtualization
		{
			get
			{
				return base.VSP_MustDisableVirtualization;
			}
			set
			{
				base.VSP_MustDisableVirtualization = value;
			}
		}

		// Token: 0x17001BCD RID: 7117
		// (get) Token: 0x0600784C RID: 30796 RVA: 0x003000A9 File Offset: 0x002FF0A9
		// (set) Token: 0x0600784D RID: 30797 RVA: 0x003000BE File Offset: 0x002FF0BE
		internal bool MeasureCaches
		{
			get
			{
				return base.VSP_MeasureCaches || !this.IsVirtualizing;
			}
			set
			{
				base.VSP_MeasureCaches = value;
			}
		}

		// Token: 0x17001BCE RID: 7118
		// (get) Token: 0x0600784E RID: 30798 RVA: 0x003000C7 File Offset: 0x002FF0C7
		// (set) Token: 0x0600784F RID: 30799 RVA: 0x003000CF File Offset: 0x002FF0CF
		private bool IsVirtualizing
		{
			get
			{
				return base.VSP_IsVirtualizing;
			}
			set
			{
				if (!base.IsItemsHost || !value)
				{
					this._realizedChildren = null;
				}
				base.VSP_IsVirtualizing = value;
			}
		}

		// Token: 0x17001BCF RID: 7119
		// (get) Token: 0x06007850 RID: 30800 RVA: 0x003000E9 File Offset: 0x002FF0E9
		// (set) Token: 0x06007851 RID: 30801 RVA: 0x003000F2 File Offset: 0x002FF0F2
		private bool HasVirtualizingChildren
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.HasVirtualizingChildren);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.HasVirtualizingChildren, value);
			}
		}

		// Token: 0x17001BD0 RID: 7120
		// (get) Token: 0x06007852 RID: 30802 RVA: 0x003000FC File Offset: 0x002FF0FC
		// (set) Token: 0x06007853 RID: 30803 RVA: 0x00300105 File Offset: 0x002FF105
		private bool AlignTopOfBringIntoViewContainer
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.AlignTopOfBringIntoViewContainer);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.AlignTopOfBringIntoViewContainer, value);
			}
		}

		// Token: 0x17001BD1 RID: 7121
		// (get) Token: 0x06007854 RID: 30804 RVA: 0x0030010F File Offset: 0x002FF10F
		// (set) Token: 0x06007855 RID: 30805 RVA: 0x00300119 File Offset: 0x002FF119
		private bool AlignBottomOfBringIntoViewContainer
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.AlignBottomOfBringIntoViewContainer);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.AlignBottomOfBringIntoViewContainer, value);
			}
		}

		// Token: 0x17001BD2 RID: 7122
		// (get) Token: 0x06007856 RID: 30806 RVA: 0x00300124 File Offset: 0x002FF124
		// (set) Token: 0x06007857 RID: 30807 RVA: 0x0030012D File Offset: 0x002FF12D
		private bool WasLastMeasurePassAnchored
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.WasLastMeasurePassAnchored);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.WasLastMeasurePassAnchored, value);
			}
		}

		// Token: 0x17001BD3 RID: 7123
		// (get) Token: 0x06007858 RID: 30808 RVA: 0x00300137 File Offset: 0x002FF137
		// (set) Token: 0x06007859 RID: 30809 RVA: 0x00300140 File Offset: 0x002FF140
		private bool ItemsChangedDuringMeasure
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.ItemsChangedDuringMeasure);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.ItemsChangedDuringMeasure, value);
			}
		}

		// Token: 0x17001BD4 RID: 7124
		// (get) Token: 0x0600785A RID: 30810 RVA: 0x0030014A File Offset: 0x002FF14A
		// (set) Token: 0x0600785B RID: 30811 RVA: 0x00300154 File Offset: 0x002FF154
		private bool IsScrollActive
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.IsScrollActive);
			}
			set
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					bool boolField = this.GetBoolField(VirtualizingStackPanel.BoolField.IsScrollActive);
					if (value != boolField)
					{
						VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.IsScrollActive, new object[]
						{
							value
						});
					}
				}
				this.SetBoolField(VirtualizingStackPanel.BoolField.IsScrollActive, value);
				if (!value)
				{
					this._scrollData.HorizontalScrollType = VirtualizingStackPanel.ScrollType.None;
					this._scrollData.VerticalScrollType = VirtualizingStackPanel.ScrollType.None;
				}
			}
		}

		// Token: 0x17001BD5 RID: 7125
		// (get) Token: 0x0600785C RID: 30812 RVA: 0x003001B8 File Offset: 0x002FF1B8
		// (set) Token: 0x0600785D RID: 30813 RVA: 0x003001C2 File Offset: 0x002FF1C2
		internal bool IgnoreMaxDesiredSize
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.IgnoreMaxDesiredSize);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.IgnoreMaxDesiredSize, value);
			}
		}

		// Token: 0x17001BD6 RID: 7126
		// (get) Token: 0x0600785E RID: 30814 RVA: 0x003001CD File Offset: 0x002FF1CD
		// (set) Token: 0x0600785F RID: 30815 RVA: 0x003001DA File Offset: 0x002FF1DA
		private bool IsMeasureCachesPending
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.IsMeasureCachesPending);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.IsMeasureCachesPending, value);
			}
		}

		// Token: 0x17001BD7 RID: 7127
		// (get) Token: 0x06007860 RID: 30816 RVA: 0x003001E8 File Offset: 0x002FF1E8
		// (set) Token: 0x06007861 RID: 30817 RVA: 0x003001F0 File Offset: 0x002FF1F0
		private bool? AreContainersUniformlySized { get; set; }

		// Token: 0x17001BD8 RID: 7128
		// (get) Token: 0x06007862 RID: 30818 RVA: 0x003001F9 File Offset: 0x002FF1F9
		// (set) Token: 0x06007863 RID: 30819 RVA: 0x00300201 File Offset: 0x002FF201
		private double? UniformOrAverageContainerSize { get; set; }

		// Token: 0x17001BD9 RID: 7129
		// (get) Token: 0x06007864 RID: 30820 RVA: 0x0030020A File Offset: 0x002FF20A
		// (set) Token: 0x06007865 RID: 30821 RVA: 0x00300212 File Offset: 0x002FF212
		private double? UniformOrAverageContainerPixelSize { get; set; }

		// Token: 0x17001BDA RID: 7130
		// (get) Token: 0x06007866 RID: 30822 RVA: 0x0030021B File Offset: 0x002FF21B
		private IList RealizedChildren
		{
			get
			{
				if (this.IsVirtualizing && this.InRecyclingMode)
				{
					this.EnsureRealizedChildren();
					return this._realizedChildren;
				}
				return base.InternalChildren;
			}
		}

		// Token: 0x17001BDB RID: 7131
		// (get) Token: 0x06007867 RID: 30823 RVA: 0x00300240 File Offset: 0x002FF240
		internal static bool IsVSP45Compat
		{
			get
			{
				return FrameworkCompatibilityPreferences.GetVSP45Compat();
			}
		}

		// Token: 0x17001BDC RID: 7132
		// (get) Token: 0x06007868 RID: 30824 RVA: 0x00300247 File Offset: 0x002FF247
		bool IStackMeasure.IsScrolling
		{
			get
			{
				return this.IsScrolling;
			}
		}

		// Token: 0x17001BDD RID: 7133
		// (get) Token: 0x06007869 RID: 30825 RVA: 0x002D7272 File Offset: 0x002D6272
		UIElementCollection IStackMeasure.InternalChildren
		{
			get
			{
				return base.InternalChildren;
			}
		}

		// Token: 0x0600786A RID: 30826 RVA: 0x0030024F File Offset: 0x002FF24F
		void IStackMeasure.OnScrollChange()
		{
			this.OnScrollChange();
		}

		// Token: 0x17001BDE RID: 7134
		// (get) Token: 0x0600786B RID: 30827 RVA: 0x00300257 File Offset: 0x002FF257
		private DependencyObject BringIntoViewLeafContainer
		{
			get
			{
				VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
				return ((scrollData != null) ? scrollData._bringIntoViewLeafContainer : null) ?? null;
			}
		}

		// Token: 0x17001BDF RID: 7135
		// (get) Token: 0x0600786C RID: 30828 RVA: 0x00300270 File Offset: 0x002FF270
		private FrameworkElement FirstContainerInViewport
		{
			get
			{
				VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
				return ((scrollData != null) ? scrollData._firstContainerInViewport : null) ?? null;
			}
		}

		// Token: 0x17001BE0 RID: 7136
		// (get) Token: 0x0600786D RID: 30829 RVA: 0x00300289 File Offset: 0x002FF289
		private double FirstContainerOffsetFromViewport
		{
			get
			{
				VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
				if (scrollData == null)
				{
					return 0.0;
				}
				return scrollData._firstContainerOffsetFromViewport;
			}
		}

		// Token: 0x17001BE1 RID: 7137
		// (get) Token: 0x0600786E RID: 30830 RVA: 0x003002A4 File Offset: 0x002FF2A4
		private double ExpectedDistanceBetweenViewports
		{
			get
			{
				VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
				if (scrollData == null)
				{
					return 0.0;
				}
				return scrollData._expectedDistanceBetweenViewports;
			}
		}

		// Token: 0x17001BE2 RID: 7138
		// (get) Token: 0x0600786F RID: 30831 RVA: 0x002E518A File Offset: 0x002E418A
		private bool CanMouseWheelVerticallyScroll
		{
			get
			{
				return SystemParameters.WheelScrollLines > 0;
			}
		}

		// Token: 0x06007870 RID: 30832 RVA: 0x003002BF File Offset: 0x002FF2BF
		private bool GetBoolField(VirtualizingStackPanel.BoolField field)
		{
			return (this._boolFieldStore & field) > ~(VirtualizingStackPanel.BoolField.HasVirtualizingChildren | VirtualizingStackPanel.BoolField.AlignTopOfBringIntoViewContainer | VirtualizingStackPanel.BoolField.WasLastMeasurePassAnchored | VirtualizingStackPanel.BoolField.ItemsChangedDuringMeasure | VirtualizingStackPanel.BoolField.IsScrollActive | VirtualizingStackPanel.BoolField.IgnoreMaxDesiredSize | VirtualizingStackPanel.BoolField.AlignBottomOfBringIntoViewContainer | VirtualizingStackPanel.BoolField.IsMeasureCachesPending);
		}

		// Token: 0x06007871 RID: 30833 RVA: 0x003002CC File Offset: 0x002FF2CC
		private void SetBoolField(VirtualizingStackPanel.BoolField field, bool value)
		{
			if (value)
			{
				this._boolFieldStore |= field;
				return;
			}
			this._boolFieldStore &= ~field;
		}

		// Token: 0x06007872 RID: 30834 RVA: 0x003002F0 File Offset: 0x002FF2F0
		private VirtualizingStackPanel.Snapshot TakeSnapshot()
		{
			VirtualizingStackPanel.Snapshot snapshot = new VirtualizingStackPanel.Snapshot();
			if (this.IsScrolling)
			{
				snapshot._scrollData = new VirtualizingStackPanel.ScrollData();
				snapshot._scrollData._offset = this._scrollData._offset;
				snapshot._scrollData._extent = this._scrollData._extent;
				snapshot._scrollData._computedOffset = this._scrollData._computedOffset;
				snapshot._scrollData._viewport = this._scrollData._viewport;
			}
			snapshot._boolFieldStore = this._boolFieldStore;
			snapshot._areContainersUniformlySized = this.AreContainersUniformlySized;
			snapshot._firstItemInExtendedViewportChildIndex = this._firstItemInExtendedViewportChildIndex;
			snapshot._firstItemInExtendedViewportIndex = this._firstItemInExtendedViewportIndex;
			snapshot._firstItemInExtendedViewportOffset = this._firstItemInExtendedViewportOffset;
			snapshot._actualItemsInExtendedViewportCount = this._actualItemsInExtendedViewportCount;
			snapshot._viewport = this._viewport;
			snapshot._itemsInExtendedViewportCount = this._itemsInExtendedViewportCount;
			snapshot._extendedViewport = this._extendedViewport;
			snapshot._previousStackPixelSizeInViewport = this._previousStackPixelSizeInViewport;
			snapshot._previousStackLogicalSizeInViewport = this._previousStackLogicalSizeInViewport;
			snapshot._previousStackPixelSizeInCacheBeforeViewport = this._previousStackPixelSizeInCacheBeforeViewport;
			snapshot._firstContainerInViewport = this.FirstContainerInViewport;
			snapshot._firstContainerOffsetFromViewport = this.FirstContainerOffsetFromViewport;
			snapshot._expectedDistanceBetweenViewports = this.ExpectedDistanceBetweenViewports;
			snapshot._bringIntoViewContainer = this._bringIntoViewContainer;
			snapshot._bringIntoViewLeafContainer = this.BringIntoViewLeafContainer;
			VirtualizingStackPanel.SnapshotData value = VirtualizingStackPanel.SnapshotDataField.GetValue(this);
			if (value != null)
			{
				snapshot._uniformOrAverageContainerSize = new double?(value.UniformOrAverageContainerSize);
				snapshot._uniformOrAverageContainerPixelSize = new double?(value.UniformOrAverageContainerPixelSize);
				snapshot._effectiveOffsets = value.EffectiveOffsets;
				VirtualizingStackPanel.SnapshotDataField.ClearValue(this);
			}
			ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
			List<VirtualizingStackPanel.ChildInfo> list = new List<VirtualizingStackPanel.ChildInfo>();
			foreach (object obj in this.RealizedChildren)
			{
				UIElement uielement = (UIElement)obj;
				list.Add(new VirtualizingStackPanel.ChildInfo
				{
					_itemIndex = itemContainerGenerator.IndexFromContainer(uielement, true),
					_desiredSize = uielement.DesiredSize,
					_arrangeRect = uielement.PreviousArrangeRect,
					_inset = (Thickness)uielement.GetValue(VirtualizingStackPanel.ItemsHostInsetProperty)
				});
			}
			snapshot._realizedChildren = list;
			return snapshot;
		}

		// Token: 0x06007873 RID: 30835 RVA: 0x0030053C File Offset: 0x002FF53C
		private string ContainerPath(DependencyObject container)
		{
			if (container == null)
			{
				return string.Empty;
			}
			VirtualizingStackPanel virtualizingStackPanel = VisualTreeHelper.GetParent(container) as VirtualizingStackPanel;
			if (virtualizingStackPanel == null)
			{
				return "{Disconnected}";
			}
			if (virtualizingStackPanel == this)
			{
				ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
				return string.Format(CultureInfo.InvariantCulture, "{0}", itemContainerGenerator.IndexFromContainer(container, true));
			}
			int num = (virtualizingStackPanel.Generator as ItemContainerGenerator).IndexFromContainer(container, true);
			DependencyObject dependencyObject = ItemsControl.ContainerFromElement(null, virtualizingStackPanel);
			if (dependencyObject == null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}", num);
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", this.ContainerPath(dependencyObject), num);
		}

		// Token: 0x040038DF RID: 14559
		private static readonly DependencyProperty ContainerSizeProperty = DependencyProperty.Register("ContainerSize", typeof(Size), typeof(VirtualizingStackPanel));

		// Token: 0x040038E0 RID: 14560
		private static readonly DependencyProperty ContainerSizeDualProperty = DependencyProperty.Register("ContainerSizeDual", typeof(VirtualizingStackPanel.ContainerSizeDual), typeof(VirtualizingStackPanel));

		// Token: 0x040038E1 RID: 14561
		private static readonly DependencyProperty AreContainersUniformlySizedProperty = DependencyProperty.Register("AreContainersUniformlySized", typeof(bool), typeof(VirtualizingStackPanel));

		// Token: 0x040038E2 RID: 14562
		private static readonly DependencyProperty UniformOrAverageContainerSizeProperty = DependencyProperty.Register("UniformOrAverageContainerSize", typeof(double), typeof(VirtualizingStackPanel));

		// Token: 0x040038E3 RID: 14563
		private static readonly DependencyProperty UniformOrAverageContainerSizeDualProperty = DependencyProperty.Register("UniformOrAverageContainerSizeDual", typeof(VirtualizingStackPanel.UniformOrAverageContainerSizeDual), typeof(VirtualizingStackPanel));

		// Token: 0x040038E4 RID: 14564
		internal static readonly DependencyProperty ItemsHostInsetProperty = DependencyProperty.Register("ItemsHostInset", typeof(Thickness), typeof(VirtualizingStackPanel));

		// Token: 0x040038E5 RID: 14565
		public new static readonly DependencyProperty IsVirtualizingProperty = VirtualizingPanel.IsVirtualizingProperty;

		// Token: 0x040038E6 RID: 14566
		public new static readonly DependencyProperty VirtualizationModeProperty = VirtualizingPanel.VirtualizationModeProperty;

		// Token: 0x040038E7 RID: 14567
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(VirtualizingStackPanel), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingStackPanel.OnOrientationChanged)), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		// Token: 0x040038E8 RID: 14568
		public static readonly RoutedEvent CleanUpVirtualizedItemEvent = EventManager.RegisterRoutedEvent("CleanUpVirtualizedItemEvent", RoutingStrategy.Direct, typeof(CleanUpVirtualizedItemEventHandler), typeof(VirtualizingStackPanel));

		// Token: 0x040038EC RID: 14572
		private VirtualizingStackPanel.BoolField _boolFieldStore;

		// Token: 0x040038ED RID: 14573
		private VirtualizingStackPanel.ScrollData _scrollData;

		// Token: 0x040038EE RID: 14574
		private int _firstItemInExtendedViewportChildIndex;

		// Token: 0x040038EF RID: 14575
		private int _firstItemInExtendedViewportIndex;

		// Token: 0x040038F0 RID: 14576
		private double _firstItemInExtendedViewportOffset;

		// Token: 0x040038F1 RID: 14577
		private int _actualItemsInExtendedViewportCount;

		// Token: 0x040038F2 RID: 14578
		private Rect _viewport;

		// Token: 0x040038F3 RID: 14579
		private int _itemsInExtendedViewportCount;

		// Token: 0x040038F4 RID: 14580
		private Rect _extendedViewport;

		// Token: 0x040038F5 RID: 14581
		private Size _previousStackPixelSizeInViewport;

		// Token: 0x040038F6 RID: 14582
		private Size _previousStackLogicalSizeInViewport;

		// Token: 0x040038F7 RID: 14583
		private Size _previousStackPixelSizeInCacheBeforeViewport;

		// Token: 0x040038F8 RID: 14584
		private double _pixelDistanceToFirstContainerInExtendedViewport;

		// Token: 0x040038F9 RID: 14585
		private double _pixelDistanceToViewport;

		// Token: 0x040038FA RID: 14586
		private List<UIElement> _realizedChildren;

		// Token: 0x040038FB RID: 14587
		private DispatcherOperation _cleanupOperation;

		// Token: 0x040038FC RID: 14588
		private DispatcherTimer _cleanupDelay;

		// Token: 0x040038FD RID: 14589
		private const int FocusTrail = 5;

		// Token: 0x040038FE RID: 14590
		private DependencyObject _bringIntoViewContainer;

		// Token: 0x040038FF RID: 14591
		private static int[] _indicesStoredInItemValueStorage;

		// Token: 0x04003900 RID: 14592
		private static readonly UncommonField<DispatcherOperation> MeasureCachesOperationField = new UncommonField<DispatcherOperation>();

		// Token: 0x04003901 RID: 14593
		private static readonly UncommonField<DispatcherOperation> AnchorOperationField = new UncommonField<DispatcherOperation>();

		// Token: 0x04003902 RID: 14594
		private static readonly UncommonField<DispatcherOperation> AnchoredInvalidateMeasureOperationField = new UncommonField<DispatcherOperation>();

		// Token: 0x04003903 RID: 14595
		private static readonly UncommonField<DispatcherOperation> ClearIsScrollActiveOperationField = new UncommonField<DispatcherOperation>();

		// Token: 0x04003904 RID: 14596
		private static readonly UncommonField<VirtualizingStackPanel.OffsetInformation> OffsetInformationField = new UncommonField<VirtualizingStackPanel.OffsetInformation>();

		// Token: 0x04003905 RID: 14597
		private static readonly UncommonField<VirtualizingStackPanel.EffectiveOffsetInformation> EffectiveOffsetInformationField = new UncommonField<VirtualizingStackPanel.EffectiveOffsetInformation>();

		// Token: 0x04003906 RID: 14598
		private static readonly UncommonField<VirtualizingStackPanel.SnapshotData> SnapshotDataField = new UncommonField<VirtualizingStackPanel.SnapshotData>();

		// Token: 0x04003907 RID: 14599
		private static UncommonField<VirtualizingStackPanel.FirstContainerInformation> FirstContainerInformationField = new UncommonField<VirtualizingStackPanel.FirstContainerInformation>();

		// Token: 0x04003908 RID: 14600
		private static readonly UncommonField<VirtualizingStackPanel.ScrollTracingInfo> ScrollTracingInfoField = new UncommonField<VirtualizingStackPanel.ScrollTracingInfo>();

		// Token: 0x02000C2E RID: 3118
		[Flags]
		private enum BoolField : byte
		{
			// Token: 0x04004B63 RID: 19299
			HasVirtualizingChildren = 1,
			// Token: 0x04004B64 RID: 19300
			AlignTopOfBringIntoViewContainer = 2,
			// Token: 0x04004B65 RID: 19301
			WasLastMeasurePassAnchored = 4,
			// Token: 0x04004B66 RID: 19302
			ItemsChangedDuringMeasure = 8,
			// Token: 0x04004B67 RID: 19303
			IsScrollActive = 16,
			// Token: 0x04004B68 RID: 19304
			IgnoreMaxDesiredSize = 32,
			// Token: 0x04004B69 RID: 19305
			AlignBottomOfBringIntoViewContainer = 64,
			// Token: 0x04004B6A RID: 19306
			IsMeasureCachesPending = 128
		}

		// Token: 0x02000C2F RID: 3119
		private enum ScrollType
		{
			// Token: 0x04004B6C RID: 19308
			None,
			// Token: 0x04004B6D RID: 19309
			Relative,
			// Token: 0x04004B6E RID: 19310
			Absolute,
			// Token: 0x04004B6F RID: 19311
			ToEnd
		}

		// Token: 0x02000C30 RID: 3120
		private class ScrollData : IStackMeasureScrollData
		{
			// Token: 0x060090D2 RID: 37074 RVA: 0x003476D4 File Offset: 0x003466D4
			internal void ClearLayout()
			{
				this._offset = default(Vector);
				this._viewport = (this._extent = (this._maxDesiredSize = default(Size)));
			}

			// Token: 0x17001FAC RID: 8108
			// (get) Token: 0x060090D3 RID: 37075 RVA: 0x00347710 File Offset: 0x00346710
			internal bool IsEmpty
			{
				get
				{
					return this._offset.X == 0.0 && this._offset.Y == 0.0 && this._viewport.Width == 0.0 && this._viewport.Height == 0.0 && this._extent.Width == 0.0 && this._extent.Height == 0.0 && this._maxDesiredSize.Width == 0.0 && this._maxDesiredSize.Height == 0.0;
				}
			}

			// Token: 0x17001FAD RID: 8109
			// (get) Token: 0x060090D4 RID: 37076 RVA: 0x003477D5 File Offset: 0x003467D5
			// (set) Token: 0x060090D5 RID: 37077 RVA: 0x003477DD File Offset: 0x003467DD
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

			// Token: 0x17001FAE RID: 8110
			// (get) Token: 0x060090D6 RID: 37078 RVA: 0x003477E6 File Offset: 0x003467E6
			// (set) Token: 0x060090D7 RID: 37079 RVA: 0x003477EE File Offset: 0x003467EE
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

			// Token: 0x17001FAF RID: 8111
			// (get) Token: 0x060090D8 RID: 37080 RVA: 0x003477F7 File Offset: 0x003467F7
			// (set) Token: 0x060090D9 RID: 37081 RVA: 0x003477FF File Offset: 0x003467FF
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

			// Token: 0x17001FB0 RID: 8112
			// (get) Token: 0x060090DA RID: 37082 RVA: 0x00347808 File Offset: 0x00346808
			// (set) Token: 0x060090DB RID: 37083 RVA: 0x00347810 File Offset: 0x00346810
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

			// Token: 0x060090DC RID: 37084 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			public void SetPhysicalViewport(double value)
			{
			}

			// Token: 0x17001FB1 RID: 8113
			// (get) Token: 0x060090DD RID: 37085 RVA: 0x00347819 File Offset: 0x00346819
			// (set) Token: 0x060090DE RID: 37086 RVA: 0x00347821 File Offset: 0x00346821
			public VirtualizingStackPanel.ScrollType HorizontalScrollType { get; set; }

			// Token: 0x17001FB2 RID: 8114
			// (get) Token: 0x060090DF RID: 37087 RVA: 0x0034782A File Offset: 0x0034682A
			// (set) Token: 0x060090E0 RID: 37088 RVA: 0x00347832 File Offset: 0x00346832
			public VirtualizingStackPanel.ScrollType VerticalScrollType { get; set; }

			// Token: 0x060090E1 RID: 37089 RVA: 0x0034783C File Offset: 0x0034683C
			public void SetHorizontalScrollType(double oldOffset, double newOffset)
			{
				if (DoubleUtil.GreaterThanOrClose(newOffset, this._extent.Width - this._viewport.Width))
				{
					this.HorizontalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
					return;
				}
				if (DoubleUtil.GreaterThan(Math.Abs(newOffset - oldOffset), this._viewport.Width))
				{
					this.HorizontalScrollType = VirtualizingStackPanel.ScrollType.Absolute;
					return;
				}
				if (this.HorizontalScrollType == VirtualizingStackPanel.ScrollType.None)
				{
					this.HorizontalScrollType = VirtualizingStackPanel.ScrollType.Relative;
				}
			}

			// Token: 0x060090E2 RID: 37090 RVA: 0x003478A4 File Offset: 0x003468A4
			public void SetVerticalScrollType(double oldOffset, double newOffset)
			{
				if (DoubleUtil.GreaterThanOrClose(newOffset, this._extent.Height - this._viewport.Height))
				{
					this.VerticalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
					return;
				}
				if (DoubleUtil.GreaterThan(Math.Abs(newOffset - oldOffset), this._viewport.Height))
				{
					this.VerticalScrollType = VirtualizingStackPanel.ScrollType.Absolute;
					return;
				}
				if (this.VerticalScrollType == VirtualizingStackPanel.ScrollType.None)
				{
					this.VerticalScrollType = VirtualizingStackPanel.ScrollType.Relative;
				}
			}

			// Token: 0x04004B70 RID: 19312
			internal bool _allowHorizontal;

			// Token: 0x04004B71 RID: 19313
			internal bool _allowVertical;

			// Token: 0x04004B72 RID: 19314
			internal Vector _offset;

			// Token: 0x04004B73 RID: 19315
			internal Vector _computedOffset = new Vector(0.0, 0.0);

			// Token: 0x04004B74 RID: 19316
			internal Size _viewport;

			// Token: 0x04004B75 RID: 19317
			internal Size _extent;

			// Token: 0x04004B76 RID: 19318
			internal ScrollViewer _scrollOwner;

			// Token: 0x04004B77 RID: 19319
			internal Size _maxDesiredSize;

			// Token: 0x04004B78 RID: 19320
			internal DependencyObject _bringIntoViewLeafContainer;

			// Token: 0x04004B79 RID: 19321
			internal FrameworkElement _firstContainerInViewport;

			// Token: 0x04004B7A RID: 19322
			internal double _firstContainerOffsetFromViewport;

			// Token: 0x04004B7B RID: 19323
			internal double _expectedDistanceBetweenViewports;

			// Token: 0x04004B7C RID: 19324
			internal long _scrollGeneration;
		}

		// Token: 0x02000C31 RID: 3121
		private class OffsetInformation
		{
			// Token: 0x17001FB3 RID: 8115
			// (get) Token: 0x060090E4 RID: 37092 RVA: 0x0034792E File Offset: 0x0034692E
			// (set) Token: 0x060090E5 RID: 37093 RVA: 0x00347936 File Offset: 0x00346936
			public List<double> previouslyMeasuredOffsets { get; set; }

			// Token: 0x17001FB4 RID: 8116
			// (get) Token: 0x060090E6 RID: 37094 RVA: 0x0034793F File Offset: 0x0034693F
			// (set) Token: 0x060090E7 RID: 37095 RVA: 0x00347947 File Offset: 0x00346947
			public double? lastPageSafeOffset { get; set; }

			// Token: 0x17001FB5 RID: 8117
			// (get) Token: 0x060090E8 RID: 37096 RVA: 0x00347950 File Offset: 0x00346950
			// (set) Token: 0x060090E9 RID: 37097 RVA: 0x00347958 File Offset: 0x00346958
			public double? lastPagePixelSize { get; set; }
		}

		// Token: 0x02000C32 RID: 3122
		private class FirstContainerInformation
		{
			// Token: 0x060090EB RID: 37099 RVA: 0x00347961 File Offset: 0x00346961
			public FirstContainerInformation(ref Rect viewport, DependencyObject firstContainer, int firstItemIndex, double firstItemOffset, long scrollGeneration)
			{
				this.Viewport = viewport;
				this.FirstContainer = firstContainer;
				this.FirstItemIndex = firstItemIndex;
				this.FirstItemOffset = firstItemOffset;
				this.ScrollGeneration = scrollGeneration;
			}

			// Token: 0x04004B82 RID: 19330
			public Rect Viewport;

			// Token: 0x04004B83 RID: 19331
			public DependencyObject FirstContainer;

			// Token: 0x04004B84 RID: 19332
			public int FirstItemIndex;

			// Token: 0x04004B85 RID: 19333
			public double FirstItemOffset;

			// Token: 0x04004B86 RID: 19334
			public long ScrollGeneration;
		}

		// Token: 0x02000C33 RID: 3123
		private class ContainerSizeDual : Tuple<Size, Size>
		{
			// Token: 0x060090EC RID: 37100 RVA: 0x00347993 File Offset: 0x00346993
			public ContainerSizeDual(Size pixelSize, Size itemSize) : base(pixelSize, itemSize)
			{
			}

			// Token: 0x17001FB6 RID: 8118
			// (get) Token: 0x060090ED RID: 37101 RVA: 0x0034799D File Offset: 0x0034699D
			public Size PixelSize
			{
				get
				{
					return base.Item1;
				}
			}

			// Token: 0x17001FB7 RID: 8119
			// (get) Token: 0x060090EE RID: 37102 RVA: 0x003479A5 File Offset: 0x003469A5
			public Size ItemSize
			{
				get
				{
					return base.Item2;
				}
			}
		}

		// Token: 0x02000C34 RID: 3124
		private class UniformOrAverageContainerSizeDual : Tuple<double, double>
		{
			// Token: 0x060090EF RID: 37103 RVA: 0x003479AD File Offset: 0x003469AD
			public UniformOrAverageContainerSizeDual(double pixelSize, double itemSize) : base(pixelSize, itemSize)
			{
			}

			// Token: 0x17001FB8 RID: 8120
			// (get) Token: 0x060090F0 RID: 37104 RVA: 0x003479B7 File Offset: 0x003469B7
			public double PixelSize
			{
				get
				{
					return base.Item1;
				}
			}

			// Token: 0x17001FB9 RID: 8121
			// (get) Token: 0x060090F1 RID: 37105 RVA: 0x003479BF File Offset: 0x003469BF
			public double ItemSize
			{
				get
				{
					return base.Item2;
				}
			}
		}

		// Token: 0x02000C35 RID: 3125
		private class EffectiveOffsetInformation
		{
			// Token: 0x17001FBA RID: 8122
			// (get) Token: 0x060090F2 RID: 37106 RVA: 0x003479C7 File Offset: 0x003469C7
			// (set) Token: 0x060090F3 RID: 37107 RVA: 0x003479CF File Offset: 0x003469CF
			public long ScrollGeneration { get; private set; }

			// Token: 0x17001FBB RID: 8123
			// (get) Token: 0x060090F4 RID: 37108 RVA: 0x003479D8 File Offset: 0x003469D8
			// (set) Token: 0x060090F5 RID: 37109 RVA: 0x003479E0 File Offset: 0x003469E0
			public List<double> OffsetList { get; private set; }

			// Token: 0x060090F6 RID: 37110 RVA: 0x003479E9 File Offset: 0x003469E9
			public EffectiveOffsetInformation(long scrollGeneration)
			{
				this.ScrollGeneration = scrollGeneration;
				this.OffsetList = new List<double>(2);
			}
		}

		// Token: 0x02000C36 RID: 3126
		private class ScrollTracer
		{
			// Token: 0x060090F7 RID: 37111 RVA: 0x00347A04 File Offset: 0x00346A04
			static ScrollTracer()
			{
				VirtualizingStackPanel.ScrollTracer._targetName = FrameworkCompatibilityPreferences.GetScrollingTraceTarget();
				VirtualizingStackPanel.ScrollTracer._flushDepth = 0;
				VirtualizingStackPanel.ScrollTracer._luThreshold = 20;
				string scrollingTraceFile = FrameworkCompatibilityPreferences.GetScrollingTraceFile();
				if (!string.IsNullOrEmpty(scrollingTraceFile))
				{
					string[] array = scrollingTraceFile.Split(';', StringSplitOptions.None);
					VirtualizingStackPanel.ScrollTracer._fileName = array[0];
					int flushDepth;
					if (array.Length > 1 && int.TryParse(array[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out flushDepth))
					{
						VirtualizingStackPanel.ScrollTracer._flushDepth = flushDepth;
					}
					int num;
					if (array.Length > 2 && int.TryParse(array[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
					{
						VirtualizingStackPanel.ScrollTracer._luThreshold = ((num <= 0) ? int.MaxValue : num);
					}
				}
				if (VirtualizingStackPanel.ScrollTracer._targetName != null)
				{
					VirtualizingStackPanel.ScrollTracer.Enable();
				}
			}

			// Token: 0x060090F8 RID: 37112 RVA: 0x00347B40 File Offset: 0x00346B40
			private static void Enable()
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled)
				{
					return;
				}
				VirtualizingStackPanel.ScrollTracer._isEnabled = true;
				Application application = Application.Current;
				if (application != null)
				{
					application.Exit += VirtualizingStackPanel.ScrollTracer.OnApplicationExit;
					application.DispatcherUnhandledException += VirtualizingStackPanel.ScrollTracer.OnUnhandledException;
				}
			}

			// Token: 0x17001FBC RID: 8124
			// (get) Token: 0x060090F9 RID: 37113 RVA: 0x00347B88 File Offset: 0x00346B88
			internal static bool IsEnabled
			{
				get
				{
					return VirtualizingStackPanel.ScrollTracer._isEnabled;
				}
			}

			// Token: 0x060090FA RID: 37114 RVA: 0x00347B90 File Offset: 0x00346B90
			internal static bool SetTarget(object o)
			{
				ItemsControl itemsControl = o as ItemsControl;
				if (itemsControl != null || o == null)
				{
					List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
					lock (obj)
					{
						VirtualizingStackPanel.ScrollTracer.CloseAllTraceLists();
						if (itemsControl != null)
						{
							VirtualizingStackPanel.ScrollTracer.Enable();
							VirtualizingStackPanel.ScrollTracer.AddToMap(itemsControl);
							VirtualizingStackPanel.ScrollTracingInfo nullInfo = VirtualizingStackPanel.ScrollTracer._nullInfo;
							int generation = nullInfo.Generation + 1;
							nullInfo.Generation = generation;
						}
					}
				}
				return itemsControl == o;
			}

			// Token: 0x060090FB RID: 37115 RVA: 0x0012F160 File Offset: 0x0012E160
			internal static void SetFileAndDepth(string filename, int flushDepth)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060090FC RID: 37116 RVA: 0x00347C04 File Offset: 0x00346C04
			private static void Flush()
			{
				List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
				lock (obj)
				{
					int i = 0;
					int count = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count;
					while (i < count)
					{
						VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2.Flush(-1);
						i++;
					}
				}
			}

			// Token: 0x060090FD RID: 37117 RVA: 0x00347C6C File Offset: 0x00346C6C
			private static void Mark(params object[] args)
			{
				VirtualizingStackPanel.ScrollTraceRecord record = new VirtualizingStackPanel.ScrollTraceRecord(VirtualizingStackPanel.ScrollTraceOp.Mark, null, -1, 0, 0, VirtualizingStackPanel.ScrollTracer.BuildDetail(args));
				List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
				lock (obj)
				{
					int i = 0;
					int count = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count;
					while (i < count)
					{
						VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2.Add(record);
						i++;
					}
				}
			}

			// Token: 0x060090FE RID: 37118 RVA: 0x00347CE8 File Offset: 0x00346CE8
			internal static bool IsConfigured(VirtualizingStackPanel vsp)
			{
				return VirtualizingStackPanel.ScrollTracingInfoField.GetValue(vsp) != null;
			}

			// Token: 0x060090FF RID: 37119 RVA: 0x00347CF8 File Offset: 0x00346CF8
			internal static void ConfigureTracing(VirtualizingStackPanel vsp, DependencyObject itemsOwner, object parentItem, ItemsControl itemsControl)
			{
				VirtualizingStackPanel.ScrollTracer scrollTracer = null;
				VirtualizingStackPanel.ScrollTracingInfo value = VirtualizingStackPanel.ScrollTracer._nullInfo;
				VirtualizingStackPanel.ScrollTracingInfo scrollTracingInfo = VirtualizingStackPanel.ScrollTracingInfoField.GetValue(vsp);
				if (scrollTracingInfo != null && scrollTracingInfo.Generation < VirtualizingStackPanel.ScrollTracer._nullInfo.Generation)
				{
					scrollTracingInfo = null;
				}
				if (parentItem == vsp)
				{
					if (scrollTracingInfo == null)
					{
						if (itemsOwner == itemsControl)
						{
							VirtualizingStackPanel.ScrollTracer.TraceList traceList = VirtualizingStackPanel.ScrollTracer.TraceListForItemsControl(itemsControl);
							if (traceList != null)
							{
								scrollTracer = new VirtualizingStackPanel.ScrollTracer(itemsControl, vsp, traceList);
							}
						}
						if (scrollTracer != null)
						{
							value = new VirtualizingStackPanel.ScrollTracingInfo(scrollTracer, VirtualizingStackPanel.ScrollTracer._nullInfo.Generation, 0, itemsOwner as FrameworkElement, null, null, 0);
						}
					}
				}
				else
				{
					VirtualizingStackPanel virtualizingStackPanel = VisualTreeHelper.GetParent(itemsOwner) as VirtualizingStackPanel;
					if (virtualizingStackPanel != null)
					{
						VirtualizingStackPanel.ScrollTracingInfo value2 = VirtualizingStackPanel.ScrollTracingInfoField.GetValue(virtualizingStackPanel);
						if (value2 != null)
						{
							scrollTracer = value2.ScrollTracer;
							if (scrollTracer != null)
							{
								ItemContainerGenerator itemContainerGenerator = virtualizingStackPanel.ItemContainerGenerator as ItemContainerGenerator;
								int num = (itemContainerGenerator != null) ? itemContainerGenerator.IndexFromContainer(itemsOwner, true) : -1;
								if (scrollTracingInfo == null)
								{
									value = new VirtualizingStackPanel.ScrollTracingInfo(scrollTracer, VirtualizingStackPanel.ScrollTracer._nullInfo.Generation, value2.Depth + 1, itemsOwner as FrameworkElement, virtualizingStackPanel, parentItem, num);
								}
								else if (object.Equals(parentItem, scrollTracingInfo.ParentItem))
								{
									if (num != scrollTracingInfo.ItemIndex)
									{
										VirtualizingStackPanel.ScrollTracer.Trace(vsp, VirtualizingStackPanel.ScrollTraceOp.ID, new object[]
										{
											"Index changed from ",
											scrollTracingInfo.ItemIndex,
											" to ",
											num
										});
										scrollTracingInfo.ChangeIndex(num);
									}
								}
								else
								{
									VirtualizingStackPanel.ScrollTracer.Trace(vsp, VirtualizingStackPanel.ScrollTraceOp.ID, new object[]
									{
										"Container recyled from ",
										scrollTracingInfo.ItemIndex,
										" to ",
										num
									});
									scrollTracingInfo.ChangeItem(parentItem);
									scrollTracingInfo.ChangeIndex(num);
								}
							}
						}
					}
				}
				if (scrollTracingInfo == null)
				{
					VirtualizingStackPanel.ScrollTracingInfoField.SetValue(vsp, value);
				}
			}

			// Token: 0x06009100 RID: 37120 RVA: 0x00347EA8 File Offset: 0x00346EA8
			internal static bool IsTracing(VirtualizingStackPanel vsp)
			{
				VirtualizingStackPanel.ScrollTracingInfo value = VirtualizingStackPanel.ScrollTracingInfoField.GetValue(vsp);
				return value != null && value.ScrollTracer != null;
			}

			// Token: 0x06009101 RID: 37121 RVA: 0x00347ED0 File Offset: 0x00346ED0
			internal static void Trace(VirtualizingStackPanel vsp, VirtualizingStackPanel.ScrollTraceOp op, params object[] args)
			{
				VirtualizingStackPanel.ScrollTracingInfo value = VirtualizingStackPanel.ScrollTracingInfoField.GetValue(vsp);
				VirtualizingStackPanel.ScrollTracer scrollTracer = value.ScrollTracer;
				if (VirtualizingStackPanel.ScrollTracer.ShouldIgnore(op, value))
				{
					return;
				}
				scrollTracer.AddTrace(vsp, op, value, args);
			}

			// Token: 0x06009102 RID: 37122 RVA: 0x00347F04 File Offset: 0x00346F04
			private static bool ShouldIgnore(VirtualizingStackPanel.ScrollTraceOp op, VirtualizingStackPanel.ScrollTracingInfo sti)
			{
				return op == VirtualizingStackPanel.ScrollTraceOp.NoOp;
			}

			// Token: 0x06009103 RID: 37123 RVA: 0x00347F0C File Offset: 0x00346F0C
			private static string DisplayType(object o)
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				bool flag2 = false;
				Type type = o.GetType();
				while (!flag2 && type != null)
				{
					if (flag)
					{
						stringBuilder.Append("/");
					}
					string text = type.ToString();
					flag2 = text.StartsWith("System.Windows.Controls.");
					if (flag2)
					{
						text = text.Substring(24);
					}
					stringBuilder.Append(text);
					flag = true;
					type = type.BaseType;
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06009104 RID: 37124 RVA: 0x00347F84 File Offset: 0x00346F84
			private static string BuildDetail(object[] args)
			{
				int num = (args != null) ? args.Length : 0;
				if (num == 0)
				{
					return string.Empty;
				}
				return string.Format(CultureInfo.InvariantCulture, VirtualizingStackPanel.ScrollTracer.s_format[num], args);
			}

			// Token: 0x06009105 RID: 37125 RVA: 0x00347FB6 File Offset: 0x00346FB6
			private void Push()
			{
				this._depth++;
			}

			// Token: 0x06009106 RID: 37126 RVA: 0x00347FC6 File Offset: 0x00346FC6
			private void Pop()
			{
				this._depth--;
			}

			// Token: 0x06009107 RID: 37127 RVA: 0x00347FD6 File Offset: 0x00346FD6
			private void Pop(VirtualizingStackPanel.ScrollTraceRecord record)
			{
				this._depth--;
				record.ChangeOpDepth(-1);
			}

			// Token: 0x06009108 RID: 37128 RVA: 0x00347FED File Offset: 0x00346FED
			private ScrollTracer(ItemsControl itemsControl, VirtualizingStackPanel vsp, VirtualizingStackPanel.ScrollTracer.TraceList traceList)
			{
				this._wrIC = new WeakReference<ItemsControl>(itemsControl);
				this._traceList = traceList;
				this.IdentifyTrace(itemsControl, vsp);
			}

			// Token: 0x06009109 RID: 37129 RVA: 0x00348018 File Offset: 0x00347018
			private static void OnApplicationExit(object sender, ExitEventArgs e)
			{
				Application application = sender as Application;
				if (application != null)
				{
					application.Exit -= VirtualizingStackPanel.ScrollTracer.OnApplicationExit;
				}
				VirtualizingStackPanel.ScrollTracer.CloseAllTraceLists();
			}

			// Token: 0x0600910A RID: 37130 RVA: 0x00348048 File Offset: 0x00347048
			private static void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
			{
				Application application = sender as Application;
				if (application != null)
				{
					application.DispatcherUnhandledException -= VirtualizingStackPanel.ScrollTracer.OnUnhandledException;
				}
				VirtualizingStackPanel.ScrollTracer.CloseAllTraceLists();
			}

			// Token: 0x0600910B RID: 37131 RVA: 0x00348078 File Offset: 0x00347078
			private void IdentifyTrace(ItemsControl ic, VirtualizingStackPanel vsp)
			{
				this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
				{
					VirtualizingStackPanel.ScrollTracer.DisplayType(ic),
					"Items:",
					ic.Items.Count,
					"Panel:",
					VirtualizingStackPanel.ScrollTracer.DisplayType(vsp),
					"Time:",
					DateTime.Now
				});
				this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
				{
					"IsVirt:",
					VirtualizingPanel.GetIsVirtualizing(ic),
					"IsVirtWhenGroup:",
					VirtualizingPanel.GetIsVirtualizingWhenGrouping(ic),
					"VirtMode:",
					VirtualizingPanel.GetVirtualizationMode(ic),
					"ScrollUnit:",
					VirtualizingPanel.GetScrollUnit(ic),
					"CacheLen:",
					VirtualizingPanel.GetCacheLength(ic),
					VirtualizingPanel.GetCacheLengthUnit(ic)
				});
				this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
				{
					"CanContentScroll:",
					ScrollViewer.GetCanContentScroll(ic),
					"IsDeferredScrolling:",
					ScrollViewer.GetIsDeferredScrollingEnabled(ic),
					"PanningMode:",
					ScrollViewer.GetPanningMode(ic),
					"HSBVisibility:",
					ScrollViewer.GetHorizontalScrollBarVisibility(ic),
					"VSBVisibility:",
					ScrollViewer.GetVerticalScrollBarVisibility(ic)
				});
				DataGrid dataGrid = ic as DataGrid;
				if (dataGrid != null)
				{
					this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
					{
						"EnableRowVirt:",
						dataGrid.EnableRowVirtualization,
						"EnableColVirt:",
						dataGrid.EnableColumnVirtualization,
						"Columns:",
						dataGrid.Columns.Count,
						"FrozenCols:",
						dataGrid.FrozenColumnCount
					});
				}
			}

			// Token: 0x0600910C RID: 37132 RVA: 0x00348274 File Offset: 0x00347274
			private void AddTrace(VirtualizingStackPanel vsp, VirtualizingStackPanel.ScrollTraceOp op, VirtualizingStackPanel.ScrollTracingInfo sti, params object[] args)
			{
				if (op == VirtualizingStackPanel.ScrollTraceOp.LayoutUpdated)
				{
					int num = this._luCount + 1;
					this._luCount = num;
					if (num > VirtualizingStackPanel.ScrollTracer._luThreshold)
					{
						this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
						{
							"Inactive at",
							DateTime.Now
						});
						ItemsControl itemsControl;
						if (this._wrIC.TryGetTarget(out itemsControl))
						{
							itemsControl.LayoutUpdated -= this.OnLayoutUpdated;
						}
						this._traceList.FlushAndClear();
						this._luCount = -1;
					}
				}
				else
				{
					int luCount = this._luCount;
					this._luCount = 0;
					if (luCount < 0)
					{
						this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
						{
							"Reactivate at",
							DateTime.Now
						});
						ItemsControl itemsControl2;
						if (this._wrIC.TryGetTarget(out itemsControl2))
						{
							itemsControl2.LayoutUpdated += this.OnLayoutUpdated;
						}
					}
				}
				VirtualizingStackPanel.ScrollTraceRecord scrollTraceRecord = new VirtualizingStackPanel.ScrollTraceRecord(op, vsp, sti.Depth, sti.ItemIndex, this._depth, VirtualizingStackPanel.ScrollTracer.BuildDetail(args));
				this._traceList.Add(scrollTraceRecord);
				switch (op)
				{
				case VirtualizingStackPanel.ScrollTraceOp.BeginMeasure:
					this.Push();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.EndMeasure:
					this.Pop(scrollTraceRecord);
					scrollTraceRecord.Snapshot = vsp.TakeSnapshot();
					this._traceList.Flush(sti.Depth);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.BeginArrange:
					this.Push();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.EndArrange:
					this.Pop(scrollTraceRecord);
					scrollTraceRecord.Snapshot = vsp.TakeSnapshot();
					this._traceList.Flush(sti.Depth);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.BSetAnchor:
					this.Push();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.ESetAnchor:
					this.Pop(scrollTraceRecord);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.BOnAnchor:
					this.Push();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.ROnAnchor:
					this.Pop(scrollTraceRecord);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.SOnAnchor:
					this.Pop();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.EOnAnchor:
					this.Pop(scrollTraceRecord);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.RecycleChildren:
				case VirtualizingStackPanel.ScrollTraceOp.RemoveChildren:
					scrollTraceRecord.RevirtualizedChildren = (args[2] as List<string>);
					break;
				}
				if (VirtualizingStackPanel.ScrollTracer._flushDepth < 0)
				{
					this._traceList.Flush(VirtualizingStackPanel.ScrollTracer._flushDepth);
				}
			}

			// Token: 0x0600910D RID: 37133 RVA: 0x0034847F File Offset: 0x0034747F
			private void OnLayoutUpdated(object sender, EventArgs e)
			{
				this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.LayoutUpdated, VirtualizingStackPanel.ScrollTracer._nullInfo, null);
			}

			// Token: 0x0600910E RID: 37134 RVA: 0x00348490 File Offset: 0x00347490
			private static VirtualizingStackPanel.ScrollTracer.TraceList TraceListForItemsControl(ItemsControl target)
			{
				VirtualizingStackPanel.ScrollTracer.TraceList traceList = null;
				List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
				lock (obj)
				{
					int i = 0;
					int count = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count;
					while (i < count)
					{
						ItemsControl itemsControl;
						if (VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item1.TryGetTarget(out itemsControl) && itemsControl == target)
						{
							traceList = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2;
							break;
						}
						i++;
					}
					if (traceList == null && target.Name == VirtualizingStackPanel.ScrollTracer._targetName)
					{
						traceList = VirtualizingStackPanel.ScrollTracer.AddToMap(target);
					}
				}
				return traceList;
			}

			// Token: 0x0600910F RID: 37135 RVA: 0x00348534 File Offset: 0x00347534
			private static VirtualizingStackPanel.ScrollTracer.TraceList AddToMap(ItemsControl target)
			{
				VirtualizingStackPanel.ScrollTracer.TraceList traceList = null;
				List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
				lock (obj)
				{
					VirtualizingStackPanel.ScrollTracer.PurgeMap();
					VirtualizingStackPanel.ScrollTracer.s_seqno++;
					string text = VirtualizingStackPanel.ScrollTracer._fileName;
					if (string.IsNullOrEmpty(text) || text == "default")
					{
						text = "ScrollTrace.stf";
					}
					if (text != "none" && VirtualizingStackPanel.ScrollTracer.s_seqno > 1)
					{
						int num = text.LastIndexOf(".", StringComparison.Ordinal);
						if (num < 0)
						{
							num = text.Length;
						}
						text = text.Substring(0, num) + VirtualizingStackPanel.ScrollTracer.s_seqno.ToString() + text.Substring(num);
					}
					traceList = new VirtualizingStackPanel.ScrollTracer.TraceList(text);
					VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Add(new Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>(new WeakReference<ItemsControl>(target), traceList));
				}
				return traceList;
			}

			// Token: 0x06009110 RID: 37136 RVA: 0x00348610 File Offset: 0x00347610
			private static void CloseAllTraceLists()
			{
				int i = 0;
				int count = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count;
				while (i < count)
				{
					VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2.FlushAndClose();
					i++;
				}
				VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Clear();
			}

			// Token: 0x06009111 RID: 37137 RVA: 0x00348654 File Offset: 0x00347654
			private static void PurgeMap()
			{
				for (int i = 0; i < VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count; i++)
				{
					ItemsControl itemsControl;
					if (!VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item1.TryGetTarget(out itemsControl))
					{
						VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2.FlushAndClose();
						VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.RemoveAt(i);
						i--;
					}
				}
			}

			// Token: 0x04004B89 RID: 19337
			private const int s_StfFormatVersion = 2;

			// Token: 0x04004B8A RID: 19338
			private const int s_MaxTraceRecords = 30000;

			// Token: 0x04004B8B RID: 19339
			private const int s_MinTraceRecords = 5000;

			// Token: 0x04004B8C RID: 19340
			private const int s_DefaultLayoutUpdatedThreshold = 20;

			// Token: 0x04004B8D RID: 19341
			private static string _targetName;

			// Token: 0x04004B8E RID: 19342
			private static bool _isEnabled;

			// Token: 0x04004B8F RID: 19343
			private static string _fileName;

			// Token: 0x04004B90 RID: 19344
			private static int _flushDepth;

			// Token: 0x04004B91 RID: 19345
			private static int _luThreshold;

			// Token: 0x04004B92 RID: 19346
			private static VirtualizingStackPanel.ScrollTracingInfo _nullInfo = new VirtualizingStackPanel.ScrollTracingInfo(null, 0, -1, null, null, null, -1);

			// Token: 0x04004B93 RID: 19347
			private static string[] s_format = new string[]
			{
				"",
				"{0}",
				"{0} {1}",
				"{0} {1} {2}",
				"{0} {1} {2} {3}",
				"{0} {1} {2} {3} {4} ",
				"{0} {1} {2} {3} {4} {5}",
				"{0} {1} {2} {3} {4} {5} {6}",
				"{0} {1} {2} {3} {4} {5} {6} {7}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13}"
			};

			// Token: 0x04004B94 RID: 19348
			private int _depth;

			// Token: 0x04004B95 RID: 19349
			private VirtualizingStackPanel.ScrollTracer.TraceList _traceList;

			// Token: 0x04004B96 RID: 19350
			private WeakReference<ItemsControl> _wrIC;

			// Token: 0x04004B97 RID: 19351
			private int _luCount = -1;

			// Token: 0x04004B98 RID: 19352
			private static List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> s_TargetToTraceListMap = new List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>>();

			// Token: 0x04004B99 RID: 19353
			private static int s_seqno;

			// Token: 0x02000C94 RID: 3220
			private class TraceList
			{
				// Token: 0x06009582 RID: 38274 RVA: 0x0034E021 File Offset: 0x0034D021
				internal TraceList(string filename)
				{
					if (filename != "none")
					{
						this._writer = new BinaryWriter(File.Open(filename, FileMode.Create));
						this._writer.Write(2);
					}
				}

				// Token: 0x06009583 RID: 38275 RVA: 0x0034E05F File Offset: 0x0034D05F
				internal void Add(VirtualizingStackPanel.ScrollTraceRecord record)
				{
					this._traceList.Add(record);
				}

				// Token: 0x06009584 RID: 38276 RVA: 0x0034E070 File Offset: 0x0034D070
				internal void Flush(int depth)
				{
					if (this._writer != null && depth <= VirtualizingStackPanel.ScrollTracer._flushDepth)
					{
						while (this._flushIndex < this._traceList.Count)
						{
							this._traceList[this._flushIndex].Write(this._writer);
							this._flushIndex++;
						}
						this._writer.Flush();
						if (this._flushIndex > 30000)
						{
							int count = this._flushIndex - 5000;
							this._traceList.RemoveRange(0, count);
							this._flushIndex = this._traceList.Count;
						}
					}
				}

				// Token: 0x06009585 RID: 38277 RVA: 0x0034E115 File Offset: 0x0034D115
				internal void FlushAndClose()
				{
					if (this._writer != null)
					{
						this.Flush(VirtualizingStackPanel.ScrollTracer._flushDepth);
						this._writer.Close();
						this._writer = null;
					}
				}

				// Token: 0x06009586 RID: 38278 RVA: 0x0034E13C File Offset: 0x0034D13C
				internal void FlushAndClear()
				{
					if (this._writer != null)
					{
						this.Flush(VirtualizingStackPanel.ScrollTracer._flushDepth);
						this._traceList.Clear();
						this._flushIndex = 0;
					}
				}

				// Token: 0x04004FD3 RID: 20435
				private List<VirtualizingStackPanel.ScrollTraceRecord> _traceList = new List<VirtualizingStackPanel.ScrollTraceRecord>();

				// Token: 0x04004FD4 RID: 20436
				private BinaryWriter _writer;

				// Token: 0x04004FD5 RID: 20437
				private int _flushIndex;
			}
		}

		// Token: 0x02000C37 RID: 3127
		private class ScrollTracingInfo
		{
			// Token: 0x17001FBD RID: 8125
			// (get) Token: 0x06009112 RID: 37138 RVA: 0x003486B3 File Offset: 0x003476B3
			// (set) Token: 0x06009113 RID: 37139 RVA: 0x003486BB File Offset: 0x003476BB
			internal VirtualizingStackPanel.ScrollTracer ScrollTracer { get; private set; }

			// Token: 0x17001FBE RID: 8126
			// (get) Token: 0x06009114 RID: 37140 RVA: 0x003486C4 File Offset: 0x003476C4
			// (set) Token: 0x06009115 RID: 37141 RVA: 0x003486CC File Offset: 0x003476CC
			internal int Generation { get; set; }

			// Token: 0x17001FBF RID: 8127
			// (get) Token: 0x06009116 RID: 37142 RVA: 0x003486D5 File Offset: 0x003476D5
			// (set) Token: 0x06009117 RID: 37143 RVA: 0x003486DD File Offset: 0x003476DD
			internal int Depth { get; private set; }

			// Token: 0x17001FC0 RID: 8128
			// (get) Token: 0x06009118 RID: 37144 RVA: 0x003486E6 File Offset: 0x003476E6
			// (set) Token: 0x06009119 RID: 37145 RVA: 0x003486EE File Offset: 0x003476EE
			internal FrameworkElement Owner { get; private set; }

			// Token: 0x17001FC1 RID: 8129
			// (get) Token: 0x0600911A RID: 37146 RVA: 0x003486F7 File Offset: 0x003476F7
			// (set) Token: 0x0600911B RID: 37147 RVA: 0x003486FF File Offset: 0x003476FF
			internal VirtualizingStackPanel Parent { get; private set; }

			// Token: 0x17001FC2 RID: 8130
			// (get) Token: 0x0600911C RID: 37148 RVA: 0x00348708 File Offset: 0x00347708
			// (set) Token: 0x0600911D RID: 37149 RVA: 0x00348710 File Offset: 0x00347710
			internal object ParentItem { get; private set; }

			// Token: 0x17001FC3 RID: 8131
			// (get) Token: 0x0600911E RID: 37150 RVA: 0x00348719 File Offset: 0x00347719
			// (set) Token: 0x0600911F RID: 37151 RVA: 0x00348721 File Offset: 0x00347721
			internal int ItemIndex { get; private set; }

			// Token: 0x06009120 RID: 37152 RVA: 0x0034872A File Offset: 0x0034772A
			internal ScrollTracingInfo(VirtualizingStackPanel.ScrollTracer tracer, int generation, int depth, FrameworkElement owner, VirtualizingStackPanel parent, object parentItem, int itemIndex)
			{
				this.ScrollTracer = tracer;
				this.Generation = generation;
				this.Depth = depth;
				this.Owner = owner;
				this.Parent = parent;
				this.ParentItem = parentItem;
				this.ItemIndex = itemIndex;
			}

			// Token: 0x06009121 RID: 37153 RVA: 0x00348767 File Offset: 0x00347767
			internal void ChangeItem(object newItem)
			{
				this.ParentItem = newItem;
			}

			// Token: 0x06009122 RID: 37154 RVA: 0x00348770 File Offset: 0x00347770
			internal void ChangeIndex(int newIndex)
			{
				this.ItemIndex = newIndex;
			}
		}

		// Token: 0x02000C38 RID: 3128
		private enum ScrollTraceOp : ushort
		{
			// Token: 0x04004BA2 RID: 19362
			NoOp,
			// Token: 0x04004BA3 RID: 19363
			ID,
			// Token: 0x04004BA4 RID: 19364
			Mark,
			// Token: 0x04004BA5 RID: 19365
			LineUp,
			// Token: 0x04004BA6 RID: 19366
			LineDown,
			// Token: 0x04004BA7 RID: 19367
			LineLeft,
			// Token: 0x04004BA8 RID: 19368
			LineRight,
			// Token: 0x04004BA9 RID: 19369
			PageUp,
			// Token: 0x04004BAA RID: 19370
			PageDown,
			// Token: 0x04004BAB RID: 19371
			PageLeft,
			// Token: 0x04004BAC RID: 19372
			PageRight,
			// Token: 0x04004BAD RID: 19373
			MouseWheelUp,
			// Token: 0x04004BAE RID: 19374
			MouseWheelDown,
			// Token: 0x04004BAF RID: 19375
			MouseWheelLeft,
			// Token: 0x04004BB0 RID: 19376
			MouseWheelRight,
			// Token: 0x04004BB1 RID: 19377
			SetHorizontalOffset,
			// Token: 0x04004BB2 RID: 19378
			SetVerticalOffset,
			// Token: 0x04004BB3 RID: 19379
			SetHOff,
			// Token: 0x04004BB4 RID: 19380
			SetVOff,
			// Token: 0x04004BB5 RID: 19381
			MakeVisible,
			// Token: 0x04004BB6 RID: 19382
			BeginMeasure,
			// Token: 0x04004BB7 RID: 19383
			EndMeasure,
			// Token: 0x04004BB8 RID: 19384
			BeginArrange,
			// Token: 0x04004BB9 RID: 19385
			EndArrange,
			// Token: 0x04004BBA RID: 19386
			LayoutUpdated,
			// Token: 0x04004BBB RID: 19387
			BSetAnchor,
			// Token: 0x04004BBC RID: 19388
			ESetAnchor,
			// Token: 0x04004BBD RID: 19389
			BOnAnchor,
			// Token: 0x04004BBE RID: 19390
			ROnAnchor,
			// Token: 0x04004BBF RID: 19391
			SOnAnchor,
			// Token: 0x04004BC0 RID: 19392
			EOnAnchor,
			// Token: 0x04004BC1 RID: 19393
			RecycleChildren,
			// Token: 0x04004BC2 RID: 19394
			RemoveChildren,
			// Token: 0x04004BC3 RID: 19395
			ItemsChanged,
			// Token: 0x04004BC4 RID: 19396
			IsScrollActive,
			// Token: 0x04004BC5 RID: 19397
			CFCIV,
			// Token: 0x04004BC6 RID: 19398
			CFIVIO,
			// Token: 0x04004BC7 RID: 19399
			SyncAveSize,
			// Token: 0x04004BC8 RID: 19400
			StoreSubstOffset,
			// Token: 0x04004BC9 RID: 19401
			UseSubstOffset,
			// Token: 0x04004BCA RID: 19402
			ReviseArrangeOffset,
			// Token: 0x04004BCB RID: 19403
			SVSDBegin,
			// Token: 0x04004BCC RID: 19404
			AdjustOffset,
			// Token: 0x04004BCD RID: 19405
			ScrollBarChangeVisibility,
			// Token: 0x04004BCE RID: 19406
			RemeasureCycle,
			// Token: 0x04004BCF RID: 19407
			RemeasureEndExpandViewport,
			// Token: 0x04004BD0 RID: 19408
			RemeasureEndChangeOffset,
			// Token: 0x04004BD1 RID: 19409
			RemeasureEndExtentChanged,
			// Token: 0x04004BD2 RID: 19410
			RemeasureRatio,
			// Token: 0x04004BD3 RID: 19411
			RecomputeFirstOffset,
			// Token: 0x04004BD4 RID: 19412
			LastPageSizeChange,
			// Token: 0x04004BD5 RID: 19413
			SVSDEnd,
			// Token: 0x04004BD6 RID: 19414
			SetContainerSize,
			// Token: 0x04004BD7 RID: 19415
			SizeChangeDuringAnchorScroll
		}

		// Token: 0x02000C39 RID: 3129
		private class ScrollTraceRecord
		{
			// Token: 0x06009123 RID: 37155 RVA: 0x00348779 File Offset: 0x00347779
			internal ScrollTraceRecord(VirtualizingStackPanel.ScrollTraceOp op, VirtualizingStackPanel vsp, int vspDepth, int itemIndex, int opDepth, string detail)
			{
				this.Op = op;
				this.VSP = vsp;
				this.VDepth = vspDepth;
				this.ItemIndex = itemIndex;
				this.OpDepth = opDepth;
				this.Detail = detail;
			}

			// Token: 0x17001FC4 RID: 8132
			// (get) Token: 0x06009124 RID: 37156 RVA: 0x003487AE File Offset: 0x003477AE
			// (set) Token: 0x06009125 RID: 37157 RVA: 0x003487B6 File Offset: 0x003477B6
			internal VirtualizingStackPanel.ScrollTraceOp Op { get; private set; }

			// Token: 0x17001FC5 RID: 8133
			// (get) Token: 0x06009126 RID: 37158 RVA: 0x003487BF File Offset: 0x003477BF
			// (set) Token: 0x06009127 RID: 37159 RVA: 0x003487C7 File Offset: 0x003477C7
			internal int OpDepth { get; private set; }

			// Token: 0x17001FC6 RID: 8134
			// (get) Token: 0x06009128 RID: 37160 RVA: 0x003487D0 File Offset: 0x003477D0
			// (set) Token: 0x06009129 RID: 37161 RVA: 0x003487D8 File Offset: 0x003477D8
			internal VirtualizingStackPanel VSP { get; private set; }

			// Token: 0x17001FC7 RID: 8135
			// (get) Token: 0x0600912A RID: 37162 RVA: 0x003487E1 File Offset: 0x003477E1
			// (set) Token: 0x0600912B RID: 37163 RVA: 0x003487E9 File Offset: 0x003477E9
			internal int VDepth { get; private set; }

			// Token: 0x17001FC8 RID: 8136
			// (get) Token: 0x0600912C RID: 37164 RVA: 0x003487F2 File Offset: 0x003477F2
			// (set) Token: 0x0600912D RID: 37165 RVA: 0x003487FA File Offset: 0x003477FA
			internal int ItemIndex { get; private set; }

			// Token: 0x17001FC9 RID: 8137
			// (get) Token: 0x0600912E RID: 37166 RVA: 0x00348803 File Offset: 0x00347803
			// (set) Token: 0x0600912F RID: 37167 RVA: 0x0034880B File Offset: 0x0034780B
			internal string Detail { get; set; }

			// Token: 0x17001FCA RID: 8138
			// (get) Token: 0x06009130 RID: 37168 RVA: 0x00348814 File Offset: 0x00347814
			// (set) Token: 0x06009131 RID: 37169 RVA: 0x00348821 File Offset: 0x00347821
			internal VirtualizingStackPanel.Snapshot Snapshot
			{
				get
				{
					return this._extraData as VirtualizingStackPanel.Snapshot;
				}
				set
				{
					this._extraData = value;
				}
			}

			// Token: 0x17001FCB RID: 8139
			// (get) Token: 0x06009132 RID: 37170 RVA: 0x0034882A File Offset: 0x0034782A
			// (set) Token: 0x06009133 RID: 37171 RVA: 0x00348821 File Offset: 0x00347821
			internal List<string> RevirtualizedChildren
			{
				get
				{
					return this._extraData as List<string>;
				}
				set
				{
					this._extraData = value;
				}
			}

			// Token: 0x06009134 RID: 37172 RVA: 0x00348837 File Offset: 0x00347837
			internal void ChangeOpDepth(int delta)
			{
				this.OpDepth += delta;
			}

			// Token: 0x06009135 RID: 37173 RVA: 0x00348848 File Offset: 0x00347848
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4}", new object[]
				{
					this.OpDepth,
					this.VDepth,
					this.ItemIndex,
					this.Op,
					this.Detail
				});
			}

			// Token: 0x06009136 RID: 37174 RVA: 0x003488AC File Offset: 0x003478AC
			internal void Write(BinaryWriter writer)
			{
				writer.Write((ushort)this.Op);
				writer.Write(this.OpDepth);
				writer.Write(this.VDepth);
				writer.Write(this.ItemIndex);
				writer.Write(this.Detail);
				if (this.Snapshot != null)
				{
					writer.Write(1);
					this.Snapshot.Write(writer, this.VSP);
					return;
				}
				List<string> revirtualizedChildren;
				if ((revirtualizedChildren = this.RevirtualizedChildren) != null)
				{
					int count = revirtualizedChildren.Count;
					writer.Write(2);
					writer.Write(count);
					for (int i = 0; i < count; i++)
					{
						writer.Write(revirtualizedChildren[i]);
					}
					return;
				}
				writer.Write(0);
			}

			// Token: 0x04004BDE RID: 19422
			private object _extraData;
		}

		// Token: 0x02000C3A RID: 3130
		private class Snapshot
		{
			// Token: 0x06009137 RID: 37175 RVA: 0x00348958 File Offset: 0x00347958
			internal void Write(BinaryWriter writer, VirtualizingStackPanel vsp)
			{
				if (this._scrollData == null)
				{
					writer.Write(false);
				}
				else
				{
					writer.Write(true);
					VirtualizingStackPanel.Snapshot.WriteVector(writer, ref this._scrollData._offset);
					VirtualizingStackPanel.Snapshot.WriteSize(writer, ref this._scrollData._extent);
					VirtualizingStackPanel.Snapshot.WriteVector(writer, ref this._scrollData._computedOffset);
				}
				writer.Write((byte)this._boolFieldStore);
				bool? areContainersUniformlySized = this._areContainersUniformlySized;
				bool flag = false;
				writer.Write(!(areContainersUniformlySized.GetValueOrDefault() == flag & areContainersUniformlySized != null));
				writer.Write((this._uniformOrAverageContainerSize != null) ? this._uniformOrAverageContainerSize.Value : -1.0);
				writer.Write((this._uniformOrAverageContainerPixelSize != null) ? this._uniformOrAverageContainerPixelSize.Value : -1.0);
				writer.Write(this._firstItemInExtendedViewportChildIndex);
				writer.Write(this._firstItemInExtendedViewportIndex);
				writer.Write(this._firstItemInExtendedViewportOffset);
				writer.Write(this._actualItemsInExtendedViewportCount);
				VirtualizingStackPanel.Snapshot.WriteRect(writer, ref this._viewport);
				writer.Write(this._itemsInExtendedViewportCount);
				VirtualizingStackPanel.Snapshot.WriteRect(writer, ref this._extendedViewport);
				VirtualizingStackPanel.Snapshot.WriteSize(writer, ref this._previousStackPixelSizeInViewport);
				VirtualizingStackPanel.Snapshot.WriteSize(writer, ref this._previousStackLogicalSizeInViewport);
				VirtualizingStackPanel.Snapshot.WriteSize(writer, ref this._previousStackPixelSizeInCacheBeforeViewport);
				writer.Write(vsp.ContainerPath(this._firstContainerInViewport));
				writer.Write(this._firstContainerOffsetFromViewport);
				writer.Write(this._expectedDistanceBetweenViewports);
				writer.Write(vsp.ContainerPath(this._bringIntoViewContainer));
				writer.Write(vsp.ContainerPath(this._bringIntoViewLeafContainer));
				writer.Write(this._realizedChildren.Count);
				for (int i = 0; i < this._realizedChildren.Count; i++)
				{
					VirtualizingStackPanel.ChildInfo childInfo = this._realizedChildren[i];
					writer.Write(childInfo._itemIndex);
					VirtualizingStackPanel.Snapshot.WriteSize(writer, ref childInfo._desiredSize);
					VirtualizingStackPanel.Snapshot.WriteRect(writer, ref childInfo._arrangeRect);
					VirtualizingStackPanel.Snapshot.WriteThickness(writer, ref childInfo._inset);
				}
				if (this._effectiveOffsets != null)
				{
					writer.Write(this._effectiveOffsets.Count);
					using (List<double>.Enumerator enumerator = this._effectiveOffsets.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							double value = enumerator.Current;
							writer.Write(value);
						}
						return;
					}
				}
				writer.Write(0);
			}

			// Token: 0x06009138 RID: 37176 RVA: 0x00348BC8 File Offset: 0x00347BC8
			private static void WriteRect(BinaryWriter writer, ref Rect rect)
			{
				writer.Write(rect.Left);
				writer.Write(rect.Top);
				writer.Write(rect.Width);
				writer.Write(rect.Height);
			}

			// Token: 0x06009139 RID: 37177 RVA: 0x00348BFA File Offset: 0x00347BFA
			private static void WriteSize(BinaryWriter writer, ref Size size)
			{
				writer.Write(size.Width);
				writer.Write(size.Height);
			}

			// Token: 0x0600913A RID: 37178 RVA: 0x00348C14 File Offset: 0x00347C14
			private static void WriteVector(BinaryWriter writer, ref Vector vector)
			{
				writer.Write(vector.X);
				writer.Write(vector.Y);
			}

			// Token: 0x0600913B RID: 37179 RVA: 0x00348C2E File Offset: 0x00347C2E
			private static void WriteThickness(BinaryWriter writer, ref Thickness thickness)
			{
				writer.Write(thickness.Left);
				writer.Write(thickness.Top);
				writer.Write(thickness.Right);
				writer.Write(thickness.Bottom);
			}

			// Token: 0x04004BDF RID: 19423
			internal VirtualizingStackPanel.ScrollData _scrollData;

			// Token: 0x04004BE0 RID: 19424
			internal VirtualizingStackPanel.BoolField _boolFieldStore;

			// Token: 0x04004BE1 RID: 19425
			internal bool? _areContainersUniformlySized;

			// Token: 0x04004BE2 RID: 19426
			internal double? _uniformOrAverageContainerSize;

			// Token: 0x04004BE3 RID: 19427
			internal double? _uniformOrAverageContainerPixelSize;

			// Token: 0x04004BE4 RID: 19428
			internal List<VirtualizingStackPanel.ChildInfo> _realizedChildren;

			// Token: 0x04004BE5 RID: 19429
			internal int _firstItemInExtendedViewportChildIndex;

			// Token: 0x04004BE6 RID: 19430
			internal int _firstItemInExtendedViewportIndex;

			// Token: 0x04004BE7 RID: 19431
			internal double _firstItemInExtendedViewportOffset;

			// Token: 0x04004BE8 RID: 19432
			internal int _actualItemsInExtendedViewportCount;

			// Token: 0x04004BE9 RID: 19433
			internal Rect _viewport;

			// Token: 0x04004BEA RID: 19434
			internal int _itemsInExtendedViewportCount;

			// Token: 0x04004BEB RID: 19435
			internal Rect _extendedViewport;

			// Token: 0x04004BEC RID: 19436
			internal Size _previousStackPixelSizeInViewport;

			// Token: 0x04004BED RID: 19437
			internal Size _previousStackLogicalSizeInViewport;

			// Token: 0x04004BEE RID: 19438
			internal Size _previousStackPixelSizeInCacheBeforeViewport;

			// Token: 0x04004BEF RID: 19439
			internal FrameworkElement _firstContainerInViewport;

			// Token: 0x04004BF0 RID: 19440
			internal double _firstContainerOffsetFromViewport;

			// Token: 0x04004BF1 RID: 19441
			internal double _expectedDistanceBetweenViewports;

			// Token: 0x04004BF2 RID: 19442
			internal DependencyObject _bringIntoViewContainer;

			// Token: 0x04004BF3 RID: 19443
			internal DependencyObject _bringIntoViewLeafContainer;

			// Token: 0x04004BF4 RID: 19444
			internal List<double> _effectiveOffsets;
		}

		// Token: 0x02000C3B RID: 3131
		private class ChildInfo
		{
			// Token: 0x0600913D RID: 37181 RVA: 0x00348C60 File Offset: 0x00347C60
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} ds: {1} ar: {2} in: {3}", new object[]
				{
					this._itemIndex,
					this._desiredSize,
					this._arrangeRect,
					this._inset
				});
			}

			// Token: 0x04004BF5 RID: 19445
			internal int _itemIndex;

			// Token: 0x04004BF6 RID: 19446
			internal Size _desiredSize;

			// Token: 0x04004BF7 RID: 19447
			internal Rect _arrangeRect;

			// Token: 0x04004BF8 RID: 19448
			internal Thickness _inset;
		}

		// Token: 0x02000C3C RID: 3132
		private class SnapshotData
		{
			// Token: 0x17001FCC RID: 8140
			// (get) Token: 0x0600913F RID: 37183 RVA: 0x00348CBA File Offset: 0x00347CBA
			// (set) Token: 0x06009140 RID: 37184 RVA: 0x00348CC2 File Offset: 0x00347CC2
			internal double UniformOrAverageContainerSize { get; set; }

			// Token: 0x17001FCD RID: 8141
			// (get) Token: 0x06009141 RID: 37185 RVA: 0x00348CCB File Offset: 0x00347CCB
			// (set) Token: 0x06009142 RID: 37186 RVA: 0x00348CD3 File Offset: 0x00347CD3
			internal double UniformOrAverageContainerPixelSize { get; set; }

			// Token: 0x17001FCE RID: 8142
			// (get) Token: 0x06009143 RID: 37187 RVA: 0x00348CDC File Offset: 0x00347CDC
			// (set) Token: 0x06009144 RID: 37188 RVA: 0x00348CE4 File Offset: 0x00347CE4
			internal List<double> EffectiveOffsets { get; set; }
		}
	}
}
