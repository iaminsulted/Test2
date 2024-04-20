using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000111 RID: 273
	[CallbackIdentity(1317)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDownloadUGCResult_t
	{
		// Token: 0x04000470 RID: 1136
		public const int k_iCallback = 1317;

		// Token: 0x04000471 RID: 1137
		public EResult m_eResult;

		// Token: 0x04000472 RID: 1138
		public UGCHandle_t m_hFile;

		// Token: 0x04000473 RID: 1139
		public AppId_t m_nAppID;

		// Token: 0x04000474 RID: 1140
		public int m_nSizeInBytes;

		// Token: 0x04000475 RID: 1141
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		// Token: 0x04000476 RID: 1142
		public ulong m_ulSteamIDOwner;
	}
}
