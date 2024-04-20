using System;

namespace MS.Internal.Interop
{
	// Token: 0x0200016F RID: 367
	[Flags]
	internal enum STGM_FLAGS
	{
		// Token: 0x04000946 RID: 2374
		CREATE = 4096,
		// Token: 0x04000947 RID: 2375
		MODE = 4096,
		// Token: 0x04000948 RID: 2376
		READ = 0,
		// Token: 0x04000949 RID: 2377
		WRITE = 1,
		// Token: 0x0400094A RID: 2378
		READWRITE = 2,
		// Token: 0x0400094B RID: 2379
		ACCESS = 3,
		// Token: 0x0400094C RID: 2380
		SHARE_DENY_NONE = 64,
		// Token: 0x0400094D RID: 2381
		SHARE_DENY_READ = 48,
		// Token: 0x0400094E RID: 2382
		SHARE_DENY_WRITE = 32,
		// Token: 0x0400094F RID: 2383
		SHARE_EXCLUSIVE = 16,
		// Token: 0x04000950 RID: 2384
		SHARING = 112
	}
}
