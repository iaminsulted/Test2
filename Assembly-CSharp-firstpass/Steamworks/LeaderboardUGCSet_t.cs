using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000147 RID: 327
	[CallbackIdentity(1111)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardUGCSet_t
	{
		// Token: 0x0400054C RID: 1356
		public const int k_iCallback = 1111;

		// Token: 0x0400054D RID: 1357
		public EResult m_eResult;

		// Token: 0x0400054E RID: 1358
		public SteamLeaderboard_t m_hSteamLeaderboard;
	}
}
