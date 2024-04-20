using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000118 RID: 280
	[CallbackIdentity(1324)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdateUserPublishedItemVoteResult_t
	{
		// Token: 0x040004A5 RID: 1189
		public const int k_iCallback = 1324;

		// Token: 0x040004A6 RID: 1190
		public EResult m_eResult;

		// Token: 0x040004A7 RID: 1191
		public PublishedFileId_t m_nPublishedFileId;
	}
}
