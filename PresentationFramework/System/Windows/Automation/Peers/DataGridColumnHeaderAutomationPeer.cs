using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000550 RID: 1360
	public sealed class DataGridColumnHeaderAutomationPeer : ButtonBaseAutomationPeer
	{
		// Token: 0x06004340 RID: 17216 RVA: 0x0021BB7E File Offset: 0x0021AB7E
		public DataGridColumnHeaderAutomationPeer(DataGridColumnHeader owner) : base(owner)
		{
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x001FC275 File Offset: 0x001FB275
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.HeaderItem;
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
