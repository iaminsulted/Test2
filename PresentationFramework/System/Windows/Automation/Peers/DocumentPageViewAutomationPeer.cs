using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200055A RID: 1370
	public class DocumentPageViewAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060043E3 RID: 17379 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public DocumentPageViewAutomationPeer(DocumentPageView owner) : base(owner)
		{
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x00109403 File Offset: 0x00108403
		protected override List<AutomationPeer> GetChildrenCore()
		{
			return null;
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x0021FA74 File Offset: 0x0021EA74
		protected override string GetAutomationIdCore()
		{
			string result = string.Empty;
			DocumentPageView documentPageView = (DocumentPageView)base.Owner;
			if (!string.IsNullOrEmpty(documentPageView.Name))
			{
				result = documentPageView.Name;
			}
			else if (documentPageView.PageNumber >= 0 && documentPageView.PageNumber < 2147483647)
			{
				result = string.Format(CultureInfo.InvariantCulture, "DocumentPage{0}", documentPageView.PageNumber + 1);
			}
			return result;
		}
	}
}
