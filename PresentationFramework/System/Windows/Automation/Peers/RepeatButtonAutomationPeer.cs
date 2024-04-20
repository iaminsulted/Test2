using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000587 RID: 1415
	public class RepeatButtonAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
	{
		// Token: 0x06004541 RID: 17729 RVA: 0x0021BB7E File Offset: 0x0021AB7E
		public RepeatButtonAutomationPeer(RepeatButton owner) : base(owner)
		{
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x002233AE File Offset: 0x002223AE
		protected override string GetClassNameCore()
		{
			return "RepeatButton";
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x0021BB8E File Offset: 0x0021AB8E
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x002233B5 File Offset: 0x002223B5
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((RepeatButton)base.Owner).AutomationButtonBaseClick();
		}
	}
}
