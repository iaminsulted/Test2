using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000150 RID: 336
	[CallbackIdentity(4605)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct BroadcastUploadStop_t
	{
		// Token: 0x04000560 RID: 1376
		public const int k_iCallback = 4605;

		// Token: 0x04000561 RID: 1377
		public EBroadcastUploadResult m_eResult;
	}
}
