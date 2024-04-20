using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C9 RID: 201
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSStatsUnloaded_t
	{
		// Token: 0x0400036F RID: 879
		public const int k_iCallback = 1108;

		// Token: 0x04000370 RID: 880
		public CSteamID m_steamIDUser;
	}
}
