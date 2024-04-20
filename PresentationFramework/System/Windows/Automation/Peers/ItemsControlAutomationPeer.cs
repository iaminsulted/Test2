using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000573 RID: 1395
	public abstract class ItemsControlAutomationPeer : FrameworkElementAutomationPeer, IItemContainerProvider
	{
		// Token: 0x060044B0 RID: 17584 RVA: 0x00221D8D File Offset: 0x00220D8D
		protected ItemsControlAutomationPeer(ItemsControl owner) : base(owner)
		{
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x00221DAC File Offset: 0x00220DAC
		public override object GetPattern(PatternInterface patternInterface)
		{
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
			else if (patternInterface == PatternInterface.ItemContainer)
			{
				if (base.Owner is ItemsControl)
				{
					return this;
				}
				return null;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x00221E14 File Offset: 0x00220E14
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = null;
			ItemPeersStorage<ItemAutomationPeer> dataChildren = this._dataChildren;
			this._dataChildren = new ItemPeersStorage<ItemAutomationPeer>();
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			ItemCollection items = itemsControl.Items;
			Panel itemsHost = itemsControl.ItemsHost;
			bool useNetFx472CompatibleAccessibilityFeatures = AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures;
			if (itemsControl.IsGrouping)
			{
				if (itemsHost == null)
				{
					return null;
				}
				if (!useNetFx472CompatibleAccessibilityFeatures)
				{
					this._reusablePeers = dataChildren;
				}
				IList list2 = itemsHost.Children;
				list = new List<AutomationPeer>(list2.Count);
				foreach (object obj in list2)
				{
					UIElementAutomationPeer uielementAutomationPeer = ((UIElement)obj).CreateAutomationPeer() as UIElementAutomationPeer;
					if (uielementAutomationPeer != null)
					{
						list.Add(uielementAutomationPeer);
						if (useNetFx472CompatibleAccessibilityFeatures)
						{
							if (this._recentlyRealizedPeers != null && this._recentlyRealizedPeers.Count > 0 && this.AncestorsInvalid)
							{
								GroupItemAutomationPeer groupItemAutomationPeer = uielementAutomationPeer as GroupItemAutomationPeer;
								if (groupItemAutomationPeer != null)
								{
									groupItemAutomationPeer.InvalidateGroupItemPeersContainingRecentlyRealizedPeers(this._recentlyRealizedPeers);
								}
							}
						}
						else if (this.AncestorsInvalid)
						{
							GroupItemAutomationPeer groupItemAutomationPeer2 = uielementAutomationPeer as GroupItemAutomationPeer;
							if (groupItemAutomationPeer2 != null)
							{
								groupItemAutomationPeer2.AncestorsInvalid = true;
								groupItemAutomationPeer2.ChildrenValid = true;
							}
						}
					}
				}
				return list;
			}
			else
			{
				if (items.Count > 0)
				{
					IList list2;
					if (this.IsVirtualized)
					{
						if (itemsHost == null)
						{
							return null;
						}
						list2 = itemsHost.Children;
					}
					else
					{
						list2 = items;
					}
					list = new List<AutomationPeer>(list2.Count);
					foreach (object obj2 in list2)
					{
						object obj3;
						if (this.IsVirtualized)
						{
							DependencyObject dependencyObject = obj2 as DependencyObject;
							obj3 = ((dependencyObject != null) ? itemsControl.ItemContainerGenerator.ItemFromContainer(dependencyObject) : null);
							if (obj3 == DependencyProperty.UnsetValue)
							{
								continue;
							}
						}
						else
						{
							obj3 = obj2;
						}
						ItemAutomationPeer itemAutomationPeer = dataChildren[obj3];
						itemAutomationPeer = this.ReusePeerForItem(itemAutomationPeer, obj3);
						if (itemAutomationPeer == null)
						{
							itemAutomationPeer = this.CreateItemAutomationPeer(obj3);
						}
						if (itemAutomationPeer != null)
						{
							AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
							if (wrapperPeer != null)
							{
								wrapperPeer.EventsSource = itemAutomationPeer;
							}
						}
						if (itemAutomationPeer != null && this._dataChildren[obj3] == null)
						{
							list.Add(itemAutomationPeer);
							this._dataChildren[obj3] = itemAutomationPeer;
						}
					}
					return list;
				}
				return null;
			}
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x00222074 File Offset: 0x00221074
		internal ItemAutomationPeer ReusePeerForItem(ItemAutomationPeer peer, object item)
		{
			if (peer == null)
			{
				peer = this.GetPeerFromWeakRefStorage(item);
				if (peer != null)
				{
					peer.AncestorsInvalid = false;
					peer.ChildrenValid = false;
				}
			}
			if (peer != null)
			{
				peer.ReuseForItem(item);
			}
			return peer;
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x0022209E File Offset: 0x0022109E
		internal void AddProxyToWeakRefStorage(WeakReference wr, ItemAutomationPeer itemPeer)
		{
			if ((base.Owner as ItemsControl).Items != null && this.GetPeerFromWeakRefStorage(itemPeer.Item) == null)
			{
				this.WeakRefElementProxyStorage[itemPeer.Item] = wr;
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x002220D4 File Offset: 0x002210D4
		IRawElementProviderSimple IItemContainerProvider.FindItemByProperty(IRawElementProviderSimple startAfter, int propertyId, object value)
		{
			base.ResetChildrenCache();
			if (propertyId != 0 && !this.IsPropertySupportedByControlForFindItem(propertyId))
			{
				throw new ArgumentException(SR.Get("PropertyNotSupported"));
			}
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			ItemCollection itemCollection = null;
			if (itemsControl != null)
			{
				itemCollection = itemsControl.Items;
			}
			if (itemCollection != null && itemCollection.Count > 0)
			{
				ItemAutomationPeer itemAutomationPeer = null;
				if (startAfter != null)
				{
					itemAutomationPeer = (base.PeerFromProvider(startAfter) as ItemAutomationPeer);
					if (itemAutomationPeer == null)
					{
						return null;
					}
				}
				int num = 0;
				if (itemAutomationPeer != null)
				{
					if (itemAutomationPeer.Item == null)
					{
						throw new InvalidOperationException(SR.Get("InavalidStartItem"));
					}
					num = itemCollection.IndexOf(itemAutomationPeer.Item) + 1;
					if (num == 0 || num == itemCollection.Count)
					{
						return null;
					}
				}
				if (propertyId == 0)
				{
					for (int i = num; i < itemCollection.Count; i++)
					{
						if (itemCollection.IndexOf(itemCollection[i]) == i)
						{
							return base.ProviderFromPeer(this.FindOrCreateItemAutomationPeer(itemCollection[i]));
						}
					}
				}
				object obj = null;
				for (int j = num; j < itemCollection.Count; j++)
				{
					ItemAutomationPeer itemAutomationPeer2 = this.FindOrCreateItemAutomationPeer(itemCollection[j]);
					if (itemAutomationPeer2 != null)
					{
						try
						{
							obj = this.GetSupportedPropertyValue(itemAutomationPeer2, propertyId);
						}
						catch (Exception ex)
						{
							if (ex is ElementNotAvailableException)
							{
								goto IL_162;
							}
						}
						if (value == null || obj == null)
						{
							if (obj == null && value == null && itemCollection.IndexOf(itemCollection[j]) == j)
							{
								return base.ProviderFromPeer(itemAutomationPeer2);
							}
						}
						else if (value.Equals(obj) && itemCollection.IndexOf(itemCollection[j]) == j)
						{
							return base.ProviderFromPeer(itemAutomationPeer2);
						}
					}
					IL_162:;
				}
			}
			return null;
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x00222268 File Offset: 0x00221268
		internal virtual bool IsPropertySupportedByControlForFindItem(int id)
		{
			return ItemsControlAutomationPeer.IsPropertySupportedByControlForFindItemInternal(id);
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x00222270 File Offset: 0x00221270
		internal static bool IsPropertySupportedByControlForFindItemInternal(int id)
		{
			return AutomationElementIdentifiers.NameProperty.Id == id || AutomationElementIdentifiers.AutomationIdProperty.Id == id || AutomationElementIdentifiers.ControlTypeProperty.Id == id;
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x002222A0 File Offset: 0x002212A0
		internal virtual object GetSupportedPropertyValue(ItemAutomationPeer itemPeer, int propertyId)
		{
			return ItemsControlAutomationPeer.GetSupportedPropertyValueInternal(itemPeer, propertyId);
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x002222A9 File Offset: 0x002212A9
		internal static object GetSupportedPropertyValueInternal(AutomationPeer itemPeer, int propertyId)
		{
			return itemPeer.GetPropertyValue(propertyId);
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x002222B4 File Offset: 0x002212B4
		protected internal virtual ItemAutomationPeer FindOrCreateItemAutomationPeer(object item)
		{
			ItemAutomationPeer itemAutomationPeer = this.ItemPeers[item];
			if (itemAutomationPeer == null)
			{
				itemAutomationPeer = this.GetPeerFromWeakRefStorage(item);
			}
			if (itemAutomationPeer == null)
			{
				itemAutomationPeer = this.CreateItemAutomationPeer(item);
				if (itemAutomationPeer != null)
				{
					itemAutomationPeer.TrySetParentInfo(this);
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

		// Token: 0x060044BB RID: 17595 RVA: 0x00222304 File Offset: 0x00221304
		internal ItemAutomationPeer CreateItemAutomationPeerInternal(object item)
		{
			return this.CreateItemAutomationPeer(item);
		}

		// Token: 0x060044BC RID: 17596
		protected abstract ItemAutomationPeer CreateItemAutomationPeer(object item);

		// Token: 0x060044BD RID: 17597 RVA: 0x00222310 File Offset: 0x00221310
		internal RecyclableWrapper GetRecyclableWrapperPeer(object item)
		{
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			if (this._recyclableWrapperCache == null)
			{
				this._recyclableWrapperCache = new RecyclableWrapper(itemsControl, item);
			}
			else
			{
				this._recyclableWrapperCache.LinkItem(item);
			}
			return this._recyclableWrapperCache;
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x00222352 File Offset: 0x00221352
		internal override IDisposable UpdateChildren()
		{
			base.UpdateChildrenInternal(5);
			this.WeakRefElementProxyStorage.PurgeWeakRefCollection();
			if (!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
			{
				return new ItemsControlAutomationPeer.UpdateChildrenHelper(this);
			}
			return null;
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x00222378 File Offset: 0x00221378
		internal ItemAutomationPeer GetPeerFromWeakRefStorage(object item)
		{
			ItemAutomationPeer itemAutomationPeer = null;
			WeakReference weakReference = this.WeakRefElementProxyStorage[item];
			if (weakReference != null)
			{
				ElementProxy elementProxy = weakReference.Target as ElementProxy;
				if (elementProxy != null)
				{
					itemAutomationPeer = (base.PeerFromProvider(elementProxy) as ItemAutomationPeer);
					if (itemAutomationPeer == null)
					{
						this.WeakRefElementProxyStorage.Remove(item);
					}
				}
				else
				{
					this.WeakRefElementProxyStorage.Remove(item);
				}
			}
			return itemAutomationPeer;
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x002223D4 File Offset: 0x002213D4
		internal AutomationPeer GetExistingPeerByItem(object item, bool checkInWeakRefStorage)
		{
			AutomationPeer automationPeer = null;
			if (checkInWeakRefStorage)
			{
				automationPeer = this.GetPeerFromWeakRefStorage(item);
			}
			if (automationPeer == null)
			{
				automationPeer = this.ItemPeers[item];
			}
			return automationPeer;
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x002223FF File Offset: 0x002213FF
		internal ItemAutomationPeer ReusablePeerFor(object item)
		{
			if (this._reusablePeers != null)
			{
				return this._reusablePeers[item];
			}
			return this.ItemPeers[item];
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x00222422 File Offset: 0x00221422
		private void ClearReusablePeers(ItemPeersStorage<ItemAutomationPeer> oldChildren)
		{
			if (this._reusablePeers == oldChildren)
			{
				this._reusablePeers = null;
			}
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x060044C3 RID: 17603 RVA: 0x00222434 File Offset: 0x00221434
		protected virtual bool IsVirtualized
		{
			get
			{
				return ItemContainerPatternIdentifiers.Pattern != null;
			}
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x060044C4 RID: 17604 RVA: 0x0022243E File Offset: 0x0022143E
		// (set) Token: 0x060044C5 RID: 17605 RVA: 0x00222446 File Offset: 0x00221446
		internal ItemPeersStorage<ItemAutomationPeer> ItemPeers
		{
			get
			{
				return this._dataChildren;
			}
			set
			{
				this._dataChildren = value;
			}
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x060044C6 RID: 17606 RVA: 0x0022244F File Offset: 0x0022144F
		// (set) Token: 0x060044C7 RID: 17607 RVA: 0x00222457 File Offset: 0x00221457
		internal ItemPeersStorage<WeakReference> WeakRefElementProxyStorage
		{
			get
			{
				return this._WeakRefElementProxyStorage;
			}
			set
			{
				this._WeakRefElementProxyStorage = value;
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x060044C8 RID: 17608 RVA: 0x00222460 File Offset: 0x00221460
		internal List<ItemAutomationPeer> RecentlyRealizedPeers
		{
			get
			{
				if (this._recentlyRealizedPeers == null)
				{
					this._recentlyRealizedPeers = new List<ItemAutomationPeer>();
				}
				return this._recentlyRealizedPeers;
			}
		}

		// Token: 0x04002532 RID: 9522
		private ItemPeersStorage<ItemAutomationPeer> _dataChildren = new ItemPeersStorage<ItemAutomationPeer>();

		// Token: 0x04002533 RID: 9523
		private ItemPeersStorage<ItemAutomationPeer> _reusablePeers;

		// Token: 0x04002534 RID: 9524
		private ItemPeersStorage<WeakReference> _WeakRefElementProxyStorage = new ItemPeersStorage<WeakReference>();

		// Token: 0x04002535 RID: 9525
		private List<ItemAutomationPeer> _recentlyRealizedPeers;

		// Token: 0x04002536 RID: 9526
		private RecyclableWrapper _recyclableWrapperCache;

		// Token: 0x02000B21 RID: 2849
		private class UpdateChildrenHelper : IDisposable
		{
			// Token: 0x06008C60 RID: 35936 RVA: 0x0033CD5C File Offset: 0x0033BD5C
			internal UpdateChildrenHelper(ItemsControlAutomationPeer peer)
			{
				this._peer = peer;
				this._oldChildren = peer.ItemPeers;
			}

			// Token: 0x06008C61 RID: 35937 RVA: 0x0033CD77 File Offset: 0x0033BD77
			void IDisposable.Dispose()
			{
				if (this._peer != null)
				{
					this._peer.ClearReusablePeers(this._oldChildren);
					this._peer = null;
				}
			}

			// Token: 0x040047DA RID: 18394
			private ItemsControlAutomationPeer _peer;

			// Token: 0x040047DB RID: 18395
			private ItemPeersStorage<ItemAutomationPeer> _oldChildren;
		}
	}
}
