using System;
using System.ComponentModel;
using MS.Internal.WindowsRuntime.Windows.Data.Text;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x020002F8 RID: 760
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static class IAlternateWordForm_Delegates
	{
		// Token: 0x02000A3C RID: 2620
		// (Invoke) Token: 0x06008577 RID: 34167
		public delegate int get_SourceTextSegment_0(IntPtr thisPtr, out TextSegment value);

		// Token: 0x02000A3D RID: 2621
		// (Invoke) Token: 0x0600857B RID: 34171
		public delegate int get_NormalizationFormat_2(IntPtr thisPtr, out AlternateNormalizationFormat value);
	}
}
