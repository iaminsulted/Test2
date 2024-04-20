using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200055F RID: 1375
	public class FlowDocumentPageViewerAutomationPeer : DocumentViewerBaseAutomationPeer
	{
		// Token: 0x060043FC RID: 17404 RVA: 0x0021FADD File Offset: 0x0021EADD
		public FlowDocumentPageViewerAutomationPeer(FlowDocumentPageViewer owner) : base(owner)
		{
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0021FD7C File Offset: 0x0021ED7C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			if (base.Owner is IFlowDocumentViewer && list != null && list.Count > 0 && list[list.Count - 1] is DocumentAutomationPeer)
			{
				list.RemoveAt(list.Count - 1);
				if (list.Count == 0)
				{
					list = null;
				}
			}
			return list;
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x0021FDD7 File Offset: 0x0021EDD7
		protected override string GetClassNameCore()
		{
			return "FlowDocumentPageViewer";
		}
	}
}
