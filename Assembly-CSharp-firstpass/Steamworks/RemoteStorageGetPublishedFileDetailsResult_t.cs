using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000112 RID: 274
	[CallbackIdentity(1318)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedFileDetailsResult_t
	{
		// Token: 0x04000477 RID: 1143
		public const int k_iCallback = 1318;

		// Token: 0x04000478 RID: 1144
		public EResult m_eResult;

		// Token: 0x04000479 RID: 1145
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400047A RID: 1146
		public AppId_t m_nCreatorAppID;

		// Token: 0x0400047B RID: 1147
		public AppId_t m_nConsumerAppID;

		// Token: 0x0400047C RID: 1148
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
		public string m_rgchTitle;

		// Token: 0x0400047D RID: 1149
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8000)]
		public string m_rgchDescription;

		// Token: 0x0400047E RID: 1150
		public UGCHandle_t m_hFile;

		// Token: 0x0400047F RID: 1151
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x04000480 RID: 1152
		public ulong m_ulSteamIDOwner;

		// Token: 0x04000481 RID: 1153
		public uint m_rtimeCreated;

		// Token: 0x04000482 RID: 1154
		public uint m_rtimeUpdated;

		// Token: 0x04000483 RID: 1155
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x04000484 RID: 1156
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000485 RID: 1157
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1025)]
		public string m_rgchTags;

		// Token: 0x04000486 RID: 1158
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x04000487 RID: 1159
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		// Token: 0x04000488 RID: 1160
		public int m_nFileSize;

		// Token: 0x04000489 RID: 1161
		public int m_nPreviewFileSize;

		// Token: 0x0400048A RID: 1162
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;

		// Token: 0x0400048B RID: 1163
		public EWorkshopFileType m_eFileType;

		// Token: 0x0400048C RID: 1164
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;
	}
}
