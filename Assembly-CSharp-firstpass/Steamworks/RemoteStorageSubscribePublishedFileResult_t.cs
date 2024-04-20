using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200010D RID: 269
	[CallbackIdentity(1313)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSubscribePublishedFileResult_t
	{
		// Token: 0x04000460 RID: 1120
		public const int k_iCallback = 1313;

		// Token: 0x04000461 RID: 1121
		public EResult m_eResult;

		// Token: 0x04000462 RID: 1122
		public PublishedFileId_t m_nPublishedFileId;
	}
}
