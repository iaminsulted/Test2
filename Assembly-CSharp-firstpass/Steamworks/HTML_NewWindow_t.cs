using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DA RID: 218
	[CallbackIdentity(4521)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_NewWindow_t
	{
		// Token: 0x040003C0 RID: 960
		public const int k_iCallback = 4521;

		// Token: 0x040003C1 RID: 961
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003C2 RID: 962
		public string pchURL;

		// Token: 0x040003C3 RID: 963
		public uint unX;

		// Token: 0x040003C4 RID: 964
		public uint unY;

		// Token: 0x040003C5 RID: 965
		public uint unWide;

		// Token: 0x040003C6 RID: 966
		public uint unTall;

		// Token: 0x040003C7 RID: 967
		public HHTMLBrowser unNewWindow_BrowserHandle;
	}
}
