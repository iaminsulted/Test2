using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A0 RID: 160
	public static class SteamVideo
	{
		// Token: 0x060009A2 RID: 2466 RVA: 0x000298B6 File Offset: 0x00027AB6
		public static void GetVideoURL(AppId_t unVideoAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamVideo_GetVideoURL(unVideoAppID);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000298C3 File Offset: 0x00027AC3
		public static bool IsBroadcasting(out int pnNumViewers)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamVideo_IsBroadcasting(out pnNumViewers);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000298D0 File Offset: 0x00027AD0
		public static void GetOPFSettings(AppId_t unVideoAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamVideo_GetOPFSettings(unVideoAppID);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x000298E0 File Offset: 0x00027AE0
		public static bool GetOPFStringForApp(AppId_t unVideoAppID, out string pchBuffer, ref int pnBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(pnBufferSize);
			bool flag = NativeMethods.ISteamVideo_GetOPFStringForApp(unVideoAppID, intPtr, ref pnBufferSize);
			pchBuffer = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}
	}
}
