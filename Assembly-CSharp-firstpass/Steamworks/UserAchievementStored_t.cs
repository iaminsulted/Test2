using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200013F RID: 319
	[CallbackIdentity(1103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementStored_t
	{
		// Token: 0x0400052B RID: 1323
		public const int k_iCallback = 1103;

		// Token: 0x0400052C RID: 1324
		public ulong m_nGameID;

		// Token: 0x0400052D RID: 1325
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bGroupAchievement;

		// Token: 0x0400052E RID: 1326
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchAchievementName;

		// Token: 0x0400052F RID: 1327
		public uint m_nCurProgress;

		// Token: 0x04000530 RID: 1328
		public uint m_nMaxProgress;
	}
}
