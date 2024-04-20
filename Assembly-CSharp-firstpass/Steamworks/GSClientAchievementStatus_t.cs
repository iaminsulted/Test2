using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C0 RID: 192
	[CallbackIdentity(206)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientAchievementStatus_t
	{
		// Token: 0x04000349 RID: 841
		public const int k_iCallback = 206;

		// Token: 0x0400034A RID: 842
		public ulong m_SteamID;

		// Token: 0x0400034B RID: 843
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_pchAchievement;

		// Token: 0x0400034C RID: 844
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUnlocked;
	}
}
