using System;

namespace Steamworks
{
	// Token: 0x0200015F RID: 351
	[Flags]
	public enum EPersonaChange
	{
		// Token: 0x0400071D RID: 1821
		k_EPersonaChangeName = 1,
		// Token: 0x0400071E RID: 1822
		k_EPersonaChangeStatus = 2,
		// Token: 0x0400071F RID: 1823
		k_EPersonaChangeComeOnline = 4,
		// Token: 0x04000720 RID: 1824
		k_EPersonaChangeGoneOffline = 8,
		// Token: 0x04000721 RID: 1825
		k_EPersonaChangeGamePlayed = 16,
		// Token: 0x04000722 RID: 1826
		k_EPersonaChangeGameServer = 32,
		// Token: 0x04000723 RID: 1827
		k_EPersonaChangeAvatar = 64,
		// Token: 0x04000724 RID: 1828
		k_EPersonaChangeJoinedSource = 128,
		// Token: 0x04000725 RID: 1829
		k_EPersonaChangeLeftSource = 256,
		// Token: 0x04000726 RID: 1830
		k_EPersonaChangeRelationshipChanged = 512,
		// Token: 0x04000727 RID: 1831
		k_EPersonaChangeNameFirstSet = 1024,
		// Token: 0x04000728 RID: 1832
		k_EPersonaChangeFacebookInfo = 2048,
		// Token: 0x04000729 RID: 1833
		k_EPersonaChangeNickname = 4096,
		// Token: 0x0400072A RID: 1834
		k_EPersonaChangeSteamLevel = 8192
	}
}
