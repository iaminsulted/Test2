using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200054E RID: 1358
	public sealed class DataGridCellAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060042FD RID: 17149 RVA: 0x0021D244 File Offset: 0x0021C244
		public DataGridCellAutomationPeer(DataGridCell owner) : base(owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x001FBDEE File Offset: 0x001FADEE
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}
	}
}
