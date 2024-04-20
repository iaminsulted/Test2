using System;

namespace Steamworks
{
	// Token: 0x0200009A RID: 154
	public static class SteamScreenshots
	{
		// Token: 0x060008E9 RID: 2281 RVA: 0x000280EC File Offset: 0x000262EC
		public static ScreenshotHandle WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight)
		{
			InteropHelp.TestIfAvailableClient();
			return (ScreenshotHandle)NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00028104 File Offset: 0x00026304
		public static ScreenshotHandle AddScreenshotToLibrary(string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight)
		{
			InteropHelp.TestIfAvailableClient();
			ScreenshotHandle result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFilename))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchThumbnailFilename))
				{
					result = (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(utf8StringHandle, utf8StringHandle2, nWidth, nHeight);
				}
			}
			return result;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00028168 File Offset: 0x00026368
		public static void TriggerScreenshot()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_TriggerScreenshot();
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00028174 File Offset: 0x00026374
		public static void HookScreenshots(bool bHook)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00028184 File Offset: 0x00026384
		public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLocation))
			{
				result = NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x000281C4 File Offset: 0x000263C4
		public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x000281D2 File Offset: 0x000263D2
		public static bool TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x000281E0 File Offset: 0x000263E0
		public static bool IsScreenshotsHooked()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_IsScreenshotsHooked();
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000281EC File Offset: 0x000263EC
		public static ScreenshotHandle AddVRScreenshotToLibrary(EVRScreenshotType eType, string pchFilename, string pchVRFilename)
		{
			InteropHelp.TestIfAvailableClient();
			ScreenshotHandle result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFilename))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchVRFilename))
				{
					result = (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddVRScreenshotToLibrary(eType, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}
	}
}
