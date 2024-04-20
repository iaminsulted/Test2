using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000ED RID: 237
	[CallbackIdentity(509)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyGameCreated_t
	{
		// Token: 0x0400040E RID: 1038
		public const int k_iCallback = 509;

		// Token: 0x0400040F RID: 1039
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000410 RID: 1040
		public ulong m_ulSteamIDGameServer;

		// Token: 0x04000411 RID: 1041
		public uint m_unIP;

		// Token: 0x04000412 RID: 1042
		public ushort m_usPort;
	}
}
