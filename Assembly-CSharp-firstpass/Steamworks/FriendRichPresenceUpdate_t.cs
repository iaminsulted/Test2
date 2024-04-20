using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AF RID: 175
	[CallbackIdentity(336)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendRichPresenceUpdate_t
	{
		// Token: 0x04000311 RID: 785
		public const int k_iCallback = 336;

		// Token: 0x04000312 RID: 786
		public CSteamID m_steamIDFriend;

		// Token: 0x04000313 RID: 787
		public AppId_t m_nAppID;
	}
}
