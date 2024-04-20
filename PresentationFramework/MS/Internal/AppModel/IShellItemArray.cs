using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A3 RID: 675
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
	[ComImport]
	internal interface IShellItemArray
	{
		// Token: 0x06001942 RID: 6466
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid rbhid, [In] ref Guid riid);

		// Token: 0x06001943 RID: 6467
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStore(int flags, [In] ref Guid riid);

		// Token: 0x06001944 RID: 6468
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyDescriptionList([In] ref PKEY keyType, [In] ref Guid riid);

		// Token: 0x06001945 RID: 6469
		uint GetAttributes(SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask);

		// Token: 0x06001946 RID: 6470
		uint GetCount();

		// Token: 0x06001947 RID: 6471
		IShellItem GetItemAt(uint dwIndex);

		// Token: 0x06001948 RID: 6472
		[return: MarshalAs(UnmanagedType.Interface)]
		object EnumItems();
	}
}
