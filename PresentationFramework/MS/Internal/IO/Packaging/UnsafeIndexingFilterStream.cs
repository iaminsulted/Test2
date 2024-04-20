using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using MS.Internal.Interop;
using MS.Win32;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200016A RID: 362
	internal class UnsafeIndexingFilterStream : Stream
	{
		// Token: 0x06000C1C RID: 3100 RVA: 0x0012F7BD File Offset: 0x0012E7BD
		internal UnsafeIndexingFilterStream(IStream oleStream)
		{
			if (oleStream == null)
			{
				throw new ArgumentNullException("oleStream");
			}
			this._oleStream = oleStream;
			this._disposed = false;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0012F7E4 File Offset: 0x0012E7E4
		public unsafe override int Read(byte[] buffer, int offset, int count)
		{
			this.ThrowIfStreamDisposed();
			PackagingUtilities.VerifyStreamReadArgs(this, buffer, offset, count);
			if (count == 0)
			{
				return 0;
			}
			int result;
			IntPtr refToNumBytesRead = new IntPtr((void*)(&result));
			long position = this.Position;
			try
			{
				try
				{
					fixed (byte* ptr = &buffer[offset])
					{
						byte* value = ptr;
						this._oleStream.Read(new IntPtr((void*)value), count, refToNumBytesRead);
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			catch (COMException innerException)
			{
				this.Position = position;
				throw new IOException("Read", innerException);
			}
			catch (IOException innerException2)
			{
				this.Position = position;
				throw new IOException("Read", innerException2);
			}
			return result;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0012F894 File Offset: 0x0012E894
		public unsafe override long Seek(long offset, SeekOrigin origin)
		{
			this.ThrowIfStreamDisposed();
			long result = 0L;
			IntPtr refToNewOffsetNullAllowed = new IntPtr((void*)(&result));
			this._oleStream.Seek(offset, (int)origin, refToNewOffsetNullAllowed);
			return result;
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0012F8C3 File Offset: 0x0012E8C3
		public override void SetLength(long newLength)
		{
			this.ThrowIfStreamDisposed();
			throw new NotSupportedException(SR.Get("StreamDoesNotSupportWrite"));
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0012F8C3 File Offset: 0x0012E8C3
		public override void Write(byte[] buf, int offset, int count)
		{
			this.ThrowIfStreamDisposed();
			throw new NotSupportedException(SR.Get("StreamDoesNotSupportWrite"));
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0012F8DA File Offset: 0x0012E8DA
		public override void Flush()
		{
			this.ThrowIfStreamDisposed();
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x0012F8E2 File Offset: 0x0012E8E2
		public override bool CanRead
		{
			get
			{
				return !this._disposed;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0012F8E2 File Offset: 0x0012E8E2
		public override bool CanSeek
		{
			get
			{
				return !this._disposed;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x0012F8ED File Offset: 0x0012E8ED
		// (set) Token: 0x06000C26 RID: 3110 RVA: 0x0012F8FE File Offset: 0x0012E8FE
		public override long Position
		{
			get
			{
				this.ThrowIfStreamDisposed();
				return this.Seek(0L, SeekOrigin.Current);
			}
			set
			{
				this.ThrowIfStreamDisposed();
				if (value < 0L)
				{
					throw new ArgumentException(SR.Get("CannotSetNegativePosition"));
				}
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x0012F924 File Offset: 0x0012E924
		public override long Length
		{
			get
			{
				this.ThrowIfStreamDisposed();
				STATSTG statstg;
				this._oleStream.Stat(out statstg, 1);
				return statstg.cbSize;
			}
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0012F94C File Offset: 0x0012E94C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._oleStream != null)
				{
					UnsafeNativeMethods.SafeReleaseComObject(this._oleStream);
				}
			}
			finally
			{
				this._oleStream = null;
				this._disposed = true;
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0012F998 File Offset: 0x0012E998
		private void ThrowIfStreamDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(null, SR.Get("StreamObjectDisposed"));
			}
		}

		// Token: 0x04000920 RID: 2336
		private IStream _oleStream;

		// Token: 0x04000921 RID: 2337
		private bool _disposed;
	}
}
