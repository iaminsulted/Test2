using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x020002AE RID: 686
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("71e806fb-8dee-46fc-bf8c-7748a8a1ae13")]
	[ComImport]
	internal interface IObjectWithProgId
	{
		// Token: 0x060019C2 RID: 6594
		void SetProgID([MarshalAs(UnmanagedType.LPWStr)] string pszProgID);

		// Token: 0x060019C3 RID: 6595
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetProgID();
	}
}
