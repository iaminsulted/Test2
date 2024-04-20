using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000138 RID: 312
	[CallbackIdentity(152)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MicroTxnAuthorizationResponse_t
	{
		// Token: 0x04000517 RID: 1303
		public const int k_iCallback = 152;

		// Token: 0x04000518 RID: 1304
		public uint m_unAppID;

		// Token: 0x04000519 RID: 1305
		public ulong m_ulOrderID;

		// Token: 0x0400051A RID: 1306
		public byte m_bAuthorized;
	}
}
