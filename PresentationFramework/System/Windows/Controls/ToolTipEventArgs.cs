using System;

namespace System.Windows.Controls
{
	// Token: 0x020007F1 RID: 2033
	public sealed class ToolTipEventArgs : RoutedEventArgs
	{
		// Token: 0x06007611 RID: 30225 RVA: 0x002EE485 File Offset: 0x002ED485
		internal ToolTipEventArgs(bool opening)
		{
			if (opening)
			{
				base.RoutedEvent = ToolTipService.ToolTipOpeningEvent;
				return;
			}
			base.RoutedEvent = ToolTipService.ToolTipClosingEvent;
		}

		// Token: 0x06007612 RID: 30226 RVA: 0x002EE4A7 File Offset: 0x002ED4A7
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((ToolTipEventHandler)genericHandler)(genericTarget, this);
		}
	}
}
