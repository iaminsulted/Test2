using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000579 RID: 1401
	public class LabelAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060044E2 RID: 17634 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public LabelAutomationPeer(Label owner) : base(owner)
		{
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x0022294A File Offset: 0x0022194A
		protected override string GetClassNameCore()
		{
			return "Text";
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x001FB806 File Offset: 0x001FA806
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Text;
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x00222954 File Offset: 0x00221954
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			if (!string.IsNullOrEmpty(nameCore) && ((Label)base.Owner).Content is string)
			{
				return AccessText.RemoveAccessKeyMarker(nameCore);
			}
			return nameCore;
		}
	}
}
