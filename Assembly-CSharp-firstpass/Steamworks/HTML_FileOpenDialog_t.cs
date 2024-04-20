using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D9 RID: 217
	[CallbackIdentity(4516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_FileOpenDialog_t
	{
		// Token: 0x040003BC RID: 956
		public const int k_iCallback = 4516;

		// Token: 0x040003BD RID: 957
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003BE RID: 958
		public string pchTitle;

		// Token: 0x040003BF RID: 959
		public string pchInitialFile;
	}
}
