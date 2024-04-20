using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A6 RID: 678
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b4db1657-70d7-485e-8e3e-6fcb5a5c1802")]
	[ComImport]
	internal interface IModalWindow
	{
		// Token: 0x06001962 RID: 6498
		[PreserveSig]
		HRESULT Show(IntPtr parent);
	}
}
