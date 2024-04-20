using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EF RID: 239
	[CallbackIdentity(512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyKicked_t
	{
		// Token: 0x04000415 RID: 1045
		public const int k_iCallback = 512;

		// Token: 0x04000416 RID: 1046
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000417 RID: 1047
		public ulong m_ulSteamIDAdmin;

		// Token: 0x04000418 RID: 1048
		public byte m_bKickedDueToDisconnect;
	}
}
