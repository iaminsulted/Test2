using System;

namespace System.Windows.Documents
{
	// Token: 0x020005EE RID: 1518
	[Flags]
	internal enum ElementEdge : byte
	{
		// Token: 0x040026D3 RID: 9939
		BeforeStart = 1,
		// Token: 0x040026D4 RID: 9940
		AfterStart = 2,
		// Token: 0x040026D5 RID: 9941
		BeforeEnd = 4,
		// Token: 0x040026D6 RID: 9942
		AfterEnd = 8
	}
}
