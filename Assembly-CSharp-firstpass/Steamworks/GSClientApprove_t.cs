using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BD RID: 189
	[CallbackIdentity(201)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientApprove_t
	{
		// Token: 0x0400033F RID: 831
		public const int k_iCallback = 201;

		// Token: 0x04000340 RID: 832
		public CSteamID m_SteamID;

		// Token: 0x04000341 RID: 833
		public CSteamID m_OwnerSteamID;
	}
}
