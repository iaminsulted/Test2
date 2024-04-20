using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000569 RID: 1385
	public class GridViewHeaderRowPresenterAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004455 RID: 17493 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public GridViewHeaderRowPresenterAutomationPeer(GridViewHeaderRowPresenter owner) : base(owner)
		{
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x00220C33 File Offset: 0x0021FC33
		protected override string GetClassNameCore()
		{
			return "GridViewHeaderRowPresenter";
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x001FC072 File Offset: 0x001FB072
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Header;
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x00220C3C File Offset: 0x0021FC3C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> childrenCore = base.GetChildrenCore();
			List<AutomationPeer> list = null;
			if (childrenCore != null)
			{
				list = new List<AutomationPeer>(childrenCore.Count);
				foreach (AutomationPeer automationPeer in childrenCore)
				{
					if (automationPeer is UIElementAutomationPeer)
					{
						GridViewColumnHeader gridViewColumnHeader = ((UIElementAutomationPeer)automationPeer).Owner as GridViewColumnHeader;
						if (gridViewColumnHeader != null && gridViewColumnHeader.Role == GridViewColumnHeaderRole.Normal)
						{
							list.Insert(0, automationPeer);
						}
					}
				}
			}
			return list;
		}
	}
}
