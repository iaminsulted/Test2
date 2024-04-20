using System;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x0200030F RID: 783
	[WindowsRuntimeType]
	internal enum AlternateNormalizationFormat
	{
		// Token: 0x04000E8A RID: 3722
		NotNormalized,
		// Token: 0x04000E8B RID: 3723
		Number,
		// Token: 0x04000E8C RID: 3724
		Currency = 3,
		// Token: 0x04000E8D RID: 3725
		Date,
		// Token: 0x04000E8E RID: 3726
		Time
	}
}
