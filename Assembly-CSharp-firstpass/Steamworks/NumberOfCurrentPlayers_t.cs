using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000143 RID: 323
	[CallbackIdentity(1107)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct NumberOfCurrentPlayers_t
	{
		// Token: 0x0400053F RID: 1343
		public const int k_iCallback = 1107;

		// Token: 0x04000540 RID: 1344
		public byte m_bSuccess;

		// Token: 0x04000541 RID: 1345
		public int m_cPlayers;
	}
}
