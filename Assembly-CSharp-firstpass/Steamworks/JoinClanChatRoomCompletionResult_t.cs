using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B5 RID: 181
	[CallbackIdentity(342)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct JoinClanChatRoomCompletionResult_t
	{
		// Token: 0x04000325 RID: 805
		public const int k_iCallback = 342;

		// Token: 0x04000326 RID: 806
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000327 RID: 807
		public EChatRoomEnterResponse m_eChatRoomEnterResponse;
	}
}
