using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200058D RID: 1421
	public class SeparatorAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004579 RID: 17785 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public SeparatorAutomationPeer(Separator owner) : base(owner)
		{
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x00223F79 File Offset: 0x00222F79
		protected override string GetClassNameCore()
		{
			return "Separator";
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x001FBC9D File Offset: 0x001FAC9D
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Separator;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
