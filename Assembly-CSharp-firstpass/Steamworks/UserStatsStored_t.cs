using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200013E RID: 318
	[CallbackIdentity(1102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsStored_t
	{
		// Token: 0x04000528 RID: 1320
		public const int k_iCallback = 1102;

		// Token: 0x04000529 RID: 1321
		public ulong m_nGameID;

		// Token: 0x0400052A RID: 1322
		public EResult m_eResult;
	}
}
