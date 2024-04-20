using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C3 RID: 195
	[CallbackIdentity(208)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct GSClientGroupStatus_t
	{
		// Token: 0x04000354 RID: 852
		public const int k_iCallback = 208;

		// Token: 0x04000355 RID: 853
		public CSteamID m_SteamIDUser;

		// Token: 0x04000356 RID: 854
		public CSteamID m_SteamIDGroup;

		// Token: 0x04000357 RID: 855
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bMember;

		// Token: 0x04000358 RID: 856
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bOfficer;
	}
}
