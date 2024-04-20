using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A6 RID: 422
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionState_t
	{
		// Token: 0x040009F6 RID: 2550
		public byte m_bConnectionActive;

		// Token: 0x040009F7 RID: 2551
		public byte m_bConnecting;

		// Token: 0x040009F8 RID: 2552
		public byte m_eP2PSessionError;

		// Token: 0x040009F9 RID: 2553
		public byte m_bUsingRelay;

		// Token: 0x040009FA RID: 2554
		public int m_nBytesQueuedForSend;

		// Token: 0x040009FB RID: 2555
		public int m_nPacketsQueuedForSend;

		// Token: 0x040009FC RID: 2556
		public uint m_nRemoteIP;

		// Token: 0x040009FD RID: 2557
		public ushort m_nRemotePort;
	}
}
