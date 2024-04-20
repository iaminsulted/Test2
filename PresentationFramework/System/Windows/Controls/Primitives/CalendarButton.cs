using System;
using System.Windows.Automation.Peers;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000824 RID: 2084
	public sealed class CalendarButton : Button
	{
		// Token: 0x0600799D RID: 31133 RVA: 0x00303C18 File Offset: 0x00302C18
		static CalendarButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarButton), new FrameworkPropertyMetadata(typeof(CalendarButton)));
		}

		// Token: 0x17001C2B RID: 7211
		// (get) Token: 0x0600799F RID: 31135 RVA: 0x00303CE2 File Offset: 0x00302CE2
		// (set) Token: 0x060079A0 RID: 31136 RVA: 0x00303CF4 File Offset: 0x00302CF4
		public bool HasSelectedDays
		{
			get
			{
				return (bool)base.GetValue(CalendarButton.HasSelectedDaysProperty);
			}
			internal set
			{
				base.SetValue(CalendarButton.HasSelectedDaysPropertyKey, value);
			}
		}

		// Token: 0x17001C2C RID: 7212
		// (get) Token: 0x060079A1 RID: 31137 RVA: 0x00303D02 File Offset: 0x00302D02
		// (set) Token: 0x060079A2 RID: 31138 RVA: 0x00303D14 File Offset: 0x00302D14
		public bool IsInactive
		{
			get
			{
				return (bool)base.GetValue(CalendarButton.IsInactiveProperty);
			}
			internal set
			{
				base.SetValue(CalendarButton.IsInactivePropertyKey, value);
			}
		}

		// Token: 0x17001C2D RID: 7213
		// (get) Token: 0x060079A3 RID: 31139 RVA: 0x00303D22 File Offset: 0x00302D22
		// (set) Token: 0x060079A4 RID: 31140 RVA: 0x00303D2A File Offset: 0x00302D2A
		internal Calendar Owner { get; set; }

		// Token: 0x060079A5 RID: 31141 RVA: 0x00303D34 File Offset: 0x00302D34
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (this.HasSelectedDays)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Selected",
					"Unselected"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unselected", useTransitions);
			}
			if (!this.IsInactive)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Active",
					"Inactive"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Inactive", useTransitions);
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

		// Token: 0x060079A6 RID: 31142 RVA: 0x00303DE4 File Offset: 0x00302DE4
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new CalendarButtonAutomationPeer(this);
		}

		// Token: 0x060079A7 RID: 31143 RVA: 0x00303DEC File Offset: 0x00302DEC
		internal void SetContentInternal(string value)
		{
			base.SetCurrentValueInternal(ContentControl.ContentProperty, value);
		}

		// Token: 0x040039AF RID: 14767
		internal static readonly DependencyPropertyKey HasSelectedDaysPropertyKey = DependencyProperty.RegisterReadOnly("HasSelectedDays", typeof(bool), typeof(CalendarButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x040039B0 RID: 14768
		public static readonly DependencyProperty HasSelectedDaysProperty = CalendarButton.HasSelectedDaysPropertyKey.DependencyProperty;

		// Token: 0x040039B1 RID: 14769
		internal static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly("IsInactive", typeof(bool), typeof(CalendarButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x040039B2 RID: 14770
		public static readonly DependencyProperty IsInactiveProperty = CalendarButton.IsInactivePropertyKey.DependencyProperty;
	}
}
