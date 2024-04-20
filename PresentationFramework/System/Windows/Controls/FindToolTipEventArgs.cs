using System;

namespace System.Windows.Controls
{
	// Token: 0x020007F3 RID: 2035
	internal sealed class FindToolTipEventArgs : RoutedEventArgs
	{
		// Token: 0x06007617 RID: 30231 RVA: 0x002EE4B6 File Offset: 0x002ED4B6
		internal FindToolTipEventArgs(ToolTip.ToolTipTrigger triggerAction)
		{
			base.RoutedEvent = ToolTipService.FindToolTipEvent;
			this._triggerAction = triggerAction;
		}

		// Token: 0x17001B68 RID: 7016
		// (get) Token: 0x06007618 RID: 30232 RVA: 0x002EE4D0 File Offset: 0x002ED4D0
		// (set) Token: 0x06007619 RID: 30233 RVA: 0x002EE4D8 File Offset: 0x002ED4D8
		internal DependencyObject TargetElement
		{
			get
			{
				return this._targetElement;
			}
			set
			{
				this._targetElement = value;
			}
		}

		// Token: 0x17001B69 RID: 7017
		// (get) Token: 0x0600761A RID: 30234 RVA: 0x002EE4E1 File Offset: 0x002ED4E1
		// (set) Token: 0x0600761B RID: 30235 RVA: 0x002EE4E9 File Offset: 0x002ED4E9
		internal bool KeepCurrentActive
		{
			get
			{
				return this._keepCurrentActive;
			}
			set
			{
				this._keepCurrentActive = value;
			}
		}

		// Token: 0x17001B6A RID: 7018
		// (get) Token: 0x0600761C RID: 30236 RVA: 0x002EE4F2 File Offset: 0x002ED4F2
		internal ToolTip.ToolTipTrigger TriggerAction
		{
			get
			{
				return this._triggerAction;
			}
		}

		// Token: 0x0600761D RID: 30237 RVA: 0x002EE4FA File Offset: 0x002ED4FA
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((FindToolTipEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x04003885 RID: 14469
		private DependencyObject _targetElement;

		// Token: 0x04003886 RID: 14470
		private bool _keepCurrentActive;

		// Token: 0x04003887 RID: 14471
		private ToolTip.ToolTipTrigger _triggerAction;
	}
}
