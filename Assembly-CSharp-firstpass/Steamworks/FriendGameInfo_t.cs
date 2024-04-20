using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A3 RID: 419
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FriendGameInfo_t
	{
		// Token: 0x040009EB RID: 2539
		public CGameID m_gameID;

		// Token: 0x040009EC RID: 2540
		public uint m_unGameIP;

		// Token: 0x040009ED RID: 2541
		public ushort m_usGamePort;

		// Token: 0x040009EE RID: 2542
		public ushort m_usQueryPort;

		// Token: 0x040009EF RID: 2543
		public CSteamID m_steamIDLobby;
	}
}
