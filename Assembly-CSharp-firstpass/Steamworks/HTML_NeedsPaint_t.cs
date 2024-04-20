using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CB RID: 203
	[CallbackIdentity(4502)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_NeedsPaint_t
	{
		// Token: 0x04000373 RID: 883
		public const int k_iCallback = 4502;

		// Token: 0x04000374 RID: 884
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000375 RID: 885
		public IntPtr pBGRA;

		// Token: 0x04000376 RID: 886
		public uint unWide;

		// Token: 0x04000377 RID: 887
		public uint unTall;

		// Token: 0x04000378 RID: 888
		public uint unUpdateX;

		// Token: 0x04000379 RID: 889
		public uint unUpdateY;

		// Token: 0x0400037A RID: 890
		public uint unUpdateWide;

		// Token: 0x0400037B RID: 891
		public uint unUpdateTall;

		// Token: 0x0400037C RID: 892
		public uint unScrollX;

		// Token: 0x0400037D RID: 893
		public uint unScrollY;

		// Token: 0x0400037E RID: 894
		public float flPageScale;

		// Token: 0x0400037F RID: 895
		public uint unPageSerial;
	}
}
