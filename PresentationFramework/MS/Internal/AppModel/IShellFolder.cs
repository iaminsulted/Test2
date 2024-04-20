using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A0 RID: 672
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214E6-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IShellFolder
	{
		// Token: 0x06001921 RID: 6433
		void ParseDisplayName(IntPtr hwnd, IBindCtx pbc, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In] [Out] ref int pchEaten, out IntPtr ppidl, [In] [Out] ref uint pdwAttributes);

		// Token: 0x06001922 RID: 6434
		IEnumIDList EnumObjects(IntPtr hwnd, SHCONTF grfFlags);

		// Token: 0x06001923 RID: 6435
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToObject(IntPtr pidl, IBindCtx pbc, [In] ref Guid riid);

		// Token: 0x06001924 RID: 6436
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToStorage(IntPtr pidl, IBindCtx pbc, [In] ref Guid riid);

		// Token: 0x06001925 RID: 6437
		[PreserveSig]
		HRESULT CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);

		// Token: 0x06001926 RID: 6438
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid);

		// Token: 0x06001927 RID: 6439
		void GetAttributesOf(uint cidl, IntPtr apidl, [In] [Out] ref SFGAO rgfInOut);

		// Token: 0x06001928 RID: 6440
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetUIObjectOf(IntPtr hwndOwner, uint cidl, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysInt, SizeParamIndex = 1)] IntPtr apidl, [In] ref Guid riid, [In] [Out] ref uint rgfReserved);

		// Token: 0x06001929 RID: 6441
		void GetDisplayNameOf(IntPtr pidl, SHGDN uFlags, out IntPtr pName);

		// Token: 0x0600192A RID: 6442
		void SetNameOf(IntPtr hwnd, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszName, SHGDN uFlags, out IntPtr ppidlOut);
	}
}
