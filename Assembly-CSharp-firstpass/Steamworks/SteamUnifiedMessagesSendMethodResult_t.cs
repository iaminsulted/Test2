using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000130 RID: 304
	[CallbackIdentity(2501)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUnifiedMessagesSendMethodResult_t
	{
		// Token: 0x040004FF RID: 1279
		public const int k_iCallback = 2501;

		// Token: 0x04000500 RID: 1280
		public ClientUnifiedMessageHandle m_hHandle;

		// Token: 0x04000501 RID: 1281
		public ulong m_unContext;

		// Token: 0x04000502 RID: 1282
		public EResult m_eResult;

		// Token: 0x04000503 RID: 1283
		public uint m_unResponseSize;
	}
}
