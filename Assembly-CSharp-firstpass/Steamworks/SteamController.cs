using System;

namespace Steamworks
{
	// Token: 0x02000088 RID: 136
	public static class SteamController
	{
		// Token: 0x060006A6 RID: 1702 RVA: 0x0002419C File Offset: 0x0002239C
		public static bool Init()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_Init();
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000241A8 File Offset: 0x000223A8
		public static bool Shutdown()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_Shutdown();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000241B4 File Offset: 0x000223B4
		public static void RunFrame()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_RunFrame();
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000241C0 File Offset: 0x000223C0
		public static int GetConnectedControllers(ControllerHandle_t[] handlesOut)
		{
			InteropHelp.TestIfAvailableClient();
			if (handlesOut.Length != 16)
			{
				throw new ArgumentException("handlesOut must be the same size as Constants.STEAM_CONTROLLER_MAX_COUNT!");
			}
			return NativeMethods.ISteamController_GetConnectedControllers(handlesOut);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000241DF File Offset: 0x000223DF
		public static bool ShowBindingPanel(ControllerHandle_t controllerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_ShowBindingPanel(controllerHandle);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000241EC File Offset: 0x000223EC
		public static ControllerActionSetHandle_t GetActionSetHandle(string pszActionSetName)
		{
			InteropHelp.TestIfAvailableClient();
			ControllerActionSetHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszActionSetName))
			{
				result = (ControllerActionSetHandle_t)NativeMethods.ISteamController_GetActionSetHandle(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00024230 File Offset: 0x00022430
		public static void ActivateActionSet(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_ActivateActionSet(controllerHandle, actionSetHandle);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002423E File Offset: 0x0002243E
		public static ControllerActionSetHandle_t GetCurrentActionSet(ControllerHandle_t controllerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return (ControllerActionSetHandle_t)NativeMethods.ISteamController_GetCurrentActionSet(controllerHandle);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00024250 File Offset: 0x00022450
		public static ControllerDigitalActionHandle_t GetDigitalActionHandle(string pszActionName)
		{
			InteropHelp.TestIfAvailableClient();
			ControllerDigitalActionHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszActionName))
			{
				result = (ControllerDigitalActionHandle_t)NativeMethods.ISteamController_GetDigitalActionHandle(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00024294 File Offset: 0x00022494
		public static ControllerDigitalActionData_t GetDigitalActionData(ControllerHandle_t controllerHandle, ControllerDigitalActionHandle_t digitalActionHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetDigitalActionData(controllerHandle, digitalActionHandle);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000242A2 File Offset: 0x000224A2
		public static int GetDigitalActionOrigins(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle, ControllerDigitalActionHandle_t digitalActionHandle, EControllerActionOrigin[] originsOut)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetDigitalActionOrigins(controllerHandle, actionSetHandle, digitalActionHandle, originsOut);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000242B4 File Offset: 0x000224B4
		public static ControllerAnalogActionHandle_t GetAnalogActionHandle(string pszActionName)
		{
			InteropHelp.TestIfAvailableClient();
			ControllerAnalogActionHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszActionName))
			{
				result = (ControllerAnalogActionHandle_t)NativeMethods.ISteamController_GetAnalogActionHandle(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000242F8 File Offset: 0x000224F8
		public static ControllerAnalogActionData_t GetAnalogActionData(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t analogActionHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetAnalogActionData(controllerHandle, analogActionHandle);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00024306 File Offset: 0x00022506
		public static int GetAnalogActionOrigins(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle, ControllerAnalogActionHandle_t analogActionHandle, EControllerActionOrigin[] originsOut)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetAnalogActionOrigins(controllerHandle, actionSetHandle, analogActionHandle, originsOut);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00024316 File Offset: 0x00022516
		public static void StopAnalogActionMomentum(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t eAction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_StopAnalogActionMomentum(controllerHandle, eAction);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00024324 File Offset: 0x00022524
		public static void TriggerHapticPulse(ControllerHandle_t controllerHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_TriggerHapticPulse(controllerHandle, eTargetPad, usDurationMicroSec);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00024333 File Offset: 0x00022533
		public static void TriggerRepeatedHapticPulse(ControllerHandle_t controllerHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec, ushort usOffMicroSec, ushort unRepeat, uint nFlags)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_TriggerRepeatedHapticPulse(controllerHandle, eTargetPad, usDurationMicroSec, usOffMicroSec, unRepeat, nFlags);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00024347 File Offset: 0x00022547
		public static void TriggerVibration(ControllerHandle_t controllerHandle, ushort usLeftSpeed, ushort usRightSpeed)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_TriggerVibration(controllerHandle, usLeftSpeed, usRightSpeed);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00024356 File Offset: 0x00022556
		public static void SetLEDColor(ControllerHandle_t controllerHandle, byte nColorR, byte nColorG, byte nColorB, uint nFlags)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_SetLEDColor(controllerHandle, nColorR, nColorG, nColorB, nFlags);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00024368 File Offset: 0x00022568
		public static int GetGamepadIndexForController(ControllerHandle_t ulControllerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetGamepadIndexForController(ulControllerHandle);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00024375 File Offset: 0x00022575
		public static ControllerHandle_t GetControllerForGamepadIndex(int nIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (ControllerHandle_t)NativeMethods.ISteamController_GetControllerForGamepadIndex(nIndex);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00024387 File Offset: 0x00022587
		public static ControllerMotionData_t GetMotionData(ControllerHandle_t controllerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetMotionData(controllerHandle);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00024394 File Offset: 0x00022594
		public static bool ShowDigitalActionOrigins(ControllerHandle_t controllerHandle, ControllerDigitalActionHandle_t digitalActionHandle, float flScale, float flXPosition, float flYPosition)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_ShowDigitalActionOrigins(controllerHandle, digitalActionHandle, flScale, flXPosition, flYPosition);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000243A6 File Offset: 0x000225A6
		public static bool ShowAnalogActionOrigins(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t analogActionHandle, float flScale, float flXPosition, float flYPosition)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_ShowAnalogActionOrigins(controllerHandle, analogActionHandle, flScale, flXPosition, flYPosition);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000243B8 File Offset: 0x000225B8
		public static string GetStringForActionOrigin(EControllerActionOrigin eOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamController_GetStringForActionOrigin(eOrigin));
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000243CA File Offset: 0x000225CA
		public static string GetGlyphForActionOrigin(EControllerActionOrigin eOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamController_GetGlyphForActionOrigin(eOrigin));
		}
	}
}
