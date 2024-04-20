using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000102 RID: 258
	[CallbackIdentity(1202)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionRequest_t
	{
		// Token: 0x04000435 RID: 1077
		public const int k_iCallback = 1202;

		// Token: 0x04000436 RID: 1078
		public CSteamID m_steamIDRemote;
	}
}
