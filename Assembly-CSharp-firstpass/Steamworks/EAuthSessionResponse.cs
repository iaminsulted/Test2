using System;

namespace Steamworks
{
	// Token: 0x0200018E RID: 398
	public enum EAuthSessionResponse
	{
		// Token: 0x040008F7 RID: 2295
		k_EAuthSessionResponseOK,
		// Token: 0x040008F8 RID: 2296
		k_EAuthSessionResponseUserNotConnectedToSteam,
		// Token: 0x040008F9 RID: 2297
		k_EAuthSessionResponseNoLicenseOrExpired,
		// Token: 0x040008FA RID: 2298
		k_EAuthSessionResponseVACBanned,
		// Token: 0x040008FB RID: 2299
		k_EAuthSessionResponseLoggedInElseWhere,
		// Token: 0x040008FC RID: 2300
		k_EAuthSessionResponseVACCheckTimedOut,
		// Token: 0x040008FD RID: 2301
		k_EAuthSessionResponseAuthTicketCanceled,
		// Token: 0x040008FE RID: 2302
		k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed,
		// Token: 0x040008FF RID: 2303
		k_EAuthSessionResponseAuthTicketInvalid,
		// Token: 0x04000900 RID: 2304
		k_EAuthSessionResponsePublisherIssuedBan
	}
}
