using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200056C RID: 1388
	public class GroupItemAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004462 RID: 17506 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public GroupItemAutomationPeer(GroupItem owner) : base(owner)
		{
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x00220EBB File Offset: 0x0021FEBB
		protected override string GetClassNameCore()
		{
			return "GroupItem";
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x00220EC4 File Offset: 0x0021FEC4
		protected override int GetPositionInSetCore()
		{
			int num = base.GetPositionInSetCore();
			if (num == -1)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = ((GroupItem)base.Owner).GetValue(ItemContainerGenerator.ItemForItemContainerProperty) as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					CollectionViewGroup parent = collectionViewGroupInternal.Parent;
					if (parent != null)
					{
						num = parent.Items.IndexOf(collectionViewGroupInternal) + 1;
					}
				}
			}
			return num;
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x00220F14 File Offset: 0x0021FF14
		protected override int GetSizeOfSetCore()
		{
			int num = base.GetSizeOfSetCore();
			if (num == -1)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = ((GroupItem)base.Owner).GetValue(ItemContainerGenerator.ItemForItemContainerProperty) as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					CollectionViewGroup parent = collectionViewGroupInternal.Parent;
					if (parent != null)
					{
						num = parent.Items.Count;
					}
				}
			}
			return num;
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x0021FC39 File Offset: 0x0021EC39
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Group;
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x00220F64 File Offset: 0x0021FF64
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.ExpandCollapse)
			{
				GroupItem groupItem = (GroupItem)base.Owner;
				if (groupItem.Expander != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(groupItem.Expander);
					if (automationPeer != null && automationPeer is IExpandCollapseProvider)
					{
						automationPeer.EventsSource = this;
						return (IExpandCollapseProvider)automationPeer;
					}
				}
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x00220FB8 File Offset: 0x0021FFB8
		protected override List<AutomationPeer> GetChildrenCore()
		{
			GroupItem groupItem = (GroupItem)base.Owner;
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(base.Owner);
			if (itemsControl != null)
			{
				ItemsControlAutomationPeer itemsControlAutomationPeer = itemsControl.CreateAutomationPeer() as ItemsControlAutomationPeer;
				if (itemsControlAutomationPeer != null)
				{
					List<AutomationPeer> list = new List<AutomationPeer>();
					bool useNetFx472CompatibleAccessibilityFeatures = AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures;
					if (!useNetFx472CompatibleAccessibilityFeatures && groupItem.Expander != null)
					{
						this._expanderPeer = UIElementAutomationPeer.CreatePeerForElement(groupItem.Expander);
						if (this._expanderPeer != null)
						{
							this._expanderPeer.EventsSource = this;
							this._expanderPeer.GetChildren();
						}
					}
					Panel itemsHost = groupItem.ItemsHost;
					if (itemsHost != null)
					{
						IEnumerable children = itemsHost.Children;
						ItemPeersStorage<ItemAutomationPeer> itemPeersStorage = new ItemPeersStorage<ItemAutomationPeer>();
						foreach (object obj in children)
						{
							UIElement uielement = (UIElement)obj;
							if (!((IGeneratorHost)itemsControl).IsItemItsOwnContainer(uielement))
							{
								UIElementAutomationPeer uielementAutomationPeer = uielement.CreateAutomationPeer() as UIElementAutomationPeer;
								if (uielementAutomationPeer != null)
								{
									list.Add(uielementAutomationPeer);
									if (useNetFx472CompatibleAccessibilityFeatures)
									{
										if (itemsControlAutomationPeer.RecentlyRealizedPeers.Count > 0 && this.AncestorsInvalid)
										{
											GroupItemAutomationPeer groupItemAutomationPeer = uielementAutomationPeer as GroupItemAutomationPeer;
											if (groupItemAutomationPeer != null)
											{
												groupItemAutomationPeer.InvalidateGroupItemPeersContainingRecentlyRealizedPeers(itemsControlAutomationPeer.RecentlyRealizedPeers);
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
							else
							{
								object obj2 = itemsControl.ItemContainerGenerator.ItemFromContainer(uielement);
								if (obj2 != DependencyProperty.UnsetValue)
								{
									ItemAutomationPeer itemAutomationPeer = useNetFx472CompatibleAccessibilityFeatures ? itemsControlAutomationPeer.ItemPeers[obj2] : itemsControlAutomationPeer.ReusablePeerFor(obj2);
									itemAutomationPeer = itemsControlAutomationPeer.ReusePeerForItem(itemAutomationPeer, obj2);
									if (itemAutomationPeer != null)
									{
										if (useNetFx472CompatibleAccessibilityFeatures)
										{
											int num = itemsControlAutomationPeer.RecentlyRealizedPeers.IndexOf(itemAutomationPeer);
											if (num >= 0)
											{
												itemsControlAutomationPeer.RecentlyRealizedPeers.RemoveAt(num);
											}
										}
									}
									else
									{
										itemAutomationPeer = itemsControlAutomationPeer.CreateItemAutomationPeerInternal(obj2);
									}
									if (itemAutomationPeer != null)
									{
										AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
										if (wrapperPeer != null)
										{
											wrapperPeer.EventsSource = itemAutomationPeer;
											if (itemAutomationPeer.ChildrenValid && itemAutomationPeer.Children == null && this.AncestorsInvalid)
											{
												itemAutomationPeer.AncestorsInvalid = true;
												wrapperPeer.AncestorsInvalid = true;
											}
										}
									}
									bool flag = itemsControlAutomationPeer.ItemPeers[obj2] == null;
									if (itemAutomationPeer != null && (flag || (itemAutomationPeer.GetParent() == this && itemPeersStorage[obj2] == null)))
									{
										list.Add(itemAutomationPeer);
										itemPeersStorage[obj2] = itemAutomationPeer;
										if (flag)
										{
											itemsControlAutomationPeer.ItemPeers[obj2] = itemAutomationPeer;
										}
									}
								}
							}
						}
						return list;
					}
					if (this._expanderPeer == null)
					{
						return null;
					}
					list.Add(this._expanderPeer);
					return list;
				}
			}
			return null;
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0022127C File Offset: 0x0022027C
		internal void InvalidateGroupItemPeersContainingRecentlyRealizedPeers(List<ItemAutomationPeer> recentlyRealizedPeers)
		{
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(base.Owner);
			if (itemsControl != null)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = itemsControl.ItemContainerGenerator.ItemFromContainer(base.Owner) as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					for (int i = 0; i < recentlyRealizedPeers.Count; i++)
					{
						object item = recentlyRealizedPeers[i].Item;
						if (collectionViewGroupInternal.LeafIndexOf(item) >= 0)
						{
							this.AncestorsInvalid = true;
							base.ChildrenValid = true;
						}
					}
				}
			}
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x002212E8 File Offset: 0x002202E8
		protected override void SetFocusCore()
		{
			GroupItem groupItem = (GroupItem)base.Owner;
			if (!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures && groupItem.Expander != null)
			{
				ToggleButton expanderToggleButton = groupItem.Expander.ExpanderToggleButton;
				if (expanderToggleButton == null || !expanderToggleButton.Focus())
				{
					throw new InvalidOperationException(SR.Get("SetFocusFailed"));
				}
			}
			else
			{
				base.SetFocusCore();
			}
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x00221342 File Offset: 0x00220342
		protected override bool IsKeyboardFocusableCore()
		{
			if (this._expanderPeer != null)
			{
				return this._expanderPeer.IsKeyboardFocusable();
			}
			return base.IsKeyboardFocusableCore();
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0022135E File Offset: 0x0022035E
		protected override bool HasKeyboardFocusCore()
		{
			if (this._expanderPeer != null)
			{
				return this._expanderPeer.HasKeyboardFocus();
			}
			return base.HasKeyboardFocusCore();
		}

		// Token: 0x0400252E RID: 9518
		private AutomationPeer _expanderPeer;
	}
}
