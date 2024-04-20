using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A0 RID: 416
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ControllerAnalogActionData_t
	{
		// Token: 0x040009DB RID: 2523
		public EControllerSourceMode eMode;

		// Token: 0x040009DC RID: 2524
		public float x;

		// Token: 0x040009DD RID: 2525
		public float y;

		// Token: 0x040009DE RID: 2526
		public byte bActive;
	}
}
