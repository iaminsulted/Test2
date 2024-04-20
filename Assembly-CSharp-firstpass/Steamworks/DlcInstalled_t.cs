using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A4 RID: 164
	[CallbackIdentity(1005)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DlcInstalled_t
	{
		// Token: 0x040002ED RID: 749
		public const int k_iCallback = 1005;

		// Token: 0x040002EE RID: 750
		public AppId_t m_nAppID;
	}
}
