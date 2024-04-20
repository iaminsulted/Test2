using System;
using System.Collections.Generic;

namespace Steamworks
{
	// Token: 0x02000099 RID: 153
	public static class SteamRemoteStorage
	{
		// Token: 0x060008B2 RID: 2226 RVA: 0x000277D4 File Offset: 0x000259D4
		public static bool FileWrite(string pchFile, byte[] pvData, int cubData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileWrite(utf8StringHandle, pvData, cubData);
			}
			return result;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00027814 File Offset: 0x00025A14
		public static int FileRead(string pchFile, byte[] pvData, int cubDataToRead)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileRead(utf8StringHandle, pvData, cubDataToRead);
			}
			return result;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00027854 File Offset: 0x00025A54
		public static SteamAPICall_t FileWriteAsync(string pchFile, byte[] pvData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_FileWriteAsync(utf8StringHandle, pvData, cubData);
			}
			return result;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00027898 File Offset: 0x00025A98
		public static SteamAPICall_t FileReadAsync(string pchFile, uint nOffset, uint cubToRead)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_FileReadAsync(utf8StringHandle, nOffset, cubToRead);
			}
			return result;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000278DC File Offset: 0x00025ADC
		public static bool FileReadAsyncComplete(SteamAPICall_t hReadCall, byte[] pvBuffer, uint cubToRead)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileReadAsyncComplete(hReadCall, pvBuffer, cubToRead);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x000278EC File Offset: 0x00025AEC
		public static bool FileForget(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileForget(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0002792C File Offset: 0x00025B2C
		public static bool FileDelete(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileDelete(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002796C File Offset: 0x00025B6C
		public static SteamAPICall_t FileShare(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_FileShare(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x000279B0 File Offset: 0x00025BB0
		public static bool SetSyncPlatforms(string pchFile, ERemoteStoragePlatform eRemoteStoragePlatform)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_SetSyncPlatforms(utf8StringHandle, eRemoteStoragePlatform);
			}
			return result;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x000279F0 File Offset: 0x00025BF0
		public static UGCFileWriteStreamHandle_t FileWriteStreamOpen(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			UGCFileWriteStreamHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (UGCFileWriteStreamHandle_t)NativeMethods.ISteamRemoteStorage_FileWriteStreamOpen(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00027A34 File Offset: 0x00025C34
		public static bool FileWriteStreamWriteChunk(UGCFileWriteStreamHandle_t writeHandle, byte[] pvData, int cubData)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileWriteStreamWriteChunk(writeHandle, pvData, cubData);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00027A43 File Offset: 0x00025C43
		public static bool FileWriteStreamClose(UGCFileWriteStreamHandle_t writeHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileWriteStreamClose(writeHandle);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00027A50 File Offset: 0x00025C50
		public static bool FileWriteStreamCancel(UGCFileWriteStreamHandle_t writeHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileWriteStreamCancel(writeHandle);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00027A60 File Offset: 0x00025C60
		public static bool FileExists(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileExists(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00027AA0 File Offset: 0x00025CA0
		public static bool FilePersisted(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FilePersisted(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00027AE0 File Offset: 0x00025CE0
		public static int GetFileSize(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_GetFileSize(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00027B20 File Offset: 0x00025D20
		public static long GetFileTimestamp(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			long result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_GetFileTimestamp(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00027B60 File Offset: 0x00025D60
		public static ERemoteStoragePlatform GetSyncPlatforms(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			ERemoteStoragePlatform result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_GetSyncPlatforms(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00027BA0 File Offset: 0x00025DA0
		public static int GetFileCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetFileCount();
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00027BAC File Offset: 0x00025DAC
		public static string GetFileNameAndSize(int iFile, out int pnFileSizeInBytes)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamRemoteStorage_GetFileNameAndSize(iFile, out pnFileSizeInBytes));
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00027BBF File Offset: 0x00025DBF
		public static bool GetQuota(out ulong pnTotalBytes, out ulong puAvailableBytes)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetQuota(out pnTotalBytes, out puAvailableBytes);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00027BCD File Offset: 0x00025DCD
		public static bool IsCloudEnabledForAccount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForAccount();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00027BD9 File Offset: 0x00025DD9
		public static bool IsCloudEnabledForApp()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForApp();
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00027BE5 File Offset: 0x00025DE5
		public static void SetCloudEnabledForApp(bool bEnabled)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamRemoteStorage_SetCloudEnabledForApp(bEnabled);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00027BF2 File Offset: 0x00025DF2
		public static SteamAPICall_t UGCDownload(UGCHandle_t hContent, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UGCDownload(hContent, unPriority);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00027C05 File Offset: 0x00025E05
		public static bool GetUGCDownloadProgress(UGCHandle_t hContent, out int pnBytesDownloaded, out int pnBytesExpected)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetUGCDownloadProgress(hContent, out pnBytesDownloaded, out pnBytesExpected);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00027C14 File Offset: 0x00025E14
		public static bool GetUGCDetails(UGCHandle_t hContent, out AppId_t pnAppID, out string ppchName, out int pnFileSizeInBytes, out CSteamID pSteamIDOwner)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr nativeUtf;
			bool flag = NativeMethods.ISteamRemoteStorage_GetUGCDetails(hContent, out pnAppID, out nativeUtf, out pnFileSizeInBytes, out pSteamIDOwner);
			ppchName = (flag ? InteropHelp.PtrToStringUTF8(nativeUtf) : null);
			return flag;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00027C42 File Offset: 0x00025E42
		public static int UGCRead(UGCHandle_t hContent, byte[] pvData, int cubDataToRead, uint cOffset, EUGCReadAction eAction)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_UGCRead(hContent, pvData, cubDataToRead, cOffset, eAction);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00027C54 File Offset: 0x00025E54
		public static int GetCachedUGCCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetCachedUGCCount();
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00027C60 File Offset: 0x00025E60
		public static UGCHandle_t GetCachedUGCHandle(int iCachedContent)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCHandle_t)NativeMethods.ISteamRemoteStorage_GetCachedUGCHandle(iCachedContent);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00027C74 File Offset: 0x00025E74
		public static SteamAPICall_t PublishWorkshopFile(string pchFile, string pchPreviewFile, AppId_t nConsumerAppId, string pchTitle, string pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IList<string> pTags, EWorkshopFileType eWorkshopFileType)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchTitle))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchDescription))
						{
							result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_PublishWorkshopFile(utf8StringHandle, utf8StringHandle2, nConsumerAppId, utf8StringHandle3, utf8StringHandle4, eVisibility, new InteropHelp.SteamParamStringArray(pTags), eWorkshopFileType);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00027D24 File Offset: 0x00025F24
		public static PublishedFileUpdateHandle_t CreatePublishedFileUpdateRequest(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (PublishedFileUpdateHandle_t)NativeMethods.ISteamRemoteStorage_CreatePublishedFileUpdateRequest(unPublishedFileId);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00027D38 File Offset: 0x00025F38
		public static bool UpdatePublishedFileFile(PublishedFileUpdateHandle_t updateHandle, string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileFile(updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00027D78 File Offset: 0x00025F78
		public static bool UpdatePublishedFilePreviewFile(PublishedFileUpdateHandle_t updateHandle, string pchPreviewFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPreviewFile))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFilePreviewFile(updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00027DB8 File Offset: 0x00025FB8
		public static bool UpdatePublishedFileTitle(PublishedFileUpdateHandle_t updateHandle, string pchTitle)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTitle(updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00027DF8 File Offset: 0x00025FF8
		public static bool UpdatePublishedFileDescription(PublishedFileUpdateHandle_t updateHandle, string pchDescription)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileDescription(updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00027E38 File Offset: 0x00026038
		public static bool UpdatePublishedFileVisibility(PublishedFileUpdateHandle_t updateHandle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileVisibility(updateHandle, eVisibility);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00027E46 File Offset: 0x00026046
		public static bool UpdatePublishedFileTags(PublishedFileUpdateHandle_t updateHandle, IList<string> pTags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTags(updateHandle, new InteropHelp.SteamParamStringArray(pTags));
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00027E5E File Offset: 0x0002605E
		public static SteamAPICall_t CommitPublishedFileUpdate(PublishedFileUpdateHandle_t updateHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_CommitPublishedFileUpdate(updateHandle);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00027E70 File Offset: 0x00026070
		public static SteamAPICall_t GetPublishedFileDetails(PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_GetPublishedFileDetails(unPublishedFileId, unMaxSecondsOld);
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00027E83 File Offset: 0x00026083
		public static SteamAPICall_t DeletePublishedFile(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_DeletePublishedFile(unPublishedFileId);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00027E95 File Offset: 0x00026095
		public static SteamAPICall_t EnumerateUserPublishedFiles(uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumerateUserPublishedFiles(unStartIndex);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00027EA7 File Offset: 0x000260A7
		public static SteamAPICall_t SubscribePublishedFile(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_SubscribePublishedFile(unPublishedFileId);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00027EB9 File Offset: 0x000260B9
		public static SteamAPICall_t EnumerateUserSubscribedFiles(uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumerateUserSubscribedFiles(unStartIndex);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00027ECB File Offset: 0x000260CB
		public static SteamAPICall_t UnsubscribePublishedFile(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UnsubscribePublishedFile(unPublishedFileId);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00027EE0 File Offset: 0x000260E0
		public static bool UpdatePublishedFileSetChangeDescription(PublishedFileUpdateHandle_t updateHandle, string pchChangeDescription)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchChangeDescription))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00027F20 File Offset: 0x00026120
		public static SteamAPICall_t GetPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_GetPublishedItemVoteDetails(unPublishedFileId);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00027F32 File Offset: 0x00026132
		public static SteamAPICall_t UpdateUserPublishedItemVote(PublishedFileId_t unPublishedFileId, bool bVoteUp)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UpdateUserPublishedItemVote(unPublishedFileId, bVoteUp);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00027F45 File Offset: 0x00026145
		public static SteamAPICall_t GetUserPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_GetUserPublishedItemVoteDetails(unPublishedFileId);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00027F57 File Offset: 0x00026157
		public static SteamAPICall_t EnumerateUserSharedWorkshopFiles(CSteamID steamId, uint unStartIndex, IList<string> pRequiredTags, IList<string> pExcludedTags)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(steamId, unStartIndex, new InteropHelp.SteamParamStringArray(pRequiredTags), new InteropHelp.SteamParamStringArray(pExcludedTags));
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00027F80 File Offset: 0x00026180
		public static SteamAPICall_t PublishVideo(EWorkshopVideoProvider eVideoProvider, string pchVideoAccount, string pchVideoIdentifier, string pchPreviewFile, AppId_t nConsumerAppId, string pchTitle, string pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IList<string> pTags)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVideoAccount))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchVideoIdentifier))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchTitle))
						{
							using (InteropHelp.UTF8StringHandle utf8StringHandle5 = new InteropHelp.UTF8StringHandle(pchDescription))
							{
								result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_PublishVideo(eVideoProvider, utf8StringHandle, utf8StringHandle2, utf8StringHandle3, nConsumerAppId, utf8StringHandle4, utf8StringHandle5, eVisibility, new InteropHelp.SteamParamStringArray(pTags));
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00028054 File Offset: 0x00026254
		public static SteamAPICall_t SetUserPublishedFileAction(PublishedFileId_t unPublishedFileId, EWorkshopFileAction eAction)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_SetUserPublishedFileAction(unPublishedFileId, eAction);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00028067 File Offset: 0x00026267
		public static SteamAPICall_t EnumeratePublishedFilesByUserAction(EWorkshopFileAction eAction, uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(eAction, unStartIndex);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002807A File Offset: 0x0002627A
		public static SteamAPICall_t EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, IList<string> pTags, IList<string> pUserTags)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(eEnumerationType, unStartIndex, unCount, unDays, new InteropHelp.SteamParamStringArray(pTags), new InteropHelp.SteamParamStringArray(pUserTags));
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x000280A8 File Offset: 0x000262A8
		public static SteamAPICall_t UGCDownloadToLocation(UGCHandle_t hContent, string pchLocation, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLocation))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UGCDownloadToLocation(hContent, utf8StringHandle, unPriority);
			}
			return result;
		}
	}
}
