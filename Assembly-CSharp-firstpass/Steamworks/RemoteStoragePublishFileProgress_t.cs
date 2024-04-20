using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200011D RID: 285
	[CallbackIdentity(1329)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileProgress_t
	{
		// Token: 0x040004BC RID: 1212
		public const int k_iCallback = 1329;

		// Token: 0x040004BD RID: 1213
		public double m_dPercentFile;

		// Token: 0x040004BE RID: 1214
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPreview;
	}
}
