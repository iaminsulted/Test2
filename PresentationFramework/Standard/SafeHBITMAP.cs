using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x02000040 RID: 64
	internal sealed class SafeHBITMAP : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000F787A File Offset: 0x000F687A
		private SafeHBITMAP() : base(true)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000F7A3D File Offset: 0x000F6A3D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return NativeMethods.DeleteObject(this.handle);
		}
	}
}
