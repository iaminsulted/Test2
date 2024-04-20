using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200014D RID: 333
	[CallbackIdentity(705)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CheckFileSignature_t
	{
		// Token: 0x0400055A RID: 1370
		public const int k_iCallback = 705;

		// Token: 0x0400055B RID: 1371
		public ECheckFileSignature m_eCheckFileSignature;
	}
}
