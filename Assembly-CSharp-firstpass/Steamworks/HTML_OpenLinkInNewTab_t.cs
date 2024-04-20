using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D0 RID: 208
	[CallbackIdentity(4507)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_OpenLinkInNewTab_t
	{
		// Token: 0x04000393 RID: 915
		public const int k_iCallback = 4507;

		// Token: 0x04000394 RID: 916
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000395 RID: 917
		public string pchURL;
	}
}
