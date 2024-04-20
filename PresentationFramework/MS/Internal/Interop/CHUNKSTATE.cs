using System;

namespace MS.Internal.Interop
{
	// Token: 0x02000175 RID: 373
	[Flags]
	internal enum CHUNKSTATE
	{
		// Token: 0x0400097C RID: 2428
		CHUNK_TEXT = 1,
		// Token: 0x0400097D RID: 2429
		CHUNK_VALUE = 2,
		// Token: 0x0400097E RID: 2430
		CHUNK_FILTER_OWNED_VALUE = 4
	}
}
