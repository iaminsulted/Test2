using System;

namespace Steamworks
{
	// Token: 0x0200015C RID: 348
	[Flags]
	public enum EFriendFlags
	{
		// Token: 0x04000703 RID: 1795
		k_EFriendFlagNone = 0,
		// Token: 0x04000704 RID: 1796
		k_EFriendFlagBlocked = 1,
		// Token: 0x04000705 RID: 1797
		k_EFriendFlagFriendshipRequested = 2,
		// Token: 0x04000706 RID: 1798
		k_EFriendFlagImmediate = 4,
		// Token: 0x04000707 RID: 1799
		k_EFriendFlagClanMember = 8,
		// Token: 0x04000708 RID: 1800
		k_EFriendFlagOnGameServer = 16,
		// Token: 0x04000709 RID: 1801
		k_EFriendFlagRequestingFriendship = 128,
		// Token: 0x0400070A RID: 1802
		k_EFriendFlagRequestingInfo = 256,
		// Token: 0x0400070B RID: 1803
		k_EFriendFlagIgnored = 512,
		// Token: 0x0400070C RID: 1804
		k_EFriendFlagIgnoredFriend = 1024,
		// Token: 0x0400070D RID: 1805
		k_EFriendFlagChatMember = 4096,
		// Token: 0x0400070E RID: 1806
		k_EFriendFlagAll = 65535
	}
}
