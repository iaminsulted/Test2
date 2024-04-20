using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DF RID: 223
	[CallbackIdentity(4526)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HideToolTip_t
	{
		// Token: 0x040003D4 RID: 980
		public const int k_iCallback = 4526;

		// Token: 0x040003D5 RID: 981
		public HHTMLBrowser unBrowserHandle;
	}
}
