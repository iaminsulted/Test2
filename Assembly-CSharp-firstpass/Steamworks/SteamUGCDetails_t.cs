using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A8 RID: 424
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCDetails_t
	{
		// Token: 0x04000A00 RID: 2560
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000A01 RID: 2561
		public EResult m_eResult;

		// Token: 0x04000A02 RID: 2562
		public EWorkshopFileType m_eFileType;

		// Token: 0x04000A03 RID: 2563
		public AppId_t m_nCreatorAppID;

		// Token: 0x04000A04 RID: 2564
		public AppId_t m_nConsumerAppID;

		// Token: 0x04000A05 RID: 2565
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
		public string m_rgchTitle;

		// Token: 0x04000A06 RID: 2566
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8000)]
		public string m_rgchDescription;

		// Token: 0x04000A07 RID: 2567
		public ulong m_ulSteamIDOwner;

		// Token: 0x04000A08 RID: 2568
		public uint m_rtimeCreated;

		// Token: 0x04000A09 RID: 2569
		public uint m_rtimeUpdated;

		// Token: 0x04000A0A RID: 2570
		public uint m_rtimeAddedToUserList;

		// Token: 0x04000A0B RID: 2571
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x04000A0C RID: 2572
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000A0D RID: 2573
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;

		// Token: 0x04000A0E RID: 2574
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x04000A0F RID: 2575
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1025)]
		public string m_rgchTags;

		// Token: 0x04000A10 RID: 2576
		public UGCHandle_t m_hFile;

		// Token: 0x04000A11 RID: 2577
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x04000A12 RID: 2578
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		// Token: 0x04000A13 RID: 2579
		public int m_nFileSize;

		// Token: 0x04000A14 RID: 2580
		public int m_nPreviewFileSize;

		// Token: 0x04000A15 RID: 2581
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;

		// Token: 0x04000A16 RID: 2582
		public uint m_unVotesUp;

		// Token: 0x04000A17 RID: 2583
		public uint m_unVotesDown;

		// Token: 0x04000A18 RID: 2584
		public float m_flScore;

		// Token: 0x04000A19 RID: 2585
		public uint m_unNumChildren;
	}
}
