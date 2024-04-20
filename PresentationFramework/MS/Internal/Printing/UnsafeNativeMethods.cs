using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Printing
{
	// Token: 0x02000156 RID: 342
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06000B54 RID: 2900
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto)]
		internal static extern int PrintDlgEx(IntPtr pdex);

		// Token: 0x06000B55 RID: 2901
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GlobalFree(IntPtr hMem);

		// Token: 0x06000B56 RID: 2902
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GlobalLock(IntPtr hMem);

		// Token: 0x06000B57 RID: 2903
		[DllImport("kernel32.dll")]
		internal static extern bool GlobalUnlock(IntPtr hMem);
	}
}
