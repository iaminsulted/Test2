using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000085 RID: 133
	public static class SteamAppList
	{
		// Token: 0x06000667 RID: 1639 RVA: 0x00023864 File Offset: 0x00021A64
		public static uint GetNumInstalledApps()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamAppList_GetNumInstalledApps();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00023870 File Offset: 0x00021A70
		public static uint GetInstalledApps(AppId_t[] pvecAppID, uint unMaxAppIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamAppList_GetInstalledApps(pvecAppID, unMaxAppIDs);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00023880 File Offset: 0x00021A80
		public static int GetAppName(AppId_t nAppID, out string pchName, int cchNameMax)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameMax);
			int num = NativeMethods.ISteamAppList_GetAppName(nAppID, intPtr, cchNameMax);
			pchName = ((num != -1) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000238B8 File Offset: 0x00021AB8
		public static int GetAppInstallDir(AppId_t nAppID, out string pchDirectory, int cchNameMax)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameMax);
			int num = NativeMethods.ISteamAppList_GetAppInstallDir(nAppID, intPtr, cchNameMax);
			pchDirectory = ((num != -1) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000238F0 File Offset: 0x00021AF0
		public static int GetAppBuildId(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamAppList_GetAppBuildId(nAppID);
		}
	}
}
