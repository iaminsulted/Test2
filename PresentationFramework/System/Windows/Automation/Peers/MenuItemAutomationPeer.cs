using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using MS.Internal.KnownBoxes;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000580 RID: 1408
	public class MenuItemAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, IInvokeProvider, IToggleProvider
	{
		// Token: 0x06004501 RID: 17665 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public MenuItemAutomationPeer(MenuItem owner) : base(owner)
		{
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x00222B41 File Offset: 0x00221B41
		protected override string GetClassNameCore()
		{
			return "MenuItem";
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x001FCAA1 File Offset: 0x001FBAA1
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.MenuItem;
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x00222B48 File Offset: 0x00221B48
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			MenuItem menuItem = (MenuItem)base.Owner;
			if (patternInterface == PatternInterface.ExpandCollapse)
			{
				MenuItemRole role = menuItem.Role;
				if ((role == MenuItemRole.TopLevelHeader || role == MenuItemRole.SubmenuHeader) && menuItem.HasItems)
				{
					result = this;
				}
			}
			else if (patternInterface == PatternInterface.Toggle)
			{
				if (menuItem.IsCheckable)
				{
					result = this;
				}
			}
			else if (patternInterface == PatternInterface.Invoke)
			{
				MenuItemRole role2 = menuItem.Role;
				if ((role2 == MenuItemRole.TopLevelItem || role2 == MenuItemRole.SubmenuItem) && !menuItem.HasItems)
				{
					result = this;
				}
			}
			else if (patternInterface == PatternInterface.SynchronizedInput)
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00222BC0 File Offset: 0x00221BC0
		protected override int GetSizeOfSetCore()
		{
			int num = base.GetSizeOfSetCore();
			if (num == -1)
			{
				MenuItem menuItem = (MenuItem)base.Owner;
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(menuItem);
				num = ItemAutomationPeer.GetSizeOfSetFromItemsControl(itemsControl, menuItem);
				using (IEnumerator enumerator = ((IEnumerable)itemsControl.Items).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Separator)
						{
							num--;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x00222C3C File Offset: 0x00221C3C
		protected override int GetPositionInSetCore()
		{
			int num = base.GetPositionInSetCore();
			if (num == -1)
			{
				MenuItem menuItem = (MenuItem)base.Owner;
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(menuItem);
				num = ItemAutomationPeer.GetPositionInSetFromItemsControl(itemsControl, menuItem);
				foreach (object obj in ((IEnumerable)itemsControl.Items))
				{
					if (obj == menuItem)
					{
						break;
					}
					if (obj is Separator)
					{
						num--;
					}
				}
			}
			return num;
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x00222CC4 File Offset: 0x00221CC4
		protected override string GetAccessKeyCore()
		{
			string text = base.GetAccessKeyCore();
			if (!string.IsNullOrEmpty(text))
			{
				MenuItemRole role = ((MenuItem)base.Owner).Role;
				if (role == MenuItemRole.TopLevelHeader || role == MenuItemRole.TopLevelItem)
				{
					text = "Alt+" + text;
				}
			}
			return text;
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x00222D08 File Offset: 0x00221D08
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			if (ExpandCollapseState.Expanded == ((IExpandCollapseProvider)this).ExpandCollapseState)
			{
				ItemsControl itemsControl = (ItemsControl)base.Owner;
				ItemCollection items = itemsControl.Items;
				if (items.Count > 0)
				{
					list = new List<AutomationPeer>(items.Count);
					for (int i = 0; i < items.Count; i++)
					{
						UIElement uielement = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as UIElement;
						if (uielement != null)
						{
							AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(uielement);
							if (automationPeer == null)
							{
								automationPeer = UIElementAutomationPeer.CreatePeerForElement(uielement);
							}
							if (automationPeer != null)
							{
								list.Add(automationPeer);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x00222D98 File Offset: 0x00221D98
		void IExpandCollapseProvider.Expand()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			MenuItem menuItem = (MenuItem)base.Owner;
			MenuItemRole role = menuItem.Role;
			if ((role != MenuItemRole.TopLevelHeader && role != MenuItemRole.SubmenuHeader) || !menuItem.HasItems)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			menuItem.OpenMenu();
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x00222DF0 File Offset: 0x00221DF0
		void IExpandCollapseProvider.Collapse()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			MenuItem menuItem = (MenuItem)base.Owner;
			MenuItemRole role = menuItem.Role;
			if ((role != MenuItemRole.TopLevelHeader && role != MenuItemRole.SubmenuHeader) || !menuItem.HasItems)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			menuItem.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x0600450B RID: 17675 RVA: 0x00222E50 File Offset: 0x00221E50
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				ExpandCollapseState result = ExpandCollapseState.Collapsed;
				MenuItem menuItem = (MenuItem)base.Owner;
				MenuItemRole role = menuItem.Role;
				if (role == MenuItemRole.TopLevelItem || role == MenuItemRole.SubmenuItem || !menuItem.HasItems)
				{
					result = ExpandCollapseState.LeafNode;
				}
				else if (menuItem.IsSubmenuOpen)
				{
					result = ExpandCollapseState.Expanded;
				}
				return result;
			}
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x00222E90 File Offset: 0x00221E90
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			MenuItem menuItem = (MenuItem)base.Owner;
			MenuItemRole role = menuItem.Role;
			if (role == MenuItemRole.TopLevelItem || role == MenuItemRole.SubmenuItem)
			{
				menuItem.ClickItem();
				return;
			}
			if (role == MenuItemRole.TopLevelHeader || role == MenuItemRole.SubmenuHeader)
			{
				menuItem.ClickHeader();
			}
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00222EDC File Offset: 0x00221EDC
		void IToggleProvider.Toggle()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			MenuItem menuItem = (MenuItem)base.Owner;
			if (!menuItem.IsCheckable)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			menuItem.SetCurrentValueInternal(MenuItem.IsCheckedProperty, BooleanBoxes.Box(!menuItem.IsChecked));
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x0600450E RID: 17678 RVA: 0x00222F34 File Offset: 0x00221F34
		ToggleState IToggleProvider.ToggleState
		{
			get
			{
				if (!((MenuItem)base.Owner).IsChecked)
				{
					return ToggleState.Off;
				}
				return ToggleState.On;
			}
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x0021CB14 File Offset: 0x0021BB14
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x00222F4C File Offset: 0x00221F4C
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			if (!string.IsNullOrEmpty(nameCore) && ((MenuItem)base.Owner).Header is string)
			{
				return AccessText.RemoveAccessKeyMarker(nameCore);
			}
			return nameCore;
		}
	}
}
