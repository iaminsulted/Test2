using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B0 RID: 176
	[CallbackIdentity(337)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameRichPresenceJoinRequested_t
	{
		// Token: 0x04000314 RID: 788
		public const int k_iCallback = 337;

		// Token: 0x04000315 RID: 789
		public CSteamID m_steamIDFriend;

		// Token: 0x04000316 RID: 790
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchConnect;
	}
}
