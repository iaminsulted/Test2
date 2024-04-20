using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x020002AA RID: 682
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("12337d35-94c6-48a0-bce7-6a9c69d4d600")]
	[ComImport]
	internal interface IApplicationDestinations
	{
		// Token: 0x060019B2 RID: 6578
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x060019B3 RID: 6579
		void RemoveDestination([MarshalAs(UnmanagedType.IUnknown)] object punk);

		// Token: 0x060019B4 RID: 6580
		void RemoveAllDestinations();
	}
}
