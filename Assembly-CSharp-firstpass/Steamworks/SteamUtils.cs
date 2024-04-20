using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009F RID: 159
	public static class SteamUtils
	{
		// Token: 0x06000985 RID: 2437 RVA: 0x0002967C File Offset: 0x0002787C
		public static uint GetSecondsSinceAppActive()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetSecondsSinceAppActive();
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00029688 File Offset: 0x00027888
		public static uint GetSecondsSinceComputerActive()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetSecondsSinceComputerActive();
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00029694 File Offset: 0x00027894
		public static EUniverse GetConnectedUniverse()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetConnectedUniverse();
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x000296A0 File Offset: 0x000278A0
		public static uint GetServerRealTime()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetServerRealTime();
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x000296AC File Offset: 0x000278AC
		public static string GetIPCountry()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetIPCountry());
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x000296BD File Offset: 0x000278BD
		public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000296CC File Offset: 0x000278CC
		public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x000296DB File Offset: 0x000278DB
		public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetCSERIPPort(out unIP, out usPort);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x000296E9 File Offset: 0x000278E9
		public static byte GetCurrentBatteryPower()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetCurrentBatteryPower();
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x000296F5 File Offset: 0x000278F5
		public static AppId_t GetAppID()
		{
			InteropHelp.TestIfAvailableClient();
			return (AppId_t)NativeMethods.ISteamUtils_GetAppID();
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00029706 File Offset: 0x00027906
		public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetOverlayNotificationPosition(eNotificationPosition);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00029713 File Offset: 0x00027913
		public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00029721 File Offset: 0x00027921
		public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetAPICallFailureReason(hSteamAPICall);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002972E File Offset: 0x0002792E
		public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00029740 File Offset: 0x00027940
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetIPCCallCount();
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002974C File Offset: 0x0002794C
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetWarningMessageHook(pFunction);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00029759 File Offset: 0x00027959
		public static bool IsOverlayEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsOverlayEnabled();
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00029765 File Offset: 0x00027965
		public static bool BOverlayNeedsPresent()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_BOverlayNeedsPresent();
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00029774 File Offset: 0x00027974
		public static SteamAPICall_t CheckFileSignature(string szFileName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(szFileName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUtils_CheckFileSignature(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x000297B8 File Offset: 0x000279B8
		public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchExistingText))
				{
					result = NativeMethods.ISteamUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, utf8StringHandle, unCharMax, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00029818 File Offset: 0x00027A18
		public static uint GetEnteredGamepadTextLength()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetEnteredGamepadTextLength();
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00029824 File Offset: 0x00027A24
		public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchText);
			bool flag = NativeMethods.ISteamUtils_GetEnteredGamepadTextInput(intPtr, cchText);
			pchText = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002985A File Offset: 0x00027A5A
		public static string GetSteamUILanguage()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetSteamUILanguage());
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002986B File Offset: 0x00027A6B
		public static bool IsSteamRunningInVR()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsSteamRunningInVR();
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00029877 File Offset: 0x00027A77
		public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00029885 File Offset: 0x00027A85
		public static bool IsSteamInBigPictureMode()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsSteamInBigPictureMode();
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00029891 File Offset: 0x00027A91
		public static void StartVRDashboard()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_StartVRDashboard();
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0002989D File Offset: 0x00027A9D
		public static bool IsVRHeadsetStreamingEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsVRHeadsetStreamingEnabled();
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x000298A9 File Offset: 0x00027AA9
		public static void SetVRHeadsetStreamingEnabled(bool bEnabled)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetVRHeadsetStreamingEnabled(bEnabled);
		}
	}
}
