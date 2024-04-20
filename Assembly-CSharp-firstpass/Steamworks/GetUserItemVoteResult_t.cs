using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012B RID: 299
	[CallbackIdentity(3409)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetUserItemVoteResult_t
	{
		// Token: 0x040004ED RID: 1261
		public const int k_iCallback = 3409;

		// Token: 0x040004EE RID: 1262
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040004EF RID: 1263
		public EResult m_eResult;

		// Token: 0x040004F0 RID: 1264
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedUp;

		// Token: 0x040004F1 RID: 1265
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedDown;

		// Token: 0x040004F2 RID: 1266
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteSkipped;
	}
}
