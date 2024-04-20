using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000119 RID: 281
	[CallbackIdentity(1325)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUserVoteDetails_t
	{
		// Token: 0x040004A8 RID: 1192
		public const int k_iCallback = 1325;

		// Token: 0x040004A9 RID: 1193
		public EResult m_eResult;

		// Token: 0x040004AA RID: 1194
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004AB RID: 1195
		public EWorkshopVote m_eVote;
	}
}
