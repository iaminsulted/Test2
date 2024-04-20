using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000124 RID: 292
	[CallbackIdentity(3402)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCRequestUGCDetailsResult_t
	{
		// Token: 0x040004D4 RID: 1236
		public const int k_iCallback = 3402;

		// Token: 0x040004D5 RID: 1237
		public SteamUGCDetails_t m_details;

		// Token: 0x040004D6 RID: 1238
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
