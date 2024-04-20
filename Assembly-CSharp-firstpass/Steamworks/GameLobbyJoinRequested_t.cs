using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AC RID: 172
	[CallbackIdentity(333)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameLobbyJoinRequested_t
	{
		// Token: 0x04000305 RID: 773
		public const int k_iCallback = 333;

		// Token: 0x04000306 RID: 774
		public CSteamID m_steamIDLobby;

		// Token: 0x04000307 RID: 775
		public CSteamID m_steamIDFriend;
	}
}
