using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000137 RID: 311
	[CallbackIdentity(143)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ValidateAuthTicketResponse_t
	{
		// Token: 0x04000513 RID: 1299
		public const int k_iCallback = 143;

		// Token: 0x04000514 RID: 1300
		public CSteamID m_SteamID;

		// Token: 0x04000515 RID: 1301
		public EAuthSessionResponse m_eAuthSessionResponse;

		// Token: 0x04000516 RID: 1302
		public CSteamID m_OwnerSteamID;
	}
}
