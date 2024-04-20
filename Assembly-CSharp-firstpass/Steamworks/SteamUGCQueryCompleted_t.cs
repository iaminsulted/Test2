using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000123 RID: 291
	[CallbackIdentity(3401)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCQueryCompleted_t
	{
		// Token: 0x040004CE RID: 1230
		public const int k_iCallback = 3401;

		// Token: 0x040004CF RID: 1231
		public UGCQueryHandle_t m_handle;

		// Token: 0x040004D0 RID: 1232
		public EResult m_eResult;

		// Token: 0x040004D1 RID: 1233
		public uint m_unNumResultsReturned;

		// Token: 0x040004D2 RID: 1234
		public uint m_unTotalMatchingResults;

		// Token: 0x040004D3 RID: 1235
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
