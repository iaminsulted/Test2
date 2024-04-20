using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Steamworks
{
	// Token: 0x020000A1 RID: 161
	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{
		// Token: 0x060009A6 RID: 2470
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Init")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamAPI_Init();

		// Token: 0x060009A7 RID: 2471
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Shutdown")]
		public static extern void SteamAPI_Shutdown();

		// Token: 0x060009A8 RID: 2472
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "RestartAppIfNecessary")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamAPI_RestartAppIfNecessary(AppId_t unOwnAppID);

		// Token: 0x060009A9 RID: 2473
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ReleaseCurrentThreadMemory")]
		public static extern void SteamAPI_ReleaseCurrentThreadMemory();

		// Token: 0x060009AA RID: 2474
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "WriteMiniDump")]
		public static extern void SteamAPI_WriteMiniDump(uint uStructuredExceptionCode, IntPtr pvExceptionInfo, uint uBuildID);

		// Token: 0x060009AB RID: 2475
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetMiniDumpComment")]
		public static extern void SteamAPI_SetMiniDumpComment(InteropHelp.UTF8StringHandle pchMsg);

		// Token: 0x060009AC RID: 2476
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "RunCallbacks")]
		public static extern void SteamAPI_RunCallbacks();

		// Token: 0x060009AD RID: 2477
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "RegisterCallback")]
		public static extern void SteamAPI_RegisterCallback(IntPtr pCallback, int iCallback);

		// Token: 0x060009AE RID: 2478
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "UnregisterCallback")]
		public static extern void SteamAPI_UnregisterCallback(IntPtr pCallback);

		// Token: 0x060009AF RID: 2479
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "RegisterCallResult")]
		public static extern void SteamAPI_RegisterCallResult(IntPtr pCallback, ulong hAPICall);

		// Token: 0x060009B0 RID: 2480
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "UnregisterCallResult")]
		public static extern void SteamAPI_UnregisterCallResult(IntPtr pCallback, ulong hAPICall);

		// Token: 0x060009B1 RID: 2481
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IsSteamRunning")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamAPI_IsSteamRunning();

		// Token: 0x060009B2 RID: 2482
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Steam_RunCallbacks_")]
		public static extern void Steam_RunCallbacks(HSteamPipe hSteamPipe, [MarshalAs(UnmanagedType.I1)] bool bGameServerCallbacks);

		// Token: 0x060009B3 RID: 2483
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Steam_RegisterInterfaceFuncs_")]
		public static extern void Steam_RegisterInterfaceFuncs(IntPtr hModule);

		// Token: 0x060009B4 RID: 2484
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Steam_GetHSteamUserCurrent_")]
		public static extern int Steam_GetHSteamUserCurrent();

		// Token: 0x060009B5 RID: 2485
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetSteamInstallPath")]
		public static extern int SteamAPI_GetSteamInstallPath();

		// Token: 0x060009B6 RID: 2486
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetHSteamPipe_")]
		public static extern int SteamAPI_GetHSteamPipe();

		// Token: 0x060009B7 RID: 2487
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTryCatchCallbacks")]
		public static extern void SteamAPI_SetTryCatchCallbacks([MarshalAs(UnmanagedType.I1)] bool bTryCatchCallbacks);

		// Token: 0x060009B8 RID: 2488
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetHSteamUser_")]
		public static extern int SteamAPI_GetHSteamUser();

		// Token: 0x060009B9 RID: 2489
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamInternal_CreateInterface_")]
		public static extern void SteamInternal_CreateInterface(IntPtr ver);

		// Token: 0x060009BA RID: 2490
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "UseBreakpadCrashHandler")]
		public static extern void SteamAPI_UseBreakpadCrashHandler(InteropHelp.UTF8StringHandle pchVersion, InteropHelp.UTF8StringHandle pchDate, InteropHelp.UTF8StringHandle pchTime, [MarshalAs(UnmanagedType.I1)] bool bFullMemoryDumps, IntPtr pvContext, IntPtr m_pfnPreMinidumpCallback);

		// Token: 0x060009BB RID: 2491
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetBreakpadAppID")]
		public static extern void SteamAPI_SetBreakpadAppID(uint unAppID);

		// Token: 0x060009BC RID: 2492
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_Init")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamGameServer_Init(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, InteropHelp.UTF8StringHandle pchVersionString);

		// Token: 0x060009BD RID: 2493
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_Shutdown")]
		public static extern void SteamGameServer_Shutdown();

		// Token: 0x060009BE RID: 2494
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_RunCallbacks")]
		public static extern void SteamGameServer_RunCallbacks();

		// Token: 0x060009BF RID: 2495
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_ReleaseCurrentThreadMemory")]
		public static extern void SteamGameServer_ReleaseCurrentThreadMemory();

		// Token: 0x060009C0 RID: 2496
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_BSecure")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamGameServer_BSecure();

		// Token: 0x060009C1 RID: 2497
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_GetSteamID")]
		public static extern ulong SteamGameServer_GetSteamID();

		// Token: 0x060009C2 RID: 2498
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_GetHSteamPipe")]
		public static extern int SteamGameServer_GetHSteamPipe();

		// Token: 0x060009C3 RID: 2499
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_GetHSteamUser")]
		public static extern int SteamGameServer_GetHSteamUser();

		// Token: 0x060009C4 RID: 2500
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamInternal_GameServer_Init_")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamInternal_GameServer_Init(uint unIP, ushort usPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, InteropHelp.UTF8StringHandle pchVersionString);

		// Token: 0x060009C5 RID: 2501
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamClient_")]
		public static extern IntPtr SteamClient();

		// Token: 0x060009C6 RID: 2502
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamGameServerClient_")]
		public static extern IntPtr SteamGameServerClient();

		// Token: 0x060009C7 RID: 2503
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamEncryptedAppTicket_BDecryptTicket(byte[] rgubTicketEncrypted, uint cubTicketEncrypted, byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] rgubKey, int cubKey);

		// Token: 0x060009C8 RID: 2504
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamEncryptedAppTicket_BIsTicketForApp(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID);

		// Token: 0x060009C9 RID: 2505
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint SteamEncryptedAppTicket_GetTicketIssueTime(byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

		// Token: 0x060009CA RID: 2506
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SteamEncryptedAppTicket_GetTicketSteamID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out CSteamID psteamID);

		// Token: 0x060009CB RID: 2507
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint SteamEncryptedAppTicket_GetTicketAppID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

		// Token: 0x060009CC RID: 2508
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamEncryptedAppTicket_BUserOwnsAppInTicket(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID);

		// Token: 0x060009CD RID: 2509
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamEncryptedAppTicket_BUserIsVacBanned(byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

		// Token: 0x060009CE RID: 2510
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamEncryptedAppTicket_GetUserVariableData(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData);

		// Token: 0x060009CF RID: 2511
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamAppList_GetNumInstalledApps();

		// Token: 0x060009D0 RID: 2512
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamAppList_GetInstalledApps([In] [Out] AppId_t[] pvecAppID, uint unMaxAppIDs);

		// Token: 0x060009D1 RID: 2513
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamAppList_GetAppName(AppId_t nAppID, IntPtr pchName, int cchNameMax);

		// Token: 0x060009D2 RID: 2514
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamAppList_GetAppInstallDir(AppId_t nAppID, IntPtr pchDirectory, int cchNameMax);

		// Token: 0x060009D3 RID: 2515
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamAppList_GetAppBuildId(AppId_t nAppID);

		// Token: 0x060009D4 RID: 2516
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsSubscribed();

		// Token: 0x060009D5 RID: 2517
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsLowViolence();

		// Token: 0x060009D6 RID: 2518
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsCybercafe();

		// Token: 0x060009D7 RID: 2519
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsVACBanned();

		// Token: 0x060009D8 RID: 2520
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamApps_GetCurrentGameLanguage();

		// Token: 0x060009D9 RID: 2521
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamApps_GetAvailableGameLanguages();

		// Token: 0x060009DA RID: 2522
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsSubscribedApp(AppId_t appID);

		// Token: 0x060009DB RID: 2523
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsDlcInstalled(AppId_t appID);

		// Token: 0x060009DC RID: 2524
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamApps_GetEarliestPurchaseUnixTime(AppId_t nAppID);

		// Token: 0x060009DD RID: 2525
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsSubscribedFromFreeWeekend();

		// Token: 0x060009DE RID: 2526
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamApps_GetDLCCount();

		// Token: 0x060009DF RID: 2527
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BGetDLCDataByIndex(int iDLC, out AppId_t pAppID, out bool pbAvailable, IntPtr pchName, int cchNameBufferSize);

		// Token: 0x060009E0 RID: 2528
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamApps_InstallDLC(AppId_t nAppID);

		// Token: 0x060009E1 RID: 2529
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamApps_UninstallDLC(AppId_t nAppID);

		// Token: 0x060009E2 RID: 2530
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamApps_RequestAppProofOfPurchaseKey(AppId_t nAppID);

		// Token: 0x060009E3 RID: 2531
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_GetCurrentBetaName(IntPtr pchName, int cchNameBufferSize);

		// Token: 0x060009E4 RID: 2532
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_MarkContentCorrupt([MarshalAs(UnmanagedType.I1)] bool bMissingFilesOnly);

		// Token: 0x060009E5 RID: 2533
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamApps_GetInstalledDepots(AppId_t appID, [In] [Out] DepotId_t[] pvecDepots, uint cMaxDepots);

		// Token: 0x060009E6 RID: 2534
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamApps_GetAppInstallDir(AppId_t appID, IntPtr pchFolder, uint cchFolderBufferSize);

		// Token: 0x060009E7 RID: 2535
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsAppInstalled(AppId_t appID);

		// Token: 0x060009E8 RID: 2536
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamApps_GetAppOwner();

		// Token: 0x060009E9 RID: 2537
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamApps_GetLaunchQueryParam(InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x060009EA RID: 2538
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_GetDlcDownloadProgress(AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		// Token: 0x060009EB RID: 2539
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamApps_GetAppBuildId();

		// Token: 0x060009EC RID: 2540
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamApps_RequestAllProofOfPurchaseKeys();

		// Token: 0x060009ED RID: 2541
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamApps_GetFileDetails(InteropHelp.UTF8StringHandle pszFileName);

		// Token: 0x060009EE RID: 2542
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamClient_CreateSteamPipe();

		// Token: 0x060009EF RID: 2543
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamClient_BReleaseSteamPipe(HSteamPipe hSteamPipe);

		// Token: 0x060009F0 RID: 2544
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamClient_ConnectToGlobalUser(HSteamPipe hSteamPipe);

		// Token: 0x060009F1 RID: 2545
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamClient_CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType);

		// Token: 0x060009F2 RID: 2546
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser);

		// Token: 0x060009F3 RID: 2547
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009F4 RID: 2548
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009F5 RID: 2549
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_SetLocalIPBinding(uint unIP, ushort usPort);

		// Token: 0x060009F6 RID: 2550
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009F7 RID: 2551
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUtils(HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009F8 RID: 2552
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009F9 RID: 2553
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009FA RID: 2554
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009FB RID: 2555
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009FC RID: 2556
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009FD RID: 2557
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009FE RID: 2558
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x060009FF RID: 2559
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A00 RID: 2560
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A01 RID: 2561
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamClient_GetIPCCallCount();

		// Token: 0x06000A02 RID: 2562
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction);

		// Token: 0x06000A03 RID: 2563
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamClient_BShutdownIfAllPipesClosed();

		// Token: 0x06000A04 RID: 2564
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A05 RID: 2565
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUnifiedMessages(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A06 RID: 2566
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A07 RID: 2567
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A08 RID: 2568
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A09 RID: 2569
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A0A RID: 2570
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamMusicRemote(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A0B RID: 2571
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A0C RID: 2572
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A0D RID: 2573
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000A0E RID: 2574
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamController_Init();

		// Token: 0x06000A0F RID: 2575
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamController_Shutdown();

		// Token: 0x06000A10 RID: 2576
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_RunFrame();

		// Token: 0x06000A11 RID: 2577
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamController_GetConnectedControllers([In] [Out] ControllerHandle_t[] handlesOut);

		// Token: 0x06000A12 RID: 2578
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamController_ShowBindingPanel(ControllerHandle_t controllerHandle);

		// Token: 0x06000A13 RID: 2579
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamController_GetActionSetHandle(InteropHelp.UTF8StringHandle pszActionSetName);

		// Token: 0x06000A14 RID: 2580
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_ActivateActionSet(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle);

		// Token: 0x06000A15 RID: 2581
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamController_GetCurrentActionSet(ControllerHandle_t controllerHandle);

		// Token: 0x06000A16 RID: 2582
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamController_GetDigitalActionHandle(InteropHelp.UTF8StringHandle pszActionName);

		// Token: 0x06000A17 RID: 2583
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ControllerDigitalActionData_t ISteamController_GetDigitalActionData(ControllerHandle_t controllerHandle, ControllerDigitalActionHandle_t digitalActionHandle);

		// Token: 0x06000A18 RID: 2584
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamController_GetDigitalActionOrigins(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle, ControllerDigitalActionHandle_t digitalActionHandle, [In] [Out] EControllerActionOrigin[] originsOut);

		// Token: 0x06000A19 RID: 2585
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamController_GetAnalogActionHandle(InteropHelp.UTF8StringHandle pszActionName);

		// Token: 0x06000A1A RID: 2586
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ControllerAnalogActionData_t ISteamController_GetAnalogActionData(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t analogActionHandle);

		// Token: 0x06000A1B RID: 2587
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamController_GetAnalogActionOrigins(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle, ControllerAnalogActionHandle_t analogActionHandle, [In] [Out] EControllerActionOrigin[] originsOut);

		// Token: 0x06000A1C RID: 2588
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_StopAnalogActionMomentum(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t eAction);

		// Token: 0x06000A1D RID: 2589
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_TriggerHapticPulse(ControllerHandle_t controllerHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec);

		// Token: 0x06000A1E RID: 2590
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_TriggerRepeatedHapticPulse(ControllerHandle_t controllerHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec, ushort usOffMicroSec, ushort unRepeat, uint nFlags);

		// Token: 0x06000A1F RID: 2591
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_TriggerVibration(ControllerHandle_t controllerHandle, ushort usLeftSpeed, ushort usRightSpeed);

		// Token: 0x06000A20 RID: 2592
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_SetLEDColor(ControllerHandle_t controllerHandle, byte nColorR, byte nColorG, byte nColorB, uint nFlags);

		// Token: 0x06000A21 RID: 2593
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamController_GetGamepadIndexForController(ControllerHandle_t ulControllerHandle);

		// Token: 0x06000A22 RID: 2594
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamController_GetControllerForGamepadIndex(int nIndex);

		// Token: 0x06000A23 RID: 2595
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ControllerMotionData_t ISteamController_GetMotionData(ControllerHandle_t controllerHandle);

		// Token: 0x06000A24 RID: 2596
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamController_ShowDigitalActionOrigins(ControllerHandle_t controllerHandle, ControllerDigitalActionHandle_t digitalActionHandle, float flScale, float flXPosition, float flYPosition);

		// Token: 0x06000A25 RID: 2597
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamController_ShowAnalogActionOrigins(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t analogActionHandle, float flScale, float flXPosition, float flYPosition);

		// Token: 0x06000A26 RID: 2598
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamController_GetStringForActionOrigin(EControllerActionOrigin eOrigin);

		// Token: 0x06000A27 RID: 2599
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamController_GetGlyphForActionOrigin(EControllerActionOrigin eOrigin);

		// Token: 0x06000A28 RID: 2600
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetPersonaName();

		// Token: 0x06000A29 RID: 2601
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_SetPersonaName(InteropHelp.UTF8StringHandle pchPersonaName);

		// Token: 0x06000A2A RID: 2602
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EPersonaState ISteamFriends_GetPersonaState();

		// Token: 0x06000A2B RID: 2603
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendCount(EFriendFlags iFriendFlags);

		// Token: 0x06000A2C RID: 2604
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetFriendByIndex(int iFriend, EFriendFlags iFriendFlags);

		// Token: 0x06000A2D RID: 2605
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EFriendRelationship ISteamFriends_GetFriendRelationship(CSteamID steamIDFriend);

		// Token: 0x06000A2E RID: 2606
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EPersonaState ISteamFriends_GetFriendPersonaState(CSteamID steamIDFriend);

		// Token: 0x06000A2F RID: 2607
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendPersonaName(CSteamID steamIDFriend);

		// Token: 0x06000A30 RID: 2608
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_GetFriendGamePlayed(CSteamID steamIDFriend, out FriendGameInfo_t pFriendGameInfo);

		// Token: 0x06000A31 RID: 2609
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendPersonaNameHistory(CSteamID steamIDFriend, int iPersonaName);

		// Token: 0x06000A32 RID: 2610
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendSteamLevel(CSteamID steamIDFriend);

		// Token: 0x06000A33 RID: 2611
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetPlayerNickname(CSteamID steamIDPlayer);

		// Token: 0x06000A34 RID: 2612
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendsGroupCount();

		// Token: 0x06000A35 RID: 2613
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern short ISteamFriends_GetFriendsGroupIDByIndex(int iFG);

		// Token: 0x06000A36 RID: 2614
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendsGroupName(FriendsGroupID_t friendsGroupID);

		// Token: 0x06000A37 RID: 2615
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendsGroupMembersCount(FriendsGroupID_t friendsGroupID);

		// Token: 0x06000A38 RID: 2616
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_GetFriendsGroupMembersList(FriendsGroupID_t friendsGroupID, [In] [Out] CSteamID[] pOutSteamIDMembers, int nMembersCount);

		// Token: 0x06000A39 RID: 2617
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_HasFriend(CSteamID steamIDFriend, EFriendFlags iFriendFlags);

		// Token: 0x06000A3A RID: 2618
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanCount();

		// Token: 0x06000A3B RID: 2619
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetClanByIndex(int iClan);

		// Token: 0x06000A3C RID: 2620
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetClanName(CSteamID steamIDClan);

		// Token: 0x06000A3D RID: 2621
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetClanTag(CSteamID steamIDClan);

		// Token: 0x06000A3E RID: 2622
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_GetClanActivityCounts(CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting);

		// Token: 0x06000A3F RID: 2623
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_DownloadClanActivityCounts([In] [Out] CSteamID[] psteamIDClans, int cClansToRequest);

		// Token: 0x06000A40 RID: 2624
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendCountFromSource(CSteamID steamIDSource);

		// Token: 0x06000A41 RID: 2625
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetFriendFromSourceByIndex(CSteamID steamIDSource, int iFriend);

		// Token: 0x06000A42 RID: 2626
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_IsUserInSource(CSteamID steamIDUser, CSteamID steamIDSource);

		// Token: 0x06000A43 RID: 2627
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_SetInGameVoiceSpeaking(CSteamID steamIDUser, [MarshalAs(UnmanagedType.I1)] bool bSpeaking);

		// Token: 0x06000A44 RID: 2628
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlay(InteropHelp.UTF8StringHandle pchDialog);

		// Token: 0x06000A45 RID: 2629
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayToUser(InteropHelp.UTF8StringHandle pchDialog, CSteamID steamID);

		// Token: 0x06000A46 RID: 2630
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayToWebPage(InteropHelp.UTF8StringHandle pchURL);

		// Token: 0x06000A47 RID: 2631
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag);

		// Token: 0x06000A48 RID: 2632
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_SetPlayedWith(CSteamID steamIDUserPlayedWith);

		// Token: 0x06000A49 RID: 2633
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayInviteDialog(CSteamID steamIDLobby);

		// Token: 0x06000A4A RID: 2634
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetSmallFriendAvatar(CSteamID steamIDFriend);

		// Token: 0x06000A4B RID: 2635
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetMediumFriendAvatar(CSteamID steamIDFriend);

		// Token: 0x06000A4C RID: 2636
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetLargeFriendAvatar(CSteamID steamIDFriend);

		// Token: 0x06000A4D RID: 2637
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_RequestUserInformation(CSteamID steamIDUser, [MarshalAs(UnmanagedType.I1)] bool bRequireNameOnly);

		// Token: 0x06000A4E RID: 2638
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_RequestClanOfficerList(CSteamID steamIDClan);

		// Token: 0x06000A4F RID: 2639
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetClanOwner(CSteamID steamIDClan);

		// Token: 0x06000A50 RID: 2640
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanOfficerCount(CSteamID steamIDClan);

		// Token: 0x06000A51 RID: 2641
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetClanOfficerByIndex(CSteamID steamIDClan, int iOfficer);

		// Token: 0x06000A52 RID: 2642
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamFriends_GetUserRestrictions();

		// Token: 0x06000A53 RID: 2643
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_SetRichPresence(InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000A54 RID: 2644
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ClearRichPresence();

		// Token: 0x06000A55 RID: 2645
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendRichPresence(CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000A56 RID: 2646
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendRichPresenceKeyCount(CSteamID steamIDFriend);

		// Token: 0x06000A57 RID: 2647
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendRichPresenceKeyByIndex(CSteamID steamIDFriend, int iKey);

		// Token: 0x06000A58 RID: 2648
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_RequestFriendRichPresence(CSteamID steamIDFriend);

		// Token: 0x06000A59 RID: 2649
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_InviteUserToGame(CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchConnectString);

		// Token: 0x06000A5A RID: 2650
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetCoplayFriendCount();

		// Token: 0x06000A5B RID: 2651
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetCoplayFriend(int iCoplayFriend);

		// Token: 0x06000A5C RID: 2652
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendCoplayTime(CSteamID steamIDFriend);

		// Token: 0x06000A5D RID: 2653
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamFriends_GetFriendCoplayGame(CSteamID steamIDFriend);

		// Token: 0x06000A5E RID: 2654
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_JoinClanChatRoom(CSteamID steamIDClan);

		// Token: 0x06000A5F RID: 2655
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_LeaveClanChatRoom(CSteamID steamIDClan);

		// Token: 0x06000A60 RID: 2656
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanChatMemberCount(CSteamID steamIDClan);

		// Token: 0x06000A61 RID: 2657
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetChatMemberByIndex(CSteamID steamIDClan, int iUser);

		// Token: 0x06000A62 RID: 2658
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_SendClanChatMessage(CSteamID steamIDClanChat, InteropHelp.UTF8StringHandle pchText);

		// Token: 0x06000A63 RID: 2659
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanChatMessage(CSteamID steamIDClanChat, int iMessage, IntPtr prgchText, int cchTextMax, out EChatEntryType peChatEntryType, out CSteamID psteamidChatter);

		// Token: 0x06000A64 RID: 2660
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_IsClanChatAdmin(CSteamID steamIDClanChat, CSteamID steamIDUser);

		// Token: 0x06000A65 RID: 2661
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_IsClanChatWindowOpenInSteam(CSteamID steamIDClanChat);

		// Token: 0x06000A66 RID: 2662
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_OpenClanChatWindowInSteam(CSteamID steamIDClanChat);

		// Token: 0x06000A67 RID: 2663
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_CloseClanChatWindowInSteam(CSteamID steamIDClanChat);

		// Token: 0x06000A68 RID: 2664
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_SetListenForFriendsMessages([MarshalAs(UnmanagedType.I1)] bool bInterceptEnabled);

		// Token: 0x06000A69 RID: 2665
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_ReplyToFriendMessage(CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchMsgToSend);

		// Token: 0x06000A6A RID: 2666
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendMessage(CSteamID steamIDFriend, int iMessageID, IntPtr pvData, int cubData, out EChatEntryType peChatEntryType);

		// Token: 0x06000A6B RID: 2667
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetFollowerCount(CSteamID steamID);

		// Token: 0x06000A6C RID: 2668
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_IsFollowing(CSteamID steamID);

		// Token: 0x06000A6D RID: 2669
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_EnumerateFollowingList(uint unStartIndex);

		// Token: 0x06000A6E RID: 2670
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, AppId_t nGameAppId, InteropHelp.UTF8StringHandle pchVersionString);

		// Token: 0x06000A6F RID: 2671
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetProduct(InteropHelp.UTF8StringHandle pszProduct);

		// Token: 0x06000A70 RID: 2672
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetGameDescription(InteropHelp.UTF8StringHandle pszGameDescription);

		// Token: 0x06000A71 RID: 2673
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetModDir(InteropHelp.UTF8StringHandle pszModDir);

		// Token: 0x06000A72 RID: 2674
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetDedicatedServer([MarshalAs(UnmanagedType.I1)] bool bDedicated);

		// Token: 0x06000A73 RID: 2675
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_LogOn(InteropHelp.UTF8StringHandle pszToken);

		// Token: 0x06000A74 RID: 2676
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_LogOnAnonymous();

		// Token: 0x06000A75 RID: 2677
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_LogOff();

		// Token: 0x06000A76 RID: 2678
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_BLoggedOn();

		// Token: 0x06000A77 RID: 2679
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_BSecure();

		// Token: 0x06000A78 RID: 2680
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_GetSteamID();

		// Token: 0x06000A79 RID: 2681
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_WasRestartRequested();

		// Token: 0x06000A7A RID: 2682
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetMaxPlayerCount(int cPlayersMax);

		// Token: 0x06000A7B RID: 2683
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetBotPlayerCount(int cBotplayers);

		// Token: 0x06000A7C RID: 2684
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetServerName(InteropHelp.UTF8StringHandle pszServerName);

		// Token: 0x06000A7D RID: 2685
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetMapName(InteropHelp.UTF8StringHandle pszMapName);

		// Token: 0x06000A7E RID: 2686
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetPasswordProtected([MarshalAs(UnmanagedType.I1)] bool bPasswordProtected);

		// Token: 0x06000A7F RID: 2687
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetSpectatorPort(ushort unSpectatorPort);

		// Token: 0x06000A80 RID: 2688
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetSpectatorServerName(InteropHelp.UTF8StringHandle pszSpectatorServerName);

		// Token: 0x06000A81 RID: 2689
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_ClearAllKeyValues();

		// Token: 0x06000A82 RID: 2690
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetKeyValue(InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

		// Token: 0x06000A83 RID: 2691
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetGameTags(InteropHelp.UTF8StringHandle pchGameTags);

		// Token: 0x06000A84 RID: 2692
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetGameData(InteropHelp.UTF8StringHandle pchGameData);

		// Token: 0x06000A85 RID: 2693
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetRegion(InteropHelp.UTF8StringHandle pszRegion);

		// Token: 0x06000A86 RID: 2694
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_SendUserConnectAndAuthenticate(uint unIPClient, byte[] pvAuthBlob, uint cubAuthBlobSize, out CSteamID pSteamIDUser);

		// Token: 0x06000A87 RID: 2695
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_CreateUnauthenticatedUserConnection();

		// Token: 0x06000A88 RID: 2696
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SendUserDisconnect(CSteamID steamIDUser);

		// Token: 0x06000A89 RID: 2697
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_BUpdateUserData(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchPlayerName, uint uScore);

		// Token: 0x06000A8A RID: 2698
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServer_GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

		// Token: 0x06000A8B RID: 2699
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EBeginAuthSessionResult ISteamGameServer_BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID);

		// Token: 0x06000A8C RID: 2700
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_EndAuthSession(CSteamID steamID);

		// Token: 0x06000A8D RID: 2701
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_CancelAuthTicket(HAuthTicket hAuthTicket);

		// Token: 0x06000A8E RID: 2702
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EUserHasLicenseForAppResult ISteamGameServer_UserHasLicenseForApp(CSteamID steamID, AppId_t appID);

		// Token: 0x06000A8F RID: 2703
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_RequestUserGroupStatus(CSteamID steamIDUser, CSteamID steamIDGroup);

		// Token: 0x06000A90 RID: 2704
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_GetGameplayStats();

		// Token: 0x06000A91 RID: 2705
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_GetServerReputation();

		// Token: 0x06000A92 RID: 2706
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServer_GetPublicIP();

		// Token: 0x06000A93 RID: 2707
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_HandleIncomingPacket(byte[] pData, int cbData, uint srcIP, ushort srcPort);

		// Token: 0x06000A94 RID: 2708
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamGameServer_GetNextOutgoingPacket(byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort);

		// Token: 0x06000A95 RID: 2709
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_EnableHeartbeats([MarshalAs(UnmanagedType.I1)] bool bActive);

		// Token: 0x06000A96 RID: 2710
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetHeartbeatInterval(int iHeartbeatInterval);

		// Token: 0x06000A97 RID: 2711
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_ForceHeartbeat();

		// Token: 0x06000A98 RID: 2712
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_AssociateWithClan(CSteamID steamIDClan);

		// Token: 0x06000A99 RID: 2713
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_ComputeNewPlayerCompatibility(CSteamID steamIDNewPlayer);

		// Token: 0x06000A9A RID: 2714
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerStats_RequestUserStats(CSteamID steamIDUser);

		// Token: 0x06000A9B RID: 2715
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_GetUserStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out int pData);

		// Token: 0x06000A9C RID: 2716
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_GetUserStat_(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out float pData);

		// Token: 0x06000A9D RID: 2717
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_GetUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

		// Token: 0x06000A9E RID: 2718
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_SetUserStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, int nData);

		// Token: 0x06000A9F RID: 2719
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_SetUserStat_(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, float fData);

		// Token: 0x06000AA0 RID: 2720
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_UpdateUserAvgRateStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

		// Token: 0x06000AA1 RID: 2721
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_SetUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName);

		// Token: 0x06000AA2 RID: 2722
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_ClearUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName);

		// Token: 0x06000AA3 RID: 2723
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerStats_StoreUserStats(CSteamID steamIDUser);

		// Token: 0x06000AA4 RID: 2724
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTMLSurface_Init();

		// Token: 0x06000AA5 RID: 2725
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTMLSurface_Shutdown();

		// Token: 0x06000AA6 RID: 2726
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamHTMLSurface_CreateBrowser(InteropHelp.UTF8StringHandle pchUserAgent, InteropHelp.UTF8StringHandle pchUserCSS);

		// Token: 0x06000AA7 RID: 2727
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_RemoveBrowser(HHTMLBrowser unBrowserHandle);

		// Token: 0x06000AA8 RID: 2728
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_LoadURL(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchURL, InteropHelp.UTF8StringHandle pchPostData);

		// Token: 0x06000AA9 RID: 2729
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight);

		// Token: 0x06000AAA RID: 2730
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_StopLoad(HHTMLBrowser unBrowserHandle);

		// Token: 0x06000AAB RID: 2731
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_Reload(HHTMLBrowser unBrowserHandle);

		// Token: 0x06000AAC RID: 2732
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_GoBack(HHTMLBrowser unBrowserHandle);

		// Token: 0x06000AAD RID: 2733
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_GoForward(HHTMLBrowser unBrowserHandle);

		// Token: 0x06000AAE RID: 2734
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_AddHeader(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000AAF RID: 2735
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_ExecuteJavascript(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchScript);

		// Token: 0x06000AB0 RID: 2736
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

		// Token: 0x06000AB1 RID: 2737
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

		// Token: 0x06000AB2 RID: 2738
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

		// Token: 0x06000AB3 RID: 2739
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseMove(HHTMLBrowser unBrowserHandle, int x, int y);

		// Token: 0x06000AB4 RID: 2740
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta);

		// Token: 0x06000AB5 RID: 2741
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_KeyDown(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers);

		// Token: 0x06000AB6 RID: 2742
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_KeyUp(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers);

		// Token: 0x06000AB7 RID: 2743
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_KeyChar(HHTMLBrowser unBrowserHandle, uint cUnicodeChar, EHTMLKeyModifiers eHTMLKeyModifiers);

		// Token: 0x06000AB8 RID: 2744
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

		// Token: 0x06000AB9 RID: 2745
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

		// Token: 0x06000ABA RID: 2746
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetKeyFocus(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bHasKeyFocus);

		// Token: 0x06000ABB RID: 2747
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_ViewSource(HHTMLBrowser unBrowserHandle);

		// Token: 0x06000ABC RID: 2748
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_CopyToClipboard(HHTMLBrowser unBrowserHandle);

		// Token: 0x06000ABD RID: 2749
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_PasteFromClipboard(HHTMLBrowser unBrowserHandle);

		// Token: 0x06000ABE RID: 2750
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_Find(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchSearchStr, [MarshalAs(UnmanagedType.I1)] bool bCurrentlyInFind, [MarshalAs(UnmanagedType.I1)] bool bReverse);

		// Token: 0x06000ABF RID: 2751
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_StopFind(HHTMLBrowser unBrowserHandle);

		// Token: 0x06000AC0 RID: 2752
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y);

		// Token: 0x06000AC1 RID: 2753
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetCookie(InteropHelp.UTF8StringHandle pchHostname, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue, InteropHelp.UTF8StringHandle pchPath, uint nExpires, [MarshalAs(UnmanagedType.I1)] bool bSecure, [MarshalAs(UnmanagedType.I1)] bool bHTTPOnly);

		// Token: 0x06000AC2 RID: 2754
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetPageScaleFactor(HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY);

		// Token: 0x06000AC3 RID: 2755
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetBackgroundMode(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bBackgroundMode);

		// Token: 0x06000AC4 RID: 2756
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_AllowStartRequest(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bAllowed);

		// Token: 0x06000AC5 RID: 2757
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_JSDialogResponse(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bResult);

		// Token: 0x06000AC6 RID: 2758
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles);

		// Token: 0x06000AC7 RID: 2759
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamHTTP_CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, InteropHelp.UTF8StringHandle pchAbsoluteURL);

		// Token: 0x06000AC8 RID: 2760
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue);

		// Token: 0x06000AC9 RID: 2761
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds);

		// Token: 0x06000ACA RID: 2762
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, InteropHelp.UTF8StringHandle pchHeaderValue);

		// Token: 0x06000ACB RID: 2763
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchParamName, InteropHelp.UTF8StringHandle pchParamValue);

		// Token: 0x06000ACC RID: 2764
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

		// Token: 0x06000ACD RID: 2765
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

		// Token: 0x06000ACE RID: 2766
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_DeferHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000ACF RID: 2767
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_PrioritizeHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000AD0 RID: 2768
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, out uint unResponseHeaderSize);

		// Token: 0x06000AD1 RID: 2769
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, byte[] pHeaderValueBuffer, uint unBufferSize);

		// Token: 0x06000AD2 RID: 2770
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize);

		// Token: 0x06000AD3 RID: 2771
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseBodyData(HTTPRequestHandle hRequest, byte[] pBodyDataBuffer, uint unBufferSize);

		// Token: 0x06000AD4 RID: 2772
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, byte[] pBodyDataBuffer, uint unBufferSize);

		// Token: 0x06000AD5 RID: 2773
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_ReleaseHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000AD6 RID: 2774
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut);

		// Token: 0x06000AD7 RID: 2775
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchContentType, byte[] pubBody, uint unBodyLen);

		// Token: 0x06000AD8 RID: 2776
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamHTTP_CreateCookieContainer([MarshalAs(UnmanagedType.I1)] bool bAllowResponsesToModify);

		// Token: 0x06000AD9 RID: 2777
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer);

		// Token: 0x06000ADA RID: 2778
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetCookie(HTTPCookieContainerHandle hCookieContainer, InteropHelp.UTF8StringHandle pchHost, InteropHelp.UTF8StringHandle pchUrl, InteropHelp.UTF8StringHandle pchCookie);

		// Token: 0x06000ADB RID: 2779
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer);

		// Token: 0x06000ADC RID: 2780
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchUserAgentInfo);

		// Token: 0x06000ADD RID: 2781
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, [MarshalAs(UnmanagedType.I1)] bool bRequireVerifiedCertificate);

		// Token: 0x06000ADE RID: 2782
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds);

		// Token: 0x06000ADF RID: 2783
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut);

		// Token: 0x06000AE0 RID: 2784
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EResult ISteamInventory_GetResultStatus(SteamInventoryResult_t resultHandle);

		// Token: 0x06000AE1 RID: 2785
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetResultItems(SteamInventoryResult_t resultHandle, [In] [Out] SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);

		// Token: 0x06000AE2 RID: 2786
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetResultItemProperty(SteamInventoryResult_t resultHandle, uint unItemIndex, InteropHelp.UTF8StringHandle pchPropertyName, IntPtr pchValueBuffer, ref uint punValueBufferSizeOut);

		// Token: 0x06000AE3 RID: 2787
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamInventory_GetResultTimestamp(SteamInventoryResult_t resultHandle);

		// Token: 0x06000AE4 RID: 2788
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected);

		// Token: 0x06000AE5 RID: 2789
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamInventory_DestroyResult(SteamInventoryResult_t resultHandle);

		// Token: 0x06000AE6 RID: 2790
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetAllItems(out SteamInventoryResult_t pResultHandle);

		// Token: 0x06000AE7 RID: 2791
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetItemsByID(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs);

		// Token: 0x06000AE8 RID: 2792
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_SerializeResult(SteamInventoryResult_t resultHandle, byte[] pOutBuffer, out uint punOutBufferSize);

		// Token: 0x06000AE9 RID: 2793
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_DeserializeResult(out SteamInventoryResult_t pOutResultHandle, byte[] pBuffer, uint unBufferSize, [MarshalAs(UnmanagedType.I1)] bool bRESERVED_MUST_BE_FALSE);

		// Token: 0x06000AEA RID: 2794
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GenerateItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayItemDefs, [In] [Out] uint[] punArrayQuantity, uint unArrayLength);

		// Token: 0x06000AEB RID: 2795
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GrantPromoItems(out SteamInventoryResult_t pResultHandle);

		// Token: 0x06000AEC RID: 2796
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef);

		// Token: 0x06000AED RID: 2797
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_AddPromoItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayItemDefs, uint unArrayLength);

		// Token: 0x06000AEE RID: 2798
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity);

		// Token: 0x06000AEF RID: 2799
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_ExchangeItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayGenerate, [In] [Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [In] [Out] SteamItemInstanceID_t[] pArrayDestroy, [In] [Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);

		// Token: 0x06000AF0 RID: 2800
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest);

		// Token: 0x06000AF1 RID: 2801
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamInventory_SendItemDropHeartbeat();

		// Token: 0x06000AF2 RID: 2802
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition);

		// Token: 0x06000AF3 RID: 2803
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, [In] [Out] SteamItemInstanceID_t[] pArrayGive, [In] [Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [In] [Out] SteamItemInstanceID_t[] pArrayGet, [In] [Out] uint[] pArrayGetQuantity, uint nArrayGetLength);

		// Token: 0x06000AF4 RID: 2804
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_LoadItemDefinitions();

		// Token: 0x06000AF5 RID: 2805
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetItemDefinitionIDs([In] [Out] SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize);

		// Token: 0x06000AF6 RID: 2806
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetItemDefinitionProperty(SteamItemDef_t iDefinition, InteropHelp.UTF8StringHandle pchPropertyName, IntPtr pchValueBuffer, ref uint punValueBufferSizeOut);

		// Token: 0x06000AF7 RID: 2807
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamInventory_RequestEligiblePromoItemDefinitionsIDs(CSteamID steamID);

		// Token: 0x06000AF8 RID: 2808
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetEligiblePromoItemDefinitionIDs(CSteamID steamID, [In] [Out] SteamItemDef_t[] pItemDefIDs, ref uint punItemDefIDsArraySize);

		// Token: 0x06000AF9 RID: 2809
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetFavoriteGameCount();

		// Token: 0x06000AFA RID: 2810
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_GetFavoriteGame(int iGame, out AppId_t pnAppID, out uint pnIP, out ushort pnConnPort, out ushort pnQueryPort, out uint punFlags, out uint pRTime32LastPlayedOnServer);

		// Token: 0x06000AFB RID: 2811
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_AddFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer);

		// Token: 0x06000AFC RID: 2812
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_RemoveFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags);

		// Token: 0x06000AFD RID: 2813
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_RequestLobbyList();

		// Token: 0x06000AFE RID: 2814
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListStringFilter(InteropHelp.UTF8StringHandle pchKeyToMatch, InteropHelp.UTF8StringHandle pchValueToMatch, ELobbyComparison eComparisonType);

		// Token: 0x06000AFF RID: 2815
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListNumericalFilter(InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToMatch, ELobbyComparison eComparisonType);

		// Token: 0x06000B00 RID: 2816
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListNearValueFilter(InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToBeCloseTo);

		// Token: 0x06000B01 RID: 2817
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable);

		// Token: 0x06000B02 RID: 2818
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter eLobbyDistanceFilter);

		// Token: 0x06000B03 RID: 2819
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListResultCountFilter(int cMaxResults);

		// Token: 0x06000B04 RID: 2820
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(CSteamID steamIDLobby);

		// Token: 0x06000B05 RID: 2821
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_GetLobbyByIndex(int iLobby);

		// Token: 0x06000B06 RID: 2822
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_CreateLobby(ELobbyType eLobbyType, int cMaxMembers);

		// Token: 0x06000B07 RID: 2823
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_JoinLobby(CSteamID steamIDLobby);

		// Token: 0x06000B08 RID: 2824
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_LeaveLobby(CSteamID steamIDLobby);

		// Token: 0x06000B09 RID: 2825
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_InviteUserToLobby(CSteamID steamIDLobby, CSteamID steamIDInvitee);

		// Token: 0x06000B0A RID: 2826
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetNumLobbyMembers(CSteamID steamIDLobby);

		// Token: 0x06000B0B RID: 2827
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_GetLobbyMemberByIndex(CSteamID steamIDLobby, int iMember);

		// Token: 0x06000B0C RID: 2828
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmaking_GetLobbyData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000B0D RID: 2829
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000B0E RID: 2830
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetLobbyDataCount(CSteamID steamIDLobby);

		// Token: 0x06000B0F RID: 2831
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_GetLobbyDataByIndex(CSteamID steamIDLobby, int iLobbyData, IntPtr pchKey, int cchKeyBufferSize, IntPtr pchValue, int cchValueBufferSize);

		// Token: 0x06000B10 RID: 2832
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_DeleteLobbyData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000B11 RID: 2833
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmaking_GetLobbyMemberData(CSteamID steamIDLobby, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000B12 RID: 2834
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_SetLobbyMemberData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000B13 RID: 2835
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SendLobbyChatMsg(CSteamID steamIDLobby, byte[] pvMsgBody, int cubMsgBody);

		// Token: 0x06000B14 RID: 2836
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetLobbyChatEntry(CSteamID steamIDLobby, int iChatID, out CSteamID pSteamIDUser, byte[] pvData, int cubData, out EChatEntryType peChatEntryType);

		// Token: 0x06000B15 RID: 2837
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_RequestLobbyData(CSteamID steamIDLobby);

		// Token: 0x06000B16 RID: 2838
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_SetLobbyGameServer(CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort, CSteamID steamIDGameServer);

		// Token: 0x06000B17 RID: 2839
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_GetLobbyGameServer(CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort, out CSteamID psteamIDGameServer);

		// Token: 0x06000B18 RID: 2840
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyMemberLimit(CSteamID steamIDLobby, int cMaxMembers);

		// Token: 0x06000B19 RID: 2841
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetLobbyMemberLimit(CSteamID steamIDLobby);

		// Token: 0x06000B1A RID: 2842
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyType(CSteamID steamIDLobby, ELobbyType eLobbyType);

		// Token: 0x06000B1B RID: 2843
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyJoinable(CSteamID steamIDLobby, [MarshalAs(UnmanagedType.I1)] bool bLobbyJoinable);

		// Token: 0x06000B1C RID: 2844
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_GetLobbyOwner(CSteamID steamIDLobby);

		// Token: 0x06000B1D RID: 2845
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyOwner(CSteamID steamIDLobby, CSteamID steamIDNewOwner);

		// Token: 0x06000B1E RID: 2846
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLinkedLobby(CSteamID steamIDLobby, CSteamID steamIDLobbyDependent);

		// Token: 0x06000B1F RID: 2847
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestInternetServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000B20 RID: 2848
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestLANServerList(AppId_t iApp, IntPtr pRequestServersResponse);

		// Token: 0x06000B21 RID: 2849
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestFriendsServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000B22 RID: 2850
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestFavoritesServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000B23 RID: 2851
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestHistoryServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000B24 RID: 2852
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestSpectatorServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000B25 RID: 2853
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_ReleaseRequest(HServerListRequest hServerListRequest);

		// Token: 0x06000B26 RID: 2854
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_GetServerDetails(HServerListRequest hRequest, int iServer);

		// Token: 0x06000B27 RID: 2855
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_CancelQuery(HServerListRequest hRequest);

		// Token: 0x06000B28 RID: 2856
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_RefreshQuery(HServerListRequest hRequest);

		// Token: 0x06000B29 RID: 2857
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmakingServers_IsRefreshing(HServerListRequest hRequest);

		// Token: 0x06000B2A RID: 2858
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_GetServerCount(HServerListRequest hRequest);

		// Token: 0x06000B2B RID: 2859
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_RefreshServer(HServerListRequest hRequest, int iServer);

		// Token: 0x06000B2C RID: 2860
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_PingServer(uint unIP, ushort usPort, IntPtr pRequestServersResponse);

		// Token: 0x06000B2D RID: 2861
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_PlayerDetails(uint unIP, ushort usPort, IntPtr pRequestServersResponse);

		// Token: 0x06000B2E RID: 2862
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_ServerRules(uint unIP, ushort usPort, IntPtr pRequestServersResponse);

		// Token: 0x06000B2F RID: 2863
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_CancelServerQuery(HServerQuery hServerQuery);

		// Token: 0x06000B30 RID: 2864
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusic_BIsEnabled();

		// Token: 0x06000B31 RID: 2865
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusic_BIsPlaying();

		// Token: 0x06000B32 RID: 2866
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern AudioPlayback_Status ISteamMusic_GetPlaybackStatus();

		// Token: 0x06000B33 RID: 2867
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_Play();

		// Token: 0x06000B34 RID: 2868
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_Pause();

		// Token: 0x06000B35 RID: 2869
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_PlayPrevious();

		// Token: 0x06000B36 RID: 2870
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_PlayNext();

		// Token: 0x06000B37 RID: 2871
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_SetVolume(float flVolume);

		// Token: 0x06000B38 RID: 2872
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ISteamMusic_GetVolume();

		// Token: 0x06000B39 RID: 2873
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_RegisterSteamMusicRemote(InteropHelp.UTF8StringHandle pchName);

		// Token: 0x06000B3A RID: 2874
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_DeregisterSteamMusicRemote();

		// Token: 0x06000B3B RID: 2875
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_BIsCurrentMusicRemote();

		// Token: 0x06000B3C RID: 2876
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_BActivationSuccess([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000B3D RID: 2877
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetDisplayName(InteropHelp.UTF8StringHandle pchDisplayName);

		// Token: 0x06000B3E RID: 2878
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetPNGIcon_64x64(byte[] pvBuffer, uint cbBufferLength);

		// Token: 0x06000B3F RID: 2879
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnablePlayPrevious([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000B40 RID: 2880
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnablePlayNext([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000B41 RID: 2881
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnableShuffled([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000B42 RID: 2882
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnableLooped([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000B43 RID: 2883
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnableQueue([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000B44 RID: 2884
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnablePlaylists([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000B45 RID: 2885
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdatePlaybackStatus(AudioPlayback_Status nStatus);

		// Token: 0x06000B46 RID: 2886
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateShuffled([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000B47 RID: 2887
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateLooped([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000B48 RID: 2888
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateVolume(float flValue);

		// Token: 0x06000B49 RID: 2889
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_CurrentEntryWillChange();

		// Token: 0x06000B4A RID: 2890
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_CurrentEntryIsAvailable([MarshalAs(UnmanagedType.I1)] bool bAvailable);

		// Token: 0x06000B4B RID: 2891
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateCurrentEntryText(InteropHelp.UTF8StringHandle pchText);

		// Token: 0x06000B4C RID: 2892
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(int nValue);

		// Token: 0x06000B4D RID: 2893
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateCurrentEntryCoverArt(byte[] pvBuffer, uint cbBufferLength);

		// Token: 0x06000B4E RID: 2894
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_CurrentEntryDidChange();

		// Token: 0x06000B4F RID: 2895
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_QueueWillChange();

		// Token: 0x06000B50 RID: 2896
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_ResetQueueEntries();

		// Token: 0x06000B51 RID: 2897
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetQueueEntry(int nID, int nPosition, InteropHelp.UTF8StringHandle pchEntryText);

		// Token: 0x06000B52 RID: 2898
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetCurrentQueueEntry(int nID);

		// Token: 0x06000B53 RID: 2899
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_QueueDidChange();

		// Token: 0x06000B54 RID: 2900
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_PlaylistWillChange();

		// Token: 0x06000B55 RID: 2901
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_ResetPlaylistEntries();

		// Token: 0x06000B56 RID: 2902
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetPlaylistEntry(int nID, int nPosition, InteropHelp.UTF8StringHandle pchEntryText);

		// Token: 0x06000B57 RID: 2903
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetCurrentPlaylistEntry(int nID);

		// Token: 0x06000B58 RID: 2904
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_PlaylistDidChange();

		// Token: 0x06000B59 RID: 2905
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_SendP2PPacket(CSteamID steamIDRemote, byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel);

		// Token: 0x06000B5A RID: 2906
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel);

		// Token: 0x06000B5B RID: 2907
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_ReadP2PPacket(byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel);

		// Token: 0x06000B5C RID: 2908
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_AcceptP2PSessionWithUser(CSteamID steamIDRemote);

		// Token: 0x06000B5D RID: 2909
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_CloseP2PSessionWithUser(CSteamID steamIDRemote);

		// Token: 0x06000B5E RID: 2910
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_CloseP2PChannelWithUser(CSteamID steamIDRemote, int nChannel);

		// Token: 0x06000B5F RID: 2911
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_GetP2PSessionState(CSteamID steamIDRemote, out P2PSessionState_t pConnectionState);

		// Token: 0x06000B60 RID: 2912
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_AllowP2PPacketRelay([MarshalAs(UnmanagedType.I1)] bool bAllow);

		// Token: 0x06000B61 RID: 2913
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamNetworking_CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		// Token: 0x06000B62 RID: 2914
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamNetworking_CreateP2PConnectionSocket(CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		// Token: 0x06000B63 RID: 2915
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamNetworking_CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec);

		// Token: 0x06000B64 RID: 2916
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_DestroySocket(SNetSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

		// Token: 0x06000B65 RID: 2917
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_DestroyListenSocket(SNetListenSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

		// Token: 0x06000B66 RID: 2918
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_SendDataOnSocket(SNetSocket_t hSocket, byte[] pubData, uint cubData, [MarshalAs(UnmanagedType.I1)] bool bReliable);

		// Token: 0x06000B67 RID: 2919
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_IsDataAvailableOnSocket(SNetSocket_t hSocket, out uint pcubMsgSize);

		// Token: 0x06000B68 RID: 2920
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_RetrieveDataFromSocket(SNetSocket_t hSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize);

		// Token: 0x06000B69 RID: 2921
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_IsDataAvailable(SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket);

		// Token: 0x06000B6A RID: 2922
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_RetrieveData(SNetListenSocket_t hListenSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket);

		// Token: 0x06000B6B RID: 2923
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_GetSocketInfo(SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote);

		// Token: 0x06000B6C RID: 2924
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_GetListenSocketInfo(SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort);

		// Token: 0x06000B6D RID: 2925
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ESNetSocketConnectionType ISteamNetworking_GetSocketConnectionType(SNetSocket_t hSocket);

		// Token: 0x06000B6E RID: 2926
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamNetworking_GetMaxPacketSize(SNetSocket_t hSocket);

		// Token: 0x06000B6F RID: 2927
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWrite(InteropHelp.UTF8StringHandle pchFile, byte[] pvData, int cubData);

		// Token: 0x06000B70 RID: 2928
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_FileRead(InteropHelp.UTF8StringHandle pchFile, byte[] pvData, int cubDataToRead);

		// Token: 0x06000B71 RID: 2929
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileWriteAsync(InteropHelp.UTF8StringHandle pchFile, byte[] pvData, uint cubData);

		// Token: 0x06000B72 RID: 2930
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileReadAsync(InteropHelp.UTF8StringHandle pchFile, uint nOffset, uint cubToRead);

		// Token: 0x06000B73 RID: 2931
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileReadAsyncComplete(SteamAPICall_t hReadCall, byte[] pvBuffer, uint cubToRead);

		// Token: 0x06000B74 RID: 2932
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileForget(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B75 RID: 2933
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileDelete(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B76 RID: 2934
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileShare(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B77 RID: 2935
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_SetSyncPlatforms(InteropHelp.UTF8StringHandle pchFile, ERemoteStoragePlatform eRemoteStoragePlatform);

		// Token: 0x06000B78 RID: 2936
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileWriteStreamOpen(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B79 RID: 2937
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWriteStreamWriteChunk(UGCFileWriteStreamHandle_t writeHandle, byte[] pvData, int cubData);

		// Token: 0x06000B7A RID: 2938
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWriteStreamClose(UGCFileWriteStreamHandle_t writeHandle);

		// Token: 0x06000B7B RID: 2939
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWriteStreamCancel(UGCFileWriteStreamHandle_t writeHandle);

		// Token: 0x06000B7C RID: 2940
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileExists(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B7D RID: 2941
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FilePersisted(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B7E RID: 2942
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_GetFileSize(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B7F RID: 2943
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern long ISteamRemoteStorage_GetFileTimestamp(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B80 RID: 2944
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ERemoteStoragePlatform ISteamRemoteStorage_GetSyncPlatforms(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B81 RID: 2945
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_GetFileCount();

		// Token: 0x06000B82 RID: 2946
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamRemoteStorage_GetFileNameAndSize(int iFile, out int pnFileSizeInBytes);

		// Token: 0x06000B83 RID: 2947
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_GetQuota(out ulong pnTotalBytes, out ulong puAvailableBytes);

		// Token: 0x06000B84 RID: 2948
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_IsCloudEnabledForAccount();

		// Token: 0x06000B85 RID: 2949
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_IsCloudEnabledForApp();

		// Token: 0x06000B86 RID: 2950
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamRemoteStorage_SetCloudEnabledForApp([MarshalAs(UnmanagedType.I1)] bool bEnabled);

		// Token: 0x06000B87 RID: 2951
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UGCDownload(UGCHandle_t hContent, uint unPriority);

		// Token: 0x06000B88 RID: 2952
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_GetUGCDownloadProgress(UGCHandle_t hContent, out int pnBytesDownloaded, out int pnBytesExpected);

		// Token: 0x06000B89 RID: 2953
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_GetUGCDetails(UGCHandle_t hContent, out AppId_t pnAppID, out IntPtr ppchName, out int pnFileSizeInBytes, out CSteamID pSteamIDOwner);

		// Token: 0x06000B8A RID: 2954
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_UGCRead(UGCHandle_t hContent, byte[] pvData, int cubDataToRead, uint cOffset, EUGCReadAction eAction);

		// Token: 0x06000B8B RID: 2955
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_GetCachedUGCCount();

		// Token: 0x06000B8C RID: 2956
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetCachedUGCHandle(int iCachedContent);

		// Token: 0x06000B8D RID: 2957
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_PublishWorkshopFile(InteropHelp.UTF8StringHandle pchFile, InteropHelp.UTF8StringHandle pchPreviewFile, AppId_t nConsumerAppId, InteropHelp.UTF8StringHandle pchTitle, InteropHelp.UTF8StringHandle pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IntPtr pTags, EWorkshopFileType eWorkshopFileType);

		// Token: 0x06000B8E RID: 2958
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_CreatePublishedFileUpdateRequest(PublishedFileId_t unPublishedFileId);

		// Token: 0x06000B8F RID: 2959
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileFile(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x06000B90 RID: 2960
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFilePreviewFile(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchPreviewFile);

		// Token: 0x06000B91 RID: 2961
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileTitle(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchTitle);

		// Token: 0x06000B92 RID: 2962
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileDescription(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchDescription);

		// Token: 0x06000B93 RID: 2963
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileVisibility(PublishedFileUpdateHandle_t updateHandle, ERemoteStoragePublishedFileVisibility eVisibility);

		// Token: 0x06000B94 RID: 2964
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileTags(PublishedFileUpdateHandle_t updateHandle, IntPtr pTags);

		// Token: 0x06000B95 RID: 2965
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_CommitPublishedFileUpdate(PublishedFileUpdateHandle_t updateHandle);

		// Token: 0x06000B96 RID: 2966
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetPublishedFileDetails(PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld);

		// Token: 0x06000B97 RID: 2967
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_DeletePublishedFile(PublishedFileId_t unPublishedFileId);

		// Token: 0x06000B98 RID: 2968
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumerateUserPublishedFiles(uint unStartIndex);

		// Token: 0x06000B99 RID: 2969
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_SubscribePublishedFile(PublishedFileId_t unPublishedFileId);

		// Token: 0x06000B9A RID: 2970
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumerateUserSubscribedFiles(uint unStartIndex);

		// Token: 0x06000B9B RID: 2971
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UnsubscribePublishedFile(PublishedFileId_t unPublishedFileId);

		// Token: 0x06000B9C RID: 2972
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchChangeDescription);

		// Token: 0x06000B9D RID: 2973
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId);

		// Token: 0x06000B9E RID: 2974
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UpdateUserPublishedItemVote(PublishedFileId_t unPublishedFileId, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

		// Token: 0x06000B9F RID: 2975
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetUserPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId);

		// Token: 0x06000BA0 RID: 2976
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(CSteamID steamId, uint unStartIndex, IntPtr pRequiredTags, IntPtr pExcludedTags);

		// Token: 0x06000BA1 RID: 2977
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_PublishVideo(EWorkshopVideoProvider eVideoProvider, InteropHelp.UTF8StringHandle pchVideoAccount, InteropHelp.UTF8StringHandle pchVideoIdentifier, InteropHelp.UTF8StringHandle pchPreviewFile, AppId_t nConsumerAppId, InteropHelp.UTF8StringHandle pchTitle, InteropHelp.UTF8StringHandle pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IntPtr pTags);

		// Token: 0x06000BA2 RID: 2978
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_SetUserPublishedFileAction(PublishedFileId_t unPublishedFileId, EWorkshopFileAction eAction);

		// Token: 0x06000BA3 RID: 2979
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(EWorkshopFileAction eAction, uint unStartIndex);

		// Token: 0x06000BA4 RID: 2980
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, IntPtr pTags, IntPtr pUserTags);

		// Token: 0x06000BA5 RID: 2981
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UGCDownloadToLocation(UGCHandle_t hContent, InteropHelp.UTF8StringHandle pchLocation, uint unPriority);

		// Token: 0x06000BA6 RID: 2982
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamScreenshots_WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight);

		// Token: 0x06000BA7 RID: 2983
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamScreenshots_AddScreenshotToLibrary(InteropHelp.UTF8StringHandle pchFilename, InteropHelp.UTF8StringHandle pchThumbnailFilename, int nWidth, int nHeight);

		// Token: 0x06000BA8 RID: 2984
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamScreenshots_TriggerScreenshot();

		// Token: 0x06000BA9 RID: 2985
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamScreenshots_HookScreenshots([MarshalAs(UnmanagedType.I1)] bool bHook);

		// Token: 0x06000BAA RID: 2986
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_SetLocation(ScreenshotHandle hScreenshot, InteropHelp.UTF8StringHandle pchLocation);

		// Token: 0x06000BAB RID: 2987
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_TagUser(ScreenshotHandle hScreenshot, CSteamID steamID);

		// Token: 0x06000BAC RID: 2988
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID);

		// Token: 0x06000BAD RID: 2989
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_IsScreenshotsHooked();

		// Token: 0x06000BAE RID: 2990
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamScreenshots_AddVRScreenshotToLibrary(EVRScreenshotType eType, InteropHelp.UTF8StringHandle pchFilename, InteropHelp.UTF8StringHandle pchVRFilename);

		// Token: 0x06000BAF RID: 2991
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		// Token: 0x06000BB0 RID: 2992
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		// Token: 0x06000BB1 RID: 2993
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateQueryUGCDetailsRequest([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		// Token: 0x06000BB2 RID: 2994
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SendQueryUGCRequest(UGCQueryHandle_t handle);

		// Token: 0x06000BB3 RID: 2995
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails);

		// Token: 0x06000BB4 RID: 2996
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, IntPtr pchURL, uint cchURLSize);

		// Token: 0x06000BB5 RID: 2997
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, IntPtr pchMetadata, uint cchMetadatasize);

		// Token: 0x06000BB6 RID: 2998
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, [In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		// Token: 0x06000BB7 RID: 2999
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue);

		// Token: 0x06000BB8 RID: 3000
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index);

		// Token: 0x06000BB9 RID: 3001
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, IntPtr pchURLOrVideoID, uint cchURLSize, IntPtr pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType);

		// Token: 0x06000BBA RID: 3002
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index);

		// Token: 0x06000BBB RID: 3003
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, IntPtr pchKey, uint cchKeySize, IntPtr pchValue, uint cchValueSize);

		// Token: 0x06000BBC RID: 3004
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_ReleaseQueryUGCRequest(UGCQueryHandle_t handle);

		// Token: 0x06000BBD RID: 3005
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddRequiredTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

		// Token: 0x06000BBE RID: 3006
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddExcludedTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

		// Token: 0x06000BBF RID: 3007
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnOnlyIDs(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnOnlyIDs);

		// Token: 0x06000BC0 RID: 3008
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnKeyValueTags(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnKeyValueTags);

		// Token: 0x06000BC1 RID: 3009
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnLongDescription(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnLongDescription);

		// Token: 0x06000BC2 RID: 3010
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnMetadata(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnMetadata);

		// Token: 0x06000BC3 RID: 3011
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnChildren(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnChildren);

		// Token: 0x06000BC4 RID: 3012
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnAdditionalPreviews(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnAdditionalPreviews);

		// Token: 0x06000BC5 RID: 3013
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnTotalOnly(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnTotalOnly);

		// Token: 0x06000BC6 RID: 3014
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays);

		// Token: 0x06000BC7 RID: 3015
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetLanguage(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

		// Token: 0x06000BC8 RID: 3016
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds);

		// Token: 0x06000BC9 RID: 3017
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetCloudFileNameFilter(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pMatchCloudFileName);

		// Token: 0x06000BCA RID: 3018
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetMatchAnyTag(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bMatchAnyTag);

		// Token: 0x06000BCB RID: 3019
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetSearchText(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pSearchText);

		// Token: 0x06000BCC RID: 3020
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays);

		// Token: 0x06000BCD RID: 3021
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddRequiredKeyValueTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

		// Token: 0x06000BCE RID: 3022
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

		// Token: 0x06000BCF RID: 3023
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType);

		// Token: 0x06000BD0 RID: 3024
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x06000BD1 RID: 3025
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemTitle(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchTitle);

		// Token: 0x06000BD2 RID: 3026
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemDescription(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchDescription);

		// Token: 0x06000BD3 RID: 3027
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemUpdateLanguage(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

		// Token: 0x06000BD4 RID: 3028
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemMetadata(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchMetaData);

		// Token: 0x06000BD5 RID: 3029
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility);

		// Token: 0x06000BD6 RID: 3030
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemTags(UGCUpdateHandle_t updateHandle, IntPtr pTags);

		// Token: 0x06000BD7 RID: 3031
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemContent(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszContentFolder);

		// Token: 0x06000BD8 RID: 3032
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemPreview(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile);

		// Token: 0x06000BD9 RID: 3033
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_RemoveItemKeyValueTags(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000BDA RID: 3034
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddItemKeyValueTag(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000BDB RID: 3035
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddItemPreviewFile(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile, EItemPreviewType type);

		// Token: 0x06000BDC RID: 3036
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddItemPreviewVideo(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszVideoID);

		// Token: 0x06000BDD RID: 3037
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, InteropHelp.UTF8StringHandle pszPreviewFile);

		// Token: 0x06000BDE RID: 3038
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, InteropHelp.UTF8StringHandle pszVideoID);

		// Token: 0x06000BDF RID: 3039
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_RemoveItemPreview(UGCUpdateHandle_t handle, uint index);

		// Token: 0x06000BE0 RID: 3040
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SubmitItemUpdate(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchChangeNote);

		// Token: 0x06000BE1 RID: 3041
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EItemUpdateStatus ISteamUGC_GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

		// Token: 0x06000BE2 RID: 3042
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SetUserItemVote(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

		// Token: 0x06000BE3 RID: 3043
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_GetUserItemVote(PublishedFileId_t nPublishedFileID);

		// Token: 0x06000BE4 RID: 3044
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x06000BE5 RID: 3045
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x06000BE6 RID: 3046
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SubscribeItem(PublishedFileId_t nPublishedFileID);

		// Token: 0x06000BE7 RID: 3047
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_UnsubscribeItem(PublishedFileId_t nPublishedFileID);

		// Token: 0x06000BE8 RID: 3048
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetNumSubscribedItems();

		// Token: 0x06000BE9 RID: 3049
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetSubscribedItems([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		// Token: 0x06000BEA RID: 3050
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetItemState(PublishedFileId_t nPublishedFileID);

		// Token: 0x06000BEB RID: 3051
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, IntPtr pchFolder, uint cchFolderSize, out uint punTimeStamp);

		// Token: 0x06000BEC RID: 3052
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		// Token: 0x06000BED RID: 3053
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_DownloadItem(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bHighPriority);

		// Token: 0x06000BEE RID: 3054
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, InteropHelp.UTF8StringHandle pszFolder);

		// Token: 0x06000BEF RID: 3055
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUGC_SuspendDownloads([MarshalAs(UnmanagedType.I1)] bool bSuspend);

		// Token: 0x06000BF0 RID: 3056
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_StartPlaytimeTracking([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		// Token: 0x06000BF1 RID: 3057
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_StopPlaytimeTracking([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		// Token: 0x06000BF2 RID: 3058
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_StopPlaytimeTrackingForAllItems();

		// Token: 0x06000BF3 RID: 3059
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_AddDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID);

		// Token: 0x06000BF4 RID: 3060
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_RemoveDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID);

		// Token: 0x06000BF5 RID: 3061
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUnifiedMessages_SendMethod(InteropHelp.UTF8StringHandle pchServiceMethod, byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext);

		// Token: 0x06000BF6 RID: 3062
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_GetMethodResponseInfo(ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out EResult peResult);

		// Token: 0x06000BF7 RID: 3063
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_GetMethodResponseData(ClientUnifiedMessageHandle hHandle, byte[] pResponseBuffer, uint unResponseBufferSize, [MarshalAs(UnmanagedType.I1)] bool bAutoRelease);

		// Token: 0x06000BF8 RID: 3064
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_ReleaseMethod(ClientUnifiedMessageHandle hHandle);

		// Token: 0x06000BF9 RID: 3065
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_SendNotification(InteropHelp.UTF8StringHandle pchServiceNotification, byte[] pNotificationBuffer, uint unNotificationBufferSize);

		// Token: 0x06000BFA RID: 3066
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUser_GetHSteamUser();

		// Token: 0x06000BFB RID: 3067
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_BLoggedOn();

		// Token: 0x06000BFC RID: 3068
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUser_GetSteamID();

		// Token: 0x06000BFD RID: 3069
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUser_InitiateGameConnection(byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, [MarshalAs(UnmanagedType.I1)] bool bSecure);

		// Token: 0x06000BFE RID: 3070
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_TerminateGameConnection(uint unIPServer, ushort usPortServer);

		// Token: 0x06000BFF RID: 3071
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, InteropHelp.UTF8StringHandle pchExtraInfo);

		// Token: 0x06000C00 RID: 3072
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_GetUserDataFolder(IntPtr pchBuffer, int cubBuffer);

		// Token: 0x06000C01 RID: 3073
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_StartVoiceRecording();

		// Token: 0x06000C02 RID: 3074
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_StopVoiceRecording();

		// Token: 0x06000C03 RID: 3075
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EVoiceResult ISteamUser_GetAvailableVoice(out uint pcbCompressed, IntPtr pcbUncompressed_Deprecated, uint nUncompressedVoiceDesiredSampleRate_Deprecated);

		// Token: 0x06000C04 RID: 3076
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EVoiceResult ISteamUser_GetVoice([MarshalAs(UnmanagedType.I1)] bool bWantCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, [MarshalAs(UnmanagedType.I1)] bool bWantUncompressed_Deprecated, IntPtr pUncompressedDestBuffer_Deprecated, uint cbUncompressedDestBufferSize_Deprecated, IntPtr nUncompressBytesWritten_Deprecated, uint nUncompressedVoiceDesiredSampleRate_Deprecated);

		// Token: 0x06000C05 RID: 3077
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EVoiceResult ISteamUser_DecompressVoice(byte[] pCompressed, uint cbCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate);

		// Token: 0x06000C06 RID: 3078
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUser_GetVoiceOptimalSampleRate();

		// Token: 0x06000C07 RID: 3079
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUser_GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

		// Token: 0x06000C08 RID: 3080
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EBeginAuthSessionResult ISteamUser_BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID);

		// Token: 0x06000C09 RID: 3081
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_EndAuthSession(CSteamID steamID);

		// Token: 0x06000C0A RID: 3082
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_CancelAuthTicket(HAuthTicket hAuthTicket);

		// Token: 0x06000C0B RID: 3083
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EUserHasLicenseForAppResult ISteamUser_UserHasLicenseForApp(CSteamID steamID, AppId_t appID);

		// Token: 0x06000C0C RID: 3084
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_BIsBehindNAT();

		// Token: 0x06000C0D RID: 3085
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_AdvertiseGame(CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer);

		// Token: 0x06000C0E RID: 3086
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUser_RequestEncryptedAppTicket(byte[] pDataToInclude, int cbDataToInclude);

		// Token: 0x06000C0F RID: 3087
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

		// Token: 0x06000C10 RID: 3088
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUser_GetGameBadgeLevel(int nSeries, [MarshalAs(UnmanagedType.I1)] bool bFoil);

		// Token: 0x06000C11 RID: 3089
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUser_GetPlayerSteamLevel();

		// Token: 0x06000C12 RID: 3090
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUser_RequestStoreAuthURL(InteropHelp.UTF8StringHandle pchRedirectURL);

		// Token: 0x06000C13 RID: 3091
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_BIsPhoneVerified();

		// Token: 0x06000C14 RID: 3092
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_BIsTwoFactorEnabled();

		// Token: 0x06000C15 RID: 3093
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_BIsPhoneIdentifying();

		// Token: 0x06000C16 RID: 3094
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_BIsPhoneRequiringVerification();

		// Token: 0x06000C17 RID: 3095
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_RequestCurrentStats();

		// Token: 0x06000C18 RID: 3096
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetStat(InteropHelp.UTF8StringHandle pchName, out int pData);

		// Token: 0x06000C19 RID: 3097
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetStat_(InteropHelp.UTF8StringHandle pchName, out float pData);

		// Token: 0x06000C1A RID: 3098
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_SetStat(InteropHelp.UTF8StringHandle pchName, int nData);

		// Token: 0x06000C1B RID: 3099
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_SetStat_(InteropHelp.UTF8StringHandle pchName, float fData);

		// Token: 0x06000C1C RID: 3100
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_UpdateAvgRateStat(InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

		// Token: 0x06000C1D RID: 3101
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetAchievement(InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

		// Token: 0x06000C1E RID: 3102
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_SetAchievement(InteropHelp.UTF8StringHandle pchName);

		// Token: 0x06000C1F RID: 3103
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_ClearAchievement(InteropHelp.UTF8StringHandle pchName);

		// Token: 0x06000C20 RID: 3104
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetAchievementAndUnlockTime(InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

		// Token: 0x06000C21 RID: 3105
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_StoreStats();

		// Token: 0x06000C22 RID: 3106
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetAchievementIcon(InteropHelp.UTF8StringHandle pchName);

		// Token: 0x06000C23 RID: 3107
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUserStats_GetAchievementDisplayAttribute(InteropHelp.UTF8StringHandle pchName, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000C24 RID: 3108
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_IndicateAchievementProgress(InteropHelp.UTF8StringHandle pchName, uint nCurProgress, uint nMaxProgress);

		// Token: 0x06000C25 RID: 3109
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUserStats_GetNumAchievements();

		// Token: 0x06000C26 RID: 3110
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUserStats_GetAchievementName(uint iAchievement);

		// Token: 0x06000C27 RID: 3111
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_RequestUserStats(CSteamID steamIDUser);

		// Token: 0x06000C28 RID: 3112
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out int pData);

		// Token: 0x06000C29 RID: 3113
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserStat_(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out float pData);

		// Token: 0x06000C2A RID: 3114
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

		// Token: 0x06000C2B RID: 3115
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserAchievementAndUnlockTime(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

		// Token: 0x06000C2C RID: 3116
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_ResetAllStats([MarshalAs(UnmanagedType.I1)] bool bAchievementsToo);

		// Token: 0x06000C2D RID: 3117
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_FindOrCreateLeaderboard(InteropHelp.UTF8StringHandle pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType);

		// Token: 0x06000C2E RID: 3118
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_FindLeaderboard(InteropHelp.UTF8StringHandle pchLeaderboardName);

		// Token: 0x06000C2F RID: 3119
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUserStats_GetLeaderboardName(SteamLeaderboard_t hSteamLeaderboard);

		// Token: 0x06000C30 RID: 3120
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetLeaderboardEntryCount(SteamLeaderboard_t hSteamLeaderboard);

		// Token: 0x06000C31 RID: 3121
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ELeaderboardSortMethod ISteamUserStats_GetLeaderboardSortMethod(SteamLeaderboard_t hSteamLeaderboard);

		// Token: 0x06000C32 RID: 3122
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ELeaderboardDisplayType ISteamUserStats_GetLeaderboardDisplayType(SteamLeaderboard_t hSteamLeaderboard);

		// Token: 0x06000C33 RID: 3123
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_DownloadLeaderboardEntries(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd);

		// Token: 0x06000C34 RID: 3124
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_DownloadLeaderboardEntriesForUsers(SteamLeaderboard_t hSteamLeaderboard, [In] [Out] CSteamID[] prgUsers, int cUsers);

		// Token: 0x06000C35 RID: 3125
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetDownloadedLeaderboardEntry(SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out LeaderboardEntry_t pLeaderboardEntry, [In] [Out] int[] pDetails, int cDetailsMax);

		// Token: 0x06000C36 RID: 3126
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_UploadLeaderboardScore(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, [In] [Out] int[] pScoreDetails, int cScoreDetailsCount);

		// Token: 0x06000C37 RID: 3127
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_AttachLeaderboardUGC(SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC);

		// Token: 0x06000C38 RID: 3128
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_GetNumberOfCurrentPlayers();

		// Token: 0x06000C39 RID: 3129
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_RequestGlobalAchievementPercentages();

		// Token: 0x06000C3A RID: 3130
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetMostAchievedAchievementInfo(IntPtr pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

		// Token: 0x06000C3B RID: 3131
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetNextMostAchievedAchievementInfo(int iIteratorPrevious, IntPtr pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

		// Token: 0x06000C3C RID: 3132
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetAchievementAchievedPercent(InteropHelp.UTF8StringHandle pchName, out float pflPercent);

		// Token: 0x06000C3D RID: 3133
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_RequestGlobalStats(int nHistoryDays);

		// Token: 0x06000C3E RID: 3134
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetGlobalStat(InteropHelp.UTF8StringHandle pchStatName, out long pData);

		// Token: 0x06000C3F RID: 3135
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetGlobalStat_(InteropHelp.UTF8StringHandle pchStatName, out double pData);

		// Token: 0x06000C40 RID: 3136
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetGlobalStatHistory(InteropHelp.UTF8StringHandle pchStatName, [In] [Out] long[] pData, uint cubData);

		// Token: 0x06000C41 RID: 3137
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetGlobalStatHistory_(InteropHelp.UTF8StringHandle pchStatName, [In] [Out] double[] pData, uint cubData);

		// Token: 0x06000C42 RID: 3138
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetSecondsSinceAppActive();

		// Token: 0x06000C43 RID: 3139
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetSecondsSinceComputerActive();

		// Token: 0x06000C44 RID: 3140
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EUniverse ISteamUtils_GetConnectedUniverse();

		// Token: 0x06000C45 RID: 3141
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetServerRealTime();

		// Token: 0x06000C46 RID: 3142
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUtils_GetIPCountry();

		// Token: 0x06000C47 RID: 3143
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetImageSize(int iImage, out uint pnWidth, out uint pnHeight);

		// Token: 0x06000C48 RID: 3144
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize);

		// Token: 0x06000C49 RID: 3145
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetCSERIPPort(out uint unIP, out ushort usPort);

		// Token: 0x06000C4A RID: 3146
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte ISteamUtils_GetCurrentBatteryPower();

		// Token: 0x06000C4B RID: 3147
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetAppID();

		// Token: 0x06000C4C RID: 3148
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition);

		// Token: 0x06000C4D RID: 3149
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed);

		// Token: 0x06000C4E RID: 3150
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ESteamAPICallFailure ISteamUtils_GetAPICallFailureReason(SteamAPICall_t hSteamAPICall);

		// Token: 0x06000C4F RID: 3151
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

		// Token: 0x06000C50 RID: 3152
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetIPCCallCount();

		// Token: 0x06000C51 RID: 3153
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction);

		// Token: 0x06000C52 RID: 3154
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsOverlayEnabled();

		// Token: 0x06000C53 RID: 3155
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_BOverlayNeedsPresent();

		// Token: 0x06000C54 RID: 3156
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUtils_CheckFileSignature(InteropHelp.UTF8StringHandle szFileName);

		// Token: 0x06000C55 RID: 3157
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, InteropHelp.UTF8StringHandle pchDescription, uint unCharMax, InteropHelp.UTF8StringHandle pchExistingText);

		// Token: 0x06000C56 RID: 3158
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetEnteredGamepadTextLength();

		// Token: 0x06000C57 RID: 3159
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetEnteredGamepadTextInput(IntPtr pchText, uint cchText);

		// Token: 0x06000C58 RID: 3160
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUtils_GetSteamUILanguage();

		// Token: 0x06000C59 RID: 3161
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsSteamRunningInVR();

		// Token: 0x06000C5A RID: 3162
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset);

		// Token: 0x06000C5B RID: 3163
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsSteamInBigPictureMode();

		// Token: 0x06000C5C RID: 3164
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUtils_StartVRDashboard();

		// Token: 0x06000C5D RID: 3165
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsVRHeadsetStreamingEnabled();

		// Token: 0x06000C5E RID: 3166
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetVRHeadsetStreamingEnabled([MarshalAs(UnmanagedType.I1)] bool bEnabled);

		// Token: 0x06000C5F RID: 3167
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamVideo_GetVideoURL(AppId_t unVideoAppID);

		// Token: 0x06000C60 RID: 3168
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamVideo_IsBroadcasting(out int pnNumViewers);

		// Token: 0x06000C61 RID: 3169
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamVideo_GetOPFSettings(AppId_t unVideoAppID);

		// Token: 0x06000C62 RID: 3170
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamVideo_GetOPFStringForApp(AppId_t unVideoAppID, IntPtr pchBuffer, ref int pnBufferSize);

		// Token: 0x06000C63 RID: 3171
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerHTTP_CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, InteropHelp.UTF8StringHandle pchAbsoluteURL);

		// Token: 0x06000C64 RID: 3172
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue);

		// Token: 0x06000C65 RID: 3173
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds);

		// Token: 0x06000C66 RID: 3174
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, InteropHelp.UTF8StringHandle pchHeaderValue);

		// Token: 0x06000C67 RID: 3175
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchParamName, InteropHelp.UTF8StringHandle pchParamValue);

		// Token: 0x06000C68 RID: 3176
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

		// Token: 0x06000C69 RID: 3177
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

		// Token: 0x06000C6A RID: 3178
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_DeferHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000C6B RID: 3179
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_PrioritizeHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000C6C RID: 3180
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, out uint unResponseHeaderSize);

		// Token: 0x06000C6D RID: 3181
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, byte[] pHeaderValueBuffer, uint unBufferSize);

		// Token: 0x06000C6E RID: 3182
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize);

		// Token: 0x06000C6F RID: 3183
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseBodyData(HTTPRequestHandle hRequest, byte[] pBodyDataBuffer, uint unBufferSize);

		// Token: 0x06000C70 RID: 3184
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, byte[] pBodyDataBuffer, uint unBufferSize);

		// Token: 0x06000C71 RID: 3185
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_ReleaseHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000C72 RID: 3186
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut);

		// Token: 0x06000C73 RID: 3187
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchContentType, byte[] pubBody, uint unBodyLen);

		// Token: 0x06000C74 RID: 3188
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerHTTP_CreateCookieContainer([MarshalAs(UnmanagedType.I1)] bool bAllowResponsesToModify);

		// Token: 0x06000C75 RID: 3189
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer);

		// Token: 0x06000C76 RID: 3190
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetCookie(HTTPCookieContainerHandle hCookieContainer, InteropHelp.UTF8StringHandle pchHost, InteropHelp.UTF8StringHandle pchUrl, InteropHelp.UTF8StringHandle pchCookie);

		// Token: 0x06000C77 RID: 3191
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer);

		// Token: 0x06000C78 RID: 3192
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchUserAgentInfo);

		// Token: 0x06000C79 RID: 3193
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, [MarshalAs(UnmanagedType.I1)] bool bRequireVerifiedCertificate);

		// Token: 0x06000C7A RID: 3194
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds);

		// Token: 0x06000C7B RID: 3195
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut);

		// Token: 0x06000C7C RID: 3196
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EResult ISteamGameServerInventory_GetResultStatus(SteamInventoryResult_t resultHandle);

		// Token: 0x06000C7D RID: 3197
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetResultItems(SteamInventoryResult_t resultHandle, [In] [Out] SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);

		// Token: 0x06000C7E RID: 3198
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetResultItemProperty(SteamInventoryResult_t resultHandle, uint unItemIndex, InteropHelp.UTF8StringHandle pchPropertyName, IntPtr pchValueBuffer, ref uint punValueBufferSizeOut);

		// Token: 0x06000C7F RID: 3199
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerInventory_GetResultTimestamp(SteamInventoryResult_t resultHandle);

		// Token: 0x06000C80 RID: 3200
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected);

		// Token: 0x06000C81 RID: 3201
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerInventory_DestroyResult(SteamInventoryResult_t resultHandle);

		// Token: 0x06000C82 RID: 3202
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetAllItems(out SteamInventoryResult_t pResultHandle);

		// Token: 0x06000C83 RID: 3203
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetItemsByID(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs);

		// Token: 0x06000C84 RID: 3204
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_SerializeResult(SteamInventoryResult_t resultHandle, byte[] pOutBuffer, out uint punOutBufferSize);

		// Token: 0x06000C85 RID: 3205
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_DeserializeResult(out SteamInventoryResult_t pOutResultHandle, byte[] pBuffer, uint unBufferSize, [MarshalAs(UnmanagedType.I1)] bool bRESERVED_MUST_BE_FALSE);

		// Token: 0x06000C86 RID: 3206
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GenerateItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayItemDefs, [In] [Out] uint[] punArrayQuantity, uint unArrayLength);

		// Token: 0x06000C87 RID: 3207
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GrantPromoItems(out SteamInventoryResult_t pResultHandle);

		// Token: 0x06000C88 RID: 3208
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef);

		// Token: 0x06000C89 RID: 3209
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_AddPromoItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayItemDefs, uint unArrayLength);

		// Token: 0x06000C8A RID: 3210
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity);

		// Token: 0x06000C8B RID: 3211
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_ExchangeItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayGenerate, [In] [Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [In] [Out] SteamItemInstanceID_t[] pArrayDestroy, [In] [Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);

		// Token: 0x06000C8C RID: 3212
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest);

		// Token: 0x06000C8D RID: 3213
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerInventory_SendItemDropHeartbeat();

		// Token: 0x06000C8E RID: 3214
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition);

		// Token: 0x06000C8F RID: 3215
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, [In] [Out] SteamItemInstanceID_t[] pArrayGive, [In] [Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [In] [Out] SteamItemInstanceID_t[] pArrayGet, [In] [Out] uint[] pArrayGetQuantity, uint nArrayGetLength);

		// Token: 0x06000C90 RID: 3216
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_LoadItemDefinitions();

		// Token: 0x06000C91 RID: 3217
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetItemDefinitionIDs([In] [Out] SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize);

		// Token: 0x06000C92 RID: 3218
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetItemDefinitionProperty(SteamItemDef_t iDefinition, InteropHelp.UTF8StringHandle pchPropertyName, IntPtr pchValueBuffer, ref uint punValueBufferSizeOut);

		// Token: 0x06000C93 RID: 3219
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerInventory_RequestEligiblePromoItemDefinitionsIDs(CSteamID steamID);

		// Token: 0x06000C94 RID: 3220
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetEligiblePromoItemDefinitionIDs(CSteamID steamID, [In] [Out] SteamItemDef_t[] pItemDefIDs, ref uint punItemDefIDsArraySize);

		// Token: 0x06000C95 RID: 3221
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_SendP2PPacket(CSteamID steamIDRemote, byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel);

		// Token: 0x06000C96 RID: 3222
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel);

		// Token: 0x06000C97 RID: 3223
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_ReadP2PPacket(byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel);

		// Token: 0x06000C98 RID: 3224
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_AcceptP2PSessionWithUser(CSteamID steamIDRemote);

		// Token: 0x06000C99 RID: 3225
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_CloseP2PSessionWithUser(CSteamID steamIDRemote);

		// Token: 0x06000C9A RID: 3226
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_CloseP2PChannelWithUser(CSteamID steamIDRemote, int nChannel);

		// Token: 0x06000C9B RID: 3227
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_GetP2PSessionState(CSteamID steamIDRemote, out P2PSessionState_t pConnectionState);

		// Token: 0x06000C9C RID: 3228
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_AllowP2PPacketRelay([MarshalAs(UnmanagedType.I1)] bool bAllow);

		// Token: 0x06000C9D RID: 3229
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerNetworking_CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		// Token: 0x06000C9E RID: 3230
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerNetworking_CreateP2PConnectionSocket(CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		// Token: 0x06000C9F RID: 3231
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerNetworking_CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec);

		// Token: 0x06000CA0 RID: 3232
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_DestroySocket(SNetSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

		// Token: 0x06000CA1 RID: 3233
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_DestroyListenSocket(SNetListenSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

		// Token: 0x06000CA2 RID: 3234
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_SendDataOnSocket(SNetSocket_t hSocket, byte[] pubData, uint cubData, [MarshalAs(UnmanagedType.I1)] bool bReliable);

		// Token: 0x06000CA3 RID: 3235
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_IsDataAvailableOnSocket(SNetSocket_t hSocket, out uint pcubMsgSize);

		// Token: 0x06000CA4 RID: 3236
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_RetrieveDataFromSocket(SNetSocket_t hSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize);

		// Token: 0x06000CA5 RID: 3237
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_IsDataAvailable(SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket);

		// Token: 0x06000CA6 RID: 3238
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_RetrieveData(SNetListenSocket_t hListenSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket);

		// Token: 0x06000CA7 RID: 3239
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_GetSocketInfo(SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote);

		// Token: 0x06000CA8 RID: 3240
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_GetListenSocketInfo(SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort);

		// Token: 0x06000CA9 RID: 3241
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ESNetSocketConnectionType ISteamGameServerNetworking_GetSocketConnectionType(SNetSocket_t hSocket);

		// Token: 0x06000CAA RID: 3242
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamGameServerNetworking_GetMaxPacketSize(SNetSocket_t hSocket);

		// Token: 0x06000CAB RID: 3243
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		// Token: 0x06000CAC RID: 3244
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		// Token: 0x06000CAD RID: 3245
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateQueryUGCDetailsRequest([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		// Token: 0x06000CAE RID: 3246
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SendQueryUGCRequest(UGCQueryHandle_t handle);

		// Token: 0x06000CAF RID: 3247
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails);

		// Token: 0x06000CB0 RID: 3248
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, IntPtr pchURL, uint cchURLSize);

		// Token: 0x06000CB1 RID: 3249
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, IntPtr pchMetadata, uint cchMetadatasize);

		// Token: 0x06000CB2 RID: 3250
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, [In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		// Token: 0x06000CB3 RID: 3251
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue);

		// Token: 0x06000CB4 RID: 3252
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index);

		// Token: 0x06000CB5 RID: 3253
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, IntPtr pchURLOrVideoID, uint cchURLSize, IntPtr pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType);

		// Token: 0x06000CB6 RID: 3254
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index);

		// Token: 0x06000CB7 RID: 3255
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, IntPtr pchKey, uint cchKeySize, IntPtr pchValue, uint cchValueSize);

		// Token: 0x06000CB8 RID: 3256
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_ReleaseQueryUGCRequest(UGCQueryHandle_t handle);

		// Token: 0x06000CB9 RID: 3257
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddRequiredTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

		// Token: 0x06000CBA RID: 3258
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddExcludedTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

		// Token: 0x06000CBB RID: 3259
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnOnlyIDs(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnOnlyIDs);

		// Token: 0x06000CBC RID: 3260
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnKeyValueTags(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnKeyValueTags);

		// Token: 0x06000CBD RID: 3261
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnLongDescription(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnLongDescription);

		// Token: 0x06000CBE RID: 3262
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnMetadata(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnMetadata);

		// Token: 0x06000CBF RID: 3263
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnChildren(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnChildren);

		// Token: 0x06000CC0 RID: 3264
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnAdditionalPreviews(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnAdditionalPreviews);

		// Token: 0x06000CC1 RID: 3265
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnTotalOnly(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnTotalOnly);

		// Token: 0x06000CC2 RID: 3266
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays);

		// Token: 0x06000CC3 RID: 3267
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetLanguage(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

		// Token: 0x06000CC4 RID: 3268
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds);

		// Token: 0x06000CC5 RID: 3269
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetCloudFileNameFilter(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pMatchCloudFileName);

		// Token: 0x06000CC6 RID: 3270
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetMatchAnyTag(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bMatchAnyTag);

		// Token: 0x06000CC7 RID: 3271
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetSearchText(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pSearchText);

		// Token: 0x06000CC8 RID: 3272
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays);

		// Token: 0x06000CC9 RID: 3273
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddRequiredKeyValueTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

		// Token: 0x06000CCA RID: 3274
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

		// Token: 0x06000CCB RID: 3275
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType);

		// Token: 0x06000CCC RID: 3276
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x06000CCD RID: 3277
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemTitle(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchTitle);

		// Token: 0x06000CCE RID: 3278
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemDescription(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchDescription);

		// Token: 0x06000CCF RID: 3279
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemUpdateLanguage(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

		// Token: 0x06000CD0 RID: 3280
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemMetadata(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchMetaData);

		// Token: 0x06000CD1 RID: 3281
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility);

		// Token: 0x06000CD2 RID: 3282
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemTags(UGCUpdateHandle_t updateHandle, IntPtr pTags);

		// Token: 0x06000CD3 RID: 3283
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemContent(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszContentFolder);

		// Token: 0x06000CD4 RID: 3284
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemPreview(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile);

		// Token: 0x06000CD5 RID: 3285
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_RemoveItemKeyValueTags(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000CD6 RID: 3286
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddItemKeyValueTag(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000CD7 RID: 3287
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddItemPreviewFile(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile, EItemPreviewType type);

		// Token: 0x06000CD8 RID: 3288
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddItemPreviewVideo(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszVideoID);

		// Token: 0x06000CD9 RID: 3289
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, InteropHelp.UTF8StringHandle pszPreviewFile);

		// Token: 0x06000CDA RID: 3290
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, InteropHelp.UTF8StringHandle pszVideoID);

		// Token: 0x06000CDB RID: 3291
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_RemoveItemPreview(UGCUpdateHandle_t handle, uint index);

		// Token: 0x06000CDC RID: 3292
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SubmitItemUpdate(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchChangeNote);

		// Token: 0x06000CDD RID: 3293
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EItemUpdateStatus ISteamGameServerUGC_GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

		// Token: 0x06000CDE RID: 3294
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SetUserItemVote(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

		// Token: 0x06000CDF RID: 3295
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_GetUserItemVote(PublishedFileId_t nPublishedFileID);

		// Token: 0x06000CE0 RID: 3296
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x06000CE1 RID: 3297
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x06000CE2 RID: 3298
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SubscribeItem(PublishedFileId_t nPublishedFileID);

		// Token: 0x06000CE3 RID: 3299
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_UnsubscribeItem(PublishedFileId_t nPublishedFileID);

		// Token: 0x06000CE4 RID: 3300
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetNumSubscribedItems();

		// Token: 0x06000CE5 RID: 3301
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetSubscribedItems([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		// Token: 0x06000CE6 RID: 3302
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetItemState(PublishedFileId_t nPublishedFileID);

		// Token: 0x06000CE7 RID: 3303
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, IntPtr pchFolder, uint cchFolderSize, out uint punTimeStamp);

		// Token: 0x06000CE8 RID: 3304
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		// Token: 0x06000CE9 RID: 3305
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_DownloadItem(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bHighPriority);

		// Token: 0x06000CEA RID: 3306
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, InteropHelp.UTF8StringHandle pszFolder);

		// Token: 0x06000CEB RID: 3307
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUGC_SuspendDownloads([MarshalAs(UnmanagedType.I1)] bool bSuspend);

		// Token: 0x06000CEC RID: 3308
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_StartPlaytimeTracking([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		// Token: 0x06000CED RID: 3309
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_StopPlaytimeTracking([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		// Token: 0x06000CEE RID: 3310
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_StopPlaytimeTrackingForAllItems();

		// Token: 0x06000CEF RID: 3311
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_AddDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID);

		// Token: 0x06000CF0 RID: 3312
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_RemoveDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID);

		// Token: 0x06000CF1 RID: 3313
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetSecondsSinceAppActive();

		// Token: 0x06000CF2 RID: 3314
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetSecondsSinceComputerActive();

		// Token: 0x06000CF3 RID: 3315
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EUniverse ISteamGameServerUtils_GetConnectedUniverse();

		// Token: 0x06000CF4 RID: 3316
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetServerRealTime();

		// Token: 0x06000CF5 RID: 3317
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamGameServerUtils_GetIPCountry();

		// Token: 0x06000CF6 RID: 3318
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetImageSize(int iImage, out uint pnWidth, out uint pnHeight);

		// Token: 0x06000CF7 RID: 3319
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize);

		// Token: 0x06000CF8 RID: 3320
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetCSERIPPort(out uint unIP, out ushort usPort);

		// Token: 0x06000CF9 RID: 3321
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte ISteamGameServerUtils_GetCurrentBatteryPower();

		// Token: 0x06000CFA RID: 3322
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetAppID();

		// Token: 0x06000CFB RID: 3323
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition);

		// Token: 0x06000CFC RID: 3324
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed);

		// Token: 0x06000CFD RID: 3325
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ESteamAPICallFailure ISteamGameServerUtils_GetAPICallFailureReason(SteamAPICall_t hSteamAPICall);

		// Token: 0x06000CFE RID: 3326
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

		// Token: 0x06000CFF RID: 3327
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetIPCCallCount();

		// Token: 0x06000D00 RID: 3328
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction);

		// Token: 0x06000D01 RID: 3329
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsOverlayEnabled();

		// Token: 0x06000D02 RID: 3330
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_BOverlayNeedsPresent();

		// Token: 0x06000D03 RID: 3331
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUtils_CheckFileSignature(InteropHelp.UTF8StringHandle szFileName);

		// Token: 0x06000D04 RID: 3332
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, InteropHelp.UTF8StringHandle pchDescription, uint unCharMax, InteropHelp.UTF8StringHandle pchExistingText);

		// Token: 0x06000D05 RID: 3333
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetEnteredGamepadTextLength();

		// Token: 0x06000D06 RID: 3334
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetEnteredGamepadTextInput(IntPtr pchText, uint cchText);

		// Token: 0x06000D07 RID: 3335
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamGameServerUtils_GetSteamUILanguage();

		// Token: 0x06000D08 RID: 3336
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsSteamRunningInVR();

		// Token: 0x06000D09 RID: 3337
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset);

		// Token: 0x06000D0A RID: 3338
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsSteamInBigPictureMode();

		// Token: 0x06000D0B RID: 3339
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_StartVRDashboard();

		// Token: 0x06000D0C RID: 3340
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsVRHeadsetStreamingEnabled();

		// Token: 0x06000D0D RID: 3341
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetVRHeadsetStreamingEnabled([MarshalAs(UnmanagedType.I1)] bool bEnabled);

		// Token: 0x040002E7 RID: 743
		internal const string NativeLibraryName = "CSteamworks";

		// Token: 0x040002E8 RID: 744
		internal const string NativeLibrary_SDKEncryptedAppTicket = "sdkencryptedappticket";
	}
}
