using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EE RID: 238
	[CallbackIdentity(510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyMatchList_t
	{
		// Token: 0x04000413 RID: 1043
		public const int k_iCallback = 510;

		// Token: 0x04000414 RID: 1044
		public uint m_nLobbiesMatching;
	}
}
