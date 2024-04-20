using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AF RID: 431
	[StructLayout(LayoutKind.Sequential)]
	internal class CCallbackBase
	{
		// Token: 0x04000A35 RID: 2613
		public const byte k_ECallbackFlagsRegistered = 1;

		// Token: 0x04000A36 RID: 2614
		public const byte k_ECallbackFlagsGameServer = 2;

		// Token: 0x04000A37 RID: 2615
		public IntPtr m_vfptr;

		// Token: 0x04000A38 RID: 2616
		public byte m_nCallbackFlags;

		// Token: 0x04000A39 RID: 2617
		public int m_iCallback;
	}
}
