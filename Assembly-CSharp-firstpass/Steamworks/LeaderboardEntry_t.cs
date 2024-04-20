using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AA RID: 426
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardEntry_t
	{
		// Token: 0x04000A1E RID: 2590
		public CSteamID m_steamIDUser;

		// Token: 0x04000A1F RID: 2591
		public int m_nGlobalRank;

		// Token: 0x04000A20 RID: 2592
		public int m_nScore;

		// Token: 0x04000A21 RID: 2593
		public int m_cDetails;

		// Token: 0x04000A22 RID: 2594
		public UGCHandle_t m_hUGC;
	}
}
