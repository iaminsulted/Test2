using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A1 RID: 673
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
	[ComImport]
	internal interface IShellItem
	{
		// Token: 0x0600192B RID: 6443
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		// Token: 0x0600192C RID: 6444
		IShellItem GetParent();

		// Token: 0x0600192D RID: 6445
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetDisplayName(SIGDN sigdnName);

		// Token: 0x0600192E RID: 6446
		uint GetAttributes(SFGAO sfgaoMask);

		// Token: 0x0600192F RID: 6447
		int Compare(IShellItem psi, SICHINT hint);
	}
}
