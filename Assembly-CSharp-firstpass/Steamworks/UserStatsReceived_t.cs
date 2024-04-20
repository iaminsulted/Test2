using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200013D RID: 317
	[CallbackIdentity(1101)]
	[StructLayout(LayoutKind.Explicit, Pack = 8)]
	public struct UserStatsReceived_t
	{
		// Token: 0x04000524 RID: 1316
		public const int k_iCallback = 1101;

		// Token: 0x04000525 RID: 1317
		[FieldOffset(0)]
		public ulong m_nGameID;

		// Token: 0x04000526 RID: 1318
		[FieldOffset(8)]
		public EResult m_eResult;

		// Token: 0x04000527 RID: 1319
		[FieldOffset(12)]
		public CSteamID m_steamIDUser;
	}
}
