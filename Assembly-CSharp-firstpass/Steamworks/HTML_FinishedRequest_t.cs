using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CF RID: 207
	[CallbackIdentity(4506)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_FinishedRequest_t
	{
		// Token: 0x0400038F RID: 911
		public const int k_iCallback = 4506;

		// Token: 0x04000390 RID: 912
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000391 RID: 913
		public string pchURL;

		// Token: 0x04000392 RID: 914
		public string pchPageTitle;
	}
}
