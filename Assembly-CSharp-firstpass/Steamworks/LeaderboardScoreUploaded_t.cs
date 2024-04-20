using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000142 RID: 322
	[CallbackIdentity(1106)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoreUploaded_t
	{
		// Token: 0x04000538 RID: 1336
		public const int k_iCallback = 1106;

		// Token: 0x04000539 RID: 1337
		public byte m_bSuccess;

		// Token: 0x0400053A RID: 1338
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x0400053B RID: 1339
		public int m_nScore;

		// Token: 0x0400053C RID: 1340
		public byte m_bScoreChanged;

		// Token: 0x0400053D RID: 1341
		public int m_nGlobalRankNew;

		// Token: 0x0400053E RID: 1342
		public int m_nGlobalRankPrevious;
	}
}
