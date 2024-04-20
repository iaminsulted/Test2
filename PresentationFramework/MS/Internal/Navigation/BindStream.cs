using System;
using System.IO;
using System.Reflection;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal.AppModel;

namespace MS.Internal.Navigation
{
	// Token: 0x02000157 RID: 343
	internal class BindStream : Stream, IStreamInfo
	{
		// Token: 0x06000B58 RID: 2904 RVA: 0x0012C308 File Offset: 0x0012B308
		internal BindStream(Stream stream, long maxBytes, Uri uri, IContentContainer cc, Dispatcher callbackDispatcher)
		{
			this._bytesRead = 0L;
			this._maxBytes = maxBytes;
			this._lastProgressEventByte = 0L;
			this._stream = stream;
			this._uri = uri;
			this._cc = cc;
			this._callbackDispatcher = callbackDispatcher;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0012C348 File Offset: 0x0012B348
		private void UpdateNavigationProgress()
		{
			for (long num = this._lastProgressEventByte + 1024L; num <= this._bytesRead; num += 1024L)
			{
				this.UpdateNavProgressHelper(num);
				this._lastProgressEventByte = num;
			}
			if (this._bytesRead == this._maxBytes && this._lastProgressEventByte < this._maxBytes)
			{
				this.UpdateNavProgressHelper(this._maxBytes);
				this._lastProgressEventByte = this._maxBytes;
			}
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0012C3BC File Offset: 0x0012B3BC
		private void UpdateNavProgressHelper(long numBytes)
		{
			if (this._callbackDispatcher != null && !this._callbackDispatcher.CheckAccess())
			{
				this._callbackDispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
				{
					this._cc.OnNavigationProgress(this._uri, numBytes, this._maxBytes);
					return null;
				}), null);
				return;
			}
			this._cc.OnNavigationProgress(this._uri, numBytes, this._maxBytes);
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0012C42B File Offset: 0x0012B42B
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0012C438 File Offset: 0x0012B438
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0012C445 File Offset: 0x0012B445
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x0012C452 File Offset: 0x0012B452
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0012C45F File Offset: 0x0012B45F
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x0012C46C File Offset: 0x0012B46C
		public override long Position
		{
			get
			{
				return this._stream.Position;
			}
			set
			{
				this._stream.Position = value;
			}
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0012C47A File Offset: 0x0012B47A
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0012C48E File Offset: 0x0012B48E
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0012C4A4 File Offset: 0x0012B4A4
		public override void Close()
		{
			this._stream.Close();
			if (this._callbackDispatcher != null && !this._callbackDispatcher.CheckAccess())
			{
				this._callbackDispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
				{
					this._cc.OnStreamClosed(this._uri);
					return null;
				}), null);
				return;
			}
			this._cc.OnStreamClosed(this._uri);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0012C4FE File Offset: 0x0012B4FE
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._stream.EndRead(asyncResult);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0012C50C File Offset: 0x0012B50C
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._stream.EndWrite(asyncResult);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0012C51A File Offset: 0x0012B51A
		public override bool Equals(object obj)
		{
			return this._stream.Equals(obj);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0012C528 File Offset: 0x0012B528
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0012C535 File Offset: 0x0012B535
		public override int GetHashCode()
		{
			return this._stream.GetHashCode();
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0012C542 File Offset: 0x0012B542
		[Obsolete("InitializeLifetimeService is obsolete.", false)]
		public override object InitializeLifetimeService()
		{
			return this._stream.InitializeLifetimeService();
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0012C550 File Offset: 0x0012B550
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this._stream.Read(buffer, offset, count);
			this._bytesRead += (long)num;
			this._maxBytes = ((this._bytesRead > this._maxBytes) ? this._bytesRead : this._maxBytes);
			if (this._lastProgressEventByte + 1024L <= this._bytesRead || num == 0)
			{
				this.UpdateNavigationProgress();
			}
			return num;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0012C5BC File Offset: 0x0012B5BC
		public override int ReadByte()
		{
			int num = this._stream.ReadByte();
			if (num != -1)
			{
				this._bytesRead += 1L;
				this._maxBytes = ((this._bytesRead > this._maxBytes) ? this._bytesRead : this._maxBytes);
			}
			if (this._lastProgressEventByte + 1024L <= this._bytesRead || num == -1)
			{
				this.UpdateNavigationProgress();
			}
			return num;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0012C62A File Offset: 0x0012B62A
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._stream.Seek(offset, origin);
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0012C639 File Offset: 0x0012B639
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0012C647 File Offset: 0x0012B647
		public override string ToString()
		{
			return this._stream.ToString();
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0012C654 File Offset: 0x0012B654
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._stream.Write(buffer, offset, count);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0012C664 File Offset: 0x0012B664
		public override void WriteByte(byte value)
		{
			this._stream.WriteByte(value);
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x0012C672 File Offset: 0x0012B672
		public Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0012C67C File Offset: 0x0012B67C
		Assembly IStreamInfo.Assembly
		{
			get
			{
				Assembly result = null;
				if (this._stream != null)
				{
					IStreamInfo streamInfo = this._stream as IStreamInfo;
					if (streamInfo != null)
					{
						result = streamInfo.Assembly;
					}
				}
				return result;
			}
		}

		// Token: 0x040008BD RID: 2237
		private long _bytesRead;

		// Token: 0x040008BE RID: 2238
		private long _maxBytes;

		// Token: 0x040008BF RID: 2239
		private long _lastProgressEventByte;

		// Token: 0x040008C0 RID: 2240
		private Stream _stream;

		// Token: 0x040008C1 RID: 2241
		private Uri _uri;

		// Token: 0x040008C2 RID: 2242
		private IContentContainer _cc;

		// Token: 0x040008C3 RID: 2243
		private Dispatcher _callbackDispatcher;

		// Token: 0x040008C4 RID: 2244
		private const long _bytesInterval = 1024L;
	}
}
