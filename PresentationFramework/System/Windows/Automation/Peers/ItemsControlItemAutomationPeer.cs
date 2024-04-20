using System;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000576 RID: 1398
	internal class ItemsControlItemAutomationPeer : ItemAutomationPeer
	{
		// Token: 0x060044D5 RID: 17621 RVA: 0x00222929 File Offset: 0x00221929
		public ItemsControlItemAutomationPeer(object item, ItemsControlWrapperAutomationPeer parent) : base(item, parent)
		{
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x001FD61F File Offset: 0x001FC61F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataItem;
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x00222933 File Offset: 0x00221933
		protected override string GetClassNameCore()
		{
			return "ItemsControlItem";
		}
	}
}
