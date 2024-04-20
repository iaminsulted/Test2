using System;

namespace Steamworks
{
	// Token: 0x02000198 RID: 408
	[Flags]
	public enum EMarketingMessageFlags
	{
		// Token: 0x04000967 RID: 2407
		k_EMarketingMessageFlagsNone = 0,
		// Token: 0x04000968 RID: 2408
		k_EMarketingMessageFlagsHighPriority = 1,
		// Token: 0x04000969 RID: 2409
		k_EMarketingMessageFlagsPlatformWindows = 2,
		// Token: 0x0400096A RID: 2410
		k_EMarketingMessageFlagsPlatformMac = 4,
		// Token: 0x0400096B RID: 2411
		k_EMarketingMessageFlagsPlatformLinux = 8,
		// Token: 0x0400096C RID: 2412
		k_EMarketingMessageFlagsPlatformRestrictions = 14
	}
}
