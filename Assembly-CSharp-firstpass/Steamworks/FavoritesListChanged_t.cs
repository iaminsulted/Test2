using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E7 RID: 231
	[CallbackIdentity(502)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListChanged_t
	{
		// Token: 0x040003EF RID: 1007
		public const int k_iCallback = 502;

		// Token: 0x040003F0 RID: 1008
		public uint m_nIP;

		// Token: 0x040003F1 RID: 1009
		public uint m_nQueryPort;

		// Token: 0x040003F2 RID: 1010
		public uint m_nConnPort;

		// Token: 0x040003F3 RID: 1011
		public uint m_nAppID;

		// Token: 0x040003F4 RID: 1012
		public uint m_nFlags;

		// Token: 0x040003F5 RID: 1013
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAdd;

		// Token: 0x040003F6 RID: 1014
		public AccountID_t m_unAccountId;
	}
}
