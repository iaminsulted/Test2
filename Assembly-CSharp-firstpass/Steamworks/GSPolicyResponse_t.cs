using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C1 RID: 193
	[CallbackIdentity(115)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSPolicyResponse_t
	{
		// Token: 0x0400034D RID: 845
		public const int k_iCallback = 115;

		// Token: 0x0400034E RID: 846
		public byte m_bSecure;
	}
}
