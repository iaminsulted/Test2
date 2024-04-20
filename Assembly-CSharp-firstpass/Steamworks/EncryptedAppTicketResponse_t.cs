using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000139 RID: 313
	[CallbackIdentity(154)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct EncryptedAppTicketResponse_t
	{
		// Token: 0x0400051B RID: 1307
		public const int k_iCallback = 154;

		// Token: 0x0400051C RID: 1308
		public EResult m_eResult;
	}
}
