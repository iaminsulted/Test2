using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EB RID: 235
	[CallbackIdentity(506)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatUpdate_t
	{
		// Token: 0x04000404 RID: 1028
		public const int k_iCallback = 506;

		// Token: 0x04000405 RID: 1029
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000406 RID: 1030
		public ulong m_ulSteamIDUserChanged;

		// Token: 0x04000407 RID: 1031
		public ulong m_ulSteamIDMakingChange;

		// Token: 0x04000408 RID: 1032
		public uint m_rgfChatMemberStateChange;
	}
}
