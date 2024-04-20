using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BA RID: 442
	public static class Packsize
	{
		// Token: 0x06000D55 RID: 3413 RVA: 0x0002A774 File Offset: 0x00028974
		public static bool Test()
		{
			int num = Marshal.SizeOf(typeof(Packsize.ValvePackingSentinel_t));
			int num2 = Marshal.SizeOf(typeof(RemoteStorageEnumerateUserSubscribedFilesResult_t));
			return num == 32 && num2 == 616;
		}

		// Token: 0x04000A58 RID: 2648
		public const int value = 8;

		// Token: 0x020002D9 RID: 729
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		private struct ValvePackingSentinel_t
		{
			// Token: 0x04000DCF RID: 3535
			private uint m_u32;

			// Token: 0x04000DD0 RID: 3536
			private ulong m_u64;

			// Token: 0x04000DD1 RID: 3537
			private ushort m_u16;

			// Token: 0x04000DD2 RID: 3538
			private double m_d;
		}
	}
}
