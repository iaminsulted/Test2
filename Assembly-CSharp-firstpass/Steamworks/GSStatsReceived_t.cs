using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C7 RID: 199
	[CallbackIdentity(1800)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsReceived_t
	{
		// Token: 0x04000369 RID: 873
		public const int k_iCallback = 1800;

		// Token: 0x0400036A RID: 874
		public EResult m_eResult;

		// Token: 0x0400036B RID: 875
		public CSteamID m_steamIDUser;
	}
}
