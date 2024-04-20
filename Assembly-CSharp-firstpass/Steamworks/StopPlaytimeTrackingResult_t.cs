using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012D RID: 301
	[CallbackIdentity(3411)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StopPlaytimeTrackingResult_t
	{
		// Token: 0x040004F5 RID: 1269
		public const int k_iCallback = 3411;

		// Token: 0x040004F6 RID: 1270
		public EResult m_eResult;
	}
}
