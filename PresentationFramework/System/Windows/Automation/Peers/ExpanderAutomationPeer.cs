using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200055D RID: 1373
	public class ExpanderAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider
	{
		// Token: 0x060043EF RID: 17391 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public ExpanderAutomationPeer(Expander owner) : base(owner)
		{
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0021FC32 File Offset: 0x0021EC32
		protected override string GetClassNameCore()
		{
			return "Expander";
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x0021FC39 File Offset: 0x0021EC39
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Group;
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x0021FC40 File Offset: 0x0021EC40
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> childrenCore = base.GetChildrenCore();
			ToggleButton expanderToggleButton = ((Expander)base.Owner).ExpanderToggleButton;
			if (!AccessibilitySwitches.UseNetFx47CompatibleAccessibilityFeatures && childrenCore != null)
			{
				foreach (AutomationPeer automationPeer in childrenCore)
				{
					UIElementAutomationPeer uielementAutomationPeer = (UIElementAutomationPeer)automationPeer;
					if (uielementAutomationPeer.Owner == expanderToggleButton)
					{
						uielementAutomationPeer.EventsSource = ((!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures && base.EventsSource != null) ? base.EventsSource : this);
						break;
					}
				}
			}
			return childrenCore;
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0021FCD8 File Offset: 0x0021ECD8
		protected override bool HasKeyboardFocusCore()
		{
			return (!AccessibilitySwitches.UseNetFx47CompatibleAccessibilityFeatures && ((Expander)base.Owner).IsExpanderToggleButtonFocused) || base.HasKeyboardFocusCore();
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0021FCFC File Offset: 0x0021ECFC
		public override object GetPattern(PatternInterface pattern)
		{
			object result;
			if (pattern == PatternInterface.ExpandCollapse)
			{
				result = this;
			}
			else
			{
				result = base.GetPattern(pattern);
			}
			return result;
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0021FD1C File Offset: 0x0021ED1C
		void IExpandCollapseProvider.Expand()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((Expander)this.Owner).IsExpanded = true;
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x0021FD3D File Offset: 0x0021ED3D
		void IExpandCollapseProvider.Collapse()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((Expander)this.Owner).IsExpanded = false;
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x060043F7 RID: 17399 RVA: 0x0021FD5E File Offset: 0x0021ED5E
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				if (!((Expander)this.Owner).IsExpanded)
				{
					return ExpandCollapseState.Collapsed;
				}
				return ExpandCollapseState.Expanded;
			}
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x0021CB14 File Offset: 0x0021BB14
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}
	}
}
