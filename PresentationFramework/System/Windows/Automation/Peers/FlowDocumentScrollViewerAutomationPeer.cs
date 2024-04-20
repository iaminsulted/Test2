using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Documents;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000561 RID: 1377
	public class FlowDocumentScrollViewerAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600440D RID: 17421 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public FlowDocumentScrollViewerAutomationPeer(FlowDocumentScrollViewer owner) : base(owner)
		{
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x002200E8 File Offset: 0x0021F0E8
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			if (patternInterface == PatternInterface.Scroll)
			{
				FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)base.Owner;
				if (flowDocumentScrollViewer.ScrollViewer != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(flowDocumentScrollViewer.ScrollViewer);
					if (automationPeer != null && automationPeer is IScrollProvider)
					{
						automationPeer.EventsSource = this;
						result = automationPeer;
					}
				}
			}
			else if (patternInterface == PatternInterface.Text)
			{
				base.GetChildren();
				if (this._documentPeer != null)
				{
					this._documentPeer.EventsSource = this;
					result = this._documentPeer.GetPattern(patternInterface);
				}
			}
			else if (patternInterface == PatternInterface.SynchronizedInput)
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x00220170 File Offset: 0x0021F170
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			if (!(base.Owner is IFlowDocumentViewer))
			{
				FlowDocument document = ((FlowDocumentScrollViewer)base.Owner).Document;
				if (document != null)
				{
					AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement(document);
					if (this._documentPeer != automationPeer)
					{
						if (this._documentPeer != null)
						{
							this._documentPeer.OnDisconnected();
						}
						this._documentPeer = (automationPeer as DocumentAutomationPeer);
					}
					if (automationPeer != null)
					{
						if (list == null)
						{
							list = new List<AutomationPeer>();
						}
						list.Add(automationPeer);
					}
				}
			}
			return list;
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x001FD726 File Offset: 0x001FC726
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Document;
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x002201E8 File Offset: 0x0021F1E8
		protected override string GetClassNameCore()
		{
			return "FlowDocumentScrollViewer";
		}

		// Token: 0x04002524 RID: 9508
		private DocumentAutomationPeer _documentPeer;
	}
}
