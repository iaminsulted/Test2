using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BB RID: 187
	[CallbackIdentity(1701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GCMessageAvailable_t
	{
		// Token: 0x0400033C RID: 828
		public const int k_iCallback = 1701;

		// Token: 0x0400033D RID: 829
		public uint m_nMessageSize;
	}
}
