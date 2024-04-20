using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x0200029A RID: 666
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct PKEY
	{
		// Token: 0x0600190E RID: 6414 RVA: 0x00162874 File Offset: 0x00161874
		private PKEY(Guid fmtid, uint pid)
		{
			this._fmtid = fmtid;
			this._pid = pid;
		}

		// Token: 0x04000D84 RID: 3460
		private readonly Guid _fmtid;

		// Token: 0x04000D85 RID: 3461
		private readonly uint _pid;

		// Token: 0x04000D86 RID: 3462
		public static readonly PKEY Title = new PKEY(new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9"), 2U);

		// Token: 0x04000D87 RID: 3463
		public static readonly PKEY AppUserModel_ID = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5U);

		// Token: 0x04000D88 RID: 3464
		public static readonly PKEY AppUserModel_IsDestListSeparator = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 6U);

		// Token: 0x04000D89 RID: 3465
		public static readonly PKEY AppUserModel_RelaunchCommand = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 2U);

		// Token: 0x04000D8A RID: 3466
		public static readonly PKEY AppUserModel_RelaunchDisplayNameResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 4U);

		// Token: 0x04000D8B RID: 3467
		public static readonly PKEY AppUserModel_RelaunchIconResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 3U);
	}
}
