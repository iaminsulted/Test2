using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200014A RID: 330
	[CallbackIdentity(702)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LowBatteryPower_t
	{
		// Token: 0x04000553 RID: 1363
		public const int k_iCallback = 702;

		// Token: 0x04000554 RID: 1364
		public byte m_nMinutesBatteryLeft;
	}
}
