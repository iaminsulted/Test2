using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000832 RID: 2098
	public class DragCompletedEventArgs : RoutedEventArgs
	{
		// Token: 0x06007B51 RID: 31569 RVA: 0x0030C202 File Offset: 0x0030B202
		public DragCompletedEventArgs(double horizontalChange, double verticalChange, bool canceled)
		{
			this._horizontalChange = horizontalChange;
			this._verticalChange = verticalChange;
			this._wasCanceled = canceled;
			base.RoutedEvent = Thumb.DragCompletedEvent;
		}

		// Token: 0x17001C7F RID: 7295
		// (get) Token: 0x06007B52 RID: 31570 RVA: 0x0030C22A File Offset: 0x0030B22A
		public double HorizontalChange
		{
			get
			{
				return this._horizontalChange;
			}
		}

		// Token: 0x17001C80 RID: 7296
		// (get) Token: 0x06007B53 RID: 31571 RVA: 0x0030C232 File Offset: 0x0030B232
		public double VerticalChange
		{
			get
			{
				return this._verticalChange;
			}
		}

		// Token: 0x17001C81 RID: 7297
		// (get) Token: 0x06007B54 RID: 31572 RVA: 0x0030C23A File Offset: 0x0030B23A
		public bool Canceled
		{
			get
			{
				return this._wasCanceled;
			}
		}

		// Token: 0x06007B55 RID: 31573 RVA: 0x0030C242 File Offset: 0x0030B242
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((DragCompletedEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x04003A41 RID: 14913
		private double _horizontalChange;

		// Token: 0x04003A42 RID: 14914
		private double _verticalChange;

		// Token: 0x04003A43 RID: 14915
		private bool _wasCanceled;
	}
}
