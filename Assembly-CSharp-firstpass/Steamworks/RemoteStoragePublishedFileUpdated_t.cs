using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200011E RID: 286
	[CallbackIdentity(1330)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUpdated_t
	{
		// Token: 0x040004BF RID: 1215
		public const int k_iCallback = 1330;

		// Token: 0x040004C0 RID: 1216
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004C1 RID: 1217
		public AppId_t m_nAppID;

		// Token: 0x040004C2 RID: 1218
		public ulong m_ulUnused;
	}
}
