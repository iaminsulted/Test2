using System;

namespace Steamworks
{
	// Token: 0x02000193 RID: 403
	[Flags]
	public enum EAppType
	{
		// Token: 0x0400092E RID: 2350
		k_EAppType_Invalid = 0,
		// Token: 0x0400092F RID: 2351
		k_EAppType_Game = 1,
		// Token: 0x04000930 RID: 2352
		k_EAppType_Application = 2,
		// Token: 0x04000931 RID: 2353
		k_EAppType_Tool = 4,
		// Token: 0x04000932 RID: 2354
		k_EAppType_Demo = 8,
		// Token: 0x04000933 RID: 2355
		k_EAppType_Media_DEPRECATED = 16,
		// Token: 0x04000934 RID: 2356
		k_EAppType_DLC = 32,
		// Token: 0x04000935 RID: 2357
		k_EAppType_Guide = 64,
		// Token: 0x04000936 RID: 2358
		k_EAppType_Driver = 128,
		// Token: 0x04000937 RID: 2359
		k_EAppType_Config = 256,
		// Token: 0x04000938 RID: 2360
		k_EAppType_Hardware = 512,
		// Token: 0x04000939 RID: 2361
		k_EAppType_Franchise = 1024,
		// Token: 0x0400093A RID: 2362
		k_EAppType_Video = 2048,
		// Token: 0x0400093B RID: 2363
		k_EAppType_Plugin = 4096,
		// Token: 0x0400093C RID: 2364
		k_EAppType_Music = 8192,
		// Token: 0x0400093D RID: 2365
		k_EAppType_Series = 16384,
		// Token: 0x0400093E RID: 2366
		k_EAppType_Shortcut = 1073741824,
		// Token: 0x0400093F RID: 2367
		k_EAppType_DepotOnly = -2147483647
	}
}
