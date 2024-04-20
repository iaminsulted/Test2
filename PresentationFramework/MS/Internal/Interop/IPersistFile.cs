using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Interop
{
	// Token: 0x0200017C RID: 380
	[ComVisible(true)]
	[Guid("0000010b-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IPersistFile
	{
		// Token: 0x06000C66 RID: 3174
		void GetClassID(out Guid pClassID);

		// Token: 0x06000C67 RID: 3175
		[PreserveSig]
		int IsDirty();

		// Token: 0x06000C68 RID: 3176
		void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);

		// Token: 0x06000C69 RID: 3177
		void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06000C6A RID: 3178
		void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		// Token: 0x06000C6B RID: 3179
		[PreserveSig]
		int GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
	}
}
