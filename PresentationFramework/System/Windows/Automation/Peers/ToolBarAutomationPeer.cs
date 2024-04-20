using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200059C RID: 1436
	public class ToolBarAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060045D8 RID: 17880 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public ToolBarAutomationPeer(ToolBar owner) : base(owner)
		{
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x00224C6A File Offset: 0x00223C6A
		protected override string GetClassNameCore()
		{
			return "ToolBar";
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x00224C71 File Offset: 0x00223C71
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ToolBar;
		}
	}
}
