using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200011B RID: 283
	[CallbackIdentity(1327)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSetUserPublishedFileActionResult_t
	{
		// Token: 0x040004B1 RID: 1201
		public const int k_iCallback = 1327;

		// Token: 0x040004B2 RID: 1202
		public EResult m_eResult;

		// Token: 0x040004B3 RID: 1203
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004B4 RID: 1204
		public EWorkshopFileAction m_eAction;
	}
}
