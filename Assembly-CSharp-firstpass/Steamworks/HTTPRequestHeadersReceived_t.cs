using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E1 RID: 225
	[CallbackIdentity(2102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestHeadersReceived_t
	{
		// Token: 0x040003DC RID: 988
		public const int k_iCallback = 2102;

		// Token: 0x040003DD RID: 989
		public HTTPRequestHandle m_hRequest;

		// Token: 0x040003DE RID: 990
		public ulong m_ulContextValue;
	}
}
