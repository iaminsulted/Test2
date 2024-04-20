using System;

namespace System.Windows.Data
{
	// Token: 0x0200045B RID: 1115
	public class DataTransferEventArgs : RoutedEventArgs
	{
		// Token: 0x060038D1 RID: 14545 RVA: 0x001E9FFD File Offset: 0x001E8FFD
		internal DataTransferEventArgs(DependencyObject targetObject, DependencyProperty dp)
		{
			this._targetObject = targetObject;
			this._dp = dp;
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x060038D2 RID: 14546 RVA: 0x001EA013 File Offset: 0x001E9013
		public DependencyObject TargetObject
		{
			get
			{
				return this._targetObject;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060038D3 RID: 14547 RVA: 0x001EA01B File Offset: 0x001E901B
		public DependencyProperty Property
		{
			get
			{
				return this._dp;
			}
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x001EA023 File Offset: 0x001E9023
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((EventHandler<DataTransferEventArgs>)genericHandler)(genericTarget, this);
		}

		// Token: 0x04001D4E RID: 7502
		private DependencyObject _targetObject;

		// Token: 0x04001D4F RID: 7503
		private DependencyProperty _dp;
	}
}
