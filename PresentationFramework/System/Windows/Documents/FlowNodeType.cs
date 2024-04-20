using System;

namespace System.Windows.Documents
{
	// Token: 0x020005F5 RID: 1525
	internal enum FlowNodeType : byte
	{
		// Token: 0x04002714 RID: 10004
		Boundary,
		// Token: 0x04002715 RID: 10005
		Start,
		// Token: 0x04002716 RID: 10006
		Run,
		// Token: 0x04002717 RID: 10007
		End = 4,
		// Token: 0x04002718 RID: 10008
		Object = 8,
		// Token: 0x04002719 RID: 10009
		Virtual = 16,
		// Token: 0x0400271A RID: 10010
		Noop = 32
	}
}
