using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C8 RID: 200
	[CallbackIdentity(1801)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsStored_t
	{
		// Token: 0x0400036C RID: 876
		public const int k_iCallback = 1801;

		// Token: 0x0400036D RID: 877
		public EResult m_eResult;

		// Token: 0x0400036E RID: 878
		public CSteamID m_steamIDUser;
	}
}
