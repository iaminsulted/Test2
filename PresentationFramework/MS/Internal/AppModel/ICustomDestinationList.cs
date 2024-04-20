using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002AC RID: 684
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6332debf-87b5-4670-90c0-5e57b408a49e")]
	[ComImport]
	internal interface ICustomDestinationList
	{
		// Token: 0x060019B7 RID: 6583
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x060019B8 RID: 6584
		[return: MarshalAs(UnmanagedType.Interface)]
		object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

		// Token: 0x060019B9 RID: 6585
		[PreserveSig]
		HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, IObjectArray poa);

		// Token: 0x060019BA RID: 6586
		void AppendKnownCategory(KDC category);

		// Token: 0x060019BB RID: 6587
		[PreserveSig]
		HRESULT AddUserTasks(IObjectArray poa);

		// Token: 0x060019BC RID: 6588
		void CommitList();

		// Token: 0x060019BD RID: 6589
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetRemovedDestinations([In] ref Guid riid);

		// Token: 0x060019BE RID: 6590
		void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x060019BF RID: 6591
		void AbortList();
	}
}
