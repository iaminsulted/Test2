using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200059F RID: 1439
	public class TreeViewDataItemAutomationPeer : ItemAutomationPeer, ISelectionItemProvider, IScrollItemProvider, IExpandCollapseProvider
	{
		// Token: 0x060045E9 RID: 17897 RVA: 0x00224EA3 File Offset: 0x00223EA3
		public TreeViewDataItemAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer, TreeViewDataItemAutomationPeer parentDataItemAutomationPeer) : base(item, null)
		{
			if (itemsControlAutomationPeer.Owner is TreeView || parentDataItemAutomationPeer == null)
			{
				base.ItemsControlAutomationPeer = itemsControlAutomationPeer;
			}
			this._parentDataItemAutomationPeer = parentDataItemAutomationPeer;
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x00224ECC File Offset: 0x00223ECC
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
			if (patternInterface == PatternInterface.ItemContainer || patternInterface == PatternInterface.SynchronizedInput)
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
				if (treeViewItemAutomationPeer != null)
				{
					if (patternInterface == PatternInterface.SynchronizedInput)
					{
						return treeViewItemAutomationPeer.GetPattern(patternInterface);
					}
					return treeViewItemAutomationPeer;
				}
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x00224F1C File Offset: 0x00223F1C
		internal override AutomationPeer GetWrapperPeer()
		{
			AutomationPeer wrapperPeer = base.GetWrapperPeer();
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = wrapperPeer as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				treeViewItemAutomationPeer.AddDataPeerInfo(this);
			}
			return wrapperPeer;
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x00224F40 File Offset: 0x00223F40
		protected override string GetClassNameCore()
		{
			return "TreeViewItem";
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x00224F47 File Offset: 0x00223F47
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.TreeItem;
		}

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x060045EE RID: 17902 RVA: 0x00224F4B File Offset: 0x00223F4B
		public TreeViewDataItemAutomationPeer ParentDataItemAutomationPeer
		{
			get
			{
				return this._parentDataItemAutomationPeer;
			}
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x00224F53 File Offset: 0x00223F53
		internal override ItemsControlAutomationPeer GetItemsControlAutomationPeer()
		{
			if (this._parentDataItemAutomationPeer == null)
			{
				return base.GetItemsControlAutomationPeer();
			}
			return this._parentDataItemAutomationPeer.GetWrapperPeer() as ItemsControlAutomationPeer;
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x00224F74 File Offset: 0x00223F74
		internal override void RealizeCore()
		{
			this.RecursiveScrollIntoView();
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x00224F7C File Offset: 0x00223F7C
		void IExpandCollapseProvider.Expand()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				((IExpandCollapseProvider)treeViewItemAutomationPeer).Expand();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x00224FA8 File Offset: 0x00223FA8
		void IExpandCollapseProvider.Collapse()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				((IExpandCollapseProvider)treeViewItemAutomationPeer).Collapse();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x060045F3 RID: 17907 RVA: 0x00224FD4 File Offset: 0x00223FD4
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
				if (treeViewItemAutomationPeer != null)
				{
					return ((IExpandCollapseProvider)treeViewItemAutomationPeer).ExpandCollapseState;
				}
				base.ThrowElementNotAvailableException();
				return ExpandCollapseState.LeafNode;
			}
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x0021CB14 File Offset: 0x0021BB14
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x00225000 File Offset: 0x00224000
		void ISelectionItemProvider.Select()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				((ISelectionItemProvider)treeViewItemAutomationPeer).Select();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x0022502C File Offset: 0x0022402C
		void ISelectionItemProvider.AddToSelection()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				((ISelectionItemProvider)treeViewItemAutomationPeer).AddToSelection();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x00225058 File Offset: 0x00224058
		void ISelectionItemProvider.RemoveFromSelection()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				((ISelectionItemProvider)treeViewItemAutomationPeer).RemoveFromSelection();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x060045F8 RID: 17912 RVA: 0x00225084 File Offset: 0x00224084
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
				return treeViewItemAutomationPeer != null && ((ISelectionItemProvider)treeViewItemAutomationPeer).IsSelected;
			}
		}

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x060045F9 RID: 17913 RVA: 0x002250A8 File Offset: 0x002240A8
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
				if (treeViewItemAutomationPeer != null)
				{
					return ((ISelectionItemProvider)treeViewItemAutomationPeer).SelectionContainer;
				}
				base.ThrowElementNotAvailableException();
				return null;
			}
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x002250D4 File Offset: 0x002240D4
		void IScrollItemProvider.ScrollIntoView()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				((IScrollItemProvider)treeViewItemAutomationPeer).ScrollIntoView();
				return;
			}
			this.RecursiveScrollIntoView();
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x00223F5D File Offset: 0x00222F5D
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseAutomationIsSelectedChanged(bool isSelected)
		{
			base.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, !isSelected, isSelected);
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x00225100 File Offset: 0x00224100
		private void RecursiveScrollIntoView()
		{
			ItemsControlAutomationPeer itemsControlAutomationPeer = base.ItemsControlAutomationPeer;
			if (this.ParentDataItemAutomationPeer != null && itemsControlAutomationPeer == null)
			{
				this.ParentDataItemAutomationPeer.RecursiveScrollIntoView();
				itemsControlAutomationPeer = base.ItemsControlAutomationPeer;
			}
			if (itemsControlAutomationPeer != null)
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = itemsControlAutomationPeer as TreeViewItemAutomationPeer;
				if (treeViewItemAutomationPeer != null && ((IExpandCollapseProvider)treeViewItemAutomationPeer).ExpandCollapseState == ExpandCollapseState.Collapsed)
				{
					((IExpandCollapseProvider)treeViewItemAutomationPeer).Expand();
				}
				ItemsControl itemsControl = itemsControlAutomationPeer.Owner as ItemsControl;
				if (itemsControl != null)
				{
					if (itemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
					{
						itemsControl.OnBringItemIntoView(base.Item);
						return;
					}
					base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(itemsControl.OnBringItemIntoView), base.Item);
				}
			}
		}

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x00225196 File Offset: 0x00224196
		// (set) Token: 0x060045FE RID: 17918 RVA: 0x0022519E File Offset: 0x0022419E
		internal ItemPeersStorage<WeakReference> WeakRefElementProxyStorageCache
		{
			get
			{
				return this._WeakRefElementProxyStorageCache;
			}
			set
			{
				this._WeakRefElementProxyStorageCache = value;
			}
		}

		// Token: 0x04002545 RID: 9541
		private TreeViewDataItemAutomationPeer _parentDataItemAutomationPeer;

		// Token: 0x04002546 RID: 9542
		private ItemPeersStorage<WeakReference> _WeakRefElementProxyStorageCache;
	}
}
