using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200015A RID: 346
	internal sealed class ByteStream : Stream
	{
		// Token: 0x06000B7D RID: 2941 RVA: 0x0012C8F8 File Offset: 0x0012B8F8
		internal ByteStream(object underlyingStream, FileAccess openAccess)
		{
			ByteStream.SecuritySuppressedIStream value = underlyingStream as ByteStream.SecuritySuppressedIStream;
			this._securitySuppressedIStream = new SecurityCriticalDataForSet<ByteStream.SecuritySuppressedIStream>(value);
			this._access = openAccess;
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0012C925 File Offset: 0x0012B925
		public override bool CanRead
		{
			get
			{
				return !this.StreamDisposed && (FileAccess.Read == (this._access & FileAccess.Read) || FileAccess.ReadWrite == (this._access & FileAccess.ReadWrite));
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x0012C949 File Offset: 0x0012B949
		public override bool CanSeek
		{
			get
			{
				return !this.StreamDisposed;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0012C954 File Offset: 0x0012B954
		public override long Length
		{
			get
			{
				this.CheckDisposedStatus();
				if (!this._isLengthInitialized)
				{
					STATSTG statstg;
					this._securitySuppressedIStream.Value.Stat(out statstg, 1);
					this._isLengthInitialized = true;
					this._length = statstg.cbSize;
				}
				return this._length;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0012C99C File Offset: 0x0012B99C
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x0012C9C8 File Offset: 0x0012B9C8
		public override long Position
		{
			get
			{
				this.CheckDisposedStatus();
				long result = 0L;
				this._securitySuppressedIStream.Value.Seek(0L, 1, out result);
				return result;
			}
			set
			{
				this.CheckDisposedStatus();
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.Get("SetPositionNotSupported"));
				}
				long num = 0L;
				this._securitySuppressedIStream.Value.Seek(value, 0, out num);
				if (value != num)
				{
					throw new IOException(SR.Get("SeekFailed"));
				}
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public override void Flush()
		{
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0012CA20 File Offset: 0x0012BA20
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckDisposedStatus();
			if (!this.CanSeek)
			{
				throw new NotSupportedException(SR.Get("SeekNotSupported"));
			}
			long result = 0L;
			int dwOrigin;
			switch (origin)
			{
			case SeekOrigin.Begin:
				dwOrigin = 0;
				if (0L > offset)
				{
					throw new ArgumentOutOfRangeException("offset", SR.Get("SeekNegative"));
				}
				break;
			case SeekOrigin.Current:
				dwOrigin = 1;
				break;
			case SeekOrigin.End:
				dwOrigin = 2;
				break;
			default:
				throw new InvalidEnumArgumentException("origin", (int)origin, typeof(SeekOrigin));
			}
			this._securitySuppressedIStream.Value.Seek(offset, dwOrigin, out result);
			return result;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0012CAB3 File Offset: 0x0012BAB3
		public override void SetLength(long newLength)
		{
			throw new NotSupportedException(SR.Get("SetLengthNotSupported"));
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0012CAC4 File Offset: 0x0012BAC4
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.CheckDisposedStatus();
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.Get("ReadNotSupported"));
			}
			int num = 0;
			if (count == 0)
			{
				return num;
			}
			if (0 > count)
			{
				throw new ArgumentOutOfRangeException("count", SR.Get("ReadCountNegative"));
			}
			if (0 > offset)
			{
				throw new ArgumentOutOfRangeException("offset", SR.Get("BufferOffsetNegative"));
			}
			if (buffer.Length == 0 || buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.Get("BufferTooSmall"), "buffer");
			}
			if (offset == 0)
			{
				this._securitySuppressedIStream.Value.Read(buffer, count, out num);
			}
			else if (0 < offset)
			{
				byte[] array = new byte[count];
				this._securitySuppressedIStream.Value.Read(array, count, out num);
				if (num > 0)
				{
					Array.Copy(array, 0, buffer, offset, num);
				}
			}
			return num;
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0012CB90 File Offset: 0x0012BB90
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(SR.Get("WriteNotSupported"));
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0012CBA1 File Offset: 0x0012BBA1
		public override void Close()
		{
			this._disposed = true;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0012CBAA File Offset: 0x0012BBAA
		internal void CheckDisposedStatus()
		{
			if (this.StreamDisposed)
			{
				throw new ObjectDisposedException(null, SR.Get("StreamObjectDisposed"));
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x0012CBC5 File Offset: 0x0012BBC5
		private bool StreamDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x040008C7 RID: 2247
		private SecurityCriticalDataForSet<ByteStream.SecuritySuppressedIStream> _securitySuppressedIStream;

		// Token: 0x040008C8 RID: 2248
		private FileAccess _access;

		// Token: 0x040008C9 RID: 2249
		private long _length;

		// Token: 0x040008CA RID: 2250
		private bool _isLengthInitialized;

		// Token: 0x040008CB RID: 2251
		private bool _disposed;

		// Token: 0x020009BE RID: 2494
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000c-0000-0000-C000-000000000046")]
		[ComImport]
		public interface SecuritySuppressedIStream
		{
			// Token: 0x060083C4 RID: 33732
			void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, out int pcbRead);

			// Token: 0x060083C5 RID: 33733
			void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, out int pcbWritten);

			// Token: 0x060083C6 RID: 33734
			void Seek(long dlibMove, int dwOrigin, out long plibNewPosition);

			// Token: 0x060083C7 RID: 33735
			void SetSize(long libNewSize);

			// Token: 0x060083C8 RID: 33736
			void CopyTo(ByteStream.SecuritySuppressedIStream pstm, long cb, out long pcbRead, out long pcbWritten);

			// Token: 0x060083C9 RID: 33737
			void Commit(int grfCommitFlags);

			// Token: 0x060083CA RID: 33738
			void Revert();

			// Token: 0x060083CB RID: 33739
			void LockRegion(long libOffset, long cb, int dwLockType);

			// Token: 0x060083CC RID: 33740
			void UnlockRegion(long libOffset, long cb, int dwLockType);

			// Token: 0x060083CD RID: 33741
			void Stat(out STATSTG pstatstg, int grfStatFlag);

			// Token: 0x060083CE RID: 33742
			void Clone(out ByteStream.SecuritySuppressedIStream ppstm);
		}
	}
}
