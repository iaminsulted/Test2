using System;
using System.IO;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x02000029 RID: 41
	internal class ResponseStream : Stream
	{
		// Token: 0x0600033B RID: 827 RVA: 0x00014D08 File Offset: 0x00012F08
		internal ResponseStream(Stream stream, HttpListenerResponse response, bool ignoreWriteExceptions)
		{
			this._stream = stream;
			this._response = response;
			if (ignoreWriteExceptions)
			{
				this._write = new Action<byte[], int, int>(this.writeWithoutThrowingException);
				this._writeChunked = new Action<byte[], int, int>(this.writeChunkedWithoutThrowingException);
			}
			else
			{
				this._write = new Action<byte[], int, int>(stream.Write);
				this._writeChunked = new Action<byte[], int, int>(this.writeChunked);
			}
			this._body = new MemoryStream();
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00014D8C File Offset: 0x00012F8C
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00014DA0 File Offset: 0x00012FA0
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00014DB4 File Offset: 0x00012FB4
		public override bool CanWrite
		{
			get
			{
				return !this._disposed;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		// (set) Token: 0x06000341 RID: 833 RVA: 0x0000F6CA File Offset: 0x0000D8CA
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

		// Token: 0x06000342 RID: 834 RVA: 0x00014DD0 File Offset: 0x00012FD0
		private bool flush(bool closing)
		{
			bool flag = !this._response.HeadersSent;
			if (flag)
			{
				bool flag2 = !this.flushHeaders(closing);
				if (flag2)
				{
					if (closing)
					{
						this._response.CloseConnection = true;
					}
					return false;
				}
				this._sendChunked = this._response.SendChunked;
				this._writeBody = (this._sendChunked ? this._writeChunked : this._write);
			}
			this.flushBody(closing);
			bool flag3 = closing && this._sendChunked;
			if (flag3)
			{
				byte[] chunkSizeBytes = ResponseStream.getChunkSizeBytes(0, true);
				this._write(chunkSizeBytes, 0, chunkSizeBytes.Length);
			}
			return true;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00014E84 File Offset: 0x00013084
		private void flushBody(bool closing)
		{
			using (this._body)
			{
				long length = this._body.Length;
				bool flag = length > 2147483647L;
				if (flag)
				{
					this._body.Position = 0L;
					int num = 1024;
					byte[] array = new byte[num];
					int arg;
					while ((arg = this._body.Read(array, 0, num)) > 0)
					{
						this._writeBody(array, 0, arg);
					}
				}
				else
				{
					bool flag2 = length > 0L;
					if (flag2)
					{
						this._writeBody(this._body.GetBuffer(), 0, (int)length);
					}
				}
			}
			this._body = ((!closing) ? new MemoryStream() : null);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00014F5C File Offset: 0x0001315C
		private bool flushHeaders(bool closing)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				WebHeaderCollection webHeaderCollection = this._response.WriteHeadersTo(memoryStream);
				long position = memoryStream.Position;
				long num = memoryStream.Length - position;
				bool flag = num > 32768L;
				if (flag)
				{
					return false;
				}
				bool flag2 = !this._response.SendChunked && this._response.ContentLength64 != this._body.Length;
				if (flag2)
				{
					return false;
				}
				this._write(memoryStream.GetBuffer(), (int)position, (int)num);
				this._response.CloseConnection = (webHeaderCollection["Connection"] == "close");
				this._response.HeadersSent = true;
			}
			return true;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00015044 File Offset: 0x00013244
		private static byte[] getChunkSizeBytes(int size, bool final)
		{
			return Encoding.ASCII.GetBytes(string.Format("{0:x}\r\n{1}", size, final ? "\r\n" : ""));
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00015080 File Offset: 0x00013280
		private void writeChunked(byte[] buffer, int offset, int count)
		{
			byte[] chunkSizeBytes = ResponseStream.getChunkSizeBytes(count, false);
			this._stream.Write(chunkSizeBytes, 0, chunkSizeBytes.Length);
			this._stream.Write(buffer, offset, count);
			this._stream.Write(ResponseStream._crlf, 0, 2);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000150CC File Offset: 0x000132CC
		private void writeChunkedWithoutThrowingException(byte[] buffer, int offset, int count)
		{
			try
			{
				this.writeChunked(buffer, offset, count);
			}
			catch
			{
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00015100 File Offset: 0x00013300
		private void writeWithoutThrowingException(byte[] buffer, int offset, int count)
		{
			try
			{
				this._stream.Write(buffer, offset, count);
			}
			catch
			{
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00015138 File Offset: 0x00013338
		internal void Close(bool force)
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this._disposed = true;
				bool flag = !force && this.flush(true);
				if (flag)
				{
					this._response.Close();
				}
				else
				{
					bool sendChunked = this._sendChunked;
					if (sendChunked)
					{
						byte[] chunkSizeBytes = ResponseStream.getChunkSizeBytes(0, true);
						this._write(chunkSizeBytes, 0, chunkSizeBytes.Length);
					}
					this._body.Dispose();
					this._body = null;
					this._response.Abort();
				}
				this._response = null;
				this._stream = null;
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000151CE File Offset: 0x000133CE
		internal void InternalWrite(byte[] buffer, int offset, int count)
		{
			this._write(buffer, offset, count);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x000151E0 File Offset: 0x000133E0
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			return this._body.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0001521F File Offset: 0x0001341F
		public override void Close()
		{
			this.Close(false);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0001522A File Offset: 0x0001342A
		protected override void Dispose(bool disposing)
		{
			this.Close(!disposing);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override int EndRead(IAsyncResult asyncResult)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00015238 File Offset: 0x00013438
		public override void EndWrite(IAsyncResult asyncResult)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			this._body.EndWrite(asyncResult);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00015270 File Offset: 0x00013470
		public override void Flush()
		{
			bool flag = !this._disposed && (this._sendChunked || this._response.SendChunked);
			if (flag)
			{
				this.flush(false);
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000152AC File Offset: 0x000134AC
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			this._body.Write(buffer, offset, count);
		}

		// Token: 0x04000132 RID: 306
		private MemoryStream _body;

		// Token: 0x04000133 RID: 307
		private static readonly byte[] _crlf = new byte[]
		{
			13,
			10
		};

		// Token: 0x04000134 RID: 308
		private bool _disposed;

		// Token: 0x04000135 RID: 309
		private HttpListenerResponse _response;

		// Token: 0x04000136 RID: 310
		private bool _sendChunked;

		// Token: 0x04000137 RID: 311
		private Stream _stream;

		// Token: 0x04000138 RID: 312
		private Action<byte[], int, int> _write;

		// Token: 0x04000139 RID: 313
		private Action<byte[], int, int> _writeBody;

		// Token: 0x0400013A RID: 314
		private Action<byte[], int, int> _writeChunked;
	}
}
