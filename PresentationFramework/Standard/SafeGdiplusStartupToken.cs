using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x02000041 RID: 65
	internal sealed class SafeGdiplusStartupToken : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600005F RID: 95 RVA: 0x000F787A File Offset: 0x000F687A
		private SafeGdiplusStartupToken() : base(true)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000F7A4A File Offset: 0x000F6A4A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return NativeMethods.GdiplusShutdown(this.handle) == Status.Ok;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000F7A5C File Offset: 0x000F6A5C
		public static SafeGdiplusStartupToken Startup()
		{
			SafeGdiplusStartupToken safeGdiplusStartupToken = new SafeGdiplusStartupToken();
			IntPtr handle;
			StartupOutput startupOutput;
			if (NativeMethods.GdiplusStartup(out handle, new StartupInput(), out startupOutput) == Status.Ok)
			{
				safeGdiplusStartupToken.handle = handle;
				return safeGdiplusStartupToken;
			}
			safeGdiplusStartupToken.Dispose();
			throw new Exception("Unable to initialize GDI+");
		}
	}
}
