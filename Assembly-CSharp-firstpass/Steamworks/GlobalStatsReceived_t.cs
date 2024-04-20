using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000148 RID: 328
	[CallbackIdentity(1112)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalStatsReceived_t
	{
		// Token: 0x0400054F RID: 1359
		public const int k_iCallback = 1112;

		// Token: 0x04000550 RID: 1360
		public ulong m_nGameID;

		// Token: 0x04000551 RID: 1361
		public EResult m_eResult;
	}
}
