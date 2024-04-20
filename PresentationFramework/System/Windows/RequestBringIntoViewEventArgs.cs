using System;

namespace System.Windows
{
	// Token: 0x0200038B RID: 907
	public class RequestBringIntoViewEventArgs : RoutedEventArgs
	{
		// Token: 0x06002489 RID: 9353 RVA: 0x001836AC File Offset: 0x001826AC
		internal RequestBringIntoViewEventArgs(DependencyObject target, Rect targetRect)
		{
			this._target = target;
			this._rcTarget = targetRect;
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600248A RID: 9354 RVA: 0x001836C2 File Offset: 0x001826C2
		public DependencyObject TargetObject
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x0600248B RID: 9355 RVA: 0x001836CA File Offset: 0x001826CA
		public Rect TargetRect
		{
			get
			{
				return this._rcTarget;
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x001836D2 File Offset: 0x001826D2
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((RequestBringIntoViewEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x0400114D RID: 4429
		private DependencyObject _target;

		// Token: 0x0400114E RID: 4430
		private Rect _rcTarget;
	}
}
