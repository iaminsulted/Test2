using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A2 RID: 674
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93")]
	[ComImport]
	internal interface IShellItem2 : IShellItem
	{
		// Token: 0x06001930 RID: 6448
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		// Token: 0x06001931 RID: 6449
		IShellItem GetParent();

		// Token: 0x06001932 RID: 6450
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetDisplayName(SIGDN sigdnName);

		// Token: 0x06001933 RID: 6451
		SFGAO GetAttributes(SFGAO sfgaoMask);

		// Token: 0x06001934 RID: 6452
		int Compare(IShellItem psi, SICHINT hint);

		// Token: 0x06001935 RID: 6453
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStore(GPS flags, [In] ref Guid riid);

		// Token: 0x06001936 RID: 6454
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreWithCreateObject(GPS flags, [MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid);

		// Token: 0x06001937 RID: 6455
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreForKeys(IntPtr rgKeys, uint cKeys, GPS flags, [In] ref Guid riid);

		// Token: 0x06001938 RID: 6456
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyDescriptionList(IntPtr keyType, [In] ref Guid riid);

		// Token: 0x06001939 RID: 6457
		void Update(IBindCtx pbc);

		// Token: 0x0600193A RID: 6458
		void GetProperty(IntPtr key, [In] [Out] PROPVARIANT pv);

		// Token: 0x0600193B RID: 6459
		Guid GetCLSID(IntPtr key);

		// Token: 0x0600193C RID: 6460
		FILETIME GetFileTime(IntPtr key);

		// Token: 0x0600193D RID: 6461
		int GetInt32(IntPtr key);

		// Token: 0x0600193E RID: 6462
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetString(IntPtr key);

		// Token: 0x0600193F RID: 6463
		uint GetUInt32(IntPtr key);

		// Token: 0x06001940 RID: 6464
		ulong GetUInt64(IntPtr key);

		// Token: 0x06001941 RID: 6465
		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetBool(IntPtr key);
	}
}
