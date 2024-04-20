using System;
using System.Windows.Interop;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200056D RID: 1389
	internal class HwndHostAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600446D RID: 17517 RVA: 0x0022137A File Offset: 0x0022037A
		public HwndHostAutomationPeer(HwndHost owner) : base(owner)
		{
			base.IsInteropPeer = true;
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0022138A File Offset: 0x0022038A
		protected override string GetClassNameCore()
		{
			return "HwndHost";
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x001FC004 File Offset: 0x001FB004
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Pane;
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x00221394 File Offset: 0x00220394
		internal override InteropAutomationProvider GetInteropChild()
		{
			if (this._interopProvider == null)
			{
				HostedWindowWrapper wrapper = null;
				IntPtr criticalHandle = ((HwndHost)base.Owner).CriticalHandle;
				if (criticalHandle != IntPtr.Zero)
				{
					wrapper = HostedWindowWrapper.CreateInternal(criticalHandle);
				}
				this._interopProvider = new InteropAutomationProvider(wrapper, this);
			}
			return this._interopProvider;
		}

		// Token: 0x0400252F RID: 9519
		private InteropAutomationProvider _interopProvider;
	}
}
