using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200010C RID: 268
	[CallbackIdentity(1312)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserPublishedFilesResult_t
	{
		// Token: 0x0400045B RID: 1115
		public const int k_iCallback = 1312;

		// Token: 0x0400045C RID: 1116
		public EResult m_eResult;

		// Token: 0x0400045D RID: 1117
		public int m_nResultsReturned;

		// Token: 0x0400045E RID: 1118
		public int m_nTotalResultCount;

		// Token: 0x0400045F RID: 1119
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;
	}
}
