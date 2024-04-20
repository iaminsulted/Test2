using System;
using System.Threading;

namespace WebSocketSharp.Net
{
	// Token: 0x02000026 RID: 38
	internal class HttpStreamAsyncResult : IAsyncResult
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x00011D02 File Offset: 0x0000FF02
		internal HttpStreamAsyncResult(AsyncCallback callback, object state)
		{
			this._callback = callback;
			this._state = state;
			this._sync = new object();
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00011D28 File Offset: 0x0000FF28
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x00011D40 File Offset: 0x0000FF40
		internal byte[] Buffer
		{
			get
			{
				return this._buffer;
			}
			set
			{
				this._buffer = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00011D4C File Offset: 0x0000FF4C
		// (set) Token: 0x060002EB RID: 747 RVA: 0x00011D64 File Offset: 0x0000FF64
		internal int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._count = value;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00011D70 File Offset: 0x0000FF70
		internal Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00011D88 File Offset: 0x0000FF88
		internal bool HasException
		{
			get
			{
				return this._exception != null;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00011DA4 File Offset: 0x0000FFA4
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00011DBC File Offset: 0x0000FFBC
		internal int Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00011DC8 File Offset: 0x0000FFC8
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x00011DE0 File Offset: 0x0000FFE0
		internal int SyncRead
		{
			get
			{
				return this._syncRead;
			}
			set
			{
				this._syncRead = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00011DEC File Offset: 0x0000FFEC
		public object AsyncState
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00011E04 File Offset: 0x00010004
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00011E5C File Offset: 0x0001005C
		public bool CompletedSynchronously
		{
			get
			{
				return this._syncRead == this._count;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00011E7C File Offset: 0x0001007C
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

		// Token: 0x060002F6 RID: 758 RVA: 0x00011EBC File Offset: 0x000100BC
		internal void Complete()
		{
			object sync = this._sync;
			lock (sync)
			{
				bool completed = this._completed;
				if (!completed)
				{
					this._completed = true;
					bool flag = this._waitHandle != null;
					if (flag)
					{
						this._waitHandle.Set();
					}
					bool flag2 = this._callback != null;
					if (flag2)
					{
						this._callback.BeginInvoke(this, delegate(IAsyncResult ar)
						{
							this._callback.EndInvoke(ar);
						}, null);
					}
				}
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00011F48 File Offset: 0x00010148
		internal void Complete(Exception exception)
		{
			this._exception = exception;
			this.Complete();
		}

		// Token: 0x0400011F RID: 287
		private byte[] _buffer;

		// Token: 0x04000120 RID: 288
		private AsyncCallback _callback;

		// Token: 0x04000121 RID: 289
		private bool _completed;

		// Token: 0x04000122 RID: 290
		private int _count;

		// Token: 0x04000123 RID: 291
		private Exception _exception;

		// Token: 0x04000124 RID: 292
		private int _offset;

		// Token: 0x04000125 RID: 293
		private object _state;

		// Token: 0x04000126 RID: 294
		private object _sync;

		// Token: 0x04000127 RID: 295
		private int _syncRead;

		// Token: 0x04000128 RID: 296
		private ManualResetEvent _waitHandle;
	}
}
