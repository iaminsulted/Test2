using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A7 RID: 167
	[CallbackIdentity(1021)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AppProofOfPurchaseKeyResponse_t
	{
		// Token: 0x040002F3 RID: 755
		public const int k_iCallback = 1021;

		// Token: 0x040002F4 RID: 756
		public EResult m_eResult;

		// Token: 0x040002F5 RID: 757
		public uint m_nAppID;

		// Token: 0x040002F6 RID: 758
		public uint m_cchKeyLength;

		// Token: 0x040002F7 RID: 759
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 240)]
		public string m_rgchKey;
	}
}
