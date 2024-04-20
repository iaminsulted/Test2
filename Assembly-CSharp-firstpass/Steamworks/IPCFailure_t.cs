using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000135 RID: 309
	[CallbackIdentity(117)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct IPCFailure_t
	{
		// Token: 0x04000510 RID: 1296
		public const int k_iCallback = 117;

		// Token: 0x04000511 RID: 1297
		public byte m_eFailureType;
	}
}
