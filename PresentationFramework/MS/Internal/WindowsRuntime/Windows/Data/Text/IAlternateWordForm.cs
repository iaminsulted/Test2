using System;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x02000311 RID: 785
	[Guid("47396C1E-51B9-4207-9146-248E636A1D1D")]
	[WindowsRuntimeType]
	internal interface IAlternateWordForm
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001D2D RID: 7469
		string AlternateText { get; }

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001D2E RID: 7470
		AlternateNormalizationFormat NormalizationFormat { get; }

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001D2F RID: 7471
		TextSegment SourceTextSegment { get; }
	}
}
