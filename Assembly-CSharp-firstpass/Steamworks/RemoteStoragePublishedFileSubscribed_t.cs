using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000115 RID: 277
	[CallbackIdentity(1321)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileSubscribed_t
	{
		// Token: 0x0400049C RID: 1180
		public const int k_iCallback = 1321;

		// Token: 0x0400049D RID: 1181
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400049E RID: 1182
		public AppId_t m_nAppID;
	}
}
