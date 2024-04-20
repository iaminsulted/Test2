using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000094 RID: 148
	public static class SteamMatchmaking
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x00026D5D File Offset: 0x00024F5D
		public static int GetFavoriteGameCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetFavoriteGameCount();
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00026D69 File Offset: 0x00024F69
		public static bool GetFavoriteGame(int iGame, out AppId_t pnAppID, out uint pnIP, out ushort pnConnPort, out ushort pnQueryPort, out uint punFlags, out uint pRTime32LastPlayedOnServer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetFavoriteGame(iGame, out pnAppID, out pnIP, out pnConnPort, out pnQueryPort, out punFlags, out pRTime32LastPlayedOnServer);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00026D7F File Offset: 0x00024F7F
		public static int AddFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_AddFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags, rTime32LastPlayedOnServer);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00026D93 File Offset: 0x00024F93
		public static bool RemoveFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_RemoveFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00026DA5 File Offset: 0x00024FA5
		public static SteamAPICall_t RequestLobbyList()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamMatchmaking_RequestLobbyList();
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00026DB8 File Offset: 0x00024FB8
		public static void AddRequestLobbyListStringFilter(string pchKeyToMatch, string pchValueToMatch, ELobbyComparison eComparisonType)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValueToMatch))
				{
					NativeMethods.ISteamMatchmaking_AddRequestLobbyListStringFilter(utf8StringHandle, utf8StringHandle2, eComparisonType);
				}
			}
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00026E14 File Offset: 0x00025014
		public static void AddRequestLobbyListNumericalFilter(string pchKeyToMatch, int nValueToMatch, ELobbyComparison eComparisonType)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
			{
				NativeMethods.ISteamMatchmaking_AddRequestLobbyListNumericalFilter(utf8StringHandle, nValueToMatch, eComparisonType);
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00026E54 File Offset: 0x00025054
		public static void AddRequestLobbyListNearValueFilter(string pchKeyToMatch, int nValueToBeCloseTo)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
			{
				NativeMethods.ISteamMatchmaking_AddRequestLobbyListNearValueFilter(utf8StringHandle, nValueToBeCloseTo);
			}
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00026E90 File Offset: 0x00025090
		public static void AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(nSlotsAvailable);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00026E9D File Offset: 0x0002509D
		public static void AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter eLobbyDistanceFilter)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_AddRequestLobbyListDistanceFilter(eLobbyDistanceFilter);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00026EAA File Offset: 0x000250AA
		public static void AddRequestLobbyListResultCountFilter(int cMaxResults)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_AddRequestLobbyListResultCountFilter(cMaxResults);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00026EB7 File Offset: 0x000250B7
		public static void AddRequestLobbyListCompatibleMembersFilter(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(steamIDLobby);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00026EC4 File Offset: 0x000250C4
		public static CSteamID GetLobbyByIndex(int iLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamMatchmaking_GetLobbyByIndex(iLobby);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00026ED6 File Offset: 0x000250D6
		public static SteamAPICall_t CreateLobby(ELobbyType eLobbyType, int cMaxMembers)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamMatchmaking_CreateLobby(eLobbyType, cMaxMembers);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00026EE9 File Offset: 0x000250E9
		public static SteamAPICall_t JoinLobby(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamMatchmaking_JoinLobby(steamIDLobby);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00026EFB File Offset: 0x000250FB
		public static void LeaveLobby(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_LeaveLobby(steamIDLobby);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00026F08 File Offset: 0x00025108
		public static bool InviteUserToLobby(CSteamID steamIDLobby, CSteamID steamIDInvitee)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_InviteUserToLobby(steamIDLobby, steamIDInvitee);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00026F16 File Offset: 0x00025116
		public static int GetNumLobbyMembers(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetNumLobbyMembers(steamIDLobby);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00026F23 File Offset: 0x00025123
		public static CSteamID GetLobbyMemberByIndex(CSteamID steamIDLobby, int iMember)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamMatchmaking_GetLobbyMemberByIndex(steamIDLobby, iMember);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00026F38 File Offset: 0x00025138
		public static string GetLobbyData(CSteamID steamIDLobby, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamMatchmaking_GetLobbyData(steamIDLobby, utf8StringHandle));
			}
			return result;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00026F7C File Offset: 0x0002517C
		public static bool SetLobbyData(CSteamID steamIDLobby, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					result = NativeMethods.ISteamMatchmaking_SetLobbyData(steamIDLobby, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00026FD8 File Offset: 0x000251D8
		public static int GetLobbyDataCount(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetLobbyDataCount(steamIDLobby);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00026FE8 File Offset: 0x000251E8
		public static bool GetLobbyDataByIndex(CSteamID steamIDLobby, int iLobbyData, out string pchKey, int cchKeyBufferSize, out string pchValue, int cchValueBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchKeyBufferSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal(cchValueBufferSize);
			bool flag = NativeMethods.ISteamMatchmaking_GetLobbyDataByIndex(steamIDLobby, iLobbyData, intPtr, cchKeyBufferSize, intPtr2, cchValueBufferSize);
			pchKey = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00027040 File Offset: 0x00025240
		public static bool DeleteLobbyData(CSteamID steamIDLobby, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = NativeMethods.ISteamMatchmaking_DeleteLobbyData(steamIDLobby, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00027080 File Offset: 0x00025280
		public static string GetLobbyMemberData(CSteamID steamIDLobby, CSteamID steamIDUser, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamMatchmaking_GetLobbyMemberData(steamIDLobby, steamIDUser, utf8StringHandle));
			}
			return result;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000270C4 File Offset: 0x000252C4
		public static void SetLobbyMemberData(CSteamID steamIDLobby, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					NativeMethods.ISteamMatchmaking_SetLobbyMemberData(steamIDLobby, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00027120 File Offset: 0x00025320
		public static bool SendLobbyChatMsg(CSteamID steamIDLobby, byte[] pvMsgBody, int cubMsgBody)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SendLobbyChatMsg(steamIDLobby, pvMsgBody, cubMsgBody);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0002712F File Offset: 0x0002532F
		public static int GetLobbyChatEntry(CSteamID steamIDLobby, int iChatID, out CSteamID pSteamIDUser, byte[] pvData, int cubData, out EChatEntryType peChatEntryType)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetLobbyChatEntry(steamIDLobby, iChatID, out pSteamIDUser, pvData, cubData, out peChatEntryType);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00027143 File Offset: 0x00025343
		public static bool RequestLobbyData(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_RequestLobbyData(steamIDLobby);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00027150 File Offset: 0x00025350
		public static void SetLobbyGameServer(CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort, CSteamID steamIDGameServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_SetLobbyGameServer(steamIDLobby, unGameServerIP, unGameServerPort, steamIDGameServer);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00027160 File Offset: 0x00025360
		public static bool GetLobbyGameServer(CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort, out CSteamID psteamIDGameServer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetLobbyGameServer(steamIDLobby, out punGameServerIP, out punGameServerPort, out psteamIDGameServer);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00027170 File Offset: 0x00025370
		public static bool SetLobbyMemberLimit(CSteamID steamIDLobby, int cMaxMembers)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLobbyMemberLimit(steamIDLobby, cMaxMembers);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0002717E File Offset: 0x0002537E
		public static int GetLobbyMemberLimit(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetLobbyMemberLimit(steamIDLobby);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0002718B File Offset: 0x0002538B
		public static bool SetLobbyType(CSteamID steamIDLobby, ELobbyType eLobbyType)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLobbyType(steamIDLobby, eLobbyType);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00027199 File Offset: 0x00025399
		public static bool SetLobbyJoinable(CSteamID steamIDLobby, bool bLobbyJoinable)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLobbyJoinable(steamIDLobby, bLobbyJoinable);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x000271A7 File Offset: 0x000253A7
		public static CSteamID GetLobbyOwner(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamMatchmaking_GetLobbyOwner(steamIDLobby);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000271B9 File Offset: 0x000253B9
		public static bool SetLobbyOwner(CSteamID steamIDLobby, CSteamID steamIDNewOwner)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLobbyOwner(steamIDLobby, steamIDNewOwner);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000271C7 File Offset: 0x000253C7
		public static bool SetLinkedLobby(CSteamID steamIDLobby, CSteamID steamIDLobbyDependent)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLinkedLobby(steamIDLobby, steamIDLobbyDependent);
		}
	}
}
