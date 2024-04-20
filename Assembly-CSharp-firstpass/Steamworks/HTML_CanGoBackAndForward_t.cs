using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D3 RID: 211
	[CallbackIdentity(4510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_CanGoBackAndForward_t
	{
		// Token: 0x0400039D RID: 925
		public const int k_iCallback = 4510;

		// Token: 0x0400039E RID: 926
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400039F RID: 927
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoBack;

		// Token: 0x040003A0 RID: 928
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoForward;
	}
}
