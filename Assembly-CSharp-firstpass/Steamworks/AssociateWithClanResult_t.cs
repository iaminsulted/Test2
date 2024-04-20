using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C5 RID: 197
	[CallbackIdentity(210)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AssociateWithClanResult_t
	{
		// Token: 0x04000361 RID: 865
		public const int k_iCallback = 210;

		// Token: 0x04000362 RID: 866
		public EResult m_eResult;
	}
}
