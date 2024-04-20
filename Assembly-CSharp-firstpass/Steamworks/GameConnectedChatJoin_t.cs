using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B2 RID: 178
	[CallbackIdentity(339)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameConnectedChatJoin_t
	{
		// Token: 0x0400031B RID: 795
		public const int k_iCallback = 339;

		// Token: 0x0400031C RID: 796
		public CSteamID m_steamIDClanChat;

		// Token: 0x0400031D RID: 797
		public CSteamID m_steamIDUser;
	}
}
