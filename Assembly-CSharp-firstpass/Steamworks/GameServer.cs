using System;

namespace Steamworks
{
	// Token: 0x020001BD RID: 445
	public static class GameServer
	{
		// Token: 0x06000D60 RID: 3424 RVA: 0x0002A834 File Offset: 0x00028A34
		public static bool Init(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString)
		{
			InteropHelp.TestIfPlatformSupported();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				result = NativeMethods.SteamGameServer_Init(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0002A878 File Offset: 0x00028A78
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_Shutdown();
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0002A884 File Offset: 0x00028A84
		public static void RunCallbacks()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_RunCallbacks();
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0002A890 File Offset: 0x00028A90
		public static void ReleaseCurrentThreadMemory()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_ReleaseCurrentThreadMemory();
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0002A89C File Offset: 0x00028A9C
		public static bool BSecure()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamGameServer_BSecure();
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0002A8A8 File Offset: 0x00028AA8
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfPlatformSupported();
			return (CSteamID)NativeMethods.SteamGameServer_GetSteamID();
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0002A8B9 File Offset: 0x00028AB9
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamGameServer_GetHSteamPipe();
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0002A8CA File Offset: 0x00028ACA
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamGameServer_GetHSteamUser();
		}
	}
}
