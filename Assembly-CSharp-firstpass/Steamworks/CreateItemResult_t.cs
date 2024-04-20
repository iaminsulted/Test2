using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000125 RID: 293
	[CallbackIdentity(3403)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CreateItemResult_t
	{
		// Token: 0x040004D7 RID: 1239
		public const int k_iCallback = 3403;

		// Token: 0x040004D8 RID: 1240
		public EResult m_eResult;

		// Token: 0x040004D9 RID: 1241
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004DA RID: 1242
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
