using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200014B RID: 331
	[CallbackIdentity(703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAPICallCompleted_t
	{
		// Token: 0x04000555 RID: 1365
		public const int k_iCallback = 703;

		// Token: 0x04000556 RID: 1366
		public SteamAPICall_t m_hAsyncCall;

		// Token: 0x04000557 RID: 1367
		public int m_iCallback;

		// Token: 0x04000558 RID: 1368
		public uint m_cubParam;
	}
}
