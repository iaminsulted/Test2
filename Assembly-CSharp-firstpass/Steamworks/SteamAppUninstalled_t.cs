using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A3 RID: 163
	[CallbackIdentity(3902)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAppUninstalled_t
	{
		// Token: 0x040002EB RID: 747
		public const int k_iCallback = 3902;

		// Token: 0x040002EC RID: 748
		public AppId_t m_nAppID;
	}
}
