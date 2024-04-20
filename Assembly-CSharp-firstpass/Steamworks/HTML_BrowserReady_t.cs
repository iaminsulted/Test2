using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CA RID: 202
	[CallbackIdentity(4501)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_BrowserReady_t
	{
		// Token: 0x04000371 RID: 881
		public const int k_iCallback = 4501;

		// Token: 0x04000372 RID: 882
		public HHTMLBrowser unBrowserHandle;
	}
}
