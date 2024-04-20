using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000168 RID: 360
	internal class ManagedIStream : IStream
	{
		// Token: 0x06000C01 RID: 3073 RVA: 0x0012EFE3 File Offset: 0x0012DFE3
		internal ManagedIStream(Stream ioStream)
		{
			if (ioStream == null)
			{
				throw new ArgumentNullException("ioStream");
			}
			this._ioStream = ioStream;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0012F000 File Offset: 0x0012E000
		void IStream.Read(byte[] buffer, int bufferSize, IntPtr bytesReadPtr)
		{
			int val = this._ioStream.Read(buffer, 0, bufferSize);
			if (bytesReadPtr != IntPtr.Zero)
			{
				Marshal.WriteInt32(bytesReadPtr, val);
			}
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0012F030 File Offset: 0x0012E030
		void IStream.Seek(long offset, int origin, IntPtr newPositionPtr)
		{
			SeekOrigin origin2;
			switch (origin)
			{
			case 0:
				origin2 = SeekOrigin.Begin;
				break;
			case 1:
				origin2 = SeekOrigin.Current;
				break;
			case 2:
				origin2 = SeekOrigin.End;
				break;
			default:
				throw new ArgumentOutOfRangeException("origin");
			}
			long val = this._ioStream.Seek(offset, origin2);
			if (newPositionPtr != IntPtr.Zero)
			{
				Marshal.WriteInt64(newPositionPtr, val);
			}
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0012F08A File Offset: 0x0012E08A
		void IStream.SetSize(long libNewSize)
		{
			this._ioStream.SetLength(libNewSize);
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0012F098 File Offset: 0x0012E098
		void IStream.Stat(out STATSTG streamStats, int grfStatFlag)
		{
			streamStats = default(STATSTG);
			streamStats.type = 2;
			streamStats.cbSize = this._ioStream.Length;
			streamStats.grfMode = 0;
			if (this._ioStream.CanRead && this._ioStream.CanWrite)
			{
				streamStats.grfMode |= 2;
				return;
			}
			if (this._ioStream.CanRead)
			{
				streamStats.grfMode |= 0;
				return;
			}
			if (this._ioStream.CanWrite)
			{
				streamStats.grfMode |= 1;
				return;
			}
			throw new IOException(SR.Get("StreamObjectDisposed"));
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0012F132 File Offset: 0x0012E132
		void IStream.Write(byte[] buffer, int bufferSize, IntPtr bytesWrittenPtr)
		{
			this._ioStream.Write(buffer, 0, bufferSize);
			if (bytesWrittenPtr != IntPtr.Zero)
			{
				Marshal.WriteInt32(bytesWrittenPtr, bufferSize);
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0012F156 File Offset: 0x0012E156
		void IStream.Clone(out IStream streamCopy)
		{
			streamCopy = null;
			throw new NotSupportedException();
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0012F160 File Offset: 0x0012E160
		void IStream.CopyTo(IStream targetStream, long bufferSize, IntPtr buffer, IntPtr bytesWrittenPtr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0012F160 File Offset: 0x0012E160
		void IStream.Commit(int flags)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0012F160 File Offset: 0x0012E160
		void IStream.LockRegion(long offset, long byteCount, int lockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0012F160 File Offset: 0x0012E160
		void IStream.Revert()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0012F160 File Offset: 0x0012E160
		void IStream.UnlockRegion(long offset, long byteCount, int lockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000910 RID: 2320
		private Stream _ioStream;
	}
}
