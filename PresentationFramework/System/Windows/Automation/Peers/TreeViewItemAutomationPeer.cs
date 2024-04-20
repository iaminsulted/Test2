using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020005A0 RID: 1440
	public class TreeViewItemAutomationPeer : ItemsControlAutomationPeer, IExpandCollapseProvider, ISelectionItemProvider, IScrollItemProvider
	{
		// Token: 0x060045FF RID: 17919 RVA: 0x0021DF60 File Offset: 0x0021CF60
		public TreeViewItemAutomationPeer(TreeViewItem owner) : base(owner)
		{
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x00224F40 File Offset: 0x00223F40
		protected override string GetClassNameCore()
		{
			return "TreeViewItem";
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x00224F47 File Offset: 0x00223F47
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.TreeItem;
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x002251A7 File Offset: 0x002241A7
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.ExpandCollapse)
			{
				return this;
			}
			if (patternInterface == PatternInterface.SelectionItem)
			{
				return this;
			}
			if (patternInterface == PatternInterface.ScrollItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x002251C4 File Offset: 0x002241C4
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> children = null;
			ItemPeersStorage<ItemAutomationPeer> itemPeers = base.ItemPeers;
			base.ItemPeers = new ItemPeersStorage<ItemAutomationPeer>();
			TreeViewItem treeViewItem = base.Owner as TreeViewItem;
			if (treeViewItem != null)
			{
				TreeViewItemAutomationPeer.iterate(this, treeViewItem, delegate(AutomationPeer peer)
				{
					if (children == null)
					{
						children = new List<AutomationPeer>();
					}
					children.Add(peer);
					return false;
				}, base.ItemPeers, itemPeers);
			}
			return children;
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x00225220 File Offset: 0x00224220
		private static bool iterate(TreeViewItemAutomationPeer logicalParentAp, DependencyObject parent, TreeViewItemAutomationPeer.IteratorCallback callback, ItemPeersStorage<ItemAutomationPeer> dataChildren, ItemPeersStorage<ItemAutomationPeer> oldChildren)
		{
			bool flag = false;
			if (parent != null)
			{
				int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
				int num = 0;
				while (num < childrenCount && !flag)
				{
					DependencyObject child = VisualTreeHelper.GetChild(parent, num);
					if (child != null && child is UIElement)
					{
						AutomationPeer automationPeer;
						if (child is TreeViewItem)
						{
							object item = (child is UIElement) ? (logicalParentAp.Owner as ItemsControl).GetItemOrContainerFromContainer(child as UIElement) : child;
							automationPeer = oldChildren[item];
							if (automationPeer == null)
							{
								automationPeer = logicalParentAp.GetPeerFromWeakRefStorage(item);
								if (automationPeer != null)
								{
									automationPeer.AncestorsInvalid = false;
									automationPeer.ChildrenValid = false;
								}
							}
							if (automationPeer == null)
							{
								automationPeer = logicalParentAp.CreateItemAutomationPeer(item);
							}
							if (automationPeer != null)
							{
								AutomationPeer wrapperPeer = (automationPeer as ItemAutomationPeer).GetWrapperPeer();
								if (wrapperPeer != null)
								{
									wrapperPeer.EventsSource = automationPeer;
								}
								if (dataChildren[item] == null && automationPeer is ItemAutomationPeer)
								{
									callback(automationPeer);
									dataChildren[item] = (automationPeer as ItemAutomationPeer);
								}
							}
						}
						else
						{
							automationPeer = UIElementAutomationPeer.CreatePeerForElement((UIElement)child);
							if (automationPeer != null)
							{
								flag = callback(automationPeer);
							}
						}
						if (automationPeer == null)
						{
							flag = TreeViewItemAutomationPeer.iterate(logicalParentAp, child, callback, dataChildren, oldChildren);
						}
					}
					else
					{
						flag = TreeViewItemAutomationPeer.iterate(logicalParentAp, child, callback, dataChildren, oldChildren);
					}
					num++;
				}
			}
			return flag;
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x00225354 File Offset: 0x00224354
		protected internal override ItemAutomationPeer FindOrCreateItemAutomationPeer(object item)
		{
			ItemAutomationPeer itemAutomationPeer = base.ItemPeers[item];
			AutomationPeer peer = this;
			if (base.EventsSource is TreeViewDataItemAutomationPeer)
			{
				peer = (base.EventsSource as TreeViewDataItemAutomationPeer);
			}
			if (itemAutomationPeer == null)
			{
				itemAutomationPeer = base.GetPeerFromWeakRefStorage(item);
			}
			if (itemAutomationPeer == null)
			{
				itemAutomationPeer = this.CreateItemAutomationPeer(item);
				if (itemAutomationPeer != null)
				{
					itemAutomationPeer.TrySetParentInfo(peer);
				}
			}
			if (itemAutomationPeer != null)
			{
				AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
				if (wrapperPeer != null)
				{
					wrapperPeer.EventsSource = itemAutomationPeer;
				}
			}
			return itemAutomationPeer;
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x00224DEB File Offset: 0x00223DEB
		internal override bool IsPropertySupportedByControlForFindItem(int id)
		{
			return base.IsPropertySupportedByControlForFindItem(id) || SelectionItemPatternIdentifiers.IsSelectedProperty.Id == id;
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x00224E08 File Offset: 0x00223E08
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

		// Token: 0x06004608 RID: 17928 RVA: 0x002253BF File Offset: 0x002243BF
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new TreeViewDataItemAutomationPeer(item, this, base.EventsSource as TreeViewDataItemAutomationPeer);
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x002253D4 File Offset: 0x002243D4
		internal override IDisposable UpdateChildren()
		{
			TreeViewDataItemAutomationPeer treeViewDataItemAutomationPeer = base.EventsSource as TreeViewDataItemAutomationPeer;
			if (treeViewDataItemAutomationPeer != null)
			{
				treeViewDataItemAutomationPeer.UpdateChildrenInternal(5);
			}
			else
			{
				base.UpdateChildrenInternal(5);
			}
			base.WeakRefElementProxyStorage.PurgeWeakRefCollection();
			return null;
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x0022540C File Offset: 0x0022440C
		internal void AddDataPeerInfo(TreeViewDataItemAutomationPeer dataPeer)
		{
			base.EventsSource = dataPeer;
			this.UpdateWeakRefStorageFromDataPeer();
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x0022541C File Offset: 0x0022441C
		internal void UpdateWeakRefStorageFromDataPeer()
		{
			if (base.EventsSource is TreeViewDataItemAutomationPeer)
			{
				if ((base.EventsSource as TreeViewDataItemAutomationPeer).WeakRefElementProxyStorageCache == null)
				{
					(base.EventsSource as TreeViewDataItemAutomationPeer).WeakRefElementProxyStorageCache = base.WeakRefElementProxyStorage;
					return;
				}
				if (base.WeakRefElementProxyStorage.Count == 0)
				{
					base.WeakRefElementProxyStorage = (base.EventsSource as TreeViewDataItemAutomationPeer).WeakRefElementProxyStorageCache;
				}
			}
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x00225482 File Offset: 0x00224482
		void IExpandCollapseProvider.Expand()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			TreeViewItem treeViewItem = (TreeViewItem)base.Owner;
			if (!treeViewItem.HasItems)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			treeViewItem.IsExpanded = true;
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x002254BB File Offset: 0x002244BB
		void IExpandCollapseProvider.Collapse()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			TreeViewItem treeViewItem = (TreeViewItem)base.Owner;
			if (!treeViewItem.HasItems)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			treeViewItem.IsExpanded = false;
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x0600460E RID: 17934 RVA: 0x002254F4 File Offset: 0x002244F4
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				TreeViewItem treeViewItem = (TreeViewItem)base.Owner;
				if (!treeViewItem.HasItems)
				{
					return ExpandCollapseState.LeafNode;
				}
				if (!treeViewItem.IsExpanded)
				{
					return ExpandCollapseState.Collapsed;
				}
				return ExpandCollapseState.Expanded;
			}
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x00225524 File Offset: 0x00224524
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			if (base.EventsSource is TreeViewDataItemAutomationPeer)
			{
				(base.EventsSource as TreeViewDataItemAutomationPeer).RaiseExpandCollapseAutomationEvent(oldValue, newValue);
				return;
			}
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x00225574 File Offset: 0x00224574
		void ISelectionItemProvider.Select()
		{
			((TreeViewItem)base.Owner).IsSelected = true;
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x00225588 File Offset: 0x00224588
		void ISelectionItemProvider.AddToSelection()
		{
			TreeView parentTreeView = ((TreeViewItem)base.Owner).ParentTreeView;
			if (parentTreeView == null || (parentTreeView.SelectedItem != null && parentTreeView.SelectedContainer != base.Owner))
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			((TreeViewItem)base.Owner).IsSelected = true;
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x002255E0 File Offset: 0x002245E0
		void ISelectionItemProvider.RemoveFromSelection()
		{
			((TreeViewItem)base.Owner).IsSelected = false;
		}

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06004613 RID: 17939 RVA: 0x002255F3 File Offset: 0x002245F3
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return ((TreeViewItem)base.Owner).IsSelected;
			}
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06004614 RID: 17940 RVA: 0x00225608 File Offset: 0x00224608
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				ItemsControl parentItemsControl = ((TreeViewItem)base.Owner).ParentItemsControl;
				if (parentItemsControl != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(parentItemsControl);
					if (automationPeer != null)
					{
						return base.ProviderFromPeer(automationPeer);
					}
				}
				return null;
			}
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x0022563C File Offset: 0x0022463C
		void IScrollItemProvider.ScrollIntoView()
		{
			((TreeViewItem)base.Owner).BringIntoView();
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x0022564E File Offset: 0x0022464E
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseAutomationIsSelectedChanged(bool isSelected)
		{
			if (base.EventsSource is TreeViewDataItemAutomationPeer)
			{
				(base.EventsSource as TreeViewDataItemAutomationPeer).RaiseAutomationIsSelectedChanged(isSelected);
				return;
			}
			base.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, !isSelected, isSelected);
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x00225689 File Offset: 0x00224689
		internal void RaiseAutomationSelectionEvent(AutomationEvents eventId)
		{
			if (base.EventsSource != null)
			{
				base.EventsSource.RaiseAutomationEvent(eventId);
				return;
			}
			base.RaiseAutomationEvent(eventId);
		}

		// Token: 0x02000B24 RID: 2852
		// (Invoke) Token: 0x06008C69 RID: 35945
		private delegate bool IteratorCallback(AutomationPeer peer);
	}
}
