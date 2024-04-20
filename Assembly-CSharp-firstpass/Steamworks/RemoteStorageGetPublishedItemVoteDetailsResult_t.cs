using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000114 RID: 276
	[CallbackIdentity(1320)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedItemVoteDetailsResult_t
	{
		// Token: 0x04000495 RID: 1173
		public const int k_iCallback = 1320;

		// Token: 0x04000496 RID: 1174
		public EResult m_eResult;

		// Token: 0x04000497 RID: 1175
		public PublishedFileId_t m_unPublishedFileId;

		// Token: 0x04000498 RID: 1176
		public int m_nVotesFor;

		// Token: 0x04000499 RID: 1177
		public int m_nVotesAgainst;

		// Token: 0x0400049A RID: 1178
		public int m_nReports;

		// Token: 0x0400049B RID: 1179
		public float m_fScore;
	}
}
