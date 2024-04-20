using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CC RID: 204
	[CallbackIdentity(4503)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_StartRequest_t
	{
		// Token: 0x04000380 RID: 896
		public const int k_iCallback = 4503;

		// Token: 0x04000381 RID: 897
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000382 RID: 898
		public string pchURL;

		// Token: 0x04000383 RID: 899
		public string pchTarget;

		// Token: 0x04000384 RID: 900
		public string pchPostData;

		// Token: 0x04000385 RID: 901
		[MarshalAs(UnmanagedType.I1)]
		public bool bIsRedirect;
	}
}
