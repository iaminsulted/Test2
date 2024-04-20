using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200056F RID: 1391
	public class ImageAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004478 RID: 17528 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public ImageAutomationPeer(Image owner) : base(owner)
		{
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x00221492 File Offset: 0x00220492
		protected override string GetClassNameCore()
		{
			return "Image";
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x001FC2AC File Offset: 0x001FB2AC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Image;
		}
	}
}
