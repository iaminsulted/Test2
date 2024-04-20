using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200059A RID: 1434
	public class ThumbAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060045CC RID: 17868 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public ThumbAutomationPeer(Thumb owner) : base(owner)
		{
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x00224BAC File Offset: 0x00223BAC
		protected override string GetClassNameCore()
		{
			return "Thumb";
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x001FB8B7 File Offset: 0x001FA8B7
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Thumb;
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
