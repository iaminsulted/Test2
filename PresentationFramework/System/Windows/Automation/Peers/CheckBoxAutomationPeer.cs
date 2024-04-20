using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000549 RID: 1353
	public class CheckBoxAutomationPeer : ToggleButtonAutomationPeer
	{
		// Token: 0x060042C9 RID: 17097 RVA: 0x0021C89C File Offset: 0x0021B89C
		public CheckBoxAutomationPeer(CheckBox owner) : base(owner)
		{
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x0021C8A5 File Offset: 0x0021B8A5
		protected override string GetClassNameCore()
		{
			return "CheckBox";
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x0010A7E1 File Offset: 0x001097E1
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.CheckBox;
		}
	}
}
