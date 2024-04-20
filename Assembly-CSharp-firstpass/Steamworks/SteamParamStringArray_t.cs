using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A7 RID: 423
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamParamStringArray_t
	{
		// Token: 0x040009FE RID: 2558
		public IntPtr m_ppStrings;

		// Token: 0x040009FF RID: 2559
		public int m_nNumStrings;
	}
}
