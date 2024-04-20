using System;

namespace Steamworks
{
	// Token: 0x0200015D RID: 349
	public enum EUserRestriction
	{
		// Token: 0x04000710 RID: 1808
		k_nUserRestrictionNone,
		// Token: 0x04000711 RID: 1809
		k_nUserRestrictionUnknown,
		// Token: 0x04000712 RID: 1810
		k_nUserRestrictionAnyChat,
		// Token: 0x04000713 RID: 1811
		k_nUserRestrictionVoiceChat = 4,
		// Token: 0x04000714 RID: 1812
		k_nUserRestrictionGroupChat = 8,
		// Token: 0x04000715 RID: 1813
		k_nUserRestrictionRating = 16,
		// Token: 0x04000716 RID: 1814
		k_nUserRestrictionGameInvites = 32,
		// Token: 0x04000717 RID: 1815
		k_nUserRestrictionTrading = 64
	}
}
