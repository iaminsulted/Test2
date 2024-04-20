using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000834 RID: 2100
	public class DragDeltaEventArgs : RoutedEventArgs
	{
		// Token: 0x06007B5A RID: 31578 RVA: 0x0030C251 File Offset: 0x0030B251
		public DragDeltaEventArgs(double horizontalChange, double verticalChange)
		{
			this._horizontalChange = horizontalChange;
			this._verticalChange = verticalChange;
			base.RoutedEvent = Thumb.DragDeltaEvent;
		}

		// Token: 0x17001C82 RID: 7298
		// (get) Token: 0x06007B5B RID: 31579 RVA: 0x0030C272 File Offset: 0x0030B272
		public double HorizontalChange
		{
			get
			{
				return this._horizontalChange;
			}
		}

		// Token: 0x17001C83 RID: 7299
		// (get) Token: 0x06007B5C RID: 31580 RVA: 0x0030C27A File Offset: 0x0030B27A
		public double VerticalChange
		{
			get
			{
				return this._verticalChange;
			}
		}

		// Token: 0x06007B5D RID: 31581 RVA: 0x0030C282 File Offset: 0x0030B282
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((DragDeltaEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x04003A44 RID: 14916
		private double _horizontalChange;

		// Token: 0x04003A45 RID: 14917
		private double _verticalChange;
	}
}
