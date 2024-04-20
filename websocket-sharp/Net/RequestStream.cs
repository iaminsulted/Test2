using System;
using System.IO;

namespace WebSocketSharp.Net
{
	// Token: 0x02000028 RID: 40
	internal class RequestStream : Stream
	{
		// Token: 0x06000328 RID: 808 RVA: 0x0001494C File Offset: 0x00012B4C
		internal RequestStream(Stream stream, byte[] buffer, int offset, int count) : this(stream, buffer, offset, count, -1L)
		{
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0001495D File Offset: 0x00012B5D
		internal RequestStream(Stream stream, byte[] buffer, int offset, int count, long contentLength)
		{
			this._stream = stream;
			this._buffer = buffer;
			this._offset = offset;
			this._count = count;
			this._bodyLeft = contentLength;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0001498C File Offset: 0x00012B8C
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600032B RID: 811 RVA: 0x000149A0 File Offset: 0x00012BA0
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600032C RID: 812 RVA: 0x000149B4 File Offset: 0x00012BB4
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		// (set) Token: 0x0600032F RID: 815 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000149C8 File Offset: 0x00012BC8
		private int fillFromBuffer(byte[] buffer, int offset, int count)
		{
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
			bool flag5 = this._bodyLeft == 0L;
			int result;
			if (flag5)
			{
				result = -1;
			}
			else
			{
				bool flag6 = this._count == 0 || count == 0;
				if (flag6)
				{
					result = 0;
				}
				else
				{
					bool flag7 = count > this._count;
					if (flag7)
					{
						count = this._count;
					}
					bool flag8 = this._bodyLeft > 0L && (long)count > this._bodyLeft;
					if (flag8)
					{
						count = (int)this._bodyLeft;
					}
					Buffer.BlockCopy(this._buffer, this._offset, buffer, offset, count);
					this._offset += count;
					this._count -= count;
					bool flag9 = this._bodyLeft > 0L;
					if (flag9)
					{
						this._bodyLeft -= (long)count;
					}
					result = count;
				}
			}
			return result;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00014B00 File Offset: 0x00012D00
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			int num = this.fillFromBuffer(buffer, offset, count);
			bool flag = num > 0 || num == -1;
			IAsyncResult result;
			if (flag)
			{
				HttpStreamAsyncResult httpStreamAsyncResult = new HttpStreamAsyncResult(callback, state);
				httpStreamAsyncResult.Buffer = buffer;
				httpStreamAsyncResult.Offset = offset;
				httpStreamAsyncResult.Count = count;
				httpStreamAsyncResult.SyncRead = ((num > 0) ? num : 0);
				httpStreamAsyncResult.Complete();
				result = httpStreamAsyncResult;
			}
			else
			{
				bool flag2 = this._bodyLeft >= 0L && (long)count > this._bodyLeft;
				if (flag2)
				{
					count = (int)this._bodyLeft;
				}
				result = this._stream.BeginRead(buffer, offset, count, callback, state);
			}
			return result;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00014BBE File Offset: 0x00012DBE
		public override void Close()
		{
			this._disposed = true;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00014BC8 File Offset: 0x00012DC8
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
			bool flag2 = asyncResult is HttpStreamAsyncResult;
			int result;
			if (flag2)
			{
				HttpStreamAsyncResult httpStreamAsyncResult = (HttpStreamAsyncResult)asyncResult;
				bool flag3 = !httpStreamAsyncResult.IsCompleted;
				if (flag3)
				{
					httpStreamAsyncResult.AsyncWaitHandle.WaitOne();
				}
				result = httpStreamAsyncResult.SyncRead;
			}
			else
			{
				int num = this._stream.EndRead(asyncResult);
				bool flag4 = num > 0 && this._bodyLeft > 0L;
				if (flag4)
				{
					this._bodyLeft -= (long)num;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00014C7C File Offset: 0x00012E7C
		public override void Flush()
		{
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00014C80 File Offset: 0x00012E80
		public override int Read(byte[] buffer, int offset, int count)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			int num = this.fillFromBuffer(buffer, offset, count);
			bool flag = num == -1;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = num > 0;
				if (flag2)
				{
					result = num;
				}
				else
				{
					num = this._stream.Read(buffer, offset, count);
					bool flag3 = num > 0 && this._bodyLeft > 0L;
					if (flag3)
					{
						this._bodyLeft -= (long)num;
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400012C RID: 300
		private long _bodyLeft;

		// Token: 0x0400012D RID: 301
		private byte[] _buffer;

		// Token: 0x0400012E RID: 302
		private int _count;

		// Token: 0x0400012F RID: 303
		private bool _disposed;

		// Token: 0x04000130 RID: 304
		private int _offset;

		// Token: 0x04000131 RID: 305
		private Stream _stream;
	}
}
