using System;

namespace Steamworks
{
	// Token: 0x02000091 RID: 145
	public static class SteamHTMLSurface
	{
		// Token: 0x060007E7 RID: 2023 RVA: 0x00026402 File Offset: 0x00024602
		public static bool Init()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTMLSurface_Init();
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0002640E File Offset: 0x0002460E
		public static bool Shutdown()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTMLSurface_Shutdown();
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002641C File Offset: 0x0002461C
		public static SteamAPICall_t CreateBrowser(string pchUserAgent, string pchUserCSS)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchUserAgent))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchUserCSS))
				{
					result = (SteamAPICall_t)NativeMethods.ISteamHTMLSurface_CreateBrowser(utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0002647C File Offset: 0x0002467C
		public static void RemoveBrowser(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_RemoveBrowser(unBrowserHandle);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002648C File Offset: 0x0002468C
		public static void LoadURL(HHTMLBrowser unBrowserHandle, string pchURL, string pchPostData)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchURL))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchPostData))
				{
					NativeMethods.ISteamHTMLSurface_LoadURL(unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000264E8 File Offset: 0x000246E8
		public static void SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetSize(unBrowserHandle, unWidth, unHeight);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000264F7 File Offset: 0x000246F7
		public static void StopLoad(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_StopLoad(unBrowserHandle);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00026504 File Offset: 0x00024704
		public static void Reload(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_Reload(unBrowserHandle);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00026511 File Offset: 0x00024711
		public static void GoBack(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GoBack(unBrowserHandle);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0002651E File Offset: 0x0002471E
		public static void GoForward(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GoForward(unBrowserHandle);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0002652C File Offset: 0x0002472C
		public static void AddHeader(HHTMLBrowser unBrowserHandle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					NativeMethods.ISteamHTMLSurface_AddHeader(unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00026588 File Offset: 0x00024788
		public static void ExecuteJavascript(HHTMLBrowser unBrowserHandle, string pchScript)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchScript))
			{
				NativeMethods.ISteamHTMLSurface_ExecuteJavascript(unBrowserHandle, utf8StringHandle);
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000265C4 File Offset: 0x000247C4
		public static void MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseUp(unBrowserHandle, eMouseButton);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000265D2 File Offset: 0x000247D2
		public static void MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseDown(unBrowserHandle, eMouseButton);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000265E0 File Offset: 0x000247E0
		public static void MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseDoubleClick(unBrowserHandle, eMouseButton);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000265EE File Offset: 0x000247EE
		public static void MouseMove(HHTMLBrowser unBrowserHandle, int x, int y)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseMove(unBrowserHandle, x, y);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000265FD File Offset: 0x000247FD
		public static void MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseWheel(unBrowserHandle, nDelta);
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0002660B File Offset: 0x0002480B
		public static void KeyDown(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyDown(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0002661A File Offset: 0x0002481A
		public static void KeyUp(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyUp(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00026629 File Offset: 0x00024829
		public static void KeyChar(HHTMLBrowser unBrowserHandle, uint cUnicodeChar, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyChar(unBrowserHandle, cUnicodeChar, eHTMLKeyModifiers);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00026638 File Offset: 0x00024838
		public static void SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetHorizontalScroll(unBrowserHandle, nAbsolutePixelScroll);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00026646 File Offset: 0x00024846
		public static void SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetVerticalScroll(unBrowserHandle, nAbsolutePixelScroll);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00026654 File Offset: 0x00024854
		public static void SetKeyFocus(HHTMLBrowser unBrowserHandle, bool bHasKeyFocus)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetKeyFocus(unBrowserHandle, bHasKeyFocus);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00026662 File Offset: 0x00024862
		public static void ViewSource(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_ViewSource(unBrowserHandle);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0002666F File Offset: 0x0002486F
		public static void CopyToClipboard(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_CopyToClipboard(unBrowserHandle);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002667C File Offset: 0x0002487C
		public static void PasteFromClipboard(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_PasteFromClipboard(unBrowserHandle);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002668C File Offset: 0x0002488C
		public static void Find(HHTMLBrowser unBrowserHandle, string pchSearchStr, bool bCurrentlyInFind, bool bReverse)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchSearchStr))
			{
				NativeMethods.ISteamHTMLSurface_Find(unBrowserHandle, utf8StringHandle, bCurrentlyInFind, bReverse);
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x000266CC File Offset: 0x000248CC
		public static void StopFind(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_StopFind(unBrowserHandle);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x000266D9 File Offset: 0x000248D9
		public static void GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GetLinkAtPosition(unBrowserHandle, x, y);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000266E8 File Offset: 0x000248E8
		public static void SetCookie(string pchHostname, string pchKey, string pchValue, string pchPath = "/", uint nExpires = 0U, bool bSecure = false, bool bHTTPOnly = false)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHostname))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchKey))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchValue))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchPath))
						{
							NativeMethods.ISteamHTMLSurface_SetCookie(utf8StringHandle, utf8StringHandle2, utf8StringHandle3, utf8StringHandle4, nExpires, bSecure, bHTTPOnly);
						}
					}
				}
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00026784 File Offset: 0x00024984
		public static void SetPageScaleFactor(HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetPageScaleFactor(unBrowserHandle, flZoom, nPointX, nPointY);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00026794 File Offset: 0x00024994
		public static void SetBackgroundMode(HHTMLBrowser unBrowserHandle, bool bBackgroundMode)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetBackgroundMode(unBrowserHandle, bBackgroundMode);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000267A2 File Offset: 0x000249A2
		public static void AllowStartRequest(HHTMLBrowser unBrowserHandle, bool bAllowed)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_AllowStartRequest(unBrowserHandle, bAllowed);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x000267B0 File Offset: 0x000249B0
		public static void JSDialogResponse(HHTMLBrowser unBrowserHandle, bool bResult)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_JSDialogResponse(unBrowserHandle, bResult);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x000267BE File Offset: 0x000249BE
		public static void FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_FileLoadDialogResponse(unBrowserHandle, pchSelectedFiles);
		}
	}
}
