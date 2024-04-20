using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000090 RID: 144
	public static class SteamGameServerUtils
	{
		// Token: 0x060007CA RID: 1994 RVA: 0x000261CA File Offset: 0x000243CA
		public static uint GetSecondsSinceAppActive()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetSecondsSinceAppActive();
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000261D6 File Offset: 0x000243D6
		public static uint GetSecondsSinceComputerActive()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetSecondsSinceComputerActive();
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000261E2 File Offset: 0x000243E2
		public static EUniverse GetConnectedUniverse()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetConnectedUniverse();
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x000261EE File Offset: 0x000243EE
		public static uint GetServerRealTime()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetServerRealTime();
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000261FA File Offset: 0x000243FA
		public static string GetIPCountry()
		{
			InteropHelp.TestIfAvailableGameServer();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetIPCountry());
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002620B File Offset: 0x0002440B
		public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002621A File Offset: 0x0002441A
		public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00026229 File Offset: 0x00024429
		public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetCSERIPPort(out unIP, out usPort);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00026237 File Offset: 0x00024437
		public static byte GetCurrentBatteryPower()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetCurrentBatteryPower();
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00026243 File Offset: 0x00024443
		public static AppId_t GetAppID()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (AppId_t)NativeMethods.ISteamGameServerUtils_GetAppID();
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00026254 File Offset: 0x00024454
		public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetOverlayNotificationPosition(eNotificationPosition);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00026261 File Offset: 0x00024461
		public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002626F File Offset: 0x0002446F
		public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetAPICallFailureReason(hSteamAPICall);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002627C File Offset: 0x0002447C
		public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0002628E File Offset: 0x0002448E
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetIPCCallCount();
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0002629A File Offset: 0x0002449A
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetWarningMessageHook(pFunction);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000262A7 File Offset: 0x000244A7
		public static bool IsOverlayEnabled()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsOverlayEnabled();
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x000262B3 File Offset: 0x000244B3
		public static bool BOverlayNeedsPresent()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_BOverlayNeedsPresent();
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x000262C0 File Offset: 0x000244C0
		public static SteamAPICall_t CheckFileSignature(string szFileName)
		{
			InteropHelp.TestIfAvailableGameServer();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(szFileName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamGameServerUtils_CheckFileSignature(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00026304 File Offset: 0x00024504
		public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchExistingText))
				{
					result = NativeMethods.ISteamGameServerUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, utf8StringHandle, unCharMax, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00026364 File Offset: 0x00024564
		public static uint GetEnteredGamepadTextLength()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextLength();
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00026370 File Offset: 0x00024570
		public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchText);
			bool flag = NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextInput(intPtr, cchText);
			pchText = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000263A6 File Offset: 0x000245A6
		public static string GetSteamUILanguage()
		{
			InteropHelp.TestIfAvailableGameServer();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetSteamUILanguage());
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000263B7 File Offset: 0x000245B7
		public static bool IsSteamRunningInVR()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsSteamRunningInVR();
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x000263C3 File Offset: 0x000245C3
		public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000263D1 File Offset: 0x000245D1
		public static bool IsSteamInBigPictureMode()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsSteamInBigPictureMode();
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000263DD File Offset: 0x000245DD
		public static void StartVRDashboard()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_StartVRDashboard();
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000263E9 File Offset: 0x000245E9
		public static bool IsVRHeadsetStreamingEnabled()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsVRHeadsetStreamingEnabled();
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x000263F5 File Offset: 0x000245F5
		public static void SetVRHeadsetStreamingEnabled(bool bEnabled)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetVRHeadsetStreamingEnabled(bEnabled);
		}
	}
}
