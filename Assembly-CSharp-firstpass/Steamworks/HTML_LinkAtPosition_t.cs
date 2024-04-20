using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D6 RID: 214
	[CallbackIdentity(4513)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_LinkAtPosition_t
	{
		// Token: 0x040003AF RID: 943
		public const int k_iCallback = 4513;

		// Token: 0x040003B0 RID: 944
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003B1 RID: 945
		public uint x;

		// Token: 0x040003B2 RID: 946
		public uint y;

		// Token: 0x040003B3 RID: 947
		public string pchURL;

		// Token: 0x040003B4 RID: 948
		[MarshalAs(UnmanagedType.I1)]
		public bool bInput;

		// Token: 0x040003B5 RID: 949
		[MarshalAs(UnmanagedType.I1)]
		public bool bLiveLink;
	}
}
