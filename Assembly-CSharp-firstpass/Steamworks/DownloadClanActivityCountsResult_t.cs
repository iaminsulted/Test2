using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B4 RID: 180
	[CallbackIdentity(341)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DownloadClanActivityCountsResult_t
	{
		// Token: 0x04000323 RID: 803
		public const int k_iCallback = 341;

		// Token: 0x04000324 RID: 804
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSuccess;
	}
}
