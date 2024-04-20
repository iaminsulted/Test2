using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200013A RID: 314
	[CallbackIdentity(163)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetAuthSessionTicketResponse_t
	{
		// Token: 0x0400051D RID: 1309
		public const int k_iCallback = 163;

		// Token: 0x0400051E RID: 1310
		public HAuthTicket m_hAuthTicket;

		// Token: 0x0400051F RID: 1311
		public EResult m_eResult;
	}
}
