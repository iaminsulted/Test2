using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AE RID: 174
	[CallbackIdentity(335)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ClanOfficerListResponse_t
	{
		// Token: 0x0400030D RID: 781
		public const int k_iCallback = 335;

		// Token: 0x0400030E RID: 782
		public CSteamID m_steamIDClan;

		// Token: 0x0400030F RID: 783
		public int m_cOfficers;

		// Token: 0x04000310 RID: 784
		public byte m_bSuccess;
	}
}
