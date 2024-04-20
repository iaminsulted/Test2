using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200013C RID: 316
	[CallbackIdentity(165)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StoreAuthURLResponse_t
	{
		// Token: 0x04000522 RID: 1314
		public const int k_iCallback = 165;

		// Token: 0x04000523 RID: 1315
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string m_szURL;
	}
}
