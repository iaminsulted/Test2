using System;

namespace Steamworks
{
	// Token: 0x02000184 RID: 388
	public enum ESteamAPICallFailure
	{
		// Token: 0x0400084D RID: 2125
		k_ESteamAPICallFailureNone = -1,
		// Token: 0x0400084E RID: 2126
		k_ESteamAPICallFailureSteamGone,
		// Token: 0x0400084F RID: 2127
		k_ESteamAPICallFailureNetworkFailure,
		// Token: 0x04000850 RID: 2128
		k_ESteamAPICallFailureInvalidHandle,
		// Token: 0x04000851 RID: 2129
		k_ESteamAPICallFailureMismatchedCallback
	}
}
