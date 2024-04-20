using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200011F RID: 287
	[CallbackIdentity(1331)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileWriteAsyncComplete_t
	{
		// Token: 0x040004C3 RID: 1219
		public const int k_iCallback = 1331;

		// Token: 0x040004C4 RID: 1220
		public EResult m_eResult;
	}
}
