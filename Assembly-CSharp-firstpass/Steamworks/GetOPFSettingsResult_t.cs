using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000152 RID: 338
	[CallbackIdentity(4624)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetOPFSettingsResult_t
	{
		// Token: 0x04000566 RID: 1382
		public const int k_iCallback = 4624;

		// Token: 0x04000567 RID: 1383
		public EResult m_eResult;

		// Token: 0x04000568 RID: 1384
		public AppId_t m_unVideoAppID;
	}
}
