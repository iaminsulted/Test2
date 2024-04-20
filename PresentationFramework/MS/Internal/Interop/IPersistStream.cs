using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Interop
{
	// Token: 0x02000179 RID: 377
	[ComVisible(true)]
	[Guid("00000109-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IPersistStream
	{
		// Token: 0x06000C51 RID: 3153
		void GetClassID(out Guid pClassID);

		// Token: 0x06000C52 RID: 3154
		[PreserveSig]
		int IsDirty();

		// Token: 0x06000C53 RID: 3155
		void Load(IStream pstm);

		// Token: 0x06000C54 RID: 3156
		void Save(IStream pstm, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06000C55 RID: 3157
		void GetSizeMax(out long pcbSize);
	}
}
