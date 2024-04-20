using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CD RID: 205
	[CallbackIdentity(4504)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_CloseBrowser_t
	{
		// Token: 0x04000386 RID: 902
		public const int k_iCallback = 4504;

		// Token: 0x04000387 RID: 903
		public HHTMLBrowser unBrowserHandle;
	}
}
