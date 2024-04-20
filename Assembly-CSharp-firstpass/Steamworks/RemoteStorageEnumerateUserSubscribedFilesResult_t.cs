using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200010E RID: 270
	[CallbackIdentity(1314)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserSubscribedFilesResult_t
	{
		// Token: 0x04000463 RID: 1123
		public const int k_iCallback = 1314;

		// Token: 0x04000464 RID: 1124
		public EResult m_eResult;

		// Token: 0x04000465 RID: 1125
		public int m_nResultsReturned;

		// Token: 0x04000466 RID: 1126
		public int m_nTotalResultCount;

		// Token: 0x04000467 RID: 1127
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x04000468 RID: 1128
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public uint[] m_rgRTimeSubscribed;
	}
}
