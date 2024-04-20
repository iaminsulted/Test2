using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200059E RID: 1438
	public class TreeViewAutomationPeer : ItemsControlAutomationPeer, ISelectionProvider
	{
		// Token: 0x060045DE RID: 17886 RVA: 0x0021DF60 File Offset: 0x0021CF60
		public TreeViewAutomationPeer(TreeView owner) : base(owner)
		{
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x00224C80 File Offset: 0x00223C80
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Tree;
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x00224C84 File Offset: 0x00223C84
		protected override string GetClassNameCore()
		{
			return "TreeView";
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x00224C8C File Offset: 0x00223C8C
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Selection)
			{
				return this;
			}
			if (patternInterface == PatternInterface.Scroll)
			{
				ItemsControl itemsControl = (ItemsControl)base.Owner;
				if (itemsControl.ScrollHost != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(itemsControl.ScrollHost);
					if (automationPeer != null && automationPeer is IScrollProvider)
					{
						automationPeer.EventsSource = this;
						return (IScrollProvider)automationPeer;
					}
				}
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x00224CE4 File Offset: 0x00223CE4
		protected override List<AutomationPeer> GetChildrenCore()
		{
			if (this.IsVirtualized)
			{
				return base.GetChildrenCore();
			}
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			ItemCollection items = itemsControl.Items;
			ItemPeersStorage<ItemAutomationPeer> itemPeers = base.ItemPeers;
			base.ItemPeers = new ItemPeersStorage<ItemAutomationPeer>();
			if (items.Count > 0)
			{
				List<AutomationPeer> list = new List<AutomationPeer>(items.Count);
				for (int i = 0; i < items.Count; i++)
				{
					if (itemsControl.ItemContainerGenerator.ContainerFromIndex(i) is TreeViewItem)
					{
						ItemAutomationPeer itemAutomationPeer = itemPeers[items[i]];
						if (itemAutomationPeer == null)
						{
							itemAutomationPeer = this.CreateItemAutomationPeer(items[i]);
						}
						if (itemAutomationPeer != null)
						{
							AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
							if (wrapperPeer != null)
							{
								wrapperPeer.EventsSource = itemAutomationPeer;
							}
						}
						if (base.ItemPeers[items[i]] == null)
						{
							list.Add(itemAutomationPeer);
							base.ItemPeers[items[i]] = itemAutomationPeer;
						}
					}
				}
				return list;
			}
			return null;
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x00224DE1 File Offset: 0x00223DE1
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new TreeViewDataItemAutomationPeer(item, this, null);
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x00224DEB File Offset: 0x00223DEB
		internal override bool IsPropertySupportedByControlForFindItem(int id)
		{
			return base.IsPropertySupportedByControlForFindItem(id) || SelectionItemPatternIdentifiers.IsSelectedProperty.Id == id;
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x00224E08 File Offset: 0x00223E08
		internal override object GetSupportedPropertyValue(ItemAutomationPeer itemPeer, int propertyId)
		{
			if (SelectionItemPatternIdentifiers.IsSelectedProperty.Id != propertyId)
			{
				return base.GetSupportedPropertyValue(itemPeer, propertyId);
			}
			ISelectionItemProvider selectionItemProvider = itemPeer.GetPattern(PatternInterface.SelectionItem) as ISelectionItemProvider;
			if (selectionItemProvider != null)
			{
				return selectionItemProvider.IsSelected;
			}
			return null;
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x00224E4C File Offset: 0x00223E4C
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			IRawElementProviderSimple[] array = null;
			TreeViewItem selectedContainer = ((TreeView)base.Owner).SelectedContainer;
			if (selectedContainer != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(selectedContainer);
				if (automationPeer.EventsSource != null)
				{
					automationPeer = automationPeer.EventsSource;
				}
				if (automationPeer != null)
				{
					array = new IRawElementProviderSimple[]
					{
						base.ProviderFromPeer(automationPeer)
					};
				}
			}
			if (array == null)
			{
				array = Array.Empty<IRawElementProviderSimple>();
			}
			return array;
		}

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x060045E7 RID: 17895 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ISelectionProvider.CanSelectMultiple
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x060045E8 RID: 17896 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}
	}
}
