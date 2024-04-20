using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DD RID: 221
	[CallbackIdentity(4524)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ShowToolTip_t
	{
		// Token: 0x040003CE RID: 974
		public const int k_iCallback = 4524;

		// Token: 0x040003CF RID: 975
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003D0 RID: 976
		public string pchMsg;
	}
}
