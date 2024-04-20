using System;

namespace System.Windows.Controls
{
	// Token: 0x0200080A RID: 2058
	internal static class VisualStates
	{
		// Token: 0x06007876 RID: 30838 RVA: 0x00300654 File Offset: 0x002FF654
		public static void GoToState(Control control, bool useTransitions, params string[] stateNames)
		{
			if (stateNames == null)
			{
				return;
			}
			foreach (string stateName in stateNames)
			{
				if (VisualStateManager.GoToState(control, stateName, useTransitions))
				{
					break;
				}
			}
		}

		// Token: 0x04003909 RID: 14601
		internal const string StateToday = "Today";

		// Token: 0x0400390A RID: 14602
		internal const string StateRegularDay = "RegularDay";

		// Token: 0x0400390B RID: 14603
		internal const string GroupDay = "DayStates";

		// Token: 0x0400390C RID: 14604
		internal const string StateBlackoutDay = "BlackoutDay";

		// Token: 0x0400390D RID: 14605
		internal const string StateNormalDay = "NormalDay";

		// Token: 0x0400390E RID: 14606
		internal const string GroupBlackout = "BlackoutDayStates";

		// Token: 0x0400390F RID: 14607
		public const string StateCalendarButtonUnfocused = "CalendarButtonUnfocused";

		// Token: 0x04003910 RID: 14608
		public const string StateCalendarButtonFocused = "CalendarButtonFocused";

		// Token: 0x04003911 RID: 14609
		public const string GroupCalendarButtonFocus = "CalendarButtonFocusStates";

		// Token: 0x04003912 RID: 14610
		public const string StateNormal = "Normal";

		// Token: 0x04003913 RID: 14611
		public const string StateMouseOver = "MouseOver";

		// Token: 0x04003914 RID: 14612
		public const string StatePressed = "Pressed";

		// Token: 0x04003915 RID: 14613
		public const string StateDisabled = "Disabled";

		// Token: 0x04003916 RID: 14614
		public const string StateReadOnly = "ReadOnly";

		// Token: 0x04003917 RID: 14615
		internal const string StateDeterminate = "Determinate";

		// Token: 0x04003918 RID: 14616
		public const string GroupCommon = "CommonStates";

		// Token: 0x04003919 RID: 14617
		public const string StateUnfocused = "Unfocused";

		// Token: 0x0400391A RID: 14618
		public const string StateFocused = "Focused";

		// Token: 0x0400391B RID: 14619
		public const string StateFocusedDropDown = "FocusedDropDown";

		// Token: 0x0400391C RID: 14620
		public const string GroupFocus = "FocusStates";

		// Token: 0x0400391D RID: 14621
		public const string StateExpanded = "Expanded";

		// Token: 0x0400391E RID: 14622
		public const string StateCollapsed = "Collapsed";

		// Token: 0x0400391F RID: 14623
		public const string GroupExpansion = "ExpansionStates";

		// Token: 0x04003920 RID: 14624
		public const string StateOpen = "Open";

		// Token: 0x04003921 RID: 14625
		public const string StateClosed = "Closed";

		// Token: 0x04003922 RID: 14626
		public const string GroupOpen = "OpenStates";

		// Token: 0x04003923 RID: 14627
		public const string StateHasItems = "HasItems";

		// Token: 0x04003924 RID: 14628
		public const string StateNoItems = "NoItems";

		// Token: 0x04003925 RID: 14629
		public const string GroupHasItems = "HasItemsStates";

		// Token: 0x04003926 RID: 14630
		public const string StateExpandDown = "ExpandDown";

		// Token: 0x04003927 RID: 14631
		public const string StateExpandUp = "ExpandUp";

		// Token: 0x04003928 RID: 14632
		public const string StateExpandLeft = "ExpandLeft";

		// Token: 0x04003929 RID: 14633
		public const string StateExpandRight = "ExpandRight";

		// Token: 0x0400392A RID: 14634
		public const string GroupExpandDirection = "ExpandDirectionStates";

		// Token: 0x0400392B RID: 14635
		public const string StateSelected = "Selected";

		// Token: 0x0400392C RID: 14636
		public const string StateSelectedUnfocused = "SelectedUnfocused";

		// Token: 0x0400392D RID: 14637
		public const string StateSelectedInactive = "SelectedInactive";

		// Token: 0x0400392E RID: 14638
		public const string StateUnselected = "Unselected";

		// Token: 0x0400392F RID: 14639
		public const string GroupSelection = "SelectionStates";

		// Token: 0x04003930 RID: 14640
		public const string StateEditable = "Editable";

		// Token: 0x04003931 RID: 14641
		public const string StateUneditable = "Uneditable";

		// Token: 0x04003932 RID: 14642
		public const string GroupEdit = "EditStates";

		// Token: 0x04003933 RID: 14643
		public const string StateActive = "Active";

		// Token: 0x04003934 RID: 14644
		public const string StateInactive = "Inactive";

		// Token: 0x04003935 RID: 14645
		public const string GroupActive = "ActiveStates";

		// Token: 0x04003936 RID: 14646
		public const string StateValid = "Valid";

		// Token: 0x04003937 RID: 14647
		public const string StateInvalidFocused = "InvalidFocused";

		// Token: 0x04003938 RID: 14648
		public const string StateInvalidUnfocused = "InvalidUnfocused";

		// Token: 0x04003939 RID: 14649
		public const string GroupValidation = "ValidationStates";

		// Token: 0x0400393A RID: 14650
		public const string StateUnwatermarked = "Unwatermarked";

