using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200010B RID: 267
	[CallbackIdentity(1311)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDeletePublishedFileResult_t
	{
		// Token: 0x04000458 RID: 1112
		public const int k_iCallback = 1311;

		// Token: 0x04000459 RID: 1113
		public EResult m_eResult;

		// Token: 0x0400045A RID: 1114
		public PublishedFileId_t m_nPublishedFileId;
	}
}
