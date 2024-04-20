﻿using System;

namespace MS.Internal.Interop
{
	// Token: 0x02000172 RID: 370
	[Flags]
	internal enum IFILTER_INIT
	{
		// Token: 0x04000968 RID: 2408
		IFILTER_INIT_CANON_PARAGRAPHS = 1,
		// Token: 0x04000969 RID: 2409
		IFILTER_INIT_HARD_LINE_BREAKS = 2,
		// Token: 0x0400096A RID: 2410
		IFILTER_INIT_CANON_HYPHENS = 4,
		// Token: 0x0400096B RID: 2411
		IFILTER_INIT_CANON_SPACES = 8,
		// Token: 0x0400096C RID: 2412
		IFILTER_INIT_APPLY_INDEX_ATTRIBUTES = 16,
		// Token: 0x0400096D RID: 2413
		IFILTER_INIT_APPLY_OTHER_ATTRIBUTES = 32,
		// Token: 0x0400096E RID: 2414
		IFILTER_INIT_INDEXING_ONLY = 64,
		// Token: 0x0400096F RID: 2415
		IFILTER_INIT_SEARCH_LINKS = 128,
		// Token: 0x04000970 RID: 2416
		IFILTER_INIT_APPLY_CRAWL_ATTRIBUTES = 256,
		// Token: 0x04000971 RID: 2417
		IFILTER_INIT_FILTER_OWNED_VALUE_OK = 512
	}
}