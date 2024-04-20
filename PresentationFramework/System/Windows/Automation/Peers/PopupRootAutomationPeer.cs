using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000583 RID: 1411
	internal class PopupRootAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600451F RID: 17695 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public PopupRootAutomationPeer(PopupRoot owner) : base(owner)
		{
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x002230F3 File Offset: 0x002220F3
		protected override string GetClassNameCore()
		{
			return "Popup";
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x0013CE2F File Offset: 0x0013BE2F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Window;
		}
	}
}
