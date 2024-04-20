using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EA RID: 234
	[CallbackIdentity(505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyDataUpdate_t
	{
		// Token: 0x04000400 RID: 1024
		public const int k_iCallback = 505;

		// Token: 0x04000401 RID: 1025
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000402 RID: 1026
		public ulong m_ulSteamIDMember;

		// Token: 0x04000403 RID: 1027
		public byte m_bSuccess;
	}
}
