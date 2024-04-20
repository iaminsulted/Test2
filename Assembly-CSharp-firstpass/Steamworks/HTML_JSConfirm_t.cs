using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D8 RID: 216
	[CallbackIdentity(4515)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSConfirm_t
	{
		// Token: 0x040003B9 RID: 953
		public const int k_iCallback = 4515;

		// Token: 0x040003BA RID: 954
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003BB RID: 955
		public string pchMessage;
	}
}
