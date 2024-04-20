using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012A RID: 298
	[CallbackIdentity(3408)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SetUserItemVoteResult_t
	{
		// Token: 0x040004E9 RID: 1257
		public const int k_iCallback = 3408;

		// Token: 0x040004EA RID: 1258
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004EB RID: 1259
		public EResult m_eResult;

		// Token: 0x040004EC RID: 1260
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteUp;
	}
}
