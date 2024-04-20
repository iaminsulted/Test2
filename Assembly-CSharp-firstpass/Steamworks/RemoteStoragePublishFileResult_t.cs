using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200010A RID: 266
	[CallbackIdentity(1309)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileResult_t
	{
		// Token: 0x04000454 RID: 1108
		public const int k_iCallback = 1309;

		// Token: 0x04000455 RID: 1109
		public EResult m_eResult;

		// Token: 0x04000456 RID: 1110
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000457 RID: 1111
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
