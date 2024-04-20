using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E9 RID: 233
	[CallbackIdentity(504)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyEnter_t
	{
		// Token: 0x040003FB RID: 1019
		public const int k_iCallback = 504;

		// Token: 0x040003FC RID: 1020
		public ulong m_ulSteamIDLobby;

		// Token: 0x040003FD RID: 1021
		public uint m_rgfChatPermissions;

		// Token: 0x040003FE RID: 1022
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLocked;

		// Token: 0x040003FF RID: 1023
		public uint m_EChatRoomEnterResponse;
	}
}
