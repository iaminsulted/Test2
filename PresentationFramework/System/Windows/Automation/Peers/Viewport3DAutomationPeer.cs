using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020005A2 RID: 1442
	public class Viewport3DAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600461B RID: 17947 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public Viewport3DAutomationPeer(Viewport3D owner) : base(owner)
		{
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x002256A7 File Offset: 0x002246A7
		protected override string GetClassNameCore()
		{
			return "Viewport3D";
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x001FBDEE File Offset: 0x001FADEE
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
