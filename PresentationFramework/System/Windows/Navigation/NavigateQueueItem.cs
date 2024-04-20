using System;
using System.Windows.Threading;

namespace System.Windows.Navigation
{
	// Token: 0x020005C9 RID: 1481
	internal class NavigateQueueItem
	{
		// Token: 0x06004778 RID: 18296 RVA: 0x00229F68 File Offset: 0x00228F68
		internal NavigateQueueItem(Uri source, object content, NavigationMode mode, object navState, NavigationService nc)
		{
			this._source = source;
			this._content = content;
			this._navState = navState;
			this._nc = nc;
			this._navigationMode = mode;
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x00229F95 File Offset: 0x00228F95
		internal void PostNavigation()
		{
			this._postedOp = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.Dispatch), null);
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x00229FB6 File Offset: 0x00228FB6
		internal void Stop()
		{
			if (this._postedOp != null)
			{
				this._postedOp.Abort();
				this._postedOp = null;
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x0600477B RID: 18299 RVA: 0x00229FD3 File Offset: 0x00228FD3
		internal Uri Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x0600477C RID: 18300 RVA: 0x00229FDB File Offset: 0x00228FDB
		internal object NavState
		{
			get
			{
				return this._navState;
			}
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x00229FE4 File Offset: 0x00228FE4
		private object Dispatch(object obj)
		{
			this._postedOp = null;
			if (this._content != null || this._source == null)
			{
				this._nc.DoNavigate(this._content, this._navigationMode, this._navState);
			}
			else
			{
				this._nc.DoNavigate(this._source, this._navigationMode, this._navState);
			}
			return null;
		}

		// Token: 0x040025D2 RID: 9682
		private Uri _source;

		// Token: 0x040025D3 RID: 9683
		private object _content;

		// Token: 0x040025D4 RID: 9684
		private object _navState;

		// Token: 0x040025D5 RID: 9685
		private NavigationService _nc;

		// Token: 0x040025D6 RID: 9686
		private NavigationMode _navigationMode;

		// Token: 0x040025D7 RID: 9687
		private DispatcherOperation _postedOp;
	}
}
