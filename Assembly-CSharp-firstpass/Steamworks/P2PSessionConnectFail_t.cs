using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000103 RID: 259
	[CallbackIdentity(1203)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct P2PSessionConnectFail_t
	{
		// Token: 0x04000437 RID: 1079
		public const int k_iCallback = 1203;

		// Token: 0x04000438 RID: 1080
		public CSteamID m_steamIDRemote;

		// Token: 0x04000439 RID: 1081
		public byte m_eP2PSessionError;
	}
}
