using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F3 RID: 243
	[CallbackIdentity(4002)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct VolumeHasChanged_t
	{
		// Token: 0x0400041F RID: 1055
		public const int k_iCallback = 4002;

		// Token: 0x04000420 RID: 1056
		public float m_flNewVolume;
	}
}
