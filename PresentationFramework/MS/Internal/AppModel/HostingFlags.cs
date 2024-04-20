using System;

namespace MS.Internal.AppModel
{
	// Token: 0x02000272 RID: 626
	[Flags]
	internal enum HostingFlags
	{
		// Token: 0x04000D0A RID: 3338
		hfHostedInIE = 1,
		// Token: 0x04000D0B RID: 3339
		hfHostedInWebOC = 2,
		// Token: 0x04000D0C RID: 3340
		hfHostedInIEorWebOC = 3,
		// Token: 0x04000D0D RID: 3341
		hfHostedInMozilla = 4,
		// Token: 0x04000D0E RID: 3342
		hfHostedInFrame = 8,
		// Token: 0x04000D0F RID: 3343
		hfIsBrowserLowIntegrityProcess = 16,
		// Token: 0x04000D10 RID: 3344
		hfInDebugMode = 32
	}
}
