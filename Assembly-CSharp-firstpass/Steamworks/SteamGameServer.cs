using System;

namespace Steamworks
{
	// Token: 0x0200008A RID: 138
	public static class SteamGameServer
	{
		// Token: 0x06000706 RID: 1798 RVA: 0x00024A34 File Offset: 0x00022C34
		public static bool InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, AppId_t nGameAppId, string pchVersionString)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				result = NativeMethods.ISteamGameServer_InitGameServer(unIP, usGamePort, usQueryPort, unFlags, nGameAppId, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00024A78 File Offset: 0x00022C78
		public static void SetProduct(string pszProduct)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszProduct))
			{
				NativeMethods.ISteamGameServer_SetProduct(utf8StringHandle);
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00024AB4 File Offset: 0x00022CB4
		public static void SetGameDescription(string pszGameDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszGameDescription))
			{
				NativeMethods.ISteamGameServer_SetGameDescription(utf8StringHandle);
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00024AF0 File Offset: 0x00022CF0
		public static void SetModDir(string pszModDir)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszModDir))
			{
				NativeMethods.ISteamGameServer_SetModDir(utf8StringHandle);
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00024B2C File Offset: 0x00022D2C
		public static void SetDedicatedServer(bool bDedicated)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetDedicatedServer(bDedicated);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00024B3C File Offset: 0x00022D3C
		public static void LogOn(string pszToken)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszToken))
			{
				NativeMethods.ISteamGameServer_LogOn(utf8StringHandle);
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00024B78 File Offset: 0x00022D78
		public static void LogOnAnonymous()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_LogOnAnonymous();
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00024B84 File Offset: 0x00022D84
		public static void LogOff()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_LogOff();
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00024B90 File Offset: 0x00022D90
		public static bool BLoggedOn()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BLoggedOn();
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00024B9C File Offset: 0x00022D9C
		public static bool BSecure()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BSecure();
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00024BA8 File Offset: 0x00022DA8
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (CSteamID)NativeMethods.ISteamGameServer_GetSteamID();
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00024BB9 File Offset: 0x00022DB9
		public static bool WasRestartRequested()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_WasRestartRequested();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00024BC5 File Offset: 0x00022DC5
		public static void SetMaxPlayerCount(int cPlayersMax)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetMaxPlayerCount(cPlayersMax);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00024BD2 File Offset: 0x00022DD2
		public static void SetBotPlayerCount(int cBotplayers)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetBotPlayerCount(cBotplayers);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00024BE0 File Offset: 0x00022DE0
		public static void SetServerName(string pszServerName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszServerName))
			{
				NativeMethods.ISteamGameServer_SetServerName(utf8StringHandle);
			}
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00024C1C File Offset: 0x00022E1C
		public static void SetMapName(string pszMapName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszMapName))
			{
				NativeMethods.ISteamGameServer_SetMapName(utf8StringHandle);
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00024C58 File Offset: 0x00022E58
		public static void SetPasswordProtected(bool bPasswordProtected)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetPasswordProtected(bPasswordProtected);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00024C65 File Offset: 0x00022E65
		public static void SetSpectatorPort(ushort unSpectatorPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetSpectatorPort(unSpectatorPort);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00024C74 File Offset: 0x00022E74
		public static void SetSpectatorServerName(string pszSpectatorServerName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszSpectatorServerName))
			{
				NativeMethods.ISteamGameServer_SetSpectatorServerName(utf8StringHandle);
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00024CB0 File Offset: 0x00022EB0
		public static void ClearAllKeyValues()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_ClearAllKeyValues();
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00024CBC File Offset: 0x00022EBC
		public static void SetKeyValue(string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					NativeMethods.ISteamGameServer_SetKeyValue(utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00024D18 File Offset: 0x00022F18
		public static void SetGameTags(string pchGameTags)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchGameTags))
			{
				NativeMethods.ISteamGameServer_SetGameTags(utf8StringHandle);
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00024D54 File Offset: 0x00022F54
		public static void SetGameData(string pchGameData)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchGameData))
			{
				NativeMethods.ISteamGameServer_SetGameData(utf8StringHandle);
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00024D90 File Offset: 0x00022F90
		public static void SetRegion(string pszRegion)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszRegion))
			{
				NativeMethods.ISteamGameServer_SetRegion(utf8StringHandle);
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00024DCC File Offset: 0x00022FCC
		public static bool SendUserConnectAndAuthenticate(uint unIPClient, byte[] pvAuthBlob, uint cubAuthBlobSize, out CSteamID pSteamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_SendUserConnectAndAuthenticate(unIPClient, pvAuthBlob, cubAuthBlobSize, out pSteamIDUser);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00024DDC File Offset: 0x00022FDC
		public static CSteamID CreateUnauthenticatedUserConnection()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (CSteamID)NativeMethods.ISteamGameServer_CreateUnauthenticatedUserConnection();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00024DED File Offset: 0x00022FED
		public static void SendUserDisconnect(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SendUserDisconnect(steamIDUser);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00024DFC File Offset: 0x00022FFC
		public static bool BUpdateUserData(CSteamID steamIDUser, string pchPlayerName, uint uScore)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPlayerName))
			{
				result = NativeMethods.ISteamGameServer_BUpdateUserData(steamIDUser, utf8StringHandle, uScore);
			}
			return result;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00024E3C File Offset: 0x0002303C
		public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (HAuthTicket)NativeMethods.ISteamGameServer_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00024E50 File Offset: 0x00023050
		public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00024E5F File Offset: 0x0002305F
		public static void EndAuthSession(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_EndAuthSession(steamID);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00024E6C File Offset: 0x0002306C
		public static void CancelAuthTicket(HAuthTicket hAuthTicket)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_CancelAuthTicket(hAuthTicket);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00024E79 File Offset: 0x00023079
		public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_UserHasLicenseForApp(steamID, appID);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00024E87 File Offset: 0x00023087
		public static bool RequestUserGroupStatus(CSteamID steamIDUser, CSteamID steamIDGroup)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_RequestUserGroupStatus(steamIDUser, steamIDGroup);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00024E95 File Offset: 0x00023095
		public static void GetGameplayStats()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_GetGameplayStats();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00024EA1 File Offset: 0x000230A1
		public static SteamAPICall_t GetServerReputation()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_GetServerReputation();
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00024EB2 File Offset: 0x000230B2
		public static uint GetPublicIP()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_GetPublicIP();
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00024EBE File Offset: 0x000230BE
		public static bool HandleIncomingPacket(byte[] pData, int cbData, uint srcIP, ushort srcPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_HandleIncomingPacket(pData, cbData, srcIP, srcPort);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00024ECE File Offset: 0x000230CE
		public static int GetNextOutgoingPacket(byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_GetNextOutgoingPacket(pOut, cbMaxOut, out pNetAdr, out pPort);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00024EDE File Offset: 0x000230DE
		public static void EnableHeartbeats(bool bActive)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_EnableHeartbeats(bActive);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00024EEB File Offset: 0x000230EB
		public static void SetHeartbeatInterval(int iHeartbeatInterval)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetHeartbeatInterval(iHeartbeatInterval);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00024EF8 File Offset: 0x000230F8
		public static void ForceHeartbeat()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_ForceHeartbeat();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00024F04 File Offset: 0x00023104
		public static SteamAPICall_t AssociateWithClan(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_AssociateWithClan(steamIDClan);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00024F16 File Offset: 0x00023116
		public static SteamAPICall_t ComputeNewPlayerCompatibility(CSteamID steamIDNewPlayer)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_ComputeNewPlayerCompatibility(steamIDNewPlayer);
		}
	}
}
