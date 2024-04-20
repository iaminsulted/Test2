using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B9 RID: 185
	[CallbackIdentity(346)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsEnumerateFollowingList_t
	{
		// Token: 0x04000333 RID: 819
		public const int k_iCallback = 346;

		// Token: 0x04000334 RID: 820
		public EResult m_eResult;

		// Token: 0x04000335 RID: 821
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public CSteamID[] m_rgSteamID;

		// Token: 0x04000336 RID: 822
		public int m_nResultsReturned;

		// Token: 0x04000337 RID: 823
		public int m_nTotalResultCount;
	}
}
