using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200059B RID: 1435
	public class ToggleButtonAutomationPeer : ButtonBaseAutomationPeer, IToggleProvider
	{
		// Token: 0x060045D0 RID: 17872 RVA: 0x0021BB7E File Offset: 0x0021AB7E
		public ToggleButtonAutomationPeer(ToggleButton owner) : base(owner)
		{
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x0021BB87 File Offset: 0x0021AB87
		protected override string GetClassNameCore()
		{
			return "Button";
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x00224BB3 File Offset: 0x00223BB3
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Toggle)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x00224BC3 File Offset: 0x00223BC3
		void IToggleProvider.Toggle()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((ToggleButton)base.Owner).OnToggle();
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x060045D5 RID: 17877 RVA: 0x00224BE3 File Offset: 0x00223BE3
		ToggleState IToggleProvider.ToggleState
		{
			get
			{
				return ToggleButtonAutomationPeer.ConvertToToggleState(((ToggleButton)base.Owner).IsChecked);
			}
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x00224BFC File Offset: 0x00223BFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal virtual void RaiseToggleStatePropertyChangedEvent(bool? oldValue, bool? newValue)
		{
			bool? flag = oldValue;
			bool? flag2 = newValue;
			if (!(flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null)))
			{
				base.RaisePropertyChangedEvent(TogglePatternIdentifiers.ToggleStateProperty, ToggleButtonAutomationPeer.ConvertToToggleState(oldValue), ToggleButtonAutomationPeer.ConvertToToggleState(newValue));
			}
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x00224C51 File Offset: 0x00223C51
		private static ToggleState ConvertToToggleState(bool? value)
		{
			if (value == null)
			{
				return ToggleState.Indeterminate;
			}
			if (value.GetValueOrDefault())
			{
				return ToggleState.On;
			}
			return ToggleState.Off;
		}
	}
}
