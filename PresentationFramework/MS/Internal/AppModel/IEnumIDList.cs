using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x0200029C RID: 668
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214F2-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IEnumIDList
	{
		// Token: 0x06001910 RID: 6416
		[PreserveSig]
		HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);

		// Token: 0x06001911 RID: 6417
		[PreserveSig]
		HRESULT Skip(uint celt);

		// Token: 0x06001912 RID: 6418
		void Reset();

		// Token: 0x06001913 RID: 6419
		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumIDList Clone();
	}
}
