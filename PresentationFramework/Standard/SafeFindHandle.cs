using System;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x0200003E RID: 62
	internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000053 RID: 83 RVA: 0x000F787A File Offset: 0x000F687A
		private SafeFindHandle() : base(true)
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000F7883 File Offset: 0x000F6883
		protected override bool ReleaseHandle()
		{
			return NativeMethods.FindClose(this.handle);
		}
	}
}
