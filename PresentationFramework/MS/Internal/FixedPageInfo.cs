using System;

namespace MS.Internal
{
	// Token: 0x020000E9 RID: 233
	internal abstract class FixedPageInfo
	{
		// Token: 0x06000428 RID: 1064
		internal abstract GlyphRunInfo GlyphRunAtPosition(int position);

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000429 RID: 1065
		internal abstract int GlyphRunCount { get; }
	}
}
