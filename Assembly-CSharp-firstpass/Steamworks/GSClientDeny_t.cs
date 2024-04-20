using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BE RID: 190
	[CallbackIdentity(202)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientDeny_t
	{
		// Token: 0x04000342 RID: 834
		public const int k_iCallback = 202;

		// Token: 0x04000343 RID: 835
		public CSteamID m_SteamID;

		// Token: 0x04000344 RID: 836
		public EDenyReason m_eDenyReason;

		// Token: 0x04000345 RID: 837
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchOptionalText;
	}
}
