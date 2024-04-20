using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000557 RID: 1367
	public sealed class DatePickerAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, IValueProvider
	{
		// Token: 0x0600438B RID: 17291 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public DatePickerAutomationPeer(DatePicker owner) : base(owner)
		{
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x0600438C RID: 17292 RVA: 0x0021EB65 File Offset: 0x0021DB65
		private DatePicker OwningDatePicker
		{
			get
			{
				return base.Owner as DatePicker;
			}
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0021EB72 File Offset: 0x0021DB72
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.ExpandCollapse || patternInterface == PatternInterface.Value)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x0021EB88 File Offset: 0x0021DB88
		protected override void SetFocusCore()
		{
			DatePicker owningDatePicker = this.OwningDatePicker;
			if (owningDatePicker.Focusable)
			{
				if (!owningDatePicker.Focus())
				{
					TextBox textBox = owningDatePicker.TextBox;
					if (textBox == null || !textBox.IsKeyboardFocused)
					{
						throw new InvalidOperationException(SR.Get("SetFocusFailed"));
					}
				}
				return;
			}
			throw new InvalidOperationException(SR.Get("SetFocusFailed"));
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x001FBDEE File Offset: 0x001FADEE
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0021EBE0 File Offset: 0x0021DBE0
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> childrenCore = base.GetChildrenCore();
			if (this.OwningDatePicker.IsDropDownOpen && this.OwningDatePicker.Calendar != null)
			{
				CalendarAutomationPeer calendarAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDatePicker.Calendar) as CalendarAutomationPeer;
				if (calendarAutomationPeer != null)
				{
					childrenCore.Add(calendarAutomationPeer);
				}
			}
			return childrenCore;
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0021EC2F File Offset: 0x0021DC2F
		protected override string GetLocalizedControlTypeCore()
		{
			return SR.Get("DatePickerAutomationPeer_LocalizedControlType");
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06004393 RID: 17299 RVA: 0x0021EC3B File Offset: 0x0021DC3B
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				if (this.OwningDatePicker.IsDropDownOpen)
				{
					return ExpandCollapseState.Expanded;
				}
				return ExpandCollapseState.Collapsed;
			}
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0021EC4D File Offset: 0x0021DC4D
		void IExpandCollapseProvider.Collapse()
		{
			this.OwningDatePicker.IsDropDownOpen = false;
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0021EC5B File Offset: 0x0021DC5B
		void IExpandCollapseProvider.Expand()
		{
			this.OwningDatePicker.IsDropDownOpen = true;
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06004396 RID: 17302 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IValueProvider.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06004397 RID: 17303 RVA: 0x0021EC69 File Offset: 0x0021DC69
		string IValueProvider.Value
		{
			get
			{
				return this.OwningDatePicker.ToString();
			}
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x0021EC76 File Offset: 0x0021DC76
		void IValueProvider.SetValue(string value)
		{
			this.OwningDatePicker.Text = value;
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x0021CA91 File Offset: 0x0021BA91
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(string oldValue, string newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, oldValue, newValue);
			}
		}
	}
}
