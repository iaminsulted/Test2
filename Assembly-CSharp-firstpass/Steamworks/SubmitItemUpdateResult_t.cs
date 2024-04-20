using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000126 RID: 294
	[CallbackIdentity(3404)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SubmitItemUpdateResult_t
	{
		// Token: 0x040004DB RID: 1243
		public const int k_iCallback = 3404;

		// Token: 0x040004DC RID: 1244
		public EResult m_eResult;

		// Token: 0x040004DD RID: 1245
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
