using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200058C RID: 1420
	public abstract class SelectorItemAutomationPeer : ItemAutomationPeer, ISelectionItemProvider
	{
		// Token: 0x06004571 RID: 17777 RVA: 0x00222929 File Offset: 0x00221929
		protected SelectorItemAutomationPeer(object owner, SelectorAutomationPeer selectorAutomationPeer) : base(owner, selectorAutomationPeer)
		{
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x00223DC0 File Offset: 0x00222DC0
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.SelectionItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x00223DD0 File Offset: 0x00222DD0
		void ISelectionItemProvider.Select()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
			if (selector == null)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			selector.SelectionChange.SelectJustThisItem(selector.NewItemInfo(base.Item, null, -1), true);
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x00223E2C File Offset: 0x00222E2C
		void ISelectionItemProvider.AddToSelection()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
			if (selector == null || (!selector.CanSelectMultiple && selector.SelectedItem != null && selector.SelectedItem != base.Item))
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			selector.SelectionChange.Begin();
			selector.SelectionChange.Select(selector.NewItemInfo(base.Item, null, -1), true);
			selector.SelectionChange.End();
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x00223EBC File Offset: 0x00222EBC
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
			selector.SelectionChange.Begin();
			selector.SelectionChange.Unselect(selector.NewItemInfo(base.Item, null, -1));
			selector.SelectionChange.End();
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x06004576 RID: 17782 RVA: 0x00223F18 File Offset: 0x00222F18
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
				return selector._selectedItems.Contains(selector.NewItemInfo(base.Item, null, -1));
			}
		}

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x06004577 RID: 17783 RVA: 0x00223F4F File Offset: 0x00222F4F
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return base.ProviderFromPeer(base.ItemsControlAutomationPeer);
			}
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x00223F5D File Offset: 0x00222F5D
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseAutomationIsSelectedChanged(bool isSelected)
		{
			base.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, !isSelected, isSelected);
		}
	}
}
