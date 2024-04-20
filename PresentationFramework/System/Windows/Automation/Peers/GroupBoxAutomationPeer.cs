using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200056B RID: 1387
	public class GroupBoxAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600445E RID: 17502 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public GroupBoxAutomationPeer(GroupBox owner) : base(owner)
		{
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x00220E78 File Offset: 0x0021FE78
		protected override string GetClassNameCore()
		{
			return "GroupBox";
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0021FC39 File Offset: 0x0021EC39
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Group;
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x00220E80 File Offset: 0x0021FE80
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			if (!string.IsNullOrEmpty(nameCore) && ((GroupBox)base.Owner).Header is string)
			{
				return AccessText.RemoveAccessKeyMarker(nameCore);
			}
			return nameCore;
		}
	}
}
