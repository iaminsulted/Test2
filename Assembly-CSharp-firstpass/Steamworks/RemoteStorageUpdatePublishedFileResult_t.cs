using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000110 RID: 272
	[CallbackIdentity(1316)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdatePublishedFileResult_t
	{
		// Token: 0x0400046C RID: 1132
		public const int k_iCallback = 1316;

		// Token: 0x0400046D RID: 1133
		public EResult m_eResult;

		// Token: 0x0400046E RID: 1134
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400046F RID: 1135
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
