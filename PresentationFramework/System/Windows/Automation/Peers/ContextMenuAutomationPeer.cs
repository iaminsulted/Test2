using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200054C RID: 1356
	public class ContextMenuAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060042E0 RID: 17120 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public ContextMenuAutomationPeer(ContextMenu owner) : base(owner)
		{
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x0021CB8E File Offset: 0x0021BB8E
		protected override string GetClassNameCore()
		{
			return "ContextMenu";
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x001FCA9D File Offset: 0x001FBA9D
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Menu;
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
