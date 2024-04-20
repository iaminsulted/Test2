using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E0 RID: 224
	[CallbackIdentity(2101)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestCompleted_t
	{
		// Token: 0x040003D6 RID: 982
		public const int k_iCallback = 2101;

		// Token: 0x040003D7 RID: 983
		public HTTPRequestHandle m_hRequest;

		// Token: 0x040003D8 RID: 984
		public ulong m_ulContextValue;

		// Token: 0x040003D9 RID: 985
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bRequestSuccessful;

		// Token: 0x040003DA RID: 986
		public EHTTPStatusCode m_eStatusCode;

		// Token: 0x040003DB RID: 987
		public uint m_unBodySize;
	}
}
