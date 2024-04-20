using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000116 RID: 278
	[CallbackIdentity(1322)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUnsubscribed_t
	{
		// Token: 0x0400049F RID: 1183
		public const int k_iCallback = 1322;

		// Token: 0x040004A0 RID: 1184
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004A1 RID: 1185
		public AppId_t m_nAppID;
	}
}
