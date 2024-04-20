using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000101 RID: 257
	[CallbackIdentity(4114)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsPlayingRepeatStatus_t
	{
		// Token: 0x04000433 RID: 1075
		public const int k_iCallback = 4114;

		// Token: 0x04000434 RID: 1076
		public int m_nPlayingRepeatStatus;
	}
}
