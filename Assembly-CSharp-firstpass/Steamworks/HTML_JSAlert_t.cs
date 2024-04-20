using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D7 RID: 215
	[CallbackIdentity(4514)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSAlert_t
	{
		// Token: 0x040003B6 RID: 950
		public const int k_iCallback = 4514;

		// Token: 0x040003B7 RID: 951
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003B8 RID: 952
		public string pchMessage;
	}
}
