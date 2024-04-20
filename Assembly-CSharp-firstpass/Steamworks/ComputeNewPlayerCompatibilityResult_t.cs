using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C6 RID: 198
	[CallbackIdentity(211)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ComputeNewPlayerCompatibilityResult_t
	{
		// Token: 0x04000363 RID: 867
		public const int k_iCallback = 211;

		// Token: 0x04000364 RID: 868
		public EResult m_eResult;

		// Token: 0x04000365 RID: 869
		public int m_cPlayersThatDontLikeCandidate;

		// Token: 0x04000366 RID: 870
		public int m_cPlayersThatCandidateDoesntLike;

		// Token: 0x04000367 RID: 871
		public int m_cClanPlayersThatDontLikeCandidate;

		// Token: 0x04000368 RID: 872
		public CSteamID m_SteamIDCandidate;
	}
}
