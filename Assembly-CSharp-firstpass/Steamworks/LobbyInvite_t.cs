using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E8 RID: 232
	[CallbackIdentity(503)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyInvite_t
	{
		// Token: 0x040003F7 RID: 1015
		public const int k_iCallback = 503;

		// Token: 0x040003F8 RID: 1016
		public ulong m_ulSteamIDUser;

		// Token: 0x040003F9 RID: 1017
		public ulong m_ulSteamIDLobby;

		// Token: 0x040003FA RID: 1018
		public ulong m_ulGameID;
	}
}