		// Token: 0x0400393B RID: 14651
		public const string StateWatermarked = "Watermarked";

		// Token: 0x0400393C RID: 14652
		public const string GroupWatermark = "WatermarkStates";

		// Token: 0x0400393D RID: 14653
		public const string StateChecked = "Checked";

		// Token: 0x0400393E RID: 14654
		public const string StateUnchecked = "Unchecked";

		// Token: 0x0400393F RID: 14655
		public const string StateIndeterminate = "Indeterminate";

		// Token: 0x04003940 RID: 14656
		public const string GroupCheck = "CheckStates";

		// Token: 0x04003941 RID: 14657
		public const string StateRegular = "Regular";

		// Token: 0x04003942 RID: 14658
		public const string StateCurrent = "Current";

		// Token: 0x04003943 RID: 14659
		public const string GroupCurrent = "CurrentStates";

		// Token: 0x04003944 RID: 14660
		public const string StateDisplay = "Display";

		// Token: 0x04003945 RID: 14661
		public const string StateEditing = "Editing";

		// Token: 0x04003946 RID: 14662
		public const string GroupInteraction = "InteractionStates";

		// Token: 0x04003947 RID: 14663
		public const string StateUnsorted = "Unsorted";

		// Token: 0x04003948 RID: 14664
		public const string StateSortAscending = "SortAscending";

		// Token: 0x04003949 RID: 14665
		public const string StateSortDescending = "SortDescending";

		// Token: 0x0400394A RID: 14666
		public const string GroupSort = "SortStates";

		// Token: 0x0400394B RID: 14667
		public const string DATAGRIDROW_stateAlternate = "Normal_AlternatingRow";

		// Token: 0x0400394C RID: 14668
		public const string DATAGRIDROW_stateMouseOver = "MouseOver";

		// Token: 0x0400394D RID: 14669
		public const string DATAGRIDROW_stateMouseOverEditing = "MouseOver_Unfocused_Editing";

		// Token: 0x0400394E RID: 14670
		public const string DATAGRIDROW_stateMouseOverEditingFocused = "MouseOver_Editing";

		// Token: 0x0400394F RID: 14671
		public const string DATAGRIDROW_stateMouseOverSelected = "MouseOver_Unfocused_Selected";

		// Token: 0x04003950 RID: 14672
		public const string DATAGRIDROW_stateMouseOverSelectedFocused = "MouseOver_Selected";

		// Token: 0x04003951 RID: 14673
		public const string DATAGRIDROW_stateNormal = "Normal";

		// Token: 0x04003952 RID: 14674
		public const string DATAGRIDROW_stateNormalEditing = "Unfocused_Editing";

		// Token: 0x04003953 RID: 14675
		public const string DATAGRIDROW_stateNormalEditingFocused = "Normal_Editing";

		// Token: 0x04003954 RID: 14676
		public const string DATAGRIDROW_stateSelected = "Unfocused_Selected";

		// Token: 0x04003955 RID: 14677
		public const string DATAGRIDROW_stateSelectedFocused = "Normal_Selected";

		// Token: 0x04003956 RID: 14678
		public const string DATAGRIDROWHEADER_stateMouseOver = "MouseOver";

		// Token: 0x04003957 RID: 14679
		public const string DATAGRIDROWHEADER_stateMouseOverCurrentRow = "MouseOver_CurrentRow";

		// Token: 0x04003958 RID: 14680
		public const string DATAGRIDROWHEADER_stateMouseOverEditingRow = "MouseOver_Unfocused_EditingRow";

		// Token: 0x04003959 RID: 14681
		public const string DATAGRIDROWHEADER_stateMouseOverEditingRowFocused = "MouseOver_EditingRow";

		// Token: 0x0400395A RID: 14682
		public const string DATAGRIDROWHEADER_stateMouseOverSelected = "MouseOver_Unfocused_Selected";

		// Token: 0x0400395B RID: 14683
		public const string DATAGRIDROWHEADER_stateMouseOverSelectedCurrentRow = "MouseOver_Unfocused_CurrentRow_Selected";

		// Token: 0x0400395C RID: 14684
		public const string DATAGRIDROWHEADER_stateMouseOverSelectedCurrentRowFocused = "MouseOver_CurrentRow_Selected";

		// Token: 0x0400395D RID: 14685
		public const string DATAGRIDROWHEADER_stateMouseOverSelectedFocused = "MouseOver_Selected";

		// Token: 0x0400395E RID: 14686
		public const string DATAGRIDROWHEADER_stateNormal = "Normal";

		// Token: 0x0400395F RID: 14687
		public const string DATAGRIDROWHEADER_stateNormalCurrentRow = "Normal_CurrentRow";

		// Token: 0x04003960 RID: 14688
		public const string DATAGRIDROWHEADER_stateNormalEditingRow = "Unfocused_EditingRow";

		// Token: 0x04003961 RID: 14689
		public const string DATAGRIDROWHEADER_stateNormalEditingRowFocused = "Normal_EditingRow";

		// Token: 0x04003962 RID: 14690
		public const string DATAGRIDROWHEADER_stateSelected = "Unfocused_Selected";

		// Token: 0x04003963 RID: 14691
		public const string DATAGRIDROWHEADER_stateSelectedCurrentRow = "Unfocused_CurrentRow_Selected";

		// Token: 0x04003964 RID: 14692
		public const string DATAGRIDROWHEADER_stateSelectedCurrentRowFocused = "Normal_CurrentRow_Selected";

		// Token: 0x04003965 RID: 14693
		public const string DATAGRIDROWHEADER_stateSelectedFocused = "Normal_Selected";
	}
}
