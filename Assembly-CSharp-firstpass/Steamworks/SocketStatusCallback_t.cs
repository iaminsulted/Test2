using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000104 RID: 260
	[CallbackIdentity(1201)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SocketStatusCallback_t
	{
		// Token: 0x0400043A RID: 1082
		public const int k_iCallback = 1201;

		// Token: 0x0400043B RID: 1083
		public SNetSocket_t m_hSocket;

		// Token: 0x0400043C RID: 1084
		public SNetListenSocket_t m_hListenSocket;

		// Token: 0x0400043D RID: 1085
		public CSteamID m_steamIDRemote;

		// Token: 0x0400043E RID: 1086
		public int m_eSNetSocketState;
	}
}
