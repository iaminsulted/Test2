using System;

namespace System.Windows.Controls
{
	// Token: 0x020007CA RID: 1994
	public class ScrollChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x0600722B RID: 29227 RVA: 0x002DD290 File Offset: 0x002DC290
		internal ScrollChangedEventArgs(Vector offset, Vector offsetChange, Size extent, Vector extentChange, Size viewport, Vector viewportChange)
		{
			this._offset = offset;
			this._offsetChange = offsetChange;
			this._extent = extent;
			this._extentChange = extentChange;
			this._viewport = viewport;
			this._viewportChange = viewportChange;
		}

		// Token: 0x17001A6F RID: 6767
		// (get) Token: 0x0600722C RID: 29228 RVA: 0x002DD2C5 File Offset: 0x002DC2C5
		public double HorizontalOffset
		{
			get
			{
				return this._offset.X;
			}
		}

		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x0600722D RID: 29229 RVA: 0x002DD2D2 File Offset: 0x002DC2D2
		public double VerticalOffset
		{
			get
			{
				return this._offset.Y;
			}
		}

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x0600722E RID: 29230 RVA: 0x002DD2DF File Offset: 0x002DC2DF
		public double HorizontalChange
		{
			get
			{
				return this._offsetChange.X;
			}
		}

		// Token: 0x17001A72 RID: 6770
		// (get) Token: 0x0600722F RID: 29231 RVA: 0x002DD2EC File Offset: 0x002DC2EC
		public double VerticalChange
		{
			get
			{
				return this._offsetChange.Y;
			}
		}

		// Token: 0x17001A73 RID: 6771
		// (get) Token: 0x06007230 RID: 29232 RVA: 0x002DD2F9 File Offset: 0x002DC2F9
		public double ViewportWidth
		{
			get
			{
				return this._viewport.Width;
			}
		}

		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x06007231 RID: 29233 RVA: 0x002DD306 File Offset: 0x002DC306
		public double ViewportHeight
		{
			get
			{
				return this._viewport.Height;
			}
		}

		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x06007232 RID: 29234 RVA: 0x002DD313 File Offset: 0x002DC313
		public double ViewportWidthChange
		{
			get
			{
				return this._viewportChange.X;
			}
		}

		// Token: 0x17001A76 RID: 6774
		// (get) Token: 0x06007233 RID: 29235 RVA: 0x002DD320 File Offset: 0x002DC320
		public double ViewportHeightChange
		{
			get
			{
				return this._viewportChange.Y;
			}
		}

		// Token: 0x17001A77 RID: 6775
		// (get) Token: 0x06007234 RID: 29236 RVA: 0x002DD32D File Offset: 0x002DC32D
		public double ExtentWidth
		{
			get
			{
				return this._extent.Width;
			}
		}

		// Token: 0x17001A78 RID: 6776
		// (get) Token: 0x06007235 RID: 29237 RVA: 0x002DD33A File Offset: 0x002DC33A
		public double ExtentHeight
		{
			get
			{
				return this._extent.Height;
			}
		}

		// Token: 0x17001A79 RID: 6777
		// (get) Token: 0x06007236 RID: 29238 RVA: 0x002DD347 File Offset: 0x002DC347
		public double ExtentWidthChange
		{
			get
			{
				return this._extentChange.X;
			}
		}

		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x06007237 RID: 29239 RVA: 0x002DD354 File Offset: 0x002DC354
		public double ExtentHeightChange
		{
			get
			{
				return this._extentChange.Y;
			}
		}

		// Token: 0x06007238 RID: 29240 RVA: 0x002DD361 File Offset: 0x002DC361
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((ScrollChangedEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x04003755 RID: 14165
		private Vector _offset;

		// Token: 0x04003756 RID: 14166
		private Vector _offsetChange;

		// Token: 0x04003757 RID: 14167
		private Size _extent;

		// Token: 0x04003758 RID: 14168
		private Vector _extentChange;

		// Token: 0x04003759 RID: 14169
		private Size _viewport;

		// Token: 0x0400375A RID: 14170
		private Vector _viewportChange;
	}
}
