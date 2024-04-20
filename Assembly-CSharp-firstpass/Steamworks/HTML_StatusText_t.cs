using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DC RID: 220
	[CallbackIdentity(4523)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_StatusText_t
	{
		// Token: 0x040003CB RID: 971
		public const int k_iCallback = 4523;

		// Token: 0x040003CC RID: 972
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003CD RID: 973
		public string pchMsg;
	}
}
