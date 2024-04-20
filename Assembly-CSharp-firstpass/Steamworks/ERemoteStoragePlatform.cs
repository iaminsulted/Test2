using System;

namespace Steamworks
{
	// Token: 0x0200016E RID: 366
	[Flags]
	public enum ERemoteStoragePlatform
	{
		// Token: 0x040007A2 RID: 1954
		k_ERemoteStoragePlatformNone = 0,
		// Token: 0x040007A3 RID: 1955
		k_ERemoteStoragePlatformWindows = 1,
		// Token: 0x040007A4 RID: 1956
		k_ERemoteStoragePlatformOSX = 2,
		// Token: 0x040007A5 RID: 1957
		k_ERemoteStoragePlatformPS3 = 4,
		// Token: 0x040007A6 RID: 1958
		k_ERemoteStoragePlatformLinux = 8,
		// Token: 0x040007A7 RID: 1959
		k_ERemoteStoragePlatformReserved2 = 16,
		// Token: 0x040007A8 RID: 1960
		k_ERemoteStoragePlatformAll = -1
	}
}
