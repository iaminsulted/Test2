using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000108 RID: 264
	[CallbackIdentity(1305)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncStatusCheck_t
	{
		// Token: 0x0400044D RID: 1101
		public const int k_iCallback = 1305;

		// Token: 0x0400044E RID: 1102
		public AppId_t m_nAppID;

		// Token: 0x0400044F RID: 1103
		public EResult m_eResult;
	}
}
