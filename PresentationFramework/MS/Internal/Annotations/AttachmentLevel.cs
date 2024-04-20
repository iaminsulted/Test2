using System;

namespace MS.Internal.Annotations
{
	// Token: 0x020002BC RID: 700
	[Flags]
	internal enum AttachmentLevel
	{
		// Token: 0x04000D9F RID: 3487
		Full = 7,
		// Token: 0x04000DA0 RID: 3488
		StartPortion = 4,
		// Token: 0x04000DA1 RID: 3489
		MiddlePortion = 2,
		// Token: 0x04000DA2 RID: 3490
		EndPortion = 1,
		// Token: 0x04000DA3 RID: 3491
		Incomplete = 256,
		// Token: 0x04000DA4 RID: 3492
		Unresolved = 0
	}
}
