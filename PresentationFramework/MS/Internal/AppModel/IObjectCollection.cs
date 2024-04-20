using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x0200029E RID: 670
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[ComImport]
	internal interface IObjectCollection : IObjectArray
	{
		// Token: 0x06001916 RID: 6422
		uint GetCount();

		// Token: 0x06001917 RID: 6423
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAt([In] uint uiIndex, [In] ref Guid riid);

		// Token: 0x06001918 RID: 6424
		void AddObject([MarshalAs(UnmanagedType.IUnknown)] object punk);

		// Token: 0x06001919 RID: 6425
		void AddFromArray(IObjectArray poaSource);

		// Token: 0x0600191A RID: 6426
		void RemoveObjectAt(uint uiIndex);

		// Token: 0x0600191B RID: 6427
		void Clear();
	}
}
