using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A2 RID: 162
	[CallbackIdentity(3901)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAppInstalled_t
	{
		// Token: 0x040002E9 RID: 745
		public const int k_iCallback = 3901;

		// Token: 0x040002EA RID: 746
		public AppId_t m_nAppID;
	}
}
