using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CE RID: 206
	[CallbackIdentity(4505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_URLChanged_t
	{
		// Token: 0x04000388 RID: 904
		public const int k_iCallback = 4505;

		// Token: 0x04000389 RID: 905
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400038A RID: 906
		public string pchURL;

		// Token: 0x0400038B RID: 907
		public string pchPostData;

		// Token: 0x0400038C RID: 908
		[MarshalAs(UnmanagedType.I1)]
		public bool bIsRedirect;

		// Token: 0x0400038D RID: 909
		public string pchPageTitle;

		// Token: 0x0400038E RID: 910
		[MarshalAs(UnmanagedType.I1)]
		public bool bNewNavigation;
	}
}
