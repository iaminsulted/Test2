using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020005A1 RID: 1441
	public class UserControlAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004618 RID: 17944 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public UserControlAutomationPeer(UserControl owner) : base(owner)
		{
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x001FBDEE File Offset: 0x001FADEE
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
