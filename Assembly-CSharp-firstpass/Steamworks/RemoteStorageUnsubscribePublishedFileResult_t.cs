using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200010F RID: 271
	[CallbackIdentity(1315)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUnsubscribePublishedFileResult_t
	{
		// Token: 0x04000469 RID: 1129
		public const int k_iCallback = 1315;

		// Token: 0x0400046A RID: 1130
		public EResult m_eResult;

		// Token: 0x0400046B RID: 1131
		public PublishedFileId_t m_nPublishedFileId;
	}
}
