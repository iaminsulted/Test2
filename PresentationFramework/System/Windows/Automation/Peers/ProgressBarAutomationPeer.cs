using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000584 RID: 1412
	public class ProgressBarAutomationPeer : RangeBaseAutomationPeer, IRangeValueProvider
	{
		// Token: 0x06004522 RID: 17698 RVA: 0x002230FA File Offset: 0x002220FA
		public ProgressBarAutomationPeer(ProgressBar owner) : base(owner)
		{
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x00223103 File Offset: 0x00222103
		protected override string GetClassNameCore()
		{
			return "ProgressBar";
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x001FCA95 File Offset: 0x001FBA95
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ProgressBar;
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x0022310A File Offset: 0x0022210A
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.RangeValue && ((ProgressBar)base.Owner).IsIndeterminate)
			{
				return null;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x0022312B File Offset: 0x0022212B
		void IRangeValueProvider.SetValue(double val)
		{
			throw new InvalidOperationException(SR.Get("ProgressBarReadOnly"));
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06004527 RID: 17703 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool IRangeValueProvider.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x06004528 RID: 17704 RVA: 0x0022313C File Offset: 0x0022213C
		double IRangeValueProvider.LargeChange
		{
			get
			{
				return double.NaN;
			}
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x06004529 RID: 17705 RVA: 0x0022313C File Offset: 0x0022213C
		double IRangeValueProvider.SmallChange
		{
			get
			{
				return double.NaN;
			}
		}
	}
}
