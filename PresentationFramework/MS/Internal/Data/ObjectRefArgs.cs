using System;

namespace MS.Internal.Data
{
	// Token: 0x02000230 RID: 560
	internal class ObjectRefArgs
	{
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x00154E96 File Offset: 0x00153E96
		// (set) Token: 0x06001561 RID: 5473 RVA: 0x00154E9E File Offset: 0x00153E9E
		internal bool IsTracing { get; set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001562 RID: 5474 RVA: 0x00154EA7 File Offset: 0x00153EA7
		// (set) Token: 0x06001563 RID: 5475 RVA: 0x00154EAF File Offset: 0x00153EAF
		internal bool ResolveNamesInTemplate { get; set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001564 RID: 5476 RVA: 0x00154EB8 File Offset: 0x00153EB8
		// (set) Token: 0x06001565 RID: 5477 RVA: 0x00154EC0 File Offset: 0x00153EC0
		internal bool NameResolvedInOuterScope { get; set; }
	}
}
