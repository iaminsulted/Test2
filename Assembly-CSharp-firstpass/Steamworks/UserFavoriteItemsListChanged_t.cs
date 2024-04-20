using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000129 RID: 297
	[CallbackIdentity(3407)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserFavoriteItemsListChanged_t
	{
		// Token: 0x040004E5 RID: 1253
		public const int k_iCallback = 3407;

		// Token: 0x040004E6 RID: 1254
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004E7 RID: 1255
		public EResult m_eResult;

		// Token: 0x040004E8 RID: 1256
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bWasAddRequest;
	}
}
