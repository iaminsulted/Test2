using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A5 RID: 421
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamItemDetails_t
	{
		// Token: 0x040009F2 RID: 2546
		public SteamItemInstanceID_t m_itemId;

		// Token: 0x040009F3 RID: 2547
		public SteamItemDef_t m_iDefinition;

		// Token: 0x040009F4 RID: 2548
		public ushort m_unQuantity;

		// Token: 0x040009F5 RID: 2549
		public ushort m_unFlags;
	}
}
