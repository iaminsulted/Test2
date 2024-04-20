using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E6 RID: 230
	[CallbackIdentity(4703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryEligiblePromoItemDefIDs_t
	{
		// Token: 0x040003EA RID: 1002
		public const int k_iCallback = 4703;

		// Token: 0x040003EB RID: 1003
		public EResult m_result;

		// Token: 0x040003EC RID: 1004
		public CSteamID m_steamID;

		// Token: 0x040003ED RID: 1005
		public int m_numEligiblePromoItemDefs;

		// Token: 0x040003EE RID: 1006
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
