using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200055B RID: 1371
	public class DocumentViewerAutomationPeer : DocumentViewerBaseAutomationPeer
	{
		// Token: 0x060043E6 RID: 17382 RVA: 0x0021FADD File Offset: 0x0021EADD
		public DocumentViewerAutomationPeer(DocumentViewer owner) : base(owner)
		{
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x0021FAE6 File Offset: 0x0021EAE6
		protected override string GetClassNameCore()
		{
			return "DocumentViewer";
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x0021FAF0 File Offset: 0x0021EAF0
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			if (patternInterface == PatternInterface.Scroll)
			{
				DocumentViewer documentViewer = (DocumentViewer)base.Owner;
				if (documentViewer.ScrollViewer != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(documentViewer.ScrollViewer);
					if (automationPeer != null && automationPeer is IScrollProvider)
					{
						automationPeer.EventsSource = this;
						result = automationPeer;
					}
				}
			}
			else
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}
	}
}
