using System;

namespace System.Windows.Data
{
	// Token: 0x02000446 RID: 1094
	public enum BindingStatus
	{
		// Token: 0x04001C84 RID: 7300
		Unattached,
		// Token: 0x04001C85 RID: 7301
		Inactive,
		// Token: 0x04001C86 RID: 7302
		Active,
		// Token: 0x04001C87 RID: 7303
		Detached,
		// Token: 0x04001C88 RID: 7304
		AsyncRequestPending,
		// Token: 0x04001C89 RID: 7305
		PathError,
		// Token: 0x04001C8A RID: 7306
		UpdateTargetError,
		// Token: 0x04001C8B RID: 7307
		UpdateSourceError
	}
}
