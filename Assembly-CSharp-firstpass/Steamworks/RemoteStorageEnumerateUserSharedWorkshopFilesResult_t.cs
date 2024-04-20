using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200011A RID: 282
	[CallbackIdentity(1326)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserSharedWorkshopFilesResult_t
	{
		// Token: 0x040004AC RID: 1196
		public const int k_iCallback = 1326;

		// Token: 0x040004AD RID: 1197
		public EResult m_eResult;

		// Token: 0x040004AE RID: 1198
		public int m_nResultsReturned;

		// Token: 0x040004AF RID: 1199
		public int m_nTotalResultCount;

		// Token: 0x040004B0 RID: 1200
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;
	}
}
