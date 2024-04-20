using System;

namespace Steamworks
{
	// Token: 0x02000168 RID: 360
	[Flags]
	public enum EChatMemberStateChange
	{
		// Token: 0x0400077B RID: 1915
		k_EChatMemberStateChangeEntered = 1,
		// Token: 0x0400077C RID: 1916
		k_EChatMemberStateChangeLeft = 2,
		// Token: 0x0400077D RID: 1917
		k_EChatMemberStateChangeDisconnected = 4,
		// Token: 0x0400077E RID: 1918
		k_EChatMemberStateChangeKicked = 8,
		// Token: 0x0400077F RID: 1919
		k_EChatMemberStateChangeBanned = 16
	}
}
