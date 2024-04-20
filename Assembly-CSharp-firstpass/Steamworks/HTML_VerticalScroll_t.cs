using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D5 RID: 213
	[CallbackIdentity(4512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_VerticalScroll_t
	{
		// Token: 0x040003A8 RID: 936
		public const int k_iCallback = 4512;

		// Token: 0x040003A9 RID: 937
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040003AA RID: 938
		public uint unScrollMax;

		// Token: 0x040003AB RID: 939
		public uint unScrollCurrent;

		// Token: 0x040003AC RID: 940
		public float flPageScale;

		// Token: 0x040003AD RID: 941
		[MarshalAs(UnmanagedType.I1)]
		public bool bVisible;

		// Token: 0x040003AE RID: 942
		public uint unPageSize;
	}
}
