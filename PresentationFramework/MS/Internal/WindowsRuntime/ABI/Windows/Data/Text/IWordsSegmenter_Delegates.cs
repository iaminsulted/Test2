using System;
using System.ComponentModel;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x020002FE RID: 766
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static class IWordsSegmenter_Delegates
	{
		// Token: 0x02000A53 RID: 2643
		// (Invoke) Token: 0x060085E2 RID: 34274
		public delegate int GetTokenAt_1(IntPtr thisPtr, IntPtr text, uint startIndex, out IntPtr result);

		// Token: 0x02000A54 RID: 2644
		// (Invoke) Token: 0x060085E6 RID: 34278
		public delegate int GetTokens_2(IntPtr thisPtr, IntPtr text, out IntPtr result);

		// Token: 0x02000A55 RID: 2645
		// (Invoke) Token: 0x060085EA RID: 34282
		public delegate int Tokenize_3(IntPtr thisPtr, IntPtr text, uint startIndex, IntPtr handler);
	}
}
