using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000836 RID: 2102
	public class DragStartedEventArgs : RoutedEventArgs
	{
		// Token: 0x06007B62 RID: 31586 RVA: 0x0030C291 File Offset: 0x0030B291
		public DragStartedEventArgs(double horizontalOffset, double verticalOffset)
		{
			this._horizontalOffset = horizontalOffset;
			this._verticalOffset = verticalOffset;
			base.RoutedEvent = Thumb.DragStartedEvent;
		}

		// Token: 0x17001C84 RID: 7300
		// (get) Token: 0x06007B63 RID: 31587 RVA: 0x0030C2B2 File Offset: 0x0030B2B2
		public double HorizontalOffset
		{
			get
			{
				return this._horizontalOffset;
			}
		}

		// Token: 0x17001C85 RID: 7301
		// (get) Token: 0x06007B64 RID: 31588 RVA: 0x0030C2BA File Offset: 0x0030B2BA
		public double VerticalOffset
		{
			get
			{
				return this._verticalOffset;
			}
		}

		// Token: 0x06007B65 RID: 31589 RVA: 0x0030C2C2 File Offset: 0x0030B2C2
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((DragStartedEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x04003A46 RID: 14918
		private double _horizontalOffset;

		// Token: 0x04003A47 RID: 14919
		private double _verticalOffset;
	}
}
