using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200059D RID: 1437
	public class ToolTipAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060045DB RID: 17883 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public ToolTipAutomationPeer(ToolTip owner) : base(owner)
		{
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x00224C75 File Offset: 0x00223C75
		protected override string GetClassNameCore()
		{
			return "ToolTip";
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x00224C7C File Offset: 0x00223C7C
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ToolTip;
		}
	}
}
