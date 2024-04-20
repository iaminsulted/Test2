using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000562 RID: 1378
	public class FrameAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004412 RID: 17426 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public FrameAutomationPeer(Frame owner) : base(owner)
		{
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x002201EF File Offset: 0x0021F1EF
		protected override string GetClassNameCore()
		{
			return "Frame";
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x001FC004 File Offset: 0x001FB004
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Pane;
		}
	}
}
