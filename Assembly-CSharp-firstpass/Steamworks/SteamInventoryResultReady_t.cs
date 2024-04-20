using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E3 RID: 227
	[CallbackIdentity(4700)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryResultReady_t
	{
		// Token: 0x040003E4 RID: 996
		public const int k_iCallback = 4700;

		// Token: 0x040003E5 RID: 997
		public SteamInventoryResult_t m_handle;

		// Token: 0x040003E6 RID: 998
		public EResult m_result;
	}
}
