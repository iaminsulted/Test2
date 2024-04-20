using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000FD RID: 253
	[CallbackIdentity(4110)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsLooped_t
	{
		// Token: 0x0400042B RID: 1067
		public const int k_iCallback = 4110;

		// Token: 0x0400042C RID: 1068
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLooped;
	}
}
