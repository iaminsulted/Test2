using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000577 RID: 1399
	internal class ItemsControlWrapperAutomationPeer : ItemsControlAutomationPeer
	{
		// Token: 0x060044D8 RID: 17624 RVA: 0x0021DF60 File Offset: 0x0021CF60
		public ItemsControlWrapperAutomationPeer(ItemsControl owner) : base(owner)
		{
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x0022293A File Offset: 0x0022193A
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new ItemsControlItemAutomationPeer(item, this);
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x00222943 File Offset: 0x00221943
		protected override string GetClassNameCore()
		{
			return "ItemsControl";
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x001390BC File Offset: 0x001380BC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.List;
		}
	}
}
