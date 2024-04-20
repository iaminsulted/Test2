using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000852 RID: 2130
	public class ScrollEventArgs : RoutedEventArgs
	{
		// Token: 0x06007D1C RID: 32028 RVA: 0x003122D1 File Offset: 0x003112D1
		public ScrollEventArgs(ScrollEventType scrollEventType, double newValue)
		{
			this._scrollEventType = scrollEventType;
			this._newValue = newValue;
			base.RoutedEvent = ScrollBar.ScrollEvent;
		}

		// Token: 0x17001CE2 RID: 7394
		// (get) Token: 0x06007D1D RID: 32029 RVA: 0x003122F2 File Offset: 0x003112F2
		public ScrollEventType ScrollEventType
		{
			get
			{
				return this._scrollEventType;
			}
		}

		// Token: 0x17001CE3 RID: 7395
		// (get) Token: 0x06007D1E RID: 32030 RVA: 0x003122FA File Offset: 0x003112FA
		public double NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		// Token: 0x06007D1F RID: 32031 RVA: 0x00312302 File Offset: 0x00311302
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((ScrollEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x04003AD6 RID: 15062
		private ScrollEventType _scrollEventType;

		// Token: 0x04003AD7 RID: 15063
		private double _newValue;
	}
}
