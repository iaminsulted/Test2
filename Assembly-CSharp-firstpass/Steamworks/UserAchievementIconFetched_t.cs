using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000145 RID: 325
	[CallbackIdentity(1109)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementIconFetched_t
	{
		// Token: 0x04000544 RID: 1348
		public const int k_iCallback = 1109;

		// Token: 0x04000545 RID: 1349
		public CGameID m_nGameID;

		// Token: 0x04000546 RID: 1350
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchAchievementName;

		// Token: 0x04000547 RID: 1351
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAchieved;

		// Token: 0x04000548 RID: 1352
		public int m_nIconHandle;
	}
}
