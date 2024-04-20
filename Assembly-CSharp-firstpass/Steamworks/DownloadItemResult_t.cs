using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000128 RID: 296
	[CallbackIdentity(3406)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DownloadItemResult_t
	{
		// Token: 0x040004E1 RID: 1249
		public const int k_iCallback = 3406;

		// Token: 0x040004E2 RID: 1250
		public AppId_t m_unAppID;

		// Token: 0x040004E3 RID: 1251
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004E4 RID: 1252
		public EResult m_eResult;
	}
}
