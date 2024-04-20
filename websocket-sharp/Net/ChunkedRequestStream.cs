using System;
using System.IO;

namespace WebSocketSharp.Net
{
	// Token: 0x02000037 RID: 55
	internal class ChunkedRequestStream : RequestStream
	{
		// Token: 0x060003C7 RID: 967 RVA: 0x00016935 File Offset: 0x00014B35
		internal ChunkedRequestStream(Stream stream, byte[] buffer, int offset, int count, HttpListenerContext context) : base(stream, buffer, offset, count)
		{
			this._context = context;
			this._decoder = new ChunkStream((WebHeaderCollection)context.Request.Headers);
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00016968 File Offset: 0x00014B68
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x00016980 File Offset: 0x00014B80
		internal ChunkStream Decoder
		{
			get
			{
				return this._decoder;
			}
			set
			{
				this._decoder = value;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001698C File Offset: 0x00014B8C
		private void onRead(IAsyncResult asyncResult)
		{
			ReadBufferState readBufferState = (ReadBufferState)asyncResult.AsyncState;
			HttpStreamAsyncResult asyncResult2 = readBufferState.AsyncResult;
			try
			{
				int num = base.EndRead(asyncResult);
				this._decoder.Write(asyncResult2.Buffer, asyncResult2.Offset, num);
				num = this._decoder.Read(readBufferState.Buffer, readBufferState.Offset, readBufferState.Count);
				readBufferState.Offset += num;
				readBufferState.Count -= num;
				bool flag = readBufferState.Count == 0 || !this._decoder.WantMore || num == 0;
				if (flag)
				{
					this._noMoreData = (!this._decoder.WantMore && num == 0);
					asyncResult2.Count = readBufferState.InitialCount - readBufferState.Count;
					asyncResult2.Complete();
				}
				else
				{
					asyncResult2.Offset = 0;
					asyncResult2.Count = Math.Min(8192, this._decoder.ChunkLeft + 6);
					base.BeginRead(asyncResult2.Buffer, asyncResult2.Offset, asyncResult2.Count, new AsyncCallback(this.onRead), readBufferState);
				}
			}
			catch (Exception ex)
			{
				this._context.Connection.SendError(ex.Message, 400);
				asyncResult2.Complete(ex);
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00016AFC File Offset: 0x00014CFC
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			bool flag = buffer == null;
			if (flag)
			{
				throw new ArgumentNullException("buffer");
			}
			bool flag2 = offset < 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("offset", "A negative value.");
			}
			bool flag3 = count < 0;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("count", "A negative value.");
			}
			int num = buffer.Length;
			bool flag4 = offset + count > num;
			if (flag4)
			{
				throw new ArgumentException("The sum of 'offset' and 'count' is greater than 'buffer' length.");
			}
			HttpStreamAsyncResult httpStreamAsyncResult = new HttpStreamAsyncResult(callback, state);
			bool noMoreData = this._noMoreData;
			IAsyncResult result;
			if (noMoreData)
			{
				httpStreamAsyncResult.Complete();
				result = httpStreamAsyncResult;
			}
			else
			{
				int num2 = this._decoder.Read(buffer, offset, count);
				offset += num2;
				count -= num2;
				bool flag5 = count == 0;
				if (flag5)
				{
					httpStreamAsyncResult.Count = num2;
					httpStreamAsyncResult.Complete();
					result = httpStreamAsyncResult;
				}
				else
				{
					bool flag6 = !this._decoder.WantMore;
					if (flag6)
					{
						this._noMoreData = (num2 == 0);
						httpStreamAsyncResult.Count = num2;
						httpStreamAsyncResult.Complete();
						result = httpStreamAsyncResult;
					}
					else
					{
						httpStreamAsyncResult.Buffer = new byte[8192];
						httpStreamAsyncResult.Offset = 0;
						httpStreamAsyncResult.Count = 8192;
						ReadBufferState readBufferState = new ReadBufferState(buffer, offset, count, httpStreamAsyncResult);
						readBufferState.InitialCount += num2;
						base.BeginRead(httpStreamAsyncResult.Buffer, httpStreamAsyncResult.Offset, httpStreamAsyncResult.Count, new AsyncCallback(this.onRead), readBufferState);
						result = httpStreamAsyncResult;
					}
				}
			}
			return result;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00016C8C File Offset: 0x00014E8C
		public override void Close()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this._disposed = true;
				base.Close();
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00016CB4 File Offset: 0x00014EB4
		public override int EndRead(IAsyncResult asyncResult)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			bool flag = asyncResult == null;
			if (flag)
			{
				throw new ArgumentNullException("asyncResult");
			}
			HttpStreamAsyncResult httpStreamAsyncResult = asyncResult as HttpStreamAsyncResult;
			bool flag2 = httpStreamAsyncResult == null;
			if (flag2)
			{
				throw new ArgumentException("A wrong IAsyncResult.", "asyncResult");
			}
			bool flag3 = !httpStreamAsyncResult.IsCompleted;
			if (flag3)
			{
				httpStreamAsyncResult.AsyncWaitHandle.WaitOne();
			}
			bool hasException = httpStreamAsyncResult.HasException;
			if (hasException)
			{
				throw new HttpListenerException(400, "I/O operation aborted.");
			}
			return httpStreamAsyncResult.Count;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00016D54 File Offset: 0x00014F54
		public override int Read(byte[] buffer, int offset, int count)
		{
			IAsyncResult asyncResult = this.BeginRead(buffer, offset, count, null, null);
			return this.EndRead(asyncResult);
		}

		// Token: 0x04000194 RID: 404
		private const int _bufferLength = 8192;

		// Token: 0x04000195 RID: 405
		private HttpListenerContext _context;

		// Token: 0x04000196 RID: 406
		private ChunkStream _decoder;

		// Token: 0x04000197 RID: 407
		private bool _disposed;

		// Token: 0x04000198 RID: 408
		private bool _noMoreData;
	}
}
