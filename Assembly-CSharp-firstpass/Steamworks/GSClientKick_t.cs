using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BF RID: 191
	[CallbackIdentity(203)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientKick_t
	{
		// Token: 0x04000346 RID: 838
		public const int k_iCallback = 203;

		// Token: 0x04000347 RID: 839
		public CSteamID m_SteamID;

		// Token: 0x04000348 RID: 840
		public EDenyReason m_eDenyReason;
	}
}
