using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A9 RID: 425
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CallbackMsg_t
	{
		// Token: 0x04000A1A RID: 2586
		public int m_hSteamUser;

		// Token: 0x04000A1B RID: 2587
		public int m_iCallback;

		// Token: 0x04000A1C RID: 2588
		public IntPtr m_pubParam;

		// Token: 0x04000A1D RID: 2589
		public int m_cubParam;
	}
}
