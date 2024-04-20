using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x0200029B RID: 667
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 8)]
	internal struct THUMBBUTTON
	{
		// Token: 0x04000D8C RID: 3468
		public const int THBN_CLICKED = 6144;

		// Token: 0x04000D8D RID: 3469
		public THB dwMask;

		// Token: 0x04000D8E RID: 3470
		public uint iId;

		// Token: 0x04000D8F RID: 3471
		public uint iBitmap;

		// Token: 0x04000D90 RID: 3472
		public IntPtr hIcon;

		// Token: 0x04000D91 RID: 3473
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szTip;

		// Token: 0x04000D92 RID: 3474
		public THBF dwFlags;
	}
}
