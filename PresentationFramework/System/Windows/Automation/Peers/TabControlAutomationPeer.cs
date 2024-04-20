using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000591 RID: 1425
	public class TabControlAutomationPeer : SelectorAutomationPeer, ISelectionProvider
	{
		// Token: 0x0600458A RID: 17802 RVA: 0x0021C8AC File Offset: 0x0021B8AC
		public TabControlAutomationPeer(TabControl owner) : base(owner)
		{
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x00224165 File Offset: 0x00223165
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new TabItemAutomationPeer(item, this);
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x001FCAF3 File Offset: 0x001FBAF3
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Tab;
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x0022416E File Offset: 0x0022316E
		protected override string GetClassNameCore()
		{
			return "TabControl";
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x002234D9 File Offset: 0x002224D9
		protected override Point GetClickablePointCore()
		{
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x0600458F RID: 17807 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return true;
			}
		}
	}
}
