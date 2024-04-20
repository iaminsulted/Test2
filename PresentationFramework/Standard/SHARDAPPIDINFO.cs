using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200004E RID: 78
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal class SHARDAPPIDINFO
	{
		// Token: 0x04000450 RID: 1104
		[MarshalAs(UnmanagedType.Interface)]
		private object psi;

		// Token: 0x04000451 RID: 1105
		[MarshalAs(UnmanagedType.LPWStr)]
		private string pszAppID;
	}
}
