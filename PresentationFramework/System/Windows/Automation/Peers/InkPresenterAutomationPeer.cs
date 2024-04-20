using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000571 RID: 1393
	public class InkPresenterAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600447E RID: 17534 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public InkPresenterAutomationPeer(InkPresenter owner) : base(owner)
		{
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x002214A0 File Offset: 0x002204A0
		protected override string GetClassNameCore()
		{
			return "InkPresenter";
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x001FBDEE File Offset: 0x001FADEE
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
