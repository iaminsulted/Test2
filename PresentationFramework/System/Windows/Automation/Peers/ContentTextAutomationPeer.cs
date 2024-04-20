using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200054B RID: 1355
	public abstract class ContentTextAutomationPeer : FrameworkContentElementAutomationPeer
	{
		// Token: 0x060042DC RID: 17116 RVA: 0x0021CB39 File Offset: 0x0021BB39
		protected ContentTextAutomationPeer(FrameworkContentElement owner) : base(owner)
		{
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x0021CB42 File Offset: 0x0021BB42
		internal new IRawElementProviderSimple ProviderFromPeer(AutomationPeer peer)
		{
			return base.ProviderFromPeer(peer);
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x0021CB4C File Offset: 0x0021BB4C
		internal DependencyObject ElementFromProvider(IRawElementProviderSimple provider)
		{
			DependencyObject result = null;
			AutomationPeer automationPeer = base.PeerFromProvider(provider);
			if (automationPeer is UIElementAutomationPeer)
			{
				result = ((UIElementAutomationPeer)automationPeer).Owner;
			}
			else if (automationPeer is ContentElementAutomationPeer)
			{
				result = ((ContentElementAutomationPeer)automationPeer).Owner;
			}
			return result;
		}

		// Token: 0x060042DF RID: 17119
		internal abstract List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end);
	}
}
