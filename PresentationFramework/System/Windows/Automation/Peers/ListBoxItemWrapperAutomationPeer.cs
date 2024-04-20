using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200057C RID: 1404
	public class ListBoxItemWrapperAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060044EF RID: 17647 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public ListBoxItemWrapperAutomationPeer(ListBoxItem owner) : base(owner)
		{
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x002229A0 File Offset: 0x002219A0
		protected override string GetClassNameCore()
		{
			return "ListBoxItem";
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x0017DBED File Offset: 0x0017CBED
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ListItem;
		}
	}
}
