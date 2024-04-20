using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A8 RID: 168
	[CallbackIdentity(1023)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FileDetailsResult_t
	{
		// Token: 0x040002F8 RID: 760
		public const int k_iCallback = 1023;

		// Token: 0x040002F9 RID: 761
		public EResult m_eResult;

		// Token: 0x040002FA RID: 762
		public ulong m_ulFileSize;

		// Token: 0x040002FB RID: 763
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public byte[] m_FileSHA;

		// Token: 0x040002FC RID: 764
		public uint m_unFlags;
	}
}
