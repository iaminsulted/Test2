using System;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200055E RID: 1374
	public class FixedPageAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060043F9 RID: 17401 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public FixedPageAutomationPeer(FixedPage owner) : base(owner)
		{
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0021FD75 File Offset: 0x0021ED75
		protected override string GetClassNameCore()
		{
			return "FixedPage";
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x001FC004 File Offset: 0x001FB004
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Pane;
		}
	}
}
