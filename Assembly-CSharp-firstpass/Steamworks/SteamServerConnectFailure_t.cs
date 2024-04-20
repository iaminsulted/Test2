using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000132 RID: 306
	[CallbackIdentity(102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServerConnectFailure_t
	{
		// Token: 0x04000505 RID: 1285
		public const int k_iCallback = 102;

		// Token: 0x04000506 RID: 1286
		public EResult m_eResult;

		// Token: 0x04000507 RID: 1287
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bStillRetrying;
	}
}
