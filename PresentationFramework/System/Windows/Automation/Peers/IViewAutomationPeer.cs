using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000578 RID: 1400
	public interface IViewAutomationPeer
	{
		// Token: 0x060044DC RID: 17628
		AutomationControlType GetAutomationControlType();

		// Token: 0x060044DD RID: 17629
		object GetPattern(PatternInterface patternInterface);

		// Token: 0x060044DE RID: 17630
		List<AutomationPeer> GetChildren(List<AutomationPeer> children);

		// Token: 0x060044DF RID: 17631
		ItemAutomationPeer CreateItemAutomationPeer(object item);

		// Token: 0x060044E0 RID: 17632
		void ItemsChanged(NotifyCollectionChangedEventArgs e);

		// Token: 0x060044E1 RID: 17633
		void ViewDetached();
	}
}
