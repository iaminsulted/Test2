using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000107 RID: 263
	[CallbackIdentity(1303)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncProgress_t
	{
		// Token: 0x04000447 RID: 1095
		public const int k_iCallback = 1303;

		// Token: 0x04000448 RID: 1096
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_rgchCurrentFile;

		// Token: 0x04000449 RID: 1097
		public AppId_t m_nAppID;

		// Token: 0x0400044A RID: 1098
		public uint m_uBytesTransferredThisChunk;

		// Token: 0x0400044B RID: 1099
		public double m_dAppPercentComplete;

		// Token: 0x0400044C RID: 1100
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUploading;
	}
}
