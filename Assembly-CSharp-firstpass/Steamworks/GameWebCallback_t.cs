using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200013B RID: 315
	[CallbackIdentity(164)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameWebCallback_t
	{
		// Token: 0x04000520 RID: 1312
		public const int k_iCallback = 164;

		// Token: 0x04000521 RID: 1313
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szURL;
	}
}
