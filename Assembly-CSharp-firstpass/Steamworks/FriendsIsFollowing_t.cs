using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B8 RID: 184
	[CallbackIdentity(345)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsIsFollowing_t
	{
		// Token: 0x0400032F RID: 815
		public const int k_iCallback = 345;

		// Token: 0x04000330 RID: 816
		public EResult m_eResult;

		// Token: 0x04000331 RID: 817
		public CSteamID m_steamID;

		// Token: 0x04000332 RID: 818
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bIsFollowing;
	}
}
