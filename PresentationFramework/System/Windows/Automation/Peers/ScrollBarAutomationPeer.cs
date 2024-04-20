using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000589 RID: 1417
	public class ScrollBarAutomationPeer : RangeBaseAutomationPeer
	{
		// Token: 0x0600454C RID: 17740 RVA: 0x002230FA File Offset: 0x002220FA
		public ScrollBarAutomationPeer(ScrollBar owner) : base(owner)
		{
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x002234D2 File Offset: 0x002224D2
		protected override string GetClassNameCore()
		{
			return "ScrollBar";
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x001FCA99 File Offset: 0x001FBA99
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ScrollBar;
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x002234D9 File Offset: 0x002224D9
		protected override Point GetClickablePointCore()
		{
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x002234F2 File Offset: 0x002224F2
		protected override AutomationOrientation GetOrientationCore()
		{
			if (((ScrollBar)base.Owner).Orientation != Orientation.Horizontal)
			{
				return AutomationOrientation.Vertical;
			}
			return AutomationOrientation.Horizontal;
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x0022350C File Offset: 0x0022250C
		internal override void SetValueCore(double val)
		{
			double horizontalPercent = -1.0;
			double verticalPercent = -1.0;
			ScrollBar scrollBar = base.Owner as ScrollBar;
			ScrollViewer scrollViewer = scrollBar.TemplatedParent as ScrollViewer;
			if (scrollViewer == null)
			{
				base.SetValueCore(val);
				return;
			}
			if (scrollBar.Orientation == Orientation.Horizontal)
			{
				horizontalPercent = val / (scrollViewer.ExtentWidth - scrollViewer.ViewportWidth) * 100.0;
			}
			else
			{
				verticalPercent = val / (scrollViewer.ExtentHeight - scrollViewer.ViewportHeight) * 100.0;
			}
			((IScrollProvider)(UIElementAutomationPeer.FromElement(scrollViewer) as ScrollViewerAutomationPeer)).SetScrollPercent(horizontalPercent, verticalPercent);
		}
	}
}
