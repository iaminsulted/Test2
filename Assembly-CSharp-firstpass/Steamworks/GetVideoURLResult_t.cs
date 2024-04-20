using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000151 RID: 337
	[CallbackIdentity(4611)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetVideoURLResult_t
	{
		// Token: 0x04000562 RID: 1378
		public const int k_iCallback = 4611;

		// Token: 0x04000563 RID: 1379
		public EResult m_eResult;

		// Token: 0x04000564 RID: 1380
		public AppId_t m_unVideoAppID;

		// Token: 0x04000565 RID: 1381
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;
	}
}
