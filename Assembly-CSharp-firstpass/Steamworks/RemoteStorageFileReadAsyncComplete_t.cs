using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000120 RID: 288
	[CallbackIdentity(1332)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileReadAsyncComplete_t
	{
		// Token: 0x040004C5 RID: 1221
		public const int k_iCallback = 1332;

		// Token: 0x040004C6 RID: 1222
		public SteamAPICall_t m_hFileReadAsync;

		// Token: 0x040004C7 RID: 1223
		public EResult m_eResult;

		// Token: 0x040004C8 RID: 1224
		public uint m_nOffset;

		// Token: 0x040004C9 RID: 1225
		public uint m_cubRead;
	}
}
