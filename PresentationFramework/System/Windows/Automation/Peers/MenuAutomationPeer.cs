using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200057F RID: 1407
	public class MenuAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060044FD RID: 17661 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public MenuAutomationPeer(Menu owner) : base(owner)
		{
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x00222B3A File Offset: 0x00221B3A
		protected override string GetClassNameCore()
		{
			return "Menu";
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x001FCA9D File Offset: 0x001FBA9D
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Menu;
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
