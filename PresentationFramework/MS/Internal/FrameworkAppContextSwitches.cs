using System;
using System.Runtime.CompilerServices;

namespace MS.Internal
{
	// Token: 0x020000ED RID: 237
	internal static class FrameworkAppContextSwitches
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x000FF621 File Offset: 0x000FE621
		public static bool DoNotApplyLayoutRoundingToMarginsAndBorderThickness
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.MS.Internal.DoNotApplyLayoutRoundingToMarginsAndBorderThickness", ref FrameworkAppContextSwitches._doNotApplyLayoutRoundingToMarginsAndBorderThickness);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x000FF632 File Offset: 0x000FE632
		public static bool GridStarDefinitionsCanExceedAvailableSpace
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.Grid.StarDefinitionsCanExceedAvailableSpace", ref FrameworkAppContextSwitches._gridStarDefinitionsCanExceedAvailableSpace);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x000FF643 File Offset: 0x000FE643
		public static bool SelectionPropertiesCanLagBehindSelectionChangedEvent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.TabControl.SelectionPropertiesCanLagBehindSelectionChangedEvent", ref FrameworkAppContextSwitches._selectionPropertiesCanLagBehindSelectionChangedEvent);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x000FF654 File Offset: 0x000FE654
		public static bool DoNotUseFollowParentWhenBindingToADODataRelation
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Data.DoNotUseFollowParentWhenBindingToADODataRelation", ref FrameworkAppContextSwitches._doNotUseFollowParentWhenBindingToADODataRelation);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x000FF665 File Offset: 0x000FE665
		public static bool UseAdornerForTextboxSelectionRendering
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", ref FrameworkAppContextSwitches._useAdornerForTextboxSelectionRendering);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x000FF676 File Offset: 0x000FE676
		public static bool AppendLocalAssemblyVersionForSourceUri
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Baml2006.AppendLocalAssemblyVersionForSourceUri", ref FrameworkAppContextSwitches._AppendLocalAssemblyVersionForSourceUriSwitchName);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000FF687 File Offset: 0x000FE687
		public static bool IListIndexerHidesCustomIndexer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Data.Binding.IListIndexerHidesCustomIndexer", ref FrameworkAppContextSwitches._IListIndexerHidesCustomIndexer);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x000FF698 File Offset: 0x000FE698
		public static bool KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement", ref FrameworkAppContextSwitches._KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x000FF6A9 File Offset: 0x000FE6A9
		public static bool ItemAutomationPeerKeepsItsItemAlive
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Automation.Peers.ItemAutomationPeerKeepsItsItemAlive", ref FrameworkAppContextSwitches._ItemAutomationPeerKeepsItsItemAlive);
			}
		}

		// Token: 0x0400061B RID: 1563
		internal const string DoNotApplyLayoutRoundingToMarginsAndBorderThicknessSwitchName = "Switch.MS.Internal.DoNotApplyLayoutRoundingToMarginsAndBorderThickness";

		// Token: 0x0400061C RID: 1564
		private static int _doNotApplyLayoutRoundingToMarginsAndBorderThickness;

		// Token: 0x0400061D RID: 1565
		internal const string GridStarDefinitionsCanExceedAvailableSpaceSwitchName = "Switch.System.Windows.Controls.Grid.StarDefinitionsCanExceedAvailableSpace";

		// Token: 0x0400061E RID: 1566
		private static int _gridStarDefinitionsCanExceedAvailableSpace;

		// Token: 0x0400061F RID: 1567
		internal const string SelectionPropertiesCanLagBehindSelectionChangedEventSwitchName = "Switch.System.Windows.Controls.TabControl.SelectionPropertiesCanLagBehindSelectionChangedEvent";

		// Token: 0x04000620 RID: 1568
		private static int _selectionPropertiesCanLagBehindSelectionChangedEvent;

		// Token: 0x04000621 RID: 1569
		internal const string DoNotUseFollowParentWhenBindingToADODataRelationSwitchName = "Switch.System.Windows.Data.DoNotUseFollowParentWhenBindingToADODataRelation";

		// Token: 0x04000622 RID: 1570
		private static int _doNotUseFollowParentWhenBindingToADODataRelation;

		// Token: 0x04000623 RID: 1571
		internal const string UseAdornerForTextboxSelectionRenderingSwitchName = "Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering";

		// Token: 0x04000624 RID: 1572
		private static int _useAdornerForTextboxSelectionRendering;

		// Token: 0x04000625 RID: 1573
		internal const string AppendLocalAssemblyVersionForSourceUriSwitchName = "Switch.System.Windows.Baml2006.AppendLocalAssemblyVersionForSourceUri";

		// Token: 0x04000626 RID: 1574
		private static int _AppendLocalAssemblyVersionForSourceUriSwitchName;

		// Token: 0x04000627 RID: 1575
		internal const string IListIndexerHidesCustomIndexerSwitchName = "Switch.System.Windows.Data.Binding.IListIndexerHidesCustomIndexer";

		// Token: 0x04000628 RID: 1576
		private static int _IListIndexerHidesCustomIndexer;

		// Token: 0x04000629 RID: 1577
		internal const string KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElementSwitchName = "Switch.System.Windows.Controls.KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement";

		// Token: 0x0400062A RID: 1578
		private static int _KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement;

		// Token: 0x0400062B RID: 1579
		internal const string ItemAutomationPeerKeepsItsItemAliveSwitchName = "Switch.System.Windows.Automation.Peers.ItemAutomationPeerKeepsItsItemAlive";

		// Token: 0x0400062C RID: 1580
		private static int _ItemAutomationPeerKeepsItsItemAlive;
	}
}
