using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000140 RID: 320
	[CallbackIdentity(1104)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardFindResult_t
	{
		// Token: 0x04000531 RID: 1329
		public const int k_iCallback = 1104;

		// Token: 0x04000532 RID: 1330
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x04000533 RID: 1331
		public byte m_bLeaderboardFound;
	}
}
