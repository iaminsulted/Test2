using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E4 RID: 228
	[CallbackIdentity(4701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryFullUpdate_t
	{
		// Token: 0x040003E7 RID: 999
		public const int k_iCallback = 4701;

		// Token: 0x040003E8 RID: 1000
		public SteamInventoryResult_t m_handle;
	}
}
