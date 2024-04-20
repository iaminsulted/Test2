using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C2 RID: 194
	[CallbackIdentity(207)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSGameplayStats_t
	{
		// Token: 0x0400034F RID: 847
		public const int k_iCallback = 207;

		// Token: 0x04000350 RID: 848
		public EResult m_eResult;

		// Token: 0x04000351 RID: 849
		public int m_nRank;

		// Token: 0x04000352 RID: 850
		public uint m_unTotalConnects;

		// Token: 0x04000353 RID: 851
		public uint m_unTotalMinutesPlayed;
	}
}
