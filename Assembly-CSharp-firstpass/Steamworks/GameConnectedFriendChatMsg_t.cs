using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B6 RID: 182
	[CallbackIdentity(343)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameConnectedFriendChatMsg_t
	{
		// Token: 0x04000328 RID: 808
		public const int k_iCallback = 343;

		// Token: 0x04000329 RID: 809
		public CSteamID m_steamIDUser;

		// Token: 0x0400032A RID: 810
		public int m_iMessageID;
	}
}
