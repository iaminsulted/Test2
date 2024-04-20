using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200057B RID: 1403
	public class ListBoxItemAutomationPeer : SelectorItemAutomationPeer, IScrollItemProvider
	{
		// Token: 0x060044E9 RID: 17641 RVA: 0x00222996 File Offset: 0x00221996
		public ListBoxItemAutomationPeer(object owner, SelectorAutomationPeer selectorAutomationPeer) : base(owner, selectorAutomationPeer)
		{
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x002229A0 File Offset: 0x002219A0
		protected override string GetClassNameCore()
		{
			return "ListBoxItem";
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x0017DBED File Offset: 0x0017CBED
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ListItem;
		}

		// Token: 0x060044EC RID: 17644 RVA: 0x002229A7 File Offset: 0x002219A7
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.ScrollItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x002229B8 File Offset: 0x002219B8
		internal override void RealizeCore()
		{
			ComboBox comboBox = base.ItemsControlAutomationPeer.Owner as ComboBox;
			if (comboBox != null)
			{
				IExpandCollapseProvider expandCollapseProvider = ((IExpandCollapseProvider)UIElementAutomationPeer.FromElement(comboBox)) as ComboBoxAutomationPeer;
				if (expandCollapseProvider.ExpandCollapseState != ExpandCollapseState.Expanded)
				{
					expandCollapseProvider.Expand();
				}
			}
			base.RealizeCore();
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x00222A00 File Offset: 0x00221A00
		void IScrollItemProvider.ScrollIntoView()
		{
			ListBox listBox = base.ItemsControlAutomationPeer.Owner as ListBox;
			if (listBox != null)
			{
				listBox.ScrollIntoView(base.Item);
				return;
			}
			ComboBoxAutomationPeer comboBoxAutomationPeer = base.ItemsControlAutomationPeer as ComboBoxAutomationPeer;
			if (comboBoxAutomationPeer != null)
			{
				comboBoxAutomationPeer.ScrollItemIntoView(base.Item);
			}
		}
	}
}
