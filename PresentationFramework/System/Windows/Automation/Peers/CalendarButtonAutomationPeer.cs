using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000548 RID: 1352
	public sealed class CalendarButtonAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060042C4 RID: 17092 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public CalendarButtonAutomationPeer(Button owner) : base(owner)
		{
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x060042C5 RID: 17093 RVA: 0x0021C86D File Offset: 0x0021B86D
		private bool IsDayButton
		{
			get
			{
				return base.Owner is CalendarDayButton;
			}
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x0021C87D File Offset: 0x0021B87D
		protected override string GetLocalizedControlTypeCore()
		{
			if (!this.IsDayButton)
			{
				return SR.Get("CalendarAutomationPeer_CalendarButtonLocalizedControlType");
			}
			return SR.Get("CalendarAutomationPeer_DayButtonLocalizedControlType");
		}
	}
}
