using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Threading;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000544 RID: 1348
	public class ButtonAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
	{
		// Token: 0x06004299 RID: 17049 RVA: 0x0021BB7E File Offset: 0x0021AB7E
		public ButtonAutomationPeer(Button owner) : base(owner)
		{
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x0021BB87 File Offset: 0x0021AB87
		protected override string GetClassNameCore()
		{
			return "Button";
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x0021BB8E File Offset: 0x0021AB8E
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x0021BB9C File Offset: 0x0021AB9C
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				((Button)base.Owner).AutomationButtonBaseClick();
				return null;
			}), null);
		}
	}
}
