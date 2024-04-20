using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200057E RID: 1406
	public class MediaElementAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060044FA RID: 17658 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public MediaElementAutomationPeer(MediaElement owner) : base(owner)
		{
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x00222B33 File Offset: 0x00221B33
		protected override string GetClassNameCore()
		{
			return "MediaElement";
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x001FBDEE File Offset: 0x001FADEE
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
