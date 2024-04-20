using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009D RID: 157
	public static class SteamUser
	{
		// Token: 0x0600093D RID: 2365 RVA: 0x00028C9C File Offset: 0x00026E9C
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamUser_GetHSteamUser();
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00028CAD File Offset: 0x00026EAD
		public static bool BLoggedOn()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BLoggedOn();
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00028CB9 File Offset: 0x00026EB9
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamUser_GetSteamID();
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00028CCA File Offset: 0x00026ECA
		public static int InitiateGameConnection(byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, bool bSecure)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_InitiateGameConnection(pAuthBlob, cbMaxAuthBlob, steamIDGameServer, unIPServer, usPortServer, bSecure);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00028CDE File Offset: 0x00026EDE
		public static void TerminateGameConnection(uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_TerminateGameConnection(unIPServer, usPortServer);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00028CEC File Offset: 0x00026EEC
		public static void TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, string pchExtraInfo = "")
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchExtraInfo))
			{
				NativeMethods.ISteamUser_TrackAppUsageEvent(gameID, eAppUsageEvent, utf8StringHandle);
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00028D2C File Offset: 0x00026F2C
		public static bool GetUserDataFolder(out string pchBuffer, int cubBuffer)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubBuffer);
			bool flag = NativeMethods.ISteamUser_GetUserDataFolder(intPtr, cubBuffer);
			pchBuffer = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00028D62 File Offset: 0x00026F62
		public static void StartVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StartVoiceRecording();
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00028D6E File Offset: 0x00026F6E
		public static void StopVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StopVoiceRecording();
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00028D7A File Offset: 0x00026F7A
		public static EVoiceResult GetAvailableVoice(out uint pcbCompressed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetAvailableVoice(out pcbCompressed, IntPtr.Zero, 0U);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00028D90 File Offset: 0x00026F90
		public static EVoiceResult GetVoice(bool bWantCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoice(bWantCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, false, IntPtr.Zero, 0U, IntPtr.Zero, 0U);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00028DB8 File Offset: 0x00026FB8
		public static EVoiceResult DecompressVoice(byte[] pCompressed, uint cbCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_DecompressVoice(pCompressed, cbCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, nDesiredSampleRate);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00028DCC File Offset: 0x00026FCC
		public static uint GetVoiceOptimalSampleRate()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoiceOptimalSampleRate();
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00028DD8 File Offset: 0x00026FD8
		public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return (HAuthTicket)NativeMethods.ISteamUser_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00028DEC File Offset: 0x00026FEC
		public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00028DFB File Offset: 0x00026FFB
		public static void EndAuthSession(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_EndAuthSession(steamID);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00028E08 File Offset: 0x00027008
		public static void CancelAuthTicket(HAuthTicket hAuthTicket)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_CancelAuthTicket(hAuthTicket);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00028E15 File Offset: 0x00027015
		public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_UserHasLicenseForApp(steamID, appID);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00028E23 File Offset: 0x00027023
		public static bool BIsBehindNAT()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsBehindNAT();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00028E2F File Offset: 0x0002702F
		public static void AdvertiseGame(CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_AdvertiseGame(steamIDGameServer, unIPServer, usPortServer);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00028E3E File Offset: 0x0002703E
		public static SteamAPICall_t RequestEncryptedAppTicket(byte[] pDataToInclude, int cbDataToInclude)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUser_RequestEncryptedAppTicket(pDataToInclude, cbDataToInclude);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00028E51 File Offset: 0x00027051
		public static bool GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetEncryptedAppTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00028E60 File Offset: 0x00027060
		public static int GetGameBadgeLevel(int nSeries, bool bFoil)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetGameBadgeLevel(nSeries, bFoil);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00028E6E File Offset: 0x0002706E
		public static int GetPlayerSteamLevel()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetPlayerSteamLevel();
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00028E7C File Offset: 0x0002707C
		public static SteamAPICall_t RequestStoreAuthURL(string pchRedirectURL)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchRedirectURL))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUser_RequestStoreAuthURL(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00028EC0 File Offset: 0x000270C0
		public static bool BIsPhoneVerified()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneVerified();
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00028ECC File Offset: 0x000270CC
		public static bool BIsTwoFactorEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsTwoFactorEnabled();
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00028ED8 File Offset: 0x000270D8
		public static bool BIsPhoneIdentifying()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneIdentifying();
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00028EE4 File Offset: 0x000270E4
		public static bool BIsPhoneRequiringVerification()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneRequiringVerification();
		}
	}
}
