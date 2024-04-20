using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000FF RID: 255
	[CallbackIdentity(4012)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerSelectsQueueEntry_t
	{
		// Token: 0x0400042F RID: 1071
		public const int k_iCallback = 4012;

		// Token: 0x04000430 RID: 1072
		public int nID;
	}
}
