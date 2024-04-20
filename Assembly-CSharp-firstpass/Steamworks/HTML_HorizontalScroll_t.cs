using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D4 RID: 212
	[CallbackIdentity(4511)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HorizontalScroll_t
	{
		// Token: 0x040003A1 RID: 929
		public const int k_iCallback = 4511;

		// Token: 0x040003A2 RID: 930
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003A3 RID: 931
		public uint unScrollMax;

		// Token: 0x040003A4 RID: 932
		public uint unScrollCurrent;

		// Token: 0x040003A5 RID: 933
		public float flPageScale;

		// Token: 0x040003A6 RID: 934
		[MarshalAs(UnmanagedType.I1)]
		public bool bVisible;

		// Token: 0x040003A7 RID: 935
		public uint unPageSize;
	}
}
