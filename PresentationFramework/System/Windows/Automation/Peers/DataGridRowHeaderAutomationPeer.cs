using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000556 RID: 1366
	public sealed class DataGridRowHeaderAutomationPeer : ButtonBaseAutomationPeer
	{
		// Token: 0x06004387 RID: 17287 RVA: 0x0021BB7E File Offset: 0x0021AB7E
		public DataGridRowHeaderAutomationPeer(DataGridRowHeader owner) : base(owner)
		{
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x001FC275 File Offset: 0x001FB275
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.HeaderItem;
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
