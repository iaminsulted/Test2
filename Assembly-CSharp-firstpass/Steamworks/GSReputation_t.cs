using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C4 RID: 196
	[CallbackIdentity(209)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSReputation_t
	{
		// Token: 0x04000359 RID: 857
		public const int k_iCallback = 209;

		// Token: 0x0400035A RID: 858
		public EResult m_eResult;

		// Token: 0x0400035B RID: 859
		public uint m_unReputationScore;

		// Token: 0x0400035C RID: 860
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x0400035D RID: 861
		public uint m_unBannedIP;

		// Token: 0x0400035E RID: 862
		public ushort m_usBannedPort;

		// Token: 0x0400035F RID: 863
		public ulong m_ulBannedGameID;

		// Token: 0x04000360 RID: 864
		public uint m_unBanExpires;
	}
}
