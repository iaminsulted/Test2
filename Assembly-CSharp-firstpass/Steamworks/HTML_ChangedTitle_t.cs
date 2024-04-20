using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D1 RID: 209
	[CallbackIdentity(4508)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ChangedTitle_t
	{
		// Token: 0x04000396 RID: 918
		public const int k_iCallback = 4508;

		// Token: 0x04000397 RID: 919
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000398 RID: 920
		public string pchTitle;
	}
}
