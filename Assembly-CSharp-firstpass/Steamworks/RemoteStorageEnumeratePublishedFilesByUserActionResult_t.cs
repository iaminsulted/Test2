using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200011C RID: 284
	[CallbackIdentity(1328)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumeratePublishedFilesByUserActionResult_t
	{
		// Token: 0x040004B5 RID: 1205
		public const int k_iCallback = 1328;

		// Token: 0x040004B6 RID: 1206
		public EResult m_eResult;

		// Token: 0x040004B7 RID: 1207
		public EWorkshopFileAction m_eAction;

		// Token: 0x040004B8 RID: 1208
		public int m_nResultsReturned;

		// Token: 0x040004B9 RID: 1209
		public int m_nTotalResultCount;

		// Token: 0x040004BA RID: 1210
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x040004BB RID: 1211
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public uint[] m_rgRTimeUpdated;
	}
}
