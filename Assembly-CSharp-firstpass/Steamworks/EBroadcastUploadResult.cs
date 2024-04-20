using System;

namespace Steamworks
{
	// Token: 0x0200019A RID: 410
	public enum EBroadcastUploadResult
	{
		// Token: 0x04000973 RID: 2419
		k_EBroadcastUploadResultNone,
		// Token: 0x04000974 RID: 2420
		k_EBroadcastUploadResultOK,
		// Token: 0x04000975 RID: 2421
		k_EBroadcastUploadResultInitFailed,
		// Token: 0x04000976 RID: 2422
		k_EBroadcastUploadResultFrameFailed,
		// Token: 0x04000977 RID: 2423
		k_EBroadcastUploadResultTimeout,
		// Token: 0x04000978 RID: 2424
		k_EBroadcastUploadResultBandwidthExceeded,
		// Token: 0x04000979 RID: 2425
		k_EBroadcastUploadResultLowFPS,
		// Token: 0x0400097A RID: 2426
		k_EBroadcastUploadResultMissingKeyFrames,
		// Token: 0x0400097B RID: 2427
		k_EBroadcastUploadResultNoConnection,
		// Token: 0x0400097C RID: 2428
		k_EBroadcastUploadResultRelayFailed,
		// Token: 0x0400097D RID: 2429
		k_EBroadcastUploadResultSettingsChanged,
		// Token: 0x0400097E RID: 2430
		k_EBroadcastUploadResultMissingAudio,
		// Token: 0x0400097F RID: 2431
		k_EBroadcastUploadResultTooFarBehind,
		// Token: 0x04000980 RID: 2432
		k_EBroadcastUploadResultTranscodeBehind
	}
}
