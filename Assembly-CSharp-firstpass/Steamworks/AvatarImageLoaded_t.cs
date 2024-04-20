using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AD RID: 173
	[CallbackIdentity(334)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AvatarImageLoaded_t
	{
		// Token: 0x04000308 RID: 776
		public const int k_iCallback = 334;

		// Token: 0x04000309 RID: 777
		public CSteamID m_steamID;

		// Token: 0x0400030A RID: 778
		public int m_iImage;

		// Token: 0x0400030B RID: 779
		public int m_iWide;

		// Token: 0x0400030C RID: 780
		public int m_iTall;
	}
}
