using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012F RID: 303
	[CallbackIdentity(3413)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoveUGCDependencyResult_t
	{
		// Token: 0x040004FB RID: 1275
		public const int k_iCallback = 3413;

		// Token: 0x040004FC RID: 1276
		public EResult m_eResult;

		// Token: 0x040004FD RID: 1277
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004FE RID: 1278
		public PublishedFileId_t m_nChildPublishedFileId;
	}
}
