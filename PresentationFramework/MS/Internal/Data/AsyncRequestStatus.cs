using System;

namespace MS.Internal.Data
{
	// Token: 0x02000200 RID: 512
	internal enum AsyncRequestStatus
	{
		// Token: 0x04000B55 RID: 2901
		Waiting,
		// Token: 0x04000B56 RID: 2902
		Working,
		// Token: 0x04000B57 RID: 2903
		Completed,
		// Token: 0x04000B58 RID: 2904
		Cancelled,
		// Token: 0x04000B59 RID: 2905
		Failed
	}
}
