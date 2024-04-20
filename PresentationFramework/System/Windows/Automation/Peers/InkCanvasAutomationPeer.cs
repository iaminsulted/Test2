using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000570 RID: 1392
	public class InkCanvasAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600447B RID: 17531 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public InkCanvasAutomationPeer(InkCanvas owner) : base(owner)
		{
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x00221499 File Offset: 0x00220499
		protected override string GetClassNameCore()
		{
			return "InkCanvas";
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x001FBDEE File Offset: 0x001FADEE
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
