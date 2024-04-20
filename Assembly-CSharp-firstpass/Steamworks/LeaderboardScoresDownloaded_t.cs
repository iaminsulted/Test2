using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000141 RID: 321
	[CallbackIdentity(1105)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoresDownloaded_t
	{
		// Token: 0x04000534 RID: 1332
		public const int k_iCallback = 1105;

		// Token: 0x04000535 RID: 1333
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x04000536 RID: 1334
		public SteamLeaderboardEntries_t m_hSteamLeaderboardEntries;

		// Token: 0x04000537 RID: 1335
		public int m_cEntryCount;
	}
}
