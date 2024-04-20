using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x020002AD RID: 685
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("36db0196-9665-46d1-9ba7-d3709eecf9ed")]
	[ComImport]
	internal interface IObjectWithAppUserModelId
	{
		// Token: 0x060019C0 RID: 6592
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x060019C1 RID: 6593
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAppID();
	}
}
