using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F0 RID: 240
	[CallbackIdentity(513)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyCreated_t
	{
		// Token: 0x04000419 RID: 1049
		public const int k_iCallback = 513;

		// Token: 0x0400041A RID: 1050
		public EResult m_eResult;

		// Token: 0x0400041B RID: 1051
		public ulong m_ulSteamIDLobby;
	}
}
