using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012C RID: 300
	[CallbackIdentity(3410)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StartPlaytimeTrackingResult_t
	{
		// Token: 0x040004F3 RID: 1267
		public const int k_iCallback = 3410;

		// Token: 0x040004F4 RID: 1268
		public EResult m_eResult;
	}
}
