using System;

namespace Steamworks
{
	// Token: 0x02000087 RID: 135
	public static class SteamClient
	{
		// Token: 0x06000686 RID: 1670 RVA: 0x00023B50 File Offset: 0x00021D50
		public static HSteamPipe CreateSteamPipe()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamPipe)NativeMethods.ISteamClient_CreateSteamPipe();
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00023B61 File Offset: 0x00021D61
		public static bool BReleaseSteamPipe(HSteamPipe hSteamPipe)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamClient_BReleaseSteamPipe(hSteamPipe);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00023B6E File Offset: 0x00021D6E
		public static HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamClient_ConnectToGlobalUser(hSteamPipe);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00023B80 File Offset: 0x00021D80
		public static HSteamUser CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamClient_CreateLocalUser(out phSteamPipe, eAccountType);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00023B93 File Offset: 0x00021D93
		public static void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_ReleaseUser(hSteamPipe, hUser);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00023BA4 File Offset: 0x00021DA4
		public static IntPtr GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUser(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00023BE4 File Offset: 0x00021DE4
		public static IntPtr GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamGameServer(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00023C24 File Offset: 0x00021E24
		public static void SetLocalIPBinding(uint unIP, ushort usPort)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_SetLocalIPBinding(unIP, usPort);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00023C34 File Offset: 0x00021E34
		public static IntPtr GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamFriends(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00023C74 File Offset: 0x00021E74
		public static IntPtr GetISteamUtils(HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUtils(hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00023CB4 File Offset: 0x00021EB4
		public static IntPtr GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamMatchmaking(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00023CF4 File Offset: 0x00021EF4
		public static IntPtr GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamMatchmakingServers(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00023D34 File Offset: 0x00021F34
		public static IntPtr GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamGenericInterface(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00023D74 File Offset: 0x00021F74
		public static IntPtr GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUserStats(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00023DB4 File Offset: 0x00021FB4
		public static IntPtr GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamGameServerStats(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00023DF4 File Offset: 0x00021FF4
		public static IntPtr GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamApps(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00023E34 File Offset: 0x00022034
		public static IntPtr GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamNetworking(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00023E74 File Offset: 0x00022074
		public static IntPtr GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamRemoteStorage(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00023EB4 File Offset: 0x000220B4
		public static IntPtr GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamScreenshots(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00023EF4 File Offset: 0x000220F4
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamClient_GetIPCCallCount();
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00023F00 File Offset: 0x00022100
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_SetWarningMessageHook(pFunction);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00023F0D File Offset: 0x0002210D
		public static bool BShutdownIfAllPipesClosed()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamClient_BShutdownIfAllPipesClosed();
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00023F1C File Offset: 0x0002211C
		public static IntPtr GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamHTTP(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00023F5C File Offset: 0x0002215C
		public static IntPtr GetISteamUnifiedMessages(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUnifiedMessages(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00023F9C File Offset: 0x0002219C
		public static IntPtr GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamController(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00023FDC File Offset: 0x000221DC
		public static IntPtr GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUGC(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0002401C File Offset: 0x0002221C
		public static IntPtr GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamAppList(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0002405C File Offset: 0x0002225C
		public static IntPtr GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamMusic(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0002409C File Offset: 0x0002229C
		public static IntPtr GetISteamMusicRemote(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamMusicRemote(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000240DC File Offset: 0x000222DC
		public static IntPtr GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamHTMLSurface(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0002411C File Offset: 0x0002231C
		public static IntPtr GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamInventory(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002415C File Offset: 0x0002235C
		public static IntPtr GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamVideo(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}
	}
}
