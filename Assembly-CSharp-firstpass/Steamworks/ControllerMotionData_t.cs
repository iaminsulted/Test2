using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A2 RID: 418
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ControllerMotionData_t
	{
		// Token: 0x040009E1 RID: 2529
		public float rotQuatX;

		// Token: 0x040009E2 RID: 2530
		public float rotQuatY;

		// Token: 0x040009E3 RID: 2531
		public float rotQuatZ;

		// Token: 0x040009E4 RID: 2532
		public float rotQuatW;

		// Token: 0x040009E5 RID: 2533
		public float posAccelX;

		// Token: 0x040009E6 RID: 2534
		public float posAccelY;

		// Token: 0x040009E7 RID: 2535
		public float posAccelZ;

		// Token: 0x040009E8 RID: 2536
		public float rotVelX;

		// Token: 0x040009E9 RID: 2537
		public float rotVelY;

		// Token: 0x040009EA RID: 2538
		public float rotVelZ;
	}
}
