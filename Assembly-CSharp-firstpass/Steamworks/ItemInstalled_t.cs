using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000127 RID: 295
	[CallbackIdentity(3405)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ItemInstalled_t
	{
		// Token: 0x040004DE RID: 1246
		public const int k_iCallback = 3405;

		// Token: 0x040004DF RID: 1247
		public AppId_t m_unAppID;

		// Token: 0x040004E0 RID: 1248
		public PublishedFileId_t m_nPublishedFileId;
	}
}
