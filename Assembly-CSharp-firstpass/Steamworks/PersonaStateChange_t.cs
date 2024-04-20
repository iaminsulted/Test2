using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A9 RID: 169
	[CallbackIdentity(304)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct PersonaStateChange_t
	{
		// Token: 0x040002FD RID: 765
		public const int k_iCallback = 304;

		// Token: 0x040002FE RID: 766
		public ulong m_ulSteamID;

		// Token: 0x040002FF RID: 767
		public EPersonaChange m_nChangeFlags;
	}
}
