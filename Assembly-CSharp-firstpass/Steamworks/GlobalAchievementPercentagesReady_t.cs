using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000146 RID: 326
	[CallbackIdentity(1110)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalAchievementPercentagesReady_t
	{
		// Token: 0x04000549 RID: 1353
		public const int k_iCallback = 1110;

		// Token: 0x0400054A RID: 1354
		public ulong m_nGameID;

		// Token: 0x0400054B RID: 1355
		public EResult m_eResult;
	}
}
