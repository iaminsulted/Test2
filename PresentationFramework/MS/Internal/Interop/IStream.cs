using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MS.Internal.Interop
{
	// Token: 0x0200017A RID: 378
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000C-0000-0000-C000-000000000046")]
	[ComVisible(true)]
	[ComImport]
	internal interface IStream
	{
		// Token: 0x06000C56 RID: 3158
		void Read(IntPtr bufferBase, int sizeInBytes, IntPtr refToNumBytesRead);

		// Token: 0x06000C57 RID: 3159
		void Write(IntPtr bufferBase, int sizeInBytes, IntPtr refToNumBytesWritten);

		// Token: 0x06000C58 RID: 3160
		void Seek(long offset, int origin, IntPtr refToNewOffsetNullAllowed);

		// Token: 0x06000C59 RID: 3161
		void SetSize(long newSize);

		// Token: 0x06000C5A RID: 3162
		void CopyTo(IStream targetStream, long bytesToCopy, IntPtr refToNumBytesRead, IntPtr refToNumBytesWritten);

		// Token: 0x06000C5B RID: 3163
		void Commit(int commitFlags);

		// Token: 0x06000C5C RID: 3164
		void Revert();

		// Token: 0x06000C5D RID: 3165
		void LockRegion(long offset, long sizeInBytes, int lockType);

		// Token: 0x06000C5E RID: 3166
		void UnlockRegion(long offset, long sizeInBytes, int lockType);

		// Token: 0x06000C5F RID: 3167
		void Stat(out STATSTG statStructure, int statFlag);

		// Token: 0x06000C60 RID: 3168
		void Clone(out IStream newStream);
	}
}
