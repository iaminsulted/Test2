using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000105 RID: 261
	[CallbackIdentity(1301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncedClient_t
	{
		// Token: 0x0400043F RID: 1087
		public const int k_iCallback = 1301;

		// Token: 0x04000440 RID: 1088
		public AppId_t m_nAppID;

		// Token: 0x04000441 RID: 1089
		public EResult m_eResult;

		// Token: 0x04000442 RID: 1090
		public int m_unNumDownloads;
	}
}
