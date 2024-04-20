using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EC RID: 236
	[CallbackIdentity(507)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatMsg_t
	{
		// Token: 0x04000409 RID: 1033
		public const int k_iCallback = 507;

		// Token: 0x0400040A RID: 1034
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400040B RID: 1035
		public ulong m_ulSteamIDUser;

		// Token: 0x0400040C RID: 1036
		public byte m_eChatEntryType;

		// Token: 0x0400040D RID: 1037
		public uint m_iChatID;
	}
}
