using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DE RID: 222
	[CallbackIdentity(4525)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_UpdateToolTip_t
	{
		// Token: 0x040003D1 RID: 977
		public const int k_iCallback = 4525;

		// Token: 0x040003D2 RID: 978
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003D3 RID: 979
		public string pchMsg;
	}
}
