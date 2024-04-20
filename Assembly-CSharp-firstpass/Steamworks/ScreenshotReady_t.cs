using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000121 RID: 289
	[CallbackIdentity(2301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ScreenshotReady_t
	{
		// Token: 0x040004CA RID: 1226
		public const int k_iCallback = 2301;

		// Token: 0x040004CB RID: 1227
		public ScreenshotHandle m_hLocal;

		// Token: 0x040004CC RID: 1228
		public EResult m_eResult;
	}
}
