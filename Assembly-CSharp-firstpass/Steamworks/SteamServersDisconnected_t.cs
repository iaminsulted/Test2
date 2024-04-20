using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000133 RID: 307
	[CallbackIdentity(103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServersDisconnected_t
	{
		// Token: 0x04000508 RID: 1288
		public const int k_iCallback = 103;

		// Token: 0x04000509 RID: 1289
		public EResult m_eResult;
	}
}
