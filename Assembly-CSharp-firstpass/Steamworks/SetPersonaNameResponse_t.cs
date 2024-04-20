using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BA RID: 186
	[CallbackIdentity(347)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SetPersonaNameResponse_t
	{
		// Token: 0x04000338 RID: 824
		public const int k_iCallback = 347;

		// Token: 0x04000339 RID: 825
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSuccess;

		// Token: 0x0400033A RID: 826
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLocalSuccess;

		// Token: 0x0400033B RID: 827
		public EResult m_result;
	}
}
