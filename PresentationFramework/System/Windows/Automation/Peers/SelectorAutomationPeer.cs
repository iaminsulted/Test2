using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200058B RID: 1419
	public abstract class SelectorAutomationPeer : ItemsControlAutomationPeer, ISelectionProvider
	{
		// Token: 0x06004566 RID: 17766 RVA: 0x0021DF60 File Offset: 0x0021CF60
		protected SelectorAutomationPeer(Selector owner) : base(owner)
		{
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x001390BC File Offset: 0x001380BC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.List;
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x00223BB7 File Offset: 0x00222BB7
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Selection)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x0021CC56 File Offset: 0x0021BC56
		internal override bool IsPropertySupportedByControlForFindItem(int id)
		{
			return SelectorAutomationPeer.IsPropertySupportedByControlForFindItemInternal(id);
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x00223BC6 File Offset: 0x00222BC6
		internal new static bool IsPropertySupportedByControlForFindItemInternal(int id)
		{
			return ItemsControlAutomationPeer.IsPropertySupportedByControlForFindItemInternal(id) || SelectionItemPatternIdentifiers.IsSelectedProperty.Id == id;
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x0021CC5E File Offset: 0x0021BC5E
		internal override object GetSupportedPropertyValue(ItemAutomationPeer itemPeer, int propertyId)
		{
			return SelectorAutomationPeer.GetSupportedPropertyValueInternal(itemPeer, propertyId);
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x00223BE4 File Offset: 0x00222BE4
		internal new static object GetSupportedPropertyValueInternal(AutomationPeer itemPeer, int propertyId)
		{
			if (SelectionItemPatternIdentifiers.IsSelectedProperty.Id != propertyId)
			{
				return ItemsControlAutomationPeer.GetSupportedPropertyValueInternal(itemPeer, propertyId);
			}
			ISelectionItemProvider selectionItemProvider = itemPeer.GetPattern(PatternInterface.SelectionItem) as ISelectionItemProvider;
			if (selectionItemProvider != null)
			{
				return selectionItemProvider.IsSelected;
			}
			return null;
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00223C24 File Offset: 0x00222C24
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			Selector selector = (Selector)base.Owner;
			int count = selector._selectedItems.Count;
			int count2 = selector.Items.Count;
			if (count > 0 && count2 > 0)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>(count);
				for (int i = 0; i < count; i++)
				{
					SelectorItemAutomationPeer selectorItemAutomationPeer = this.FindOrCreateItemAutomationPeer(selector._selectedItems[i].Item) as SelectorItemAutomationPeer;
					if (selectorItemAutomationPeer != null)
					{
						list.Add(base.ProviderFromPeer(selectorItemAutomationPeer));
					}
				}
				return list.ToArray();
			}
			return null;
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x00223CAD File Offset: 0x00222CAD
		bool ISelectionProvider.CanSelectMultiple
		{
			get
			{
				return ((Selector)base.Owner).CanSelectMultiple;
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x0600456F RID: 17775 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x00223CC0 File Offset: 0x00222CC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseSelectionEvents(SelectionChangedEventArgs e)
		{
			if (base.ItemPeers.Count == 0)
			{
				base.RaiseAutomationEvent(AutomationEvents.SelectionPatternOnInvalidated);
				return;
			}
			Selector selector = (Selector)base.Owner;
			int count = selector._selectedItems.Count;
			int count2 = e.AddedItems.Count;
			int count3 = e.RemovedItems.Count;
			if (count == 1 && count2 == 1)
			{
				SelectorItemAutomationPeer selectorItemAutomationPeer = this.FindOrCreateItemAutomationPeer(selector._selectedItems[0].Item) as SelectorItemAutomationPeer;
				if (selectorItemAutomationPeer != null)
				{
					selectorItemAutomationPeer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
					return;
				}
			}
			else
			{
				if (count2 + count3 > 20)
				{
					base.RaiseAutomationEvent(AutomationEvents.SelectionPatternOnInvalidated);
					return;
				}
				for (int i = 0; i < count2; i++)
				{
					SelectorItemAutomationPeer selectorItemAutomationPeer2 = this.FindOrCreateItemAutomationPeer(e.AddedItems[i]) as SelectorItemAutomationPeer;
					if (selectorItemAutomationPeer2 != null)
					{
						selectorItemAutomationPeer2.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
					}
				}
				for (int i = 0; i < count3; i++)
				{
					SelectorItemAutomationPeer selectorItemAutomationPeer3 = this.FindOrCreateItemAutomationPeer(e.RemovedItems[i]) as SelectorItemAutomationPeer;
					if (selectorItemAutomationPeer3 != null)
					{
						selectorItemAutomationPeer3.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
					}
				}
			}
		}
	}
}
