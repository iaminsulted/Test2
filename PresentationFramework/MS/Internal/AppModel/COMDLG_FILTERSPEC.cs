using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x02000299 RID: 665
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct COMDLG_FILTERSPEC
	{
		// Token: 0x04000D82 RID: 3458
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszName;

		// Token: 0x04000D83 RID: 3459
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszSpec;
	}
}
