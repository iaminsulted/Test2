using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002AB RID: 683
	[Guid("3c594f9f-9f30-47a1-979a-c9e83d3d0a06")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IApplicationDocumentLists
	{
		// Token: 0x060019B5 RID: 6581
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x060019B6 RID: 6582
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetList(ADLT listtype, uint cItemsDesired, [In] ref Guid riid);
	}
}
