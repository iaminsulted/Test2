using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000590 RID: 1424
	public class StatusBarItemAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004587 RID: 17799 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public StatusBarItemAutomationPeer(StatusBarItem owner) : base(owner)
		{
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x0022415E File Offset: 0x0022315E
		protected override string GetClassNameCore()
		{
			return "StatusBarItem";
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x001FB806 File Offset: 0x001FA806
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Text;
		}
	}
}
