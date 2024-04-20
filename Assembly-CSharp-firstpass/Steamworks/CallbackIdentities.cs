using System;

namespace Steamworks
{
	// Token: 0x020001B1 RID: 433
	internal class CallbackIdentities
	{
		// Token: 0x06000D2F RID: 3375 RVA: 0x0002A024 File Offset: 0x00028224
		public static int GetCallbackIdentity(Type callbackStruct)
		{
			object[] customAttributes = callbackStruct.GetCustomAttributes(typeof(CallbackIdentityAttribute), false);
			int num = 0;
			if (num >= customAttributes.Length)
			{
				throw new Exception("Callback number not found for struct " + ((callbackStruct != null) ? callbackStruct.ToString() : null));
			}
			return ((CallbackIdentityAttribute)customAttributes[num]).Identity;
		}
	}
}
