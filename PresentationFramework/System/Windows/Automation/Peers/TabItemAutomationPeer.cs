using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000592 RID: 1426
	public class TabItemAutomationPeer : SelectorItemAutomationPeer, ISelectionItemProvider
	{
		// Token: 0x06004590 RID: 17808 RVA: 0x00222996 File Offset: 0x00221996
		public TabItemAutomationPeer(object owner, TabControlAutomationPeer tabControlAutomationPeer) : base(owner, tabControlAutomationPeer)
		{
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x00224175 File Offset: 0x00223175
		protected override string GetClassNameCore()
		{
			return "TabItem";
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x001A5A01 File Offset: 0x001A4A01
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.TabItem;
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x0022417C File Offset: 0x0022317C
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			if (!string.IsNullOrEmpty(nameCore))
			{
				TabItem tabItem = base.GetWrapper() as TabItem;
				if (tabItem != null && tabItem.Header is string)
				{
					return AccessText.RemoveAccessKeyMarker(nameCore);
				}
			}
			return nameCore;
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x002241BC File Offset: 0x002231BC
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			TabItem tabItem = base.GetWrapper() as TabItem;
			if (tabItem != null && tabItem.IsSelected)
			{
				TabControl tabControl = base.ItemsControlAutomationPeer.Owner as TabControl;
				if (tabControl != null)
				{
					ContentPresenter selectedContentPresenter = tabControl.SelectedContentPresenter;
					if (selectedContentPresenter != null)
					{
						List<AutomationPeer> children = new FrameworkElementAutomationPeer(selectedContentPresenter).GetChildren();
						if (children != null)
						{
							if (list == null)
							{
								list = children;
							}
							else
							{
								list.AddRange(children);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x00224228 File Offset: 0x00223228
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			TabItem tabItem = base.GetWrapper() as TabItem;
			if (tabItem != null && tabItem.IsSelected)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x0022426C File Offset: 0x0022326C
		internal override void RealizeCore()
		{
			Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
			if (selector != null && this != null)
			{
				if (selector.CanSelectMultiple)
				{
					((ISelectionItemProvider)this).AddToSelection();
					return;
				}
				((ISelectionItemProvider)this).Select();
			}
		}
	}
}
