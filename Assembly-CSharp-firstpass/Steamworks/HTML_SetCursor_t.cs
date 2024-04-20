using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DB RID: 219
	[CallbackIdentity(4522)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SetCursor_t
	{
		// Token: 0x040003C8 RID: 968
		public const int k_iCallback = 4522;

		// Token: 0x040003C9 RID: 969
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003CA RID: 970
		public uint eMouseCursor;
	}
}
