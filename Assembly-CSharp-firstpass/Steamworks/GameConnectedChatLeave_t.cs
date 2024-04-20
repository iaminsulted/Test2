using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B3 RID: 179
	[CallbackIdentity(340)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct GameConnectedChatLeave_t
	{
		// Token: 0x0400031E RID: 798
		public const int k_iCallback = 340;

		// Token: 0x0400031F RID: 799
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000320 RID: 800
		public CSteamID m_steamIDUser;

		// Token: 0x04000321 RID: 801
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bKicked;

		// Token: 0x04000322 RID: 802
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bDropped;
	}
}
