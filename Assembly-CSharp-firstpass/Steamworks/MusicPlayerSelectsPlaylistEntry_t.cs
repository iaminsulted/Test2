using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000100 RID: 256
	[CallbackIdentity(4013)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerSelectsPlaylistEntry_t
	{
		// Token: 0x04000431 RID: 1073
		public const int k_iCallback = 4013;

		// Token: 0x04000432 RID: 1074
		public int nID;
	}
}
