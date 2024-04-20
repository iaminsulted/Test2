using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200014E RID: 334
	[CallbackIdentity(714)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GamepadTextInputDismissed_t
	{
		// Token: 0x0400055C RID: 1372
		public const int k_iCallback = 714;

		// Token: 0x0400055D RID: 1373
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSubmitted;

		// Token: 0x0400055E RID: 1374
		public uint m_unSubmittedText;
	}
}
