using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MS.Internal.Interop
{
	// Token: 0x0200017B RID: 379
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000109-0000-0000-C000-000000000046")]
	[ComVisible(true)]
	[ComImport]
	internal interface IPersistStreamWithArrays
	{
		// Token: 0x06000C61 RID: 3169
		void GetClassID(out Guid pClassID);

		// Token: 0x06000C62 RID: 3170
		[PreserveSig]
		int IsDirty();

		// Token: 0x06000C63 RID: 3171
		void Load(IStream pstm);

		// Token: 0x06000C64 RID: 3172
		void Save(IStream pstm, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06000C65 RID: 3173
		void GetSizeMax(out long pcbSize);
	}
}
