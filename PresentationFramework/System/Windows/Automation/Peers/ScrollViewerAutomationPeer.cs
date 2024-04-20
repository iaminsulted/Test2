using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200058A RID: 1418
	public class ScrollViewerAutomationPeer : FrameworkElementAutomationPeer, IScrollProvider
	{
		// Token: 0x06004553 RID: 17747 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public ScrollViewerAutomationPeer(ScrollViewer owner) : base(owner)
		{
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x002235A0 File Offset: 0x002225A0
		protected override string GetClassNameCore()
		{
			return "ScrollViewer";
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x001FC004 File Offset: 0x001FB004
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Pane;
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x002235A8 File Offset: 0x002225A8
		protected override bool IsControlElementCore()
		{
			DependencyObject templatedParent = ((ScrollViewer)base.Owner).TemplatedParent;
			return (templatedParent == null || templatedParent is ContentPresenter) && base.IsControlElementCore();
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x002235D9 File Offset: 0x002225D9
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Scroll)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x002235E8 File Offset: 0x002225E8
		void IScrollProvider.Scroll(ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			bool flag = horizontalAmount != ScrollAmount.NoAmount;
			bool flag2 = verticalAmount != ScrollAmount.NoAmount;
			ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
			if ((flag && !this.HorizontallyScrollable) || (flag2 && !this.VerticallyScrollable))
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			switch (horizontalAmount)
			{
			case ScrollAmount.LargeDecrement:
				scrollViewer.PageLeft();
				break;
			case ScrollAmount.SmallDecrement:
				scrollViewer.LineLeft();
				break;
			case ScrollAmount.NoAmount:
				break;
			case ScrollAmount.LargeIncrement:
				scrollViewer.PageRight();
				break;
			case ScrollAmount.SmallIncrement:
				scrollViewer.LineRight();
				break;
			default:
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			switch (verticalAmount)
			{
			case ScrollAmount.LargeDecrement:
				scrollViewer.PageUp();
				return;
			case ScrollAmount.SmallDecrement:
				scrollViewer.LineUp();
				return;
			case ScrollAmount.NoAmount:
				return;
			case ScrollAmount.LargeIncrement:
				scrollViewer.PageDown();
				return;
			case ScrollAmount.SmallIncrement:
				scrollViewer.LineDown();
				return;
			default:
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x002236D8 File Offset: 0x002226D8
		void IScrollProvider.SetScrollPercent(double horizontalPercent, double verticalPercent)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			bool flag = horizontalPercent != -1.0;
			bool flag2 = verticalPercent != -1.0;
			ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
			if ((flag && !this.HorizontallyScrollable) || (flag2 && !this.VerticallyScrollable))
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			if ((flag && horizontalPercent < 0.0) || horizontalPercent > 100.0)
			{
				throw new ArgumentOutOfRangeException("horizontalPercent", SR.Get("ScrollViewer_OutOfRange", new object[]
				{
					"horizontalPercent",
					horizontalPercent.ToString(CultureInfo.InvariantCulture),
					"0",
					"100"
				}));
			}
			if ((flag2 && verticalPercent < 0.0) || verticalPercent > 100.0)
			{
				throw new ArgumentOutOfRangeException("verticalPercent", SR.Get("ScrollViewer_OutOfRange", new object[]
				{
					"verticalPercent",
					verticalPercent.ToString(CultureInfo.InvariantCulture),
					"0",
					"100"
				}));
			}
			if (flag)
			{
				scrollViewer.ScrollToHorizontalOffset((scrollViewer.ExtentWidth - scrollViewer.ViewportWidth) * horizontalPercent * 0.01);
			}
			if (flag2)
			{
				scrollViewer.ScrollToVerticalOffset((scrollViewer.ExtentHeight - scrollViewer.ViewportHeight) * verticalPercent * 0.01);
			}
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x0600455A RID: 17754 RVA: 0x00223848 File Offset: 0x00222848
		double IScrollProvider.HorizontalScrollPercent
		{
			get
			{
				if (!this.HorizontallyScrollable)
				{
					return -1.0;
				}
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				return scrollViewer.HorizontalOffset * 100.0 / (scrollViewer.ExtentWidth - scrollViewer.ViewportWidth);
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x0600455B RID: 17755 RVA: 0x00223894 File Offset: 0x00222894
		double IScrollProvider.VerticalScrollPercent
		{
			get
			{
				if (!this.VerticallyScrollable)
				{
					return -1.0;
				}
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				return scrollViewer.VerticalOffset * 100.0 / (scrollViewer.ExtentHeight - scrollViewer.ViewportHeight);
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x0600455C RID: 17756 RVA: 0x002238E0 File Offset: 0x002228E0
		double IScrollProvider.HorizontalViewSize
		{
			get
			{
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				if (scrollViewer.ScrollInfo == null || DoubleUtil.IsZero(scrollViewer.ExtentWidth))
				{
					return 100.0;
				}
				return Math.Min(100.0, scrollViewer.ViewportWidth * 100.0 / scrollViewer.ExtentWidth);
			}
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x0600455D RID: 17757 RVA: 0x00223940 File Offset: 0x00222940
		double IScrollProvider.VerticalViewSize
		{
			get
			{
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				if (scrollViewer.ScrollInfo == null || DoubleUtil.IsZero(scrollViewer.ExtentHeight))
				{
					return 100.0;
				}
				return Math.Min(100.0, scrollViewer.ViewportHeight * 100.0 / scrollViewer.ExtentHeight);
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x0600455E RID: 17758 RVA: 0x0022399E File Offset: 0x0022299E
		bool IScrollProvider.HorizontallyScrollable
		{
			get
			{
				return this.HorizontallyScrollable;
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x0600455F RID: 17759 RVA: 0x002239A6 File Offset: 0x002229A6
		bool IScrollProvider.VerticallyScrollable
		{
			get
			{
				return this.VerticallyScrollable;
			}
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x002239AE File Offset: 0x002229AE
		private static bool AutomationIsScrollable(double extent, double viewport)
		{
			return DoubleUtil.GreaterThan(extent, viewport);
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x002239B7 File Offset: 0x002229B7
		private static double AutomationGetScrollPercent(double extent, double viewport, double actualOffset)
		{
			if (!ScrollViewerAutomationPeer.AutomationIsScrollable(extent, viewport))
			{
				return -1.0;
			}
			return actualOffset * 100.0 / (extent - viewport);
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x002239DC File Offset: 0x002229DC
		private static double AutomationGetViewSize(double extent, double viewport)
		{
			if (DoubleUtil.IsZero(extent))
			{
				return 100.0;
			}
			return Math.Min(100.0, viewport * 100.0 / extent);
		}

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x06004563 RID: 17763 RVA: 0x00223A0C File Offset: 0x00222A0C
		private bool HorizontallyScrollable
		{
			get
			{
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				return scrollViewer.ScrollInfo != null && DoubleUtil.GreaterThan(scrollViewer.ExtentWidth, scrollViewer.ViewportWidth);
			}
		}

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x06004564 RID: 17764 RVA: 0x00223A40 File Offset: 0x00222A40
		private bool VerticallyScrollable
		{
			get
			{
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				return scrollViewer.ScrollInfo != null && DoubleUtil.GreaterThan(scrollViewer.ExtentHeight, scrollViewer.ViewportHeight);
			}
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x00223A74 File Offset: 0x00222A74
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseAutomationEvents(double extentX, double extentY, double viewportX, double viewportY, double offsetX, double offsetY)
		{
			if (ScrollViewerAutomationPeer.AutomationIsScrollable(extentX, viewportX) != ((IScrollProvider)this).HorizontallyScrollable)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.HorizontallyScrollableProperty, ScrollViewerAutomationPeer.AutomationIsScrollable(extentX, viewportX), ((IScrollProvider)this).HorizontallyScrollable);
			}
			if (ScrollViewerAutomationPeer.AutomationIsScrollable(extentY, viewportY) != ((IScrollProvider)this).VerticallyScrollable)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.VerticallyScrollableProperty, ScrollViewerAutomationPeer.AutomationIsScrollable(extentY, viewportY), ((IScrollProvider)this).VerticallyScrollable);
			}
			if (ScrollViewerAutomationPeer.AutomationGetViewSize(extentX, viewportX) != ((IScrollProvider)this).HorizontalViewSize)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.HorizontalViewSizeProperty, ScrollViewerAutomationPeer.AutomationGetViewSize(extentX, viewportX), ((IScrollProvider)this).HorizontalViewSize);
			}
			if (ScrollViewerAutomationPeer.AutomationGetViewSize(extentY, viewportY) != ((IScrollProvider)this).VerticalViewSize)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.VerticalViewSizeProperty, ScrollViewerAutomationPeer.AutomationGetViewSize(extentY, viewportY), ((IScrollProvider)this).VerticalViewSize);
			}
			if (ScrollViewerAutomationPeer.AutomationGetScrollPercent(extentX, viewportX, offsetX) != ((IScrollProvider)this).HorizontalScrollPercent)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.HorizontalScrollPercentProperty, ScrollViewerAutomationPeer.AutomationGetScrollPercent(extentX, viewportX, offsetX), ((IScrollProvider)this).HorizontalScrollPercent);
			}
			if (ScrollViewerAutomationPeer.AutomationGetScrollPercent(extentY, viewportY, offsetY) != ((IScrollProvider)this).VerticalScrollPercent)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.VerticalScrollPercentProperty, ScrollViewerAutomationPeer.AutomationGetScrollPercent(extentY, viewportY, offsetY), ((IScrollProvider)this).VerticalScrollPercent);
			}
		}
	}
}
