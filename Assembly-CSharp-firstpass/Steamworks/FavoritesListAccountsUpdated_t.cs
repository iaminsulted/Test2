using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F1 RID: 241
	[CallbackIdentity(516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListAccountsUpdated_t
	{
		// Token: 0x0400041C RID: 1052
		public const int k_iCallback = 516;

		// Token: 0x0400041D RID: 1053
		public EResult m_eResult;
	}
}
