using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000117 RID: 279
	[CallbackIdentity(1323)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileDeleted_t
	{
		// Token: 0x040004A2 RID: 1186
		public const int k_iCallback = 1323;

		// Token: 0x040004A3 RID: 1187
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004A4 RID: 1188
		public AppId_t m_nAppID;
	}
}
