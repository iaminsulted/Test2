using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AA RID: 170
	[CallbackIdentity(331)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameOverlayActivated_t
	{
		// Token: 0x04000300 RID: 768
		public const int k_iCallback = 331;

		// Token: 0x04000301 RID: 769
		public byte m_bActive;
	}
}
