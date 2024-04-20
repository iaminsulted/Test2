using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D2 RID: 210
	[CallbackIdentity(4509)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SearchResults_t
	{
		// Token: 0x04000399 RID: 921
		public const int k_iCallback = 4509;

		// Token: 0x0400039A RID: 922
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400039B RID: 923
		public uint unResults;

		// Token: 0x0400039C RID: 924
		public uint unCurrentMatch;
	}
}
