using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200057A RID: 1402
	public class ListBoxAutomationPeer : SelectorAutomationPeer
	{
		// Token: 0x060044E6 RID: 17638 RVA: 0x0021C8AC File Offset: 0x0021B8AC
		public ListBoxAutomationPeer(ListBox owner) : base(owner)
		{
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x0021C8B5 File Offset: 0x0021B8B5
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new ListBoxItemAutomationPeer(item, this);
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x0022298F File Offset: 0x0022198F
		protected override string GetClassNameCore()
		{
			return "ListBox";
		}
	}
}
