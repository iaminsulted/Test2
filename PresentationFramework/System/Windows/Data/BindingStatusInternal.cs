using System;

namespace System.Windows.Data
{
	// Token: 0x02000447 RID: 1095
	internal enum BindingStatusInternal : byte
	{
		// Token: 0x04001C8D RID: 7309
		Unattached,
		// Token: 0x04001C8E RID: 7310
		Inactive,
		// Token: 0x04001C8F RID: 7311
		Active,
		// Token: 0x04001C90 RID: 7312
		Detached,
		// Token: 0x04001C91 RID: 7313
		AsyncRequestPending,
		// Token: 0x04001C92 RID: 7314
		PathError,
		// Token: 0x04001C93 RID: 7315
		UpdateTargetError,
		// Token: 0x04001C94 RID: 7316
		UpdateSourceError
	}
}
