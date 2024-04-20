using System;

namespace Steamworks
{
	// Token: 0x0200009C RID: 156
	public static class SteamUnifiedMessages
	{
		// Token: 0x06000938 RID: 2360 RVA: 0x00028BE8 File Offset: 0x00026DE8
		public static ClientUnifiedMessageHandle SendMethod(string pchServiceMethod, byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext)
		{
			InteropHelp.TestIfAvailableClient();
			ClientUnifiedMessageHandle result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchServiceMethod))
			{
				result = (ClientUnifiedMessageHandle)NativeMethods.ISteamUnifiedMessages_SendMethod(utf8StringHandle, pRequestBuffer, unRequestBufferSize, unContext);
			}
			return result;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00028C30 File Offset: 0x00026E30
		public static bool GetMethodResponseInfo(ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out EResult peResult)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUnifiedMessages_GetMethodResponseInfo(hHandle, out punResponseSize, out peResult);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00028C3F File Offset: 0x00026E3F
		public static bool GetMethodResponseData(ClientUnifiedMessageHandle hHandle, byte[] pResponseBuffer, uint unResponseBufferSize, bool bAutoRelease)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUnifiedMessages_GetMethodResponseData(hHandle, pResponseBuffer, unResponseBufferSize, bAutoRelease);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00028C4F File Offset: 0x00026E4F
		public static bool ReleaseMethod(ClientUnifiedMessageHandle hHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUnifiedMessages_ReleaseMethod(hHandle);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00028C5C File Offset: 0x00026E5C
		public static bool SendNotification(string pchServiceNotification, byte[] pNotificationBuffer, uint unNotificationBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchServiceNotification))
			{
				result = NativeMethods.ISteamUnifiedMessages_SendNotification(utf8StringHandle, pNotificationBuffer, unNotificationBufferSize);
			}
			return result;
		}
	}
}
