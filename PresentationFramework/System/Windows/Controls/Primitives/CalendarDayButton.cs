using System;
using System.Windows.Automation.Peers;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000825 RID: 2085
	public sealed class CalendarDayButton : Button
	{
		// Token: 0x060079A8 RID: 31144 RVA: 0x00303DFC File Offset: 0x00302DFC
		static CalendarDayButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarDayButton), new FrameworkPropertyMetadata(typeof(CalendarDayButton)));
		}

		// Token: 0x17001C2E RID: 7214
		// (get) Token: 0x060079AA RID: 31146 RVA: 0x00303F99 File Offset: 0x00302F99
		public bool IsToday
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsTodayProperty);
			}
		}

		// Token: 0x17001C2F RID: 7215
		// (get) Token: 0x060079AB RID: 31147 RVA: 0x00303FAB File Offset: 0x00302FAB
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsSelectedProperty);
			}
		}

		// Token: 0x17001C30 RID: 7216
		// (get) Token: 0x060079AC RID: 31148 RVA: 0x00303FBD File Offset: 0x00302FBD
		public bool IsInactive
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsInactiveProperty);
			}
		}

		// Token: 0x17001C31 RID: 7217
		// (get) Token: 0x060079AD RID: 31149 RVA: 0x00303FCF File Offset: 0x00302FCF
		public bool IsBlackedOut
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsBlackedOutProperty);
			}
		}

		// Token: 0x17001C32 RID: 7218
		// (get) Token: 0x060079AE RID: 31150 RVA: 0x00303FE1 File Offset: 0x00302FE1
		public bool IsHighlighted
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsHighlightedProperty);
			}
		}

		// Token: 0x17001C33 RID: 7219
		// (get) Token: 0x060079AF RID: 31151 RVA: 0x00303FF3 File Offset: 0x00302FF3
		// (set) Token: 0x060079B0 RID: 31152 RVA: 0x00303FFB File Offset: 0x00302FFB
		internal Calendar Owner { get; set; }

		// Token: 0x060079B1 RID: 31153 RVA: 0x00303DE4 File Offset: 0x00302DE4
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new CalendarButtonAutomationPeer(this);
		}

		// Token: 0x060079B2 RID: 31154 RVA: 0x00304004 File Offset: 0x00303004
		internal override void ChangeVisualState(bool useTransitions)
		{
			VisualStates.GoToState(this, useTransitions, new string[]
			{
				"Active",
				"Inactive"
			});
			if (this.IsInactive)
			{
				VisualStateManager.GoToState(this, "Inactive", useTransitions);
			}
			VisualStateManager.GoToState(this, "RegularDay", useTransitions);
			if (this.IsToday && this.Owner != null && this.Owner.IsTodayHighlighted)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Today",
					"RegularDay"
				});
			}
			VisualStateManager.GoToState(this, "Unselected", useTransitions);
			if (this.IsSelected || this.IsHighlighted)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Selected",
					"Unselected"
				});
			}
			if (this.IsBlackedOut)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"BlackoutDay",
					"NormalDay"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "NormalDay", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"CalendarButtonFocused",
					"CalendarButtonUnfocused"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "CalendarButtonUnfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x060079B3 RID: 31155 RVA: 0x002A8649 File Offset: 0x002A7649
		internal void NotifyNeedsVisualStateUpdate()
		{
			base.UpdateVisualState();
		}

		// Token: 0x060079B4 RID: 31156 RVA: 0x00303DEC File Offset: 0x00302DEC
		internal void SetContentInternal(string value)
		{
			base.SetCurrentValueInternal(ContentControl.ContentProperty, value);
		}

		// Token: 0x040039B4 RID: 14772
		private const int DEFAULTCONTENT = 1;

		// Token: 0x040039B5 RID: 14773
		internal static readonly DependencyPropertyKey IsTodayPropertyKey = DependencyProperty.RegisterReadOnly("IsToday", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x040039B6 RID: 14774
		public static readonly DependencyProperty IsTodayProperty = CalendarDayButton.IsTodayPropertyKey.DependencyProperty;

		// Token: 0x040039B7 RID: 14775
		internal static readonly DependencyPropertyKey IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly("IsSelected", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x040039B8 RID: 14776
		public static readonly DependencyProperty IsSelectedProperty = CalendarDayButton.IsSelectedPropertyKey.DependencyProperty;

		// Token: 0x040039B9 RID: 14777
		internal static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly("IsInactive", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x040039BA RID: 14778
		public static readonly DependencyProperty IsInactiveProperty = CalendarDayButton.IsInactivePropertyKey.DependencyProperty;

		// Token: 0x040039BB RID: 14779
		internal static readonly DependencyPropertyKey IsBlackedOutPropertyKey = DependencyProperty.RegisterReadOnly("IsBlackedOut", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x040039BC RID: 14780
		public static readonly DependencyProperty IsBlackedOutProperty = CalendarDayButton.IsBlackedOutPropertyKey.DependencyProperty;

		// Token: 0x040039BD RID: 14781
		internal static readonly DependencyPropertyKey IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x040039BE RID: 14782
		public static readonly DependencyProperty IsHighlightedProperty = CalendarDayButton.IsHighlightedPropertyKey.DependencyProperty;
	}
}
