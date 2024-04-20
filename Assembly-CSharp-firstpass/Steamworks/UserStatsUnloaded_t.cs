using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000144 RID: 324
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsUnloaded_t
	{
		// Token: 0x04000542 RID: 1346
		public const int k_iCallback = 1108;

		// Token: 0x04000543 RID: 1347
		public CSteamID m_steamIDUser;
	}
}
