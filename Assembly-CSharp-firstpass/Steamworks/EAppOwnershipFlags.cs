using System;

namespace Steamworks
{
	// Token: 0x02000192 RID: 402
	[Flags]
	public enum EAppOwnershipFlags
	{
		// Token: 0x04000919 RID: 2329
		k_EAppOwnershipFlags_None = 0,
		// Token: 0x0400091A RID: 2330
		k_EAppOwnershipFlags_OwnsLicense = 1,
		// Token: 0x0400091B RID: 2331
		k_EAppOwnershipFlags_FreeLicense = 2,
		// Token: 0x0400091C RID: 2332
		k_EAppOwnershipFlags_RegionRestricted = 4,
		// Token: 0x0400091D RID: 2333
		k_EAppOwnershipFlags_LowViolence = 8,
		// Token: 0x0400091E RID: 2334
		k_EAppOwnershipFlags_InvalidPlatform = 16,
		// Token: 0x0400091F RID: 2335
		k_EAppOwnershipFlags_SharedLicense = 32,
		// Token: 0x04000920 RID: 2336
		k_EAppOwnershipFlags_FreeWeekend = 64,
		// Token: 0x04000921 RID: 2337
		k_EAppOwnershipFlags_RetailLicense = 128,
		// Token: 0x04000922 RID: 2338
		k_EAppOwnershipFlags_LicenseLocked = 256,
		// Token: 0x04000923 RID: 2339
		k_EAppOwnershipFlags_LicensePending = 512,
		// Token: 0x04000924 RID: 2340
		k_EAppOwnershipFlags_LicenseExpired = 1024,
		// Token: 0x04000925 RID: 2341
		k_EAppOwnershipFlags_LicensePermanent = 2048,
		// Token: 0x04000926 RID: 2342
		k_EAppOwnershipFlags_LicenseRecurring = 4096,
		// Token: 0x04000927 RID: 2343
		k_EAppOwnershipFlags_LicenseCanceled = 8192,
		// Token: 0x04000928 RID: 2344
		k_EAppOwnershipFlags_AutoGrant = 16384,
		// Token: 0x04000929 RID: 2345
		k_EAppOwnershipFlags_PendingGift = 32768,
		// Token: 0x0400092A RID: 2346
		k_EAppOwnershipFlags_RentalNotActivated = 65536,
		// Token: 0x0400092B RID: 2347
		k_EAppOwnershipFlags_Rental = 131072,
		// Token: 0x0400092C RID: 2348
		k_EAppOwnershipFlags_SiteLicense = 262144
	}
}
