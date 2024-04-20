using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000134 RID: 308
	[CallbackIdentity(113)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ClientGameServerDeny_t
	{
		// Token: 0x0400050A RID: 1290
		public const int k_iCallback = 113;

		// Token: 0x0400050B RID: 1291
		public uint m_uAppID;

		// Token: 0x0400050C RID: 1292
		public uint m_unGameServerIP;

		// Token: 0x0400050D RID: 1293
		public ushort m_usGameServerPort;

		// Token: 0x0400050E RID: 1294
		public ushort m_bSecure;

		// Token: 0x0400050F RID: 1295
		public uint m_uReason;
	}
}
