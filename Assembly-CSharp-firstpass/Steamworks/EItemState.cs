using System;

namespace Steamworks
{
	// Token: 0x0200017C RID: 380
	[Flags]
	public enum EItemState
	{
		// Token: 0x0400081B RID: 2075
		k_EItemStateNone = 0,
		// Token: 0x0400081C RID: 2076
		k_EItemStateSubscribed = 1,
		// Token: 0x0400081D RID: 2077
		k_EItemStateLegacyItem = 2,
		// Token: 0x0400081E RID: 2078
		k_EItemStateInstalled = 4,
		// Token: 0x0400081F RID: 2079
		k_EItemStateNeedsUpdate = 8,
		// Token: 0x04000820 RID: 2080
		k_EItemStateDownloading = 16,
		// Token: 0x04000821 RID: 2081
		k_EItemStateDownloadPending = 32
	}
}
