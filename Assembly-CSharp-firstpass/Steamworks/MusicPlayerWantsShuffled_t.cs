using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000FC RID: 252
	[CallbackIdentity(4109)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsShuffled_t
	{
		// Token: 0x04000429 RID: 1065
		public const int k_iCallback = 4109;

		// Token: 0x0400042A RID: 1066
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bShuffled;
	}
}
