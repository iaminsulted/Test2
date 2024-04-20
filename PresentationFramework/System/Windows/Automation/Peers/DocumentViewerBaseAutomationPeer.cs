using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200055C RID: 1372
	public class DocumentViewerBaseAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060043E9 RID: 17385 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public DocumentViewerBaseAutomationPeer(DocumentViewerBase owner) : base(owner)
		{
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x0021FB44 File Offset: 0x0021EB44
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			if (patternInterface == PatternInterface.Text)
			{
				base.GetChildren();
				if (this._documentPeer != null)
				{
					this._documentPeer.EventsSource = this;
					result = this._documentPeer.GetPattern(patternInterface);
				}
			}
			else
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x0021FB8C File Offset: 0x0021EB8C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			AutomationPeer documentAutomationPeer = this.GetDocumentAutomationPeer();
			if (this._documentPeer != documentAutomationPeer)
			{
				if (this._documentPeer != null)
				{
					this._documentPeer.OnDisconnected();
				}
				this._documentPeer = (documentAutomationPeer as DocumentAutomationPeer);
			}
			if (documentAutomationPeer != null)
			{
				if (list == null)
				{
					list = new List<AutomationPeer>();
				}
				list.Add(documentAutomationPeer);
			}
			return list;
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x001FD726 File Offset: 0x001FC726
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Document;
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x0021FAE6 File Offset: 0x0021EAE6
		protected override string GetClassNameCore()
		{
			return "DocumentViewer";
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0021FBE4 File Offset: 0x0021EBE4
		private AutomationPeer GetDocumentAutomationPeer()
		{
			AutomationPeer result = null;
			IDocumentPaginatorSource document = ((DocumentViewerBase)base.Owner).Document;
			if (document != null)
			{
				if (document is UIElement)
				{
					result = UIElementAutomationPeer.CreatePeerForElement((UIElement)document);
				}
				else if (document is ContentElement)
				{
					result = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)document);
				}
			}
			return result;
		}

		// Token: 0x04002522 RID: 9506
		private DocumentAutomationPeer _documentPeer;
	}
}
