using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000113 RID: 275
	[CallbackIdentity(1319)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateWorkshopFilesResult_t
	{
		// Token: 0x0400048D RID: 1165
		public const int k_iCallback = 1319;

		// Token: 0x0400048E RID: 1166
		public EResult m_eResult;

		// Token: 0x0400048F RID: 1167
		public int m_nResultsReturned;

		// Token: 0x04000490 RID: 1168
		public int m_nTotalResultCount;

		// Token: 0x04000491 RID: 1169
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x04000492 RID: 1170
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public float[] m_rgScore;

		// Token: 0x04000493 RID: 1171
		public AppId_t m_nAppId;

		// Token: 0x04000494 RID: 1172
		public uint m_unStartIndex;
	}
}
