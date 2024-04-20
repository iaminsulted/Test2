using System;

namespace Steamworks
{
	// Token: 0x020001BC RID: 444
	public static class SteamAPI
	{
		// Token: 0x06000D56 RID: 3414 RVA: 0x0002A7AF File Offset: 0x000289AF
		public static bool InitSafe()
		{
			return SteamAPI.Init();
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0002A7B6 File Offset: 0x000289B6
		public static bool Init()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_Init();
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0002A7C2 File Offset: 0x000289C2
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_Shutdown();
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0002A7CE File Offset: 0x000289CE
		public static bool RestartAppIfNecessary(AppId_t unOwnAppID)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0002A7DB File Offset: 0x000289DB
		public static void ReleaseCurrentThreadMemory()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_ReleaseCurrentThreadMemory();
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0002A7E7 File Offset: 0x000289E7
		public static void RunCallbacks()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_RunCallbacks();
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0002A7F3 File Offset: 0x000289F3
		public static bool IsSteamRunning()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_IsSteamRunning();
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0002A7FF File Offset: 0x000289FF
		public static HSteamUser GetHSteamUserCurrent()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.Steam_GetHSteamUserCurrent();
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0002A810 File Offset: 0x00028A10
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamAPI_GetHSteamPipe();
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0002A821 File Offset: 0x00028A21
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamAPI_GetHSteamUser();
		}
	}
}
