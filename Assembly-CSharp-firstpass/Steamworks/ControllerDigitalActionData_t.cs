using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A1 RID: 417
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ControllerDigitalActionData_t
	{
		// Token: 0x040009DF RID: 2527
		public byte bState;

		// Token: 0x040009E0 RID: 2528
		public byte bActive;
	}
}
