using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E2 RID: 226
	[CallbackIdentity(2103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestDataReceived_t
	{
		// Token: 0x040003DF RID: 991
		public const int k_iCallback = 2103;

		// Token: 0x040003E0 RID: 992
		public HTTPRequestHandle m_hRequest;

		// Token: 0x040003E1 RID: 993
		public ulong m_ulContextValue;

		// Token: 0x040003E2 RID: 994
		public uint m_cOffset;

		// Token: 0x040003E3 RID: 995
		public uint m_cBytesReceived;
	}
}
