using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000596 RID: 1430
	public abstract class TextAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060045B0 RID: 17840 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		protected TextAutomationPeer(FrameworkElement owner) : base(owner)
		{
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x00224678 File Offset: 0x00223678
		protected override string GetNameCore()
		{
			string name = AutomationProperties.GetName(base.Owner);
			if (string.IsNullOrEmpty(name))
			{
				AutomationPeer labeledByCore = this.GetLabeledByCore();
				if (labeledByCore != null)
				{
					name = labeledByCore.GetName();
				}
			}
			return name ?? string.Empty;
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x0021CB42 File Offset: 0x0021BB42
		internal new IRawElementProviderSimple ProviderFromPeer(AutomationPeer peer)
		{
			return base.ProviderFromPeer(peer);
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x0021CB4C File Offset: 0x0021BB4C
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

		// Token: 0x060045B4 RID: 17844
		internal abstract List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end);
	}
}
