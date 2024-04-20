using System;

namespace Steamworks
{
	// Token: 0x0200016C RID: 364
	public enum ESNetSocketState
	{
		// Token: 0x04000792 RID: 1938
		k_ESNetSocketStateInvalid,
		// Token: 0x04000793 RID: 1939
		k_ESNetSocketStateConnected,
		// Token: 0x04000794 RID: 1940
		k_ESNetSocketStateInitiated = 10,
		// Token: 0x04000795 RID: 1941
		k_ESNetSocketStateLocalCandidatesFound,
		// Token: 0x04000796 RID: 1942
		k_ESNetSocketStateReceivedRemoteCandidates,
		// Token: 0x04000797 RID: 1943
		k_ESNetSocketStateChallengeHandshake = 15,
		// Token: 0x04000798 RID: 1944
		k_ESNetSocketStateDisconnecting = 21,
		// Token: 0x04000799 RID: 1945
		k_ESNetSocketStateLocalDisconnect,
		// Token: 0x0400079A RID: 1946
		k_ESNetSocketStateTimeoutDuringConnect,
		// Token: 0x0400079B RID: 1947
		k_ESNetSocketStateRemoteEndDisconnected,
		// Token: 0x0400079C RID: 1948
		k_ESNetSocketStateConnectionBroken
	}
}
