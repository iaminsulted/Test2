using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B1 RID: 177
	[CallbackIdentity(338)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameConnectedClanChatMsg_t
	{
		// Token: 0x04000317 RID: 791
		public const int k_iCallback = 338;

		// Token: 0x04000318 RID: 792
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000319 RID: 793
		public CSteamID m_steamIDUser;

		// Token: 0x0400031A RID: 794
		public int m_iMessageID;
	}
}
