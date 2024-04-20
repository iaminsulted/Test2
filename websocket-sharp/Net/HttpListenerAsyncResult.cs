using System;
using System.Threading;

namespace WebSocketSharp.Net
{
	// Token: 0x0200003F RID: 63
	internal class HttpListenerAsyncResult : IAsyncResult
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x0001843B File Offset: 0x0001663B
		internal HttpListenerAsyncResult(AsyncCallback callback, object state)
		{
			this._callback = callback;
			this._state = state;
			this._sync = new object();
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00018460 File Offset: 0x00016660
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00018478 File Offset: 0x00016678
		internal bool EndCalled
		{
			get
			{
				return this._endCalled;
			}
			set
			{
				this._endCalled = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00018484 File Offset: 0x00016684
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x0001849C File Offset: 0x0001669C
		internal bool InGet
		{
			get
			{
				return this._inGet;
			}
			set
			{
				this._inGet = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x000184A8 File Offset: 0x000166A8
		public object AsyncState
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x000184C0 File Offset: 0x000166C0
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				object sync = this._sync;
				WaitHandle result;
				lock (sync)
				{
					ManualResetEvent manualResetEvent;
					if ((manualResetEvent = this._waitHandle) == null)
					{
						manualResetEvent = (this._waitHandle = new ManualResetEvent(this._completed));
					}
					result = manualResetEvent;
				}
				return result;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00018518 File Offset: 0x00016718
		public bool CompletedSynchronously
		{
			get
			{
				return this._syncCompleted;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x00018530 File Offset: 0x00016730
		public bool IsCompleted
		{
			get
			{
				object sync = this._sync;
				bool completed;
				lock (sync)
				{
					completed = this._completed;
				}
				return completed;
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00018570 File Offset: 0x00016770
		private static void complete(HttpListenerAsyncResult asyncResult)
		{
			object sync = asyncResult._sync;
			lock (sync)
			{
				asyncResult._completed = true;
				ManualResetEvent waitHandle = asyncResult._waitHandle;
				bool flag = waitHandle != null;
				if (flag)
				{
					waitHandle.Set();
				}
			}
			AsyncCallback callback = asyncResult._callback;
			bool flag2 = callback == null;
			if (!flag2)
			{
				ThreadPool.QueueUserWorkItem(delegate(object state)
				{
					try
					{
						callback(asyncResult);
					}
					catch
					{
					}
				}, null);
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00018618 File Offset: 0x00016818
		internal void Complete(Exception exception)
		{
			this._exception = ((this._inGet && exception is ObjectDisposedException) ? new HttpListenerException(995, "The listener is closed.") : exception);
			HttpListenerAsyncResult.complete(this);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001864A File Offset: 0x0001684A
		internal void Complete(HttpListenerContext context)
		{
			this.Complete(context, false);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00018656 File Offset: 0x00016856
		internal void Complete(HttpListenerContext context, bool syncCompleted)
		{
			this._context = context;
			this._syncCompleted = syncCompleted;
			HttpListenerAsyncResult.complete(this);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00018670 File Offset: 0x00016870
		internal HttpListenerContext GetContext()
		{
			bool flag = this._exception != null;
			if (flag)
			{
				throw this._exception;
			}
			return this._context;
		}

		// Token: 0x040001AE RID: 430
		private AsyncCallback _callback;

		// Token: 0x040001AF RID: 431
		private bool _completed;

		// Token: 0x040001B0 RID: 432
		private HttpListenerContext _context;

		// Token: 0x040001B1 RID: 433
		private bool _endCalled;

		// Token: 0x040001B2 RID: 434
		private Exception _exception;

		// Token: 0x040001B3 RID: 435
		private bool _inGet;

		// Token: 0x040001B4 RID: 436
		private object _state;

		// Token: 0x040001B5 RID: 437
		private object _sync;

		// Token: 0x040001B6 RID: 438
		private bool _syncCompleted;

		// Token: 0x040001B7 RID: 439
		private ManualResetEvent _waitHandle;
	}
}
