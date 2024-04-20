using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B7 RID: 183
	[CallbackIdentity(344)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsGetFollowerCount_t
	{
		// Token: 0x0400032B RID: 811
		public const int k_iCallback = 344;

		// Token: 0x0400032C RID: 812
		public EResult m_eResult;

		// Token: 0x0400032D RID: 813
		public CSteamID m_steamID;

		// Token: 0x0400032E RID: 814
		public int m_nCount;
	}
}
