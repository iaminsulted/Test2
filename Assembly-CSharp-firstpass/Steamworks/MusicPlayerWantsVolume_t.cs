using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000FE RID: 254
	[CallbackIdentity(4011)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsVolume_t
	{
		// Token: 0x0400042D RID: 1069
		public const int k_iCallback = 4011;

		// Token: 0x0400042E RID: 1070
		public float m_flNewVolume;
	}
}
