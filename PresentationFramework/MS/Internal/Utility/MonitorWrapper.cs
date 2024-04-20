using System;
using System.Threading;

namespace MS.Internal.Utility
{
	// Token: 0x020002E1 RID: 737
	internal class MonitorWrapper
	{
		// Token: 0x06001BD8 RID: 7128 RVA: 0x0016A814 File Offset: 0x00169814
		public IDisposable Enter()
		{
			Monitor.Enter(this._syncRoot);
			Interlocked.Increment(ref this._enterCount);
			return new MonitorWrapper.MonitorHelper(this);
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0016A833 File Offset: 0x00169833
		public void Exit()
		{
			Invariant.Assert(Interlocked.Decrement(ref this._enterCount) >= 0, "unmatched call to MonitorWrapper.Exit");
			Monitor.Exit(this._syncRoot);
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001BDA RID: 7130 RVA: 0x0016A85B File Offset: 0x0016985B
		public bool Busy
		{
			get
			{
				return this._enterCount > 0;
			}
		}

		// Token: 0x04000E67 RID: 3687
		private int _enterCount;

		// Token: 0x04000E68 RID: 3688
		private object _syncRoot = new object();

		// Token: 0x02000A20 RID: 2592
		private class MonitorHelper : IDisposable
		{
			// Token: 0x06008501 RID: 34049 RVA: 0x00327CAA File Offset: 0x00326CAA
			public MonitorHelper(MonitorWrapper monitorWrapper)
			{
				this._monitorWrapper = monitorWrapper;
			}

			// Token: 0x06008502 RID: 34050 RVA: 0x00327CB9 File Offset: 0x00326CB9
			public void Dispose()
			{
				if (this._monitorWrapper != null)
				{
					this._monitorWrapper.Exit();
					this._monitorWrapper = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x040040B3 RID: 16563
			private MonitorWrapper _monitorWrapper;
		}
	}
}
