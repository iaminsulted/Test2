using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A5 RID: 165
	[CallbackIdentity(1008)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RegisterActivationCodeResponse_t
	{
		// Token: 0x040002EF RID: 751
		public const int k_iCallback = 1008;

		// Token: 0x040002F0 RID: 752
		public ERegisterActivationCodeResult m_eResult;

		// Token: 0x040002F1 RID: 753
		public uint m_unPackageRegistered;
	}
}
