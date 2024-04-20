using System;
using System.Collections;
using System.Threading;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000216 RID: 534
	internal class DefaultAsyncDataDispatcher : IAsyncDataDispatcher
	{
		// Token: 0x06001442 RID: 5186 RVA: 0x001513F8 File Offset: 0x001503F8
		void IAsyncDataDispatcher.AddRequest(AsyncDataRequest request)
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Add(request);
			}
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessRequest), request);
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00151458 File Offset: 0x00150458
		void IAsyncDataDispatcher.CancelAllRequests()
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				for (int i = 0; i < this._list.Count; i++)
				{
					((AsyncDataRequest)this._list[i]).Cancel();
				}
				this._list.Clear();
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x001514D0 File Offset: 0x001504D0
		private void ProcessRequest(object o)
		{
			AsyncDataRequest asyncDataRequest = (AsyncDataRequest)o;
			try
			{
				asyncDataRequest.Complete(asyncDataRequest.DoWork());
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				asyncDataRequest.Fail(ex);
			}
			catch
			{
				asyncDataRequest.Fail(new InvalidOperationException(SR.Get("NonCLSException", new object[]
				{
					"processing an async data request"
				})));
			}
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Remove(asyncDataRequest);
			}
		}

		// Token: 0x04000BBC RID: 3004
		private ArrayList _list = new ArrayList();
	}
}
