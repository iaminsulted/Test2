using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x0200029F RID: 671
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
	[ComImport]
	internal interface IPropertyStore
	{
		// Token: 0x0600191C RID: 6428
		uint GetCount();

		// Token: 0x0600191D RID: 6429
		PKEY GetAt(uint iProp);

		// Token: 0x0600191E RID: 6430
		void GetValue([In] ref PKEY pkey, [In] [Out] PROPVARIANT pv);

		// Token: 0x0600191F RID: 6431
		void SetValue([In] ref PKEY pkey, PROPVARIANT pv);

		// Token: 0x06001920 RID: 6432
		void Commit();
	}
}
