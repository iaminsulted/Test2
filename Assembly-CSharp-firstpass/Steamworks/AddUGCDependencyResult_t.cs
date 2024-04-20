using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012E RID: 302
	[CallbackIdentity(3412)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AddUGCDependencyResult_t
	{
		// Token: 0x040004F7 RID: 1271
		public const int k_iCallback = 3412;

		// Token: 0x040004F8 RID: 1272
		public EResult m_eResult;

		// Token: 0x040004F9 RID: 1273
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004FA RID: 1274
		public PublishedFileId_t m_nChildPublishedFileId;
	}
}
