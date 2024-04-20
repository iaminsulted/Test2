using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000106 RID: 262
	[CallbackIdentity(1302)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncedServer_t
	{
		// Token: 0x04000443 RID: 1091
		public const int k_iCallback = 1302;

		// Token: 0x04000444 RID: 1092
		public AppId_t m_nAppID;

		// Token: 0x04000445 RID: 1093
		public EResult m_eResult;

		// Token: 0x04000446 RID: 1094
		public int m_unNumUploads;
	}
}
