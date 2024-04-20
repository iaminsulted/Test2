using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200058E RID: 1422
	public class SliderAutomationPeer : RangeBaseAutomationPeer
	{
		// Token: 0x0600457D RID: 17789 RVA: 0x002230FA File Offset: 0x002220FA
		public SliderAutomationPeer(Slider owner) : base(owner)
		{
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x00223F80 File Offset: 0x00222F80
		protected override string GetClassNameCore()
		{
			return "Slider";
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x001FCB4F File Offset: 0x001FBB4F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Slider;
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x002234D9 File Offset: 0x002224D9
		protected override Point GetClickablePointCore()
		{
			return new Point(double.NaN, double.NaN);
		}
	}
}
