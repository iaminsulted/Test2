using System;

namespace MS.Internal.Interop
{
	// Token: 0x02000176 RID: 374
	internal struct STAT_CHUNK
	{
		// Token: 0x0400097F RID: 2431
		internal uint idChunk;

		// Token: 0x04000980 RID: 2432
		internal CHUNK_BREAKTYPE breakType;

		// Token: 0x04000981 RID: 2433
		internal CHUNKSTATE flags;

		// Token: 0x04000982 RID: 2434
		internal uint locale;

		// Token: 0x04000983 RID: 2435
		internal FULLPROPSPEC attribute;

		// Token: 0x04000984 RID: 2436
		internal uint idChunkSource;

		// Token: 0x04000985 RID: 2437
		internal uint cwcStartSource;

		// Token: 0x04000986 RID: 2438
		internal uint cwcLenSource;
	}
}
